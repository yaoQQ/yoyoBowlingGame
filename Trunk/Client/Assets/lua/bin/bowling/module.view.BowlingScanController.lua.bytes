require "bowling:module/data/BowlingGameManager"
require "bowling:module/data/BowlingPlayerScore"
require "bowling:enum/BowlingCameraDirection"

BowlingScanController = {}
local this = BowlingScanController

this.BowlingBasePlayer=nil

BowlingScanController.ballLayer=LayerMask.NameToLayer("Game_1")


function BowlingScanController.SetBasePlayer(obj)
	this.BowlingBasePlayer=obj
end 

this.testFrame=1
this.testAddZPos=true
this.scanHandle=nil

function BowlingScanController.setScanBall(obj)
	this.scanHandle = obj
end
function BowlingScanController.Update()

	this.UpdatePlayerBall()
	if Input.GetKeyUp("p") then

		MVPGameModule.sendInitScreen()
	end
	if Input.GetKeyUp("o") then

		MVPGameModule.SendStopPos()
	end
	if Input.GetKeyUp("c") then

		MVPGameModule.sendChoosePerson()
	end
	if Input.GetKeyUp("w") then
		this.testAddZPos = not this.testAddZPos
		showFloatTips("Input.GetKeyUp(w) this.testAddZPos="..tostring(this.testAddZPos))
	end

	if Input.GetKeyUp("a") then

		--Time.timeScale =1
		--showFloatTips("Time.timeScale="..tostring(Time.timeScale))
		BowlingPinContainer.TestAllPinDownNum(7)
	end
	if Input.GetKeyUp("s") then

		 Time.timeScale =2
		showFloatTips("Time.timeScale="..tostring(Time.timeScale))
	end
	if Input.GetKeyUp("d") then
		BowlingGameManager.isEditor=true
	end
end 
function BowlingScanController.init()
	this.addNotice()
	this.angelScreen = 0
	--GlobalTimeManager.Instance.timerController:AddTimer("addTestPosTime", 200, -1, this.testAdd)
	--GlobalTimeManager.Instance.timerController:AddTimer("checkPefectPosList", 1000, -1, this.getPefectPosList)
end
function this.addNotice()
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.updateScreenPos,this.updateHandScreenPos)
end
function this.removeNotice()
	 NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.updateScreenPos, this.updateHandScreenPos)
end


function this.isStartControl()
	if BowlingScene.playerBowling.isGameStop then
		return false
	end
	if BowlingScene.playerBowling.m_fired then
		return false
	end
	return true
end
function this.updateHandScreenPos(notcie, rsp)
	 local req = rsp:GetObj()
	if this.isStartControl()==false then
		return
	end

	if req then
		local leftScreenPos = req[1]
		local righScreenPos = req[2]
		local selectPos =nil
		if PlayerScanPos.isRightHandle then
			selectPos = righScreenPos
		else
			selectPos = leftScreenPos
		end
		local ball = BowlingScene.playerBowling.target.transform
		local curPosition =Vector3.zero
		local wordPos = BowlingBall.getBallScreenPosToWorldPos(selectPos)
		curPosition.x = wordPos.x
		curPosition.z = ball.position.z
		curPosition.y = ball.position.y
		ball.position =curPosition
		
		--BowlingScene.playerBowling.target.transform.position = Vector3.Lerp(BowlingScene.playerBowling.target.transform.position,this.targetBallMoveX, Time.deltaTime*2)
		if ball.position.x>BowlingBall.m_dragLimitX then
			ball.position =Vector3(BowlingBall.m_dragLimitX,ball.position.y,ball.position.z)
		elseif ball.position.x<-BowlingBall.m_dragLimitX then
			ball.position=Vector3(-BowlingBall.m_dragLimitX,ball.position.y,ball.position.z)
		end

	end
end


function this.testAdd()
		local addPos = Vector3.zero
		if this.testAddZPos then
			addPos = Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.y)
		else
			addPos = Vector3(Input.mousePosition.x,Input.mousePosition.y,0)
		end
		--showFloatTips("PlayerScanPos.testAddPersonPos(addPos) addPos="..tostring(addPos))
		PlayerScanPos.testAddPersonPos(addPos)
		
end




this.targetBallMoveX =nil

