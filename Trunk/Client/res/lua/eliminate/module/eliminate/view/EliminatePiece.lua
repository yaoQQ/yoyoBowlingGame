---
--- Created by Lichongzhi.
--- DateTime: 2017/11/28 14:29
---

require "base:enum/NoticeType"
require "eliminate:module/eliminate/PersonalExtend"
require "eliminate:module/eliminate/EliminateConfig"
require "eliminate:module/eliminate/ObjectPool"
local NoticeManager = CS.NoticeManager
local Loger = CS.Loger
local DOTween = CS.DG.Tweening.DOTween
local RectTransform = CS.UnityEngine.RectTransform
local Time = CS.UnityEngine.Time
local Vector3 = CS.UnityEngine.Vector3
local Vector2 = CS.UnityEngine.Vector2
local Image = CS.UnityEngine.UI.Image
local GameObject = CS.UnityEngine.GameObject
local ImageWidget = CS.ImageWidget

EliminatePiece = {}
local this = EliminatePiece
function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

this.pieceIndex = ""
this.pieceScore = 0
this.X = 0
this.Y = 0
this.pieceType = 0
this.pieceColor = 0

this.isWaitEli = false      -- 等待消除状态
this.isWaitFall = false     -- 等待下落状态
this.isSwap = false         -- 交换状态
this.isSwapFail = false     -- 交换失败状态, 用以区分成功和失败的交换
this.isFindRange = false    -- 是否被寻找过了, 用于BFS的节点寻找
this.isFreeze  = false      -- 冰冻状态
this.isActived = false      -- 是否被激活了, 用于第二类特殊雷, 防止重复点击激活
-- 组件类数据
this.gameObject = nil
this.buttonWidget = nil
this.rectTransform = nil
this.pieceImage = nil
this.effectObj = nil

-- Cro

function this:initPieceLogicData(_x, _y, _type)
    self.X = _x
    self.Y = _y
    self.pieceType = _type
    local key = string.format("%s%s", _x, _y)
    self.pieceIndex = key
    self.pieceScore = EliminateConfig.SCORE_UNIT
    if self.pieceImage ~= nil then
        self.pieceImage.Img:CrossFadeAlpha(1, 0, true)
    end
end

function this:initPieceComponentData(gameObject)
    self.gameObject = gameObject
    self.rectTransform = gameObject:GetComponent(typeof(RectTransform))
    self.pieceImage  = gameObject:GetComponent(typeof(ImageWidget));
    self.buttonWidget = gameObject:GetComponent(typeof(ImageWidget))
end

--===========================================访问器========================================
function this:setSize(w, h)
    self.rectTransform.sizeDelta = Vector2(w, h)
end

function this:setPiecePosition(vector3)
    self.rectTransform.localPosition = vector3
end

function this:getPiecePosition()
    return self.rectTransform.localPosition
end

function this:setParent(root)
    self.rectTransform:SetParent(root)
    self.rectTransform.localScale = Vector3.one
end

function this:setType(type)
    self.pieceType = type
end

function this:getType()
    return self.pieceType
end

function this:setSwapState(state)
    self.isSwap = state
end

function this:getSwapState()
    return self.isSwap
end

function this:setWaitEliState(state)
    self.isWaitEli = state
end

function this:setWaitFallState(state, type)
    self.isWaitFall = state
end

function this:getWaitFallState()
    return self.isWaitFall
end

function this:getWaitEliState()
    return self.isWaitEli
end

function this:setFreezeState(state, result)
    self.isFreeze = state
end

function this:getFreezeState()
    return self.isFreeze
end

function this:setActivedState(state)
    self.isActived = state
end

function this:getActivedState()
    return self.isActived
end

--===================================================================================
function this:isCanMatch()
    return self.pieceType ~= EliminateConfig.PieceType.EMPTY
            and self.isWaitEli == false
            and self.isWaitFall == false
            and self.isSwapFail == false
            and self.isFreeze == false

end

function this:isCanMove()
    return self.pieceType ~= EliminateConfig.PieceType.EMPTY
            and self.isWaitEli == false
            and self.isWaitFall == false
            and self.isSwap == false
            and self.isSwapFail == false
            and self.isFreeze == false
end

function this:swapEliState(newX, newY)
    local targetPiece = EliminateGrid:GetInstance():getPieceByPos(newX, newY)
    if targetPiece ~= nil and targetPiece:getWaitEliState() then
        self.isWaitEli = targetPiece:getWaitFallState()
        targetPiece:setWaitEliState(false)
    end
end

function this:onFallPiece(newX, newY, time)
    --print(string.format("piece下落, (%s,%s) -> (%s,%s)", self.X, self.Y, newX, newY))
    local tempPoint = {x = self.X, y = self.Y}
    if self:getFreezeState() then
        self:setFreezeState(false, "fall")
    end
    local startPos = self.rectTransform.localPosition
    local targetPos = EliminateGrid:GetInstance():getPositionByGrid(newX, newY)
    self:setWaitFallState(true, "fall")
    self:swapEliState(newX, newY)
    self:updateSprite()
    self:updatePieceIndex(newX, newY)
    local fallCro = nil
    fallCro = coroutine.start(function ()
            for t = 0, time, Time.fixedDeltaTime do
                self.rectTransform.localPosition = Vector3.Lerp(startPos, targetPos, t / time);
                coroutine.step(fallCro)
            end
        self:setPiecePosition(targetPos)
        --print(string.format("块下落完毕, (%s,%s) -> (%s,%s)", tempPoint.x, tempPoint.y, newX, newY))
        self:setWaitFallState(false, "falled")
    end)
