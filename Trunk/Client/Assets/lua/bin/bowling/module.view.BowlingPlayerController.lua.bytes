require "bowling:module/data/BowlingGameManager"
require "bowling:module/data/BowlingPlayerScore"
require "bowling:enum/BowlingCameraDirection"

BowlingPlayerController = {}
local this = BowlingPlayerController

this.BowlingBasePlayer=nil

BowlingPlayerController.ballLayer=LayerMask.NameToLayer("Game_1")


function BowlingPlayerController.SetBasePlayer(obj)
	this.BowlingBasePlayer=obj
end 

this.testFrame=1
function BowlingPlayerController.Update()
	this.UpdatePlayerBall()
	if Input.GetKeyUp("p") then
		--BowlingGameManager.RestGame()
		--ViewManager.open(UIViewEnum.BowlingScore)
		--BowlingPinContainer.TestAllPinDown()
		--BowlingEZReplayManager.record()
		--BowlingFollowCamera.playRecordLists()
		--BowlingPinContainer.TestAllPinDown()
		-- AudioManager.playSound("bowling", "bowling_pins_down_lot")
		--BowlingFollowCamera.stopCameraMove()
		--BowlingGuideView.Play()
	--	MVPGameModule.sendInitScreen()
	end
	if Input.GetKeyUp("o") then
		--BowlingGameManager.isShowPinMotion=true
	--	BowlingPinContainer.TestAllPinDownNum(7)
		--BowlingGameManager.onGameOver()
		--ViewManager.open(UIViewEnum.BowlingLoadingView)
	--	BowlingFollowCamera.playCaameraRecord(BowlingCameraDirection.topScene)
		--MVPGameModule.SendStopPos()
	end
	if Input.GetKeyUp("c") then
		--BowlingGameManager.isShowPinMotion=false
		--showFloatTips("BowlingGameManager.isShowPinMotion="..tostring(BowlingGameManager.isShowPinMotion))
		--BowlingPinContainer.getAllUpPinsMove(true)
		--ViewManager.close(UIViewEnum.BowlingLoadingView)
		--MVPGameModule.sendChoosePerson()
	end
	if Input.GetKeyUp("w") then
		--ViewManager.open(UIViewEnum.BowlingGuideView)
		--BowlingPinContainer.TestAllPinDownNum(3)
		--BowlingPinContainer.getAllUpPinsMove(false)
		--NoticeManager.Instance:Dispatch(BowlingEvent.overThenMotion)
		--showFloatTips("跳过动画")
		--printDebug("获取的轨迹点="..table.tostring(PlayerScanPos.personList))
	end

	if Input.GetKeyUp("a") then
		--BowlingEZReplayManager.close()
		--BowlingBall.addHoriBall(Vector3(-200,0,0))
		--BowlingPinContainer.removePin()
		--Time.timeScale =1
		--showFloatTips("Time.timeScale="..tostring(Time.timeScale))
	--	printDebug(" testManage="..tostring(testManage))
		--testManage:showStage()
		--testManage:addScanPos(Input.mousePosition)
		--testManage.transform.parent.gameObject:SetActive(false)
	end
	if Input.GetKeyUp("s") then
		--BowlingEZReplayManager.close()
		--BowlingPinContainer.TestAllPinDownNum(1)
		--BowlingPinContainer.getAllUpPinsMove(false)
		-- Time.timeScale =2
		--showFloatTips("Time.timeScale="..tostring(Time.timeScale))
	end
	if Input.GetKeyUp("d") then
		--NoticeManager.Instance:Dispatch(BowlingEvent.isConnect)
		--BowlingFollowCamera.playCameraRecord(BowlingCameraDirection.topScene)
		--Time.timeScale =3
		--showFloatTips("Time.timeScale="..tostring(Time.timeScale))
	end
end 



function this.GetScreenWorldPint()
	local depth =0
	local touchPoint =  BowlingScene.MainCamera:ScreenToWorldPoint(Input.mousePosition + Vector3(0, 0, depth))
	--local leftX =0
	--local rightX=0
	--touchPoint.x = Mathf.Clamp(touchPoint.x,)
	return touchPoint
end

