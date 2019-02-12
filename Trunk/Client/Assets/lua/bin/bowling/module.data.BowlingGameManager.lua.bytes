
require "bowling:module/view/BowlingBall"
BowlingGameManager = {}
local this = BowlingGameManager





BowlingGameManager.isShowPinMotion =true
BowlingGameManager.isScanGame=false
BowlingGameManager.isShowLoading=true --loading 界面展示
BowlingGameManager.isEditor=false --true 测试生成添加模拟坐标



--碰撞回调
function BowlingGameManager.onBallHitBowlingPit()
	BowlingGameManager.GameScript.onBallHitBowlingPit()
end


--游戏开始
function BowlingGameManager.init()
	--this.initGameScript()
	this.addNotice()
	this.initGameScript()
end
function this.addNotice()
	 NoticeManager.Instance:AddNoticeLister(BowlingEvent.onGameResetStart,this.ReStartGame)
	 NoticeManager.Instance:AddNoticeLister(NoticeType.GM_Send_To_MainGateway, this.onGMSend2MainGateway)
end
function this.removeNotice()
	 NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.onGameResetStart, this.ReStartGame)
	 NoticeManager.Instance:RemoveNoticeLister(NoticeType.GM_Send_To_MainGateway, this.onGMSend2MainGateway)
end
function this.onGMSend2MainGateway(noticeType, notice)
    local cmd = notice:GetObj()
  
	
	if cmd=="removeLoading" then
		BowlingLoadingView.connectServer()
	end
	if cmd=="server" then
		ViewManager.open(UIViewEnum.SelectServerView)
	end
	if cmd=="1" then
		MVPGameModule.sendInitScreen()
	end
	if cmd=="2" then
		MVPGameModule.SendStopPos()
	end
	if cmd=="4" then
		MVPGameModule.sendChoosePerson(1)
	end
	
	if cmd=="0" then
		moveDebug1 = true
		moveDebug2 = true
	end
	if cmd=="9" then
		moveDebug1=false
		moveDebug2 =false
	end
end


function this.initData()
	printDebug("BowlingGameManager this.initData()")
	
	BowlingGameManager.GameScript.m_throw=0 --一阶段第几次投
	BowlingGameManager.GameScript.m_frame=1  -- 是第几次1~10
	BowlingGameManager.GameScript.m_score=0  --当次得分
	BowlingGameManager.GameScript.m_gutterBall=false  --是否为渠道球
	BowlingGameManager.GameScript.m_gameover=false  --是否游戏结束
	BowlingGameManager.GameScript.MAX_NOM_FRAMES=10  --是否为渠道球
	BowlingGameManager.GameScript.m_prevScore=0  --前一次得分
	BowlingGameManager.GameScript.m_canReset=0  --是否可以重置游戏
	BowlingGameManager.GameScript.m_currentTurn=0  --当前回合
	BowlingGameManager.GameScript.nomPlayers=1  --当前回合玩家
end
function this.ReStartGame()
	--printDebug("################## BowlingGameManager@@@ AddNoticeLister(BowlingEvent.onGameResetStart  this.ReStartGame()")
	this.initData()
	BowlingGameManager.RestGame()
	BowlingPlayerScore.Rest()
--	ViewManager.open(UIViewEnum.BowlingStartCountView)
end

