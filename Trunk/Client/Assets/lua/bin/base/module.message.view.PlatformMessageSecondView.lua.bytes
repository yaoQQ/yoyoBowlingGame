require "base:mid/message/Mid_platform_message_second_panel"
require "base:module/message/data/PlatformMessageProxy"

--主界面：消息
PlatformMessageSecondView = BaseView:new()
local this = PlatformMessageSecondView
this.viewName = "PlatformMessageSecondView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Message_Second_View, true)

--设置加载列表
this.loadOrders = {
    "base:message/platform_message_second_panel"
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

local m_messageType = nil
local m_currMessageListData = {}
--override 打开UI回调
function this:onShowHandler(msg)
    local go = self:getViewGO()
    if go == nil then
        return
    end
    self:initView()
    self:addNotice()
    go.transform:SetAsLastSibling()
    m_messageType = msg.msgType
    self:updateMessageList()
    --请求邮件列表
    local data = {
        mail_type = ProtoEnumCommon.MailType.MailType_Normal,
        page_index = 0,
        per_page_count = 100
    }
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Mail, data)
    --打开子界面
    ViewManager.open(UIViewEnum.Platform_Global_View, UIViewEnum.Platform_Message_Second_View)
    if m_messageType == PlatformGlobalMessageView.MsgType.UserRedPacketMsg then
        this:sendReqSetUserNotifyStatus()
    end
end

--override 关闭UI回调
function this:onClose()
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Get_All_Mail, this.updateMessageList)
    NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Update_Mail_Data, this.updateMessageList)
    NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Confirm_UserMsg, this.updateMessageList)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(
        PlatformGlobalNoticeType.Platform_Update_Mail_Data,
        this.updateMessageList
    )
    NoticeManager.Instance:RemoveNoticeLister(PlatformGlobalNoticeType.Platform_Get_All_Mail, this.updateMessageList)
    NoticeManager.Instance:RemoveNoticeLister(PlatformGlobalNoticeType.Platform_Confirm_UserMsg, this.updateMessageList)
end

function this:initView()
    this.main_mid.ignore_Image.gameObject:SetActive(false)
end

function this:addEvent()
    self.main_mid.back_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Message_Second_View)
        end
    )
    self.main_mid.ignore_Image:AddEventListener(UIEvent.PointerClick, this.sendAllMail)
end

function this.updateMessageList()
    if m_messageType == PlatformGlobalMessageView.MsgType.MailMsg then
        this.main_mid.top_text.text = "奖励消息"
        m_currMessageListData = PlatformMessageProxy.getNormalMailData()
    elseif m_messageType == PlatformGlobalMessageView.MsgType.UserRedPacketMsg then
        this.main_mid.top_text.text = "红包消息"
        m_currMessageListData = PlatformMessageProxy.getRedPacketListData()
    elseif m_messageType == PlatformGlobalMessageView.MsgType.UserMatchMsg then
        this.main_mid.top_text.text = "活动消息"
        m_currMessageListData = PlatformMessageProxy.getMatchListData()
    elseif m_messageType == PlatformGlobalMessageView.MsgType.UserEventMsg then
        this.main_mid.top_text.text = "系统公告"
        m_currMessageListData = PlatformMessageProxy.getEventListData()
    end

    this.main_mid.messagelist_CellRecycleScrollPanel:SetCellData(m_currMessageListData, this.onUpdateMessageList, true)
end