---射线获取对象
function BowlingPlayerController.Raycast(ray,rayDistance,layer)
		local hit=nil
		if Physics.Raycast(ray,rayDistance,layer) then
			local hitArray =Physics.RaycastAll(ray,rayDistance)
			if hitArray then
				--printDebug("<color='red'> BowlingPlayerController.Raycast() hitArray="..tostring(hitArray))
			--	printDebug("<color='red'> BowlingPlayerController.Raycast() hitArray.Length="..tostring(hitArray.Length))
			
			end
			for i=0,hitArray.Length-1 do
				
				if hitArray[i].transform.gameObject.layer==layer then
					hit = hitArray[i]
					return true
				end
			end
			
		end
		return false
end

function this.clickPlayerBall()
		if BowlingScene.playerBowling.m_fired==false then
			BowlingScene.playerBowling.m_mouseDown =true
			BowlingScene.playerBowling.m_screenPoint =BowlingBall.getBallScreenPos()
			BowlingScene.playerBowling.m_prevLoc = Vector3(Input.mousePosition.x, Input.mousePosition.y, BowlingScene.playerBowling.m_screenPoint.z)
			BowlingScene.playerBowling.m_offset =  BowlingScene.playerBowling.target.transform.position - BowlingScene.MainCamera:ScreenToWorldPoint(Vector3(Input.mousePosition.x, Input.mousePosition.y, BowlingScene.playerBowling.m_screenPoint.z))
			
		--	printDebug("###### BowlingScene.playerBowling.m_mouseDown="..tostring(BowlingScene.playerBowling.m_mouseDown))
		--	printDebug("###### BowlingScene.playerBowling.m_prevLoc="..tostring(BowlingScene.playerBowling.m_prevLoc))
		--	printDebug("###### BowlingScene.playerBowling.m_screenPoint="..tostring(BowlingScene.playerBowling.m_screenPoint))
		--	printDebug("###### BowlingScene.playerBowling.m_offset="..tostring(BowlingScene.playerBowling.m_offset))
		end
end


function this.UpdatePlayerBall()
	if BowlingScene.playerBowling.isGameStop then
		return
	end
	if BowlingScene.playerBowling.m_fired then
		return
	end

	if  Input.GetMouseButtonDown() then
		local ray=BowlingScene.MainCamera:ScreenPointToRay(Input.mousePosition)
		local rayDistance = 1000
		local hit=BowlingPlayerController.Raycast(ray,rayDistance,BowlingPlayerController.ballLayer)
		if hit then
			this.clickPlayerBall()
		end
		
	end

	if BowlingScene.playerBowling.m_fired==false and BowlingScene.playerBowling.m_mouseDown then
		local curScreenPoint = Vector3(Input.mousePosition.x, Input.mousePosition.y, BowlingScene.playerBowling.m_screenPoint.z)
		
		local UpOrDown = BowlingScene.playerBowling.m_prevLoc.y - curScreenPoint.y
		local d1 = Mathf.Abs(BowlingScene.playerBowling.m_screenPoint.y - curScreenPoint.y)
		--printDebug("=============================")
		--local ballScreenPoint = 
		--printDebug("@@@@@@@@@ drag m_prevLoc="..tostring(BowlingScene.playerBowling.m_prevLoc))
		
		--printDebug("@@@@@@@@@ drag curScreenPoint="..tostring(curScreenPoint))
		--printDebug("@@@@@@@@@ drag d1="..tostring(d1))
		--printDebug("@@@@@@@@@ drag UpOrDown="..tostring(UpOrDown))
		--printDebug("@@@@@@@@@ drag BowlingScene.playerBowling.m_offset="..tostring(BowlingScene.playerBowling.m_offset))
		if d1< BowlingScene.playerBowling.m_dragThreshold then --是左右水平移动
			local curPosition = BowlingScene.MainCamera:ScreenToWorldPoint(curScreenPoint) + BowlingScene.playerBowling.m_offset
			local ball = BowlingScene.playerBowling.target.transform

			curPosition.z = ball.position.z
			curPosition.y = ball.position.y

			local currX = tonumber(string.format("%0.2f", curPosition.x))
			if currX>BowlingBall.m_dragLimitX then
				curPosition.x = BowlingBall.m_dragLimitX
			elseif currX<-BowlingBall.m_dragLimitX then
				curPosition.x = -BowlingBall.m_dragLimitX
			end
			ball.position = curPosition
			this.moveInitPos=curScreenPoint -- 开始移动计算速度初始位置
			this.moveInitScreenPos =curScreenPoint -- 开始移动初始位置
			--printDebug("after @@@@@@@@@ drag curPosition.x="..tostring(curPosition.x))
			--printDebug("<color='red'>@@@@@@@@@ this.this.moveInitScreenPos="..tostring(this.moveInitScreenPos).."</color>")
			--this.MouseUpFireBall()
		elseif UpOrDown<0 then  --是向上选择方向s
			this.angelScreen = this.countBallfowardAngel(curScreenPoint)
			--printDebug("@@@this.rotateBall()  this.angelScreen="..tostring(this.angelScreen))
			BowlingScene.playerBowling.target.transform.localEulerAngles =  Vector3(0,this.angelScreen,0)
			
			--printDebug("<color='red'>@@@@@@@@@ this.angelScreen="..tostring(this.angelScreen).."</color>")
			--printDebug("<color='yellow'>@@@@@@@@@ this.moveInitScreenPos="..tostring(this.moveInitScreenPos).."</color>")
			--printDebug("<color='yellow'>@@@@@@@@@ curScreenPoint2="..tostring(curScreenPoint).."</color>")
			--printDebug("<color='yellow'>@@@@@@@@@ BowlingScene.playerBowling.target.transform.localRotation="..tostring(BowlingScene.playerBowling.target.transform.localRotation).."</color>")
			--printDebug("<color='yellow'>@@@@@@@@@ BowlingScene.playerBowling.target.transform.localEulerAngles="..tostring(BowlingScene.playerBowling.target.transform.localEulerAngles).."</color>")

			
			this.countMoveSpeed(curScreenPoint)
			if  Input.GetMouseButtonUp() then
				local distance = Vector3.Distance(curScreenPoint,this.moveInitPos)
				local speed = 0
				if this.moveUpTimeCount~=0 then
					speed = distance/this.moveUpTimeCount
				end
				this.MouseUpFireBall(speed)
			end
			
			
		else
			
		end
		
	end
	if  Input.GetMouseButtonUp() then
		--printDebug("PlayerController（） Input.GetMouseButtonUp()")
		BowlingScene.playerBowling.m_mouseDown =false
		--this.clearPos()
	---	if this.BowlingBasePlayer then
			--this.BowlingBasePlayer.fireBall()  -- 投球
		--end
	end
