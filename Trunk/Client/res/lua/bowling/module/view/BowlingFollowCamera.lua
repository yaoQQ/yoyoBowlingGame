require "bowling:module/data/BowlingModelManager"
require "bowling:enum/BowlingCameraDirection"
require "bowling:module/data/BowlingEZReplayManager"


BowlingFollowCamera = {}
local this = BowlingFollowCamera

this.MainCamera=nil --摄象机
this.target=nil   --跟随对象

this.maxZ =13 -- 摄像机跟随最大Z位置
this.lerpTime =2 -- 当前位置到上一位置平滑时间

this.defaultSpeed =3
this.newPosition =nil-- 当前位置到上一位置平滑时间


function BowlingFollowCamera.init()
	BowlingFollowCamera.addEvent()
end
function BowlingFollowCamera.addEvent()
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.startRecordPlaying,this.stopCameraMove)
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.recordPlaying,this.stopCameraMove)
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.exitGame,this.dispose)
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.overThenMotion,this.overTheMotion)
end
function BowlingFollowCamera.removeEvent()
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.startRecordPlaying,this.stopCameraMove)
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.recordPlaying,this.stopCameraMove)
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.exitGame,this.dispose)
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.overThenMotion,this.overTheMotion)
end


function this.stopCameraMove()
	this.lerpTime=0
	this.isRestGame = false
end

