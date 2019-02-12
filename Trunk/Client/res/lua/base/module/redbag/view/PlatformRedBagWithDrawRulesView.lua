
require "base:enum/UIViewEnum"
require "base:mid/redbag/Mid_platform_redbag_withdraw_rules_panel"
require "base:module/redbag/data/PlatformNewRedBagProxy"

PlatformRedBagWithDrawRulesView= BaseView:new()
local this = PlatformRedBagWithDrawRulesView
this.viewName = "PlatformRedBagWithDrawRulesView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_RedBag_WithDraw_Rules_View,false)

--设置加载列表
this.loadOrders=
{
	"base:redbag/platform_redbag_withdraw_rules_panel",
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
	printDebug("=====================PlatformRedBagWithDrawRulesView调用完毕======================")

	local go = self:getViewGO()
	if go == nil then return end
	go.transform:SetAsLastSibling()
    
end
---------------------------------------------------------------------------
function this:addEvent()
    self.main_mid.close_Image:AddEventListener(UIEvent.PointerClick,function ()
    	ViewManager.close(UIViewEnum.Platform_RedBag_WithDraw_Rules_View)
    end)
end


