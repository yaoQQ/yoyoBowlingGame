LoginDataProxy = {}

--是否使用token登录
LoginDataProxy.isTokenLogin = false

--是否连接上gateway服
LoginDataProxy.isConnectGateway = false

--是否登录上gateway服
LoginDataProxy.isLoginGateway = false

--是否已获取用户信息
LoginDataProxy.isGetUserInfo = false

--游客账号
LoginDataProxy.guestAccount = ""

--个人登录信息
LoginDataProxy.gatewayIP = ""
LoginDataProxy.gatewayPort = 0
LoginDataProxy.playerId = 0			-- 玩家ID
LoginDataProxy.accountName = ""		-- 玩家账号
LoginDataProxy.serverTime = 0		-- 服务器时间(这个是登录时的服务器时间，仅用于登录验证，不用于对时)
LoginDataProxy.loginSignkey = ""	-- 登录signkey
LoginDataProxy.token = ""			-- 登录token

local LOGIN_CACHE_KEY = "LOGIN_CACHE_2"

function LoginDataProxy.saveLoginInfo(accountName, playerId, token)
	local loginInfo = {}
	loginInfo.ip = GameConfig.loginIP
	loginInfo.port = GameConfig.loginPort
	loginInfo.accountName = accountName
	loginInfo.playerId = playerId
	loginInfo.token = token
	PlayerPrefs.SetString(LOGIN_CACHE_KEY, JsonUtil.encode(loginInfo))
end

function LoginDataProxy.getLoginInfo()
	local loginInfo = PlayerPrefs.GetString(LOGIN_CACHE_KEY, "")
	if loginInfo == "" then
		return nil
	end
	return JsonUtil.decode(PlayerPrefs.GetString(LOGIN_CACHE_KEY, ""))
end

function LoginDataProxy.logout()
	--是否使用token登录
	LoginDataProxy.isTokenLogin = false
	--是否连接上gateway服
	LoginDataProxy.isConnectGateway = false
	--是否登录上gateway服
	LoginDataProxy.isLoginGateway = false
	--是否已获取用户信息
	LoginDataProxy.isGetUserInfo = false
	--个人登录信息
	LoginDataProxy.gatewayIP = ""
	LoginDataProxy.gatewayPort = 0
	LoginDataProxy.playerId = 0			-- 玩家ID
	LoginDataProxy.accountName = ""		-- 玩家账号
	LoginDataProxy.serverTime = 0		-- 服务器时间(这个是登录时的服务器时间，仅用于登录验证，不用于对时)
	LoginDataProxy.loginSignkey = ""	-- 登录signkey
	LoginDataProxy.token = ""			-- 登录token
	
	--断开连接
	NetworkManager.Instance:Disconnect(GameConfig.ServerName.MainGateway)
	--清除登录缓存
	PlayerPrefs.SetString(LOGIN_CACHE_KEY, "")
	
	ViewManager.closeAll()
	
	if IS_UNITY_EDITOR then
		--显示上方状态栏
		ViewManager.open(UIViewEnum.StatusbarView)
	end
	ViewManager.open(UIViewEnum.LoginView, nil, function ()
		ViewManager.open(UIViewEnum.Platform_Global_Shop_View)
	end)
	
	--各个模块监听该通知，处理数据卸载或初始化
	NoticeManager.Instance:Dispatch(NoticeType.Logout)
end