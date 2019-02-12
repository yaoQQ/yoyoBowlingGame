require "base:enum/PlatformGuessBetType"
require "base:module/guess/data/PlatformGuessBetProxy"
	

PlatformGuessBetModule = BaseModule:new()
local this = PlatformGuessBetModule
this.moduleName = "PlatformGuessBet"

------------------------------注册由服务器发来的协议------------------------------
function this.initRegisterNet()
	this.netFuncList={}
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspFindNearMatchGuessRoom,this.onRspNearGuessBet)--获取附近竞猜
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGetMatchGuessRoomList,this.onRspGetGuessRoomList)--获取商户的竞猜列表
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspChatChannelOp,this.onRspChatChannelOp)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxNotifySendChat,this.onNotitySendChat)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxNotifyChatChannelChange,this.onNotifyChatChannelChange)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspAnswerQuestionInfo,this.onRspAnswerQuestionInfo)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGetRoomQuestionRankInfo,this.onRspGetRoomQuestionRankInfo)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGetDynamicGuesssList,this.onRspGetDynamicGuesssList)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspViewRoomRankReward,this.onRspViewRoomRankReward)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspAnswerDynamicGuesssInfo,this.onRspAnswerDynamicGuesssInfo)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxNotifyMyRoomGuessScore,this.onNotifyMyRoomGuessScore)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspScoreExchangeCoupon,this.onRspScoreExchangeCoupon)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGetRoomRankReward,this.onRspGetRoomRankReward)
	this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspMyDynamicGuesssList,this.onRspMyDynamicGuesssList)
end

------------------------------由服务器发来的协议响应------------------------------
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
	end	
	return self.notificationList	
end

------------------------------通知响应------------------------------
function this:onNotificationLister(noticeType,notice)
	local switch = {
	}
	local fSwitch = switch[noticeType] --switch func  
	if fSwitch then --key exists  
		fSwitch() --do func  
	else --key not found  
		self:withoutRegistNotice(noticeType)--用于报错提醒
	end
end

------------------------------发协议------------------------------

--请求获取附近赛事信息
function this.sendReqNearGuessBet(lng, lat)
	local req = {}
	req.lng = lng
	req.lat = lat
	req.distance = 100000	--暂定1公里
	req.maxcount = 10 --暂定最多10个点
	--printDebug("请求获取附近赛事信息:"..table.tostring(req))
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqFindNearMatchGuessRoom", req)
end

--根据商户 id  获取赛事列表
function this.sendReqGetMatchGuessRoomList(shop_id)
	local req = {}
	req.shop_id = shop_id
	req.page = 0
	req.page_count = 30
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetMatchGuessRoomList", req)
end

--进入竞猜房间
function this.sendReqGuessChatChannelJoin(channel_id)
	local req = {}
	req.chat_type = ProtoEnumCommon.ChatType.ChatType_MatchGuess
	req.channel_id = channel_id
	req.op = ProtoEnumCommon.ChatChannelOp.ChatChannelOp_Join
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqChatChannelOp", req)
end

--退出竞猜房间
function this.sendReqGuessChatChannelLeave(channel_id)
	local req = {}
	req.chat_type = ProtoEnumCommon.ChatType.ChatType_MatchGuess
	req.channel_id = channel_id
	req.op = ProtoEnumCommon.ChatChannelOp.ChatChannelOp_Leave
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqChatChannelOp", req)
end

--请求动态问题列表
function this.sendReqGetDynamicGuesssList(room_id)
	local req = {}
	req.room_id = room_id
	req.page_count = 1000
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetDynamicGuesssList", req)
end

--聊天
function this.sendReqGuessSendChat(channel_id, chat_info)
	local req = {}
	req.chat_type = ProtoEnumCommon.ChatType.ChatType_MatchGuess
	req.channel_id = channel_id
	req.chat_info = chat_info
	req.chat_flag = "0"
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqSendChat", req)
end

