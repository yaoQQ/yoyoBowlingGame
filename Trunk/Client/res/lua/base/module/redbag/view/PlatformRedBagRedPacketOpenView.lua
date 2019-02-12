

require "base:enum/UIViewEnum"
require "base:mid/redbag/Mid_platform_redbag_redpacket_open_panel"

--打开红包界面
PlatformRedBagRedPacketOpenView = BaseView:new()
local this = PlatformRedBagRedPacketOpenView
this.viewName = "PlatformRedBagRedPacketOpenView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Help_View, UIViewEnum.Platform_RedBag_RedPacket_Open_View, false)

--设置加载列表
this.loadOrders =
{
    "base:redbag/platform_redbag_redpacket_open_panel",
}

local activeId = 0
local redpacketId = 0
local redpacketType = nil
local isFromChat = false
local openRedPacketCount = 0

--override 加载UI完成回调
function this:onLoadUIEnd(uiName,gameObject)
    
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
    self.main_mid.top_open_image.gameObject:SetActive(false)
    self.main_mid.open_effect:PreLoad()
end

--领取现金

function this:addEvent()
    self.main_mid.close_image:AddEventListener(UIEvent.PointerClick, this.closeEventHandler)
    self.main_mid.mask_image:AddEventListener(UIEvent.PointerClick,  this.closeEventHandler)
    self.main_mid.openredbag_btn:AddEventListener(UIEvent.PointerClick, this.openBtnHandler)
end

function this:removeEvent()
    self.main_mid.close_image:RemoveEventListener(UIEvent.PointerClick, this.closeEventHandler)
    self.main_mid.mask_image:RemoveEventListener(UIEvent.PointerClick, this.closeEventHandler)
    self.main_mid.openredbag_btn:RemoveEventListener(UIEvent.PointerClick, this.openBtnHandler)
end

this.isCanPoint = false
--override 打开UI回调
function this:onShowHandler()
    self:addNotice()
    local data = PlatformRedBagProxy:GetInstance():getOpenRedBagData("Open_Data")
    if table.empty(data) then
        return
    end
    --self.onRedPacketCountHandler()
    self:showPacketView(data)
    self.main_mid.open_effect:PreLoad()
    self:initAnimEvents()
    openRedPacketCount = openRedPacketCount-1
    local anim = self.main_mid.red_packet_animator:GetAnimClip("packet_elastic")
    if anim ~= nil then
        local length = math.ceil(anim.length * 1000)
        GlobalTimeManager.Instance.timerController:AddTimer(
            "OpenBtn", 
            length, 
            1, function ()
            print("按钮可用")
            this.isCanPoint = true
        end)
    else
        this.isCanPoint = true
    end
    this.isLockClose = false
    self:addEvent()
end

function this:addNotice()
	
end

function this:removeNotice()
	
end

function this.openBtnHandler()
    if this.isCanPoint == false then
        print("按钮还不可用~~~")
        return
    end
    this.isCanPoint = false
    PlatformGlobalModule.sendReqReceiveOnLineRedPacket()
    --NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Receive_OnLine_RedPacket)
end

this.isLockClose = false
-- 收到成功回复之后播放效果(包括动画和特效), 同时锁定关闭本界面许可
function this:openSuccessHandler()
    this.isLockClose = true
    printDebug("播放了哦")
    self.main_mid.open_btn_animator:Play("open_btn_open");
    -- 防止异常导致无法关闭界面
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("RedPacketOpenViewExceptionClose")
    GlobalTimeManager.Instance.timerController:AddTimer("RedPacketOpenViewExceptionClose", 10000, 1, function ()
        this.isLockClose = false
    end)
end

--override 关闭UI回调
function this:onClose()
    self.main_mid.red_packet_animator:ResetEvents()
    self.main_mid.open_btn_animator:ResetEvents()

    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("OpenBtn")
    this.isCanPoint = false
    this.isLockClose = false
    self.main_mid.red_packet_animator:Play("packet_idle");
    this:removeNotice()
    self:removeEvent()
    NoticeManager.Instance:Dispatch(
        PlatformGlobalNoticeType.Platform_Rsp_Get_Myself_Online_RedPacket_Info)

end

local isShare = false
function this:initAnimEvents()
    self.main_mid.red_packet_animator:Play("packet_idle");
    self.main_mid.red_packet_animator:Play("packet_elastic");
    self.main_mid.red_packet_animator:AddEndAnimationEvent(
        "packet_elastic",
         function (str)
        self.main_mid.open_btn_animator:Play("open_btn_elastic");
    end)

    self.main_mid.open_btn_animator:AddEndAnimationEvent(
        "open_btn_open", 
        function (str)
        self.main_mid.open_effect:Play()
        self.main_mid.red_packet_animator:Play("packet_open");
    end)

    self.main_mid.red_packet_animator:AddEndAnimationEvent(
        "packet_open",
         function (str)     
        this.isLockClose = false
        GlobalTimeManager.Instance.timerController:RemoveTimerByKey("LBSRedPacketOpenViewClose")
        GlobalTimeManager.Instance.timerController:AddTimer("LBSRedPacketOpenViewClose", 3000, 1, function ()
            PlatformLBSRedPacketOpenView.close(false)
        end)
        this:shareEvent()
    end)
end



function this:showPacketView(data)
    local data = PlatformNewRedBagProxy:GetInstance():getMyselfRedBagData()  
    this.main_mid.name_text.text = "在线红包"
    --downloadUserHead(data.headUrl, this.main_mid.head_circleImage)
    this.main_mid.remain_text.text = "剩余领取次数:"..data.left_can_receive_num
end

function this.closeEventHandler(eventData)
    this.close(false)
end

function this.close(isForce)
    if isForce then
        ViewManager.close(UIViewEnum.Platform_RedBag_RedPacket_Open_View)
    else
        if this.isLockClose then
            return
        end
    end
    ViewManager.close(UIViewEnum.Platform_RedBag_RedPacket_Open_View)
end
local m_endData = {}
function this:shareEvent()
    local data = PlatformRedBagProxy:GetInstance():getRedBagEndData()
	if data == nil then
		return
	end
    m_endData = data.reward_list
   -- printDebug("getRedBagEndData()" .. table.tostring(m_endData))
    for i = 1, #m_endData do
        if m_endData[i].item_type == ProtoEnumCommon.ItemType.ItemType_Cash then
            isShare = true
            break
        end
    end
    if isShare then 
        --红包刷新响应
        NoticeManager.Instance:Dispatch(NoticeType.User_Update_Cash)
        ViewManager.open(UIViewEnum.Platform_RedBag_End_View ,0)
        isShare = false
    else
        --红包刷新响应
        NoticeManager.Instance:Dispatch(NoticeType.User_Update_Cash)
        ViewManager.open(UIViewEnum.Platform_Share_End_View , 0)
    end
    ViewManager.close(UIViewEnum.Platform_RedBag_RedPacket_Open_View)
end

