PlatformSDK = {}

--重启应用
function PlatformSDK.restartApp()
	CSPlatformSDK.RestartApp()
end

function PlatformSDK.activeInitializeUI()
	if IS_IOS then
		CSPlatformSDK.ActiveInitializeUI()
	end
end

function PlatformSDK.getSimOperator()
	return CSPlatformSDK.GetSimOperator()
end

local m_wxAuthSucceedCallback = nil
local m_wxAuthFailCallback = nil
--微信登录
function PlatformSDK.wxLogin(succeedCallback, failCallback)
	if IS_UNITY_EDITOR then
		showFloatTips("PC版不支持微信登录")
		return
	end

	if CSPlatformSDK.IsWXAppInstalled() then
		printDebug("已安装微信客户端")
		m_wxAuthSucceedCallback = succeedCallback
		m_wxAuthFailCallback = failCallback
		PlatformSDK.wxSendAuth()
	else
		printDebug("未安装微信客户端")
		showFloatTips("请先安装微信")
	end
end

--微信绑定
function PlatformSDK.wxBind(succeedCallback, failCallback)
	if IS_UNITY_EDITOR then
		showFloatTips("PC版不支持微信绑定")
		return
	end
	
	if CSPlatformSDK.IsWXAppInstalled() then
		printDebug("已安装微信客户端")
		m_wxAuthSucceedCallback = succeedCallback
		m_wxAuthFailCallback = failCallback
		PlatformSDK.wxSendAuth()
	else
		printDebug("未安装微信客户端")
		showFloatTips("请先安装微信")
	end
end

--微信授权
function PlatformSDK.wxSendAuth()
	CSPlatformSDK.WxSendAuth()
end

--微信授权成功
function PlatformSDK.wxAuthSucceed(authCode)
	if m_wxAuthSucceedCallback ~= nil then
		m_wxAuthSucceedCallback(authCode)
	end
end

--微信授权失败
function PlatformSDK.wxAuthFail(errCode)
	if m_wxAuthFailCallback ~= nil then
		m_wxAuthFailCallback(errCode)
	end
end

--微信充值
function PlatformSDK.wxPay(prepayId, nonceStr, sign, timeStamp, packageValue, callback)
	CSPlatformSDK.WxPay(prepayId, nonceStr, sign, timeStamp, packageValue, function (resultStatus)
		if callback ~= nil then
			callback(resultStatus)
		end
	end)
end

local m_aliPayAuthSucceedCallback = nil
local m_aliPayAuthFailCallback = nil
--支付宝登录
function PlatformSDK.aliPayLogin(succeedCallback, failCallback)
	m_aliPayAuthSucceedCallback = succeedCallback
	m_aliPayAuthFailCallback = failCallback
	LoginModule.sendReqAliPaySign()
end

--支付宝绑定
function PlatformSDK.aliPayBind(succeedCallback, failCallback)
	m_aliPayAuthSucceedCallback = succeedCallback
	m_aliPayAuthFailCallback = failCallback
	PlatformUserModule.sendReqAliPaySign()
end

--支付宝授权
function PlatformSDK.aliPayAuth(sign)
	--拉起支付宝SDK授权
	CSPlatformSDK.AlipaySendAuth(sign)
end

--支付宝授权成功
function PlatformSDK.aliPayAuthSucceed(authCode)
	if m_aliPayAuthSucceedCallback ~= nil then
		m_aliPayAuthSucceedCallback(authCode)
	end
end

--支付宝授权失败
function PlatformSDK.aliPayAuthFail(errCode)
	if m_aliPayAuthFailCallback ~= nil then
		m_aliPayAuthFailCallback(errCode)
	end
end

--支付宝充值
--[[
resultStatus结果码含义
返回码	含义
9000	订单支付成功
8000	正在处理中，支付结果未知（有可能已经支付成功），请查询商户订单列表中订单的支付状态
4000	订单支付失败
5000	重复请求
6001	用户中途取消
6002	网络连接出错
6004	支付结果未知（有可能已经支付成功），请查询商户订单列表中订单的支付状态
其它	其它支付错误
--]]
function PlatformSDK.aliPay(orderInfo, callback)
	CSPlatformSDK.AliPay(orderInfo, function (resultStatus)
		if callback ~= nil then
			callback(resultStatus)
		end
	end)
end

--isShow显示、隐藏状态栏，isWhite true白色字体  false黑色
function PlatformSDK.showStatusBar(isShow,isWhite)
	CSPlatformSDK.ShowStatusBar(isShow,isWhite)
end

function PlatformSDK.takePhonePhoto(isFromCamera, callback, isCut, width, height)
	CSPlatformSDK.TakePhonePhoto(isFromCamera, function (sprite, bytes)
		if callback ~= nil then
			callback(sprite, bytes)
		end
	end, isCut, width, height)
end

function PlatformSDK.scanQRCode()
	local errorQRCode = function ()
		Alert.showAlertMsg(nil, "无效的二维码", "确定", nil)
	end
	
	CSPlatformSDK.ScanQRCode(function (str)
		print("QRCode:"..str)
		local strs = string.split(str, "&")
		
		if #strs ~= 3 or strs[1] ~= "yoyo" then
			errorQRCode()
			return
		end
		
		if strs[2] == "add_friend" then
			PlatformUserModule.sendReqBaseUserInfo(tonumber(strs[3]))
		else
			errorQRCode()
		end
	end, nil, nil)
end