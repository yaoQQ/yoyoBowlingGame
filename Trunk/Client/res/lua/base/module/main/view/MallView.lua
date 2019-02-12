

require "base:enum/UIViewEnum"
require "base:mid/Mid_mall_panel"

MallView = BaseView:new()
local this = MallView
this.viewName = "MallView"

--设置面板特性
this:setViewAttribute(UIViewType.Pop_view, UIViewEnum.MallView, false)

--设置加载列表
this.loadOrders=
{
	"base:mall_panel",
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	--.静态方法
	UITools.SetParentAndAlign(gameObject, self.container)
	
	self.main_mid.btn_close:AddEventListener(UIEvent.PointerClick, self.onBtnCloseClick)
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

function this.onBtnCloseClick(eventData)
	ViewManager.close(UIViewEnum.MallView)
end