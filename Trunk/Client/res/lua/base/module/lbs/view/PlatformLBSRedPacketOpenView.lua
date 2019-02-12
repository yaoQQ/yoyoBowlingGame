---
--- Created by Lichongzhi.
--- DateTime: 2018\8\14 0014 14:03
---

require "base:enum/UIViewEnum"
require "base:mid/lbs/Mid_platform_lbs_redpacket_open_panel"

--打开红包界面
PlatformLBSRedPacketOpenView = BaseView:new()
local this = PlatformLBSRedPacketOpenView
this.viewName = "PlatformLBSRedPacketOpenView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_LBS_RedPacket_Open_View, false)

--设置加载列表
this.loadOrders = {
    "base:lbs/platform_lbs_redpacket_open_panel"
}

local activeId = 0
local redpacketId = 0
local redpacketType = nil
local isFromChat = false
local openRedPacketCount = 0

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid = Mid_platform_lbs_redpacket_open_panel
    self:BindMonoTable(gameObject, self.main_mid)
    self.main_mid.top_open_image.gameObject:SetActive(false)
    --self:addEvent()
    self.main_mid.open_effect:PreLoad()
end

--领取现金
local function OpenCashPacketFunction(redpacketType, activeId, redpacketId, isFromChat)
    if isFromChat then
        PlatformRedpacketModule.sendReqRcvActiveCashRedPacket(redpacketType, activeId, redpacketId)
    else
        PlatformRedpacketModule.sendReqRcvMapRedPacket(redpacketType, redpacketId)
    end
end

function this:addEvent()
    self.main_mid.close_image:AddEventListener(UIEvent.PointerClick, this.closeEventHandler)
    self.main_mid.mask_image:AddEventListener(UIEvent.PointerClick, this.closeEventHandler)
    self.main_mid.openredbag_btn:AddEventListener(UIEvent.PointerClick, this.openBtnHandler)
    --temp
    self.main_mid.add_btn.gameObject:SetActive(false)
    --self.main_mid.add_btn:AddEventListener(UIEvent.PointerClick, self.AddTimesClick)
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
    local data = PlatformRedPacketProxy.GetOpenLBSPacketData("RedPacket_Open_Data")
    if table.empty(data) then
        return
    end
    activeId = data.activeId
    redpacketId = data.redpacketId
    redpacketType = data.redpacketType
    isFromChat = data.isFromChat
    self:showPacketView(data)
    self:initAnimEvents()
    local anim = self.main_mid.red_packet_animator:GetAnimClip("packet_elastic")
    if anim ~= nil then
        local length = math.ceil(anim.length * 1000)
        GlobalTimeManager.Instance.timerController:RemoveTimerByKey("LBSRedPacketOpenViewOpenBtn")
        GlobalTimeManager.Instance.timerController:AddTimer(
            "LBSRedPacketOpenViewOpenBtn",
            length,
            1,
            function()
                --print("按钮可用")
                this.isCanPoint = true
            end
        )
    else
        this.isCanPoint = true
    end
    this.isLockClose = false
    self:addEvent()
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_LBSRedPacketCounter, this.onRedPacketCountHandler)
end

function this.onRedPacketCountHandler()
    openRedPacketCount = PlatformUserProxy:GetInstance():getDailyLBSRedPacketCount()
    this.main_mid.remain_text.text = string.format("剩余领取次数:  %s", openRedPacketCount)
end

function this.openBtnHandler()
    if this.isCanPoint == false then
        --print("按钮还不可用~~~")
        return
    end
    this.isCanPoint = false
    OpenCashPacketFunction(redpacketType, activeId, redpacketId, isFromChat)
end

this.isLockClose = false
-- 收到成功回复之后播放效果(包括动画和特效), 同时锁定关闭本界面许可
function this:openSuccessHandler()
    this.isLockClose = true
    self.main_mid.open_btn_animator:Play("open_btn_open")
    -- 防止异常导致无法关闭界面
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("LBSRedPacketOpenViewExceptionClose")
    GlobalTimeManager.Instance.timerController:AddTimer("LBSRedPacketOpenViewExceptionClose", 10000, 1, function ()
        this.isLockClose = false
    end)
end

function this.AddTimesClick()
    showTopTips("功能开发中")
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
            GlobalTimeManager.Instance.timerController:RemoveTimerByKey("LBSRedPacketOpenViewClose")
            GlobalTimeManager.Instance.timerController:AddTimer("LBSRedPacketOpenViewClose", 3000, 1, function ()
                PlatformLBSRedPacketOpenView.close(false)
            end)
            ViewManager.open(UIViewEnum.Platform_LBS_RedPacket_Detail_View)
        end
    )
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
    self:resetPhotos()
    this.isCanPoint = false
    self.main_mid.red_packet_animator:ResetEvents()
    self.main_mid.open_btn_animator:ResetEvents()
    self.main_mid.red_packet_animator:Play("packet_idle")
    self:removeEvent()
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_LBSRedPacketCounter, this.onRedPacketCountHandler)
end

function this:showPacketView(data)
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
    self.main_mid.remain_text.text = string.format("剩余领取次数:  %s", openRedPacketCount)
end

function this:resetPhotos()
    self.main_mid.head_circleImage:SetPng(nil)
    self.main_mid.name_text.text = ""
    self.main_mid.title_text.text = ""
end

--外部调用打开界面
function this.openLBSRedPacketOpenView()
    openRedPacketCount = PlatformUserProxy:GetInstance():getDailyLBSRedPacketCount()
    if openRedPacketCount <= 0 then
        Alert.showAlertMsg(nil,"今日领取红包的次数已到达上限!\n(每日00:00点重置领取次数)"
        ,"确定")
		return
    end
	
	local data = PlatformRedPacketProxy.GetOpenLBSPacketData("RedPacket_Open_Data")
	
	if data.positionLimitType then
		if data.positionLimitType == ProtoEnumCommon.LBSPositionLimitType.LBSPositionLimitType_Near then
			--判断商圈范围
			local dis = MapManager.getDistance(MapManager.userLng, MapManager.userLat, data.lng, data.lat)
			if dis > TableBaseParameter.data[22].parameter then
				Alert.showAlertMsg(nil, "这是附近红包，距离"..TableBaseParameter.data[22].parameter.."米内的用户才能领取", "我知道了", nil)
				return
			end
		elseif data.positionLimitType == ProtoEnumCommon.LBSPositionLimitType.LBSPositionLimitType_City then
			--判断城市
			if MapManager.userCityCode ~= data.cityCode then
				--判断商圈范围
				local dis = MapManager.getDistance(MapManager.userLng, MapManager.userLat, data.lng, data.lat)
				if dis > TableBaseParameter.data[22].parameter then
					Alert.showAlertMsg(nil, "这是城市红包，只有位于该城市的用户才能领取", "我知道了", nil)
					return
				end
			end
		end
	end
	
	ViewManager.open(UIViewEnum.Platform_LBS_RedPacket_Open_View)
end

function this.closeEventHandler(eventData)
    this.close(false)
end

function this.close(isForce)
    if isForce then
        ViewManager.close(UIViewEnum.Platform_LBS_RedPacket_Open_View)
    else
        if this.isLockClose then
            return
        end
    end
    ViewManager.close(UIViewEnum.Platform_LBS_RedPacket_Open_View)
end

--override
--返回键关闭界面
function this:closeByEsc()
	this.close(false)
end