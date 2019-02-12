

require "base:enum/UIViewEnum"
require "base:mid/coupon/Mid_platform_coupon_use_know_panel"

PlatformCouponUseKnowView = BaseView:new()
local this = PlatformCouponUseKnowView
this.viewName = "PlatformCouponUseKnowView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Coupon_User_Know_View, true)

--设置加载列表
this.loadOrders=
{
	"base:coupon/platform_coupon_use_know_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

function this:onShowHandler(msg)
	printDebug("=====================Platform_Coupon_User_Know_View调用完毕======================")
	local go = self:getViewGO()
	go.transform:SetAsLastSibling()
	
	self:updateUserKnowNotic()

end

function this:addEvent()
	self.main_mid.back_Image:AddEventListener(UIEvent.PointerClick,function (eventData)
		ViewManager.close(UIViewEnum.Platform_Coupon_User_Know_View)
	end)
end


this.currDetailCouponData = nil
function this:updateUserKnowNotic ()
	this.currDetailCouponData = PlatformCouponProxy.getSelectedCouponData()
	
	if this.currDetailCouponData == nil then
		printDebug("当前所选卡券详细信息为空！")
		return
	end
	self.main_mid.tips_Text.text=this.currDetailCouponData.coupon.notice
end