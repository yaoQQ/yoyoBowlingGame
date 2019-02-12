---
--- Created by Lichongzhi.
--- DateTime: 2018\10\30 0030 14:31
---

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "coin:mid/Mid_coin_game_panel"
require "coin:module/view/CoinPiece"
require "coin:module/CoinConfig"

local Screen = CS.UnityEngine.Screen
local UIExEventTool = CS.UIExEventTool
local UICamera = CS.UIManager.Instance.UICamera
local RectTransformUtility = CS.UnityEngine.RectTransformUtility


CoinGameView = BaseView:new()
local this = CoinGameView
this.viewName = "CoinGameView"
--设置面板特性
this:setViewAttribute(UIViewType.Game_1, UIViewEnum.Coin_GameView)

--设置加载列表
this.loadOrders=
{
    "coin:coin_game_panel"
}

this.score = 0
this.timerCount = 0
this.targetNum = 0
this.startTimerCount = 0
this.stackDic = nil
this.pieceDic = nil
this.calDic = nil
this.curMoveCoin = nil
this.isRingStarted = false
this.isTimerOver = false
this.ringStartTime = 0  -- 该环开始时间
this.ringOverTime = 0   -- 该环结束时间
this.moveBounds = nil

this.StartTimerKey  = "CoinStartTimerCount" -- 开始倒计时计时器
this.GameTimerKey   = "CoinGameTimerKey"    -- 整局计时器
this.RingTimerKey   = "CoinRingTimerKey"    -- 两环之间的时间间隔计时器

function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_coin_game_panel
    self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
    self:hide()
end

function this:addEvent()
    self.main_mid.exit_image:AddEventListener(UIEvent.PointerClick,function()
        self:activeExitConfirmPanel(true)
    end)
    self.main_mid.exit_confirm_btn:AddEventListener(UIEvent.PointerClick,function()
        self:activeExitConfirmPanel(false)
        self.isRingStarted = false
        CoinNetModule.sendReqUpdateMatchState()
    end)
    self.main_mid.exit_continue_btn:AddEventListener(UIEvent.PointerClick,function()
        self:activeExitConfirmPanel(false)
    end)

end

function this:activeExitConfirmPanel(state)
    self.main_mid.exit_confirm_panel.gameObject:SetActive(state)
end

function this:onShowHandler(msg)
    Loger.PrintWarning(self.viewName," view加载完毕打开时可重写")
    self.main_mid.start_timer_image.gameObject:SetActive(false)
    self.main_mid.exit_confirm_panel.gameObject:SetActive(false)
    self.moveBounds = UIExEventTool.GetBounds(self.main_mid.move_clamp_panel.rectTransform, self.main_mid.coin_game_panel.rectTransform)
    self:activeExitConfirmPanel(false)

    --print(string.format("moveBounds X:(%s,%s)",self.moveBounds.min.x,self.moveBounds.max.x))
    --print(string.format("moveBounds Y:(%s,%s)",self.moveBounds.min.y,self.moveBounds.max.y))
    --temp
    self.main_mid.move_clamp_panel.gameObject:SetActive(false)
    --endTemp
    self:onStartGameView()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(this.StartTimerKey)
    GlobalTimeManager.Instance.timerController:AddTimer(this.StartTimerKey, 1000, -1, function ()
        if self.startTimerCount > 0  then
            self.startTimerCount = self.startTimerCount - 1
            self:onStartTimer()
        elseif self.startTimerCount == 0 then
            self:startRing()
        end
    end)
end

function this:onStartGameView()
    self:initGameData()
    self.main_mid.score_text.text = tostring(self.score)
    self.main_mid.timer_text.text = tostring(self.timerCount)
    self.main_mid.target_text.text = tostring(0)
    self.main_mid.start_timer_image:SetPng(nil)
end

function this:startRing()
    self:initRingGameData()
    self:onRingStartGameView()
end