--请求回答问题
function this.sendReqAnswerQuestionInfo(room_id, question_id, choose)
	PlatformGuessBetProxy.updateChatQuestionChooseData(question_id, choose)
	local req = {}
	req.room_id = room_id
	req.question_id = question_id
	req.choose = choose
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqAnswerQuestionInfo", req)
end

--请求获取答题排行榜信息
function this.sendReqGetRoomQuestionRankInfo(room_id, question_id)
	local req = {}
	req.room_id = room_id
	req.question_id = question_id
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetRoomQuestionRankInfo", req)
end

--请求动态问题下注
function this.sendReqAnswerDynamicGuesssInfo(room_id, question_id, choose, bet_score)
	local req = {}
	req.room_id = room_id
	req.question_id = question_id
	req.choose = choose
	req.bet_score = bet_score
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqAnswerDynamicGuesssInfo", req)
end

--请求查看奖励
function this.sendReqViewRoomRankReward(room_id, match_id)
	local req = {}
	req.room_id = room_id
	req.match_id = match_id
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqViewRoomRankReward", req)
end

--请求兑换积分奖励
function this.sendReqScoreExchangeCoupon(room_id, exchange_index, exchange_count)
	local req = {}
	req.room_id = room_id
	req.exchange_index = exchange_index
	req.exchange_count = exchange_count
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqScoreExchangeCoupon", req)
end

--请求兑换排行奖励
function this.sendReqGetRoomRankReward(room_id)
	local req = {}
	req.room_id = room_id
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetRoomRankReward", req)
end

--请求我的竞猜信息
function this.sendReqMyDynamicGuesssList(room_id)
	local req = {}
	req.room_id = room_id
	req.page = 0
	req.page_count = 1000
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqMyDynamicGuesssList", req)
end

------------------------------收协议------------------------------

--收到更新附近竞猜信息
function this.onRspNearGuessBet(protoBytes)
	local rsp = this.decodeProtoBytes("platform","RspFindNearMatchGuessRoom", protoBytes)
	
    PlatformLBSDataProxy.setGuessListData(rsp)
end

--收到商户的竞猜信息
function this.onRspGetGuessRoomList(protoBytes)
	local rsp = this.decodeProtoBytes("platform","RspGetMatchGuessRoomList", protoBytes)
	if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
		if rsp.room_info == nil then
			rsp.room_info = {}
		end
		if rsp.detail_info == nil then
			rsp.detail_info = {}
		end
		PlatformGuessBetProxy.setRoomListInfo(rsp)
		ViewManager.open(UIViewEnum.Platform_GuessBet_RoomListView)
	else
		Loger.PrintError("错误: "..rsp.result)
	end
end

function this.onRspGetDynamicGuesssList(protoBytes)
	local rsp = this.decodeProtoBytes("platform","RspGetDynamicGuesssList", protoBytes)
	if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
		if rsp.guess_info == nil then
			rsp.guess_info = {}
		end
		PlatformGuessBetProxy.setBetListInfo(rsp)
	else
		Loger.PrintError("错误: "..rsp.result)
	end
end

function this.onRspChatChannelOp(protoBytes)
	local rsp = this.decodeProtoBytes("platform","RspChatChannelOp", protoBytes)

	if rsp.result == 0 and rsp.chat_type == "ChatType_MatchGuess" then
		if rsp.op == "ChatChannelOp_Leave" then
			PlatformGuessBetProxy.clearRoomChatMsgList()
			ViewManager.close(UIViewEnum.Platform_GuessBet_ChatRoomView)
		elseif rsp.op == "ChatChannelOp_Join" then
			PlatformGuessBetProxy.clearRoomChatMsgList()
			PlatformGuessBetProxy.setCurRoomId(rsp.channel_id)
			ViewManager.open(UIViewEnum.Platform_GuessBet_ChatRoomView, rsp.channel_id)
		end
	end
end


