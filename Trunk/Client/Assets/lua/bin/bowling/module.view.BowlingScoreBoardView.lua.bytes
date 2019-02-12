
require "base:enum/UIViewEnum"
require "bowling:module/data/BowlingPlayerScore"


BowlingScoreBoardView =BaseView:new()
local this=BowlingScoreBoardView
this.viewName="BowlingScoreBoardView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.BowlingScore, false)

--设置加载列表
this.loadOrders=
{
	"bowling:bowling_scoreboard",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
	--printDebug("@@@@@@@@@BowlingScoreBoardView this.main_mid  onLoadUIEnd()="..tostring(this.main_mid))
	this.initView()
end
--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.onGameOver, this.gameOver)
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.onGameResetStart,this.clearBoard)
	NoticeManager.Instance:AddNoticeLister(BowlingEvent.exitGame,this.dispose)
end
function this:addEvent()
	this.main_mid.back_btn:AddEventListener(UIEvent.PointerClick, this.quitGame)
	this.main_mid.openMotionBtn:AddEventListener(UIEvent.PointerClick, this.openMotion)
	this.main_mid.closeMotionBtn:AddEventListener(UIEvent.PointerClick, this.closeMotion)
end
--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end

function this:removeNotice()
	 NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.onGameOver, this.gameOver)
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.onGameResetStart,this.initView)
	NoticeManager.Instance:RemoveNoticeLister(BowlingEvent.exitGame,this.dispose)
end

this.scoreLabels={}
this.m_playerIndex =0
this.m_prevScore=0
this.m_playerScore = {}
this.m_dictionary = {} -- 保存玩家的得分

function this.initView()
	this.clearBoard()
	this.m_playerScore[1] = BowlingPlayerScore
	this.main_mid.openMotionBtn.gameObject:SetActive(false)
end

--index 1~10 的次数  roll 1~2的得分
function this.getScoreLabel(index,roll)
	local label= this.main_mid["f"..index.."t"..roll]
	return label
end
--index 1~10 的次数  roll 1~2的得分
function this.getResultLabel(index)
	local label= this.main_mid["f"..index.."s"]
	return label
end

function BowlingScoreBoardView.onSetScore(frameIndex,throwIndex,score,gutterBall,playerIndex)
	printDebug("=============begain BowlingScoreBoardView.onSetScore())")
	printDebug("frameIndex="..tostring(frameIndex))
	printDebug("throwIndex="..tostring(throwIndex))
	printDebug("<color='red'>score="..tostring(score).."</color>")
	printDebug("gutterBall="..tostring(gutterBall))
	printDebug("playerIndex="..tostring(playerIndex))
	
	this.m_playerIndex = playerIndex
	local str = "f"..frameIndex.."t"..throwIndex
	--printDebug("========str="..str)
	local scoreSTR = tostring(score)
	--this.clearBoard()
	if this.main_mid.windowTitle then
		this.main_mid.windowTitle.text = "Player "..(playerIndex+1)
	end

	local BowlingPlayerScore = this.m_playerScore[1]
	BowlingPlayerScore.addScore(score,frameIndex,throwIndex)

	local scoreResult= BowlingGameManager.scoreResult(true)
	
	local isEndRound =false --不是最后一球
	if frameIndex ==BowlingGameManager.GameScript.MAX_NOM_FRAMES then
		isEndRound=true
		if throwIndex ==3 then
			this.updateFrameScores(frameIndex)
		end
	end
	if throwIndex ==1 then
			--前一次投球为补球	
			local preScore= this.getPreScoreStr(frameIndex,throwIndex,playerIndex)
			--printDebug("<color='green'>@@@=====throwIndex ==1========isEndRound preScore="..tostring(preScore).."</color>")
			--printDebug("<color='green'>@@@=========throwIndex ==1====isEndRound scoreResult="..tostring(scoreResult).."</color>")
			if preScore=="/" then --上一球是补球，下一球直接更新补球的结果
				this.updateFrameScores(frameIndex-1)
			end
	elseif throwIndex ==2 then
			if not isEndRound and scoreResult~=BowlingScoreResult.Spare then --不是补球和最后一回合则马上刷新当前分数
				this.updateFrameScores(frameIndex)
			elseif isEndRound then --是最后一回合
				local preScore= this.getPreScoreStr(frameIndex,throwIndex,playerIndex) 
				--printDebug("<color='green'>@@@====throwIndex ==2 =========isEndRound preScore="..tostring(preScore).."</color>")
				--printDebug("<color='green'>@@@====throwIndex ==2 =========isEndRound scoreResult="..tostring(scoreResult).."</color>")
				if preScore~="X" and scoreResult~=BowlingScoreResult.Spare then --上一球不是是全中和补球，直接刷新结果
					this.updateFrameScores(frameIndex)
				end
			end
	end
	--printDebug("<color='green'>@@@@@@@@@@@@@  scoreResult="..scoreResult.."</color>")
	local switch = {
        -- 平台
        [BowlingScoreResult.Strike] = function()
			--printDebug("Strike1!")
			showFloatTips("Strike1")
			BowlingGameManager.showTitleCard("Strike1!")
			scoreSTR ="X"
			BowlingGameManager.strikeUI()
			AudioManager.playSound("bowling", "bowling_cheer")	
        end,
        [BowlingScoreResult.Spare] = function()
			--printDebug("Spare!")
			showFloatTips("Spare!")
			BowlingGameManager.showTitleCard("Spare!")
			scoreSTR ="/"
			BowlingGameManager.spareUI()
			AudioManager.playSound("bowling", "bowling_cheer")	
        end,
        -- 业务
        [BowlingScoreResult.normalScore] = function()
			--printDebug(score.." Pins!")
			showFloatTips(score.." Pins!")
			BowlingGameManager.showTitleCard(score.." Pins!")
			BowlingGameManager.pinKnockDown()
        end,
        [BowlingScoreResult.Gutter] = function()
			--printDebug("Gutter Ball!")
			showFloatTips("Gutter Ball!\n 0 Pins")
			BowlingGameManager.showTitleCard("Gutter Ball!")
			BowlingGameManager.gutterBallUI()
			AudioManager.playSound("bowling", "bowling_sigh")	
        end,

    }
	
	
	BowlingScene.printColor("red"," BowlingScoreBoardView.onSetScore  scoreResult="..tostring(scoreResult))
    local fSwitch = switch[scoreResult] --switch func
    if fSwitch then --key exists
        fSwitch() --do func
    end
	local str2 = str.."player"..playerIndex
	this.m_dictionary[str2] = scoreSTR
	--printDebug("======== str2="..str2.." scoreSTR="..scoreSTR)
	for i=1,frameIndex do
		for j=1,3 do
			local id = "f"..i.."t"..j
			local id2= id.."player"..playerIndex
			local label = this.getScoreLabel(i,j)
			local scoreRecord = this.m_dictionary[id2]
			if label and scoreRecord then
				label.text = scoreRecord
				--printDebug("======== scoreRecord="..scoreRecord)
			end
		end
	end
		--printDebug("=============end BowlingScoreBoardView.onSetScore())")
