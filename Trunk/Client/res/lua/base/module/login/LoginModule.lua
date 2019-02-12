require "base:enum/NoticeType"
require "base:enum/PlatformNoticeType"

LoginModule = BaseModule:new()
local this = LoginModule
this.moduleName = "Login"

------------------------------注册由服务器发来的协议------------------------------
function this.initRegisterNet()
    this.netFuncList = {}
	this:AddNetLister(ProtoEnumLogin.MsgIdx.MsgIdxRspAliPaySign, this.onRspAliPaySign)
    this:AddNetLister(ProtoEnumLogin.MsgIdx.MsgIdxRspLogin, this.onRspLogin)
    this:AddNetLister(ProtoEnumLogin.MsgIdx.MsgIdxRspSMSAuth, this.onRspSMSAuth)
    this:AddNetLister(ProtoEnumLogin.MsgIdx.MsgIdxRspRegister, this.onRspRegister)
    this:AddNetLister(ProtoEnumLogin.MsgIdx.MsgIdxRspResetPwd, this.onRspResetPwd)
    this:AddNetLister(ProtoEnumLogin.MsgIdx.MsgIdxNotifyLoginInfo, this.onNotifyLoginInfo)

	this:AddNetLister(ProtoEnumGateway.MsgIdx.MsgIdxRspLogin, this.onRspGatewayLogin)
    this:AddNetLister(ProtoEnumGateway.MsgIdx.MsgIdxRspHeartbeat, this.onRspHeartbeat)
    this:AddNetLister(ProtoEnumGateway.MsgIdx.MsgIdxNotifyLogout, this.onNotifyLogout)
end

------------------------------由服务器发来的协议响应------------------------------

function this.onNetMsgLister(protoID, protoBytes)
    local nfSwitch = this.netFuncList[protoID]
    if nfSwitch then
        nfSwitch(protoBytes)
    else
        this:withoutRegistNotice(protoID)
    end
end
------------------------------注册通知------------------------------
function this:getRegisterNotificationList()
    if self.notificationList == nil then
        self.notificationList = {}
        self.switch = {}
		self:AddNotifictionLister(NoticeType.Focus_TimeOut, this.onFocus_TimeOut)
        self:AddNotifictionLister(NoticeType.Login_WxAuthSucceed, this.onLogin_WxAuthSucceed)
        self:AddNotifictionLister(NoticeType.Login_WxAuthFail, this.onLogin_WxAuthFail)
		self:AddNotifictionLister(NoticeType.Login_AlipayAuthSucceed, this.onLogin_AlipayAuthSucceed)
        self:AddNotifictionLister(NoticeType.Login_AlipayAuthFail, this.onLogin_AlipayAuthFail)
    end
    return self.notificationList
end

------------------------------通知响应------------------------------
function this.onFocus_TimeOut(noticeType, notice)
    if LoginDataProxy.isLoginGateway then
		ShowWaiting(true, "login")
		NetworkManager.Instance:Disconnect(GameConfig.ServerName.MainGateway)
		this.connectMainGatewayServer()
	end
end

function this.onLogin_WxAuthSucceed(noticeType, notice)
	local authCode = notice:GetObj()
	PlatformSDK.wxAuthSucceed(authCode)
end
function this.onLogin_WxAuthFail(noticeType, notice)
	local errCode = notice:GetObj()
	PlatformSDK.wxAuthFail(errCode)
end

function this.onLogin_AlipayAuthSucceed(noticeType, notice)
	local authCode = notice:GetObj()
	PlatformSDK.aliPayAuthSucceed(authCode)
end
function this.onLogin_AlipayAuthFail(noticeType, notice)
	local errCode = notice:GetObj()
	PlatformSDK.aliPayAuthFail(errCode)
end

------------------------------发协议------------------------------

--请求支付宝授权信息
function this.sendReqAliPaySign()
    local req = {}
    this.sendNetMsg(GameConfig.ServerName.MainLogin, "login", "ReqAliPaySign", req)
end

--请求登录MainLogin
function this.sendReqMainLogin(login_type, account_name, player_id, pwd)
    local req = {}
    req.login_type = login_type
    req.account_name = account_name
    req.device_id = DeviceUtil.GetDeviceUniqueIdentifier()
	if IS_ANDROID then
		req.device_os = ProtoEnumCommon.DeviceOS.DeviceOS_Android
	elseif IS_IOS then
		req.device_os = ProtoEnumCommon.DeviceOS.DeviceOS_IOS
	else
		req.device_os = ProtoEnumCommon.DeviceOS.DeviceOS_Other
	end
    --TODO
    req.player_id = player_id
    if login_type == ProtoEnumCommon.LoginType.LoginTypeMobile then
        req.pwd = UtilMethod.GetMD5HashFromString(pwd)
    else
        req.pwd = pwd
    end
	if UtilMethod.channel == nil then
		req.line_no = "00000000"
	else
		req.line_no = tostring(UtilMethod.channel)
	end
    this.sendNetMsg(GameConfig.ServerName.MainLogin, "login", "ReqLogin", req)
