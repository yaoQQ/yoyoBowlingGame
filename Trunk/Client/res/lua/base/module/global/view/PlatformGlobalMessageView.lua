require "base:enum/UIViewEnum"
require "base:mid/global/Mid_platform_message_panel"
require "base:module/message/data/PlatformMessageProxy"

--主界面：消息
PlatformGlobalMessageView = BaseView:new()
local this = PlatformGlobalMessageView
this.viewName = "PlatformGlobalMessageView"

--设置面板特性
this:setViewAttribute(UIViewType.Main_view, UIViewEnum.Platform_Global_Message_View, true)

--设置加载列表
this.loadOrders = {
    "base:global/platform_message_panel"
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

--override 打开UI回调
function this:onShowHandler(msg)
    local go = self:getViewGO()
    if go == nil then
        return
    end
    go.transform:SetAsLastSibling()
    self:addNotice()
    --请求邮件列表
    local data = {
        mail_type = ProtoEnumCommon.MailType.MailType_Normal,
        page_index = 0,
        per_page_count = 100
    }
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Mail, data)
    self:updateMessagePanel()
    --打开子界面
    ViewManager.open(UIViewEnum.Platform_Global_View, UIViewEnum.Platform_Global_Message_View)
end

function this:onClose()
    --关闭子界面
    ViewManager.close(UIViewEnum.Platform_Global_View)
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Update_Mail_Data, this.updateMessagePanel)
    NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Get_All_Mail, this.updateMessagePanel)
    NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Confirm_UserMsg, this.updateMessagePanel)
    NoticeManager.Instance:AddNoticeLister(PlatformFriendType.Receive_Update_Friend_List, this.updateMessagePanel)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(
        PlatformGlobalNoticeType.Platform_Update_Mail_Data,
        this.updateMessagePanel
    )
    NoticeManager.Instance:RemoveNoticeLister(PlatformGlobalNoticeType.Platform_Get_All_Mail, this.updateMessagePanel)
    NoticeManager.Instance:RemoveNoticeLister(
        PlatformGlobalNoticeType.Platform_Confirm_UserMsg,
        this.updateMessagePanel
    )
    NoticeManager.Instance:RemoveNoticeLister(PlatformFriendType.Receive_Update_Friend_List, this.updateMessagePanel)
end
function this:addEvent()
end

this.MsgType = {
    None = 0, --无状态
    FriendMsg = 1, --好友消息
    MailMsg = 2, --邮件消息
    UserRedPacketMsg = 3, --红包消息
    UserMatchMsg = 4, --活动消息
    UserEventMsg = 5 --系统通知
}

function this:SortUserListDataByTime(list)
    if list == nil then
        return
    end
    table.sort(
        list,
        function(a, b)
            if a.time ~= b.time then
                return tonumber(a.time) > tonumber(b.time)
            end
            return false
        end
    )
end

