require "base:module/example/data/PlatformExampleDataProxy"
	
--平台模块示例
PlatformExampleModule = BaseModule:new()
local this = PlatformExampleModule
this.moduleName = "PlatformExample"

--特殊处理例子协议
ProtoEnumPlatform.MsgIdx.MsgIdxRspExample=999999999
ProtoEnumPlatform.MsgIdx.MsgIdxNotifyExample=999999998
------------------------------注册由服务器发来的协议------------------------------
function this:initRegisterNet()
	this.netFuncList={}
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspExample,this.onRspExample)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxNotifyExample,this.onNotifyExample)
end
function this.onNetMsgLister(protoID,protoBytes)
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
		self.switch={}
		--全局的通知在这里注册
		self:AddNotifictionLister(NoticeType.Example,this.onNoticeExample)
	end
	return self.notificationList
end


--通知响应示例
function this.onNoticeExample(noticeType, notice)
	local data = notice:GetObj()
	
	--数据处理
end

------------------------------发协议------------------------------

--请求
function this.sendReqExample(param1, param2)
	local req = {}
	req.param1 = param1
	req.param2 = param2
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqExample", req)
end

------------------------------收协议------------------------------

--请求返回
function this.onRspExample(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "RspExample", protoBytes)
	
	--收到服务器发来的协议后，一般有2种处理方法
	--1.一般情况下，把数据交给数据层处理
    --PlatformExampleDataProxy.setRspExampleData(rsp)
	
	--2.如果该数据仅影响接下来要打开的界面上的数据显示，可以不通过数据层，直接打开对应界面，通过打开界面的参数传递数据
	--ViewManager.open(UIViewEnum.Platform_Example_XXX_View, rsp)
end

--服务器广播通知
function this.onNotifyExample(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "NotifyExample", protoBytes)
	
	--和上面的处理方法类似
end