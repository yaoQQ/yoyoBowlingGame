require "base:enum/UIViewEnum"
require "base:mid/shop/Mid_platform_shop_chat_panel"
require "base:enum/PlatformFriendType"

--商家赛事详情界面
PlatformGlobalShopChatView = BaseView:new()
local this = PlatformGlobalShopChatView
this.viewName = "PlatformGlobalShopChatView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Global_Shop_Chat_View, true)

--设置加载列表
this.loadOrders = {
    "base:shop/platform_shop_chat_panel"
}

this.shopId = 0

this.OpenActiviteState = {
    ChatViewRank = 1, --从聊天打开
    GameExitRank = 2 --从游戏打开
}
this.curOpenactiviteState = this.OpenActiviteState.ChatViewRank
--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
    self:InitComponent()
end

--外部调用打开界面
function this.showPlatformGlobalShopChatView(activityId)
    PlatformLBSDataProxy.setActivityDataById(activityId)

    local activityData = PlatformLBSDataProxy.getActivitySingleData()
	
	if activityData.apply.position_type == ProtoEnumCommon.LBSPositionLimitType.LBSPositionLimitType_Near then
		--判断商圈范围
		local dis = MapManager.getDistance(MapManager.userLng, MapManager.userLat, activityData.apply.lng, activityData.apply.lat)
		if dis > TableBaseParameter.data[22].parameter then
			Alert.showAlertMsg(nil, "这是附近赛事，距离"..TableBaseParameter.data[22].parameter.."米内的用户才能参加", "我知道了", nil)
			return
		end
	elseif activityData.apply.position_type == ProtoEnumCommon.LBSPositionLimitType.LBSPositionLimitType_City then
		--判断城市
		if MapManager.userCityCode ~= activityData.apply.city_code then
			--判断商圈范围
			local dis = MapManager.getDistance(MapManager.userLng, MapManager.userLat, activityData.apply.lng, activityData.apply.lat)
			if dis > TableBaseParameter.data[22].parameter then
				Alert.showAlertMsg(nil, "这是城市赛事，只有位于该城市的用户才能参加", "我知道了", nil)
				return
			end
		end
	end

    ViewManager.open(
        UIViewEnum.Platform_Global_Shop_Chat_View,
        {id = activityData.shop_id, dataId = activityId}
    )
end

function this:onShowHandler(msg)
    self:addNotice()
    self.shopId = msg.id
    PlatformLBSDataProxy.setActivityDataById(msg.dataId)
    self:upGlobalShopChatData()
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Notify_Send_Chat, this.onNotifySendChat)
    NoticeManager.Instance:AddNoticeLister(NoticeType.Activity_Update_ActiveGameState, this.onUpdateActiveGameState)
    NoticeManager.Instance:AddNoticeLister(NoticeType.Activity_Update_ActiveIsStart, this.onUpdateActiveIsStart)
    NoticeManager.Instance:AddNoticeLister(NoticeType.Activity_ActiveStart, this.onActiveStart)
    NoticeManager.Instance:AddNoticeLister(NoticeType.Game_Exit_Notice_View, this.onGameExitMsg)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(PlatformGlobalNoticeType.Platform_Notify_Send_Chat, this.onNotifySendChat)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Activity_Update_ActiveGameState, this.onUpdateActiveGameState)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Activity_Update_ActiveIsStart, this.onUpdateActiveIsStart)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Activity_ActiveStart, this.onActiveStart)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Game_Exit_Notice_View, this.onGameExitMsg)
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()

    this.main_mid.head_Image:SetPng(nil)
    this.main_mid.shopname_Text.text = ""

    this.main_mid.title_text.text = ""

    this.main_mid.activitybegintime_Text.text = ""

    if table.empty(this.currActivityData) == false then
        --向服务器发送退出聊天室请求
        NoticeManager.Instance:Dispatch(
            PlatformGlobalNoticeType.Platform_Req_Chat_Channel_Op,
            {
                chat_type = ProtoEnumCommon.ChatType.ChatType_Activity,
                channel_id = this.currActivityData.active_id,
                op = ProtoEnumCommon.ChatChannelOp.ChatChannelOp_Leave
            }
        )
    end
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("GlobalShopActivityTimeCountDown")
end

