require "bowling:module/data/BowlingPlayerScore"
require "bowling:module/data/BowlingGameManager"

BowlingPinContainer = {}
local this = BowlingPinContainer

this.Pins={}
this.pinWobbleRotZ =0.26 --可以看到保龄球的z角度
this.pinDownCount =0 --倒得瓶个数计算
this.pinWobbleFrames =3 --计算摇摆次数，确定为击倒状态
this.pinMoveChange =0.25 --击倒位移变化，大于为击倒
this.moveUpY = 1.12  --放置高点位置
this.isAnimation = false  --放置高点位置



--pins容器
function BowlingPinContainer.init(content)
	if content==nil then
		return
	end
	local len = content.transform.childCount
	--printDebug(" BowlingPinContainer.init() content.transform.childCount="..tostring(len))
	for i=0,len-1 do
--	for i=0,2 do
		local pinObj = content.transform:GetChild(i)
		this.initPin(pinObj,(i+1))
	end
	
end


function this.initPin(obj,index)
		local Pin={}
		Pin.target = obj.gameObject
		Pin.target.transform.localEulerAngles = Vector3(-90,0,0)
		Pin.target.gameObject.layer=BowlingPlayerController.ballLayer
		Pin.target.gameObject.name =tostring(index.."test")
		
		Pin.index =index
		Pin.m_rigidBody =obj:GetComponent(typeof(Rigidbody))
		Pin.m_rigidBody.mass = 1.5
		Pin.m_pinWobbleCounter=0 --瓶摇晃次数
		Pin.m_initalPos =obj.transform.position --初始放置位置
		Pin.m_initalrote =Pin.target.transform.rotation --初始角度
		Pin.m_isDown =false --是否被有效击倒
		Pin.isCollider =false --是否被碰到
		Pin.isAnimation =false --是否在动画中
		Pin.isColliderTime =0 --是否被碰到
		BowlingEZReplayManager.mark4Recording(Pin.target)
		
		Pin.target.transform.position =Vector3(Pin.m_initalPos.x,this.moveUpY,Pin.m_initalPos.z) --初始动画位置
		Pin.m_rigidBody.isKinematic =true
		--printDebug("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&& Pin.target.transform.localEulerAngles = "..tostring(Pin.target.transform.localEulerAngles))
		--printDebug("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&& Pin.target.dest.transform.eulerAngles = "..tostring(Pin.target.transform.eulerAngles))
		--printDebug("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&& Pin.target.dest.transform.rotation = "..tostring(Pin.target.transform.rotation))
		--printDebug("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&& Pin.target.dest.transform.localRotation = "..tostring(Pin.target.transform.localRotation))
		
		--EZReplayManager.mark4Recording(Pin.target, false, "pin")
	--	printDebug("@@@@@@@@@@@@@@@@@@ 碰撞到球瓶 Pin.target.transform.localEulerAngles="..tostring(Pin.target.transform.localEulerAngles))
		--printDebug("@@@@@@@@@@@@@@@@@@ 碰撞到球瓶 Pin.target.transform.rotation="..tostring(Pin.target.transform.rotation))
		--激活
		Pin.OnEnable=function()
			
		end
		--失效
		Pin.OnDisable=function()
			
		end
		Pin.Update=function()
	
			if Pin.isCollider ==false or Pin.m_isDown==true or this.isAnimation ==true then
				return
			end

			local d0 =(Pin.target.transform.position - Pin.m_initalPos).magnitude -- 偏移位置
			--local rotaZ = Mathf.Abs(Pin.target.transform.rotation.z)

			local rotaZ = Mathf.Abs(Pin.target.transform.rotation.z-Pin.m_initalrote.z)
			local rotaX = Mathf.Abs(Pin.target.transform.rotation.x-Pin.m_initalrote.x)
		--	printDebug("@@@@@@@@@@@@@@@@@@ 碰撞到球瓶 name="..Pin.target.gameObject.name.." 位移d0="..tostring(d0)))
			if Pin.m_isDown==false  and (rotaZ>this.pinWobbleRotZ or rotaX>this.pinWobbleRotZ)  then
				if Pin.m_pinWobbleCounter>this.pinWobbleFrames then
					Pin.OnPinDown()
			--[[		printDebug("11111@@@@@@@@@@@@@@@@@@ 碰撞到球瓶 ["..tostring(Pin.index).."] Pin.m_isDown="..tostring(Pin.m_isDown))
					printDebug("11111@@@@@@@@@@@@@@@@@@ rotaZ="..tostring(rotaZ))
					printDebug("11111@@@@@@@@@@@@@@@@@@ rotaX="..tostring(rotaX))--]]
					--printDebug("11111@@@@@@@@@@@@@@@@@@ Pin.target.transform.localRotation.x="..tostring(Pin.target.transform.localRotation.x))
				end
				Pin.m_pinWobbleCounter = Pin.m_pinWobbleCounter+1
				--printDebug("@@@@@@@@@@@@@@@@@@ 碰撞到球瓶name="..tostring(Pin.target.gameObject.name).."摇晃次数 Pin.m_pinWobbleCounter="..tostring(Pin.m_pinWobbleCounter))
			end
			
			if d0>this.pinMoveChange and Pin.m_isDown==false then
				Pin.OnPinDown()
				--printDebug("22222@@@@@@@@@@@@@@@@@@ 碰撞到球瓶 ["..tostring(Pin.index).."] Pin.m_isDown="..tostring(Pin.m_isDown))
			end
			
		end
		--撞倒
		Pin.OnPinDown = function()
			Pin.m_isDown=true
			this.pinDownCount = this.pinDownCount+1
			BowlingGameManager.pinDown()
			--if this.pinDownCount>5 then
				--AudioManager.playSound("bowling", "bowling_pins_down_lot")	
			--else
				AudioManager.playSound("bowling", "bowling_pins_down_less")	
			--end
			--[[		local initZ = Mathf.Abs(Pin.m_initalrote.z)
			local rotaZ = Mathf.Abs(Pin.target.transform.rotation.z)
			
			local d1 =Mathf.Abs(initZ - rotaZ)
		--	printDebug("@@@@@@@@@@@@@@@@@@ 碰撞到球瓶 name="..Pin.target.gameObject.name.." 位移d0="..tostring(d0))
		--	printDebug("@@@@@@@@@@@@@@@@@@ 碰撞到球瓶 Pin.target.transform.rotation.z="..tostring(Pin.target.transform.rotation.z))
			if d1>this.pinWobbleRotZ and Pin.m_isDown==false then
				if Pin.m_pinWobbleCounter>this.pinWobbleFrames then
						Pin.m_isDown=true
						this.pinDownCount = this.pinDownCount+1
						BowlingGameManager.pinDown()
					printDebug("11111@@@@@@@@@@@@@@@@@@ 碰撞到球瓶 ["..tostring(Pin.index).."] Pin.m_isDown="..tostring(Pin.m_isDown))
				end
				Pin.m_pinWobbleCounter = Pin.m_pinWobbleCounter+1
				--printDebug("@@@@@@@@@@@@@@@@@@ 碰撞到球瓶name="..tostring(Pin.target.gameObject.name).."摇晃次数 Pin.m_pinWobbleCounter="..tostring(Pin.m_pinWobbleCounter))
			end--]]
		end
		Pin.OnCollisionEnter=function(collision)
			if this.isAnimation  then
					return
			end
			Pin.isCollider =true
		end
		Pin.OnCollisionExit=function(collision)
			
		end
		Pin.Reset=function()
			Pin.m_rigidBody.velocity =Vector3.zero
			Pin.m_rigidBody.angularVelocity=Vector3.zero
			Pin.m_isDown=false
			Pin.isCollider=false
			Pin.m_rigidBody.isKinematic =true
			Pin.target.transform.position = Pin.m_initalPos
			Pin.target.transform.rotation = Pin.m_initalrote
			Pin.isColliderTime=0
			Pin.m_pinWobbleCounter=0
			Pin.target:SetActive(true)
			--printDebug("22222@@@@@@@@@@@@@@@@@@ 碰撞到球瓶 Pin.Reset Pin.target:SetActive(true) Pin.isCollider="..tostring(Pin.isCollider))
		end
		PhysicGameManager.Instance:addUpdateFun(Pin.Update)
		PhysicGameManager.Instance:addPhysic(Pin.target.gameObject,Pin.OnCollisionEnter,Pin.OnCollisionExit)
		--PhysicGameManager.Instance:addEnableFun(Pin.target.gameObject,Pin.OnEnable,Pin.OnDisable)
		this.Pins[index] = Pin
