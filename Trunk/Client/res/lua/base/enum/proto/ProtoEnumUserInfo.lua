--协议工具生成的代码，不要手动修改
ProtoEnumUserInfo = {}
ProtoEnumUserInfo.MsgIdx =
{
	-- 通知用户登录完成
	MsgIdxNotifyLoginComplete = 12000,
	-- 请求用户详细信息
	MsgIdxReqUserInfo = 12001,
	-- 请求用户详细信息返回
	MsgIdxRspUserInfo = 12002,
	-- 修改用户基本信息请求
	MsgIdxReqChangeUserBaseInfo = 12003,
	-- 修改用户基本信息返回
	MsgIdxRspChangeUserBaseInfo = 12004,
	-- 请求更新用户位置请求
	MsgIdxReqUpUserPosition = 12005,
	-- 请求更新用户信息返回
	MsgIdxRspUpUserPosition = 12006,
	-- 获取用户坐标请求
	MsgIdxReqGetUserCoord = 12007,
	-- 获取用户坐标返回
	MsgIdxRspGetUserCoord = 12008,
	-- 通知用户信息改变
	MsgIdxNotifyUserInfoChange = 12010,
	-- 通知Item发生改变
	MsgIdxNotifyItemChange = 12012,
	-- 请求用户游戏资料
	MsgIdxReqUserGameData = 12013,
	-- 请求用户游戏资料返回
	MsgIdxRspUserGameData = 12014,
	-- 请求用户基本资料
	MsgIdxReqBaseUserInfo = 12015,
	-- 请求用户基本资料返回
	MsgIdxRspBaseUserInfo = 12016,
	-- 请求提现
	MsgIdxReqGetMoney = 12017,
	-- 请求提现返回
	MsgIdxRspGetMoney = 12018,
	-- 通知用户获取到掉落奖励
	MsgIdxNotifyGetLoot = 12020,
	-- 请求添加相册图片
	MsgIdxReqAddAlbumPic = 12121,
	-- 请求添加相册图片返回
	MsgIdxRspAddAlbumPic = 12122,
	-- 请求相册列表
	MsgIdxReqAlbumPicList = 12123,
	-- 请求相册列表返回
	MsgIdxRspAlbumPicList = 12124,
	-- 请求删除相册
	MsgIdxReqDelAlbumPic = 12125,
	-- 请求删除相册返回
	MsgIdxRspDelAlbumPic = 12126,
	-- 请求修改相册排序
	MsgIdxReqMoidfyAlbumPic = 12127,
	-- 请求修改相册排序返回
	MsgIdxRspMoidfyAlbumPic = 12128,
	-- 请求提现CD剩余时间
	MsgIdxReqGetMoneyCD = 12129,
	-- 请求提现CD剩余时间返回
	MsgIdxRspGetMoneyCD = 12130,
	-- 请求绑定第三方账号
	MsgIdxReqBindThirdPartyAccount = 12131,
	-- 请求绑定第三方账号返回
	MsgIdxRspBindThirdPartyAccount = 12132,
	-- 请求支付宝签名
	MsgIdxReqAliPaySign = 12133,
	-- 请求支付宝签名返回
	MsgIdxRspAliPaySign = 12134,
}
ProtoEnumUserInfo.LootSource =
{
	-- 在线红包
	LootSource_Online_RedPacket = "LootSource_Online_RedPacket",
	-- 邮件奖励
	LootSource_MailReward = "LootSource_MailReward",
	-- 签到奖励
	LootSource_SignReward = "LootSource_SignReward",
	-- 月签到奖励
	LootSource_MonSignReward = "LootSource_MonSignReward",
	-- 一键领取邮件
	LootSource_AllMailReward = "LootSource_AllMailReward",
}
ProtoEnumUserInfo.GetMoneyResult =
{
	-- 成功
	GetMoneyResult_Success = "GetMoneyResult_Success",
	-- 失败
	GetMoneyResult_Fail = "GetMoneyResult_Fail",
	-- 提现账号不存在
	GetMoneyResult_InvalidAccount = "GetMoneyResult_InvalidAccount",
	-- 余额不足
	GetMoneyResult_NoEnough = "GetMoneyResult_NoEnough",
	-- 提现冷却中
	GetMoneyResult_NoReady = "GetMoneyResult_NoReady",
}
ProtoEnumUserInfo.ReqAddAlbumPicResult =
{
	-- 成功
	ReqAddAlbumPicSuccess = "ReqAddAlbumPicSuccess",
	-- 失败
	ReqAddAlbumPicFail = "ReqAddAlbumPicFail",
}
ProtoEnumUserInfo.BindAccountResult =
{
	-- 成功
	BindAccountResultSuccess = "BindAccountResultSuccess",
	-- 已经存在
	BindAccountResultDublicated = "BindAccountResultDublicated",
	-- 失败
	BindAccountResultFail = "BindAccountResultFail",
}
