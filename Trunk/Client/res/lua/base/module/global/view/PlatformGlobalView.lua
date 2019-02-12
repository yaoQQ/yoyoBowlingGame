require "base:enum/UIViewEnum"
require "base:mid/global/Mid_platform_global_panel"
require "base:enum/PlatformNoticeType"
require "base:enum/NoticeType"
require "base:module/Shop/data/ShopGlobalConfig"

--主界面公共底图界面
PlatformGlobalView = BaseView:new()
local this = PlatformGlobalView
this.viewName = "PlatformGlobalView"

--设置面板特性（界面层级、界面枚举、是否参与界面堆栈）
this:setViewAttribute(UIViewType.Main_view, UIViewEnum.Platform_Global_View, false)

--设置加载列表
this.loadOrders = {
    "base:global/platform_global_panel"
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    --下面两行默认需要调用

    UITools.SetParentAndAlign(gameObject, self.container)

    --设置UI中间代码
    self.main_mid = Mid_platform_global_panel
    self:BindMonoTable(gameObject, self.main_mid)
    --添加UI事件监听
    self:addEvent()
    this.main_mid.redbag_rp_image.gameObject:SetActive(false)
end

--override 打开UI回调
function this:onShowHandler(msg)
    --设置到最上层
    local go = self:getViewGO()
    if go == nil then
        return
    end
    go.transform:SetAsLastSibling()

    --打开界面时添加UI通知监听
    self:addNotice()

    --根据父界面进行界面初始化
    local parentViewEnum = msg
    self:initViewByParentViewEnum(parentViewEnum)

    this.notifyRedPoint()
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end

function this:initViewByParentViewEnum(parentViewEnum)
    if this.currSelectIcon ~= nil then
        this.currSelectIcon:ChangeIcon(0)
    end
    if this.currSelectTxt ~= nil then
        this.currSelectTxt.color = CSColor(0.588, 0.765, 0.784, 1)
    end

    if parentViewEnum == UIViewEnum.Platform_Global_Personal_View then
        --我的
        this.currSelectIcon = this.main_mid.bottom_personage_Icon
        this.currSelectTxt = this.main_mid.bottom_personage_Text.Txt
    elseif parentViewEnum == UIViewEnum.Platform_Global_RedBag_View then
        --“商铺”
        this.currSelectIcon = this.main_mid.bottom_redbag_Icon
        this.currSelectTxt = this.main_mid.bottom_redbag_Text.Txt
    elseif parentViewEnum == UIViewEnum.Platform_Global_Shop_View then
        --“消息”
        this.currSelectIcon = this.main_mid.bottom_business_Icon
        this.currSelectTxt = this.main_mid.bottom_business_Text.Txt
    elseif parentViewEnum == UIViewEnum.Platform_Global_Message_View then
        --“游戏”
        this.currSelectIcon = this.main_mid.bottom_chat_icon
        this.currSelectTxt = this.main_mid.bottom_chat_text.Txt
    elseif parentViewEnum == UIViewEnum.Platform_Global_Game_View then
        this.currSelectIcon = this.main_mid.bottom_game_Icon
        this.currSelectTxt = this.main_mid.bottom_game_Text.Txt
    end
    this.currSelectIcon:ChangeIcon(1)
    this.currSelectTxt.color = CSColor(0.05, 0.67, 0.886, 1)
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(PlatformFriendType.Notify_Update_Red_Point, this.notifyRedPoint)
    NoticeManager.Instance:AddNoticeLister(
        PlatformGlobalNoticeType.Platform_Rsp_Get_Myself_Online_RedPacket_Info,
        this.onUpdateMyRedPacket
    )
    NoticeManager.Instance:AddNoticeLister(
        PlatformGlobalNoticeType.Platform_Rsp_Get_Friend_Online_RedPacket_Info,
        this.onUpdateFriendRedPacket
    )
    NoticeManager.Instance:AddNoticeLister(
        PlatformGlobalNoticeType.Platform_Get_All_Mail,
        this.onUpdateChatOnlineMsgCount
    )
    NoticeManager.Instance:AddNoticeLister(
        PlatformGlobalNoticeType.Platform_Confirm_UserMsg,
        this.onUpdateChatOnlineMsgCount
    )
    NoticeManager.Instance:AddNoticeLister(
        PlatformGlobalNoticeType.Platform_Update_Mail_Data,
        this.onUpdateChatOnlineMsgCount
    )
    NoticeManager.Instance:AddNoticeLister(
        PlatformFriendType.Receive_Update_Friend_List,
        this.notifyRedPointByUpdateFriendAndChat
    )
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(PlatformFriendType.Notify_Update_Red_Point, this.notifyRedPoint)
    NoticeManager.Instance:RemoveNoticeLister(
        PlatformGlobalNoticeType.Platform_Rsp_Get_Myself_Online_RedPacket_Info,
        this.onUpdateMyRedPacket
    )
    NoticeManager.Instance:RemoveNoticeLister(
        PlatformGlobalNoticeType.Platform_Rsp_Get_Friend_Online_RedPacket_Info,
        this.onUpdateFriendRedPacket
    )

    NoticeManager.Instance:RemoveNoticeLister(
        PlatformGlobalNoticeType.Platform_Get_All_Mail,
        this.onUpdateChatOnlineMsgCount
    )
    NoticeManager.Instance:RemoveNoticeLister(
        PlatformGlobalNoticeType.Platform_Confirm_UserMsg,
        this.onUpdateChatOnlineMsgCount
    )
    NoticeManager.Instance:RemoveNoticeLister(
        PlatformGlobalNoticeType.Platform_Update_Mail_Data,
        this.onUpdateChatOnlineMsgCount
    )
    NoticeManager.Instance:RemoveNoticeLister(
        PlatformFriendType.Receive_Update_Friend_List,
        this.notifyRedPointByUpdateFriendAndChat
    )
end

this.currSelectIcon = nil
this.currSelectViewEnum = nil
this.currSelectTxt = nil

function this:addEvent()
    self.main_mid.btn_personage:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            ViewManager.open(UIViewEnum.Platform_Global_Personal_View)
        end
    )
    --点击了“红包”
    self.main_mid.btn_redbag:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            ViewManager.open(UIViewEnum.Platform_Global_RedBag_View)
        end
    )

    --点击了“商铺”
    self.main_mid.btn_business:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            ViewManager.open(UIViewEnum.Platform_Global_Shop_View)
        end
    )

    --点击了“消息”
    self.main_mid.btn_chat:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            ViewManager.open(UIViewEnum.Platform_Global_Message_View)
        end
    )

    --点击了“游戏”
    self.main_mid.btn_game:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            ViewManager.open(UIViewEnum.Platform_Global_Game_View)
        end
    )
