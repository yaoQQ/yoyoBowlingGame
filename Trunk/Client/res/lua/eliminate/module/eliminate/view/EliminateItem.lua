---
--- Created by Lichongzhi.
--- DateTime: 2018\4\8 0008 15:28
---
require "base:enum/NoticeType"
require "eliminate:module/eliminate/PersonalExtend"
require "eliminate:module/eliminate/EliminateConfig"

local Loger = CS.Loger
local RectTransform = CS.UnityEngine.RectTransform
local Time = CS.UnityEngine.Time
local Vector3 = CS.UnityEngine.Vector3
local Vector2 = CS.UnityEngine.Vector2
local GameObject = CS.UnityEngine.GameObject
local UIEvent = CS.UIEvent
local ImageWidget = CS.ImageWidget
local Screen = CS.UnityEngine.Screen
local NoticeManager = CS.NoticeManager

EliminateItem = {}
local this = EliminateItem
function this:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

-- item索引
this.itemIndex = ""
-- 组件类数据
this.rectTransform = nil
this.gameObject = nil
this.itemImage = nil
this.itemType = 0
this.remainTime = 0
this.activatingEffectCount = 0
this.tableInfo = nil
this.itemEffectList = nil
this.itemActivatingEffectPosList = nil
this.itemResultAreaList = nil
--cro
this.effectCro = nil
this.dropCro = nil


function this:initItemLogicData(index)
    self.itemIndex = tostring(index)
end

function this:initItemComponentData(gameObject)
    self.gameObject = gameObject
    self.rectTransform = gameObject:GetComponent(typeof(RectTransform))
    self.itemImage = gameObject:GetComponent(typeof(ImageWidget))
    self.itemImage:AddEventListener(UIEvent.PointerDown, this.onItemPointerDown)
    self.gameObject.name = self.itemIndex
end

--=============================访问器=================================
function this:setItemPosition(pos)
    self.rectTransform.anchoredPosition = Vector2(pos.x, pos.y)
end

function this:addItemActivatingEffectPos(pos)
    if self.itemActivatingEffectPosList == nil then
        self.itemActivatingEffectPosList = {}
    end
    table.insert(self.itemActivatingEffectPosList, pos)
end

function this:setItemType(itemType)
    self.itemImage.ignoreEventSign = false
    self.itemType = itemType
    self.tableInfo = ItemDataBase.query(itemType)
    --self.tableInfo = TableEliminateItemDataBase.data[itemType]
    --print("tableInfo = "..table.tostring(self.tableInfo))
    self:updateItemSprite()
end

function this:getItemType()
    return self.itemType
end

function this:getItemIndex()
    return self.itemIndex
end

function this:getItemName()
    return self.tableInfo.name
end

function this:getCoverRule()
    return self.tableInfo.coverRule
end

function this:updateItemSprite()
    local itemList = EliminatePoolView:getItemList()
    self.itemImage:SetPng(itemList[self.itemType])
end

function this:getItemNature()
    return self.tableInfo.itemNature
end

function this:getItemTableDuration()
    return self.tableInfo.duration
end

function this:addItemResultArea(point)
    if self.itemResultAreaList == nil then
        self.itemResultAreaList = {}
    end
    table.insert(self.itemResultAreaList, point)
end

function this:getItemResultArea()
    return self.itemResultAreaList
end

--==============================================================