end

--请求短信验证码
function this.sendReqMainSMSAuth(phone_number, auth_type)
    local req = {}
    req.phone_number = phone_number
    req.type = auth_type
    this.sendNetMsg(GameConfig.ServerName.MainLogin, "login", "ReqSMSAuth", req)
end

--请求注册
function this.sendReqRegister(tel, verify_code, pwd)
    local req = {}
    req.tel = tel
    req.verify_code = verify_code
    req.pwd = UtilMethod.GetMD5HashFromString(pwd)
    if UtilMethod.channel == nil then
		req.line_no = "00000000"
	else
		req.line_no = tostring(UtilMethod.channel)
	end
    this.sendNetMsg(GameConfig.ServerName.MainLogin, "login", "ReqRegister", req)
end

--请求重置密码
function this.sendReqResetPwd(account_name, verify_code, pwd)
    local req = {}
    req.account_name = account_name
    req.verify_code = verify_code
    req.pwd = UtilMethod.GetMD5HashFromString(pwd)
    this.sendNetMsg(GameConfig.ServerName.MainLogin, "login", "ReqResetPwd", req)
end

--请求登录gateway
function this.sendReqMainGatewayLogin(player_id, account_name, server_time, sign_key, token)
    local req = {}
    req.player_id = player_id
    req.account_name = account_name
    req.server_time = server_time
    req.sign_key = sign_key
    req.token = token
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "gateway", "ReqLogin", req)
end

--请求心跳
function this.sendReqHeartbeat()
    local req = {}
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "gateway", "ReqHeartbeat", req)
end

------------------------------收协议------------------------------

function this.onRspAliPaySign(protoBytes)
    local rsp = this.decodeProtoBytes("login", "RspAliPaySign", protoBytes)
	
	if IS_UNITY_EDITOR then
		showFloatTips("PC版不支持支付宝登录")
		ShowWaiting(false, "login")
		return
	end
	
	PlatformSDK.aliPayAuth(rsp.sign)
	
	ShowWaiting(false, "login")
end

function this.onRspLogin(protoBytes)
    local rsp = this.decodeProtoBytes("login", "RspLogin", protoBytes)
    if rsp.result == ProtoEnumLogin.LoginResult.LoginResultSuccess then
        --保存token
        LoginDataProxy.saveLoginInfo(rsp.account_name, rsp.player_id, rsp.token)
    else
		ShowWaiting(false, "login")
        if LoginDataProxy.isTokenLogin then
            printDebug("token登录失败：" .. rsp.result)
            ViewManager.open(UIViewEnum.LoginView)
            ViewManager.open(UIViewEnum.BgView)
        else
            if rsp.result == ProtoEnumLogin.LoginResult.LoginResultErrInternal then
                showFloatTips("服务器内部错误")
            elseif rsp.result == ProtoEnumLogin.LoginResult.LoginResultErrAlready then
                showFloatTips("该帐号已经登录")
            elseif rsp.result == ProtoEnumLogin.LoginResult.LoginResultErrInvalidAuthCode then
                showFloatTips("无效的授权码")
            elseif rsp.result == ProtoEnumLogin.LoginResult.LoginResultErrAuthFailed then
                showFloatTips("第三方授权错误")
            elseif rsp.result == ProtoEnumLogin.LoginResult.LoginResultErrTokenExpired then
                printError("没有保存RefreshToken或已失效(此时前端需重新拉起微信请求或者手机号码验证)")
            elseif rsp.result == ProtoEnumLogin.LoginResult.LoginResultErrInvalidAccount then
                showFloatTips("无效的帐号")
            elseif rsp.result == ProtoEnumLogin.LoginResult.LoginResultErrInvalidPhoneNumber then
                showFloatTips("无效的手机号码")
            elseif rsp.result == ProtoEnumLogin.LoginResult.LoginResultErrInvalidPwd then
                showFloatTips("密码错误")
            elseif rsp.result == ProtoEnumLogin.LoginResult.LoginResultInvalidToken then
                printError("无效的Token")
            end
        end
    end
    LoginDataProxy.isTokenLogin = false
end