end
function this.notifyRedPointByUpdateFriendAndChat()
    this.onUpdateChatOnlineMsgCount()
    this.notifyRedPoint()
end

function this.notifyRedPoint()
    local applyData = PlatformFriendProxy:GetInstance():getReceiveAddFriendApplyData()
    if applyData ~= nil then
        if applyData.notFriendCount ~= nil and applyData.notFriendCount > 0 then
            if applyData.notFriendCount > 10 then
                if applyData.notFriendCount <= 99 then
                    this.main_mid.personage_rp_Text.text = applyData.notFriendCount
                else
                    this.main_mid.personage_rp_Text.text = "99"
                end
            else
                this.main_mid.personage_rp_Text.text = applyData.notFriendCount
            end
            this.main_mid.personage_rp_Image.gameObject:SetActive(true)
        else
            this.main_mid.personage_rp_Image.gameObject:SetActive(false)
        end
    end
end

function this.onUpdateMyRedPacket()
    this.showRedPacketTip()
end

function this.onUpdateFriendRedPacket()
    this.showRedPacketTip()
end
-- 红包红点通知
function this.showRedPacketTip()
    print("平台大厅红包红点通知")
    local myData = PlatformNewRedBagProxy:GetInstance():getMyselfRedBagData()
    local friendData = PlatformNewRedBagProxy:GetInstance():getStealRedBagData()
    local isShow = false
    if myData == nil and friendData == nil then
        isShow = false
    else
        if myData ~= nil then
            if myData.iscan_receive == 1 then
                isShow = true
            end
        end
        if friendData ~= nil then
            for _, v in pairs(friendData) do
                if v.leftseconds == 0 then
                    isShow = true
                    break
                end
            end
        end
    end
    this.main_mid.redbag_rp_image.gameObject:SetActive(isShow)
end
----------------------------------------外部调用---------------------------------

--进入游戏屏蔽界面
function this:enterGameHide()
    ViewManager.close(UIViewEnum.Platform_Global_View)
    if this.currSelectViewEnum ~= nil then
        ViewManager.close(this.currSelectViewEnum)
    end
end

----------------------------------------按钮点击事件---------------------------

--功能还在开发中，敬请期待！
function this.onExpect(eventData)
    showFloatTips("功能还在开发中，敬请期待！")
end

this.mainChatListData = nil
this.isNotVisitedCount = 0
--聊天红点通知
function this:onUpdateChatOnlineMsgCount()
    if not this.main_mid then
        return
    end
    local mail_count = PlatformMessageProxy.getRedPointCount()

    this.isNotVisitedCount = 0
    this.isNotVisitedCount = mail_count
    if this.isNotVisitedCount > 0 then
        this.main_mid.chat_rp_Image.gameObject:SetActive(true)

        if this.isNotVisitedCount > 99 then
            this.main_mid.chat_rp_Text.text = 99
        else
            this.main_mid.chat_rp_Text.text = this.isNotVisitedCount
        end
    else
        this.main_mid.chat_rp_Image.gameObject:SetActive(false)
    end
end