function this.onNotitySendChat(protoBytes)
	local rsp = this.decodeProtoBytes("platform","NotifySendChat", protoBytes)
	if rsp.chat_type ~= ProtoEnumCommon.ChatType.ChatType_MatchGuess then
		return
	end
	--common.ChatType|chat_type
	--common.MsgPlayerBaseInfo|user_base_info
	--common.MsgChatInfo|chat_info
	if rsp.chat_info.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_Redpacket then
		rsp.chat_info.msg = ProtobufManager.decode("common", "MsgActiveCashRedPacketSt", rsp.chat_info.msg)
	elseif rsp.chat_info.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_Coupon then
		rsp.chat_info.msg = ProtobufManager.decode("common", "MsgActiveCouponRedPacketSt", rsp.chat_info.msg)
	elseif rsp.chat_info.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_Audio then
		rsp.chat_info.msg = ProtobufManager.decode("common", "MsgChatAudioInfo", rsp.chat_info.msg)
	elseif rsp.chat_info.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_MatchGuess then
		rsp.chat_info.msg = ProtobufManager.decode("common", "MatchGuessQuestionDetail", rsp.chat_info.msg)
		PlatformGuessBetProxy.addChatQuestionData(rsp.chat_info.msg)
	elseif rsp.chat_info.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_DynamicMatchGuess then
		local betInfo = ProtobufManager.decode("common", "MatchGuessQuestionDetail", rsp.chat_info.msg)
		PlatformGuessBetProxy.addBetInfo(betInfo)
	elseif rsp.chat_info.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_MatchGuessEventIive then
		rsp.chat_info.msg = ProtobufManager.decode("common", "MsgMatchDetailTLive", rsp.chat_info.msg)
	end
	printDebug("onNotitySendChat:"..table.tostring(rsp))
	PlatformGuessBetProxy.addRoomChatMsg(rsp)
end
 
function this.onNotifyChatChannelChange(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "NotifyChatChannelChange", protoBytes)

	PlatformGuessBetProxy.setRoomNum(rsp.player_count)
	--printDebug("+++++++++++++++++++准备改变 聊天页面人数信息：+++++")
	NoticeManager.Instance:Dispatch(PlatformGuessBetType.platform_Fresh_GuessRoom_Data_MSG)
end

--请求回答问题返回
function this.onRspAnswerQuestionInfo(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "RspAnswerQuestionInfo", protoBytes)
	if rsp.result == ProtoEnumCommon.AnswerQuestionResult.AnswerQuestionResult_Success or
		rsp.result == ProtoEnumCommon.AnswerQuestionResult.AnswerQuestionResult_RightNotRank then
		showFloatTips("回答正确")
		PlatformGuessBetProxy.updateChatQuestionAnswerData(rsp.question_id, true)
	elseif rsp.result == ProtoEnumCommon.AnswerQuestionResult.AnswerQuestionResult_Wrong then
		showFloatTips("回答错误")
		PlatformGuessBetProxy.updateChatQuestionAnswerData(rsp.question_id, false)
	end
end

function this.onRspGetRoomQuestionRankInfo(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "RspGetRoomQuestionRankInfo", protoBytes)
	if rsp.rank_info == nil then
		rsp.rank_info = {}
	end
	ViewManager.open(UIViewEnum.Platform_GuessBet_QuestionRankView, rsp)
end

function this.onRspViewRoomRankReward(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "RspViewRoomRankReward", protoBytes)
	if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess  then
		PlatformGuessBetProxy.setRewardInfo(rsp)
		if rsp.reward_type == ProtoEnumCommon.MatchGuessRoomRewardType.MatchGuessRoomRewardType_Rank then
			ViewManager.open(UIViewEnum.Platform_GuessBet_RewardRankView)
		else
			ViewManager.open(UIViewEnum.Platform_GuessBet_RewardScoreView)
		end
	else
		Loger.PrintError("错误: "..rsp.result)
	end
end

