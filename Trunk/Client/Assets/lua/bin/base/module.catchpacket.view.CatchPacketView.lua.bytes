---
--- Created by Administrator.
--- DateTime: 2018\9\18 0018 13:47
---

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "base:mid/catchpacket/Mid_catchpacket_game_panel"

local RectTransformUtility = CS.UnityEngine.RectTransformUtility
local UICamera = CS.UIManager.Instance.UICamera

CatchPacketView = BaseView:new()
local this = CatchPacketView
this.viewName = "CatchPacketView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Help_View, UIViewEnum.CatchPacket_GameView, true)

local StartTimerKey  = "CatchPacketStartTimer" -- 开始倒计时
local GameTimerKey  = "CatchPacketGameTimer" -- 游戏倒计时
local MammonMove    = "CatchPacketMammonMove"
local PigMove       = "CatchPacketPigMove"
local PigParalyzed  = "CatchPacketPigParalyzed"
local ItemSpawn     = "CatchPacketItemSpawn"
local ItemMove      = "CatchPacketItemMove"

local SpawnTimespan = 0.3       --道具产生间隔(秒)
local ParalyzedTimespan = 1       --麻痹时长(秒)

local mammonSpeed = 500   --财神的移动速度(像素),右为正, 左为负
local pigSpeed = 1000           --小猪的移动速度(像素), 右为正, 左为负
local itemSpeed = -500          --道具的基础移动速度(像素), 上为正, 下为负
local mammonBound = { -540 + 150, 540 - 150 }
local pigBound = { -540 + 100, 540 - 100 }
local groundY = -800
local Horizontal = 0

local p
local m
local itemDataQueue = {}
local dropItemList = {}
local catchItemList = {}
-- 不同道具掉落速度倍率
local ItemTypeSpeedScale =
{
    [1] = 1.5,
    [2] = 1.2,
    [3] = 0.8,
    [4] = 1,
}
-- 不同道具得分
local ItemTypeScore =
{
    [1] = 10,
    [2] = 5,
    [3] = 1,
    [4] = 0,
}

this.startTimerCount = 0
this.gameTimerCount = 0
this.isPause = false
this.isGameStarted = false
this.isParalyzed = false     -- 是否被麻痹

this.totalMoney = 0       -- 理论红包额度
this.catchMoney = 0       -- 实际得分
this.totalScore = 0       -- 理论总分
this.catchScore = 0       -- 实际得分
this.leftHeld = false
this.rightHeld = false


--设置加载列表
this.loadOrders =
{
    "base:catchpacket/catchpacket_game_panel"
}

function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_catchpacket_game_panel
    self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    p = self.main_mid.pig_spine
    m = self.main_mid.mammon_spine
    self:addEvent()
end

function this:onShowHandler(msg)
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
    self.totalMoney = msg
    self:initAllItemData()
    self:hideOtherPanel()
    self:startTimer()
    NoticeManager.Instance:AddNoticeLister(NoticeType.Socket_Error, function ()
        self:backToPlatform()
    end)
end

function this:addEvent()
    self.main_mid.left_btn:AddEventListener(UIEvent.PointerDown, this.leftEventHandler)
    self.main_mid.left_btn:AddEventListener(UIEvent.PointerUp, this.stopEventHandler)
    self.main_mid.left_image:AddEventListener(UIEvent.PointerDown, this.leftEventHandler)
    self.main_mid.left_image:AddEventListener(UIEvent.PointerUp, this.stopEventHandler)

    self.main_mid.right_btn:AddEventListener(UIEvent.PointerDown, this.rightEventHandler)
    self.main_mid.right_btn:AddEventListener(UIEvent.PointerUp, this.stopEventHandler)
    self.main_mid.right_image:AddEventListener(UIEvent.PointerDown, this.rightEventHandler)
    self.main_mid.right_image:AddEventListener(UIEvent.PointerUp, this.stopEventHandler)

    self.main_mid.exit_btn:AddEventListener(UIEvent.PointerClick, function ()
        self.main_mid.popup_panel.gameObject:SetActive(true)
    end)
    self.main_mid.exit_confirm_btn:AddEventListener(UIEvent.PointerClick, function ()
        self.main_mid.popup_panel.gameObject:SetActive(true)
        self:backToPlatform()
    end)
    self.main_mid.exit_continue_btn:AddEventListener(UIEvent.PointerClick, function ()
        self.main_mid.popup_panel.gameObject:SetActive(false)
    end)
    self.main_mid.over_exit_btn:AddEventListener(UIEvent.PointerClick, function ()
        self:backToPlatform()
    end)