--更新消息界面
function this:updateMessagePanel()
    this.currMsgListData = {}
    local mailListData = PlatformMessageProxy.getNormalTitleMailData()
    if not table.empty(mailListData) then
        mailListData.msgType = this.MsgType.MailMsg
        mailListData.time = mailListData.create_time
        table.insert(this.currMsgListData, mailListData)
    end

    local userRedPacketData = PlatformMessageProxy.getRedPacketListData()
    local userSaveIdList = {}

    printDebug("红包消息的的数据啊" .. table.tostring(userRedPacketData))
    if not table.empty(userRedPacketData) then
        for i = 1, #userRedPacketData do
            userRedPacketData[i].msgType = this.MsgType.UserRedPacketMsg
            if userRedPacketData[i].status == ProtoEnumCommon.MsgStatus.MsgStatus_UnRead then
                table.insert(userSaveIdList, userRedPacketData[i].save_id)
            end
        end
        table.insert(this.currMsgListData, userRedPacketData[1])
    end
    local userMatchData = PlatformMessageProxy.getMatchListData()
    printDebug("活动消息的的数据啊" .. table.tostring(userMatchData))
    if not table.empty(userMatchData) then
        for i = 1, #userMatchData do
            userMatchData[i].msgType = this.MsgType.UserMatchMsg
            if userMatchData[i].status == ProtoEnumCommon.MsgStatus.MsgStatus_UnRead then
                table.insert(userSaveIdList, userMatchData[i].save_id)
            end
        end
        table.insert(this.currMsgListData, userMatchData[1])
    end

    local userEventData = PlatformMessageProxy.getEventListData()
    printDebug("系统公告的的数据啊" .. table.tostring(userEventData))
    if not table.empty(userEventData) then
        for i = 1, #userEventData do
            userEventData[i].msgType = this.MsgType.UserEventMsg
            if userEventData[i].status == ProtoEnumCommon.MsgStatus.MsgStatus_UnRead then
                table.insert(userSaveIdList, userEventData[i].save_id)
            end
        end
        table.insert(this.currMsgListData, userEventData[1])
    end

    this:SortUserListDataByTime(this.currMsgListData)
    --好友消息
    local friendMsgPlayerIdList = {}
    local frinedListData = PlatformFriendProxy:GetInstance():getFriendChatListData()
    if not table.empty(frinedListData) then
        for i = 1, #frinedListData do
            local curChatPlayerData = PlatformFriendProxy:GetInstance():getFriendDataById(frinedListData[i].playerId)
            if curChatPlayerData ~= nil and curChatPlayerData ~= "" then
                frinedListData[i].msgType = this.MsgType.FriendMsg
                table.insert(this.currMsgListData, frinedListData[i])
                if frinedListData[i].unVisitedCount ~= 0 then
                    table.insert(friendMsgPlayerIdList, frinedListData[i].playerId)
                end
            end
        end
    end

    this.main_mid.ignore_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            local isHaveIgnore = false
            if not table.empty(friendMsgPlayerIdList) then
                for _, v in pairs(friendMsgPlayerIdList) do
                    PlatformFriendProxy:GetInstance():setCurrChatFriendDataReaded(v)
                end
                isHaveIgnore = true
            end
            if not table.empty(userSaveIdList) then
                PlatformMessageModule.sendReqSetUserNotifyStatus(
                    ProtoEnumCommon.MsgStatus.MsgStatus_Read,
                    userSaveIdList
                )
                isHaveIgnore = true
            end
            if not isHaveIgnore then
                return showTopTips("没有可忽略未读消息")
            end
            --更新主页红点
            PlatformGlobalView:onUpdateChatOnlineMsgCount()
            --更新列表红点
            this:updateMessagePanel()
        end
    )
    if #this.currMsgListData == 0 then
        this.main_mid.noMessage_Image.gameObject:SetActive(true)
        this.main_mid.chatlist_CellRecycleScrollPanel.gameObject:SetActive(false)
    else
        this.main_mid.noMessage_Image.gameObject:SetActive(false)
        this.main_mid.chatlist_CellRecycleScrollPanel.gameObject:SetActive(true)
        this.main_mid.chatlist_CellRecycleScrollPanel:SetCellData(this.currMsgListData, this.onSetMessageInfo, true)
    end
end