end

function this:onShufflePiece(newX, newY, time)
    self:updatePieceIndex(newX, newY)
    --print(string.format("(%s,%s)移到(%s,%s),time = %s", self.X, self.Y, newX, newY, time))
    local startPos = self.rectTransform.localPosition
    local targetPos = EliminateGrid:GetInstance():getPositionByGrid(newX, newY)
    local shuffleCro = nil
    shuffleCro = coroutine.start(function ()
        self:onSelectedPiece()
        coroutine.step(shuffleCro)
        for t = 0, time, Time.fixedDeltaTime do
            self.rectTransform.localPosition = Vector3.Lerp(startPos, targetPos, t / time);
            coroutine.step(shuffleCro)
        end
        self:updateSprite()
        self.rectTransform.localPosition = targetPos
    end)
end

function this:onClearPiece()
    --print(string.format("清除piece = (%s,%s)", self.X, self.Y))
    local clearCro = nil
    clearCro = coroutine.start(function ()
        self:onSelectedPiece()
        coroutine.step(clearCro)
        self:onEliminatePiece()
        coroutine.step(clearCro)
        self:spawnEliEffect()
        self.pieceImage.Img:CrossFadeAlpha(0, 0.2, true)
        --self.pieceImage.Img:CrossFadeAlpha(0, 0, true)
        coroutine.wait(clearCro, 200)
        EliminateGrid:GetInstance():poolPiece(self)
        self:resetLogicData()
    end)
end

function this:spawnEliEffect()
    local effectObj = ObjectPool:GetInstance():getObject("fx_piece_eli")
    local root = EliminateGrid:GetInstance():getPieceRoot()
    local localPos = EliminateGrid:GetInstance():getPositionByGrid(self.X, self.Y)

    local pos = root:TransformPoint(localPos)
    effectObj:SetActive(true)
    effectObj.transform.position = pos
    local poolEffectCro = nil
    poolEffectCro = coroutine.start(function ()
        coroutine.wait(poolEffectCro, 1000)
        ObjectPool:GetInstance():poolObject(effectObj)
    end)
end
function this:resetLogicData()
    self.pieceIndex = string.format("piecePool_%s%s", self.X, self.Y)
    self.pieceType = EliminateConfig.PieceType.EMPTY
    self.pieceColor = EliminateConfig.ColorType.NONE
    self.pieceImage.activeGray = false
    self.gameObject.name = self.pieceIndex
    self.isWaitEli = false
    self.isWaitFall = false
    self.isSwap = false
    self.isSwapFail = false
    self.isActived = false
    if self.effectObj ~= nil then
        ObjectPool:GetInstance():poolObject(self.effectObj)
        self.effectObj = nil
    end
    self.isFindRange = false
    self:updateSprite()
end

function this:updateSprite()
    --print("消消--piece更新颜色sprite, color = "..self.pieceColor)
    if self:isSpecial() == false or self.pieceType == EliminateConfig.PieceType.EMPTY then
        local normalList = EliminatePoolView:getNormalList()
        self.pieceImage:SetPng(normalList[self.pieceColor])
    end
end

-- 更新piece索引
function this:updatePieceIndex(newX, newY)
    self.X = newX
    self.Y = newY
    self.pieceIndex = string.format("%s%s",newX, newY)
    self.gameObject.name = self.pieceIndex
end

function this:setColor(newColor, isHideSprite)
    self.pieceColor = newColor
    if isHideSprite then
        local normalList = EliminatePoolView:getNormalList()
        self.pieceImage:SetPng(normalList[0])
    else
        self:updateSprite()
    end
end

--================================Item相关=============================
function this:pieceGrayStart()
    self.pieceImage.activeGray = true
end

function this:pieceGrayEnd()
    self.pieceImage.activeGray = false
end

function this:pieceShakeStart()
    local shakeCro = nil
    shakeCro = coroutine.start(function()
        while true do
            local min = -10
            local max = 10
            local originPos = EliminateGrid:GetInstance():getPositionByGrid(self.X, self.Y)
            local randomX = math.random(min, max)
            local randomY = math.random(min, max)
            local targetPos = Vector3(originPos.x + randomX, originPos.y + randomY, 0)
            self:setPiecePosition(targetPos)
            coroutine.step(shakeCro)
        end
    end)
end

function this:pieceShakeEnd()
    self:setPiecePosition(EliminateGrid:GetInstance():getPositionByGrid(self.X, self.Y))
end

--=======================================================================
function this:getColor()
    return self.pieceColor
end

function this:setFindState(state)
    self.isFindRange = state
end

function this:getFindState()
    return self.isFindRange
end

