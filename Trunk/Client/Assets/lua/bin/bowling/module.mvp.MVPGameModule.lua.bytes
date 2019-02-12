require "bowling:module/mvp/data/JavaGlobalNotice"

--商店模块
MVPGameModule = BaseModule:new()
local this = MVPGameModule
this.moduleName = "MVPGameModule"



------------------------------注册由Java服务器发来的协议------------------------------
function this.initRegisterNet()
	printDebug("this.initRegisterNet()")
    this.netFuncList = {}
	--注册消息
	printDebug("MVPMGameModule this.initRegisterNet()")
	--[[printDebug("MVPMGameModule avaDataProtocol.sendInitScreen="..tostring(JavaDataProtocol.sendInitScreen))
	printDebug("MVPMGameModule JavaDataProtocol.SendStopPos="..tostring(JavaDataProtocol.SendStopPos))
	printDebug("MVPMGameModule JavaDataProtocol.sendChoosePerson="..tostring(JavaDataProtocol.sendChoosePerson))
	printDebug("MVPMGameModule JavaDataProtocol.sendPositionContent="..tostring(JavaDataProtocol.sendPositionContent))--]]

	
    this:AddMVPJsonNetLister(JavaDataProtocol.sendInitScreen, this.acceptInitScreenRsp)
	this:AddMVPJsonNetLister(JavaDataProtocol.SendStopPos, this.acceptStopPosRsp)
	
	--游戏包更新安装相关
	this:AddMVPJsonNetLister(JavaDataProtocol.sendChoosePerson, this.acceptChoosePersonRsp)---发送APK版本号 返回
	this:AddMVPJsonNetLister(JavaDataProtocol.sendPositionContent, this.acceptPositionContentRsp)--获取APK下载连接 返回
	this:AddMVPJsonNetLister(JavaDataProtocol.sendHear, this.acceptHearRsp)---安装APK 返回
	
	

	GlobalTimeManager.Instance.timerController:AddTimer("hertTick", 4000, -1, this.sendHeat)

end
------------------------------发协议------------------------------
--描述：客户端通知服务器开始发坐标的命令，主要字段有pid（协议号），screen（屏幕尺寸）
function MVPGameModule.sendInitScreen()
	printDebug("MVPGameModule.sendInitScreen() ")
	local requestTable={pid=1,screen={width=1080,height=1920}}
	this.sendMVPToJavaMsg(JavaDataProtocol.sendInitScreen,requestTable)
end

--描述：客户端通知服务器结束发坐标的命令，主要字段有pid（协议号）
function MVPGameModule.SendStopPos()
	printDebug("MVPGameModule.SendStopPos() ")
	local requestTable={pid=2}
	this.sendMVPToJavaMsg(JavaDataProtocol.SendStopPos,requestTable)
end

--描述：客户端发送给服务器，确定选中的人的id，服务器收到后，后续发送协议3到客户端的时候只会发该人的信息
function MVPGameModule.sendChoosePerson(personId)
	printDebug("MVPGameModule.sendChoosePerson() ")
	--personId=1
	local requestTable={pid=4,person_id=personId}
	this.sendMVPToJavaMsg(JavaDataProtocol.sendChoosePerson,requestTable)
end

--描述：客户端发送给服务器，确定选中的人的id，服务器收到后，后续发送协议3到客户端的时候只会发该人的信息
function MVPGameModule.sendHeat()
	--printDebug("MVPGameModule.sendHeat() ")
	--TimeManager.startHeartbeat
	local requestTable={pid=5,sequence=1,time=TimeManager.GetlocalTime()}
	this.sendMVPToJavaMsg(JavaDataProtocol.sendHear,requestTable)
end


-------------------收协议------------
function this.acceptInitScreenRsp(jsonData)
	jsonToTable = JsonUtil.decode(jsonData)
	printDebug("@@@@@@@@@@@@@@@acceptInitScreenRsp 返回="..table.tostring(jsonToTable))
	if jsonToTable==nil then
		return
	end
	if jsonToTable.result==1 then
		showTopTips("acceptInitScreenRsp 失败")
	elseif jsonToTable.result==0 then
		showTopTips("acceptInitScreenRsp 成功")
	end
end
function this.acceptStopPosRsp(jsonData)
	jsonToTable = JsonUtil.decode(jsonData)
	printDebug("@@@@@@@@@@@@@@@acceptStopPosRsp 返回="..table.tostring(jsonToTable))
	if jsonToTable==nil then
		return
	end
	if jsonToTable.result==0 then
		--showTopTips("acceptStopPosRsp 成功")
	elseif jsonToTable.result==1 then
		--showTopTips("acceptStopPosRsp 失败")
	end
end
function this.acceptChoosePersonRsp(jsonData)
	jsonToTable = JsonUtil.decode(jsonData)
	printDebug("@@@@@@@@@@@@@@@acceptChoosePersonRsp 返回="..table.tostring(jsonToTable))
	if jsonToTable==nil then
		return
	end
	if jsonToTable.result==0 then
		showTopTips("acceptChoosePersonRsp 成功")
	elseif jsonToTable.result==1 then
		showTopTips("acceptChoosePersonRsp 失败")
	end
end

this.testPos={}
this.count=0
function this.acceptPositionContentRsp(jsonData)
	jsonToTable = JsonUtil.decode(jsonData)
	--printDebug("@@@@@@@@@@@@@@@acceptPositionContentRsp 返回="..table.tostring(jsonToTable))
	if jsonToTable==nil then
		return
	end
	this.count =this.count+1
	if this.count>=2 then
		this.count=0
		--printDebug("<color='blue'>生成人物轨迹22222 this.testPos=="..table.tostring(jsonToTable).."</color>")
		PlayerScanPos.addPersonPos(jsonToTable)
		--printDebug("<color='blue'>生成人物轨迹22222 this.testPos=="..table.tostring(jsonToTable).."</color>")
	end
	--table.insert(this.testPos,rightPos)
	
	---printDebug("<color='blue'>生成人物轨迹22222 this.testPos=="..table.tostring(jsonToTable).."</color>")
end
function this.acceptHearRsp(jsonData)
	jsonToTable = JsonUtil.decode(jsonData)
	--printDebug("@@@@@@@@@@@@@@@acceptHearRsp 返回="..table.tostring(jsonToTable))
	if jsonToTable==nil then
		return
	end
	if jsonToTable.result==1 then
		showTopTips("心跳包失败")
	elseif jsonToTable.result==0 then
		--showTopTips("收到心跳包")
		NoticeManager.Instance:Dispatch(BowlingEvent.isConnect)
	end
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
     --[[   self:AddNotifictionLister(MVPGlobalNoticeType.reportGameResultReq, this.reportGameResultReq)
		self:AddNotifictionLister(MVPGlobalNoticeType.installApkReq, this.reqInstallApkReq)
		self:AddNotifictionLister(MVPGlobalNoticeType.upgradeApkReq, this.reqUpgradeApkReq)--]]
    end
    return self.notificationList
end
