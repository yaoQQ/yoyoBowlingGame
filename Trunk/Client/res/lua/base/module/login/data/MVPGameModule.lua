

--商店模块
MVPGameModule = BaseModule:new()
local this = MVPGameModule
this.moduleName = "MVPGameModule"

------------------------------注册由Java服务器发来的协议------------------------------
function this.initRegisterNet()
    this.netFuncList = {}
	--注册消息


    this:AddMVPJsonNetLister(JavaDataProtocol.reportGameResultRsp, this.acceptGameResultRsp)
	this:AddMVPJsonNetLister(JavaDataProtocol.startGameRsp, this.acceptStartGameRsp)
	
	--游戏包更新安装相关
	this:AddMVPJsonNetLister(JavaDataProtocol.apkVersionRsp, this.acceptApkVersionRsp)---发送APK版本号 返回
	this:AddMVPJsonNetLister(JavaDataProtocol.upgradeApkRsp, this.acceptUpdateApkRsp)--获取APK下载连接 返回
	this:AddMVPJsonNetLister(JavaDataProtocol.installApkRsp, this.acceptInstallApkRsp)---安装APK 返回
	
	
--[[	this:AddMVPJsonNetLister(JavaDataProtocol.cargolaneRsp, this.acceptCargolaneRsp)
	
	this:AddMVPJsonNetLister(JavaDataProtocol.goodsListRsp, this.acceptGoodsList)--]]
end

---兼容老框架
function this.onNetMsgLister(protoID, jsonData)
    local nfSwitch = this.netFuncList[protoID]
    if nfSwitch then
        nfSwitch(jsonData)
    else
        this:withoutRegistNotice(protoID)
    end
end

--- 注册的消息，返回数据
function this.onMVPJsonMsgLister(protoID, jsonData)
    local nfSwitch = this.netFuncList[protoID]
    if nfSwitch then
        nfSwitch(jsonData)
    else
        this:withoutRegistNotice(protoID)
    end
end

------------------------------注册通知，或发送数据到java端------------------------------
function this:getRegisterNotificationList()
    if self.notificationList == nil then
        self.notificationList = {}
        self.switch = {}
        self:AddNotifictionLister(MVPGlobalNoticeType.reportGameResultReq, this.reportGameResultReq)
		self:AddNotifictionLister(MVPGlobalNoticeType.installApkReq, this.reqInstallApkReq)
		self:AddNotifictionLister(MVPGlobalNoticeType.upgradeApkReq, this.reqUpgradeApkReq)
    end
    return self.notificationList
end

------------------------------发协议------------------------------

--发送数据到java端
function this.SendDataToJava(data)
   -- local rep = notice:GetObj()
    this.sendMVPToJavaMsg(JavaDataProtocol.loginReq,data)
	--showTopTips("onSendDataToJava="..notice)
end

---获取APK下载连接 请求
function this.SendUpdateApkReq(versionStr)
	printDebug("请求 获取APK下载连接 ")
	requestTable={oper="upgradeApkReq",param={version=versionStr}}
	this.sendMVPToJavaMsg(JavaDataProtocol.upgradeApkReq,requestTable)
end
---安装APK 请求
function this.SendInstallApkReq(localApkPath)
	printDebug("请求 安装APK  localApkPath="..tostring(localApkPath))
	requestTable={oper="installApkReq",param={filePath=localApkPath}}
	this.sendMVPToJavaMsg(JavaDataProtocol.installApkReq,requestTable)
end

---发送APK版本号 返回
function this.acceptApkVersionRsp(jsonData)
	jsonToTable = JsonUtil.decode(jsonData)
	printDebug("@@@@@@@@@@@@@@@MVPGameModule  获取后台APK版本号="..table.tostring(jsonToTable))
	if jsonToTable==nil then
		return
	end
	if jsonToTable.ret==1 then
		if jsonToTable.result==nil or jsonToTable.result[1]==nil then
			showTopTips("获取后台APK版本号为空！")
			return
		end
		local version = jsonToTable.result[1].version
		NoticeManager.Instance:Dispatch(MVPGlobalNoticeType.apkVersionRsp, version)
	elseif jsonToTable.ret==0 then
		showTopTips("获取后台APK版本号失败")
	end
