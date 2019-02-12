

BowlingBasePlayer = {}
local this = BowlingBasePlayer

this.m_ball=nil --球
this.power=5 --@100 玩家施加的力 2~ 7.4
this.m_fired=false--是否准备投球
this.playerIndex=0--玩家标志
this.m_myTurn=false--是否我的回合
this.m_frame=0--第几次
this.m_gameOver=false--是否结束
this.xMax=1--（-1 1）角度变化范围






this.xScalar=0.25   --影响系数

function BowlingBasePlayer.setBall(ball)
	this.m_ball = ball
end 

function BowlingBasePlayer.Update()
	
end 
function BowlingBasePlayer.fireBall(power)

	--power=7.4

	--this.m_ball.fireBall(3)
	if this.m_fired==false then 
		--this.m_fired=true
		if this.m_gameOver ==false then
			local vel =800 --Random.Range(100,500)--Random.Range(1000,5000)
			--local angel = Random.Range(-this.xMax,this.xMax)--原来代码
			--local quaternion =Quaternion.AngleAxis(angel,Vector3.up)--原来代码
			--local forceVec = quaternion*this.m_ball.target.transform.forward*vel*power--原来代码
			local forceVec = this.m_ball.target.transform.forward*vel*power
			
		--	printDebug("=====================begain BowlingBasePlayer.fireBall()=========================")
			printDebug("@@@forceVec="..tostring(forceVec))
			--printDebug("angel="..tostring(angel))
			--printDebug("quaternion="..tostring(quaternion))
		--	printDebug("vel="..tostring(vel))
		--	printDebug("this.power="..tostring(this.power))--]]
			this.m_ball.fireBall(forceVec)
			BowlingFollowCamera.setCameraFllowBallSpeed(forceVec)
			--printDebug("=====================end BowlingBasePlayer.fireBall()=========================")
		end
	end
end 