this.recordList=nil
this.isRestDelayTime=1
this.isRestGame=false
this.isPlaying =false
function BowlingFollowCamera.playRecordLists(playNum)--顺序播放录制动画,playNum播放几段
	if not BowlingGameManager.isShowPinMotion then
		this.overTheMotion()
		return
	end
	printDebug("<color='red'>@@@@@@@@@@BowlingFollowCamera.playRecordLists() playNum="..tostring(playNum).."</color>")
	NoticeManager.Instance:Dispatch(BowlingEvent.startRecordPlaying)
	if playNum==2 then
		this.recordList={BowlingCameraDirection.topScene,BowlingCameraDirection.farwordScene,BowlingCameraDirection.playOver}
	else
		this.recordList={BowlingCameraDirection.topScene,BowlingCameraDirection.farwordScene,BowlingCameraDirection.playOver}
	end
	GlobalTimeManager.Instance.timerController:AddTimer("playRecord", 2000, #this.recordList, this.playRecord)
end



function this.overTheMotion()
		if this.isPlaying then
			GlobalTimeManager.Instance.timerController:RemoveTimerByKey("playRecord", 2000, #this.recordList, this.playRecord)
			BowlingEZReplayManager.close()
			this.isPlaying=false
		end
		local scoreResult= BowlingGameManager.GameScript.resetPins()
		BowlingFollowCamera.restCameraSpeed(scoreResult)
end

function this.playRecord()
	printDebug("<color='blue'>this.playRecord() begain this.recordList="..table.tostring(this.recordList).."</color>")
	local currDir = this.recordList[1]
	table.remove(this.recordList,1)
	this.isPlaying =true
	this.playCameraRecord(currDir)

end
function this.playCameraRecord(dir)
	printDebug("<color='blue'>this.playCameraRecord dir="..tostring(dir).."</color>")
	if dir==nil or dir=="" then
		return
	end
	 local switch = {
        -- 平台
        [BowlingCameraDirection.farwordScene] = function()
            this.MainCamera.transform.localPosition =Vector3(0,0.62,14.6)
			this.MainCamera.transform.localEulerAngles =Vector3.zero
			BowlingEZReplayManager.play(-1,true,false,false)
        end,
        [BowlingCameraDirection.backScene] = function()
           this.MainCamera.transform.localPosition =Vector3(0,0.9,22.33)
			this.MainCamera.transform.localEulerAngles =Vector3(0,180,0)
			BowlingEZReplayManager.play(-1,true,false,false)
        end,

        -- 业务
        [BowlingCameraDirection.topScene] = function()
            this.MainCamera.transform.localPosition =Vector3(0,2.9,18.6)
			this.MainCamera.transform.localEulerAngles =Vector3(90,0,0)
			BowlingEZReplayManager.play(-1,true,false,false)
        end,
        [BowlingCameraDirection.leftScene] = function()
            this.MainCamera.transform.localPosition =Vector3(-1.24,1.3,15.42)
			this.MainCamera.transform.localEulerAngles =Vector3(17.2,19.3,0)
			BowlingEZReplayManager.play(-1,true,false,false)
        end,
		[BowlingCameraDirection.playOver] = function()
			this.overTheMotion()
        end,
      
    }
    local fSwitch = switch[dir] --switch func
    if fSwitch then --key exists
        fSwitch() --do func
    else --key not found
       Loger.PrintWarning("BowlingFollowCamera"," 无效BowlingFollowCamera.playCaameraRecord",dir)
    end
end

function BowlingFollowCamera.setFollowTarget(obj)
	this.target = obj
end
function BowlingFollowCamera.setCamera(obj)
	this.MainCamera = obj
	this.MainCamera.fieldOfView = 50
end

this.deltZ = -1.25
function BowlingFollowCamera.Update()
	if this.target==nil then
		printDebug("BowlingFollowCamera.LateUpdate() this.target=nil")
		return
	end

	this.newPosition = this.target.position
--[[	this.newPosition.z = this.newPosition.z-3.32
	this.newPosition.y=1.72--]]

	this.newPosition.z = this.newPosition.z+this.deltZ 
	this.newPosition.y=0.75
--[[	if this.fired then
		this.newPosition.z = this.newPosition.z-2
	end--]]
	if this.newPosition.z>this.maxZ then
		this.newPosition.z = this.maxZ
	end
	if  not BowlingBall.m_fired then --摄像机X只在开始时跟随球水平运动
		--this.newPosition.x=this.newPosition.x
		
		if this.isRestGame  then --延迟返回
			this.isRestDelayTime =this.isRestDelayTime-Time.deltaTime
			if this.isRestDelayTime>0 then
				return
			end
			this.lerpTime = this.lerpTime + 0.005
			if this.lerpTime>this.defaultSpeed then
				this.lerpTime = this.defaultSpeed
				this.isRestGame =false
			end
		else
		
		end
	--[[	--local distance = Vector3.Distance(this.MainCamera.transform.position,this.target.position)
		if distance<1.5 then

		end--]]
		
	else
		this.newPosition.x=this.MainCamera.transform.position.x
	end

	--printDebug("BowlingFollowCamera.Update() this.newPosition="..tostring(this.newPosition))
	--printDebug("BowlingFollowCamera.Update()this.target.position="..tostring(this.target.position))
end

--重置摄像头移动速度  球运动速度 forceVec.z= 1600~6000
function BowlingFollowCamera.setCameraFllowBallSpeed(forceVec)
	printDebug("<color='red'>BowlingFollowCamera.setCameraSpeed() forceVec="..tostring(forceVec).."</color>")
	--this.lerpTime = forceVec.z/2400--*0.65--0.6~2.5 摄像机转化的实际最小和最大移动速度
	this.lerpTime = forceVec.z/2200--*0.65--0.6~2.5 摄像机转化的实际最小和最大移动速度
--[[	printDebug("<color='red'>BowlingFollowCamera.setCameraSpeed() this.lerpTime="..tostring(this.lerpTime).."</color>")
	printDebug("<color='red'>BowlingFollowCamera.setCameraSpeed() Time.smoothDeltaTime*this.lerpTime="..tostring(Time.smoothDeltaTime*this.lerpTime).."</color>")
	printDebug("<color='red'>BowlingFollowCamera.setCameraSpeed() Time.deltaTime*this.lerpTime="..tostring(Time.deltaTime*this.lerpTime).."</color>")
--]]
end
function BowlingFollowCamera.restCameraSpeed(scoreResult)
	--this.lerpTime = this.defaultSpeed
	if scoreResult==BowlingScoreResult.Spare then
		this.lerpTime = this.defaultSpeed
		this.isRestDelayTime = 0.5
		this.isRestGame = true
	else
		this.lerpTime =this.defaultSpeed
		this.isRestDelayTime=0
		this.isRestGame = false
	end
	
	--this.MainCamera.transform.position =Vector3(0,0.84,-1.2)
	this.MainCamera.transform.position =Vector3(0,0.75,12.5)--摄像机由最远距离拉近到球动画
	this.MainCamera.transform.localEulerAngles =Vector3(6,0,0)
	PlayerScanPos.initPerson()
	GlobalTimeManager.Instance.timerController:AddTimer("scanMousePosition", 2000, 1, BowlingFollowCamera.getPosNet)
end

function BowlingFollowCamera.getPosNet()
	
	MVPGameModule.sendInitScreen()
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("scanMousePosition", 2000, 1, BowlingFollowCamera.getPosNet)
end

function BowlingFollowCamera.LateUpdate()
	if this.target==nil then
		printDebug("BowlingFollowCamera.LateUpdate() this.target=nil")
		return
	end

	
	--printDebug("<color='red'> speed="..tostring(speed).."</color>")
--[[	printDebug("<color='red'> this.MainCamera.transform.position="..tostring(this.MainCamera.position).."</color>")
	printDebug("<color='red'> this.newPosition="..tostring(this.newPosition).."</color>")--]]
	this.MainCamera.transform.position = Vector3.Lerp(this.MainCamera.transform.position,this.newPosition, Time.deltaTime*this.lerpTime)
	--printDebug("this.MainCamera.transform.position.z="..tostring(cameraPos.z))
end
function this.dispose()
	BowlingFollowCamera.removeEvent()
	BowlingFollowCamera = {}
end