function this.updateBallMoveX()
	if this.targetBallMoveX ==nil then
		return
	end
	--local moveX = 
	BowlingScene.playerBowling.target.transform.position = Vector3.Lerp(BowlingScene.playerBowling.target.transform.position,this.targetBallMoveX, Time.deltaTime*2)
	if BowlingScene.playerBowling.target.transform.position.x>BowlingBall.m_dragLimitX then
		BowlingScene.playerBowling.target.transform.position =Vector3(BowlingBall.m_dragLimitX,BowlingScene.playerBowling.target.transform.position.y,BowlingScene.playerBowling.target.transform.position.z)
	elseif BowlingScene.playerBowling.target.transform.position.x<-BowlingBall.m_dragLimitX then
		BowlingScene.playerBowling.target.transform.position =Vector3(-BowlingBall.m_dragLimitX,BowlingScene.playerBowling.target.transform.position.y,BowlingScene.playerBowling.target.transform.position.z)
	end
end

function this.UpdatePlayerBall()
		
	if BowlingGameManager.isEditor then
		if BowlingScene.playerBowling.m_fired then
			return
		end
		local addPos = Vector3.zero
		if this.testAddZPos then
			addPos = Vector3(Input.mousePosition.x,Input.mousePosition.y,Input.mousePosition.y)
		else
			addPos = Input.mousePosition
		end
		PlayerScanPos.testAddPersonPos(addPos)
	end
	
	PlayerScanPos.getSamePosList()
	if this.isStartControl()==false then
		return
	end

	local currPerfectPosList=PlayerScanPos.getPerfectPosList()
	
	--this.updateBallMoveX()
	if currPerfectPosList==nil or #currPerfectPosList<=0 then
		return
	end
	
	--this.farward ={isLeft =0,isRight =1,isUP =2,isDown =3,none=-1}
	printDebug("<color='red'>@@@@@@@@@ currPerfectPosList="..tostring(currPerfectPosList).."</color>")
	local farward = currPerfectPosList.farward
	local currPos =currPerfectPosList[1]   --当前屏幕坐标

	
	local str = PlayerScanPos.returnStr(farward)
	--showTopTips(" farward="..tostring(str).." 点数量="..tostring(#this.getCurrPerfectPosList))
	--currPos=nil
	if currPos==nil then
		return
	end


	if BowlingScene.playerBowling.m_fired==false  then
			local pos1 =currPerfectPosList[1] 
			local pos2 =currPerfectPosList[#currPerfectPosList] 
			if pos1~=nil and pos2~=nil  then
				--local currPosToBallXDis = pos2.x -pos1.x--目标点与球x距离
			--	local ballToCurrPosDis = Vector3.Distance(Vector3(pos1.x,pos1.y,0),Vector3(pos2.x,pos2.y,0))
				local currPosToBallXDis = pos2.z -pos1.z--目标点与球x距离
				local ballToCurrPosDis = Vector3.Distance(Vector3(pos1.x,pos1.y,0),Vector3(pos2.x,pos2.y,0))
				if  ballToCurrPosDis<=1 then
					
				else
					local angel = Mathf.Asin(currPosToBallXDis/ballToCurrPosDis)*Mathf.Rad2Deg  -- 轨迹圆弧角度
					angel=angel/50
					this.angelScreen = angel
					printDebug("<color='red'>@@@@@@@@@ this.angelScreen="..tostring(this.angelScreen).."</color>")
					BowlingScene.playerBowling.target.transform.localEulerAngles =  Vector3(0,this.angelScreen,0)
				end
				BowlingScene.playerBowling.target.transform.localEulerAngles =  Vector3(0,this.angelScreen,0)
			end

			if  farward==2 then ---##@@@@@注意isMouseUp 判断
				
				local maxNum=PlayerScanPos.getMaxPosZIndex(currPerfectPosList)[2]
				local minNum=PlayerScanPos.getMinPosZIndex(currPerfectPosList)[2]

				local distanceZ =Mathf.Abs(pos2.z -pos1.z)
				this.MouseUpFireBall(distanceZ)
			end
			if  farward==3 then ---##@@@@@注意isMouseUp 判断 向下
				
			end
			currPerfectPosList=nil

	end

end




function this.MouseUpFireBall(speed)
		printDebug("<color='red'>*******@@@@@@@@ this.MouseUpFireBall() hand speed="..tostring(speed).."</color>")
		local power = this.SpeedSwitchToPower(speed)
		printDebug("<color='red'>*******@@@@@@@@ this.MouseUpFireBall() hand  power="..tostring(power).."</color>")
		this.BowlingBasePlayer.fireBall(power)

		
		MVPGameModule.SendStopPos()
		PlayerScanPos.initPerson()
end

function this.SpeedSwitchToPower(speed)
	local power =5
	if speed then
		power=speed/200
	end
	printDebug("<color='red'>*******@@@@@@@@ cout 临时 力大小power="..tostring(power).."</color>")
	printDebug("<color='red'>*******@@@@@@@@  speed="..tostring(speed).."</color>")
	if power<2.5 then
		power=2.5
	end
	if power>6 then
		power=6
	end

	return power
end

