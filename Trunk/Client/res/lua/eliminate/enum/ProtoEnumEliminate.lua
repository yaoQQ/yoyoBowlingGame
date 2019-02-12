--协议工具生成的代码，不要手动修改
ProtoEnumEliminate = {}
ProtoEnumEliminate.MsgIdx =
{
	-- 请求登录
	MsgIdxReqLoginLogin = 40001,
	-- 请求登录返回
	MsgIdxRspLoginLogin = 40002,
	-- 请求登录
	MsgIdxReqLogin = 40011,
	-- 请求登录返回
	MsgIdxRspLogin = 40012,
	-- 请求心跳
	MsgIdxReqHeartbeat = 40013,
	-- 请求心跳返回
	MsgIdxRspHeartbeat = 40014,
	-- 请求房间匹配游戏
	MsgIdxReqRoomMatchBegin = 40100,
	-- 请求房间匹配游戏返回
	MsgIdxRspRoomMatchBegin = 40101,
	-- 请求创建房间
	MsgIdxReqCreateRoom = 40102,
	-- 请求创建房间返回
	MsgIdxRspCreateRoom = 40103,
	-- 房间信息通知
	MsgIdxNotifyRoomInfo = 40104,
	-- 请求房间准备
	MsgIdxReqSetReady = 40105,
	-- 请求房间准备返回
	MsgIdxRspSetReady = 40106,
	-- 请求房间踢人
	MsgIdxReqKickMember = 40107,
	-- 请求房间踢人返回
	MsgIdxRspKickMember = 40108,
	-- 请求进入房间
	MsgIdxReqEnterRoom = 40109,
	-- 请求进入房间返回
	MsgIdxRspEnterRoom = 40110,
	-- 请求开始游戏
	MsgIdxReqQuitRoom = 40111,
	-- 请求开始游戏返回
	MsgIdxRspQuitRoom = 40112,
	-- 请求开始游戏
	MsgIdxReqRoomStartGame = 40113,
	-- 请求开始游戏返回
	MsgIdxRspRoomStartGame = 40114,
	-- 请求广播游戏信息（进度，分数）
	MsgIdxReqBoardCastGameInfo = 40115,
	-- 广播游戏信息(进度，分数)
	MsgIdxNotifyBoardCastGameInfo = 40116,
	-- 广播游戏正式开始
	MsgIdxNotifyGameRealStart = 40117,
	-- 广播游戏时间结束
	MsgIdxNotifyGameTimeOut = 40118,
	-- 请求取消匹配
	MsgIdxReqCancelMatch = 40119,
	-- 请求取消匹配返回
	MsgIdxRspCancelMatch = 40120,
	-- 请求游戏正式结束
	MsgIdxReqRealGameEnd = 40121,
	-- 广播游戏正式结束
	MsgIdxNotifyRealGameEnd = 40122,
	-- 请求使用道具
	MsgIdxReqUseItem = 40123,
	-- 广播使用道具
	MsgIdxNotifyUseItem = 40124,
	-- 请求房间聊天
	MsgIdxReqRoomChat = 40125,
	-- 广播房间聊天
	MsgIdxNotifyRoomChat = 40126,
	-- 请求退出游戏
	MsgIdxReqQuitEliminateGame = 40127,
	-- 分数通知
	MsgIdxNotifyMyRoomGuessScore = 40128,
	-- 发送聊天通知
	MsgIdxNotifySendChat = 40129,
	-- 改变频道通知
	MsgIdxNotifyChatChannelChange = 40130,
	-- 物品改变
	MsgIdxNotifyItemChange = 40131,
}
ProtoEnumEliminate.LoginResult =
{
	-- 连接成功
	LoginResultSuccess = "LoginResultSuccess",
	-- 内部错误
	LoginResultErrInternal = "LoginResultErrInternal",
	-- 无效的Token
	LoginResultInvalidToken = "LoginResultInvalidToken",
	-- 无效的登录Key
	LoginResultErrInvalidSignKey = "LoginResultErrInvalidSignKey",
	-- 重复登录
	LoginResultErrDuplicateLogin = "LoginResultErrDuplicateLogin",
}
ProtoEnumEliminate.RoomMemberCount =
{
	-- 
	RoomMemberCount_FOUR = "RoomMemberCount_FOUR",
	-- 
	RoomMemberCount_THREE = "RoomMemberCount_THREE",
	-- 
	RoomMemberCount_TWO = "RoomMemberCount_TWO",
	-- 
	RoomMemberCount_ONE = "RoomMemberCount_ONE",
}
ProtoEnumEliminate.RspCreateRoomResult =
{
	-- 成功
	RspCreateRoomResult_Success = "RspCreateRoomResult_Success",
	-- 等级不足
	RspCreateRoomResult_LevelLimit = "RspCreateRoomResult_LevelLimit",
	-- 已经在其他房间
	RspCreateRoomResult_InRoom = "RspCreateRoomResult_InRoom",
}
ProtoEnumEliminate.RoomOperateType =
{
	-- 进入房间
	RoomOperateType_Enter = "RoomOperateType_Enter",
	-- 剔除出房间
	RoomOperateType_Kick = "RoomOperateType_Kick",
	-- 成员准备
	RoomOperateType_Ready = "RoomOperateType_Ready",
	-- 成员未准备
	RoomOperateType_NotReady = "RoomOperateType_NotReady",
	-- 退出房间
	RoomOperateType_Quit = "RoomOperateType_Quit",
	-- 房间开始游戏
	RoomOperateType_GameStart = "RoomOperateType_GameStart",
}
ProtoEnumEliminate.KickMemberResult =
{
	-- 成功
	KickMemberResult_Success = "KickMemberResult_Success",
	-- 不是房主
	KickMemberResult_IsNotOwner = "KickMemberResult_IsNotOwner",
	-- 找不到成员
	KickMemberResult_IsNotInRoom = "KickMemberResult_IsNotInRoom",
	-- 不能剔自己
	KickMemberResult_CannotKickOwn = "KickMemberResult_CannotKickOwn",
}
ProtoEnumEliminate.EnterRoomResult =
{
	-- 成功
	EnterRoomResult_Success = "EnterRoomResult_Success",
	-- 没有该房间
	EnterRoomResult_NotRoomID = "EnterRoomResult_NotRoomID",
	-- 密码错误
	EnterRoomResult_PasswordError = "EnterRoomResult_PasswordError",
	-- 已经在其他房间
	EnterRoomResult_InRoom = "EnterRoomResult_InRoom",
	-- 房间满人
	EnterRoomResult_Full = "EnterRoomResult_Full",
	-- 房间在游戏中
	EnterRoomResult_Game = "EnterRoomResult_Game",
}
ProtoEnumEliminate.QuitRoomResult =
{
	-- 成功
	QuitRoomResult_Success = "QuitRoomResult_Success",
	-- 没有该房间
	QuitRoomResult_NotRoomID = "QuitRoomResult_NotRoomID",
}
ProtoEnumEliminate.StartGameResult =
{
	-- 成功
	StartGameResult_Success = "StartGameResult_Success",
	-- 没有该房间
	StartGameResult_NotRoomID = "StartGameResult_NotRoomID",
	-- 不是房主
	StartGameResult_IsNotOwner = "StartGameResult_IsNotOwner",
	-- 还没有全部准备
	StartGameResult_NotAllReady = "StartGameResult_NotAllReady",
}
ProtoEnumEliminate.BoardCastDefine =
{
	-- 广播分数
	BoardCastDefine_Score = "BoardCastDefine_Score",
	-- 广播进度
	BoardCastDefine_Progress = "BoardCastDefine_Progress",
	-- 广播离线
	BoardCastDefine_OffLine = "BoardCastDefine_OffLine",
}
ProtoEnumEliminate.CancelMatchResult =
{
	-- 成功
	CancelMatchResult_Success = "CancelMatchResult_Success",
	-- 不在匹配池
	CancelMatchResult_NotInPool = "CancelMatchResult_NotInPool",
}
ProtoEnumEliminate.RoomChatMsgType =
{
	-- 文字
	RoomChatMsgType_Text = "RoomChatMsgType_Text",
	-- 图片
	RoomChatMsgType_Picture = "RoomChatMsgType_Picture",
	-- 红包
	RoomChatMsgType_Redpacket = "RoomChatMsgType_Redpacket",
	-- 优惠券
	RoomChatMsgType_Coupon = "RoomChatMsgType_Coupon",
	-- 语音
	RoomChatMsgType_Audio = "RoomChatMsgType_Audio",
}
ProtoEnumEliminate.SceneLoginResult =
{
	-- 连接成功
	SceneLoginResultSuccess = "SceneLoginResultSuccess",
	-- 内部错误
	SceneLoginResultErrInternal = "SceneLoginResultErrInternal",
	-- 无效的账号,为空或者长度超过64位
	SceneLoginResultErrInvalidAccount = "SceneLoginResultErrInvalidAccount",
	-- 无效的登录Key
	SceneLoginResultErrInvalidSignKey = "SceneLoginResultErrInvalidSignKey",
	-- 重复登录
	SceneLoginResultErrDuplicateLogin = "SceneLoginResultErrDuplicateLogin",
}
