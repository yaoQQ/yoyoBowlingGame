---
--- Created by Lichongzhi.
--- DateTime: 2018\11\2 0009 14:48
--- 用于管理平台的游戏的登录

require "base:enum/NoticeType"
require "base:enum/proto/ProtoEnumGame"

GameLoginModule = BaseModule:new()
local this = GameLoginModule
this.moduleName = "GameLoginModule"


------------------------------注册由服务器发来的协议------------------------------
function this.initRegisterNet()
    this:registerNetMsg(ProtoEnumGame.MsgIdx.MsgIdxRspLoginLogin)
    this:registerNetMsg(ProtoEnumGame.MsgIdx.MsgIdxRspLogin)
end

------------------------------由服务器发来的协议响应------------------------------
function this.onNetMsgLister(protoID, protoBytes)
    if ProtoEnumGame.MsgIdx.MsgIdxRspLoginLogin == protoID then
        this.onRspLoginLogin(protoBytes)
    elseif ProtoEnumGame.MsgIdx.MsgIdxRspLogin == protoID then
        this.onRspLoginGateway(protoBytes)
    end
end

------------------------------注册通知------------------------------
function this:getRegisterNotificationList()
    if self.notificationList == nil then
        self.notificationList = {}
        -- 平台
        table.insert(self.notificationList, CommonNoticeType.Game_Login)
        table.insert(self.notificationList, CommonNoticeType.Exception)
    end
    return self.notificationList
end

function this:onNotificationLister(noticeType, notice)
    local switch = {
    -- 平台
        [CommonNoticeType.Game_Login] = function()
            self:onGame_Login(notice)
        end,
        [CommonNoticeType.Exception] = function()
            self:onGame_Exception(notice)
        end,
    }
    local fSwitch = switch[noticeType] --switch func
    if fSwitch then --key exists
        fSwitch() --do func
    else --key not found
        self:withoutRegistNotice(noticeType)--用于报错提醒
    end
end

local loginInfo = nil
function this:onGame_Login(notice)
    local obj = notice:GetObj()
    loginInfo = {}
    loginInfo = obj
    this.connectLoginServer()
end

function this:onGame_Exception(notice)
    this.showReconnect()
end

--============================================ 网络消息 ==========================================
function this.onRspLoginLogin(protoBytes)
    local msg = this.decodeProtoBytes("game","RspLoginLogin", protoBytes)
    if msg.result == ProtoEnumGame.LoginResult.LoginResultSuccess then
        loginInfo.gateway_ip = msg.gateway_domain
        loginInfo.gateway_port = msg.gateway_port
        loginInfo.gateway_token = msg.token
        loginInfo.gateway_login_signkey = msg.login_signkey
        loginInfo.gateway_server_time = msg.server_time
        this.connectGatewayServer()
    else
        printError("游戏登录-错误-"..msg.result)
        this.showReconnect()
    end
end

function this.onRspLoginGateway(protoBytes)
    local msg = this.decodeProtoBytes("game","RspLogin", protoBytes)
    if msg.result == ProtoEnumGame.LoginResult.LoginResultSuccess then
        ShowWaiting(false, "GameLogin")
        --print("loginInfo: "..table.tostring(loginInfo))
        NoticeManager.Instance:Dispatch(CommonNoticeType.Game_Enter, loginInfo)
    else
        printError("游戏登录-错误-"..msg.result)
        this.showReconnect()
    end
end

function this.showReconnect()
    ShowWaiting(false, "GameLogin")
    NetworkManager.Instance:Disconnect(loginInfo.login_name)
    NetworkManager.Instance:Disconnect(loginInfo.gateway_name)
    Alert.showAlertMsg(
            "网络连接错误",
            "",
            "退出",
            function()
                CSLoger.debug(Color.Yellow, "========游戏登录, 抛出游戏退出事件=========", GameManager.curGameId)
                NoticeManager.Instance:Dispatch(CommonNoticeType.Game_Exit)
                GameManager.exitGame(loginInfo.game_id)
            end
    )
end

--=============================连接服务器=============================
function this.connectLoginServer()
    --连接login服前先断开
    ShowWaiting(true, "GameLogin")
    NetworkManager.Instance:Disconnect(loginInfo.login_name)
	NetworkManager.Instance:RegisterLoginServer(loginInfo.login_name)
    NetworkManager.Instance:Connect(
            loginInfo.login_name,
            loginInfo.login_ip,
            loginInfo.login_port,
            function(result)
                if result == 0 then
                    print("游戏登录, 请求登录登录服")
                    local req = {}
                    req.player_id = loginInfo.player_id
                    req.token = loginInfo.token
                    req.active_id = loginInfo.active_id
                    this.sendNetMsg(loginInfo.login_name, "game","ReqLoginLogin", req)
                elseif result ~= 4 then --排除主动断开
                    this.showReconnect()
                end
            end
    )
end

function this.connectGatewayServer()
    print("游戏登录-连接gateway服")
    NetworkManager.Instance:Disconnect(loginInfo.login_name)
    NetworkManager.Instance:Disconnect(loginInfo.gateway_name)
    ShowWaiting(true, "GameLogin")
    NetworkManager.Instance:Connect(
            loginInfo.gateway_name,
            loginInfo.gateway_ip,
            loginInfo.gateway_port,
            function(result)
                if result == 0 then
                    local req = {}
                    req.player_id = loginInfo.player_id
                    req.token = loginInfo.gateway_token
                    req.sign_key = loginInfo.gateway_login_signkey
                    req.server_time = loginInfo.gateway_server_time
                    this.sendNetMsg(loginInfo.gateway_name, "game", "ReqLogin", req)
                else
                    print("游戏登录-连接gateway服失败：" .. result)
                    this.connectLoginServer()
                end
            end
    )
end