function this:InitComponent()
    this.main_mid.activitybegintimeBG_Image.gameObject:SetActive(false)
    this.main_mid.noticeToStart_Image_Toggle.gameObject:SetActive(false)
    this.main_mid.enter_game_Button.gameObject:SetActive(false)
    this.main_mid.enter_game_time.gameObject:SetActive(false)
    this.main_mid.checkrank_Text.gameObject:SetActive(false)
end

function this.onGameExitMsg(...)
    this.curOpenactiviteState = this.OpenActiviteState.GameExitRank
    --刷新数据
    NoticeManager.Instance:Dispatch(
        PlatformNoticeType.Platform_Req_Active_Rank,
        {active_id = this.currActivityData.active_id, page_index = 0, num = 100}
    )
end

this.activeIsStart = false
--获取同步是否选择提醒返回
function this.onUpdateActiveIsStart(key, result)
    local isStart = result:GetObj()
    this.main_mid.noticeToStart_Image_Toggle:ChangeIcon(tonumber(isStart))
    this.activeIsStart = tonumber(isStart) == 1
    this.main_mid.noticeToStart_Text.text = this.activeIsStart and "已设置地图上活动提醒" or "<color=#0dace2>活动开始时在地图上提醒我</color>"
end
--设置选择提醒返回
function this.onActiveStart()
    if this.currActivityData then
        --设置成功后同步状态并拉取关注列表
        PlatformShopModule.sendReqIsActiveStartNotify(this.currActivityData.active_id)
        PlatformShopModule.sendReqActiveStartListNotify()
    end
end

--广播聊天消息
function this.onNotifySendChat(notice, rsp)
    local req = rsp:GetObj()
    PlatformGlobalProxy:GetInstance():addChatNotify(tostring(req.channel_id), req)

    this.onFlushChatPanel()
end

function this.sendChatMsg()
    --发送文字
    if this.main_mid.chatImput_InputField.text ~= "" then
        if this.curActivityGameState == this.activityGameState.StateEnum.GameactivityEnd then
            return showTopTips("活动已结束")
        end
        local chatMsg = {
            player_id = LoginDataProxy.playerId,
            -- msg_id = getUUID(),
            chat_msg_type = ProtoEnumCommon.ChatMsgType.ChatMsgType_Text,
            msg = this.main_mid.chatImput_InputField.text,
            time = TimeManager.getServerUnixTime()
        }
        if DisableTermsManager.Instance:IsMatch(chatMsg.msg) then
            Alert.showAlertMsg(nil, "您输入的文字包含违规内容，请修改后再尝试", "确定")
        else
            NoticeManager.Instance:Dispatch(
                PlatformGlobalNoticeType.Platform_Req_Send_Chat,
                {
                    chat_type = ProtoEnumCommon.ChatType.ChatType_Activity,
                    channel_id = this.currActivityData.active_id,
                    chat_info = chatMsg
                }
            )
            this.main_mid.chatImput_InputField.text = ""
        end
    end
end

function this:addEvent()
    self.main_mid.back_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Global_Shop_Chat_View)
        end
    )
    this.main_mid.noticeToStart_Image_Toggle:AddEventListener(
        UIEvent.PointerClick,
        function()
            PlatformShopModule.sendReqActiveStartNotify(this.currActivityData.active_id, not this.activeIsStart)
        end
    )

    self.main_mid.checkreward_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.open(UIViewEnum.Platform_Active_Reward_View)
        end
    )
    self.main_mid.checkrank_Text:AddEventListener(
        UIEvent.PointerClick,
        function()
            --请求排行榜信息
            this.curOpenactiviteState = this.OpenActiviteState.ChatViewRank
            NoticeManager.Instance:Dispatch(
                PlatformNoticeType.Platform_Req_Active_Rank,
                {active_id = this.currActivityData.active_id, page_index = 0, num = 100}
            )
        end
    )

    --查看规则
    self.main_mid.checkRule_Text:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            this.main_mid.rule_Panel.gameObject:SetActive(true)
        end
    )
    self.main_mid.go_Button:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            this.main_mid.rule_Panel.gameObject:SetActive(false)
        end
    )

    self.main_mid.OPenChat_Window:AddEventListener(
        UIEvent.PointerClick,
        function(...)
            if this.curActivityGameState == this.activityGameState.StateEnum.GameactivityEnd then
                return showTopTips("活动已结束")
            end
            ViewManager.open(UIViewEnum.Platform_Active_Chat_View)
        end
    )
    self.main_mid.chatImput_InputField:OnEndEdit(self.sendChatMsg)
