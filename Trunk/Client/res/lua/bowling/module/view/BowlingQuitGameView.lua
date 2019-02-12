
require "base:enum/UIViewEnum"


BowlingQuitGameView =BaseView:new()
local this=BowlingQuitGameView
this.viewName="BowlingQuitGameView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.BowlingQuitGameView, false)

--设置加载列表
this.loadOrders=
{
	"bowling:bowling_quit_game",
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
	self:addNotice()

end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.exitGame,this.dispose)

end
function this:addEvent()
	this.main_mid.okBtn:AddEventListener(UIEvent.PointerClick,this.quitGame)
	this.main_mid.cencleBtn:AddEventListener(UIEvent.PointerClick,this.cancleFun)
end

function this.quitGame()
	--NoticeManager.Instance:Dispatch(BowlingEvent.onGameResetStart)
	BowlingScene.dispose()
	Application.Quit()
end
function this.cancleFun()
	ViewManager.close(UIViewEnum.BowlingQuitGameView)
end
--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.exitGame,this.dispose)

end



--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end


function this.dispose()
	ViewManager.close(UIViewEnum.BowlingQuitGameView)
	ViewManager.destroyView(UIViewEnum.BowlingQuitGameView)
	
end