function this:processItemActivating()
    if table.empty(self.tableInfo) then
        Loger.PrintError("错误-表格配置中不存在该道具, itemType=",self.itemType)
        return
    end
    if self.itemEffectList == nil then
        self.itemEffectList = {}
    end
    if self.itemType == EliminateConfig.ItemType.Freeze then
        self.activatingEffectCount = 2
    else
        self.activatingEffectCount = 1
    end
    for i = 1, self.activatingEffectCount do
        if self.tableInfo.activatingEffect ~= "" then
            local effectObj = ObjectPool:GetInstance():getObject(self.tableInfo.activatingEffect)
            table.insert(self.itemEffectList, effectObj)
        end
    end

    if self.tableInfo.duration > 0  then
        self.remainTime = self.tableInfo.duration
        local mapMarginInfo = EliminateGrid:GetInstance():getMapMargin()
        --print("mapMarginInfo = "..table.tostring(mapMarginInfo))
        self:initItemEffectPos()
        local speedX, speedY = self:getRandomSpeed()
        self.effectCro = coroutine.start(function ()
            while(self.remainTime > 0) do
                self.remainTime = self.remainTime - Time.deltaTime
                if self.itemType == EliminateConfig.ItemType.Fog then
                    local fogEffect = self.itemEffectList[1]
                    if fogEffect ~= nil then
                        fogEffect.transform:Translate(Time.deltaTime * speedX, Time.deltaTime * speedY, 0)
                        local effectX = fogEffect.transform.position.x
                        local effectY = fogEffect.transform.position.y
                        if effectY >= mapMarginInfo.maxY then
                            speedX, speedY = self:updateSpeedByDirect(EliminateConfig.FourDirect.Up)
                        elseif effectY <= mapMarginInfo.minY then
                            speedX, speedY = self:updateSpeedByDirect(EliminateConfig.FourDirect.Down)
                        elseif effectX <= mapMarginInfo.minX then
                            speedX, speedY = self:updateSpeedByDirect(EliminateConfig.FourDirect.Left)
                        elseif  effectX >= mapMarginInfo.maxX then
                            speedX, speedY = self:updateSpeedByDirect(EliminateConfig.FourDirect.Right)
                        end
                    end
                end
                coroutine.step(self.effectCro)
            end
            self:itemTimeUp()
        end)
    else
        self:itemTimeUp()
    end
end

function this:initItemEffectPos()
    if table.empty(self.itemEffectList)then
        return
    end
    if self.itemType == EliminateConfig.ItemType.Craze then
        self.itemEffectList[1].transform.position = Vector3.zero
    elseif self.itemType == EliminateConfig.ItemType.Freeze then
        for i = 1, #self.itemEffectList do
            self.itemEffectList[i].transform.position = self.itemActivatingEffectPosList[i]
        end
    elseif self.itemType == EliminateConfig.ItemType.Fog then
        local mapMarginInfo = EliminateGrid:GetInstance():getMapMargin()
        self.itemEffectList[1].transform.position = mapMarginInfo.center
    end
end

this.speedMin = -2
this.speedMax = 2
function this:getRandomSpeed()
    local speedX = math.random(this.speedMin, this.speedMax)
    local speedY = math.random(this.speedMax, this.speedMax)
    return speedX, speedY
end

-- 根据碰到的边缘方向来决定下一次随机的方向
function this:updateSpeedByDirect(direct)
    local speedX = 0
    local speedY = 0
    if direct == EliminateConfig.FourDirect.Up then
        speedX = math.random(this.speedMin, this.speedMax)
        speedY = math.random(this.speedMin, 0)
    elseif direct == EliminateConfig.FourDirect.Down then
        speedX = math.random(this.speedMin, this.speedMax)
        speedY = math.random(0, this.speedMax)
    elseif direct == EliminateConfig.FourDirect.Left then
        speedX = math.random(0, this.speedMax)
        speedY = math.random(this.speedMin, this.speedMax)
    elseif direct == EliminateConfig.FourDirect.Right then
        speedX = math.random(this.speedMin, 0)
        speedY = math.random(this.speedMin, this.speedMax)
    else
        Loger.PrintError("错误-消消-未知方向, direct = ",direct)
    end
    return speedX, speedY
end

function this:poolItemEffect(effect)
    if effect == nil then
        return
    end
    ObjectPool:GetInstance():poolObject(effect)
end