end

this.moveUpTimeCount=0  --计算时间
this.moveInitScreenPos =Vector3.zero ----有效滑动屏幕初始点
this.moveInitPos =Vector3.zero ----有效滑动屏幕初始点

this.moveUpTime =0.2 --判定为有效移动时间
this.moveUpTimeFrame =0.2 --判定是否为移动状态
this.moveUpTimeTotal =2 --判定是否为移动状态
this.isSwitchLastPos =0  --记录移动时的位置
this.angelScreen =0 --球的偏移角度

function this.countMoveSpeed(curScreenPoint)
	if BowlingScene.playerBowling.m_mouseDown==false then
		return 0
	end
	this.moveUpTimeCount = this.moveUpTimeCount + Time.deltaTime --移动时间

	if this.showTouch(curScreenPoint) then --有屏幕手指按键（手机测试）
		
	else--（电脑测试）判定为有效移动时间
		this.showPcTouch(curScreenPoint)
	end
	

	--this.addPos() --屏蔽曲线球计算 路径
	--printDebug("@@@@ 移动距离 distance="..tostring(distance))
	---printDebug("@@@@ 移动速度 speed="..tostring(speed))
	return speed
	
end

function this.showPcTouch(curScreenPoint)
		if this.moveUpTimeCount<this.moveUpTime then --判定为有效移动时间
			this.moveInitPos =  curScreenPoint  --重新计算位置
			this.isSwitchLastPos = this.moveInitPos
		end
		if this.moveUpTimeCount>this.moveUpTimeTotal then --（电脑测试）判定为有效移动时间
			this.moveUpTimeCount=0
		end
		if this.moveUpTimeCount>this.moveUpTimeFrame then --（电脑测试） 每this.moveUpTimeFrame 检测一次
			local isMoveDistance = Vector3.Distance(this.isSwitchLastPos,curScreenPoint)
				--printDebug("#####isMoveDistance="..tostring(isMoveDistance))
				if isMoveDistance<0.5 then --判断距离检测手指有没移动
					this.moveUpTimeCount=0
					this.moveInitPos=curScreenPoint --手指停止移动的初始点
					printDebug("手指没有移动！！！！！")
				end
			
			this.moveUpTimeFrame =this.moveUpTimeCount+this.moveUpTime  --每this.moveUpTimeFrame 检测一次时间设置
			this.isSwitchLastPos  = curScreenPoint
		end
