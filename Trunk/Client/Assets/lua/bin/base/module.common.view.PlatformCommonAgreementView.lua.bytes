

require "base:enum/UIViewEnum"
require "base:mid/common/Mid_platform_common_agreement_panel"

PlatformCommonAgreementView =BaseView:new()
local this=PlatformCommonAgreementView
this.viewName="PlatformCommonAgreementView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Help_View, UIViewEnum.Platform_Common_Agreement_View, true)

--设置加载列表
this.loadOrders=
{
	"base:common/platform_common_agreement_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

function this:addEvent()
	--用户协议界面
	self.main_mid.BtnAgreementBack:AddEventListener(UIEvent.PointerClick, self.onBtnAgreementBack)

end

--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
end

--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end


function this:addNotice()
end

function this:removeNotice()
end

--用户协议界面返回
function this.onBtnAgreementBack(eventData)
	ViewManager.close(UIViewEnum.Platform_Common_Agreement_View)
end