end

this.currShopBaseData = nil

this.currActivityData = nil
this.currAct2ShopData = nil
this.currActIsPlayData = nil
this.currPageIndex = 0
function this.changeIndex(index)
    if this.currPageIndex == index then
        return
    end
    this.currPageIndex = index
    if index == 1 then
        this.main_mid.shoppngPage1:ChangeIcon(0)
        this.main_mid.shoppngPage2:ChangeIcon(1)
        this.main_mid.shoppngPage1_text.text = "1"
        this.main_mid.shoppngPage2_text.text = "<color=#0dace2>2</color>"
        this.shop_imageIndex = 0
    elseif index == 2 then
        this.main_mid.shoppngPage1_text.text = "<color=#0dace2>1</color>"
        this.main_mid.shoppngPage2_text.text = "2"
        this.main_mid.shoppngPage1:ChangeIcon(1)
        this.main_mid.shoppngPage2:ChangeIcon(0)
        this.shop_imageIndex = 1
    end
end

function this.onUpdateActiveGameState(key, rsp)
    local state = rsp:GetObj()
    this.currActIsPlayData = state
    this.updateTimerState()
end

local function GetTimeStr(time)
    local h = math.floor(time / 3600)
    local m = math.floor((time - h * 3600) / 60)
    local s = time - h * 3600 - m * 60
    local function full(num)
        if num < 10 then
            return string.concat("0", num)
        end
        return tostring(num)
    end
    return string.concat(full(h), ":", full(m), ":", full(s))
end
local function CountDowm(self, timer, targettext, strTimeParse, colorStrStart, colorStrEnd)
    local curtimer = timer - TimeManager.getServerUnixTime()
    if curtimer <= 0 then
        return this.updateTimerState()
    end
    targettext.text = string.concat(colorStrStart or "", GetTimeStr(curtimer), strTimeParse, colorStrEnd or "")
end

local function CalTimerCount(self, timer, targettext, strTimeParse, colorStrStart, colorStrEnd)
    CountDowm(self, timer, targettext, strTimeParse, colorStrStart, colorStrEnd)
    GlobalTimeManager.Instance.timerController:AddTimer(
        "GlobalShopActivityTimeCountDown",
        1000,
        -1,
        function()
            CountDowm(self, timer, targettext, strTimeParse, colorStrStart, colorStrEnd)
        end
    )
end

--没有到宣传时间
local function onNoneStart(self)
    --这个时候赛事是有问题的
    ViewManager.close(UIViewEnum.Platform_Global_Shop_Chat_View)
    showTopTips("活动未开启")
end
--宣传时间开始
local function onPublictyStart(self)
    this.main_mid.activityStarttime_Text.gameObject:SetActive(true)
    this.main_mid.activitybegintimeBG_Image.gameObject:SetActive(false)
    this.main_mid.noticeToStart_Image_Toggle.gameObject:SetActive(true)
    this.main_mid.checkrank_Text.gameObject:SetActive(false)
    this.main_mid.enter_game_Button.gameObject:SetActive(false)
    this.main_mid.enter_game_time.gameObject:SetActive(false)
    local maxTime = this.currActivityData.apply.start_time
    CalTimerCount(self, maxTime, this.main_mid.activityStarttime_Text, "", "开始倒计时: <color=#0dace2>", "</color>")
end
--宣传时间结束并游戏开始
local function onPublictyEndGameStart(self)
    this.main_mid.activityStarttime_Text.gameObject:SetActive(false)
    this.main_mid.activitybegintimeBG_Image.gameObject:SetActive(false)
    this.main_mid.noticeToStart_Image_Toggle.gameObject:SetActive(false)
    this.main_mid.checkrank_Text.gameObject:SetActive(true)
    this.main_mid.enter_game_Button.gameObject:SetActive(true)
    this.main_mid.enter_game_time.gameObject:SetActive(true)
    local maxTime = this.currActivityData.apply.extra_time
    CalTimerCount(self, maxTime, this.main_mid.enter_game_time, "后活动结束")
    this.main_mid.enter_game_Button:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            NoticeManager.Instance:Dispatch(
                NoticeType.ActivityToEnterGame,
                {
                    gameId = this.curGameInfoData.game_type,
                    shopId = this.shopId,
                    roomId = this.currActivityData.active_id
                }
            )
            PlatformShopModule.gameExitReturn = false
        end
    )
