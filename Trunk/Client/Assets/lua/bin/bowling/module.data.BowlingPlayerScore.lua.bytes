

BowlingPlayerScore = {}
local this = BowlingPlayerScore

this.currentRoll=0 --//当前轮数 满分加一次
this.score=0 --计算分数
this.m_rollFrameCount={} -- 《次数，m_rolls对应位置》
this.m_rolls={}--顺序记得分数

this.m_score=0 --玩家得分数
this.m_playerDone = false
this.m_perfectGame =false
this.frameIndex=1


function BowlingPlayerScore.ScoreBoardView()

end
--玩家得分数
function BowlingPlayerScore.TestScore()
	local countTable={1,2,3,4,5,6}
	---1
	BowlingPlayerScore.addScore(3,1)
	BowlingPlayerScore.addScore(3,2)
	
	---2
	BowlingPlayerScore.addScore(3,3)
	BowlingPlayerScore.addScore(3,4)
	
	---3
	BowlingPlayerScore.addScore(3,5)
	BowlingPlayerScore.addScore(3,6)
	
	--4
	BowlingPlayerScore.addScore(3,7)
	BowlingPlayerScore.addScore(3,8)
	
	--5
	BowlingPlayerScore.addScore(3,9)
	BowlingPlayerScore.addScore(3,10)
	
	--6
	BowlingPlayerScore.addScore(3,11)
	BowlingPlayerScore.addScore(3,12)
	
	--7
	BowlingPlayerScore.addScore(3,13)
	BowlingPlayerScore.addScore(3,14)
	
	--8
	BowlingPlayerScore.addScore(3,15)
	BowlingPlayerScore.addScore(3,16)
	
	--9
	BowlingPlayerScore.addScore(3,17)
	BowlingPlayerScore.addScore(3,18)
	
	--10
	BowlingPlayerScore.addScore(3,19)
	BowlingPlayerScore.addScore(7,20)
	BowlingPlayerScore.addScore(1,21)

	
	--BowlingPlayerScore.addScore(3,5)
	--BowlingPlayerScore.addScore(5,6)
	
	BowlingPlayerScore.showLog("@@@@BowlingPlayerScore.TestScore()  this.m_rolls="..table.tostring(this.m_rolls))
	BowlingPlayerScore.showLog("@@@@BowlingPlayerScore.TestScore()  this.m_rollFrameCount="..table.tostring(this.m_rollFrameCount))
	--for i=1,#countTable do
		BowlingPlayerScore.showLog("===========@@@@begain BowlingPlayerScore.TestScore()")
		local index =20
		local totalScore=BowlingPlayerScore.getScoreForFrame(index)
		BowlingPlayerScore.showLog("===========@@@@end BowlingPlayerScore.TestScore()  index=["..tostring(index).."]  totalScore="..tostring(totalScore))
	--end
--[[		printDebug("===========@@@@begain2 BowlingPlayerScore.TestScore()")
		local index2 =5
		local totalScore=BowlingPlayerScore.getScoreForFrame(index2)
		printDebug("===========@@@@end2 BowlingPlayerScore.TestScore()  index2=["..tostring(index2).."]  totalScore="..tostring(totalScore))--]]
end 

function BowlingPlayerScore.showFrame(frame)
	local totalScore=BowlingPlayerScore.getScoreForFrame(frame)
	BowlingPlayerScore.showLog("===========@@@@end BowlingPlayerScore.TestScore()  frame=["..tostring(frame).."]  totalScore="..tostring(totalScore))
end

--玩家得分数
function BowlingPlayerScore.getScore()
	return this.m_score
end 

--玩家得分数
function BowlingPlayerScore.setPlayerDone(isDone)
	this.m_playerDone = isDone
end 
--玩家得分数 frameIndex投的次数记录分数  score 当前次数所得分 throwIndex第几次投球
function BowlingPlayerScore.addScore(score,currframeIndex,throwIndex)
	--printDebug("begain++++++++=+++++=++=+++BowlingPlayerScore.addScore() score="..score.."  currframeIndex="..tostring(currframeIndex))
	this.m_rolls[this.frameIndex] = score
	this.frameIndex = this.frameIndex+1
	if currframeIndex==BowlingGameManager.GameScript.MAX_NOM_FRAMES then-- 最后一局 ，总共10局
		local scoreIndex = this.m_rollFrameCount[currframeIndex]
		if scoreIndex then
			local indexScore = this.m_rolls[scoreIndex]--当前局指向分数，有x固定指向x，有/固定指向/,
			if indexScore==10 or throwIndex==3 then --默认指向x,不指向新分数
				return
			end
		end
	end
	this.m_rollFrameCount[currframeIndex] = #this.m_rolls--指向当前局的计算分数标志位  1~10次数
end 


