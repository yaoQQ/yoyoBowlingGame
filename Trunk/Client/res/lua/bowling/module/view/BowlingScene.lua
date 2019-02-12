require "bowling:module/data/BowlingModelManager"
require "bowling:module/data/BowlingTag"
require "bowling:module/data/BowlingGameManager"
require "bowling:module/view/BowlingFollowCamera"
require "bowling:module/view/BowlingPlayerController"
require "bowling:module/view/BowlingBasePlayer"
require "bowling:module/view/BowlingPinContainer"
require "bowling:module/view/BowlingPinResetAnimation"
require "bowling:module/view/BowlingScanController"
require "bowling:module/data/BowlingEvent"
require "bowling:module/data/BowlingUtils"

BowlingScene = {}
local this = BowlingScene
BowlingScene.bowlingSceneRoot=nil
BowlingScene.playerBowling=nil
BowlingScene.MainCamera=nil
BowlingScene.pitRegion=nil


BowlingScene.dest = nil

local BowlingCameraName ="BowlingCamera" --��Ϸ���������
function BowlingScene.initBowlingScene()
	printDebug("@@@@@@@@@@@@@@initBowlingScene~~")
	Application.targetFrameRate = 60
	BowlingGameManager.init()
	BowlingModelManager.createModel("bowling","gamescene",function(obj)
		local bowlingScenes =GameObject("bowlingScenes")
		BowlingScene.bowlingSceneRoot = bowlingScenes
		local currObj=GameObject.Instantiate(obj)
		currObj.transform.parent = BowlingScene.bowlingSceneRoot.transform
		currObj.transform.localEulerAngles = Vector3.zero
		currObj.transform.position = Vector3.zero
		BowlingScene.initPlayerBowling()

    end)
end
function BowlingScene.initPlayerBowling()
	printDebug("@@@@@@@@@@@@@@initPlayerBowling~~")
	BowlingModelManager.createModel("bowling","ball1",function(obj)
		local BallObj=GameObject.Instantiate(obj)
		BallObj.transform.parent = BowlingScene.bowlingSceneRoot.transform
		BallObj.transform.eulerAngles =Vector3.zero
		--BallObj.transform.position =Vector3.zero
		this.initView()
		this.initBall(BallObj)
		this.initGame()

		NoticeManager.Instance:Dispatch(BowlingEvent.onGameResetStart)
    end)

end

function this.addNotice()
	 NoticeManager.Instance:AddNoticeLister(BowlingEvent.endRoundReset,this.endRoundRest)
end
function this.removeNotice()
	 NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.endRoundReset,this.endRoundRest)
end

--��ʼ�������
function this.initBall(obj)
		
		BowlingBall.initBall(obj)
		BowlingScene.playerBowling = BowlingBall
		BowlingBasePlayer.setBall(BowlingBall)
		BowlingPlayerController.SetBasePlayer(BowlingBasePlayer)
		BowlingScanController.SetBasePlayer(BowlingBasePlayer)
end

function this.addEvent()
	PhysicGameManager.Instance:addLateUpdateFun(this.LateUpdate)
	PhysicGameManager.Instance:addUpdateFun(this.Update)
end

function this.initView()
	local cameraObj = GameObject.Find(BowlingCameraName)
	BowlingScene.MainCamera=cameraObj:GetComponent(typeof(Camera))
	printDebug("@@@@@@@@@@@  BowlingScene.MainCamera="..tostring(BowlingScene.MainCamera))
	BowlingEZReplayManager.init()
	local pinContain = GameObject.Find("Deck")--��������������
	BowlingPinContainer.init(pinContain)--��ʼ��������
	BowlingScene.dest = pinContain	
	
	local pitRegionObj = GameObject.Find("PitRegion")--�J������ײ��
	BowlingBall.initPitRegion(pitRegionObj)
	
	local restPin = GameObject.Find("restPin")--��ƿ���ƿ��������
	local clearPinHand = GameObject.Find("clearPinHand")--��ƿ���ƿ��������
	BowlingPinResetAnimation.init(clearPinHand,restPin)
	
	local scanHandle = GameObject.Find("ScanHandle")--ɨ���ֶ���
	BowlingScanController.setScanBall(scanHandle)
	BowlingScanController.init()
