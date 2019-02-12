require "base:enum/NoticeType"
require "eliminate:enum/EliminateNoticeType"
require "eliminate:enum/ProtoEnumEliminate"
require "eliminate:module/eliminate/util/EliminateUtility"

require "eliminate:module/eliminate/view/EliminateGrid"
require "eliminate:module/eliminate/data/EliminateDataProxy"
require "eliminate:module/eliminate/ObjectPool"

EliminateModule = BaseModule:new()
local this = EliminateModule
local NoticeManager = CS.NoticeManager

this.moduleName = "EliminateModule"

--初始化net侦听
function this.initRegisterNet()
    if table.empty(ProtoEnumEliminate.MsgIdx) == false  then
        for _, v in pairs(ProtoEnumEliminate.MsgIdx) do
            this:registerNetMsg(v)
        end
    end
end
function this.onNetMsgLister(protoID,protoBytes)

end
function this:getRegisterNotificationList()
    if self.notificationList == nil then
        self.notificationList = {}
        -- 平台
        table.insert(self.notificationList, CommonNoticeType.Game_Enter)
        table.insert(self.notificationList, CommonNoticeType.Game_Exit)
        -- 业务
        for _, v in pairs(EliminateNoticeType) do
            table.insert(self.notificationList, v)
        end
    end
    return self.notificationList
end

function this:onNotificationLister(noticeType, notice)
    if GameManager.curGameId ~= EnumGameID.Eliminate then
        return
    end
    local switch = {
        -- 平台
        [CommonNoticeType.Game_Enter] = function()
            self:onEliminate_EnterScene(notice)
        end,
        [CommonNoticeType.Game_Exit] = function()
            self:onEliminate_GameExit(notice)
        end,
        -- 业务
        [EliminateNoticeType.LoadStart] = function()
            self:onLoadStart(notice)
        end,
        [EliminateNoticeType.LoadStep] = function()
            self:onLoadStep(notice)
        end,
        [EliminateNoticeType.LoadComplete] = function()
            self:onLoadComplete(notice)
        end,
        [EliminateNoticeType.GameReadyStart] = function()
            self:onGameReadyStart(notice)
        end,
        [EliminateNoticeType.StartTimerCount] = function()
            self:onStartTimerCount(notice)
        end,
        [EliminateNoticeType.GameFormalStart] = function()
            self:onGameFormalStart(notice)
        end,
        [EliminateNoticeType.PointedDown] = function()
            self:onPointedDown(notice)
        end,
        [EliminateNoticeType.PointedDrag] = function()
            self:onPointedDrag(notice)
        end,
        [EliminateNoticeType.TimerCountDecrease] = function()
            self:onTimerCountDecrease(notice)
        end,
        [EliminateNoticeType.ScoreChange] = function()
            self:onScoreChange(notice)
        end,
        [EliminateNoticeType.ComboChange] = function()
            self:onComboChange(notice)
        end,
        [EliminateNoticeType.ComboBreak] = function()
            self:onComboBreak(notice)
        end,
        [EliminateNoticeType.DazzleMomentStart] = function()
            self:onDazzleMomentStart(notice)
        end,
        [EliminateNoticeType.DazzleMomentOver] = function()
            self:onDazzleMomentOver(notice)
        end,
        [EliminateNoticeType.RankChange] = function()
            self:onRankChange(notice)
        end,
        [EliminateNoticeType.ItemSpawnStart] = function()
            self:onItemSpawnStart(notice)
        end,
        [EliminateNoticeType.ItemSpawnOver] = function()
            self:onItemSpawnOver(notice)
        end,
        [EliminateNoticeType.ItemClick] = function()
            self:onItemClick(notice)
        end,
        [EliminateNoticeType.ItemActive] = function()
            self:onItemActive(notice)
        end,
        [EliminateNoticeType.ItemTimeUp] = function()
            self:onItemTimeUp(notice)
        end,
        [EliminateNoticeType.RefreshMap] = function()
            self:onRefreshMap(notice)
        end,
        [EliminateNoticeType.TimeUp] = function()
            self:onTimeUp(notice)
        end,
        [EliminateNoticeType.AccountClear] = function()
            self:onAccountClear(notice)
        end,
        [EliminateNoticeType.OverTimerCount] = function()
            self:onOverTimerCount(notice)
        end,
        [EliminateNoticeType.GameOver] = function()
            self:onGameOver(notice)
        end,
        [EliminateNoticeType.BackToPlatform] = function()
            self:onBackToPlatform(notice)
        end,
        --=============V2==============
        [EliminateNoticeType.MatchStart] = function()
            self:onMatchStart(notice)
        end,
        [EliminateNoticeType.MatchEnd] = function()
            self:onMatchEnd(notice)
        end,

    }
    local fSwitch = switch[noticeType] --switch func
    if fSwitch then --key exists
        fSwitch() --do func
    else --key not found
        self:withoutRegistNotice(noticeType)--用于报错提醒
    end