function this.onRspSMSAuth(protoBytes)
	ShowWaiting(false, "login")
    local rsp = this.decodeProtoBytes("login", "RspSMSAuth", protoBytes)
    if rsp.result == ProtoEnumLogin.SMSAuthResult.SMSAuthResultSuccess then
        NoticeManager.Instance:Dispatch(NoticeType.Login_SMSAuthSucceed, rsp)
    elseif rsp.result == ProtoEnumLogin.SMSAuthResult.SMSAuthResultErr then
        showFloatTips("验证码获取失败")
    elseif rsp.result == ProtoEnumLogin.SMSAuthResult.SMSAuthResultErrInvalidNumber then
        showFloatTips("无效的手机号码")
    elseif rsp.result == ProtoEnumLogin.SMSAuthResult.SMSAuthResultErrExist then
        showFloatTips("该手机号码已被注册")
    elseif rsp.result == ProtoEnumLogin.SMSAuthResult.SMSAuthResultErrNotExist then
        showFloatTips("该手机号码尚未注册")
    elseif rsp.result == ProtoEnumLogin.SMSAuthResult.SMSAuthResultErrNotTime then
        showFloatTips("请求太频繁，请稍后再试")
    end
end

function this.onRspRegister(protoBytes)
    local rsp = this.decodeProtoBytes("login", "RspRegister", protoBytes)
	if rsp.result ~= ProtoEnumLogin.ReqRegisterResult.ReqRegisterResultSuccess then
		ShowWaiting(false, "login")
	end
    if rsp.result == ProtoEnumLogin.ReqRegisterResult.ReqRegisterResultSuccess then
        NoticeManager.Instance:Dispatch(NoticeType.Login_RegisterSucceed, rsp)
    elseif rsp.result == ProtoEnumLogin.ReqRegisterResult.ReqRegisterResultFail then
        showFloatTips("注册失败")
    elseif rsp.result == ProtoEnumLogin.ReqRegisterResult.ReqRegisterResultExisted then
        showFloatTips("该手机号码已被注册")
    elseif rsp.result == ProtoEnumLogin.ReqRegisterResult.ReqRegisterResultInvalidVerfyCode then
		showFloatTips("验证码错误")
    end
end

function this.onRspResetPwd(protoBytes)
    local rsp = this.decodeProtoBytes("login", "RspResetPwd", protoBytes)
	if rsp.result ~= ProtoEnumLogin.ReqResetPwdResult.ReqResetPwdResultSuccess then
		ShowWaiting(false, "login")
	end
    if rsp.result == ProtoEnumLogin.ReqResetPwdResult.ReqResetPwdResultSuccess then
        NoticeManager.Instance:Dispatch(NoticeType.Login_ResetSucceed, rsp)
    elseif rsp.result == ProtoEnumLogin.ReqResetPwdResult.ReqResetPwdResultFail then
        showFloatTips("重置密码失败")
    elseif rsp.result == ProtoEnumLogin.ReqResetPwdResult.ReqResetPwdResultInvalidVerfyCode then
		showFloatTips("验证码错误")
    elseif rsp.result == ProtoEnumLogin.ReqResetPwdResult.ReqResetPwdResultInvalidAccount then
        showFloatTips("该手机号码尚未注册")
    elseif rsp.result == ProtoEnumLogin.ReqResetPwdResult.ReqResetPwdResultPwdTooShort then
		showFloatTips("密码太短，密码长度必须为8~16位")
    elseif rsp.result == ProtoEnumLogin.ReqResetPwdResult.ReqResetPwdResultPwdTooLong then
		showFloatTips("密码太长，密码长度必须为8~16位")
    end
end

function this.onNotifyLoginInfo(protoBytes)
    local rsp = this.decodeProtoBytes("login", "NotifyLoginInfo", protoBytes, token)
    LoginDataProxy.playerId = rsp.player_id
    LoginDataProxy.gatewayIP = rsp.gateway_domain
    LoginDataProxy.gatewayPort = rsp.gateway_port
    LoginDataProxy.serverTime = rsp.server_time
    LoginDataProxy.loginSignkey = rsp.login_signkey
    LoginDataProxy.token = rsp.token
	
    --断开login，连接gateway
    NetworkManager.Instance:Disconnect(GameConfig.ServerName.MainLogin)
    this.connectMainGatewayServer()
end

function this.onRspGatewayLogin(protoBytes)
    require "base:module/global/view/PlatformGlobalView"
    local rsp = this.decodeProtoBytes("gateway", "RspLogin", protoBytes)
	if rsp.result ~= ProtoEnumGateway.LoginResult.LoginResultSuccess then
		ShowWaiting(false, "login")
	end
    if rsp.result == ProtoEnumGateway.LoginResult.LoginResultSuccess then
        LoginDataProxy.isLoginGateway = true
        ViewManager.close(UIViewEnum.SelectServerView)
        ViewManager.close(UIViewEnum.LoginView)

        NoticeManager.Instance:Dispatch(NoticeType.Login_LoginGatewaySucceed)

        ViewManager.open(UIViewEnum.BgView)
    elseif rsp.result == ProtoEnumGateway.LoginResult.LoginResultErrInternal then
        showFloatTips("服务器内部错误")
    elseif rsp.result == ProtoEnumGateway.LoginResult.LoginResultErrInvalidAccount then
        showFloatTips("无效的账号")
    elseif rsp.result == ProtoEnumGateway.LoginResult.LoginResultErrInvalidSignKey then
        showFloatTips("无效的登录Key")
    elseif rsp.result == ProtoEnumGateway.LoginResult.LoginResultErrDuplicateLogin then
        showFloatTips("重复登录")
    end
