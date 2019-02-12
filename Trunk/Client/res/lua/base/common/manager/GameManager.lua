GameManager = {}

EnumGameID = {
    Eliminate = 1,
    Mahjong = 2,
    Bowling = 3,	--保龄球
    Jump = 4,
    Marbles = 5,
	Doodle = 6,
	Coin = 7,	-- 抢金币
	Animal = 8, -- 斗兽棋
}

EnumGameType = {
    Hall = 1,			--游戏大厅
    NormalMatch = 2,	--普通赛事
    OfficialMatch = 3,	--官方赛事
}

GameManager.curGameId = 0

-- 根据当前平台登录的服务器类型获取区url
local function getRegionUrl()
	local url = ""
	if GameConfig.loginIP == GameConfig.loginIPTest and GameConfig.loginPort == GameConfig.loginPortTest then
		url = "http://config.test.51e-sport.com:9000/api/game_config/get_config"
	elseif GameConfig.loginIP == GameConfig.loginIP2 and GameConfig.loginPort == GameConfig.loginPort2 then
		url = "http://config.beta.51e-sport.com:9000/api/game_config/get_config"
	elseif GameConfig.loginIP == GameConfig.loginIP3 and GameConfig.loginPort == GameConfig.loginPort3 then
		url = "http://config.ga.51e-sport.com:9000/api/game_config/get_config"
	else
		url = "http://config.dev.51e-sport.com:9000/api/game_config/get_config"
	end
	return url
end

-- Http发送计时器, 到时间没有回复时即认为是网络断开, 返回平台
local HttpTimerKey = "GameMangerHttp"
--初始化
function GameManager.init()
	--注册游戏包名
	CSGameProcessManager.RegisterPackageName(EnumGameID.Eliminate, "eliminate")
	CSGameProcessManager.RegisterPackageName(EnumGameID.Mahjong, "mahjonghul")
	CSGameProcessManager.RegisterPackageName(EnumGameID.Bowling, "bowling")
	CSGameProcessManager.RegisterPackageName(EnumGameID.Marbles, "marbles")
	CSGameProcessManager.RegisterPackageName(EnumGameID.Coin, "coin")
	CSGameProcessManager.RegisterPackageName(EnumGameID.Animal, "animal")
end

--检查下载
function GameManager.checkDownloadOrStartGame(gameId, gameType, shopId, roomId)
    ShowWaiting(true, "Game_CheckDownload")
    CSGameProcessManager.CheckDownload(
        gameId,
        function(size)
            printDebug("游戏需要下载的size:" .. size)
            ShowWaiting(false, "Game_CheckDownload")
            if size == 0 then
                GameManager.startGame(gameId, gameType, shopId, roomId)
            elseif size > 0 then
                local msg = {}
                msg.gameId = gameId
				msg.gameType = gameType
				msg.shopId = shopId
				msg.roomId = roomId
                msg.size = size
                ViewManager.open(UIViewEnum.Platform_Game_Update_View, msg)
            end
        end
    )
end

--进入游戏
function GameManager.enterGame(gameId, gameType, shopId, activeId)
    printDebug(string.format("进入游戏, curGameId = %s, curGameType = %s, curShopId = %s, curRoomId = %s)", gameId, gameType, shopId, activeId))
    if IS_INCLUDE_GAME then
        GameManager.startGame(gameId, gameType, shopId, activeId)
    else
        GameManager.checkDownloadOrStartGame(gameId, gameType, shopId, activeId)
    end

end

