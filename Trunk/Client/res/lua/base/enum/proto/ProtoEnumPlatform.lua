--协议工具生成的代码，不要手动修改
ProtoEnumPlatform = {}
ProtoEnumPlatform.MsgIdx =
{
	-- 请求获取邮件列表
	MsgIdxReqGetMailList = 12100,
	-- 请求获取邮件列表返回
	MsgIdxRspGetMailList = 12101,
	-- 请求删除邮件
	MsgIdxReqDelMail = 12103,
	-- 请求删除邮件返回
	MsgIdxRspDelMail = 12104,
	-- 请求获取邮件奖励
	MsgIdxReqMailAttach = 12105,
	-- 请求获取邮件奖励返回
	MsgIdxRspMailAttach = 12106,
	-- 通知有新邮件(在线)
	MsgIdxNotifyNewMail = 12108,
	-- 获取附近用户请求
	MsgIdxReqFindNearUsers = 12203,
	-- 获取附近用户返回
	MsgIdxRspFindNearUsers = 12204,
	-- 请求附近红包
	MsgIdxReqFindNearRedPacket = 12209,
	-- 请求附近红包返回
	MsgIdxRspFindNearRedPacket = 12210,
	-- 请求更新活动游戏分数
	MsgIdxReqUpdateActiveGameScore = 12221,
	-- 请求更新活动游戏分数返回
	MsgIdxRspUpdateActiveGameScore = 12222,
	-- 请求领取地图红包
	MsgIdxReqRcvMapRedPacket = 12251,
	-- 请求领取地图红包返回
	MsgIdxRspRcvMapRedPacket = 12252,
	-- 获取地图红包领取记录
	MsgIdxReqMapRedPacketRcvHistory = 12253,
	-- 获取地图红包领取记录返回
	MsgIdxRspMapRedPacketRcvHistory = 12254,
	-- 请求加入聊天频道
	MsgIdxReqChatChannelOp = 12301,
	-- 请求加入聊天频道返回
	MsgIdxRspChatChannelOp = 12302,
	-- 请求发送聊天消息
	MsgIdxReqSendChat = 12303,
	-- 通知聊天人数变化
	MsgIdxNotifyChatChannelChange = 12306,
	-- 广播聊天消息
	MsgIdxNotifySendChat = 12308,
	-- 请求执行GM命令
	MsgIdxReqGMCommand = 12901,
	-- 请求执行GM命令返回
	MsgIdxRspGMCommand = 12902,
	-- 请求获取好友红包信息
	MsgIdxReqGetFriendOnlineRedPacketInfo = 13151,
	-- 请求获取好友红包信息返回
	MsgIdxRspGetFriendOnlineRedPacketInfo = 13152,
	-- 请求自己的在线红包信息
	MsgIdxReqGetMyselfOnlineRedPacketInfo = 13153,
	-- 请求自己的在线红包信息返回
	MsgIdxRspGetMyselfOnlineRedPacketInfo = 13154,
	-- 请求领取在线红包
	MsgIdxReqReceiveOnLineRedPacket = 13155,
	-- 请求领取在线红包返回
	MsgIdxRspReceiveOnLineRedPacket = 13156,
	-- 请求偷红包
	MsgIdxReqStealOnlineRedPacket = 13157,
	-- 请求偷红包返回
	MsgIdxRspStealOnlineRedPacket = 13158,
	-- 通知红包被偷
	MsgIdxNotifyRobbedOnlineRedPacket = 13160,
	-- 请求在线红包分享奖励
	MsgIdxReqOnlineRedPacketShareRewards = 13161,
	-- 请求在线红包分享奖励返回
	MsgIdxRspOnlineRedPacketShareRewards = 13162,
	-- 获取热门游戏请求
	MsgIdxReqGetHotGame = 13171,
	-- 获取热门游戏返回
	MsgIdxRspGetHotGame = 13172,
	-- 请求创建群组
	MsgIdxReqCreateGroupChat = 15001,
	-- 请求创建群组返回
	MsgIdxRspCreateGroupChat = 15002,
	-- 请求获取群组列表
	MsgIdxReqGetGroupChatList = 15003,
	-- 请求获取群组列表返回
	MsgIdxRspGetGroupChatList = 15004,
	-- 请求修改群组信息
	MsgIdxReqModifyGroupChatInfo = 15005,
	-- 请求修改群组信息返回
	MsgIdxRspModifyGroupChatInfo = 15006,
	-- 请求申请加入群组
	MsgIdxReqApplyJoinGroupChat = 15007,
	-- 请求申请加入群组返回
	MsgIdxRspApplyJoinGroupChat = 15008,
	-- 请求退出群组
	MsgIdxReqExitGroupChat = 15009,
	-- 请求退出群组返回
	MsgIdxRspExitGroupChat = 15010,
	-- 广播聊天消息
	MsgIdxNotifyChatMsg = 15011,
	-- 广播群组信息改变
	MsgIdxNotifyGroupChatInfoChanged = 15012,
	-- 请求获取申请加入群组的列表
	MsgIdxReqGetApplyJoinGroupChatList = 15013,
	-- 请求获取申请加入群组的列表返回
	MsgIdxRspGetApplyJoinGroupChatList = 15014,
	-- 请求搜索群组
	MsgIdxReqSearchGroupChat = 15015,
	-- 请求搜索群组返回
	MsgIdxRspSearchGroupChat = 15016,
	-- 请求处理申请加入群组
	MsgIdxReqDealApplyJoinGroupChat = 15017,
	-- 请求处理申请加入群组返回
	MsgIdxRspDealApplyJoinGroupChat = 15018,
	-- 请求获取群组信息
	MsgIdxReqGetGroupChatInfo = 15019,
	-- 请求获取群组信息返回
	MsgIdxRspGetGroupChatInfo = 15020,
	-- 请求获取群组成员列表
	MsgIdxReqGetGroupChatMemberList = 15021,
	-- 请求获取群组成员列表返回
	MsgIdxRspGetGroupChatMemberList = 15022,
	-- 请求获取群组聊天信息
	MsgIdxReqGetGroupChatMsgList = 15023,
	-- 请求获取群组聊天信息返回
	MsgIdxRspGetGroupChatMsgList = 15024,
	-- 请求发送群组聊天消息
	MsgIdxReqSendGroupChatMsg = 15025,
	-- 请求发送群组聊天消息返回
	MsgIdxRspSendGroupChatMsg = 15026,
	-- 请求进入赛事房间信息
	MsgIdxReqGetMatchGuessRoomInfo = 17001,
	-- 请求进入赛事房间信息返回
	MsgIdxRspGetMatchGuessRoomInfo = 17002,
	-- 请求赛事房间列表
	MsgIdxReqGetMatchGuessRoomList = 17003,
	-- 请求赛事房间列表返回
	MsgIdxRspGetMatchGuessRoomList = 17004,
	-- 请求获取赛事问题信息
	MsgIdxReqGetMatchGuessRoomQuestionInfo = 17005,
	-- 请求获取赛事问题信息返回
	MsgIdxRspGetMatchGuessRoomQuestionInfo = 17006,
	-- 请求回答竞猜题目信息
	MsgIdxReqAnswerQuestionInfo = 17007,
	-- 请求回答竞猜题目信息返回
	MsgIdxRspAnswerQuestionInfo = 17008,
	-- 请求获取题目排行榜信息
	MsgIdxReqGetRoomQuestionRankInfo = 17009,
	-- 请求获取题目排行榜信息返回
	MsgIdxRspGetRoomQuestionRankInfo = 17010,
	-- 请求获取比赛信息
	MsgIdxReqGetMatchInfo = 17011,
	-- 请求获取比赛信息返回
	MsgIdxRspGetMatchInfo = 17012,
	-- 请求领取排行榜奖励
	MsgIdxReqGetRoomRankReward = 17013,
	-- 请求领取排行榜奖励返回
	MsgIdxRspGetRoomRankReward = 17014,
	-- 请求兑换优惠券
	MsgIdxReqScoreExchangeCoupon = 17015,
	-- 请求兑换优惠券返回
	MsgIdxRspScoreExchangeCoupon = 17016,
	-- 查看房间奖励
	MsgIdxReqViewRoomRankReward = 17017,
	-- 查看房间奖励返回
	MsgIdxRspViewRoomRankReward = 17018,
	-- 请求附近赛事房间
	MsgIdxReqFindNearMatchGuessRoom = 17019,
	-- 请求附近赛事房间返回
	MsgIdxRspFindNearMatchGuessRoom = 17020,
	-- 请求动态问题列表
	MsgIdxReqGetDynamicGuesssList = 17021,
	-- 请求动态问题列表返回
	MsgIdxRspGetDynamicGuesssList = 17022,
	-- 通知动态问题信息
	MsgIdxNotifyDynamicGuesssInfo = 17023,
	-- 请求动态问题下注
	MsgIdxReqAnswerDynamicGuesssInfo = 17024,
	-- 请求动态问题下注返回
	MsgIdxRspAnswerDynamicGuesssInfo = 17025,
	-- 请求我的竞猜列表
	MsgIdxReqMyDynamicGuesssList = 17026,
	-- 请求我的竞猜列表返回
	MsgIdxRspMyDynamicGuesssList = 17027,
	-- 我的房间积分通知
	MsgIdxNotifyMyRoomGuessScore = 17028,
	-- 请求设置用户通知消息状态
	MsgIdxReqSetUserNotifyStatus = 17037,
	-- 请求设置用户通知消息状态返回
	MsgIdxRspSetUserNotifyStatus = 17038,
	-- 通知用户消息
	MsgIdxNotifyUserMsg = 17040,
	-- 请求确认收到用户通知消息
	MsgIdxReqConfirmUserMsg = 17041,
	-- 请求确认收到用户通知消息返回
	MsgIdxRspConfirmUserMsg = 17042,
	-- 请求获取用户通知消息
	MsgIdxReqGetUserNotifyMsg = 17043,
	-- 请求获取用户通知消息返回
	MsgIdxRspGetUserNotifyMsg = 17044,
	-- 充值返回通知
	MsgIdxNotifyRechargeMsg = 17045,
	-- 请求所有活动
	MsgIdxReqFindNearAllActivity = 17047,
	-- 请求所有活动返回
	MsgIdxRspFindNearAllActivity = 17048,
	-- 请求获取充值账单列表
	MsgIdxReqGetBillingList = 17049,
	-- 请求获取充值账单列表返回
	MsgIdxRspGetBillingList = 17050,
	-- 请求热门关键字搜索
	MsgIdxReqGetHotSearchText = 17051,
	-- 请求热门关键字搜索返回
	MsgIdxRspGetHotSearchText = 17052,
	-- 请求官方所有附近活动
	MsgIdxReqFindNearAllOfficialActivity = 17053,
	-- 请求官方所有附近活动返回
	MsgIdxRspFindNearAllOfficialActivity = 17054,
	-- 请求官方所有活动
	MsgIdxReqGetOfficalActivity = 17055,
	-- 请求官方所有活动返回
	MsgIdxRspGetOfficalActivity = 17056,
	-- 请求官方活动排行榜
	MsgIdxReqGetOfficalActivityRank = 17057,
	-- 请求官方活动排行榜返回
	MsgIdxRspGetOfficalActivityRank = 17058,
	-- 请求每日更新通知
	MsgIdxNotifyDailyCounterChange = 17060,
	-- 请求金币补助
	MsgIdxReqGetBankruptcySubsidy = 17061,
	-- 请求金币补助返回
	MsgIdxRspGetBankruptcySubsidy = 17062,
	-- 请求领取所有邮件的物品
	MsgIdxReqGetAllMailItem = 17063,
	-- 请求领取所有邮件的物品返回
	MsgIdxRspGetAllMailItem = 17064,
	-- 请求设备日志信息
	MsgIdxReqDeviceLogInfo = 17065,
	-- 跑马灯通知
	MsgIdxNotifyHorseRaceLamp = 17067,
	-- 请求获取签到信息
	MsgIdxReqGetSignInfo = 17091,
	-- 请求获取签到信息返回
	MsgIdxRspGetSignInfo = 17092,
	-- 请求签到
	MsgIdxReqSign = 17093,
	-- 请求签到返回
	MsgIdxRspSign = 17094,
	-- 请求领取月签到奖励
	MsgIdxReqRcvMonSignReward = 17095,
	-- 请求领取月签到奖励回复
	MsgIdxRspRcvMonSignReward = 17096,
	-- 请求玩家排行
	MsgIdxReqPlayerRank = 17097,
	-- 请求玩家排行返回
	MsgIdxRspPlayerRank = 17098,
}
ProtoEnumPlatform.RcvOnlineRPResult =
{
	-- 成功
	RcvOnlineRPResult_Success = "RcvOnlineRPResult_Success",
	-- 等待
	RcvOnlineRPResult_Wait = "RcvOnlineRPResult_Wait",
	-- 其它错误
	RcvOnlineRPResult_OtherError = "RcvOnlineRPResult_OtherError",
	-- 超过次数
	RcvOnlineRPResult_OverNum = "RcvOnlineRPResult_OverNum",
}
ProtoEnumPlatform.StealOnlineRPResult =
{
	-- 成功
	StealOnlineRPResult_Success = "StealOnlineRPResult_Success",
	-- 已经偷过
	StealOnlineRPResult_Stole = "StealOnlineRPResult_Stole",
	-- 可偷次数用完
	StealOnlineRPResult_OverNum = "StealOnlineRPResult_OverNum",
	-- 等待中
	StealOnlineRPResult_Wait = "StealOnlineRPResult_Wait",
	-- 参数错误
	StealOnlineRPResult_ParamError = "StealOnlineRPResult_ParamError",
	-- 失败
	StealOnlineRPResult_Fail = "StealOnlineRPResult_Fail",
}
ProtoEnumPlatform.SignResult =
{
	-- 签到成功
	SignResult_Success = "SignResult_Success",
	-- 签到失败
	SignResult_Fail = "SignResult_Fail",
	-- 今天已经签过
	SignResult_HasSined = "SignResult_HasSined",
}
ProtoEnumPlatform.RcvMonSignReward =
{
	-- 领取月签到奖励成功
	RcvMonSignReward_Success = "RcvMonSignReward_Success",
	-- 领取月签到奖励失败
	RcvMonSignReward_Fail = "RcvMonSignReward_Fail",
	-- 已经领取月签到奖励
	RcvMonSignReward_HasReceived = "RcvMonSignReward_HasReceived",
}
ProtoEnumPlatform.PlayerRankType =
{
	-- 获得红包总额
	PlayerRankType_GetCash = "PlayerRankType_GetCash",
	-- 月获得红包总额
	PlayerRankType_MonthlyGetCash = "PlayerRankType_MonthlyGetCash",
	-- 参加游戏次数
	PlayerRankType_JoinGame = "PlayerRankType_JoinGame",
	-- 月参加游戏次数
	PlayerRankType_MonthJoinGame = "PlayerRankType_MonthJoinGame",
	-- 参加游戏冠军
	PlayerRankType_GameChampion = "PlayerRankType_GameChampion",
	-- 月参加游戏冠军
	PlayerRankType_MonthGameChampion = "PlayerRankType_MonthGameChampion",
}