end
--获取APK下载连接 返回
function this.acceptUpdateApkRsp(jsonData)
	jsonToTable = JsonUtil.decode(jsonData)
	printDebug("@@@@@@@@@@@@@@@MVPGameModule 获取APK下载连接 返回="..table.tostring(jsonToTable))
	if jsonToTable==nil then
		return
	end
	if jsonToTable.ret==1 then
		if jsonToTable.result==nil or jsonToTable.result[1]==nil then
			showTopTips("获取APK下载连接为空！！")
			return
		end
		--showTopTips("支付成功，进入游戏")
		local fileUrl = jsonToTable.result[1].fileUrl--//下载链接
		local filePath = jsonToTable.result[1].filePath--安装路径  到文件夹 
		printDebug("@@@@@@@@@@@@@@@MVPGameModule 获取APK下载连接 fileUrl="..tostring(fileUrl))
		printDebug("@@@@@@@@@@@@@@@MVPGameModule 获取APK安装路径 filePath="..tostring(filePath))
		
		NoticeManager.Instance:Dispatch(MVPGlobalNoticeType.apkFileSavePath, filePath)
		NoticeManager.Instance:Dispatch(MVPGlobalNoticeType.apkFileUrl, fileUrl)
	elseif jsonToTable.ret==0 then
		showTopTips("安装APK失败")
	end
	
	
end
---安装APK 返回
function this.acceptInstallApkRsp(jsonData)
	jsonToTable = JsonUtil.decode(jsonData)
	printDebug("@@@@@@@@@@@@@@@MVPGameModule  安装APK 返回="..table.tostring(jsonToTable))
	if jsonToTable==nil then
		return
	end
	if jsonToTable.ret==1 then
		showTopTips("安装APK成功")
	elseif jsonToTable.ret==0 then
		showTopTips("安装APK失败")
	end
	
end



--游戏结果上报  请求 (游戏结束时调用)
--gameLevelP// 玩家当前闯关号(1,2,3)
--orderIdP//游戏关联订单id
--resP// 游戏结果（0 输 1 赢）
--C# 游戏出啊过来的请求刷新Java服务端数据
function this.reportGameResultReq(noticeType, notice)
    local rep = notice:GetObj()
	jsonToTable = JsonUtil.decode(rep)
	printDebug("@@@@@@@@  jsonToTable="..table.tostring(jsonToTable))
	local currGameLevel = jsonToTable.gameLevel
	local ispass = jsonToTable.res
	local requestTable={gameLevel=currGameLevel,res=ispass}
	CommonUI.ShowGameResult(requestTable)
end
function this.reqUpgradeApkReq(noticeType, notice)
    local version = notice:GetObj()
	this.SendUpdateApkReq(version)
end

function this.reqInstallApkReq(noticeType, notice)
    local localPath = notice:GetObj()
	this.SendInstallApkReq(localPath)
end


--------------------------------------------收到协议(服务端发出)-------------------------------

--广播消息
function this.acceptJsonData(jsonData)
	printDebug("@@@@@@@@@@@@@@@MVPGameModule  acceptJsonData="..tostring(jsonData))
--[[	tableToJson =JsonUtil.encode(test)      --table转json
	printDebug("@@@@showGame() json jsonTest="..tostring(tableToJson))--]]
	jsonToTable = JsonUtil.decode(jsonData)
    NoticeManager.Instance:Dispatch(MVPGlobalNoticeType.GetJavaData, jsonData)
end


--[[{
    "oper" : "startGameRsp",    
    "ret" : <int> // 0 失败 1 成功
    "result" : [
    ]       
}--]]
--返回(网络请求失败)
function this.acceptGameResultRsp(jsonData)
	
	--tablePara={oper="startGameRsp",ret=1,result={}}
	jsonToTable = JsonUtil.decode(jsonData)
	if jsonToTable==nil then
		return
	end
	printDebug("@@@@@@@@@@@@@@@MVPGameModule  acceptGameResultRsp jsonToTable="..table.tostring(jsonToTable))
	if jsonToTable.ret==1 then
		showTopTips("上传游戏数据成功")
	elseif jsonToTable.ret==0 then
		showTopTips("上传游戏数据失败")
	end
  

