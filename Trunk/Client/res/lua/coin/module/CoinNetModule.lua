
CoinNetModule = BaseModule:new()
local this = CoinNetModule
this.moduleName = "CoinNetModule"
local loginInfo = nil
local ExitTimeOutKey = "CoinOverTimeOut"    -- 退出游戏请求超时计时, 超时之后前端直接退出

function this.InitLoginInfo(info)
    loginInfo = info
end

--初始化net侦听
function this.initRegisterNet()
    this:registerNetMsg(ProtoEnumGame.MsgIdx.MsgIdxRspJoinMatch)
    this:registerNetMsg(ProtoEnumGame.MsgIdx.MsgIdxRspUpdateMatchState)
    this:registerNetMsg(ProtoEnumGame.MsgIdx.MsgIdxRspGameRank)

end

function this.onNetMsgLister(protoID, protoBytes)
    if GameManager.curGameId ~= EnumGameID.Coin then
        return
    end
    if ProtoEnumGame.MsgIdx.MsgIdxRspJoinMatch == protoID then
        this.onRspJoinMatch(protoBytes)
    elseif ProtoEnumGame.MsgIdx.MsgIdxRspUpdateMatchState == protoID then
        this.onRspUpdateMatchState(protoBytes)
    elseif ProtoEnumGame.MsgIdx.MsgIdxRspGameRank == protoID then
        this.RspGameRank(protoBytes)
    end
end

function this:getRegisterNotificationList()
    if self.notificationList == nil then
        self.notificationList = {}
    end
    return self.notificationList
end

function this:onNotificationLister(noticeType, notice)
    local switch = {

    }
    local fSwitch = switch[noticeType] --switch func
    if fSwitch then --key exists
        fSwitch() --do func
    else --key not found
        self:withoutRegistNotice(noticeType)--用于报错提醒
    end
end

function this:OnExitGame()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(ExitTimeOutKey)
    if loginInfo ~= nil then
        NetworkManager.Instance:Disconnect(loginInfo.gateway_name)
    end
end
--=========================发送消息=======================

function this.sendReqJoinMatch(match_type, active_id, player_id)
    local req = {}
    req.match_type = match_type
    req.active_id = active_id
    req.player_id = player_id
    this.sendNetMsg(loginInfo.gateway_name, "game","ReqJoinMatch", req)
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
        print("抢金币-请求退出响应时间过长, 前端自我退出")
        NoticeManager.Instance:Dispatch(CoinNoticeType.BackToPlatform)
    end)
end

function this.ReqGameRank()
    local req = {}
    req.active_id = loginInfo.active_id
    req.page_index = 0
    req.num = 20
    this.sendNetMsg(loginInfo.gateway_name, "game","ReqGameRank", req)
end

--=================================收到消息=========================

function this.onRspJoinMatch(protoBytes)
    local msg = this.decodeProtoBytes("game","RspJoinMatch", protoBytes)
    if msg.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        NoticeManager.Instance:Dispatch(CoinNoticeType.LoadStart)
    else
        NoticeManager.Instance:Dispatch(CommonNoticeType.Exception)
    end
end

function this.onRspUpdateMatchState(protoBytes)
    local msg = this.decodeProtoBytes("game","RspUpdateMatchState", protoBytes)
    NoticeManager.Instance:Dispatch(CoinNoticeType.BackToPlatform)
end

function this.RspGameRank(protoBytes)
    local msg = this.decodeProtoBytes("game","RspGameRank", protoBytes)
    ViewManager.open(UIViewEnum.Coin_OverView, nil, function ()
        if msg.player_rank_info == nil then
            return
        end
        CoinOverView:showOverView(msg.player_rank_info.rank)
    end)
end