function this:onStartTimer()
    local count = self.startTimerCount
    local timerImageList = CoinPoolView:GetStartTimerList()
    if count < 0 or count >= timerImageList.Length then
        Loger.PrintError("错误-开始倒计时计数错误, count = ", count)
        return
    end
    local image = timerImageList[count]
    local curObj = self.main_mid.start_timer_image
    curObj.gameObject:SetActive(true)
    curObj.Img:CrossFadeAlpha(1, 0, true)
    curObj:SetPng(image)
    curObj.Img:SetNativeSize()
    local mySequence = DOTween.Sequence()
    curObj.transform.localScale = Vector3.one * 0.5
    local scale_1 = curObj.transform:DOScale(Vector3.one * 1.2, 0.3)
    local scale_2 = curObj.transform:DOScale(Vector3.one, 0.2)
    mySequence:Append(scale_1)
    mySequence:Append(scale_2)
    mySequence:AppendCallback(function()
        curObj.Img:CrossFadeAlpha(0, 0.5, true)
    end)
end

-- 初始化全局数据
function this:initGameData()
    self.score = 0
    self.timerCount = 60
    self.startTimerCount = 4
end

-- 初始化当前环数据
function this:initRingGameData()
    self.calDic = {}
    self.stackDic = {}
    self.pieceDic = {}
    -- 按照权重随机, 暂时先不做
    --local totalWeight = 0
    --local randomItem = nil
    --for _, v in pairs(TableCoinCoinParameter.data) do
    --    totalWeight = totalWeight + v.weight
    --end
    --for _, v in pairs(TableCoinCoinParameter.data) do
    --    if math.single_prob(v.weight / totalWeight) == 1 then
    --        randomItem = v
    --        break
    --    end
    --end
    local tableItem = TableCoinCoinParameter.data[1]
    if tableItem == nil then
        printError("表格数据错误")
        return
    end
    local keyIndex = 0
    for i = 1, math.random(tableItem.number1[1], tableItem.number1[2]) do
        keyIndex = keyIndex + 1
        local coinItem = {}
        coinItem.key = keyIndex
        coinItem.faceValue = tableItem.denomination1
        table.insert(self.stackDic, coinItem)
    end
    for i = 1, math.random(tableItem.number2[1], tableItem.number2[2]) do
        keyIndex = keyIndex + 1
        local coinItem = {}
        coinItem.key = keyIndex
        coinItem.faceValue = tableItem.denomination2
        table.insert(self.stackDic, coinItem)
    end
    for i = 1, math.random(tableItem.number5[1], tableItem.number5[2]) do
        keyIndex = keyIndex + 1
        local coinItem = {}
        coinItem.key = keyIndex
        coinItem.faceValue = tableItem.denomination5
        table.insert(self.stackDic, coinItem)
    end
    self.targetNum =  math.random(tableItem.subject[1], tableItem.subject[2])
    --print("stackDic = "..table.tostring(self.stackDic))
end

