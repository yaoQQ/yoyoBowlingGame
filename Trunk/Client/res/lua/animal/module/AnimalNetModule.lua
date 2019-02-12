
AnimalNetModule = BaseModule:new()
local this = AnimalNetModule
this.moduleName = "AnimalNetModule"
local loginInfo = nil
local ExitTimeOutKey = "AnimalOverTimeOut"    -- 退出游戏请求超时计时, 超时之后前端直接退出

function this.InitLoginInfo(info)
    loginInfo = info
end

--初始化net侦听
function this.initRegisterNet()
    this:registerNetMsg(ProtoEnumGame.MsgIdx.MsgIdxRspJoinMatch)
    this:registerNetMsg(ProtoEnumGame.MsgIdx.MsgIdxRspRoomMatchBegin)
    this:registerNetMsg(ProtoEnumGame.MsgIdx.MsgIdxNotifyLoadingProgress)
    this:registerNetMsg(ProtoEnumGame.MsgIdx.MsgIdxNotifyLoadingFinish)
    this:registerNetMsg(ProtoEnumGame.MsgIdx.MsgIdxRspUpdateMatchState)
    this:registerNetMsg(ProtoEnumGame.MsgIdx.MsgIdxRspGameRank)

    if table.empty(ProtoEnumAnimalChess.MsgIdx) == false  then
        for _, v in pairs(ProtoEnumAnimalChess.MsgIdx) do
            this:registerNetMsg(v)
        end
    end
end

function this:getRegisterNotificationList()
    if self.notificationList == nil then
        self.notificationList = {}
    end
    return self.notificationList
end


function this.onNetMsgLister(protoID, protoBytes)
    if GameManager.curGameId ~= EnumGameID.Animal then
        return
    end
    if ProtoEnumGame.MsgIdx.MsgIdxRspJoinMatch == protoID then
        this.RspJoinMatch(protoBytes)
    elseif ProtoEnumGame.MsgIdx.MsgIdxRspRoomMatchBegin == protoID then
        this.RspRoomMatchBegin(protoBytes)
    elseif ProtoEnumGame.MsgIdx.MsgIdxNotifyLoadingProgress == protoID then
        this.NotifyLoadingProgress(protoBytes)
    elseif ProtoEnumGame.MsgIdx.MsgIdxNotifyLoadingFinish == protoID then
        this.NotifyLoadingFinish(protoBytes)
    elseif ProtoEnumGame.MsgIdx.MsgIdxRspUpdateMatchState == protoID then
        this.onRspUpdateMatchState(protoBytes)
    elseif ProtoEnumGame.MsgIdx.MsgIdxRspGameRank == protoID then
        this.onRspGameRank(protoBytes)

        -- 游戏部分
    elseif ProtoEnumAnimalChess.MsgIdx.MsgIdxRspReverseChess == protoID then
        this.RspReverseChess(protoBytes)
    elseif ProtoEnumAnimalChess.MsgIdx.MsgIdxNotifyRoundTurn == protoID then
        this.NotifyRoundTurn(protoBytes)
    elseif ProtoEnumAnimalChess.MsgIdx.MsgIdxNotifyMoveChess == protoID then
        this.NotifyMoveChess(protoBytes)
    elseif ProtoEnumAnimalChess.MsgIdx.MsgIdxNotifyGameOver == protoID then
        this.NotifyGameOver(protoBytes)
    elseif ProtoEnumAnimalChess.MsgIdx.MsgIdxNotifyScoreChange == protoID then
        this.NotifyScoreChange(protoBytes)
    end
end

function this:OnExitGame()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(ExitTimeOutKey)
    if not table.empty(loginInfo) then
        NetworkManager.Instance:Disconnect(loginInfo.gateway_name)
    end
end

--============================================ 发送 ==========================================
function this.ReqJoinMatch()
    local req = {}
    req.match_type = loginInfo.game_type
    req.active_id = loginInfo.active_id
    req.player_id = loginInfo.player_id
    this.sendNetMsg(loginInfo.gateway_name, "game","ReqJoinMatch", req)
end

function this.ReqRoomMatchBegin()
    local req = {}
    req.active_id = loginInfo.active_id
    this.sendNetMsg(loginInfo.gateway_name, "game","ReqRoomMatchBegin", req)
end

function this.ReqLoadingProgress()
    local progress = AnimalDataProxy:GetMeLoadProgress()
    local req = {}
    req.loading_progress = progress
    this.sendNetMsg(loginInfo.gateway_name, "game","ReqLoadingProgress", req)
