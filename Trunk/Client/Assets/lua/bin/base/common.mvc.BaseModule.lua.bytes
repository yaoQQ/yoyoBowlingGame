BaseModule={}
local this=BaseModule
function this:new()
    local o = {}
    setmetatable(o, self)
	self.__index = self
    return o
end


this.moduleName=""
this.notificationList=nil

function this:getModuleName()
	return self.moduleName
end

function this:getRegisterNotificationList()
	Loger.PrintError(self.ModuleName," 抽象方法需重写：getRegisterNotificationList")
end


function this:withoutRegistNotice( noticeType )
	Loger.PrintWarning(self.ModuleName," 注册通知缺少处理方法：",noticeType)
end

--通知(可重写兼容旧做法)
function this:onNotificationLister(noticeType,notice)
    local fSwitch = self.switch[noticeType] 
	if fSwitch then  
	   fSwitch(self,notice) 
	else 
	  	self:withoutRegistNotice(noticeType)
	end
end
--注册方法
function this:AddNotifictionLister(noticeType,HandlerFunc)
	if not noticeType then
		return printError("注册 noticeType 为空")
	end
	if not HandlerFunc then
		return printError("注册 Function 为空")
	end
	if self.switch[noticeType] then
		return printError("重复消息ID "..noticeType)
	end
	table.insert(self.notificationList,noticeType) 
	self.switch[noticeType]=HandlerFunc
end

--初始化net侦听
function this:initRegisterNet()
	Loger.PrintWarning(self.ModuleName," 抽象方法需重写：initRegisterNet")
end

--initRegisterNet中调用注册
function this:registerNetMsg(protoID)
	NetworkEventManager.Instance:RegisterEventHandler(protoID, self.onNetMsgLister)
end



function this:AddNetLister(protoId,HandlerFunc)
	if not protoId then
		return printError("注册 protoId 为空")
	end
	if not HandlerFunc then
		return printError("注册Net Function为空")
	end
	if self.netFuncList[protoId] then
		return printError("重复消息ID "..protoId)
	end
	self:registerNetMsg(protoId)
	self.netFuncList[protoId]=HandlerFunc
end
----需重写的侦听方法
function this.onNetMsgLister(protoID,protoBytes)
	Loger.PrintError(this.ModuleName," 抽象方法需重写：onNetMsgLister")
end


--发送net  protoBytes 可以为空
function this.sendNetMsg(serverName, spaceName, msgName, msg)
	if IS_UNITY_EDITOR or IS_TEST_SERVER then
		if msgName ~= "ReqHeartbeat" then
			printDebug("发送协议("..msgName..")："..table.tostring(msg))
		end
	end
	
	--退出登录后不再发协议
	if serverName == GameConfig.ServerName.MainGateway and not LoginDataProxy.isConnectGateway then
		printWarning("退出登录后不再发协议("..msgName..")")
		return
	end
	
	ProtobufManager.send(serverName, spaceName, msgName, msg)
end

function this.sendHttp(msgName, param, rspCallBack)
	--ProtobufManager.sendHttp(msgName, param, rspCallBack)
end

function this.sendHttpProtobuf(msgName, msg, rspCallBack)
	ProtobufManager.sendHttpProtobuf(msgName, msg, rspCallBack)
end

function this.encodeProtoBytes(spaceName, msgName, msg)
	return ProtobufManager.encode(spaceName, msgName, msg)
end

function this.decodeProtoBytes(spaceName, msgName, protoBytes)
	local msg = ProtobufManager.decode(spaceName, msgName, protoBytes)
	if IS_UNITY_EDITOR or IS_TEST_SERVER then
		if msgName ~= "RspHeartbeat" then
			printDebug("收到协议("..msgName..")："..table.tostring(msg))
		end
	end
	return msg
end

-------------------------------------MVPGameData--------------------
---注册（私有）：MVP的protoId消息（registerNetMsg）
function this:registerMVPJsonMsg(protoID)
	JavaNetWorkManager.Instance:RegisterEventHandler(protoID, self.onMVPJsonMsgLister)
end

---注册：接受protoId消息的json数据毁掉（AddNetLister）
function this:AddMVPJsonNetLister(protoId,HandlerFunc)
	if not protoId then
		return printError("注册 protoId 为空")
	end
	if not HandlerFunc then
		return printError("注册Net Function为空")
	end
	if self.netFuncList[protoId] then
		return printError("重复消息ID "..protoId)
	end
	self:registerMVPJsonMsg(protoId)
	self.netFuncList[protoId]=HandlerFunc
	printDebug("注册AddMVPJsonNetLister（） protoId="..tostring(protoId))
	printDebug("注册AddMVPJsonNetLister（） HandlerFunc="..tostring(HandlerFunc))
end
----需重写的侦听方法（onNetMsgLister）
function this.onMVPJsonMsgLister(protoID,jsonData)
	Loger.PrintError(this.ModuleName," 抽象方法需重写：onNetMsgLister")
end

---  发送消息到Java
--发送net  protoBytes 可以为空
function this.sendMVPToJavaMsg(protoID,tableData)
	if IS_UNITY_EDITOR or IS_TEST_SERVER then
		if protoID ~= "5" then
			--printDebug("发送协议("..protoID..")："..tostring(tableData))
		end
	end
	tableToJson =JsonUtil.encode(tableData)      --table转json
	printDebug("@@@sendMVPToJavaMsg发送协议  tableToJson"..tostring(tableToJson))
	NetworkManager.Instance:SendMsgToJavaMessage(protoID, tableToJson)
end