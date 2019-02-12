
require "base:enum/UIViewEnum"
require "base:mid/set/Mid_platform_set_about_panel"

PlatformSetAboutView =BaseView:new()
local this=PlatformSetAboutView
this.viewName="PlatformSetAboutView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Set_AboutView, true)

--设置加载列表
this.loadOrders=
{
	"base:set/platform_set_about_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
	self:updataVersion()
end

function this:addEvent()
	self.main_mid.back_Image:AddEventListener(UIEvent.PointerClick,function ()
		ViewManager.close(UIViewEnum.Platform_Set_AboutView)
	end)
	self.main_mid.agreement_Image:AddEventListener(UIEvent.PointerClick,self.onBtnAgreeEvent)

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

function this:updataVersion()
	local version = UtilMethod.GetCurVersion()  
	if version ~= nil and version ~= "" then
		this.main_mid.edition_Text.text = "版本号:"..version
	else
		this.main_mid.edition_Text.text = ""
	end
end

function this:onBtnAgreeEvent()
	ViewManager.open(UIViewEnum.Platform_Common_Agreement_View)
end
