require "bowling:module/data/BowlingModelManager"
require "bowling:module/data/BowlingTag"
require "bowling:module/data/BowlingEZReplayManager"

BowlingBall = {}
local this = BowlingBall

this.pitRegion={}--保龄球碰撞重置区域
this.count=false--保龄球碰撞重置区域,开始计算时间
BowlingBall.playerBallName ="playerBall" --玩家的保龄球
BowlingBall.isGameStop=false


--初始化球对象
function BowlingBall.init()
	this.addNotice()
end
function this.addNotice()
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.exitGame,this.dispose)
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.endRoundReset,this.endRoundRest)
end
function this.removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.endRoundReset,this.endRoundRest)
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.exitGame,this.dispose)
end

function this.endRoundRest()
		this.pitRegion.stayTime=0
		this.isCount=false
		BowlingBall.m_fired =false --是否射击
		this.addHoriForce=Vector3.zero
		printDebug("<color='red'>BowlingBall this.endRoundRest() this.isCount=false </color>")
end
function BowlingBall.initBall(obj)
		BowlingBall.target = obj
		BowlingBall.target.name = BowlingBall.playerBallName
		BowlingBall.target.gameObject.layer=BowlingPlayerController.ballLayer
		BowlingBall.name =BowlingScene.playerBallName
		BowlingBall.index =1 
		BowlingBall.m_rigidBody =obj:GetComponent(typeof(Rigidbody))
		BowlingBall.m_rigidBody.mass = 6
		BowlingBall.angVel =Vector3(60,0,0) -- 球运动的旋转
		BowlingBall.m_initalPos =BowlingBall.target.transform.position
		BowlingBall.m_prevLocs =Vector3.zero -- 鼠标点击位置
		BowlingBall.m_initalRot =BowlingBall.target.transform.rotation
		BowlingBall.m_screenPoint =Vector3.zero -- 球所在屏幕位置
		BowlingBall.gutterPositions =Vector2(-0.65,0.65) --最先和最大沟位置
		BowlingBall.m_fired =false --是否射击
		BowlingBall.m_mouseDown =false --是否点击
		BowlingBall.m_offset =Vector3.zero --是否点击
		BowlingEZReplayManager.mark4Recording(obj)
		
		BowlingBall.m_dragThreshold =60 --判断是为水平移动
		BowlingBall.m_dragLimitX =0.4 --球水平位移限制最大x位置点
		BowlingBall.pointerGO =BowlingBall.target.transform:Find("pointer").gameObject
		BowlingBall.pointerGO.transform.eulerAngles = Vector3.zero
		BowlingBall.forceMode =1 --Force = 0,Impulse = 1,VelocityChange = 2,Acceleration = 5
			
		BowlingBall.Update=function()
			--printDebug("BowlingBall update() time=".. Time.time)
			if not BowlingBall.m_fired then
		
				return
			end
			if  BowlingBall.target.transform.position.y<-40 then
				printDebug("-- 错误出边界---")
					BowlingGameManager.RestGame() -- 错误出边界
			end
			--开始录制回放
			if BowlingBall.target.transform.position.z>16 then
				BowlingEZReplayManager.record()
				this.addHoriForceTime = this.addHoriForceFrameTime --补添加旋转力
				--printDebug("-- BowlingBall.target.transform.position.z>16---")
			end
			
			--屏蔽曲线球
		--[[	if BowlingBall.target.transform.position.z>5 then
					---添加旋转力
				if this.addHoriForce==Vector3.zero then
					return
				end
				this.addHoriForceTime = this.addHoriForceTime-Time.deltaTime
				--printDebug("-- BowlingBall.target.transform.position.z>5---this.addHoriForce="..tostring(this.addHoriForce))
				if this.addHoriForceTime<0 then
					this.addHoriForceTime =this.addHoriForceFrameTime
					BowlingBall.m_rigidBody:AddForce(this.addHoriForce)
					--printDebug("BowlingBall.m_rigidBody:AddForce(this.addHoriForce)="..tostring(this.addHoriForce))
				end
			end--]]

		end

		BowlingBall.fireBall=function(vec)