function this.onRspAnswerDynamicGuesssInfo(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "RspAnswerDynamicGuesssInfo", protoBytes)
	if rsp.result == ProtoEnumCommon.MatchGuessAnswerGuessResult.MatchGuessAnswerGuessResult_Success then
		PlatformGuessBetProxy.updateDynamicItem(rsp.question_id, rsp.choose, rsp.bet_score)
		NoticeManager.Instance:Dispatch(PlatformGuessBetType.Platform_GuessBet_UpdateDynamicListView)
	elseif rsp.result == ProtoEnumCommon.MatchGuessAnswerGuessResult.MatchGuessAnswerGuessResult_Fail then
		showFloatTips("下注失败")
		--Loger.PrintError("下注失败: "..rsp.result)
	elseif rsp.result == ProtoEnumCommon.MatchGuessAnswerGuessResult.MatchGuessAnswerGuessResult_ScoreNotEnough then
		showFloatTips("积分不足")
	elseif rsp.result == ProtoEnumCommon.MatchGuessAnswerGuessResult.MatchGuessAnswerGuessResult_TimeOut then
		showFloatTips("下注时间已截止")
	end
end

function this.onNotifyMyRoomGuessScore(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "NotifyMyRoomGuessScore", protoBytes)
	if PlatformGuessBetProxy.getCurRoomId() == rsp.room_id then
		PlatformGuessBetProxy.setCurGuessRoomMoney(rsp.score)
	end
end

function this.onRspScoreExchangeCoupon(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "RspScoreExchangeCoupon", protoBytes)
	if rsp.result == ProtoEnumCommon.MatchGuessScoreExchangeResult.MatchGuessScoreExchangeResult_Success then
		showFloatTips("兑换成功")
		PlatformGuessBetProxy.updateScoreRewardItem(rsp.exchange_index, rsp.have_changed)
		NoticeManager.Instance:Dispatch(PlatformGuessBetType.Platform_GuessBet_UpdateRewardView)
	elseif rsp.result == ProtoEnumCommon.MatchGuessScoreExchangeResult.MatchGuessScoreExchangeResult_Fail then
		showFloatTips("兑换失败")
	elseif rsp.result == ProtoEnumCommon.MatchGuessScoreExchangeResult.MatchGuessScoreExchangeResult_ExchangeOver then
		showFloatTips("已兑换完")
	elseif rsp.result == ProtoEnumCommon.MatchGuessScoreExchangeResult.MatchGuessScoreExchangeResult_ScoreNotEnough then
		showFloatTips("积分不足")
	end
end

function this.onRspGetRoomRankReward(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "RspGetRoomRankReward", protoBytes)
	if rsp.result == ProtoEnumCommon.MatchGuessGetRewardResult.MatchGuessGetRewardResult_Success then
		showFloatTips("领取成功")
		--PlatformGuessBetProxy.updateRankRewardItem()
		--NoticeManager.Instance:Dispatch(PlatformGuessBetType.Platform_GuessBet_UpdateRewardView)
	elseif rsp.result == ProtoEnumCommon.MatchGuessGetRewardResult.MatchGuessGetRewardResult_Fail then
		showFloatTips("领取失败")
	elseif rsp.result == ProtoEnumCommon.MatchGuessGetRewardResult.MatchGuessGetRewardResult_NotInRank then
		showFloatTips("没有领取资格")
	elseif rsp.result == ProtoEnumCommon.MatchGuessGetRewardResult.MatchGuessGetRewardResult_HaveGot then
		showFloatTips("已领取过")
	elseif rsp.result == ProtoEnumCommon.MatchGuessGetRewardResult.MatchGuessGetRewardResult_MatchNotEnd then
		showFloatTips("赛事未结束，不能领取")
	end
end

--请求我的竞猜信息列表返回
function this.onRspMyDynamicGuesssList(protoBytes)
	local rsp = this.decodeProtoBytes("platform", "RspMyDynamicGuesssList", protoBytes)
	if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
		if rsp.answer_info == nil then
			rsp.answer_info = {}
		end
		ViewManager.open(UIViewEnum.Platform_GuessBet_MyBetView, rsp.answer_info)
	else
		Loger.PrintError("请求我的竞猜信息列表错误: "..rsp.result)
	end
end