function this:isMoveable()
    return self.pieceType ~= EliminateConfig.PieceType.EMPTY
end

function this:isClearable()
    return self.pieceType ~= EliminateConfig.PieceType.EMPTY
end

function this:isColored()
    return self.pieceColor ~= EliminateConfig.ColorType.NONE
end

function this:isNormal()
    return self.pieceType == EliminateConfig.PieceType.NORMAL
end

function this:isSpecial()
    return self:isFirstKindSpe() or self:isSecondKindSpe()
end

function this:isFirstKindSpe()
    return self.pieceType == EliminateConfig.PieceType.BOMB
end

function this:isSecondKindSpe()
    return self.pieceType == EliminateConfig.PieceType.CROSS_CLEAR or self.pieceType == EliminateConfig.PieceType.SAME_CLEAR
end

-- 获取消除面积, 普通块为当前所在格, 特殊雷有各自规则
function this:getEliRange()
    local rangList ={}
    local point = {x = self.X, y = self.Y}
    local map = EliminateGrid:GetInstance():getGridMap()
    local isInDanceMoment = EliminateGrid:GetInstance():getDanceMomentState()
    local list = {}
    if isInDanceMoment then
        if self.pieceType == EliminateConfig.PieceType.BOMB then
            list = EliminateExtension:getBombNear_DanceMoment(self.X, self.Y, map)
        elseif self.pieceType == EliminateConfig.PieceType.CROSS_CLEAR then
            list = EliminateExtension:getCross_DanceMoment(point, map)
        elseif self.pieceType == EliminateConfig.PieceType.SAME_CLEAR  then
            list = EliminateExtension:getMaxSameColor(point, map)
        end
    else
        if self.pieceType == EliminateConfig.PieceType.BOMB then
            list = EliminateExtension:getBombNear(self.X, self.Y, map)
        elseif self.pieceType == EliminateConfig.PieceType.CROSS_CLEAR then
            list = EliminateExtension:getCross(point, map)
        elseif self.pieceType == EliminateConfig.PieceType.SAME_CLEAR  then
            list = EliminateExtension:getMaxSameColor(point, map)
        end
    end
    --print("getEliRange, list = "..table.tostring(list))
    table.extend(rangList, list)
    return rangList
end

function this:onSelectedPiece()
    local selectedList =  EliminatePoolView:getSelectedList()
    if self.pieceType == EliminateConfig.PieceType.NORMAL then
        self.pieceImage:SetPng(selectedList[self.pieceColor])
    end
end

function this:createSpeEffect(effectName)
    self.effectObj = ObjectPool:GetInstance():getObject(effectName)
    self.effectObj.transform.localScale = Vector3.one
    self.effectObj.transform:SetParent(self.rectTransform)
    self.effectObj.transform.localPosition = Vector3.zero
end

function this:onSpecialPiece()
    if self:isSpecial() == false then
        return
    end
    local specialList =  EliminatePoolView:getSpecialList()
    if self.pieceType == EliminateConfig.PieceType.CROSS_CLEAR then
        self.pieceImage:SetPng(specialList[specialList.Length - 1])
        self:createSpeEffect("fx_cross_special")
    elseif self.pieceType == EliminateConfig.PieceType.SAME_CLEAR then
        local sameSpecialCro = nil
        sameSpecialCro = coroutine.start( function ()
            while self:isSpecial() do
                for i = 1, EliminateConfig.ColorType.COUNT - 1 do
                    self.pieceImage:SetPng(specialList[i])
                    coroutine.wait(sameSpecialCro, 100)
                end
            end
        end)
    elseif self.pieceType == EliminateConfig.PieceType.BOMB then
        self.pieceImage:SetPng(specialList[self.pieceColor])
        self:createSpeEffect("fx_bomb_special")
    else
        self.pieceImage.sprite = specialList[self.pieceColor]
    end
end

function this:onEliminatePiece()
    local eliminateList =  EliminatePoolView:getEliminateList()
    if self.pieceType == EliminateConfig.PieceType.NORMAL then
        self.pieceImage:SetPng(eliminateList[self.pieceColor])
    end
end

--========================================访问器===================================
function this:setSwapFailState(state)
    --print(string.format("(%s,%s,swapFailState: %s)", self.X, self.Y, state))
    self.isSwapFail = state
end

function this:getSwapFailState()
    return self.isSwapFail
end
--========================================事件=====================================

function this:onDestroy()
    GameObject.Destroy(self.gameObject)
    self.gameObject = nil
    self.buttonWidget = nil
    self.rectTransform = nil
    self.pieceImage = nil
    self.transform = nil
end

--=======================================LogTest=====================================
function this:logIsCanMatch()
    return string.format("(%s,%s),isWaitEli = %s, isWaitFall = %s, isSwap = %s, isSwapFail = %s, isFreeze = %s",
            self.X, self.Y, self.isWaitEli, self.isWaitFall, self.isSwap, self.isSwapFail, self.isFreeze)
end

function this:logPosition()
    return string.format("(%s,%s), localPosition = (%s,%s)",
            self.X, self.Y, self.rectTransform.localPosition.x, self.rectTransform.localPosition.y)
end