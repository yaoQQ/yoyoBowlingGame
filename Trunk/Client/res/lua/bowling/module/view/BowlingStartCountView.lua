
require "base:enum/UIViewEnum"


BowlingStartCountView =BaseView:new()
local this=BowlingStartCountView
this.viewName="BowlingStartCountView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.BowlingStartCountView, false)

--设置加载列表
this.loadOrders=
{
	"bowling:bowling_start_count",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
	this.initView()
	--printDebug("@@@@@@@@@BowlingGuideView this.main_mid  onLoadUIEnd()="..tostring(this.main_mid))
end
--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
	 this.startShow()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.exitGame,this.dispose)
	--NoticeManager.Instance:AddNoticeLister(BowlingEvent.onGameResetStart,this.ReStartGame)
end
function this:addEvent()
	
end
--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.exitGame,this.dispose)
	--NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.onGameResetStart, this.ReStartGame)
end

function this.initView()
	printDebug("BowlingGuideView --this.initView()")
	this.ballPosCircle= BowlingBall.getBallScreenPos()
	this.numList={this.main_mid.num3,this.main_mid.num2,this.main_mid.num1,this.main_mid.num4}
	
	--this.showMotion()
end

function this.startShow()
	GlobalTimeManager.Instance.timerController:AddTimer("scanMousePosition", 1000, 4, this.begainCount)
	BowlingScene.playerBowling.isGameStop =true
end

this.numList=nil
this.count=0
function this.begainCount()
	this.count=this.count+1
	this.showNum(this.count)
	if this.count>#this.numList then
		this.count =0
	end
end
function this.showRoteBg()
	this.main_mid.targetCircleBg.transform.localEulerAngles = this.main_mid.targetCircleBg.transform.localEulerAngles +Vector3(0,0,5*Time.deltaTime)
end

function this.showNum(num)
--[[	local len=#this.numList
	if num>=1 and num<=len then
		for i=1,len do
			this.numList[i].gameObject:SetActive(false)
		end
		
		
	end--]]
	if num<#this.numList then
		this.showDownMotion(this.numList[num].gameObject,num)
	elseif num==#this.numList then
		this.showScaleMotion(this.numList[num].gameObject,num)
	end
	

end

function this.showDownMotion(obj,num)
	obj.gameObject:SetActive(true)
	local sequ = DOTween.Sequence()
	sequ:SetDelay(1)
	--local mySequenceMove = obj.transform:DOLocalMove(Vector3(540,120,0),1)
	--mySequenceMove:From()
	--mySequenceMove:SetEase
	sequ.onComplete = function()
			obj.gameObject:SetActive(false)
	--[[	local sequ2 = DOTween.Sequence()
		local moveOut = obj.transform:DOLocalMove(Vector3(-150,-326,0),0.8)
		moveOut.onComplete =function()
			obj.gameObject:SetActive(false)
			obj.transform.position=Vector3.zero
			if num== #this.numList then
				this.closeView()
			end
		end
		sequ2:Append(moveOut)--]]
		
	end
	--sequ:Append(mySequenceMove)
end

function this.showScaleMotion(obj,num)
	obj.gameObject:SetActive(true)
	local sequ = DOTween.Sequence()
	local mySequenceMove = obj.transform:DOScale(Vector3(0.1,0.1,0.1),1)
	mySequenceMove:From()
	--mySequenceMove:SetEase
	mySequenceMove.onComplete = function()
			obj.gameObject:SetActive(false)
			obj.transform.position=Vector3.zero
			if num== #this.numList then
				this.closeView()
			end
	end
	sequ:Append(mySequenceMove)
end

function this.showRotaOut(obj,num)
	local sequ = DOTween.Sequence()
	local mySequenceMove = obj.transform:DOScale(Vector3(0.5,0.5,0.5),3)
	this.testEr = 0
	mySequenceMove.onUpdate = function()
		this.testEr = this.testEr +Time.deltaTimed*3
		obj.transform.localEulerAngles =  Vector3(this.testEr,0,0)
		printDebug("update sequ.onUpdate() this.testEr="..tostring(this.testEr))
			printDebug("update sequ.onUpdate() this.testEr+Time.deltaTime*100="..tostring(this.testEr+Time.deltaTime*100))

		obj.transform.position =  Vector3(obj.transform.position.x,obj.transform.position.y+Time.deltaTime*2,0)-- -770
		printDebug("update sequ.onUpdate() obj.transform.localEulerAngles="..tostring(obj.transform.localEulerAngles))
		printDebug("update sequ.onUpdate() obj.transform.position="..tostring(obj.transform.position))
		if obj.transform.position.y>1.1 then
			obj.transform.position = Vector3(0,1.1,0)
		end

	end
	
	mySequenceMove.onComplete =function()
			--obj.gameObject:SetActive(false)
			obj.transform.localScale=Vector3.one
			obj.transform.position=Vector3.zero
			obj.transform.localEulerAngles =Vector3.zero
			if num== #this.numList then
				this.closeView()
			end
		
	end
	sequ:Append(mySequenceMove)
end

--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
	BowlingScene.playerBowling.isGameStop =false
end
function this.closeView()
		GlobalTimeManager.Instance.timerController:RemoveTimerByKey("scanMousePosition", 1000, 4, this.begainCount)
		ViewManager.close(UIViewEnum.BowlingStartCountView)
		this.count=0
end

function this.handMoveMotion()
	local sequ = DOTween.Sequence()
	mySequenceMove = this.main_mid.hand3.transform:DOLocalMove(Vector3(532,-1600,0),2)
	sequ:SetLoops(-1)
	sequ:Append(mySequenceMove)
end


function this.dispose()
	ViewManager.close(UIViewEnum.BowlingStartCountView)
	ViewManager.destroyView(UIViewEnum.BowlingStartCountView)
	
end