function this:onRingStartGameView()
    self.main_mid.score_text.text = tostring(self.score)
    self.main_mid.target_text.text = tostring(self.targetNum)

    local bounds = UIExEventTool.GetBounds(self.main_mid.stack_panel.rectTransform, self.main_mid.coin_game_panel.rectTransform)
    for _, v in pairs(self.stackDic) do
        local x = 0
        local y = 0
        local piece = CoinPiece:new()
        if v.faceValue == 1 then
            x = Random.Range(bounds.min.x + CoinConfig.CoinSize.Coin1.x / 2, bounds.max.x - CoinConfig.CoinSize.Coin1.x / 2)
            y = Random.Range(bounds.min.y + CoinConfig.CoinSize.Coin1.y / 2, bounds.max.y - CoinConfig.CoinSize.Coin1.y / 2)
        elseif v.faceValue == 2 then
            x = Random.Range(bounds.min.x + CoinConfig.CoinSize.Coin2.x / 2, bounds.max.x - CoinConfig.CoinSize.Coin2.x / 2)
            y = Random.Range(bounds.min.y + CoinConfig.CoinSize.Coin2.y / 2, bounds.max.y - CoinConfig.CoinSize.Coin2.y / 2)
        elseif v.faceValue == 5 then
            x = Random.Range(bounds.min.x + CoinConfig.CoinSize.Coin5.x / 2, bounds.max.x - CoinConfig.CoinSize.Coin5.x / 2)
            y = Random.Range(bounds.min.y + CoinConfig.CoinSize.Coin5.y / 2, bounds.max.y - CoinConfig.CoinSize.Coin5.y / 2)
        else
            printError("抢金币-错误, 不存在的面值: "..v.faceValue)
            break
        end
        local go = GameObject.Instantiate(CoinPoolView:GetCoinPrefab().gameObject)
        piece:InitCoinPiece(v.key, v.faceValue, go)
        piece:SetPieceParent(self.main_mid.coin_game_panel.rectTransform)
        piece:SetPiecePosition(Vector2(x, y))
        piece:SetPieceParent(self.main_mid.stack_panel.rectTransform)
        piece:PlaySpawnAnim()
        self.pieceDic[tostring(v.key)] = piece
    end
    self.isRingStarted = true
    self.ringStartTime = Time.time

    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(this.StartTimerKey)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(this.GameTimerKey)
    GlobalTimeManager.Instance.timerController:AddTimer(this.GameTimerKey, 1000, -1, function ()
        if self.timerCount < 0 then
            return
        end
        self.timerCount = self.timerCount - 1
        if self.timerCount >= 0 then
            self.main_mid.timer_text.text = tostring(self.timerCount)
        else
            print("抢金币-全局时间到")
            self.isRingStarted = false
            self:OnPieceUp()
            CoinNetModule.ReqGameRank()
        end
    end)
end

function this:GetCurMoveCoin()
    return self.curMoveCoin
end
--=================================事件响应============================

function this:OnPieceDown(_key)
    if not self.isRingStarted then
        return
    end
    if self.pieceDic[_key] == nil then
        printError("抢金币-错误-不存在该索引的金币".._key)
    else
        self.curMoveCoin = self.pieceDic[_key]
        --print(string.format("按下了%s", _key))
        self.curMoveCoin:SetPieceLast()
    end
end

function this:OnPieceDrag(eventData)
    if not self.isRingStarted then
        return
    end
    if self.curMoveCoin == nil then
        --printError("抢金币-错误-没有正在移动的金币")
        return
    end
    local coin = self.curMoveCoin

    local localCursor
    local isValue, localCursor = RectTransformUtility.ScreenPointToLocalPointInRectangle(self.main_mid.coin_game_panel.rectTransform, eventData.position, eventData.pressEventCamera, localCursor)
    if isValue == false then
        return
    end

    local curPos = coin:GetPiecePosition()
    local ratio = CoinConfig.GameWidth / Screen.width
    local actualCanvasX = CoinConfig.GameWidth
    local actualCanvasY = Screen.height * ratio
    local deltaLocalX = eventData.delta.x * actualCanvasX / Screen.width
    local deltaLocalY = eventData.delta.y * actualCanvasY / Screen.height
    local targetPos = curPos + Vector2(deltaLocalX, deltaLocalY)
    local x = Mathf.Clamp(targetPos.x, self.moveBounds.min.x, self.moveBounds.max.x)
    local y = Mathf.Clamp(targetPos.y, self.moveBounds.min.y , self.moveBounds.max.y)
    coin:SetPiecePosition(Vector2(x, y))
end

