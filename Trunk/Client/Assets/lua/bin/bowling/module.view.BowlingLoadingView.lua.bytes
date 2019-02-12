
require "base:enum/UIViewEnum"


BowlingLoadingView =BaseView:new()
local this=BowlingLoadingView
this.viewName="BowlingLoadingView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.BowlingLoadingView, false)

--设置加载列表
this.loadOrders=
{
	"bowling:bowling_loading_view",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end
--override 打开UI回调
function this:onShowHandler(msg)
	this.init =false
	this:addNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.exitGame,this.dispose)
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.isConnect,this.connectServer)

end

this.init =false
function this.connectServer()
	if this.init then
		return
	end
	BowlingScene.initBowlingScene()
	ViewManager.close(UIViewEnum.BowlingLoadingView)
	this.init = true
end
function this:addEvent()

end

--override 关闭UI回调
function this:onClose()	

end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.exitGame,this.dispose)
end



--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end


function this.dispose()
	ViewManager.close(UIViewEnum.BowlingLoadingView)
	ViewManager.destroyView(UIViewEnum.BowlingLoadingView)
	
end