end

function this.onRspHeartbeat(protoBytes)
    local rsp = this.decodeProtoBytes("gateway", "RspHeartbeat", protoBytes)
    TimeManager.setHeartbeatServerTime(rsp.server_time)
end

function this.onNotifyLogout(protoBytes)
    --TODO
end

--=============================开始登录=============================

function this.startLoginMainLoginServer()
    local loginCache = LoginDataProxy.getLoginInfo()
    if loginCache == nil then
        ViewManager.open(UIViewEnum.LoginView)
        ViewManager.open(UIViewEnum.BgView)
    else
		printDebug("loginCache:" .. table.tostring(loginCache))
        local ip = loginCache.ip
        local port = loginCache.port
        local accountName = loginCache.accountName
        local playerId = loginCache.playerId
        local token = loginCache.token

        if ip ~= GameConfig.loginIP or port ~= GameConfig.loginPort or token == nil then
            ViewManager.open(UIViewEnum.LoginView)
            ViewManager.open(UIViewEnum.BgView)
        else
            printDebug("token登录")
            LoginDataProxy.isTokenLogin = true
            ShowWaiting(true, "login")
            LoginModule.connectMainLoginServer(
                function()
                    LoginModule.sendReqMainLogin(ProtoEnumCommon.LoginType.LoginTypeToken, accountName, playerId, token)
                end
            )
        end
    end
end

--=============================连接服务器=============================
function this.connectMainLoginServer(succeedcallBack)
    --连接login服前先断开
    NetworkManager.Instance:Disconnect(GameConfig.ServerName.MainLogin)

    --[[--测试包禁止连接正式服
    if IS_TEST_SERVER and GameConfig.loginIP == GameConfig.loginIP3 and GameConfig.loginPort == GameConfig.loginPort3 then
        Alert.showAlertMsg(nil, "测试包禁止连接正式服", "确定")
        return
    end--]]

    printDebug("连接login服")
	NetworkManager.Instance:RegisterLoginServer(GameConfig.ServerName.MainLogin)
    NetworkManager.Instance:Connect(
        GameConfig.ServerName.MainLogin,
        GameConfig.loginIP,
        GameConfig.loginPort,
        function(result)
            if result == 0 then
                printDebug("连接login服成功")
                if succeedcallBack ~= nil then
                    succeedcallBack()
                end
		elseif result == 2 or result == 3 then --排除主动断开
                printDebug("连接login服失败：" .. result)
                ShowWaiting(false, "login")
                Alert.showAlertMsg(nil, "网络连接错误", "确定")
            end
        end
    )
end

function this.connectMainGatewayServer()
    printDebug("连接gateway服")
    NetworkManager.Instance:Connect(
        GameConfig.ServerName.MainGateway,
        LoginDataProxy.gatewayIP,
        LoginDataProxy.gatewayPort,
        function(result)
            if result == 0 then
                --NoticeManager.Instance:Dispatch(NoticeType.Socket_Connect_Succeed)
                printDebug("连接gateway服成功")
				LoginDataProxy.isConnectGateway = true
                TimeManager.startHeartbeat()
                this.sendReqMainGatewayLogin(
                    LoginDataProxy.playerId,
                    LoginDataProxy.accountName,
                    LoginDataProxy.serverTime,
                    LoginDataProxy.loginSignkey,
                    LoginDataProxy.token
                )
            elseif result == 2 or result == 3 then --排除主动断开
                printDebug("连接gateway服失败：" .. result)
                ShowWaiting(false, "login")
				if GameManager.curGameId <= 0 then
					--没有进入游戏，才弹出断线提示框
					printDebug("弹出断线提示框")
					Alert.showVerifyMsg(
						nil,
						"网络连接错误",
						"重新连接",
						function()
							ShowWaiting(true, "login")
							this.connectMainGatewayServer()
						end,
						"退出",
						function()
							NoticeManager.Instance:Dispatch(NoticeType.Normal_QuitGame)
						end
					)
				else
					--在游戏中就1秒后重连
					GlobalTimeManager.Instance.timerController:AddTimer("reconnectInGame", 1000, 1, this.connectMainGatewayServer)
				end
                NoticeManager.Instance:Dispatch(NoticeType.Socket_Error)
			elseif result == 1 then --已经连接
				ShowWaiting(false, "login")
            end
        end
    )
end
