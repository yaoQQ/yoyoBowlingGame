--协议工具生成的代码，不要手动修改
ProtoEnumCoupon = {}
ProtoEnumCoupon.MsgIdx =
{
	-- 请求玩家优惠券列表
	MsgIdxReqGetPlayerCouponList = 13001,
	-- 请求玩家优惠券列表返回
	MsgIdxRspGetPlayerCouponList = 13002,
	-- 请求领取优惠券
	MsgIdxReqRcvCoupon = 13003,
	-- 请求领取优惠券返回
	MsgIdxRspRcvCoupon = 13004,
	-- 请求删除优惠券
	MsgIdxReqDelCoupon = 13005,
	-- 请求删除优惠券返回
	MsgIdxRspDelCoupon = 13006,
	-- 通知核销优惠券信息
	MsgIdxNotifyConsumeCoupon = 13007,
	-- 请求商店优惠卷信息返回
	MsgIdxRspShopCouponInfo = 12802,
	-- 请求商店优惠卷信息
	MsgIdxReqShopCouponInfo = 12801,
	-- 请求附近优惠卷
	MsgIdxReqFindNearCoupon = 12803,
	-- 请求附近优惠卷返回
	MsgIdxRspFindNearCoupon = 12804,
}
ProtoEnumCoupon.RspShopCouponInfoResult =
{
	-- 成功
	RspShopCouponInfoResult_Success = "RspShopCouponInfoResult_Success",
	-- 失败
	RspShopCouponInfoResult_Failed = "RspShopCouponInfoResult_Failed",
	-- 商店不存在
	RspShopCouponInfoResult_ShopNotExits = "RspShopCouponInfoResult_ShopNotExits",
	-- 参数错误
	RspShopCouponInfoResult_ParamErr = "RspShopCouponInfoResult_ParamErr",
}
