RechargeManager = {}

local m_succeedCallback = nil

--打开商城
--int shopType	商城类型 1.钻石 2.金币 3.优卡
--int fromType	来源类型 0.平台 >0.游戏id
--function succeedCallback	充值兑换成功回调
function RechargeManager.openShop(shopType, fromType, succeedCallback)
	local msg = {}
	msg.shopType = shopType
	msg.fromType = fromType
	msg.succeedCallback = succeedCallback
	ViewManager.open(UIViewEnum.Platform_Mall_View, msg)
end

--打开横版商城
--int shopType	商城类型 1.钻石 2.金币 3.优卡
--int fromType	来源类型 0.平台 >0.游戏id
--function succeedCallback	充值兑换成功回调
function RechargeManager.openShopLands(shopType, fromType, succeedCallback)
	local msg = {}
	msg.shopType = shopType
	msg.fromType = fromType
	msg.succeedCallback = succeedCallback
	ViewManager.open(UIViewEnum.Platform_Mall_Lands_View, msg)
end

--根据需要消费的金币数量查询需要兑换的钻石数量或需要充值的金额
--1.平台会查询不低于该金币数量的最小兑换档位
--2.若钻石数量足够，则返回该最小兑换档位
--3.若钻石数量不足，则返回不低于缺少钻石数量的最小充值档位
--int money	需要消费的金币数量
--function resultCallback(rechargeItemId, rechargeCash, rechargeDiamond, exchangeItemId, exchangeDiamond, exchangeMoney) 查询结果回调
	--int rechargeItemId 充值商品Id
	--int rechargeCash 充值需要现金(RMB)
	--int rechargeDiamond 充值获得钻石
	--int exchangeItemId 兑换商品Id
	--int exchangeDiamond 兑换需要钻石
	--int exchangeMoney 充值获得金币
	--rechargeItemId, exchangeItemId为0，代表查询失败
	--仅rechargeItemId为0，代表不需要充值，只需要兑换
function RechargeManager.checkRechargeAndExchange(money, resultCallback)
	if resultCallback == nil then
		return
	end
	local exchangeItemId = 0
	local exchangeDiamond = 0
	local exchangeMoney = 0
	--遍历兑换表
	for k,v in pairs(TableBaseShopExchange.data) do
		if v.src_type == 2 and v.dest_type == 3 and v.dest_item >= money then
			if exchangeMoney == 0 or v.dest_item < exchangeMoney then
				exchangeItemId = v.id
				exchangeDiamond = v.src_item
				exchangeMoney = v.dest_item
			end
		end
	end
	if exchangeMoney == 0 then
		resultCallback(0, 0, 0, 0, 0, 0)
		return
	end
	local curDiamond = CommonDataProxy.getDiamond()
	if curDiamond >= exchangeDiamond then
		resultCallback(0, 0, 0, exchangeDiamond, exchangeMoney)
		return
	end
	
	--钻石不足
	local needDiamond = exchangeDiamond - curDiamond
	local rechargeItemId = 0
	local rechargeCash = 0
	local rechargeDiamond = 0
	--遍历充值表
	for k,v in pairs(TableBaseRechargeShop.data) do
		if v.type == 2 and v.item >= needDiamond then
			if rechargeDiamond == 0 or v.item < rechargeDiamond then
				rechargeItemId = v.id
				rechargeCash = v.cash
				rechargeDiamond = v.item
			end
		end
	end
	if exchangeMoney == 0 then
		resultCallback(0, 0, 0, 0, 0, 0)
		return
	end
	resultCallback(rechargeItemId, rechargeCash, rechargeDiamond, exchangeItemId, exchangeDiamond, exchangeMoney)
end

--充值
--string billingType		充值类型 0.用户版 1.商家版
--string token				登录token
--string itemId				商品Id
--string playerId			玩家Id
--string payType			充值类型 0.支付宝 1.微信
--function succeedCallback	成功回调
function RechargeManager.recharge(billingType, token, itemId, playerId, payType, succeedCallback)
	printDebug("充值")
	ShowWaiting(true, "recharge")
	m_succeedCallback = succeedCallback
	local param = "billingType="..billingType.."&token="..token.."&itemId="..itemId.."&playerId="..playerId.."&payType="..payType
	ProtobufManager.sendHttpRecharge("player_recharge", param, function (rsp)
		if rsp == nil then
			ShowWaiting(false, "recharge")
			showFloatTips("充值出错")
			return
		end
		printDebug("充值返回:"..table.tostring(rsp))
		if rsp.result == 200 then
			if payType == "0" then
				printDebug("支付宝充值")
				PlatformSDK.aliPay(rsp.data, function (resultStatus)
					printDebug("支付宝充值返回")
					if resultStatus == "9000" or resultStatus == "8000" or resultStatus == "6004" then
						RechargeManager.checkOrder("0", LoginDataProxy.token, rsp.out_trade_no, playerId)
					else
						ShowWaiting(false, "recharge")
						if resultStatus == "4000" then
							showFloatTips("支付失败")
						elseif resultStatus == "5000" then
							showFloatTips("重复请求")
						elseif resultStatus == "6001" then
							showFloatTips("支付取消")
						elseif resultStatus == "6002" then
							showFloatTips("网络连接出错")
						else
							showFloatTips("支付错误("..resultStatus..")")
						end
					end
				end)
			elseif payType == "1" then
				printDebug("微信充值")
				PlatformSDK.wxPay(rsp.prepay_id, rsp.noncestr, rsp.sign, rsp.timestamp, rsp.package, function (resultStatus)
					printDebug("微信充值返回"..resultStatus)
					if resultStatus == "0" then
						RechargeManager.checkOrder("0", LoginDataProxy.token, rsp.out_trade_no, playerId)
					else
						ShowWaiting(false, "recharge")
						if resultStatus == "-1" then
							showFloatTips("支付失败")
						elseif resultStatus == "-2" then
							showFloatTips("支付取消")
						else
							showFloatTips("支付错误("..resultStatus..")")
						end
					end
				end)
			end
		else
			ShowWaiting(false, "recharge")
			showFloatTips("充值出错("..rsp.result..")")
		end
	end)
