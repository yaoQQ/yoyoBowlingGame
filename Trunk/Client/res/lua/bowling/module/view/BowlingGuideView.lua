
require "base:enum/UIViewEnum"
require "bowling:module/data/PlayerScanPos"

BowlingGuideView =BaseView:new()
local this=BowlingGuideView
this.viewName="BowlingGuideView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.BowlingGuideView, false)

--设置加载列表
this.loadOrders=
{
	"bowling:bowling_guide_view",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
	--printDebug("@@@@@@@@@BowlingGuideView this.main_mid  onLoadUIEnd()="..tostring(this.main_mid))
end
--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
	this.initView()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.exitGame,this.dispose)
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.onGameResetStart,this.ReStartGame)
end
function this:addEvent()
	
end
--override 关闭UI回调
function this:onClose()	
	self:removeNotice()

end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.exitGame,this.dispose)
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.onGameResetStart, this.ReStartGame)
end

this.hand3=nil
this.targetCircleBg=nil
this.targetLogBg=nil

this.ballPosCircle= nil --保龄球初始位置范围
function this.initView()
	printDebug("BowlingGuideView --this.initView()")
	
	this.hand3 = this.main_mid.hand3:GetComponent(typeof(RectTransform))
	this.targetCircleBg = this.main_mid.targetCircleBg:GetComponent(typeof(RectTransform))
	--GlobalTimeManager.Instance.timerController:AddTimer("scanMousePosition", 1000, -1, this.isHoldBowling)
	this.showMotion()
	PhysicGameManager.Instance:addUpdateFun(this.Update)
	BowlingScene.playerBowling.isGameStop =true
	PlayerScanPos.initPerson()
	MVPGameModule.sendInitScreen()
end

function this.startShow()
	
end

this.scanTime=0.5
function this.Update()
	if BowlingGameManager.isEditor then
		if BowlingScene.playerBowling.m_fired then
			return
		end
		local addPos = Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.y)
		PlayerScanPos.testAddPersonPos(addPos)
		--BowlingScene.printColor("red","PlayerScanPos.testAddPersonPos() addPos="..tostring(addPos))
	end
	
	this.main_mid.targetCircleBg.transform.localEulerAngles = this.main_mid.targetCircleBg.transform.localEulerAngles +Vector3(0,0,15*Time.deltaTime)

	--printDebug("BowlingGuideView this.Update() addPos="..tostring(addPos))
	--PlayerScanPos.testAddPersonPos(addPos)
	this.scanTime = this.scanTime - Time.deltaTime
	if this.scanTime<0 then
		if this.isScanBowling(this.main_mid.targetCircleBg)~=nil then
			BowlingGameManager.isScanGame =true
			this.scanSuccess()
		end
		this.scanTime=0.5
		this.isHoldBowling()
	end

end
function this.isScanBowling(targetCircleBg)
	local person=PlayerScanPos.switchPerson(targetCircleBg)
	if person then
		
		return person
	end
	return nil
end
function this.isHoldBowling()
	this.ballPosCircle= BowlingBall.getBallScreenPos()
	local distance = Vector3.Distance(Input.mousePosition,this.ballPosCircle)

	if distance<50 then
		this.scanSuccess()
		return true
	end
	return false
end
function this.scanSuccess()
		PhysicGameManager.Instance:removeUpdateFun(this.Update)
		--showFloatTips("识别成功！")
		this.ballPosCircle= BowlingBall.getBallScreenPos()
		BowlingPlayerController.clickPlayerBall(this.ballPosCircle)
		ViewManager.close(UIViewEnum.BowlingGuideView)
		ViewManager.open(UIViewEnum.BowlingStartCountView)
		PlayerScanPos.isSelectPerson=true
		BowlingScene.playerBowling.isGameStop=true
		--MVPGameModule.sendInitScreen()
		BowlingPunishTimeView.selectOver()
		
end
function this.showRoteBg()
	this.main_mid.targetCircleBg.transform.localEulerAngles = this.main_mid.targetCircleBg.transform.localEulerAngles +Vector3(0,0,5*Time.deltaTime)
end

function this.showMotion()
	this.handMoveMotion()
	--this.circleMotion()

end

function this.handMoveMotion()
	local sequ = DOTween.Sequence()
	mySequenceMove = this.main_mid.hand3.transform:DOLocalMove(Vector3(532,-1600,0),2)
	sequ:SetLoops(-1)
	sequ:Append(mySequenceMove)
end
function this.test()
	BowlingScene.printColor("red","this.main_mid.hand3.transform="..tostring(this.main_mid.hand3.transform.localPosition))	
end

function this.ReStartGame()

end

function this.Play()
	BowlingScene.printColor("red","this.main_mid.hand3.transform.position="..tostring(this.main_mid.hand3.transform.position))	
	BowlingScene.printColor("red","this.main_mid.hand3.transform.localposition="..tostring(this.main_mid.hand3.transform.localPosition))	
	BowlingScene.printColor("red","this.hand3="..tostring(this.hand3.rect))	
end

function this.dispose()
	ViewManager.close(UIViewEnum.BowlingGuideView)
	ViewManager.destroyView(UIViewEnum.BowlingGuideView)
end