function this.onUpdateMessageList(go, data, index)
    if m_messageType == PlatformGlobalMessageView.MsgType.MailMsg then
        this.main_mid.ignore_Image.gameObject:SetActive(true)
        local item = this.main_mid.messagelistcellArr[index + 1]
        local iconIndex = data.award_flag and 0 or 1 --0为灰图
        local time = this:getShowTiem(data.create_time)
        item.second_Icon:ChangeIcon(iconIndex)
        item.title_Text.text = data.subject
        item.second_honor_Text.text = time
        item.second_Image:AddEventListener(
            UIEvent.PointerClick,
            function(eventData)
                data.msgType = PlatformGlobalMessageView.MsgType.MailMsg
                PlatformMessageProxy.setMessageMainData(data)
                ViewManager.open(UIViewEnum.Platform_Message_Main_View)
            end
        )
    elseif m_messageType == PlatformGlobalMessageView.MsgType.UserRedPacketMsg then
        this.main_mid.ignore_Image.gameObject:SetActive(false)
        local item = this.main_mid.messagelistcellArr[index + 1]
        local time = this:getShowTiem(data.time)
        item.second_Icon:ChangeIcon(2)
        -- if data.status == ProtoEnumCommon.MsgStatus.MsgStatus_Read then
        --     item.second_Icon:ChangeIcon(2)
        -- else
        --     item.second_Icon:ChangeIcon(3)
        -- end
        if data.content == " " then
            item.title_Text.text = data.title1
        else
            item.title_Text.text = data.content
        end

        item.second_honor_Text.text = time
        item.second_Image:AddEventListener(
            UIEvent.PointerClick,
            function(eventData)
                ViewManager.close(UIViewEnum.Platform_Message_Second_View)
                ViewManager.close(UIViewEnum.Platform_Global_Message_View)
                ViewManager.open(UIViewEnum.Platform_Global_RedBag_View)
            end
        )
    elseif m_messageType == PlatformGlobalMessageView.MsgType.UserMatchMsg then
        this.main_mid.ignore_Image.gameObject:SetActive(false)
        local item = this.main_mid.messagelistcellArr[index + 1]
        local time = this:getShowTiem(data.time)
        if data.status == ProtoEnumCommon.MsgStatus.MsgStatus_Read then
            item.second_Icon:ChangeIcon(4)
        else
            item.second_Icon:ChangeIcon(5)
        end

        item.title_Text.text = data.title1
        item.second_honor_Text.text = time

        item.second_Image:AddEventListener(
            UIEvent.PointerClick,
            function(eventData)
                PlatformMessageModule.sendReqConfirmUserMsg(data.save_id)
                data.msgType = PlatformGlobalMessageView.MsgType.UserMatchMsg
                PlatformMessageProxy.setMessageMainData(data)
                ViewManager.open(UIViewEnum.Platform_Message_Main_View)
            end
        )
    elseif m_messageType == PlatformGlobalMessageView.MsgType.UserEventMsg then
        this.main_mid.ignore_Image.gameObject:SetActive(false)
        local item = this.main_mid.messagelistcellArr[index + 1]
        local time = this:getShowTiem(data.time)
        if data.status == ProtoEnumCommon.MsgStatus.MsgStatus_Read then
            item.second_Icon:ChangeIcon(6)
        else
            item.second_Icon:ChangeIcon(7)
        end
        item.title_Text.text = data.title1
        item.second_honor_Text.text = time

        item.second_Image:AddEventListener(
            UIEvent.PointerClick,
            function(eventData)
                PlatformMessageModule.sendReqConfirmUserMsg(data.save_id)
                data.msgType = PlatformGlobalMessageView.MsgType.UserEventMsg
                PlatformMessageProxy.setMessageMainData(data)
                ViewManager.open(UIViewEnum.Platform_Message_Main_View)
            end
        )
    else
        printDebug("未知消息类型")
    end
end

-- 根据时间显示不同内容
function this:getShowTiem(messageTime)
    local messageShowTime = ""
    local time = os.time()

    -- 获取当前时间
    local currentTime = os.date("*t", time)

    -- 获取消息时间
    local messageTime = tonumber(messageTime)

    local differenceTime =
        tonumber(currentTime.hour) * 3600 + tonumber(currentTime.min) * 60 + tonumber(currentTime.sec)
    if messageTime < (time - differenceTime - 172800) then
        messageShowTime = tostring(os.date("%m/%d", math.floor(messageTime)))
    elseif messageTime < (time - differenceTime - 86400) then
        messageShowTime = "前天"
    elseif messageTime < (time - differenceTime) then
        messageShowTime = "昨天"
    else
        messageShowTime = tostring(os.date("%H:%M", math.floor(messageTime)))
    end
    return messageShowTime
end

function this.sendAllMail()
    local isHaveAward = false
    if not table.empty(m_currMessageListData) then
        for k, v in pairs(m_currMessageListData) do
            if not v.award_flag then
                isHaveAward = true
                break
            end
        end
    end
    --无可领取时跳出
    if not isHaveAward then
        return showTopTips("没有可领取的邮件")
    end
    PlatformMessageModule.onReqGetAllMailItem()
    --请求邮件列表
    local data = {
        mail_type = ProtoEnumCommon.MailType.MailType_Normal,
        page_index = 0,
        per_page_count = 100
    }
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Mail, data)
end

function this:sendReqSetUserNotifyStatus()
    local statusData = {}
    for i = 1, #m_currMessageListData do
        if m_currMessageListData[i].status == ProtoEnumCommon.MsgStatus.MsgStatus_UnRead then
            table.insert(statusData, m_currMessageListData[i].save_id)
        end
    end
    PlatformMessageModule.sendReqSetUserNotifyStatus(ProtoEnumCommon.MsgStatus.MsgStatus_Read, statusData)
end