end

function this:addTimer()
    GlobalTimeManager.Instance.timerController:AddTimer(MammonMove, -1, -1, function ()
        self:ProcessMammonMove();
    end)
    GlobalTimeManager.Instance.timerController:AddTimer(PigMove, -1, -1, function ()
        if self.isParalyzed then
            return
        end
        self:ProcessPigFace();
        self:ProcessPigMove();
    end)
    GlobalTimeManager.Instance.timerController:AddTimer(ItemSpawn, SpawnTimespan * 1000, -1, function ()
        self:spawnItem();
    end)
    GlobalTimeManager.Instance.timerController:AddTimer(ItemMove, -1, -1, function ()
        self:ProcessItemMove();
    end)
end

function this:removeTimer()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(MammonMove)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(PigMove)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(ItemSpawn)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(ItemMove)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(GameTimerKey)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(StartTimerKey)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(PigParalyzed)

end


function this:getInput()
    local leftHeld = this.leftHeld
    local rightHeld = this.rightHeld
    if leftHeld == rightHeld then
        Horizontal = 0
    elseif leftHeld then
        Horizontal = -1
    else
        Horizontal = 1
    end
end

function this.leftEventHandler(eventData)
    this.leftHeld = true
    this:getInput()
end

function this.rightEventHandler(eventData)
    this.rightHeld = true
    this:getInput()
end

function this.stopEventHandler(eventData)
    this.leftHeld = false
    this.rightHeld = false
    this:getInput()
    this:stopPigAnim()
end

function this:stopPigAnim()
    if self.isParalyzed then
        return
    end
    if (p.skeleton.AnimationState:GetCurrent(0).Animation.Name ~= "piglet_stop") then
        p.skeleton.AnimationState:SetAnimation(0, "piglet_stop", true);
    end
end

function this:ProcessPigFace()
    if (Horizontal == -1) then
        p.skeleton.Skeleton.FlipX = false;
    elseif (Horizontal == 1) then
        p.skeleton.Skeleton.FlipX = true;
    end
end

function this:ProcessPigMove()
    local speed = Horizontal * pigSpeed;
    local targetX = p.rectTransform.localPosition.x + Time.deltaTime * speed;
    local targetY = p.rectTransform.localPosition.y;
    local targetZ = p.rectTransform.localPosition.z;
    targetX = Mathf.Clamp(targetX, pigBound[1], pigBound[2]);
    p.rectTransform.localPosition = Vector3(targetX, targetY, targetZ);
    if (speed ~= 0 and p.skeleton.AnimationState:GetCurrent(0).Animation.Name ~= "piglet_run") then
        p.skeleton.AnimationState:SetAnimation(0, "piglet_run", true);
    end
end

function this:ProcessMammonMove()
    local speed = 0;
    if (m.skeleton.Skeleton.FlipX) then
        speed = mammonSpeed;
    else
        speed = -mammonSpeed;
    end
    local targetX = m.rectTransform.localPosition.x + Time.deltaTime * speed;
    local targetY = m.rectTransform.localPosition.y;
    local targetZ = m.rectTransform.localPosition.z;
    m.rectTransform.localPosition = Vector3(targetX, targetY, targetZ);
    if (m.rectTransform.localPosition.x <= mammonBound[1]) then
        m.skeleton.Skeleton.FlipX = true;
    elseif (m.rectTransform.localPosition.x >= mammonBound[2]) then
        m.skeleton.Skeleton.FlipX = false;
    end
end

function this:ProcessItemMove()
    for _, item in pairs(dropItemList) do
        if item.isMoveEnd == false then
            local speed = ItemTypeSpeedScale[item.itemType] * itemSpeed

            local targetX = item.imageWidget.transform.localPosition.x;
            local targetY = item.imageWidget.transform.localPosition.y + Time.deltaTime * speed;
            local targetZ = item.imageWidget.transform.localPosition.z;
            item.imageWidget.transform.localPosition = Vector3(targetX, targetY, targetZ);
            local centerPoint = UICamera:WorldToScreenPoint(item.imageWidget.transform.position)
            --print(string.format("移动的道具所在屏幕点为: (%s,%s)",centerPoint.x, centerPoint.y))
            local isCenterIn = RectTransformUtility.RectangleContainsScreenPoint(self.main_mid.collider_panel.rectTransform, Vector2(centerPoint.x, centerPoint.y), UICamera)
            if isCenterIn and item.isMoveEnd == false then
                item.isMoveEnd = true
                item.imageWidget:SetPng(nil)
                item.flashEffectWidget:Stop()
                --print("移进计算区域, type: "..item.itemType)
                self.main_mid.collider_effect:Play()
                table.insert(catchItemList, item)
                if item.itemType == 4 then
                    self.main_mid.paralyzed_effect:Play()
                    self:pigParalyzed()
                    AudioManager.playSound("base", "catch_packet_paralayzed")
                else
                    AudioManager.playSound("base", "catch_packet_catched")
                end
            end
            if item.imageWidget.transform.localPosition.y <= groundY  then
                item.isMoveEnd = true
                item.imageWidget.Img:CrossFadeAlpha(0, 1, true)
                item.flashEffectWidget:Stop()
            end
        end
    end
