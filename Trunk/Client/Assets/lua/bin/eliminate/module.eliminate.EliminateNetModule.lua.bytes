
EliminateNetModule = BaseModule:new()
local this = EliminateNetModule
this.moduleName = "EliminateNetModule"
local loginInfo = nil
local ExitTimeOutKey = "EliminateOverTimeOut"    -- 退出游戏请求超时计时, 超时之后前端直接退出

function this.InitLoginInfo(info)
    loginInfo = info
end

--初始化net侦听
function this.initRegisterNet()
    if table.empty(ProtoEnumGame.MsgIdx) == false  then
        for _, v in pairs(ProtoEnumGame.MsgIdx) do
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
    if GameManager.curGameId ~= EnumGameID.Eliminate then
        return
    end
    if ProtoEnumGame.MsgIdx.MsgIdxRspJoinMatch == protoID then
        this.onRspJoinMatch(protoBytes)
    elseif ProtoEnumGame.MsgIdx.MsgIdxRspRoomMatchBegin == protoID then
        this.onRspRoomMatchBegin(protoBytes)
    elseif ProtoEnumGame.MsgIdx.MsgIdxNotifyLoadingProgress == protoID then
        this.onNotifyLoadingProgress(protoBytes)
    elseif ProtoEnumGame.MsgIdx.MsgIdxNotifyLoadingFinish == protoID then
        this.onNotifyLoadingFinish(protoBytes)
    elseif ProtoEnumGame.MsgIdx.MsgIdxRspGameRank == protoID then
        this.onRspGameRank(protoBytes)
        -- 游戏部分
    elseif ProtoEnumGame.MsgIdx.MsgIdxRspUpdateMatchState == protoID then
        this.onRspUpdateMatchState(protoBytes)
    end
end

function this:OnExitGame()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(ExitTimeOutKey)
    if not table.empty(loginInfo) then
        NetworkManager.Instance:Disconnect(loginInfo.gateway_name)
    end
end

--=========================发送消息=======================
function this.sendReqJoinMatch()
    local req = {}
    req.match_type = loginInfo.game_type
    req.active_id = loginInfo.active_id
    req.player_id = loginInfo.player_id
    this.sendNetMsg(loginInfo.gateway_name, "game","ReqJoinMatch", req)
end

function this.sendReqRoomMatchBegin()
    local req = {}
    req.active_id = loginInfo.active_id
    this.sendNetMsg(loginInfo.gateway_name, "game","ReqRoomMatchBegin", req)
end

function this.sendReqLoadingProgress(progress)
    local req = {}
    req.loading_progress = progress
    this.sendNetMsg(loginInfo.gateway_name, "game","ReqLoadingProgress", req)
end

function this.sendReqUpdateMatchScore(score)
    local req = {}
    req.score = score
    this.sendNetMsg(loginInfo.gateway_name, "game","ReqUpdateMatchScore", req)
end

function this.sendReqUpdateMatchState()
    local req = {}
    req.state = ProtoEnumCommon.AactiveGameState.AactiveGameState_UnCanJion
    this.sendNetMsg(loginInfo.gateway_name, "game","ReqUpdateMatchState", req)
    GlobalTimeManager.Instance.timerController:AddTimer(ExitTimeOutKey, 6000, 1, function ()
        print("消消-请求退出响应时间过长, 前端自我退出")
        NoticeManager.Instance:Dispatch(EliminateNoticeType.BackToPlatform)
    end)
end

function this.sendReqGameRank()
    local req = {}
    req.active_id = loginInfo.active_id
    req.page_index = 0
    req.num = 20
    this.sendNetMsg(loginInfo.gateway_name, "game","ReqGameRank", req)
end

--============================================ 接收 ==========================================
function this.onRspJoinMatch(protoBytes)
    local msg = this.decodeProtoBytes("game","RspJoinMatch", protoBytes)
    if msg.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        NoticeManager.Instance:Dispatch(EliminateNoticeType.MatchStart)
    else
        NoticeManager.Instance:Dispatch(CommonNoticeType.Exception)
    end
end

function this.onRspRoomMatchBegin(protoBytes)
    local msg = this.decodeProtoBytes("game","RspRoomMatchBegin", protoBytes)
    NoticeManager.Instance:Dispatch(EliminateNoticeType.MatchEnd, msg)
end

function this.onRspUpdateMatchState(protoBytes)
    local msg = this.decodeProtoBytes("game","RspUpdateMatchState", protoBytes)
    NoticeManager.Instance:Dispatch(EliminateNoticeType.BackToPlatform)
end

function this.onNotifyLoadingProgress(protoBytes)
    local msg = this.decodeProtoBytes("game","NotifyLoadingProgress", protoBytes)

end

function this.onNotifyLoadingFinish(protoBytes)
    local msg = this.decodeProtoBytes("game","NotifyLoadingFinish", protoBytes)
    NoticeManager.Instance:Dispatch(EliminateNoticeType.LoadComplete)
end

function this.onRspGameRank(protoBytes)
    local msg = this.decodeProtoBytes("game","RspGameRank", protoBytes)
    NoticeManager.Instance:Dispatch(EliminateNoticeType.GameOver, msg)
end