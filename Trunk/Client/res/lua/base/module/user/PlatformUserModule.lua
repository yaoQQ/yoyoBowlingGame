--用户模块
PlatformUserModule = BaseModule:new()
local this = PlatformUserModule
this.moduleName = "PlatformUser"

------------------------------注册由服务器发来的协议------------------------------
function this.initRegisterNet()
	this.netFuncList = {} 
	--请求修改用户信息返回
	this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxRspChangeUserBaseInfo, this.onRspChangeUserBaseInfo)
	--请求更新用户位置返回
	this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxRspUpUserPosition, this.onRspUpUserPosition)
	--通知Item发生改变
	this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxNotifyItemChange, this.onNotifyItemChange)
	--请求用户基本资料返回
    this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxRspBaseUserInfo, this.onRspBaseUserInfo)
	--通知用户获取到掉落奖励
    this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxNotifyGetLoot, this.onNotifyGetLoot)
	--请求提现返回
    this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxRspGetMoney, this.onRspGetMoney)
	--请求提现CD结束时间返回
	this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxRspGetMoneyCD, this.onRspGetMoneyCD)
	--请求增加用户图片返回
	this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxRspAddAlbumPic, this.onRspAddAlbumPic)
	--请求相册图片列表返回
	this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxRspAlbumPicList,this.onRspAlbumPicList)
	--请求刪除用户图片返回
	this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxRspDelAlbumPic, this.onRspDelAlbumPic)
	--请求修改相册图片返回
	this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxRspMoidfyAlbumPic, this.onRspMoidfyAlbumPic)
	--请求支付宝签名返回
	this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxRspAliPaySign, this.onRspAliPaySign)
	--请求绑定第三方账号返回
	this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxRspBindThirdPartyAccount, this.onRspBindThirdPartyAccount)
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
		self.switch = {}
	end
    return self.notificationList
end

------------------------------发协议------------------------------

--[[message ReqChangeUserBaseInfo
{ 
	// 协议号
	optional MsgIdx msg_idx = 1 [default = MsgIdxReqChangeUserBaseInfo];
	// 昵称
	optional string nick_name = 2;
	// 性别：1男  2女
	optional int32 sex = 3;
	// 用户头像图片下载链接
	optional string head_url = 4;
	// 显示地址，默认true
	optional bool show_address = 5;
	// 地址
	optional string address = 6;
	// 出生日期
	optional int64 birthday = 7;
} --]]
--请求修改用户基本信息
function this.sendReqChangeUserBaseInfo(req)
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "userinfo", "ReqChangeUserBaseInfo", req)
end

--请求更新用户位置
function this.sendReqUpUserPosition(lng, lat, city_code)
    local req = {}
    req.longitude = lng
    req.latitude = lat
	req.city_code = city_code
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "userinfo", "ReqUpUserPosition", req)
end

--请求用户基本资料
function this.sendReqBaseUserInfo(player_id)
    local req = {}
    req.player_id = player_id
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "userinfo", "ReqBaseUserInfo", req)
end

-- 请求提现
function this.sendReqGetMoney(pay_type, id)
	local req  = {}
	req.pay_type = pay_type
	req.id = id
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "userinfo", "ReqGetMoney", req)
end

--请求提现CD结束时间
function this.sendGetMoneyCD()
    local req = {}
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "userinfo", "ReqGetMoneyCD", req)
end

--请求添加相册图片
function this.sendReqAddAlbumPic(album_pic_info_list)
	local req = {}
	req.album_pic_info_list = album_pic_info_list
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "userinfo", "ReqAddAlbumPic", req)
end

--请求删除相册图片
function this.sendReqDelAlbumPic(id_list)
	local req = {}
	req.id_list = id_list
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "userinfo", "ReqDelAlbumPic", req)
end

--请求修改相册图片
function this.sendReqMoidfyAlbumPic(album_pic_info_list)
	local req = {}
	req.album_pic_info_list = album_pic_info_list
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "userinfo", "ReqMoidfyAlbumPic", req)
end

--请求支付宝签名
function this.sendReqAliPaySign()
	local req = {}
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "userinfo", "ReqAliPaySign", req)
end

--请求绑定第三方账号
--account_type  第三方账号类型，0支付宝，1微信
--auth_code  授权码
function this.sendReqBindThirdPartyAccount(account_type, auth_code)
	local req = {}
	req.account_type = account_type
	req.auth_code = auth_code
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "userinfo", "ReqBindThirdPartyAccount", req)
end

------------------------------收协议------------------------------

--请求修改用户基本信息返回
function this.onRspChangeUserBaseInfo(protoBytes)
	local rsp = this.decodeProtoBytes("userinfo", "RspChangeUserBaseInfo", protoBytes)

	PlatformUserProxy:GetInstance():changeUserBaseInfo(rsp)
end

--请求更新用户位置返回
function this.onRspUpUserPosition(protoBytes)
    local rsp = this.decodeProtoBytes("userinfo", "RspUpUserPosition", protoBytes)
    printDebug("请求更新用户位置返回的消息：" .. table.tostring(rsp))
    if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        printDebug("请求更新用户位置成功")
    elseif rsp.result == ProtoEnumCommon.ReqResult.ReqResultFail then
        printDebug("请求更新用户位置失败:")
    elseif rsp.result == ProtoEnumCommon.ReqResult.ReqResultFailNoFound then
    end
end

