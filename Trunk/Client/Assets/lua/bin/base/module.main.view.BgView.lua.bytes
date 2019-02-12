

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "base:mid/Mid_bg_panel"

BgView = BaseView:new()
local this = BgView
this.viewName = "BgView"

--设置面板特性
this:setViewAttribute(UIViewType.Global_View, UIViewEnum.BgView, false)

--设置加载列表
this.loadOrders=
{
	"base:bg_panel",
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	--.静态方法
	UITools.SetParentAndAlign(gameObject, self.container)
end

--override 打开UI回调
function this:onShowHandler(msg)
	--关闭加载界面
	LoadingBarController.SetProgress(1)
	
	ViewManager.close(UIViewEnum.BgView)
end

--override 关闭UI回调
function this:onClose()
end