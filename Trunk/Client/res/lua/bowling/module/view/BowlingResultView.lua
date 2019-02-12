
require "base:enum/UIViewEnum"


BowlingResultView =BaseView:new()
local this=BowlingResultView
this.viewName="BowlingResultView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.BowlingResultView, false)

--设置加载列表
this.loadOrders=
{
	"bowling:bowling_score_result",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
	this.initView()

end
--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
	 this.startShow(msg)
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.exitGame,this.dispose)

end
function this:addEvent()
	this.main_mid.backBtn:AddEventListener(UIEvent.PointerClick,this.reStartGame)
end

function this.reStartGame()
	BowlingGameManager.ResetStartGame()
	ViewManager.close(UIViewEnum.BowlingResultView)
end
--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.exitGame,this.dispose)

end

function this.initView()

	
	--this.showMotion()
end

function this.startShow(score)
	printDebug("BowlingResultView this.startShow(score)="..tostring(score))
	printDebug("this.main_mid.scoreText="..tostring(this.main_mid.scoreText))
	this.main_mid.scoreText.text="<b>"..tostring(score).."</b>"
	this.showDownMotion()
	BowlingScene.playerBowling.isGameStop =false
	AudioManager.playSound("bowling", "bowling_cheer")	
end





function this.showDownMotion()
	local sequ = DOTween.Sequence()
	--sequ:SetDelay(1)
	local mySequenceMove = this.main_mid.bowling_score_result.transform:DOLocalMove(Vector3(-540,2545,0),0.8)
	mySequenceMove:From()
	--mySequenceMove:SetEase
	--mySequenceMove.onComplete = function()

	--end
	sequ:Append(mySequenceMove)
end


--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end


function this.dispose()
	ViewManager.close(UIViewEnum.BowlingResultView)
	ViewManager.destroyView(UIViewEnum.BowlingResultView)
	
end