end

--[[{
  ret = 1
  result = json.array: 00000001075216B0
  {
    [1] = json.object: 0000000107521DF0
    {
      orderId = 123
    }
  }
  oper = "startGameRsp"
}--]]
--游戏开始 返回 支付成功或失败
function this.acceptStartGameRsp(jsonData)
	jsonToTable = JsonUtil.decode(jsonData)
	printDebug("@@@@@@@@@@@@@@@MVPGameModule  acceptStartGameRsp jsonToTable="..table.tostring(jsonToTable))
	if jsonToTable==nil then
		return
	end
	if jsonToTable.ret==1 then
		if jsonToTable.result==nil or jsonToTable.result[1]==nil then
			showTopTips("游戏支付订单为空！！进入游戏失败")
			return
		end
		--showTopTips("支付成功，进入游戏")
		local orlderId = jsonToTable.result[1].orderId   -- // 游戏关联订单id
		local gameGrade = jsonToTable.result[1].gameGrade  -- //游戏难度系数
		local gameControl = jsonToTable.result[1].gameControl--  1-控制,绝对不许通过游戏, 0-不控制,看玩家操作
		JavaDataManager.setOrlderId(orlderId)
		JavaDataManager.setGameGrade(gameGrade)
		JavaDataManager.setGameControl(gameControl)
		printDebug("@@@支付成功 订单 orlderId ="..tostring(orlderId))
		printDebug("@@@支付成功 难度 gameGrade ="..tostring(gameGrade))
		printDebug("@@@支付成功 控制 gameControl ="..tostring(gameControl))
		local gameData = PlatformUserProxy.getGameData()
		if gameData.gameType == LoginView.gameType.lipstickGame then
			--开始游戏倒计时界面
			ViewManager.open(UIViewEnum.MvpGame_Count_View,this.startLipstick)
		elseif gameData.gameType == LoginView.gameType.ballGame then
			local basketGameData ={}
			basketGameData.gameGrade = gameGrade
			basketGameData.gameControl = gameControl
			local basketGameDataStr =JsonUtil.encode(basketGameData)      --table转json
			NoticeManager.Instance:Dispatch(MVPGlobalNoticeType.GetMVPGameData,basketGameDataStr)
			
			SceneManager.Instance:LoadScene("LoadingScene")
		else
			printDebug("++++未知游戏类型")
		end
		
		MVPGameModule.getGameGodd()
	elseif jsonToTable.ret==0 then
		showTopTips("支付失败")
	end

end

function MVPGameModule.TestGamelevel()
		JavaDataManager.setOrlderId(123)
		JavaDataManager.setGameGrade(2)
		printDebug("@@@支付成功 orlderId ="..tostring(JavaDataManager.getOrlderId()))
		printDebug("@@@支付成功 gameGrade ="..tostring(JavaDataManager.getGameGrade()))
		NoticeManager.Instance:Dispatch(MVPGlobalNoticeType.GetMVPGameDificult, JavaDataManager.getGameGrade())
		SceneManager.Instance:LoadScene("LoadingScene")
end

function this.startLipstick()
	ViewManager.open(UIViewEnum.Lipstick_View)
end

function MVPGameModule.getGameGodd()
	 local baseData = PlatformUserProxy:GetInstance():getGameData()
		local goodStr=nil
		if baseData~=nil then
			 goodStr = JsonUtil.encode(baseData)
		end

		printDebug("@@@支付成功 orlderId ="..tostring(orlderId))
		printDebug("@@@支付成功 商品信息 ="..tostring(goodStr))
		
		NoticeManager.Instance:Dispatch(MVPGlobalNoticeType.StartMVPGame, goodStr)
end