end
function this.showTouch(curScreenPoint)
	if Input.touchCount>0 then
		local touchType=Input.GetTouch(0).phase
		BowlingScene.printColor("blue","@@@@ 手指移动距离 Input.touchCount>0 touchType="..tostring(touchType))
		local typeNum=PhysicGameManager.Instance:getEnumNumber(touchType)
		BowlingScene.printColor("blue","@@@@ 手指移动距离 typeNum="..tostring(typeNum))
		
		if typeNum==0 then --TouchPhase.Began
			this.moveInitPos = curScreenPoint
			printDebug("手指 TouchPhase.Began  this.moveInitPos="..tostring(this.moveInitPos))
		elseif typeNum==1 then --TouchPhase.Move
		
		elseif typeNum==2 then --TouchPhase.Stationary --     A finger is touching the screen but hasn't moved.
			
			this.moveUpTimeCount=0
			this.moveInitPos=curScreenPoint --手指停止移动的初始点
			printDebug("手指没有移动！！！！！")
		elseif typeNum==3 then --TouchPhase.Ended
			printDebug("手指 TouchPhase.Ended  ="..tostring(curScreenPoint))
		elseif typeNum==4 then --TouchPhase.Canceled
			printDebug("手指 TouchPhase.Canceled  ="..tostring(curScreenPoint))
		end
		return true
	end
	return false
end


this.posList={}
this.posCuntDistance =1 --记录点距离
this.BallfowardAngelConst =10 --球面向实际角度，灵敏度 越大越小
this.curPos=Vector3.zero
function this.clearPos()
	this.curPos=Vector3.zero
	this.posList={}
end

--屏幕位置点转为保龄球朝向度数
function this.countBallfowardAngel(screenPos)

	local ballScreenPos =this.moveInitScreenPos --有效滑动屏幕初始点
	local ballToCurrPosDis = Vector3.Distance(screenPos,ballScreenPos) --球与目标点距离
	if  ballToCurrPosDis<=this.posCuntDistance then
		return this.angelScreen 
	end
	local currPosToBallXDis = screenPos.x -ballScreenPos.x--目标点与球x距离
	local angel = Mathf.Asin(currPosToBallXDis/ballToCurrPosDis)*Mathf.Rad2Deg  -- 轨迹圆弧角度
	angel=angel/this.BallfowardAngelConst
	--[[printDebug("<color='blue'>@@@@@@@@@countBallfowardAngel() this.moveInitScreenPos="..tostring(this.moveInitScreenPos).."</color>")
	printDebug("<color='blue'>@@@@@@@@@countBallfowardAngel() screenPos="..tostring(screenPos).."</color>")
	printDebug("<color='blue'>@@@@@@@@@countBallfowardAngel() ballToCurrPosDis="..tostring(ballToCurrPosDis).."</color>")
	printDebug("<color='blue'>@@@@@@@@@countBallfowardAngel() currPosToBallXDis="..tostring(currPosToBallXDis).."</color>")
		printDebug("<color='blue'>@@@@@@@@@countBallfowardAngel() angel="..tostring(angel).."</color>")--]]
	return angel