end
--游戏结束时间
local function onGameactivityEnd(self)
    this.main_mid.activityStarttime_Text.gameObject:SetActive(false)
    this.main_mid.activitybegintimeBG_Image.gameObject:SetActive(true)
    this.main_mid.noticeToStart_Image_Toggle.gameObject:SetActive(false)
    this.main_mid.enter_game_Button.gameObject:SetActive(false)
    this.main_mid.enter_game_time.gameObject:SetActive(false)
    this.main_mid.checkrank_Text.gameObject:SetActive(true)
    this.main_mid.activitybegintime_Text.text = string.concat("<color=#ffffff>", "活动已结束", "</color>")
end
--已经参与过活动了
local function onHavePlayGame(self)
    this.main_mid.activityStarttime_Text.gameObject:SetActive(false)
    this.main_mid.noticeToStart_Image_Toggle.gameObject:SetActive(false)
    this.main_mid.checkrank_Text.gameObject:SetActive(true)
    this.main_mid.enter_game_Button.gameObject:SetActive(false)
    this.main_mid.activitybegintimeBG_Image.gameObject:SetActive(true)
    this.main_mid.enter_game_time.gameObject:SetActive(true)
    local maxTime = this.currActivityData.apply.extra_time
    CalTimerCount(self, maxTime, this.main_mid.enter_game_time, "", "剩余入场时间: ")
    this.main_mid.activitybegintime_Text.text = string.concat("<color=#ffffff>", "你已参加活动", "</color>")
end

this.activityGameState = {}

this.activityGameState.StateEnum = {
    NoneStart = 0, --没有到宣传时间
    PublictyStart = 1, --宣传时间开始
    PublictyEndGameStart = 2, --宣传时间结束并游戏开始
    GameactivityEnd = 3, --游戏结束时间
    HavePlayGame = 4 --已经参与过活动了
}

this.activityGameState[this.activityGameState.StateEnum.NoneStart] = onNoneStart
this.activityGameState[this.activityGameState.StateEnum.PublictyStart] = onPublictyStart
this.activityGameState[this.activityGameState.StateEnum.PublictyEndGameStart] = onPublictyEndGameStart
this.activityGameState[this.activityGameState.StateEnum.GameactivityEnd] = onGameactivityEnd
this.activityGameState[this.activityGameState.StateEnum.HavePlayGame] = onHavePlayGame

function this.updateTimerState(...)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("GlobalShopActivityTimeCountDown")
    this.curActivityGameState = this.activityGameState.StateEnum.GameactivityEnd
    local time = TimeManager.getServerUnixTime()
    if
        this.currActIsPlayData == ProtoEnumCommon.AactiveGameState.AactiveGameState_UnCanJion and
            time < this.currActivityData.apply.extra_time
     then
        this.curActivityGameState = this.activityGameState.StateEnum.HavePlayGame
        return this.activityGameState[this.activityGameState.StateEnum.HavePlayGame](this)
    end
    if time < this.currActivityData.apply.propaganda_start_time then
        this.curActivityGameState = this.activityGameState.StateEnum.NoneStart
        this.activityGameState[this.activityGameState.StateEnum.NoneStart](this)
    elseif time > this.currActivityData.apply.propaganda_start_time and time < this.currActivityData.apply.start_time then
        this.curActivityGameState = this.activityGameState.StateEnum.PublictyStart
        this.activityGameState[this.activityGameState.StateEnum.PublictyStart](this)
    elseif time < this.currActivityData.apply.extra_time then
        this.curActivityGameState = this.activityGameState.StateEnum.PublictyEndGameStart
        this.activityGameState[this.activityGameState.StateEnum.PublictyEndGameStart](this)
    else
        this.curActivityGameState = this.activityGameState.StateEnum.GameactivityEnd
        this.activityGameState[this.activityGameState.StateEnum.GameactivityEnd](this)
    end
end