end

--============================================ 事件 ==========================================

function this:onEliminate_EnterScene(notice)
    local obj = notice:GetObj()
    if obj.game_id == EnumGameID.Eliminate then
        print("消消-收到进入场景通知, obj = "..table.tostring(obj))
        EliminateNetModule.InitLoginInfo(obj)
        EliminateNetModule.sendReqJoinMatch()
        --test
        --GlobalTimeManager.Instance.timerController:RemoveTimerByKey("EliminateTestInput")
        --GlobalTimeManager.Instance.timerController:AddTimer("EliminateTestInput", -1, -1, function ()
        --    if Input.GetKeyDown(KeyCode.R) then
        --        --NoticeManager.Instance:Dispatch(EliminateNoticeType.RefreshMap)
        --    elseif Input.GetKeyDown(KeyCode.T) then
        --        --EliminateGrid:GetInstance():checkIsHaveWaitEliTest()
        --        local colorList = EliminateConfig.GetColorList()
        --        print("colorList: "..table.tostring(colorList))
        --    end
        --end)
    end
end

function this:onGameReadyStart(notice)
    --print("消消-事件-GameReadyStart, time = "..cur)
    AudioManager.playBGM("eliminate", "game_bgm")
    EliminateDataProxy:GetInstance():onReadyDataProxy()
    EliminateGrid:GetInstance():onGameReadyGrid()
end

function this:onStartTimerCount()
    --print("消消-事件-StartTimerCount")
    EliminateGameView:onStartTimerGameView()
end

function this:onGameFormalStart(notice)
    EliminateDataProxy:GetInstance():onStartDataProxy()
    EliminateGrid:GetInstance():onGameStartGrid()
end

function this:onPointedDown(notice)
    local obj = notice:GetObj()
    --print("消消-事件-点击按下")
    EliminateGrid:GetInstance():onPointedDownGrid(obj.screenPosition)
end

function this:onPointedDrag(notice)
    --print("消消-事件-滑动")
    local obj = notice:GetObj()
    EliminateGrid:GetInstance():onPointedDragGrid(obj)
end

function this:onScoreChange(notice)
    local obj = notice:GetObj()
    --print("消消-事件-分数变化,Score = "..obj.score)
    EliminateDataProxy:GetInstance():addMeScore(obj.deltaScore)
    EliminateScene:onScoreChangeScene()
    EliminateNetModule.sendReqUpdateMatchScore(EliminateDataProxy:GetInstance():getMeScore())
end

function this:onTimerCountDecrease(notice)
    EliminateDataProxy:GetInstance():onTimerCountDecreaseDataProxy()
    EliminateGameView:onCountDecreaseGameView()
end

function this:onComboChange(notice)
    local obj = notice:GetObj()
    EliminateGrid:GetInstance():onComboGrid()
    EliminateGameView:onComboGameView()
    EliminateScene:onComboScene(obj.index)
end

function this:onComboBreak(notice)
    local obj = notice:GetObj()
    --print("消消-事件-ComboBreak, obj = "..table.tostring(obj))
    EliminateScene:onComboBreakScene(obj.index)
    EliminateGameView:onComboBreakGameView()
end

function this:onRankChange(notice)
    --print("消消-事件-RankChange")
    EliminateScene:onRankChangeScene()
end

function this:onItemSpawnStart(notice)
    local obj = notice:GetObj()
    --print("消消-事件-ItemSpawnStart, obj = "..table.tostring(obj))
    EliminateGrid:GetInstance():onItemSpawnStartGrid(obj.itemType)
end

function this:onItemSpawnOver(notice)

