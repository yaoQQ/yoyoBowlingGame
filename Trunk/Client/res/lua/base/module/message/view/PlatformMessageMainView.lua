require "base:mid/message/Mid_platform_message_main_panel"
require "base:module/message/data/PlatformMessageProxy"

--主界面：消息
PlatformMessageMainView = BaseView:new()
local this = PlatformMessageMainView
this.viewName = "PlatformMessageMainView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Message_Main_View, true)

--设置加载列表
this.loadOrders = {
    "base:message/platform_message_main_panel"
}
local m_mainData = {}
local isGet = false
local itemIcon = nil
local isReward = false
--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    self:addEvent()
end

function this:addEvent()
    self.main_mid.answer_back_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Message_Main_View)
        end
    )
    self.main_mid.mask_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            this.main_mid.mid_reward_Panel.gameObject:SetActive(false)
            isReward = false
        end
    )
    self.main_mid.del_mail_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            local local_mainData = PlatformMessageProxy.getMessageMainData()
            local req = {}
            req.mail_type = local_mainData.msgType
            req.mail_id = m_mainData.id
            PlatformMessageModule.onReqDelMail(req)
            this.main_mid.del_mail_Image.gameObject:SetActive(false)
        end
    )
end
--override 打开UI回调
function this:onShowHandler(msg)
    --打开界面时添加UI通知监听
    self:addNotice()
    self:initView()
    self:updataMainMessagePanel()
    --打开界面时初始化，一般用于处理没有数据时的默认的界面显示
end

--override 关闭UI回调
function this:onClose()
    --关闭界面时移除UI通知监听
    self:removeNotice()
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Del_Mail_Data, this.Close)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(PlatformGlobalNoticeType.Platform_Del_Mail_Data, this.Close)
end
-- 外部关闭
function this.Close()
    ViewManager.close(UIViewEnum.Platform_Message_Main_View)
end
--打开界面时初始化
function this:initView()
    this.main_mid.mid_reward_Panel.gameObject:SetActive(false)
    this.main_mid.buttom_Panel.gameObject:SetActive(false)
    this.main_mid.reward_Panel.gameObject:SetActive(false)
    this.main_mid.go_active_Panel.gameObject:SetActive(false)
    this.main_mid.achieve_Button.gameObject:SetActive(true)
    this.main_mid.get_Image.gameObject:SetActive(false)
    isGet = false
    local setContent = this.main_mid.answer_ScrollPanel.transform:GetChild(0)
    local cRectm1 = setContent.transform:GetComponent(typeof(RectTransform))
    cRectm1.anchoredPosition = Vector2(0, 0)
end
--更新消息界面
function this:updataMainMessagePanel()
    m_mainData = PlatformMessageProxy.getMessageMainData()
    this.main_mid.del_mail_Image.gameObject:SetActive(false)
    if m_mainData.msgType == PlatformGlobalMessageView.MsgType.MailMsg then
        this.main_mid.pagename_Text.text = "奖励消息"
        this.main_mid.answer_title_Text.text = m_mainData.subject
        this.main_mid.answer_text.text = m_mainData.body
        this.main_mid.time_Text.text = os.date("%Y/%m/%d %H:%M", m_mainData.create_time)
        this.main_mid.buttom_Panel.gameObject:SetActive(true)
        this.main_mid.reward_Panel.gameObject:SetActive(true)
        this.main_mid.see_reward_Image:AddEventListener(UIEvent.PointerClick, this.openMidRewardPanel)
        this.main_mid.achieve_Button:AddEventListener(UIEvent.PointerClick, this.achieveMail)
        this.main_mid.achieve_Button.gameObject:SetActive(not m_mainData.award_flag)
        this.main_mid.get_Image.gameObject:SetActive(m_mainData.award_flag)
        this.main_mid.del_mail_Image.gameObject:SetActive(m_mainData.award_flag)
    elseif m_mainData.msgType == PlatformGlobalMessageView.MsgType.UserRedPacketMsg then
        this.main_mid.del_mail_Image.gameObject:SetActive(true)
        this.main_mid.pagename_Text.text = "红包消息"
        this.main_mid.answer_title_Text.text = m_mainData.title2
        this.main_mid.answer_text.text = m_mainData.content
        this.main_mid.time_Text.text = os.date("%Y/%m/%d %H:%M", m_mainData.time)
    elseif m_mainData.msgType == PlatformGlobalMessageView.MsgType.UserMatchMsg then
        this.main_mid.pagename_Text.text = "活动消息"
        this.main_mid.answer_title_Text.text = m_mainData.title2
        this.main_mid.answer_text.text = m_mainData.content
        this.main_mid.time_Text.text = os.date("%Y/%m/%d %H:%M", m_mainData.time)
    elseif m_mainData.msgType == PlatformGlobalMessageView.MsgType.UserEventMsg then
        this.main_mid.pagename_Text.text = "系统通知"
        this.main_mid.answer_title_Text.text = m_mainData.title2
        this.main_mid.answer_text.text = m_mainData.content
        this.main_mid.time_Text.text = os.date("%Y/%m/%d %H:%M", m_mainData.time)
    else
        printError("未知消息类型")
    end