--好友消息
local function onFriendMsg(item, data, index)
    item.config_Panel.gameObject:SetActive(false)
    item.friend_Panel.gameObject:SetActive(true)
    this.curChatPlayerData = PlatformFriendProxy:GetInstance():getFriendDataById(data.playerId)
    downloadUserHead(this.curChatPlayerData.player_base_info.head_url, item.head_Image)
    item.name_Text.text = this.curChatPlayerData.player_base_info.nick_name
    item.honor_Text.text = data.my_chat_info[#data.my_chat_info].msg
    item.time_Text.text = os.date("%Y/%m/%d %H:%M", data.my_chat_info[#data.my_chat_info].time)

    if data.isVisited then
        item.msg_icon.gameObject:SetActive(false)
    else
        if data.unVisitedCount == 0 then
            item.msg_icon.gameObject:SetActive(false)
        else
            item.msgcount_Text.text = data.unVisitedCount > 99 and 99 or data.unVisitedCount

            item.msg_icon.gameObject:SetActive(true)
        end
    end
    item.press_Image.name = data.playerId
    item.press_Image:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            local selectedObj = eventData.pointerPress
            --  PlatformFriendProxy:GetInstance():setCurrChatFriendData(tonumber(selectedObj.name))
            FriendChatDataProxy.currChatFriendId = tonumber(selectedObj.name)
            ViewManager.open(UIViewEnum.Platform_Friend_Chat_View, {isMain = true})
        end
    )
end

--邮件消息
local function onMailMsg(item, data, index)
    item.config_Panel.gameObject:SetActive(true)
    item.friend_Panel.gameObject:SetActive(false)
    item.configname_Text.text = "奖励消息"
    item.confighonor_Text.text = data.subject
    -- printError("+++++++++++"..data.subject)
    local iconIndex = data.award_flag and 0 or 1 --0为灰图
    item.config_Icon:ChangeIcon(iconIndex)
    local mail_count = PlatformMessageProxy.getMailCount()
    item.configredpoint_Icon.gameObject:SetActive(mail_count > 0)
    item.configredpoint_Text.text = mail_count
    item.config_image:AddEventListener(
        UIEvent.PointerClick,
        function()
            --printError("我点击了")
            ViewManager.open(UIViewEnum.Platform_Message_Second_View, data)
        end
    )
end

local function onUserRedPacketMsg(item, data, index)
    item.config_Panel.gameObject:SetActive(true)
    item.friend_Panel.gameObject:SetActive(false)
    item.configname_Text.text = "红包消息"
    if data.content == " " then
        item.confighonor_Text.text = data.title1
    else
        item.confighonor_Text.text = data.content
    end
    item.config_Icon:ChangeIcon(2)
    -- local redPacketListNum = PlatformMessageProxy.getRedPacketListNum()
    item.configredpoint_Icon.gameObject:SetActive(false)
    --redPacketListNum > 0)
    -- item.configredpoint_Text.text = redPacketListNum
    item.config_image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.open(UIViewEnum.Platform_Message_Second_View, data)
        end
    )
end

local function onUserMatchMsg(item, data, index)
    item.config_Panel.gameObject:SetActive(true)
    item.friend_Panel.gameObject:SetActive(false)
    item.configname_Text.text = "活动消息"
    item.confighonor_Text.text = data.title1
    if data.status == ProtoEnumCommon.MsgStatus.MsgStatus_Read then
        item.config_Icon:ChangeIcon(4)
    else
        item.config_Icon:ChangeIcon(5)
    end
    local redpacketcount = PlatformMessageProxy.getMatchListNum()
    item.configredpoint_Icon.gameObject:SetActive(redpacketcount > 0)
    item.configredpoint_Text.text = redpacketcount
    item.config_image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.open(UIViewEnum.Platform_Message_Second_View, data)
        end
    )
end

local function onUserEventMsg(item, data, index)
    item.config_Panel.gameObject:SetActive(true)
    item.friend_Panel.gameObject:SetActive(false)
    item.configname_Text.text = "系统通知"
    item.confighonor_Text.text = data.title1
    if data.status == ProtoEnumCommon.MsgStatus.MsgStatus_Read then
        item.config_Icon:ChangeIcon(6)
    else
        item.config_Icon:ChangeIcon(7)
    end
    local redpacketcount = PlatformMessageProxy.getEventListNum()
    item.configredpoint_Icon.gameObject:SetActive(redpacketcount > 0)
    item.configredpoint_Text.text = redpacketcount
    item.config_image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.open(UIViewEnum.Platform_Message_Second_View, data)
        end
    )
end

this.MessageTypeFunc = {}
this.MessageTypeFunc[this.MsgType.FriendMsg] = onFriendMsg
this.MessageTypeFunc[this.MsgType.MailMsg] = onMailMsg
this.MessageTypeFunc[this.MsgType.UserRedPacketMsg] = onUserRedPacketMsg
this.MessageTypeFunc[this.MsgType.UserMatchMsg] = onUserMatchMsg
this.MessageTypeFunc[this.MsgType.UserEventMsg] = onUserEventMsg

--设置聊天信息
function this.onSetMessageInfo(go, data, index)
    local item = this.main_mid.friendchatlistcellArr[index + 1]
    this.MessageTypeFunc[data.msgType](item, data, index)
end
