

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "base:mid/Mid_statusbar_panel"

StatusbarView = BaseView:new()
local this = StatusbarView
this.viewName = "StatusbarView"

--设置面板特性
this:setViewAttribute(UIViewType.Plot_View, UIViewEnum.StatusbarView, false)

--设置加载列表
this.loadOrders=
{
	"base:statusbar_panel",
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
end

--override 关闭UI回调
function this:onClose()
end