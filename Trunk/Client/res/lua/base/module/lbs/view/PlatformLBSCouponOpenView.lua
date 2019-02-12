---
--- Created by Lichongzhi.
--- DateTime: 2018\8\14 0014 14:03
---

require "base:enum/UIViewEnum"
require "base:mid/lbs/Mid_platform_lbs_coupon_open_panel"

--打开红包界面
PlatformLBSCouponOpenView = BaseView:new()
local this = PlatformLBSCouponOpenView
this.viewName = "PlatformLBSCouponOpenView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_LBS_Coupon_Open_View, false)

--设置加载列表
this.loadOrders = {
    "base:lbs/platform_lbs_coupon_open_panel"
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    self:addEvent()
end

local activeId = 0
local red_packet_id = 0
local red_packet_type = nil
local isFromChat = false
local couponId = 0

--领取优惠卷
local function OpenCouponPacketFunction(redpacketType, activeId, redpacketId, couponId, isFromChat)
    if isFromChat then
        PlatformRedpacketModule.sendReqReceiveActiveCouponRedpacket(redpacketType, activeId, redpacketId, couponId)
    else
        PlatformRedpacketModule.sendReqRcvCoupon(redpacketType, redpacketId, couponId)
    end
end

function this:addEvent()
    self.main_mid.close_image:AddEventListener(UIEvent.PointerClick, self.close)
    self.main_mid.mask_image:AddEventListener(UIEvent.PointerClick, self.close)
    self.main_mid.openredbag_btn:AddEventListener(
        UIEvent.PointerClick,
        function()
            OpenCouponPacketFunction(red_packet_type, activeId, red_packet_id, couponId, isFromChat)
        end
    )
end

--外部调用打开界面
function this.openPlatformLBSCouponOpenView(isCheckDis)
	if isCheckDis then
		--判断商圈范围
		local data = PlatformRedPacketProxy.GetOpenLBSPacketData("Coupon_Open_Data")
		local dis = MapManager.getDistance(MapManager.userLng, MapManager.userLat, data.lng, data.lat)
		if dis > TableBaseParameter.data[22].parameter then
			Alert.showVerifyMsg(nil, "该优惠券可使用的门店离您有点远，确定要领取么？", "取消", nil, "确定", function ()
				ViewManager.open(UIViewEnum.Platform_LBS_Coupon_Open_View)
			end)
			return
		end
	end
	
	ViewManager.open(UIViewEnum.Platform_LBS_Coupon_Open_View)
end

--override 打开UI回调
function this:onShowHandler()
    local data = PlatformRedPacketProxy.GetOpenLBSPacketData("Coupon_Open_Data")
    if table.empty(data) then
        return self.close()
    end
    activeId = data.activeId
    red_packet_id = data.redpacketId
    red_packet_type = data.redpacketType
    isFromChat = data.isFromChat
    couponId = data.coupon_id
    self.main_mid.coupon_bg_effect:Play()
    self:showCouponView(data)
end

function this:showCouponView(data)
    print("Coupon_Open_Data = " .. table.tostring(data))
    self.main_mid.title_text.text = data.title
    self.main_mid.name_text.text = data.name
    self.main_mid.coupon_shop_name_text.text = data.name
    self.main_mid.coupon_name_text.text = data.couponName
    data.packetStyle = data.packetStyle or 1
    self.main_mid.coupon_bg_icon:ChangeIcon(data.packetStyle)
    downloadMerchantHead(data.headUrl, self.main_mid.coupon_image)
    downloadMerchantHead(data.headUrl, self.main_mid.head_circleImage)
    if data.packetStyle > 0 then
        self.main_mid.coupon_diy_image.gameObject:SetActive(false)
    else
        self.main_mid.coupon_diy_image.gameObject:SetActive(true)
        downloadImage(data.iconUrl, self.main_mid.coupon_diy_image)
    end

end

-- 收到成功回复之后播放效果(包括动画和特效)
function this:openSuccessHandler()
    this.close()
    ViewManager.open(UIViewEnum.Platform_LBS_Coupon_Detail_View)
end

function this.close()
    ViewManager.close(UIViewEnum.Platform_LBS_Coupon_Open_View)
end