--[[			printDebug("@@@@BowlingBall.fireBall() vec="..tostring(vec))
			printDebug("@@@@BowlingBall.fireBall() BowlingBall="..table.tostring(BowlingBall))--]]
			if BowlingBall.m_rigidBody and Time.timeScale~=0 then
			
				if BowlingBall.pointerGO then
					BowlingBall.pointerGO:SetActive(false)
				end
				BowlingBall.m_fired =true
				BowlingBall.m_rigidBody.isKinematic=false
				this.isCount=true
				--local addforceVec =BowlingBall.target.transform.rotation * vec --原来代码
				local addforceVec =vec
				
				--printDebug("@@@@BowlingBall.fireBall() AddForce="..tostring(addforceVec))
				--BowlingBall.m_rigidBody:AddForce(Vector3(2.2, 0, 3244.1))
				BowlingBall.m_rigidBody:AddForce(addforceVec)
				--  球运动速度 forceVec.z= 1600~6000  旋转角度 6~60
				BowlingBall.m_rigidBody.angularVelocity=Vector3(addforceVec.z,0,0)/79
				--printDebug("<color='blue'>BowlingBall.setMoveRotate() BowlingBall.m_rigidBody.angularVelocity=="..tostring(BowlingBall.m_rigidBody.angularVelocity).."</color>")
				BowlingGameManager.fireBall()
				AudioManager.playSound("bowling", "bowling_fire")	
			end

		end
		
		this.addHoriForce=Vector3.zero --添加曲线力
		this.addHoriForceFrameTime =0.2
		this.addHoriForceTime =this.addHoriForceFrameTime
		BowlingBall.addHoriBall=function(vec,power)
			printDebug("<color='blue'>addHoriBall()  vec="..tostring(vec).."</color>")
			printDebug("<color='blue'>addHoriBall()  power="..tostring(power).."</color>")
			--BowlingBall.m_rigidBody:AddForce(vec)
			this.addHoriForce = vec
		end


		BowlingBall.OnCollisionEnter=function(collision)
			if collision.gameObject.name== BowlingTag.Gutter then
				BowlingGameManager.gutterBall()
				this.ballInGutter()
				local pos = BowlingBall.target.transform.position
				if pos.x>0 then
					--pos.x =  BowlingBall.gutterPositions.y
				end
				if pos.x<0 then
					---pos.x =  BowlingBall.gutterPositions.x
				end
				--BowlingBall.m_startGutterXPos.x
				 BowlingBall.target.transform.position = pos
				--BowlingEZReplayManager.stop()
			--	printDebug("@@@@ 碰撞设置保龄球位置 BowlingBall.target.transform.position"..tostring(BowlingBall.target.transform.position))
			end
		end
		BowlingBall.OnCollisionExit=function(collision)
			
		end
		BowlingBall.OnCollisionStay=function(collision)
			if collision.gameObject.name== BowlingTag.Gutter then -- 进入沟渠
				this.ballInGutter()
			end
		end
		---保龄球进入重置区域
		this.pitRegion.OnTriggerStay=function(collision)
			if this.isCount==false then
				return
			end
			if collision.gameObject.name==BowlingBall.playerBallName and this.isCount then
				this.pitRegion.stayTime=this.pitRegion.stayTime+Time.deltaTime
				if this.pitRegion.stayTime>3 then--投球一回合结束
				--	printDebug("======= this.pitRegion.OnTriggerStay  NoticeManager.Instance:Dispatch(BowlingEvent.endRoundReset)======")
					NoticeManager.Instance:Dispatch(BowlingEvent.endRoundReset)

				end
			end
		end

		PhysicGameManager.Instance:addUpdateFun(BowlingBall.Update)
		PhysicGameManager.Instance:addPhysic(BowlingBall.target.gameObject,BowlingBall.OnCollisionEnter,BowlingBall.OnCollisionExit,BowlingBall.OnCollisionStay)
		PhysicGameManager.Instance:addPhysicTrigger(this.pitRegion.target.gameObject,nil,this.pitRegion.OnTriggerStay)
		
end
function this.ballInGutter()
	--BowlingBall.m_rigidBody.angularVelocity = Vector3.zero
	local vel= BowlingBall.m_rigidBody.velocity
	--vel.y=0
	BowlingBall.m_rigidBody.velocity = vel
	this.addHoriForce=Vector3.zero
end
function BowlingBall.initPitRegion(obj)
	this.pitRegion.stayTime=0
	this.pitRegion.target = obj
	
end
function BowlingBall.Update()


end

function BowlingBall.reset(resultScore)
		BowlingBall.target.transform.position = BowlingBall.m_initalPos
		BowlingBall.target.transform.rotation = BowlingBall.m_initalRot
		BowlingBall.m_prevLocs =Vector3.zero -- 鼠标点击位置
		BowlingBall.m_screenPoint =Vector3.zero -- 球所在屏幕位置
		BowlingBall.m_fired =false --是否射击
		BowlingBall.m_mouseDown =false --是否点击
		BowlingBall.m_offset =Vector3.zero --是否点击
		BowlingBall.m_rigidBody.isKinematic=true
		BowlingBall.pointerGO.transform.eulerAngles = Vector3.zero
		BowlingBall.pointerGO:SetActive(true)
		BowlingFollowCamera.restCameraSpeed(resultScore)
end

function BowlingBall.getBallScreenPos()
	local screenPoint = BowlingScene.MainCamera:WorldToScreenPoint(BowlingBall.target.transform.position)
	return screenPoint
end


function BowlingBall.getBallScreenPosToWorldPos(screenPos)
		if BowlingBall.m_screenPoint ==Vector3.zero then
			BowlingScene.playerBowling.m_screenPoint =BowlingBall.getBallScreenPos()
		end
		local worldPos= BowlingScene.MainCamera:ScreenToWorldPoint(Vector3(screenPos.x, screenPos.y, BowlingScene.playerBowling.m_screenPoint.z))
		return worldPos
end

function BowlingBall.dispose()
	this.removeNotice()
end