function this.initGameScript()
	--printDebug("BowlingGameManager this.initGameScript()")
	BowlingGameManager.GameScript={}
	this.initData()
	
	BowlingGameManager.GameScript.onBallHitBowlingPit=function()
	
	end
	--球瓶倒
	BowlingGameManager.GameScript.onPinDown=function()
		BowlingGameManager.GameScript.m_score=BowlingGameManager.GameScript.m_score+1
	--	printDebug("BowlingGameManager.GameScript.onPinDown()  BowlingGameManager.GameScript.m_score="..BowlingGameManager.GameScript.m_score)
	end
	--进入渠道球
	BowlingGameManager.GameScript.onGutterBall=function()
		BowlingGameManager.GameScript.m_gutterBall=true
	end
	--进入渠道球
	BowlingGameManager.GameScript.onGameOver=function()
		BowlingGameManager.GameScript.m_gameover=true 
		BowlingGameManager.GameScript.m_throw=0
		BowlingGameManager.GameScript.m_frame=1
		BowlingGameManager.GameScript.m_score=0
		BowlingGameManager.GameScript.m_gutterBall=false
		BowlingGameManager.GameScript.m_prevScore=0
		BowlingGameManager.GameScript.m_canReset=0 
		BowlingGameManager.GameScript.m_currentTurn=0 
		BowlingGameManager.GameScript.nomPlayers=1 
		
	end
	
	BowlingGameManager.GameScript.showScore=function()
		BowlingGameManager.GameScript.m_throw = BowlingGameManager.GameScript.m_throw+1
		if BowlingGameManager.GameScript.m_gameover==false then
			BowlingScoreBoardView.onSetScore(BowlingGameManager.GameScript.m_frame,BowlingGameManager.GameScript.m_throw,BowlingGameManager.GameScript.m_score,BowlingGameManager.GameScript.m_gutterBall,BowlingGameManager.GameScript.nomPlayers)
		end
	end
	--重置保龄球  this.endRoundRest() 或播放完录像处罚
	BowlingGameManager.GameScript.resetPins=function()
		BowlingGameManager.GameScript.m_gutterBall=false
		local scoreResult = BowlingScoreResult.normalScore
		if BowlingGameManager.GameScript.m_gameover==false then
			printDebug("<color='blue'>=============BowlingGameManager.GameScript.resetPins() ====</color> BowlingGameManager.GameScript.m_frame="..tostring(BowlingGameManager.GameScript.m_frame))
			if BowlingGameManager.GameScript.m_frame <BowlingGameManager.GameScript.MAX_NOM_FRAMES then
				if BowlingGameManager.GameScript.m_throw==1 then
					scoreResult=BowlingGameManager.GameScript.handleThrow1()
				elseif BowlingGameManager.GameScript.m_throw==2 then
					scoreResult=BowlingGameManager.GameScript.handleThrow2()
					
				end
			else
				scoreResult=BowlingGameManager.GameScript.handleFinalThrow()	
			end
		end
		return scoreResult
	end

	BowlingGameManager.GameScript.handleThrow1=function()
		--printDebug("<color='blue'>GameScript.handleThrow1()</color>")
		--BowlingGameManager.GameScript.m_frame=BowlingGameManager.GameScript.m_frame+1
		if BowlingGameManager.GameScript.m_score ~=10 then
			--printDebug("<color='blue'>GameScript.handleThrow1()  BowlingGameManager.GameScript.m_score ~=10 </color>")
			BowlingGameManager.removePins()
			return BowlingScoreResult.Spare
		else
			BowlingGameManager.GameScript.m_frame=BowlingGameManager.GameScript.m_frame+1
			BowlingGameManager.GameScript.m_throw=0
			BowlingGameManager.GameScript.m_score=0
			BowlingGameManager.GameScript.nextTurn()
			return BowlingScoreResult.normalScore
		end
	end
	
	BowlingGameManager.GameScript.handleThrow2=function()
		--printDebug("<color='blue'>GameScript.handleThrow2()</color>")
		BowlingGameManager.GameScript.m_frame = BowlingGameManager.GameScript.m_frame+1
		BowlingGameManager.GameScript.m_throw =0
		BowlingGameManager.GameScript.m_score=0
		BowlingGameManager.RestGame()
		return BowlingScoreResult.normalScore
	end
	BowlingGameManager.GameScript.handleFinalThrow=function()
		printDebug("<color='green'>GameScript.handleFinalThrow() BowlingGameManager.GameScript.m_throw="..tostring(BowlingGameManager.GameScript.m_throw).."</color>")
		local  scoreResult = BowlingScoreResult.normalScore
		if BowlingGameManager.GameScript.m_throw==1 then
			if BowlingGameManager.GameScript.m_score~=10 then
				BowlingGameManager.removePins()
				scoreResult=BowlingScoreResult.Spare
			else
				BowlingGameManager.RestGame()
			end
		elseif BowlingGameManager.GameScript.m_throw==2 then 
			local comboScore = BowlingGameManager.GameScript.m_prevScore + BowlingGameManager.GameScript.m_score
			if BowlingGameManager.GameScript.m_score==10  or comboScore==10 then
				BowlingGameManager.RestGame()
				BowlingGameManager.GameScript.m_prevScore=0
			elseif BowlingGameManager.GameScript.m_prevScore==10 then
				BowlingGameManager.removePins()
				scoreResult=BowlingScoreResult.Spare
			else
				BowlingGameManager.onGameOver()
			end
		else 
			BowlingGameManager.onGameOver()
			--printDebug("<color='green'>GameScript.handleFinalThrow()</color>")
		end
		BowlingGameManager.GameScript.m_score=0
		return scoreResult
	end
	
		--重置保龄球
	BowlingGameManager.GameScript.nextTurn=function()
		--printDebug("<color='blue'>GameScript.nextTurn()</color>")
		--printDebug("BowlingGameManager.GameScript.m_currentTurn="..tostring(BowlingGameManager.GameScript.m_currentTurn))
			BowlingGameManager.RestGame()
	end

