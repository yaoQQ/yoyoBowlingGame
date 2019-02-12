require "base:enum/NoticeType"
require "animal:enum/AnimalNoticeType"
require "animal:enum/proto/ProtoEnumAnimalChess"

require "animal:module/AnimalObjectPool"
require "animal:module/AnimalConfig"
require "animal:module/data/AnimalDataProxy"
require "animal:module/data/AnimalPlayerData"
require "animal:module/util/AnimalUtility"
require "animal:module/view/AnimalPiece"

AnimalModule = BaseModule:new()
local this = AnimalModule

this.moduleName = "AnimalModule"

function this:getRegisterNotificationList()
    if self.notificationList == nil then
        self.notificationList = {}
        -- 平台
        table.insert(self.notificationList, CommonNoticeType.Game_Enter)
        table.insert(self.notificationList, CommonNoticeType.Game_Exit)
        -- 业务
        for _, v in pairs(AnimalNoticeType) do
            table.insert(self.notificationList, v)
        end
    end
    return self.notificationList
end

function this:onNotificationLister(noticeType, notice)
    if GameManager.curGameId ~= EnumGameID.Animal then
        return
    end
    local switch = {
        -- 平台
        [CommonNoticeType.Game_Enter] = function()
            self:onAnimal_EnterScene(notice)
        end,
        [CommonNoticeType.Game_Exit] = function()
            self:onAnimal_Game_Exit(notice)
        end,
        -- 业务
        [AnimalNoticeType.MatchStart] = function()
            self:onAnimal_MatchStart(notice)
        end,
        [AnimalNoticeType.MatchEnd] = function()
            self:onAnimal_MatchEnd(notice)
        end,
        [AnimalNoticeType.LoadStart] = function()
            self:onAnimal_LoadStart(notice)
        end,
        [AnimalNoticeType.LoadStep] = function()
            self:onAnimal_LoadStep(notice)
        end,
        [AnimalNoticeType.LoadComplete] = function()
            self:onAnimal_LoadComplete(notice)
        end,
        [AnimalNoticeType.RoundTurn] = function()
            self:onAnimal_RoundTurn(notice)
        end,
        [AnimalNoticeType.Reverse] = function()
            self:onAnimal_Reverse(notice)
        end,
        [AnimalNoticeType.MoveResult] = function()
            self:onAnimal_MoveResult(notice)
        end,
        [AnimalNoticeType.Select] = function()
            self:onAnimal_Select(notice)
        end,
        [AnimalNoticeType.SelectCancel] = function()
            self:onAnimal_SelectCancel(notice)
        end,
        [AnimalNoticeType.Dead] = function()
            self:onAnimal_Dead(notice)
        end,
        [AnimalNoticeType.ScoreChange] = function()
            self:onAnimal_ScoreChange(notice)
        end,
        [AnimalNoticeType.GameOver] = function()
            self:onAnimal_GameOver(notice)
        end,
        [AnimalNoticeType.GameRank] = function()
            self:onAnimal_GameRank(notice)
        end,
        [AnimalNoticeType.BackToPlatform] = function()
            self:onAnimal_BackToPlatform(notice)
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

function this:onAnimal_EnterScene(notice)
    local obj = notice:GetObj()
    if obj.game_id ~= EnumGameID.Animal then
        return
    end
    print("斗兽棋-收到进入场景通知, obj = "..table.tostring(obj))
    AnimalNetModule.InitLoginInfo(obj)
    AnimalNetModule.ReqJoinMatch()

    -- test
    --GlobalTimeManager.Instance.timerController:AddTimer("AnimalInput", -1, -1, function ()
    --    if Input.GetKeyDown(KeyCode.A) then
    --        print("斗兽棋, 测试")
    --        --local msg = {}
    --        --msg.game_result = 2
    --        --NoticeManager.Instance:Dispatch(AnimalNoticeType.GameOver, msg)
    --        AnimalUtility.CountScroll(0, 8000, AnimalGameView.main_mid.score_text, 1)
    --    end
    --end)

end

function this:onAnimal_MatchStart(notice)
    local msg = notice:GetObj()
    ViewManager.open(UIViewEnum.Animal_MatchView, nil, function ()
        AnimalNetModule.ReqRoomMatchBegin()
        AnimalMatchView:OnMatchStart(msg.player_info)
    end)
end

function this:onAnimal_MatchEnd(notice)
    local msg = notice:GetObj()
    --print("斗兽棋-obj: "..table.tostring(msg))
    AnimalDataProxy:OnMatchEnd(msg.match_info_list)
    AnimalMatchView:OnMatchEnd(msg.match_info_list)
    NoticeManager.Instance:Dispatch(AnimalNoticeType.LoadStart)
end

function this:onAnimal_LoadStart(notice)
    print("斗兽棋-加载开始")
    AnimalDataProxy:OnLoadStart()
    AnimalMatchView:OnLoadStart()
end

function this:onAnimal_LoadStep(notice)
    AnimalDataProxy:OnLoadStep()
    AnimalMatchView:OnLoadStep()
end

function this:onAnimal_LoadComplete(notice)
    AnimalGameView:OnLoadComplete()
end

function this:onAnimal_RoundTurn(notice)
    local obj = notice:GetObj()
    AnimalGameView:OnRoundTurn(obj)
end

function this:onAnimal_Reverse(notice)
    local obj = notice:GetObj()
    AnimalGameView:OnReverse(obj)
end

function this:onAnimal_MoveResult(notice)
    local msg = notice:GetObj()
    AnimalGameView:OnMoveResult(msg)
end

function this:onAnimal_Select(notice)
    local obj = notice:GetObj()
    AnimalGameView:OnSelect(obj.x, obj.y)
end

function this:onAnimal_SelectCancel(notice)
    AnimalGameView:OnSelectCancel()
end

function this:onAnimal_Dead(notice)
    local obj = notice:GetObj()
    AnimalDataProxy:OnDeadDataProxy(obj)
end

function this:onAnimal_ScoreChange(notice)
    local msg = notice:GetObj()
    AnimalGameView:OnScoreChange(msg.score)
end

function this:onAnimal_GameOver(notice)
    local obj = notice:GetObj()
    AnimalGameView:OnOver()
    AnimalOverView:OnAnimalOverView(obj.game_result)
end

function this:onAnimal_GameRank(notice)
    local msg = notice:GetObj()
    AnimalOverView:OnGameRank(msg.player_rank_info)
end

function this:onAnimal_BackToPlatform(notice)
    AudioManager.stopBGM()
    AnimalDataProxy:OnExitDataProxy()
    AnimalGameView:OnExitGameView()
    AnimalMatchView:OnExitMatchView()
    AnimalOverView:OnExitOverView()
    AnimalObjectPool:GetInstance():OnDestroyObjectPool()
    AnimalNetModule:OnExitGame()
    GameManager.exitGame(EnumGameID.Animal)
end

function this:onAnimal_Game_Exit(notice)
    CSLoger.debug(Color.Yellow, "========斗兽棋-离线事件响应=========")
    self:onAnimal_BackToPlatform(notice)
end