function this:itemDrop()
    self.itemImage.Img:SetNativeSize()
    local image_w = self.rectTransform.sizeDelta.x
    local ratio = EliminateConfig.standardWidth / Screen.width
    local s_w = Screen.width * ratio
    local s_h = EliminateConfig.standardHeight
    --print("s_w = "..s_w)
    --print("s_h = "..s_h)
    local startX = math.random(-s_w /2 + image_w/2, s_w /2 - image_w/2)
    local startY = s_h /2
    local speed = -2
    self.rectTransform.anchoredPosition = Vector2(startX, startY)
    self.dropCro = coroutine.start(function ()
        local curY = self.rectTransform.anchoredPosition.y
        while curY > (-s_h / 2) do
            self.rectTransform:Translate(0, Time.deltaTime * speed, 0);
            curY = self.rectTransform.anchoredPosition.y
            coroutine.step(self.dropCro)
        end
        EliminateGrid:GetInstance():onItemSpawnOverGrid(self)
    end)
end

function this:itemToPool()
    self.itemImage.ignoreEventSign = true
    self.itemType = 0
    self.tableInfo = nil
    self:updateItemSprite()
    self:setItemPosition(EliminateConfig.POOL_POS)
end

function this:setItemParent(root)
    self.rectTransform:SetParent(root)
    self.rectTransform.localScale = Vector3.one
end

function this:itemClick()
    self.itemImage.ignoreEventSign = true
    self:setItemPosition(EliminateConfig.POOL_POS)
end

function this:itemActive()
    self.itemImage.ignoreEventSign = true
    self:setItemPosition(EliminateConfig.POOL_POS)
    self:processItemActivating()
end

function this:itemRepeatActive()
    self.itemImage.ignoreEventSign = true
    self:setItemPosition(EliminateConfig.POOL_POS)
    if self.tableInfo.coverRule == EliminateConfig.ItemCoverRule.Time then
        self.remainTime = self.remainTime + self.tableInfo.duration
    elseif self.tableInfo.coverRule == EliminateConfig.ItemCoverRule.Effect then
        self:processItemActivating()
    elseif self.tableInfo.coverRule == EliminateConfig.ItemCoverRule.Independent then
        self:processItemActivating()
    elseif self.tableInfo.coverRule == EliminateConfig.ItemCoverRule.Refresh then
        self.remainTime = self.tableInfo.duration
    else
        Loger.PrintError("错误-消消-未支持的叠加规则: ", self.tableInfo.coverRule)
    end
end

function this:itemActivated()
    self.itemImage.ignoreEventSign = true
    self:setItemPosition(EliminateConfig.POOL_POS)
end

function this:itemTimeUp()
    NoticeManager.Instance:Dispatch(EliminateNoticeType.ItemTimeUp, { item = self})
end

function this:itemEnd()
    coroutine.stop(self.dropCro)
    coroutine.stop(self.effectCro)
    for i = 1, self.activatingEffectCount do
        if self.tableInfo.endEffect ~= "" then
            local endEffect = ObjectPool:GetInstance():getObject(self.tableInfo.endEffect)
            endEffect.transform.position = self.itemActivatingEffectPosList[i]
            table.insert(self.itemEffectList, endEffect)
        end
    end

    for _, v in pairs(self.itemEffectList) do
        self:poolItemEffect(v)
    end
    self.itemEffectList = nil
    self:itemToPool()
end

function this:onDestroy()
    GameObject.Destroy(self.gameObject)
    coroutine.stop(self.dropCro)
    coroutine.stop(self.effectCro)
    self.dropCro = nil
    self.effectCro = nil
    self.rectTransform = nil
    self.gameObject = nil
    self.itemImage = nil
    self.itemIndex = ""
    self.itemType = 0
    self.activatingEffectCount = 0
    self.itemEffectList = nil
    self.itemActivatingEffectPosList = nil
    self.itemResultAreaList = nil
end

function this.onItemPointerDown(eventData)
    local itemIndex = eventData.pointerCurrentRaycast.gameObject.name
    NoticeManager.Instance:Dispatch(EliminateNoticeType.ItemClick, { itemIndex = itemIndex})
end