end

--游戏开始
function BowlingGameManager.ResetStartGame()
	NoticeManager.Instance:Dispatch(BowlingEvent.onGameResetStart)
end

--游戏重致
function BowlingGameManager.RestGame()
	printDebug("<color='blue'>BowlingGameManager.RestGame()</color>")
	BowlingBall.reset(BowlingScoreResult.normalScore)
	BowlingPinContainer.reset()
	BowlingPunishTimeView.startCount()
end

--游戏结束
function BowlingGameManager.GameOver()

end
function BowlingGameManager.fireBall()
		--BowlingGameManager.isShowPinMotion =true
		BowlingScene.resetCount()
end



--isCountScore 是否this.m_prevScore保存数据，加入到积分计算
function BowlingGameManager.scoreResult(isCountScore)
	local frameIndex = BowlingGameManager.GameScript.m_frame
	local throwIndex = BowlingGameManager.GameScript.m_throw
	local score = BowlingGameManager.GameScript.m_score
	local gutterBall = BowlingGameManager.GameScript.m_gutterBall 
	--local playerIndex = BowlingGameManager.GameScript.nomPlayers
	local scoreResult =BowlingScoreResult.normalScore
	if throwIndex ==1 and isCountScore then
		BowlingGameManager.GameScript.m_prevScore =score
		--printDebug("<color='red'>BowlingGameManager.GameScript.m_prevScore="..tostring(BowlingGameManager.GameScript.m_prevScore).."</color>")
	end
	printDebug("BowlingGameManager.scoreResult  frameIndex="..tostring(frameIndex))
	printDebug("BowlingGameManager.scoreResult  throwIndex="..tostring(throwIndex))
	printDebug("BowlingGameManager.scoreResult  score="..tostring(score))
	printDebug("BowlingGameManager.scoreResult  scoreResult="..tostring(scoreResult))
	if score==0 and gutterBall then
		scoreResult = BowlingScoreResult.Gutter
	elseif (score==10 and throwIndex==1) or (score==10 and throwIndex==2 and frameIndex==BowlingGameManager.GameScript.MAX_NOM_FRAMES ) or(score==10 and throwIndex==3 and frameIndex==BowlingGameManager.GameScript.MAX_NOM_FRAMES) then
		scoreResult = BowlingScoreResult.Strike
		--printDebug("scoreResult = BowlingScoreResult.Strike  score="..tostring(score).."  throwIndex="..tostring(throwIndex))
	elseif (score+BowlingGameManager.GameScript.m_prevScore )==10 and throwIndex>=2 then
		scoreResult = BowlingScoreResult.Spare
	else
		scoreResult = BowlingScoreResult.normalScore
	end
	return scoreResult
end


	
function BowlingGameManager.removePins()
	--printDebug("<color='blue'>BowlingGameManager.removePins()</color>")
	BowlingGameManager.GameScript.m_score=0
	BowlingPinContainer.removePin()
	BowlingBall.reset(BowlingScoreResult.Spare)
	BowlingPunishTimeView.startCount()
end


--进入沟渠
function BowlingGameManager.gutterBall()
	BowlingGameManager.GameScript.onGutterBall()
end

--球瓶倒下 保龄瓶倒触发
function BowlingGameManager.pinDown()
	BowlingGameManager.GameScript.onPinDown()
end



--球瓶倒下 保龄瓶倒触发
function BowlingGameManager.playersTurn(currTurn)
	
end

--投保龄球一次
function BowlingGameManager.onSetScore()
	
end

--球进入gutter, 得分 展示
function BowlingGameManager.showTitleCard()
	
end
--球进入gutter, 得分 展示
function BowlingGameManager.gutterBallUI()
	
end

--完整10分
function BowlingGameManager.strikeUI()
	
end
--补分为10分
function BowlingGameManager.spareUI()
	
end
--没有达到10分
function BowlingGameManager.pinKnockDown()
	
end

--游戏结束
function BowlingGameManager.onGameOver()
	BowlingGameManager.setGameTimeScale(1)
	NoticeManager.Instance:Dispatch(BowlingEvent.onGameOver)
	BowlingGameManager.GameScript.onGameOver()
	
end

--退出游戏
function BowlingGameManager.dispose()
	this.removeNotice()
	BowlingGameManager = {}
end

function BowlingGameManager.setGameTimeScale(scale)
	Time.timeScale =scale
end