end

function this.ReqReverseChess(x, y)
    local req = {}
    req.position = {}
    req.position.x = x
    req.position.y = y
    this.sendNetMsg(loginInfo.gateway_name, "animalchess","ReqReverseChess", req)
end

function this.ReqReqMoveChess(origin, target)
    local req = {}
    req.old_position = {}
    req.new_position = {}
    req.old_position = origin
    req.new_position = target
    this.sendNetMsg(loginInfo.gateway_name, "animalchess","ReqMoveChess", req)
end

function this.ReqUpdateMatchState()
    local req = {}
    req.state = ProtoEnumCommon.AactiveGameState.AactiveGameState_UnCanJion
    this.sendNetMsg(loginInfo.gateway_name, "game","ReqUpdateMatchState", req)
    GlobalTimeManager.Instance.timerController:AddTimer(ExitTimeOutKey, 6000, 1, function ()
        print("斗兽棋-请求退出响应时间过长, 前端自我退出")
        NoticeManager.Instance:Dispatch(AnimalNoticeType.BackToPlatform)
    end)
end

function this.ReqSurrender()
    local req = {}
    this.sendNetMsg(loginInfo.gateway_name, "animalchess","ReqSurrender", req)
end

function this.ReqGameRank()
    local req = {}
    req.active_id = loginInfo.active_id
    req.page_index = 0
    req.num = 20
    this.sendNetMsg(loginInfo.gateway_name, "game","ReqGameRank", req)
end

function this.sendNotifyOperationFinish(id)
    local req = {}
    req.player_id = id
    this.sendNetMsg(loginInfo.gateway_name, "animalchess","NotifyOperationFinish", req)
end

--============================================ 接收 ==========================================
function this.RspJoinMatch(protoBytes)
    local msg = this.decodeProtoBytes("game","RspJoinMatch", protoBytes)
    if msg.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        NoticeManager.Instance:Dispatch(AnimalNoticeType.MatchStart, msg)
    else
        NoticeManager.Instance:Dispatch(CommonNoticeType.Exception)
    end
end

function this.RspRoomMatchBegin(protoBytes)
    local msg = this.decodeProtoBytes("game","RspRoomMatchBegin", protoBytes)
    NoticeManager.Instance:Dispatch(AnimalNoticeType.MatchEnd, msg)
end

function this.NotifyLoadingProgress(protoBytes)
    local msg = this.decodeProtoBytes("game","NotifyLoadingProgress", protoBytes)

end

function this.NotifyLoadingFinish(protoBytes)
    local msg = this.decodeProtoBytes("game","NotifyLoadingFinish", protoBytes)
    NoticeManager.Instance:Dispatch(AnimalNoticeType.LoadComplete)
end

function this.RspReverseChess(protoBytes)
    local msg = this.decodeProtoBytes("animalchess","RspReverseChess", protoBytes)
    if msg.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        NoticeManager.Instance:Dispatch(AnimalNoticeType.Reverse, msg)
    else
        printError("斗兽棋-错误-result: "..msg.result)
    end
end

function this.NotifyRoundTurn(protoBytes)
    local msg = this.decodeProtoBytes("animalchess","NotifyRoundTurn", protoBytes)
    NoticeManager.Instance:Dispatch(AnimalNoticeType.RoundTurn, msg)
end

function this.NotifyMoveChess(protoBytes)
    local msg = this.decodeProtoBytes("animalchess","NotifyMoveChess", protoBytes)
    NoticeManager.Instance:Dispatch(AnimalNoticeType.MoveResult, msg)
end

function this.NotifyGameOver(protoBytes)
    local msg = this.decodeProtoBytes("animalchess","NotifyGameOver", protoBytes)
    NoticeManager.Instance:Dispatch(AnimalNoticeType.GameOver, msg)
end

function this.NotifyScoreChange(protoBytes)
    local msg = this.decodeProtoBytes("animalchess","NotifyScoreChange", protoBytes)
    NoticeManager.Instance:Dispatch(AnimalNoticeType.ScoreChange, msg)
end

function this.onRspGameRank(protoBytes)
    local msg = this.decodeProtoBytes("game","RspGameRank", protoBytes)
    NoticeManager.Instance:Dispatch(AnimalNoticeType.GameRank, msg)
end

function this.onRspUpdateMatchState(protoBytes)
    local msg = this.decodeProtoBytes("game","RspUpdateMatchState", protoBytes)
    NoticeManager.Instance:Dispatch(AnimalNoticeType.BackToPlatform)
end