end

function this:onItemClick(notice)
    local obj = notice:GetObj()
    --print("消消-事件-onItemClick, obj = "..table.tostring(obj))
    --EliminateGrid:GetInstance():onItemClickGrid(obj.itemIndex)
end

function this:onItemActive(notice)
    local obj = notice:GetObj()
    local item = EliminateGrid:GetInstance():getItemFromActivatingOrPool(obj.itemType)
    item:setItemType(obj.itemType)
    print("消消-事件-onItemActive, obj = "..table.tostring(obj))
    EliminateGrid:GetInstance():onItemActiveGrid(item)
end

function this:onItemTimeUp(notice)
    local obj = notice:GetObj()
    EliminateGrid:GetInstance():onItemTimeUpGrid(obj.item)
end

function this:onDazzleMomentStart(notice)
    --print("消消-事件-onDanceMomentStart")
    EliminateGrid:GetInstance():onDanceMomentStartGrid()
    EliminateGameView:OnDazzleStartGameView()
    EliminateScene:OnDazzleStartScene()
end

function this:onDazzleMomentOver(notice)
    --print("消消-事件-onDanceMomentOver")
    EliminateGrid:GetInstance():onDanceMomentOverGrid()
    EliminateScene:OnDazzleOverScene()

end

function this:onRefreshMap(notice)
    --print("消消-事件-刷新地图")
    EliminateGameView:onShuffleGameView()
    EliminateGrid:GetInstance():onShuffleGrid()
end

function this:onTimeUp()
    --print("消消-事件-时间到")
    AudioManager.playSound("eliminate", "time_up")
    AudioManager.stopBGM()
    EliminateGameView:onTimeUpGameView()
    EliminateGrid:GetInstance():onTimeUpGrid()
    EliminateScene:onTimeUpScene()
end

function this:onAccountClear()
    --print("消消-事件-结算消除")
    EliminateGameView:onAccountClearGameView()
    EliminateGrid:GetInstance():onAccountClearGrid()
end

function this:onOverTimerCount()
    AudioManager.playSound("eliminate", "over_timer")
end

function this:onGameOver(notice)
    local obj = notice:GetObj()
    EliminateDataProxy:GetInstance():onOverDataProxy()
    EliminateGrid:GetInstance():onGameOverGrid()
    EliminateGameView:onOverGameView()
    EliminateOverView:openEliminateOverView(obj)
    EliminatePopupView:onGameOverPopup()
    EliminateScene:onOverScene()
end

function this:onBackToPlatform(notice)
    print("消消-事件-返回大厅")
    AudioManager.stopBGM()
    EliminateDataProxy:GetInstance():onExitDataProxy()
    EliminateGrid:GetInstance():onExitGrid()
    EliminateGameView:onExitGameView()
    EliminateScene:onExitScene()
    EliminateLoadView:onExitLoadView()
    ObjectPool:GetInstance():onDestroyObjectPool()
    EliminateNetModule:OnExitGame()
    GameManager.exitGame(EnumGameID.Eliminate)
end

function this:onEliminate_GameExit(notice)
    print("消消-事件-掉线")
    self:onBackToPlatform(notice)
end
--=================================V2================================
function this:onMatchStart(notice)
    EliminateNetModule.sendReqRoomMatchBegin()
end

function this:onMatchEnd(notice)
    local obj = notice:GetObj()
    EliminateDataProxy:GetInstance():InitPlayerInfo(obj.match_info_list)
    NoticeManager.Instance:Dispatch(EliminateNoticeType.LoadStart)
end

function this:onLoadStart(notice)
    EliminateLoadView:openEliminateLoadView(function ()
        EliminateDataProxy:GetInstance():onLoadStartDataProxy()
    end)
end

function this:onLoadStep(notice)
    EliminateDataProxy:GetInstance():OnLoadStep()
    EliminateLoadView:OnLoadStep()
end

function this:onLoadComplete(notice)
    print("消消-事件-onLoadComplete")
    EliminateGrid:GetInstance():initGrid()
    EliminateScene:onEliminateEnterScene()
    EliminateLoadView:hide()
    EliminateGameView:onLoadCompleteGameView()
    NoticeManager.Instance:Dispatch(EliminateNoticeType.GameReadyStart)
end