end

-- 小猪被麻痹了
function this:pigParalyzed()
    --print("被麻痹了")
    self.isParalyzed = true
    p.skeleton.AnimationState:SetAnimation(0, "piglet_lightning", true);

    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(PigParalyzed)
    GlobalTimeManager.Instance.timerController:AddTimer(PigParalyzed, ParalyzedTimespan * 1000, -1, function ()
        --print("解除麻痹")
        self.isParalyzed = false
        self.main_mid.paralyzed_effect:Stop()
        p.skeleton.AnimationState:SetAnimation(0, "piglet_stop", true);
    end)
end

function this:initAllItemData()
    self.gameTimerCount = 30
    self.totalScore = 0
    local totalCount = Mathf.FloorToInt(self.gameTimerCount / SpawnTimespan);
    --local totalCount = 1
    --print("totalCount: "..totalCount)
    itemDataQueue = Queue.new()
    for i = 1, totalCount do
        local itemType = 0;
        local index = Random.Range(1, 100);
        local itemScore = 0
        if index <= 10 then
            itemType = 1;
            itemScore = 10
        elseif (index > 10 and index <= 30) then
            itemType = 2;
            itemScore = 5
        elseif (index > 30 and index <= 80) then
            itemType = 3;
            itemScore = 1
        elseif (index > 80 and index <= 100) then
            itemType = 4;
            itemScore = 0
        end
        self.totalScore = self.totalScore + itemScore
        Queue.enqueue(itemDataQueue, itemType)
    end

    --print("itemDataQueue: "..table.tostring(itemDataQueue))
    --print("totalScore: "..self.totalScore)
    AudioManager.playBGM("base", "catch_packet_bgm")
end

function this:spawnItem()
    if (itemDataQueue.count ~= 0) then
        if (m.skeleton.AnimationState:GetCurrent(0).Animation.Name ~= "anim_002") then
            m.skeleton.AnimationState:SetAnimation(0, "anim_002", true);
        end
        local itemType = Queue.dequeue(itemDataQueue)
        local image = GameObject.Instantiate(self.main_mid.itemPre);
        image:SetPng(self.main_mid.item_pool_icon.IconArr[itemType - 1]);
        image.Img:SetNativeSize();
        image.transform:SetParent(self.main_mid.map_panel.transform);
        image.transform.localPosition = m.rectTransform.localPosition;
        image.transform.localScale = Vector3.one;
        local item ={}
        item.itemType = itemType
        item.imageWidget = image
        item.isMoveEnd = false
        item.flashEffectWidget = image.transform:Find("flash_effect"):GetComponent(typeof(CS.EffectWidget))
        if itemType == 4 then
            item.flashEffectWidget:Play()
        end
        table.insert(dropItemList, item)
    end
end

function this:hideOtherPanel()
    self.main_mid.over_panel.gameObject:SetActive(false)
    self.main_mid.popup_panel.gameObject:SetActive(false)
    self.main_mid.start_timer_icon.gameObject:SetActive(false)
    self.main_mid.timer_bg.gameObject:SetActive(false)
    self.main_mid.fog_image.gameObject:SetActive(false)
    self.main_mid.item_pool_icon.gameObject:SetActive(false)
    self.main_mid.timer_text.text = ""
    local pigLocalPosition = self.main_mid.pig_spine.transform.localPosition
    local mammonLocalPosition = self.main_mid.mammon_spine.transform.localPosition
    self.main_mid.pig_spine.transform.localPosition = Vector3(0, pigLocalPosition.y, pigLocalPosition.z)
    self.main_mid.mammon_spine.transform.localPosition = Vector3(0, mammonLocalPosition.y, mammonLocalPosition.z)
end