--开始游戏
function GameManager.startGame(gameId, gameType, shopId, activeId)
	GameManager.curGameId = gameId
	printDebug(string.format("开始游戏, curGameId = %s, curGameType = %s, curShopId = %s, activeId = %s)", gameId, gameType, shopId, activeId))
	
	ViewManager.saveStackAndCloseAllView()
	
	--隐藏状态栏
	if not IS_UNITY_EDITOR then
		PlatformSDK.showStatusBar(false,false)
	end

	if gameId == EnumGameID.Mahjong then
		require "mahjonghul:MH_Package"
		MH_Package:init(function ()
			local gameData = {}
			gameData.game_id = gameId
			gameData.token = LoginDataProxy.token
			gameData.player_id = LoginDataProxy.playerId
			gameData.game_type = gameType
			gameData.shop_id = shopId
			gameData.room_id = activeId
			NoticeManager.Instance:Dispatch(CommonNoticeType.Game_Enter, gameData)
			
			UtilMethod.SwitchScreenOrientation(true)
			
			--隐藏状态栏
			if not IS_UNITY_EDITOR then
				PlatformSDK.showStatusBar(false,false)
			end
		end)
	elseif gameId == EnumGameID.Marbles then
		require "marbles:BB_Package"
		BB_Package:init(function ()
			local gameData = {}
			gameData.game_id = gameId
			gameData.token = LoginDataProxy.token
			gameData.player_id = LoginDataProxy.playerId
			gameData.game_type = gameType
			gameData.shop_id = shopId
			gameData.room_id = activeId
			NoticeManager.Instance:Dispatch(CommonNoticeType.Game_Enter, gameData)
		end)
	elseif gameId == EnumGameID.Bowling then
		require "bowling:BowlingPackage"
		BowlingPackage:init(function ()
			local gameData = {}
			gameData.game_id = gameId
			gameData.token = LoginDataProxy.token
			gameData.player_id = LoginDataProxy.playerId
			gameData.game_type = gameType
			gameData.shop_id = shopId
			gameData.room_id = activeId
			NoticeManager.Instance:Dispatch(CommonNoticeType.Game_Enter, gameData)
		end)
	else
		local function firstToUpper(str)
			return (str:gsub("^%l", string.upper))
		end
		local name_e = TableBaseGameList.data[gameId].name_e
		local packName = string.format("%s%s", firstToUpper(name_e), "Package")
		local packagePath = string.format("%s:%s", name_e, packName)
		--print("packagePath: "..packagePath)
		local package = require(packagePath)
		if not package then
			printError("目标路径不存在, path: %s"..packagePath)
			GameManager.exitGame(gameId)
			return
		end
		local gameUrl = string.format("id=%s&srv_type=%s&srv_name=%s",1000,"game", gameId)
		local gameData = {}
		gameData.game_id = gameId
		gameData.token = LoginDataProxy.token
		gameData.player_id = LoginDataProxy.playerId
		gameData.game_type = gameType
		gameData.shop_id = shopId
		gameData.active_id = activeId
		gameData.login_name = string.format("%s%s", name_e, "login")
		gameData.gateway_name = string.format("%s%s", name_e, "gateway")
		ShowWaiting(true, "GameHttp")
		ProtobufManager.sendHttpUrl(getRegionUrl(), gameUrl, function (rsp)
			ShowWaiting(false, "GameHttp")
			if rsp == nil then
				printError("HTTP错误, 退出游戏")
				GlobalTimeManager.Instance.timerController:AddTimer(HttpTimerKey, 2000, 1, function ()
					GameManager.exitGame(gameId)
				end)
				return
			end
			if rsp.result == 200 then
				Application.targetFrameRate = 60
				gameData.login_ip = rsp.ip
				gameData.login_port = rsp.port
				package:init(function()
					NoticeManager.Instance:Dispatch(CommonNoticeType.Game_Login, gameData)
				end)
			else
				printError("请求登录信息错误, 退出游戏")
				GameManager.exitGame(gameId)
				return
			end
		end)
	end
end

function GameManager.onGameEnterFinish(notice, data)
    local tab = data:GetObj()
    printDebug(string.format("进入游戏完毕, curGameId = %s, gameMode = %s)", tab.gameId, tab.curActiveId))
end

--退出游戏
function GameManager.exitGame(gameId)
	--[[--容错
	if gameId == nil or gameId < 0 then
		printWarning("退出游戏id错误")
		gameId = GameManager.curGameId
	else
		if gameId ~= GameManager.curGameId then
			printError("退出游戏id错误")
		end
	end--]]
	
	GameManager.curGameId = 0
	Application.targetFrameRate = 30
	UtilMethod.SwitchScreenOrientation(false)
	
	--显示状态栏
	if IS_UNITY_EDITOR then
		ViewManager.open(UIViewEnum.StatusbarView)
	else
		PlatformSDK.showStatusBar(true,false)
	end
	GameManager.backPlatform(gameId)
	if NetworkManager.Instance:IsConnected(GameConfig.ServerName.MainGateway) == false then
		--如果平台断线了就重连
		printDebug("平台断线了，重连")
		ShowWaiting(true, "login")
		LoginModule.connectMainGatewayServer()
	end
end

function GameManager.backPlatform(gameId)
	ViewManager.closeAllViewAndRevertStack()
	NoticeManager.Instance:Dispatch(NoticeType.Game_Exit_Notice_View)
	
	if gameId == EnumGameID.Eliminate then
		CSResReleaseManager.ReleasePackage("eliminate")
	elseif gameId == EnumGameID.Mahjong then
		CSResReleaseManager.ReleasePackage("mahjonghul")
	elseif gameId == EnumGameID.Marbles then
		CSResReleaseManager.ReleasePackage("marbles")
	elseif gameId == EnumGameID.Doodle then
		CSResReleaseManager.ReleasePackage("doodle")
	elseif gameId == EnumGameID.Coin then
		CSResReleaseManager.ReleasePackage("coin")
	elseif gameId == EnumGameID.Animal then
		CSResReleaseManager.ReleasePackage("animal")
	elseif gameId == EnumGameID.Bowling then
		CSResReleaseManager.ReleasePackage("bowling")
	end
end