--通知Item发生改变
function this.onNotifyItemChange(protoBytes)
	local notify = this.decodeProtoBytes("userinfo", "NotifyItemChange", protoBytes)
	
	PlatformUserProxy:GetInstance():updateUserItem(notify.item_info)
end

--请求用户基本资料返回
function this.onRspBaseUserInfo(protoBytes)
    local rsp = this.decodeProtoBytes("userinfo", "RspBaseUserInfo", protoBytes)
    if rsp.result ~= 0 then
        Alert.showAlertMsg(nil, "用户不存在，请重新搜索", "确定")
        PlatformCommonSearchView:clearInputTxt()
        return
    end
    PlatformFriendProxy:GetInstance():setBaseUserInfo(rsp)
    if PlatformFriendRecommendView:getIsOpen() then
        PlatformFriendRecommendView:updateFriendSearchList()
    end

    PlatformCommonSearchView:clearInputTxt()
end

--通知用户获取到掉落奖励
function this.onNotifyGetLoot(protoBytes)
    local rsp = this.decodeProtoBytes("userinfo", "NotifyGetLoot", protoBytes)
    
	ViewManager.open(UIViewEnum.Platform_Message_Reward_View, rsp)
end

--请求提现返回
function this.onRspGetMoney(protoBytes)
    local rsp = this.decodeProtoBytes("userinfo", "RspGetMoney", protoBytes)
	
    PlatformUserModule.sendGetMoneyCD()
	
    if rsp.result == ProtoEnumUserInfo.GetMoneyResult.GetMoneyResult_Success then
        local baseData = PlatformUserProxy:GetInstance():getUserInfo()
        printDebug("baseData" .. baseData.cash)
        Alert.showAlertMsg("提现成功", "请到支付宝账号查收!", "确定")
    elseif ProtoEnumUserInfo.GetMoneyResult.GetMoneyResult_Fail == rsp.result then
        Alert.showAlertMsg(nil, "提现失败("..rsp.error_code..")", "确定")
    elseif ProtoEnumUserInfo.GetMoneyResult.GetMoneyResult_InvalidAccount == rsp.result then
        Alert.showAlertMsg(nil, "提现账号不存在", "确定")
    elseif ProtoEnumUserInfo.GetMoneyResult.GetMoneyResult_NoEnough == rsp.result then
        Alert.showAlertMsg(nil, "余额不足", "确定")
    end
	
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Rsp_Get_Money, rsp)
end

--请求提现CD结束时间返回
function this.onRspGetMoneyCD(protoBytes)
	local rsp = this.decodeProtoBytes("userinfo", "RspGetMoneyCD", protoBytes)
	PlatformRedBagProxy:GetInstance():setCDData({remain_time = rsp.remain_time})
	ViewManager.open(UIViewEnum.Platform_RedBag_WithDraw_View)
    --ViewManager.open(UIViewEnum.Platform_RedBag_WithDraw_View, {remain_time = rsp.remain_time})
end

--请求添加相册图片返回
function this.onRspAddAlbumPic(protoBytes)
	local rsp = this.decodeProtoBytes("userinfo", "RspAddAlbumPic", protoBytes)
	
	MainModule.sendReqAlbumPicList(LoginDataProxy.playerId)
end

--请求相册图片列表返回
function this.onRspAlbumPicList(protoBytes)
	local rsp = this.decodeProtoBytes("userinfo", "RspAlbumPicList", protoBytes)

	PlatformUserProxy:GetInstance():setUserPhotosData(rsp)
end

--请求刪除用户图片返回
function this.onRspDelAlbumPic(protoBytes)
	local rsp = this.decodeProtoBytes("userinfo", "RspDelAlbumPic", protoBytes)
	MainModule.sendReqAlbumPicList(LoginDataProxy.playerId)
	if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
		for i = 1, #rsp.url_list do
			PlatformPicManagerProxy:GetInstance():deleteSprite(rsp.url_list[i])
			EnlargePhotoView:deletePhoto(rsp.url_list[i])
		end
	end
end

--请求排序用户图片返回
function this.onRspMoidfyAlbumPic(protoBytes)
	local rsp = this.decodeProtoBytes("userinfo","RspMoidfyAlbumPic", protoBytes)
	if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
		MainModule.sendReqAlbumPicList(LoginDataProxy.playerId)
	else
		printError("错误-请求排序相册: "..rsp.result)
	end
end

--请求支付宝签名返回
function this.onRspAliPaySign(protoBytes)
    local rsp = this.decodeProtoBytes("userinfo", "RspAliPaySign", protoBytes)
	
	if IS_UNITY_EDITOR then
		showFloatTips("PC版不支持支付宝绑定")
		return
	end
	
	PlatformSDK.aliPayAuth(rsp.sign)
end

--请求绑定第三方账号返回
function this.onRspBindThirdPartyAccount(protoBytes)
	local rsp = this.decodeProtoBytes("userinfo", "RspBindThirdPartyAccount", protoBytes)
	
	if rsp.result == ProtoEnumUserInfo.BindAccountResult.BindAccountResultSuccess then
		showFloatTips("绑定成功")
		PlatformUserProxy:GetInstance():updateAlipayNick(rsp.nick_name)
	elseif rsp.result == ProtoEnumUserInfo.BindAccountResult.BindAccountResultDublicated then
		showFloatTips("该支付宝账号已被绑定过")
	elseif rsp.result == ProtoEnumUserInfo.BindAccountResult.BindAccountResultFail then
		showFloatTips("绑定失败")
	end
end