end

function this.endRoundRest()
		--չʾ�������
		BowlingGameManager.GameScript.showScore()

		GlobalTimeManager.Instance.timerController:RemoveTimerByKey("errorRest", 10000, 1, this.endRoundRest)
		--printDebug("<color='blue'>this.endRoundRest()=======+++++++++++++++ this.isCount="..tostring(this.isCount).."</color>")
		--printDebug("BowlingScene ---this.endRoundRest()")
		BowlingEZReplayManager.stop()
		--printDebug("BowlingScene ---this.endRoundRest()")
		
		--�ж���ʲô��� չʾ¼�������
		local scoreResult= BowlingGameManager.scoreResult(false)
		--printDebug("<color='red'>this.endRoundRest() scoreResult="..tostring(scoreResult).."</color>")
		local switch = {
        -- ƽ̨
        [BowlingScoreResult.Strike] = function()
			printDebug("Strike1!")
			BowlingFollowCamera.playRecordLists()
        end,
        [BowlingScoreResult.Spare] = function()
			printDebug("Spare!")
			BowlingFollowCamera.playRecordLists(2)
        end,
        -- ҵ��
        [BowlingScoreResult.normalScore] = function()
			printDebug("normalScore Pins!")
			BowlingGameManager.GameScript.resetPins()
		end,
        [BowlingScoreResult.Gutter] = function()
			printDebug("Gutter Ball!")
			BowlingGameManager.GameScript.resetPins()
        end,
    }
    local fSwitch = switch[scoreResult] --switch func
    if fSwitch then --key exists
        fSwitch() --do func
    end
end

function this.initGame()
	BowlingFollowCamera.init()
	BowlingBall.init()
	BowlingFollowCamera.setCamera(BowlingScene.MainCamera)
	--printDebug("@@@@@@@@@@@  this.initGame() BowlingScene.MainCamera="..tostring(BowlingScene.MainCamera))
	--printDebug("@@@@@@@@@@@  this.initGame() BowlingScene.playerBowling="..tostring(BowlingScene.playerBowling))
	BowlingFollowCamera.setFollowTarget(BowlingScene.playerBowling.target.transform)
	this.addEvent()
	this.addNotice()
	

	ViewManager.open(UIViewEnum.BowlingScore)
	ViewManager.open(UIViewEnum.BowlingPunishTimeView)
	ViewManager.open(UIViewEnum.BowlingGuideView)
	
end

function this.Update()
	--printDebug("BowlingPlayerController update() time=".. Time.time)
	if BowlingScene.playerBowling.isGameStop then
		return
	end
	BowlingFollowCamera.Update()
	BowlingScanController.Update()
	BowlingPlayerController.Update()

end
function this.LateUpdate()
	BowlingFollowCamera.LateUpdate()
end


function BowlingScene.resetCount()
	
	BowlingPunishTimeView.StopCount()
	--�����10�뿨ס��� @@@
	GlobalTimeManager.Instance.timerController:AddTimer("errorRest", 10000, 1, this.endRoundRest)
	--printDebug("=======++++++++BowlingScene.reset()+++++++ this.isCount="..tostring(this.isCount))
end



function BowlingScene.dispose()
	GameObject.Destroy(BowlingScene.bowlingSceneRoot)
	PhysicGameManager.Instance:clear()
	BowlingScene = {}
	BowlingGameManager.dispose()
	BowlingEZReplayManager.dispose()
	NoticeManager.Instance:Dispatch(BowlingEvent.exitGame)
end

function BowlingScene.printColor(colorName,str)
	if moveDebug1 then
		return
	end
	printDebug("moveDebug2<color='"..colorName.."'>"..str.."</color>")
end