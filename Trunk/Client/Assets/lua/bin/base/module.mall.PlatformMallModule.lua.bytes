
require "base:module/mall/data/PlatformMallProxy"

PlatformMallModule = BaseModule:new()
local this = PlatformMallModule

this.moduleName="PlatformMall"

--==================================================通信（服务器推送）====================================
function this.initRegisterNet()
	this.netFuncList={}
	
	--this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspFriendList,this.rspFriendsList)
	

end

function this.onNetMsgLister(protoID,protoBytes)
	local nfSwitch = this.netFuncList[protoID] 
	if nfSwitch then  
		nfSwitch(protoBytes) 
	else 
		this:withoutRegistNotice(protoID)
	end
end
--==================================================消息==================================================

function this:getRegisterNotificationList()
	if self.notificationList == nil then
		self.notificationList = {} 
		self.switch={}
	    self:AddNotifictionLister(PlatformMallType.Platform_Rsp_Mall_List,this.onRspMallList)  

	    

	end
    return self.notificationList
end


---------------------------------------------收到消息(客户端或服务端发出)---------------------------
--请求好友列表返回
function this:onRspMallList(notice)
	local rsp = notice:GetObj()











end

--请求好友操作返回
function this:onRspFriendOp(notice)
	local rsp = notice:GetObj()

	
end