end


--获取前一球的分数状态
function this.getPreScoreStr(frameIndex,throwIndex,playerIndex)
	throwIndex = throwIndex-1
	local preFrame=false
	if throwIndex<1 then
		throwIndex=2
		frameIndex = frameIndex -1
	end

	local preStr= "f"..(frameIndex).."t"..tostring(throwIndex).."player"..playerIndex
	local preScore= this.m_dictionary[preStr] 
	return preScore

end

function this.updateFrameScores(frameIndex)
	for i=1,frameIndex do
		local label=this.getResultLabel(i)
		if label then
			--local scoreTem = this.m_playerScore[1].getScoreForFrame()
			local scoreTem = BowlingPlayerScore.getScoreForFrame(i)
			label.text = tostring(scoreTem)
		end
	end
end

function this.openMotion()
	BowlingGameManager.isShowPinMotion =true
	this.main_mid.openMotionBtn.gameObject:SetActive(false)
	this.main_mid.closeMotionBtn.gameObject:SetActive(true)
	showFloatTips("开启动画成功")
end

function this.closeMotion()
	BowlingGameManager.isShowPinMotion =false
	NoticeManager.Instance:Dispatch(BowlingEvent.overThenMotion)
	this.main_mid.openMotionBtn.gameObject:SetActive(true)
	this.main_mid.closeMotionBtn.gameObject:SetActive(false)
	showFloatTips("关闭动画成功")
end


function this.clearBoard()
	for index=1,10 do
		local scoreLabel1= this.getScoreLabel(index,1)
		scoreLabel1.text = ""
		local scoreLabel2= this.getScoreLabel(index,2)
		scoreLabel2.text = ""
		local scoreLabel3= this.getScoreLabel(10,3)
		scoreLabel3.text = ""
		local resultLabel= this.getResultLabel(index)
		resultLabel.text = ""
	end
	this.m_playerIndex =0
	this.m_prevScore=0
	this.m_playerScore[1] = BowlingPlayerScore
	this.m_dictionary = {} -- 保存玩家的得分
end

function  this.gameOver()
	local totalScore = this.getResultLabel(10)
	--showFloatTips("游戏结束\n 得分="..tostring(totalScore))
	--Alert.showVerifyMsg("游戏结果","总得分："..tostring(totalScore.text),"重新开始",BowlingGameManager.ResetStartGame,"退出游戏",BowlingScene.dispose,true)
	ViewManager.open(UIViewEnum.BowlingResultView,tostring(totalScore.text))
	BowlingFollowCamera.stopCameraMove()
end
function this.quitGame()
	ViewManager.open(UIViewEnum.BowlingQuitGameView)
end

function this.dispose()
	this.clearBoard()
	ViewManager.close(UIViewEnum.BowlingPunishTimeView)
	ViewManager.destroyView(UIViewEnum.BowlingScore)
end





