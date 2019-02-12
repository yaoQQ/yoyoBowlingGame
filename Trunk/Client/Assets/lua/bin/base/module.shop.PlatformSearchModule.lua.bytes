require "base:module/shop/data/PlatformShopSearchProxy"
	
--平台模块示例
PlatformSearchModule = BaseModule:new()
local this = PlatformSearchModule
this.moduleName = "PlatformSearch"

--特殊处理例子协议
------------------------------注册由服务器发来的协议------------------------------
function this:initRegisterNet()
	this.netFuncList={}
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGetHotSearchText,this.onRspGetHotSearchText)
end

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
		self.switch={}
		--全局的通知在这里注册 
	end
	return self.notificationList
end



------------------------------发协议------------------------------

--请求获取热门搜索
function this.sendReqGetHotSearchText()
	local req = {}
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetHotSearchText", req)
end

------------------------------收协议------------------------------

--请求返回
function this.onRspGetHotSearchText(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "RspGetHotSearchText", protoBytes)
	PlatformShopSearchProxy.setHotSearchData(rsp.hot_text)	
end