--更新商家详细信息
function this:upGlobalShopChatData()
    this.currActivityData = PlatformLBSDataProxy.getActivitySingleData()

    this.currAct2ShopData = PlatformLBSDataProxy.getRedBagShopDataByShopId(this.currActivityData.shop_id)

    this.curGameInfoData = this.currActivityData.apply

    if this.currActivityData == nil or this.currAct2ShopData == nil then
        return
    end
    PlatformShopModule.sendReqIsActiveStartNotify(this.currActivityData.active_id)

    PlatformShopModule.sendReqGetActiveGameState(this.currActivityData.active_id)

    downloadMerchantHead(this.currAct2ShopData.headurl, this.main_mid.head_Image)

    this.main_mid.shopname_Text.text = this.currAct2ShopData.name

    this.targetZoneUrl = this.currActivityData.propagate.special_zone_url

    if this.targetZoneUrl == nil then
        this.targetZoneUrl = {}
    end

    printDebug("活动赛事详情商家宣传图" .. table.tostring(this.targetZoneUrl))

    this.main_mid.rule_Panel.gameObject:SetActive(false)

    this.main_mid.shoppngPage1.gameObject:SetActive(#this.targetZoneUrl > 5)
    this.main_mid.shoppngPage2.gameObject:SetActive(#this.targetZoneUrl > 5)
    this.changeIndex(1)
    this.main_mid.shoppng_CellRecycleScrollPanel:SetCellData(this.targetZoneUrl, this.updatePhotos, true)

    this.main_mid.title_text.text = this.curGameInfoData.subject
    downloadGameIcon(this.curGameInfoData.game_type, this.main_mid.game_Icon)
    this.main_mid.gamename_Text.text = TableBaseGameList.data[this.curGameInfoData.game_type].name

    local chatMsgData = PlatformGlobalProxy:GetInstance():getChatNotify(tostring(this.currActivityData.active_id))
    if chatMsgData == nil then
        chatMsgData = {}
    end
    this.main_mid.haveRedBag.gameObject:SetActive(false)
    this.main_mid.haveCoupon.gameObject:SetActive(false)

    this.main_mid.chatCellRecycleScrollPanel:SetCellData(
        chatMsgData,
        function(go, data, index)
            this.updateChat(go, data, index)
        end,
        false
    )

    --向服务器发送进入房间请求
    NoticeManager.Instance:Dispatch(
        PlatformGlobalNoticeType.Platform_Req_Chat_Channel_Op,
        {
            chat_type = ProtoEnumCommon.ChatType.ChatType_Activity,
            channel_id = this.currActivityData.active_id,
            op = ProtoEnumCommon.ChatChannelOp.ChatChannelOp_Join
        }
    )
    --查看规则
    local gameData = TableBaseGameList.data[this.curGameInfoData.game_type]
    this.main_mid.rule_title_Text.text = gameData.name
    downloadGameIcon(this.curGameInfoData.game_type, this.main_mid.rule_Icon)
    local ruleData = {}
    for i = 1, gameData.count do
        ruleData[i] = {}
        ruleData[i].rule = gameData[string.concat("rule", i)]
        ruleData[i].intro = gameData[string.concat("intro", i)]
    end
    this.main_mid.rule_CellRecycleScrollPanel:SetCellData(ruleData, this.updateRule, false)
end

function this.updateRule(go, data, index)
    local item = this.main_mid.ruleItemArr[index + 1]
    item.title_text.text = data.rule
    item.number_Text.text = data.intro
end

this.currShopNotifyData = nil

--更新聊天
function this.onFlushChatPanel()
    this.currShopNotifyData = PlatformGlobalProxy:GetInstance():getChatNotify(tostring(this.currActivityData.active_id))

    if this.currShopNotifyData == nil then
        return
    end
    this.main_mid.haveRedBag.gameObject:SetActive(false)
    this.main_mid.haveCoupon.gameObject:SetActive(false)
    this.main_mid.chatCellRecycleScrollPanel:SetCellData(
        this.currShopNotifyData,
        function(go, data, index)
            this.updateChat(go, data, index)
        end,
        true
    )
    this.main_mid.chatCellRecycleScrollPanel:SetToContentBottom()
end

this.chatCellOtherPictureNameTab = {}
this.chatCellHeadPicTab = {}

--界限byte计算 1个汉字约等于1.785个数字或字母
local function GetStringWordNum(str, lit)
    local lenInByte = #str
    local count = 0
    local i = 1
    local lastStr = ""
    while true do
        local curByte = string.byte(str, i)
        if i > lenInByte then
            break
        end
        if count >= lit then
            lastStr = "..."
            break
        end
        local byteCount = 1
        if curByte > 0 and curByte < 128 then
            byteCount = 1
        elseif curByte >= 128 and curByte < 224 then
            byteCount = 2
            lit = lit - 0.785
        elseif curByte >= 224 and curByte < 240 then
            byteCount = 3
            lit = lit - 0.785
        elseif curByte >= 240 and curByte <= 247 then
            byteCount = 4
            lit = lit - 0.785
        else
            break
        end
        i = i + byteCount
        count = count + 1
    end
    return count, i - 1, lastStr
end
--限制字符
local limitMsgCount = 48

---文本消息
function this.TextUpdateChatMsgFunction(item, userInfo, msgInfo, data, index, IsSelf)
    local strNameType = msgInfo.player_id == this.currAct2ShopData.player_id and "一一一   " or ""
    local msgStr = string.concat(strNameType, userInfo.nick_name, ":   ", msgInfo.msg)
    local count, byteCount, lastStr = GetStringWordNum(msgStr, limitMsgCount)
    msgStr = string.concat(string.sub(msgStr, 0, byteCount), lastStr)

    item.chat_text.text =
        IsSelf and string.concat("<color=#0dace2>", msgStr, "</color>") or
        string.concat("<color=#424242>", msgStr, "</color>")
end

---图片消息
function this.PictureUpdateChatMsgFuction(item, userInfo, msgInfo, data, index, IsSelf)
    local strNameType = msgInfo.player_id == this.currAct2ShopData.player_id and "一一一   " or ""
    local msgStr = string.concat(strNameType, userInfo.nick_name, ":   ", "[图片]")
    item.chat_text.text =
        IsSelf and string.concat("<color=#0dace2>", msgStr, "</color>") or
        string.concat("<color=#424242>", msgStr, "</color>")
end
---现金红包消息
function this.CashRedpacketUpdateChatMsgFuction(item, userInfo, msgInfo, data, index, IsSelf)
    local strNameType = msgInfo.player_id == this.currAct2ShopData.player_id and "一一一   " or ""
    local msgStr = string.concat(strNameType, userInfo.nick_name, ":   ", "[红包]")
    item.chat_text.text =
        IsSelf and string.concat("<color=#0dace2>", msgStr, "</color>") or
        string.concat("<color=#424242>", msgStr, "</color>")
    this.main_mid.haveRedBag.gameObject:SetActive(true)
end
---卡卷红包消息
function this.CouponUpdateChatMsgFuction(item, userInfo, msgInfo, data, index, IsSelf)
    local strNameType = msgInfo.player_id == this.currAct2ShopData.player_id and "一一一   " or ""
    local msgStr = string.concat(strNameType, userInfo.nick_name, ":", "[券包]")
    item.chat_text.text =
        IsSelf and string.concat("<color=#0dace2>", msgStr, "</color>") or
        string.concat("<color=#424242>", msgStr, "</color>")
    this.main_mid.haveCoupon.gameObject:SetActive(true)
end

--#region end 更新聊天相关

local ChatMsgType = {}

ChatMsgType[ProtoEnumCommon.ChatMsgType.ChatMsgType_Text] = this.TextUpdateChatMsgFunction ---文本消息
ChatMsgType[ProtoEnumCommon.ChatMsgType.ChatMsgType_Picture] = this.PictureUpdateChatMsgFuction ---图片消息
ChatMsgType[ProtoEnumCommon.ChatMsgType.ChatMsgType_Redpacket] = this.CashRedpacketUpdateChatMsgFuction ---现金红包消息
ChatMsgType[ProtoEnumCommon.ChatMsgType.ChatMsgType_Coupon] = this.CouponUpdateChatMsgFuction ---卡卷红包消息

--更新聊天内容
function this.updateChat(go, data, index)
    local item = nil
    item = this.main_mid.chatCellArr[index + 1]
    local userInfo = data.user_base_info
    local msgInfo = data.chat_info
    local b = msgInfo.player_id == LoginDataProxy.playerId
    item.name_type.gameObject:SetActive(msgInfo.player_id == this.currAct2ShopData.player_id)
    return ChatMsgType[msgInfo.chat_msg_type](item, userInfo, msgInfo, data, index, b)
end

--设置图片
function this.updatePhotos(go, data, index, dataIndex)
    local item = this.main_mid.shoppngListArr[index + 1]
    downloadImage(data, item.cellitem1_shop_image)
    this.changeIndex(1)
    if dataIndex >= 5 then
        this.changeIndex(2)
    end
end