end

--一回合结束重置
function BowlingPinContainer.reset()
	if BowlingGameManager.isShowPinMotion then
		local sequ2 = this.cleanPinsSequ()
		sequ2.onComplete = function()
			for i=1,#this.Pins do
				local pin = this.Pins[i]
				pin.Reset()
				pin.target.transform.position= Vector3(pin.m_initalPos.x,this.moveUpY,pin.m_initalPos.z)
			end	
			BowlingPinContainer.getAllUpPinsMove(false)--再播放放瓶动画
			this.pinDownCount = 0
			BowlingPinResetAnimation.HideCleanHand()
		end
	else
		for i=1,#this.Pins do
			local pin = this.Pins[i]
			pin.Reset()
			pin.m_rigidBody.isKinematic =false
		end
		this.pinDownCount = 0
		BowlingPinResetAnimation.HideCleanHand()
	end
	
end

---回收瓶和重置瓶（半回合）
function BowlingPinContainer.removePin()
	if BowlingGameManager.isShowPinMotion then
		local motionGetPinTime= BowlingPinContainer.getAllUpPinsMove(true) --播放拿起瓶动画
		this.RestPinsMotion(motionGetPinTime)
	else
		for i=1,#this.Pins do
			local Pin = this.Pins[i]
			if Pin.m_isDown==false then
				Pin.target.transform.position =Pin.m_initalPos
				Pin.target.transform.rotation = Pin.m_initalrote
				Pin.m_rigidBody.isKinematic =false
						--printDebug("finish down "..Pin.target.name)
				this.isAnimation  =false
			else
				Pin.target:SetActive(false)
			end
			
		end
	end
