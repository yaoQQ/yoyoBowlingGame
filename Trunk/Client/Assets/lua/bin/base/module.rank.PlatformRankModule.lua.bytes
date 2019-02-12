require "base:module/rank/data/PlatformRankDataProxy"
	
--签到模块
PlatformRankModule = BaseModule:new()
local this = PlatformRankModule
this.moduleName = "PlatformRank"

------------------------------注册由服务器发来的协议------------------------------
function this:initRegisterNet()
	this.netFuncList={}
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspPlayerRank, this.onRspPlayerRank)
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

--请求玩家排行
function this.sendReqPlayerRank(rank_type, param)
	local req = {}
	req.rank_type = rank_type
	req.param = param
	req.page_index = 0
	req.per_page_num = 50
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqPlayerRank", req)
end

--请求玩家排行返回
function this.onRspPlayerRank(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "RspPlayerRank", protoBytes)
	if rsp.rank_info == nil then
		rsp.rank_info = {}
	end
	
	if rsp.rank_type == ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthlyGetCash or
		rsp.rank_type == ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthJoinGame or
		rsp.rank_type == ProtoEnumPlatform.PlayerRankType.PlayerRankType_MonthGameChampion then
		NoticeManager.Instance:Dispatch(NoticeType.Rank_Update_RankInfo, rsp)
	elseif rsp.rank_type == ProtoEnumPlatform.PlayerRankType.PlayerRankType_GetCash or
		rsp.rank_type == ProtoEnumPlatform.PlayerRankType.PlayerRankType_JoinGame or
		rsp.rank_type == ProtoEnumPlatform.PlayerRankType.PlayerRankType_GameChampion then
		NoticeManager.Instance:Dispatch(NoticeType.Rank_Update_RankTotalInfo, rsp)
	end
end