end

--显示奖励列表界面
function this:openMidRewardPanel()
    isReward = not isReward
    if isReward then
        this.main_mid.mid_reward_Panel.gameObject:SetActive(true)
        this.main_mid.closereward_Button:AddEventListener(
            UIEvent.PointerClick,
            function()
                this.main_mid.mid_reward_Panel.gameObject:SetActive(false)
                isReward = false
            end
        )
        this.main_mid.reward_CellRecycleScrollPanel:SetCellData(m_mainData.item_info_list, this.onSetMessageInfo, true)
        this.main_mid.Text.text = "共" .. #m_mainData.item_info_list .. "个附件"
        local cRectm = this.main_mid.bg_Image.rectTransform
        local panelHeight = 210 * (#m_mainData.item_info_list) + 300
        if panelHeight > 1505 then
            panelHeight = 1505
        end
        cRectm.sizeDelta = Vector2(10, panelHeight)
    else
        this.main_mid.mid_reward_Panel.gameObject:SetActive(false)
    end
end

--设置奖励列表
function this.onSetMessageInfo(go, data, index)
    --printError("设置奖励")
    local item = this.main_mid.rewardCelllArr[index + 1]
    if isGet then
        item.noget_reward_Icon.gameObject:SetActive(false)
        itemIcon = item.get_reward_Icon
    else
        item.get_reward_Icon.gameObject:SetActive(false)
        itemIcon = item.noget_reward_Icon
    end
    itemIcon.gameObject:SetActive(true)
 printDebug("+++++++++++++++奖励数据 data = "..table.tostring(data))
    if data.item_type == ProtoEnumCommon.ItemType.ItemType_Coupon then
        itemIcon:ChangeIcon(0)
        item.reward_Text.text = data.item_name.."  x "..data.item_count
    elseif data.item_type == ProtoEnumCommon.ItemType.ItemType_Cash then
        itemIcon:ChangeIcon(1)
        item.reward_Text.text = "红包" .. (data.item_count / 100) .. "元"
    elseif data.item_type == ProtoEnumCommon.ItemType.ItemType_Diamond then
        itemIcon:ChangeIcon(2)
        item.reward_Text.text = "钻石 x" .. data.item_count
    elseif data.item_type == ProtoEnumCommon.ItemType.ItemType_Money then
        itemIcon:ChangeIcon(3)
        item.reward_Text.text = "金币 x" .. data.item_count
    else
        itemIcon:ChangeIcon(4)
        item.reward_Text.text = "优卡 x" .. data.item_count
    end
end

--收取附件
function this.achieveMail()
    this.main_mid.mid_reward_Panel.gameObject:SetActive(false)
    this.main_mid.get_Image.gameObject:SetActive(true)
    m_mainData.award_flag = true
    this.achieveshow()
    local data = {
        mail_type = ProtoEnumCommon.MailType.MailType_Normal,
        mail_id = m_mainData.id
    }
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Mail_Attach, data)
end

function this.achieveshow()
    isGet = true
    this:updataMainMessagePanel()
end