end

--清除和放下没倒的瓶
function this.RestPinsMotion(motionGetPinTime)
		local sequ = DOTween.Sequence()
		sequ:SetDelay(motionGetPinTime)
		--printDebug("<color='blue'>BowlingPinContainer.removePin()  motionGetPinTime="..tostring(motionGetPinTime).."</color>")
		sequ.onComplete = function()
				local sequ2=this.cleanPinsSequ()
				sequ2.onComplete = function()
					for i=1,#this.Pins do
						local pin = this.Pins[i]
						if pin.m_isDown then
							pin.target:SetActive(false)
						end
					end	
					BowlingPinResetAnimation.HideCleanHand()
					local delayTime=BowlingPinContainer.getAllUpPinsMove(false)--再播放放瓶动画
					
				end
		end	

end
function this.cleanPinsSequ()
		local motionCleanTime= BowlingPinResetAnimation.onSweepPins() --再播放清除瓶动画
		local sequ2 = DOTween.Sequence()
		sequ2:SetDelay(motionCleanTime)
		this.isAnimation  =true
		return sequ2
end




--拿起所有没倒得瓶 true拿起瓶  false放下瓶
function BowlingPinContainer.getAllUpPinsMove(isUp)
		--printDebug("<color='blue'>  BowlingPinContainer.getAllUpPinsMove() isUp="..tostring(isUp).."</color>")
		BowlingPinResetAnimation.onSetPins()
		this.isAnimation  =true
		local mySequenceMove=nil

		--上下动画总时间
		local animationDownTime = 0.3
		local animationUpTime = 0.3
		--printDebug("<color='blue'>  BowlingPinContainer.getAllUpPinsMove() this.Pins="..table.tostring(this.Pins).."</color>")
		for i=1,#this.Pins do
			local Pin = this.Pins[i]
			if Pin.m_isDown==false then
				Pin.m_rigidBody.isKinematic =true
				
				if isUp then
					local sequ = DOTween.Sequence()
					mySequenceMove = Pin.target.transform:DOMoveY(this.moveUpY, animationUpTime)
					mySequenceMove:SetDelay(0.5)
					mySequenceMove.onComplete=function()
						--printDebug("finish up "..Pin.target.name)
						--printDebug("finish "..pin.target.name)
						--BowlingPinResetAnimation.HideRestHand()
					end
					sequ:Append(mySequenceMove)
					
				else
					--printDebug("begain down "..Pin.target.name)
					local sequ = DOTween.Sequence()
					mySequenceMove = Pin.target.transform:DOMoveY(Pin.m_initalPos.y,animationDownTime)
					mySequenceMove.onComplete=function()
						Pin.target.transform.position =Pin.m_initalPos
						Pin.target.transform.rotation = Pin.m_initalrote
						Pin.m_rigidBody.isKinematic =false
						--printDebug("finish down "..Pin.target.name)
						this.isAnimation  =false
						
					end
					sequ:Append(mySequenceMove)
				end
			end
			
		end
		 this.hideResetmotionObj(animationDownTime+animationUpTime)
		return animationDownTime+animationUpTime --播放动画时间

end

--定时隐藏RestHand装置
function this.hideResetmotionObj(delayTime)
	local sequ3 = DOTween.Sequence()
	sequ3:SetDelay(delayTime)
	sequ3.onComplete = function()
		BowlingPinResetAnimation.HideRestHand()
	end
end

function BowlingPinContainer.TestAllPinDown()
		if #this.Pins<=0 then
			return
		end
		for i=1,#this.Pins do
			local pin = this.Pins[i]
			if pin.m_isDown==false then
				pin.OnPinDown()
			end
		end
		BowlingGameManager.GameScript.showScore()
		BowlingGameManager.GameScript.resetPins()
end
function BowlingPinContainer.TestAllPinDownNum(num)
		if #this.Pins<=0 then
			return
		end
		local count = 0
		for i=1,#this.Pins do
			local pin = this.Pins[i]
			if pin.m_isDown==false then
				pin.OnPinDown()
				count = count+1
			end
			if count>= num then
				break
			end
		end
		BowlingGameManager.GameScript.showScore()
		BowlingGameManager.GameScript.resetPins()
end



function BowlingPinContainer.dispose()
	this.Pins={}
end