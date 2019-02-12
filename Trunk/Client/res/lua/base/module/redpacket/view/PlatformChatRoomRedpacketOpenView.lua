require "base:enum/UIViewEnum"

--打开红包界面
PlatformChatRoomRedpacketOpenView = BaseView:new()
local this = PlatformChatRoomRedpacketOpenView
this.viewName = "PlatformChatRoomRedpacketOpenView"

--设置面板特性
this:setViewAttribute(UIViewType.Pop_view, UIViewEnum.Platform_Chat_Room_RedPacket_Open_View, false)

--设置加载列表
this.loadOrders = {
    "base:redpacket/platform_chat_room_redpacket_open_panel"
}

local activeId = 0
local redpacketId = 0
local redpacketType = nil
local isFromChat = false

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    self:initData()
    self:addEvent()
end

function this:initData()
    self.main_mid.top_open_image.gameObject:SetActive(false)
    self.main_mid.has_receive_Image.gameObject:SetActive(false)
    self.main_mid.open_btn_animator.gameObject:SetActive(true)
    self.main_mid.get_receive_redbag_text.gameObject:SetActive(false)
end

function this:addEvent()
    self.main_mid.close_image:AddEventListener(UIEvent.PointerClick, self.close)
    self.main_mid.mask_image:AddEventListener(UIEvent.PointerClick, self.close)
    self.main_mid.openredbag_btn:AddEventListener(UIEvent.PointerClick, self.openBtnHandler)
    self.main_mid.get_receive_redbag_text:AddEventListener(UIEvent.PointerClick, self.opendetailHandler)
end

this.isCanPoint = false
--override 打开UI回调
function this:onShowHandler()
    local data = PlatformRedPacketProxy.GetOpenLBSPacketData("ChatRoom_RedPacket_Open_Data")
    if table.empty(data) then
        return this.onClose()
    end
    
    activeId = data.activeId
    redpacketId = data.redpacketId
    redpacketType = data.redpacketType
    isFromChat = data.isFromChat

    this:showPacketView(data)
    this:initAnimEvents()
end

this.isLockClose = false

function this.openBtnHandler()
    this.isLockClose = true
    local data = PlatformRedPacketProxy.GetOpenLBSPacketData("ChatRoom_RedPacket_Open_Data")
    PlatformRedpacketModule.sendReqRcvActiveCashRedPacket(data.redpacketType, data.activeId, data.redpacketId)
    this.main_mid.open_btn_animator:Play("open_btn_open")
end
function this.opendetailHandler()
    this.isLockClose = false
    PlatformChatRoomRedpacketOpenView.close(false)
    ViewManager.open(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
end
function this:initAnimEvents()
    self.main_mid.red_packet_animator:Play("packet_idle")
    self.main_mid.red_packet_animator:Play("packet_elastic")
    self.main_mid.red_packet_animator:AddEndAnimationEvent(
        "packet_elastic",
        function(str)
            self.main_mid.open_btn_animator:Play("open_btn_elastic")
        end
    )
    self.main_mid.open_btn_animator:AddEndAnimationEvent(
        "open_btn_open",
        function(str)
            self.main_mid.open_effect:Play()
            self.main_mid.red_packet_animator:Play("packet_open")
        end
    )
    self.main_mid.red_packet_animator:AddEndAnimationEvent(
        "packet_open",
        function(str)
            this.isLockClose = false
            PlatformChatRoomRedpacketOpenView.close(false)
            ViewManager.open(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
        end
    )
end

--override 关闭UI回调
function this:onClose()
    self:resetPhotos()
    self.main_mid.red_packet_animator:ResetEvents()
    self.main_mid.open_btn_animator:ResetEvents()
    self.main_mid.red_packet_animator:Play("packet_idle")
end

function this:showPacketView(data)
    printDebug("+++++++++++++++++红包数据"..table.tostring(data))
    downloadMerchantHead(data.headUrl, self.main_mid.head_circleImage)
    self.main_mid.name_text.text = data.name
    self.main_mid.official_image.gameObject:SetActive(data.is_official)
    local x = 0
    local y = self.main_mid.name_text.rectTransform.anchoredPosition.y
    if data.is_official then
        x = x - 50
    end
    local namePos = Vector2(x, y)
    self.main_mid.name_text.rectTransform.anchoredPosition = namePos
    self.main_mid.title_text.text = data.title

    local b = data.getState == PacketGetState.Empty
    self.main_mid.top_open_image.gameObject:SetActive(b)
    self.main_mid.has_receive_Image.gameObject:SetActive(b)
    self.main_mid.open_btn_animator.gameObject:SetActive(not b)
    self.main_mid.openredbag_btn.gameObject:SetActive(not b)
    self.main_mid.get_receive_redbag_text.gameObject:SetActive(b)
end

function this:resetPhotos()
    self.main_mid.head_circleImage:SetPng(nil)
    self.main_mid.name_text.text = ""
    self.main_mid.title_text.text = ""
end

function this.close(isForce)
    if isForce then
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
    else
        if this.isLockClose then
            return
        end
    end
    ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
end