function this:OnPieceUp()
    if not self.isRingStarted then
        self.curMoveCoin = nil
        return
    end
    if self.curMoveCoin == nil then
        printError("抢金币-错误-没有正在移动的金币")
    else
        --print("松开")
        self:checkInCalBounds()
        self.curMoveCoin = nil
        local sum = self:calSum()
        -- 单环结束
        if sum == self.targetNum then
            --print("计算区域的和等于目标数字")
            self.isRingStarted = false
            self.ringOverTime = Time.time
            local ringScore = self:calRingScore()
            self.score = self.score + ringScore
            self.main_mid.score_text.text = tostring(self.score)
            CoinNetModule.sendReqUpdateMatchScore(self.score)
            self:playRingEndEffect(function()
                self:poolAllStack()
                self:restartRing()
            end)
        else
            --print("计算区域的和不等于目标数字")
        end
    end
end

function this:restartRing()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(this.RingTimerKey)
    GlobalTimeManager.Instance.timerController:AddTimer(this.RingTimerKey, 1000, 1, function ()
        self:destroyAllStack()
        self:startRing()
    end)
end

function this:checkInCalBounds()
    local worldPos = self.curMoveCoin:GetPieceWorldPosition()
    local centerPoint = UICamera:WorldToScreenPoint(worldPos)
    --print(string.format("移动的金币所在屏幕点为: (%s,%s)",centerPoint.x, centerPoint.y))
    local isCenterIn = RectTransformUtility.RectangleContainsScreenPoint(self.main_mid.cal_panel.rectTransform, Vector2(centerPoint.x, centerPoint.y), UICamera)
    if isCenterIn then
        --print("移进计算区域")
        self.calDic[self.curMoveCoin.key] = self.curMoveCoin
        self:playInCalEffect()
    else
        --print("移出计算区域")
        self.calDic[self.curMoveCoin.key] = nil
    end
end

function this:playInCalEffect()
    local obj = CoinObjectPool:GetInstance():GetObject("q_fx_star_001")
    local pos = self.curMoveCoin:GetPieceWorldPosition()
    obj.transform.localPosition = pos
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("CoinPickSuccess")
    GlobalTimeManager.Instance.timerController:AddTimer("CoinPickSuccess", 500, 1, function ()
        CoinObjectPool:GetInstance():PoolObject(obj)
    end)
end

function this:playRingEndEffect(action)
    local obj = CoinObjectPool:GetInstance():GetObject("q_fx_souji_001")
    local pos = self.main_mid.cal_panel.rectTransform.position
    obj.transform.localScale = Vector3.one
    obj.transform.localPosition = pos
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("CoinRingEndEffect")
    GlobalTimeManager.Instance.timerController:AddTimer("CoinRingEndEffect", 2000, 1, function ()
        CoinObjectPool:GetInstance():PoolObject(obj)
        if action ~= nil then
            action()
        end
    end)
end

function this:calSum()
    local sum = 0
    if table.empty(self.calDic) then
        return sum
    end
    for _, v in pairs(self.calDic) do
        sum = sum + v.faceValue
    end
    return sum
end

function this:calRingScore()
    local baseScore = 50
    local ringDeltaTime = self.ringOverTime - self.ringStartTime
    local ringScore = 0
    ringScore = Mathf.RoundToInt(((Mathf.Pow((1.3 +(self.targetNum * 0.005)), (11 - ringDeltaTime))) * 10 + baseScore))
    return ringScore
end

function this:poolAllStack()
    if self:getIsLoaded() == false then
        return
    end
    if self.pieceDic ~= nil then
        for _, v in pairs(self.pieceDic) do
            v:PlayPoolAnim()
        end
    end
end

function this:destroyAllStack()
    if self:getIsLoaded() == false then
        return
    end
    if self.pieceDic ~= nil then
        for _, v in pairs(self.pieceDic) do
            v:DestroyPiece()
        end
    end
end
function this:onOverGameView()

end
--=========================================
function this:GetScore()
    return self.score
end


function this:OnExitGameView()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(this.StartTimerKey)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(this.GameTimerKey)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(this.RingTimerKey)
    if self:getIsLoaded() == false then
        return
    end
    self:poolAllStack()
end