
require "base:enum/UIViewEnum"


BowlingPunishTimeView =BaseView:new()
local this=BowlingPunishTimeView
this.viewName="BowlingPunishTimeView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.BowlingPunishTimeView, false)

--设置加载列表
this.loadOrders=
{
	"bowling:bowling_punishtime",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
	this.initView()
	--printDebug("@@@@@@@@@BowlingPunishTimeView this.main_mid  onLoadUIEnd()="..tostring(this.main_mid))
end
--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.exitGame,this.dispose)
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.onGameResetStart,this.ReStartGame)
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.updateScreenPos,this.updateHandScreenPos)

end
function this:addEvent()
	--[[this.main_mid.back_btn:AddEventListener(UIEvent.PointerClick, this.gameOver)--]]
end
--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.exitGame,this.dispose)
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.onGameResetStart, this.ReStartGame)
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.updateScreenPos,this.updateHandScreenPos)
end

function this.updateHandScreenPos(notcie, rsp)
	 local req = rsp:GetObj()

	if req then
		local leftScreenPos = Vector3(req[1].x,req[1].y,0)
		local righScreenPos = Vector3(req[2].x,req[2].y,0)

		local leftWorldPos= BowlingUtils.UguiToScreen(this.main_mid.handleft,leftScreenPos)
		local rightWorldPos= BowlingUtils.UguiToScreen(this.main_mid.handright,righScreenPos)
		
		local isNotLimit = not PlayerScanPos.isSelectPerson --默认true

		local isShowRight = true
		local isShowLeft = true
		isShowRight = isNotLimit or PlayerScanPos.isRightHandle
		isShowLeft = isNotLimit or (not PlayerScanPos.isRightHandle)

		if leftWorldPos and isShowLeft then
	
			BowlingPunishTimeView.setLeftUpdatePos(req[1])
			this.main_mid.handleft.transform.position = leftWorldPos
		end
		if rightWorldPos and isShowRight then
			BowlingPunishTimeView.setRightUpdatePos(req[2])
			this.main_mid.handright.transform.position = rightWorldPos
		end
	end
end





function this.initView()
	printDebug("BowlingPunishTimeView --this.initView()")
	this.main_mid.timeCount.text =""

	this.currTime = this.RoundTotalTime
	BowlingPunishTimeView.setScreenUpdatePos("屏幕大小 width="..tostring(CS.UnityEngine.Screen.width).." height="..tostring(CS.UnityEngine.Screen.height)) 
	BowlingPunishTimeView.startCount()
end

function this.setHandScreenPos(screenPos)
	BowlingUtils.UguiToScreen(screenPos)
end

this.isPunish =false
this.RoundTotalTime = 20
this.currTime =0

--玩家超时后的己方下一球，操作时间减半为10秒，继续超时，减为5秒并持续到玩家重新操作或球局结束（20》10》5=5=5=5）
function BowlingPunishTimeView.startCount()
	--printDebug("@2@BowlingPunishTimeView.startCount()")
	if this.main_mid==nil then
		return
	end
	if this.isPunish then
		this.RoundTotalTime=Mathf.Ceil(this.RoundTotalTime/2)
		this.RoundTotalTime = this.RoundTotalTime<5 and 5 or this.RoundTotalTime
		--printDebug("this.isPunish="..tostring(this.isPunish).." this.RoundTotalTime ="..tostring(this.RoundTotalTime ))
	else
		this.RoundTotalTime = 20
	end
	this.currTime = this.RoundTotalTime
	this.main_mid.timeCount.text =tostring(this.currTime)
	--printDebug("BowlingPunishTimeView.startCount() this.currTime="..tostring(this.currTime))
	--GlobalTimeManager.Instance.timerController:AddTimer("punishTime", 1000, this.currTime, this.reFreshTime)
end

function BowlingPunishTimeView.selectOver()
		if PlayerScanPos.isRightHandle then
			this.main_mid.handleft.gameObject:SetActive(false)
		else
			this.main_mid.handright.gameObject:SetActive(false)
		end
end
function BowlingPunishTimeView.setLeftUpdatePos(pos) 
		if this.main_mid~=nil then
			this.main_mid.leftPosText.text="leftPos="..tostring(pos)
		end
end
function BowlingPunishTimeView.setRightUpdatePos(pos) 
	if this.main_mid~=nil and pos then
		this.main_mid.rightPosText.text="rightPos="..tostring(pos)
	end
end
function BowlingPunishTimeView.setScreenUpdatePos(pos) 
	if this.main_mid~=nil and pos then
		this.main_mid.screenPosText.text="info:"..tostring(pos)
	end
end

--玩家操作停止倒计时
function BowlingPunishTimeView.StopCount()
	this.isPunish = false
	this.main_mid.timeCount.text =""
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("punishTime", 1000, this.currTime, this.reFreshTime)
end

function this.reFreshTime()
	this.currTime = this.currTime -1
	this.main_mid.timeCount.text =tostring(this.currTime)
	if this.currTime<=0 then --玩家没操作 ，惩罚
		this.isPunish =true
		printDebug("this.currTime<=0  Dispatch(BowlingEvent.endRoundReset)")
		NoticeManager.Instance:Dispatch(BowlingEvent.endRoundReset)
		this.currTime =this.RoundTotalTime
	end
end

function this.ReStartGame()
	this.isPunish = false
	this.RoundTotalTime = 20
	this.currTime =0
	BowlingPunishTimeView.startCount()
end

function this.dispose()
	ViewManager.close(UIViewEnum.BowlingPunishTimeView)
	ViewManager.destroyView(UIViewEnum.BowlingPunishTimeView)
end