end
function this.countMoveAngel(power)
	if power<5 then
		return
	end
	local count = #this.posList
	local middleCount = Mathf.CeilToInt(count/2) --轨迹中间点
	local begainVec = this.posList[1] --轨迹开始点
	local middleVec = this.posList[middleCount] --轨迹中间点
	local endVec = this.posList[count] --轨迹结束点
	
	local begainToEndDis = Vector3.Distance(begainVec,endVec)/2
	local begainToMiddleDis = Vector3.Distance(begainVec,middleVec)
	
	local targetEndFoward = middleVec - endVec
	local targetBegainFoward = begainVec - middleVec
	local angelVecBegain = Vector3.Angle(middleVec,begainVec)
	local angelVecEnd = Vector3.Angle(middleVec,endVec)
	local angelVecEndFoward = Vector3.Angle(targetBegainFoward,targetEndFoward)
	

	
	local horDis  = (begainVec.x-middleVec.x)--移动x距离--判断是否有弧度或直线运行
	local horFowardDis  = (endVec.x-middleVec.x)--方向--左或右运动
	--printDebug("<color='yellow'>@@@@@  (middleVec.x-begainVec.x)="..tostring((middleVec.x-begainVec.x)).."</color>")
	--printDebug("<color='yellow'>@@@@@  horDis="..tostring(horDis).."</color>")
	--printDebug("<color='yellow'>@@@@@  this.isCirclePosList()="..tostring(this.isCirclePosList()).."</color>")
	if Mathf.Abs(horDis)>15 and this.isCirclePosList() then
		horFowardDis = horFowardDis>0 and 1 or -1
		
	else
		horFowardDis =0
	end
	printDebug("<color='yellow'>@@@@@  horFowardDis="..tostring(horFowardDis).."</color>")
	
	local angel = Mathf.Asin(begainToEndDis/begainToMiddleDis)*Mathf.Rad2Deg  -- 轨迹圆弧角度
	local angel2 = Mathf.Acos(begainToEndDis/begainToMiddleDis)*Mathf.Rad2Deg  -- 轨迹圆弧角度
	
	if begainToEndDis>begainToMiddleDis then  --弧度最大值
		angel =90
	end
	
	--printDebug("<color='blue'>@@@@@  this.posList.ount="..tostring(count).."</color>")
	--printDebug("<color='blue'>@@@@@  this.posList="..table.tostring(this.posList).."</color>")
	--printDebug("<color='blue'>@@@@@  begainVec="..tostring(begainVec).." middleVec="..tostring(middleVec).." endVec="..tostring(endVec).."</color>")
	--printDebug("<color='blue'>@@@@@  begainToEndDis="..tostring(begainToEndDis).." begainToMiddleDis="..tostring(begainToMiddleDis).."</color>")
	--printDebug("<color='blue'>@@@@@  angel="..tostring(angel).."</color>")
	--printDebug("<color='blue'>@@@@@  angel2="..tostring(angel2).."</color>")

	--printDebug("<color='blue'>@@@@@  angelVecBegain="..tostring(angelVecBegain).."</color>")
	--printDebug("<color='blue'>@@@@@  angelVecEnd="..tostring(angelVecEnd).."</color>")
	--printDebug("<color='blue'>@@@@@  angelVecEndFoward="..tostring(angelVecEndFoward).."</color>")
	
	--printDebug("<color='blue'>@@@@@  horFowardDis="..tostring(horFowardDis).."</color>")
	--local begainVec = #this.posList[1]
	BowlingBall.addHoriBall(Vector3(angel*horFowardDis,0,0),power)
end
function this.addPos()
	if Vector3.Distance(Input.mousePosition,this.curPos)>this.posCuntDistance then
		this.curPos = Input.mousePosition
		if #this.posList>100 then
			return
		end
		table.insert(this.posList,this.curPos)
		--printDebug("<color='yellow'>  this.addPos() this.curPos ="..tostring(this.curPos).."</color>")
	end
	
end

--判断轨迹是否为曲线
function this.isCirclePosList()
	local count = #this.posList
	local middleCount = Mathf.CeilToInt(count/2) --轨迹中间点
	local begainVec = this.posList[1] --轨迹开始点
	local middleVec = this.posList[middleCount] --轨迹中间点
	local endVec = this.posList[count] --轨迹结束点
	local targetEndFoward = middleVec.x - endVec.x
	local targetBegainFoward = middleVec.x - begainVec.x
	if (targetEndFoward>0 and targetBegainFoward>0) or (targetEndFoward<0 and targetBegainFoward<0) then
		return true
	end
	return false
end


function this.countHorizontalSpeed()
	
end


function this.IsHandMove()
	if this.moveUpTimeCount>this.moveUpTimeTotal then --超过时间重新计算速度
		local curScreenPoint = Vector3(Input.mousePosition.x, Input.mousePosition.y, BowlingScene.playerBowling.m_screenPoint.z)
	end
end


function this.MouseUpFireBall(speed)
		--printDebug("<color='red'>*******@@@@@@@@ this.MouseUpFireBall() hand speed="..tostring(speed).."</color>")
		local power = this.SpeedSwitchToPower(speed)
		--printDebug("<color='red'>*******@@@@@@@@ this.MouseUpFireBall() hand  power="..tostring(power).."</color>")
		this.BowlingBasePlayer.fireBall(power)
		--this.countMoveAngel(power)  --屏蔽曲线球
end

function this.SpeedSwitchToPower(speed)
	local power =5
	if speed then
		power=speed/150
	end
	--printDebug("<color='red'>*******@@@@@@@@ cout 临时 力大小power="..tostring(power).."</color>")
	--printDebug("<color='red'>*******@@@@@@@@  speed="..tostring(speed).."</color>")
	if power<2.5 then
		power=2.5
	end
	if power>6 then
		power=6
	end
	power=6
	return power
end
