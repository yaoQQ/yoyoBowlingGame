--签到模块
PlatformSignModule = BaseModule:new()
local this = PlatformSignModule
this.moduleName = "PlatformSign"

------------------------------注册由服务器发来的协议------------------------------
function this:initRegisterNet()
	this.netFuncList={}
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGetSignInfo, this.onRspGetSignInfo)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspSign, this.onRspSign)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspRcvMonSignReward, this.onRspRcvMonSignReward)
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

------------------------------收发协议------------------------------

--请求获取签到信息
function this.sendReqGetSignInfo()
	local req = {}
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetSignInfo", req)
end

--请求获取签到信息返回
function this.onRspGetSignInfo(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "RspGetSignInfo", protoBytes)
	
	if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
		NoticeManager.Instance:Dispatch(NoticeType.Sign_Init_SignInfo, rsp)
	else
		Alert.showAlertMsg(nil, "获取签到信息失败，请稍后再试", "确定", function()
			ViewManager.close(UIViewEnum.Platform_Sign_View)
		end)
	end
end

--请求签到
function this.sendReqSign(cash_num)
	local req = {}
	req.cash_num = cash_num
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqSign", req)
end

--请求签到返回
function this.onRspSign(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "RspSign", protoBytes)
	
	if rsp.result == ProtoEnumPlatform.SignResult.SignResult_Success then
		NoticeManager.Instance:Dispatch(NoticeType.Sign_Update_SignInfo, rsp)
		--红包刷新响应
		NoticeManager.Instance:Dispatch(NoticeType.User_Update_Cash)
	elseif rsp.result == ProtoEnumPlatform.SignResult.SignResult_Fail then
		showFloatTips("签到失败")
	elseif rsp.result == ProtoEnumPlatform.SignResult.SignResult_HasSined then
		showFloatTips("今天已签到过")
	end
end

--请求领取月签到奖励
function this.sendReqRcvMonSignReward(day)
	local req = {}
	req.day = day
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqRcvMonSignReward", req)
end

--请求领取月签到奖励返回
function this.onRspRcvMonSignReward(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "RspRcvMonSignReward", protoBytes)
	
	if rsp.result == ProtoEnumPlatform.RcvMonSignReward.RcvMonSignReward_Success then
		NoticeManager.Instance:Dispatch(NoticeType.Sign_Update_MonthInfo, rsp)
	elseif rsp.result == ProtoEnumPlatform.RcvMonSignReward.RcvMonSignReward_Fail then
		showFloatTips("领取奖励失败")
	elseif rsp.result == ProtoEnumPlatform.RcvMonSignReward.RcvMonSignReward_HasReceived then
		showFloatTips("已领取过该奖励")
	end
end