--计算当前 frameIndex投的次数记录分数 所得总分
function BowlingPlayerScore.calculateScore(frameIndex)
	this.score=0
	this.currentRoll=0
	this.scoreAllRolls(frameIndex)
	return this.score
end 

--计算当前 frameIndex投的次数记录分数 所得总分
function BowlingPlayerScore.getScoreForFrame(index)
--	printDebug("BowlingPlayerScore.getScoreForFrame() index="..tostring(index))
	local frameIndex = this.m_rollFrameCount[index]
	--printDebug("BowlingPlayerScore.getScoreForFrame() frameIndex="..tostring(frameIndex))
	local scoreTep = BowlingPlayerScore.calculateScore(frameIndex)
	if scoreTep>this.m_score then
		this.m_score = scoreTep
	end
	
	-- 错误答案大于300
	if scoreTep>300 then
		this.m_perfectGame =true
	end
--	printDebug("=========BowlingPlayerScore.getScoreForFrame() result score="..tostring(scoreTep))
	return scoreTep
end 

function this.scoreAllRolls(frameIndex)
	--printDebug("@@@@@@ this.scoreAllRolls()  frameIndex="..tostring(frameIndex))
	for frame=0,frameIndex-1 do
		if frameIndex>this.currentRoll and #this.m_rolls>this.currentRoll then
			BowlingPlayerScore.showLog("#####begain  this.scoreAllRolls("..frameIndex..")  this.currentRoll="..tostring(this.currentRoll))
			if this.m_rolls[this.currentRoll+1]==10 then
				this.scoreStrike(frameIndex)
			elseif this.sumUpRollsFromCurrent(2,frameIndex,true)==10 then
				this.scoreSpare(frameIndex)
			else
				this.scoreNormal(frameIndex)
			end
			BowlingPlayerScore.showLog("============########end  this.scoreAllRolls("..frameIndex..")  this.currentRoll="..tostring(this.currentRoll))
			BowlingPlayerScore.showLog("=========#######end  this.scoreAllRolls("..frameIndex..")  this.score="..tostring(this.score))
		end
	end
end

--  //补全加后面一次投球,
function this.scoreNormal(frameIndex)
	BowlingPlayerScore.showLog("@@@this.scoreNormal() frameIndex="..tostring(frameIndex))
	this.score = this.score +this.sumUpRollsFromCurrent(2,frameIndex)
	this.currentRoll = this.currentRoll+2
end
--  //补全加后面一次投球,
function this.scoreSpare(frameIndex)
	BowlingPlayerScore.showLog("====@@@this.scoreSpare() frameIndex="..tostring(frameIndex))
	this.score = this.score +this.sumUpRollsFromCurrent(3,frameIndex)
	this.currentRoll = this.currentRoll+2
end
--  //满分加后面两次投球得分
function this.scoreStrike(frameIndex)
	BowlingPlayerScore.showLog("==========@@@this.scoreStrike() frameIndex="..tostring(frameIndex))
	this.score = this.score +this.sumUpRollsFromCurrent(3,frameIndex)
	this.currentRoll = this.currentRoll+1
end


--  //最后次数分数 与后面次数分数计算 numberOfRolls后面相加次数 frameIndex当前次数
function this.sumUpRollsFromCurrent(numberOfRolls,frameIndex,notSshow)
	local sum=0
	if not notSshow then
		BowlingPlayerScore.showLog("@@@this.sumUpRollsFromCurrent() numberOfRolls="..tostring(numberOfRolls))
	end
	for i=1,numberOfRolls do
		local n = this.currentRoll+i
		if not notSshow then
			BowlingPlayerScore.showLog("@@@this.sumUpRollsFromCurrent() i="..tostring(i).." this.currentRoll="..this.currentRoll.." n="..tostring(n))
			BowlingPlayerScore.showLog("@@@this.sumUpRollsFromCurrent() #this.m_rolls="..tostring(#this.m_rolls))
		end
		
		if #this.m_rolls>=n then
			
			sum =sum + this.m_rolls[n]
		end
	end
	if not notSshow then
		BowlingPlayerScore.showLog("=================@@@@@@@@@@this.sumUpRollsFromCurrent() sum="..tostring(sum))
	end
	return sum
end

this.notShow =true
function BowlingPlayerScore.showLog(str)
	if  this.notShow then
		return
	end
	printDebug("<color='green'>"..tostring(str).."</color>")
end

function BowlingPlayerScore.Rest()
	this.currentRoll=0 --//当前轮数 满分加一次
	this.score=0 --计算分数
	this.m_rollFrameCount={} -- 《次数，m_rolls对应位置》
	this.m_rolls={}--顺序记得分数

	this.m_score=0 --玩家得分数
	this.m_playerDone = false
	this.m_perfectGame =false
	this.frameIndex=1
end 


function BowlingPlayerScore.Update()
	
end 