end

--查询订单
--string billingType		充值类型 0.用户版 1.商家版
--string token				登录token
--string out_trade_no		商品Id
--string playerId			玩家Id
function RechargeManager.checkOrder(billingType, token, out_trade_no, playerId)
	printDebug("充值查询订单")
	local param = "billingType="..billingType.."&token="..token.."&account_type=0&out_trade_no="..out_trade_no.."&playerId="..playerId
	ProtobufManager.sendHttpRecharge("player_getorder", param, function (rsp)
		if rsp == nil then
			RechargeManager.checkOrderDelay("0", LoginDataProxy.token, out_trade_no, playerId)
			return
		end
		printDebug("充值查询订单返回"..table.tostring(rsp))
		if rsp.result == 200 then
			ShowWaiting(false, "recharge")
			--showFloatTips("充值成功")
			
			--刷新货币金额
			PlatformUserProxy:GetInstance():updateRechargeResult(tonumber(rsp.cash), tonumber(rsp.diamond), tonumber(rsp.money))
			
			--成功回调
			if m_succeedCallback ~= nil then
				m_succeedCallback()
			end
		else
			RechargeManager.checkOrderDelay("0", LoginDataProxy.token, out_trade_no, playerId)
		end
	end)
end

--延迟查询订单
--string billingType		充值类型 0.用户版 1.商家版
--string token				登录token
--string out_trade_no		商品Id
--string playerId			玩家Id
function RechargeManager.checkOrderDelay(billingType, token, out_trade_no, playerId)
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("RechargeCheckOrder")
	GlobalTimeManager.Instance.timerController:AddTimer("RechargeCheckOrder", 3000, 1, function ()
		RechargeManager.checkOrder(billingType, token, out_trade_no, playerId)
	end)
end

--兑换
--string billingType		充值类型 0.用户版 1.商家版
--string token				登录token
--string itemId				商品Id
--string playerId			玩家Id
--string payType			充值类型 0.支付宝 1.微信
--function succeedCallback	成功回调
function RechargeManager.exchange(billingType, token, itemId, playerId, succeedCallback)
	printDebug("兑换")
	ShowWaiting(true, "recharge")
	m_succeedCallback = succeedCallback
	local param = "billingType="..billingType.."&token="..token.."&itemId="..itemId.."&playerId="..playerId
	ProtobufManager.sendHttpRecharge("player_shopexchange", param, function (rsp)
		if rsp == nil then
			ShowWaiting(false, "recharge")
			showFloatTips("兑换出错")
			return
		end
		printDebug("兑换返回:"..table.tostring(rsp))
		if rsp.result == 200 then
			ShowWaiting(false, "recharge")
			--showFloatTips("兑换成功")
			
			--刷新货币金额
			PlatformUserProxy:GetInstance():updateRechargeResult(tonumber(rsp.cash), tonumber(rsp.diamond), tonumber(rsp.money))
			
			--成功回调
			if m_succeedCallback ~= nil then
				m_succeedCallback()
			end
		else
			ShowWaiting(false, "recharge")
			showFloatTips("兑换出错("..rsp.result..")")
		end
	end)
end

--充值兑换
--string billingType		充值类型 0.用户版 1.商家版
--string rechargeItemId		充值商品Id
--string playerId			玩家Id
--string payType			充值类型 0.支付宝 1.微信
--string exchangeItemId		兑换商品Id
--function succeedCallback	成功回调
function RechargeManager.rechargeAndExchange(billingType, rechargeItemId, playerId, payType, exchangeItemId, succeedCallback)
	printDebug("充值兑换")
	ShowWaiting(true, "recharge")
	m_succeedCallback = succeedCallback
	local param = "billingType="..billingType.."&itemId="..rechargeItemId.."&playerId="..playerId.."&payType="..payType.."&exchangeItemId="..exchangeItemId
	ProtobufManager.sendHttpRecharge("player_recharge", param, function (rsp)
		if rsp == nil then
			ShowWaiting(false, "recharge")
			showFloatTips("充值兑换出错")
			return
		end
		printDebug("充值兑换返回:"..table.tostring(rsp))
		if rsp.result == 200 then
			if payType == "0" then
				printDebug("支付宝充值")
				PlatformSDK.aliPay(rsp.data, function (resultStatus)
					printDebug("支付宝充值返回")
					if resultStatus == "9000" or resultStatus == "8000" or resultStatus == "6004" then
						RechargeManager.checkOrder("0", LoginDataProxy.token, rsp.out_trade_no, playerId)
					else
						ShowWaiting(false, "recharge")
						if resultStatus == "4000" then
							showFloatTips("支付失败")
						elseif resultStatus == "5000" then
							showFloatTips("重复请求")
						elseif resultStatus == "6001" then
							showFloatTips("支付取消")
						elseif resultStatus == "6002" then
							showFloatTips("网络连接出错")
						else
							showFloatTips("支付错误("..resultStatus..")")
						end
					end
				end)
			end
		else
			ShowWaiting(false, "recharge")
			showFloatTips("充值兑换出错("..rsp.result..")")
		end
	end)
end