function this:startTimer()
    self.startTimerCount = 4
    self.main_mid.fog_image.gameObject:SetActive(true)
    GlobalTimeManager.Instance.timerController:AddTimer(StartTimerKey, 1000, -1, function ()
        if self.startTimerCount > 0  then
            self:showStartTimerAnim(self.startTimerCount)
            self.startTimerCount = self.startTimerCount - 1
            AudioManager.playSound("base", "catch_packet_start_timer")
            --print("count: "..self.startTimerCount)
            if self.startTimerCount == 0 then
                self:startGame()
                AudioManager.playSound("base", "catch_packet_go")
            end
        end
    end)
    self.main_mid.mammon_spine.skeleton.AnimationState:SetAnimation(0, "anim_001", true)
    self.main_mid.pig_spine.skeleton.AnimationState:SetAnimation(0, "piglet_stop", true)

end

function this:startGame()
    self.main_mid.timer_bg.gameObject:SetActive(true)
    self.main_mid.timer_text.text = string.format("%s%s", self.gameTimerCount, "s")
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(StartTimerKey)
    --print("开始游戏")
    GlobalTimeManager.Instance.timerController:AddTimer(GameTimerKey, 1000, -1, function ()
        self.main_mid.timer_text.text = string.format("%s%s", self.gameTimerCount, "s")
        if self.gameTimerCount > 0  then
            self.gameTimerCount = self.gameTimerCount - 1
        elseif self.gameTimerCount == 0 then
            self:gameOver()
        end
    end)
    self:addTimer()
end

function this:gameOver()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(GameTimerKey)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(MammonMove)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(PigParalyzed)
    self.main_mid.mammon_spine.skeleton.AnimationState:SetAnimation(0, "anim_001", true)

    self.isGameStarted = false
    self.catchScore = 0
    for i, v in pairs(catchItemList) do
        local itemScore = ItemTypeScore[v.itemType]
        self.catchScore = self.catchScore + itemScore
    end
    --print("游戏结束, catchScore: "..self.catchScore)
    self.catchMoney = 0
    if self.totalScore ~= 0 then
        self.catchMoney = math.floor(self.totalMoney * (self.catchScore / self.totalScore))
    end
    self.main_mid.over_money_text.text = string.format("获得\n\n%s元", CS.System.String.Format("{0:0.##}", self.catchMoney / 100))
    self.main_mid.popup_panel.gameObject:SetActive(false)
    self.main_mid.over_panel.gameObject:SetActive(true)
    AudioManager.playSound("base", "catch_packet_account")

end

function this:showStartTimerAnim(count)
    if count < 0 then
        return
    end
    --print("count: "..count)
    self.main_mid.start_timer_icon:ChangeIcon(count - 1)
    local curObj = self.main_mid.start_timer_icon
    curObj.gameObject:SetActive(true)
    curObj:setNativeSize()
    curObj.Img:CrossFadeAlpha(1, 0, true)
    local mySequence = DOTween.Sequence()
    curObj.transform.localScale = Vector3.one * 0.5
    local scale_1 = curObj.transform:DOScale(Vector3.one * 1.2, 0.3)
    local scale_2 = curObj.transform:DOScale(Vector3.one, 0.2)
    mySequence:Append(scale_1)
    mySequence:Append(scale_2)
    if count == 1 then
        --print("渐隐雾效")
        self.main_mid.fog_image.Img:CrossFadeAlpha(0, 1, true)
    end
    mySequence:AppendCallback(function()
        curObj.Img:CrossFadeAlpha(0, 0.5, true)
        if count == 0 then
            self.main_mid.fog_image.gameObject:SetActive(false)
        end
        if count == 0 then
            curObj.gameObject:SetActive(false)
        end
    end)
end

function this:backToPlatform()
    AudioManager.stopBGM()
    for i, v in pairs(dropItemList) do
        GameObject.Destroy(v.imageWidget.gameObject)
    end
    itemDataQueue = {}
    dropItemList = {}
    catchItemList = {}
    self.startTimerCount = 0
    self.gameTimerCount = 0
    self.totalMoney = 0
    self.totalScore = 0
    self.practicalScore = 0
    self.isGameStarted = false
    self.isParalyzed = false
    this.leftHeld = false
    this.rightHeld = false
    self:removeTimer()
    NoticeManager.Instance:Dispatch(NoticeType.CatchPacket_End, self.catchMoney)
    self.catchMoney = 0
    ViewManager.close(UIViewEnum.CatchPacket_GameView)
end





