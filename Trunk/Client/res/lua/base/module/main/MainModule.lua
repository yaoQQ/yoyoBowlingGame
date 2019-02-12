require "base:manager/MapManager"
require "base:module/main/data/MapPos"
require "base:enum/GlobalOnceData"

MainModule = BaseModule:new()
local this = MainModule
this.moduleName = "Main"

------------------------------注册由服务器发来的协议------------------------------
function this.initRegisterNet()
    this.netFuncList = {}
    this:AddNetLister(ProtoEnumUserInfo.MsgIdx.MsgIdxNotifyLoginComplete, this.onNotifyLoginComplete)
    --登录完成返回，所有的协议要在收到这个返回之后才能够请求

    --通知计数器重置
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxNotifyDailyCounterChange, this.onNotifyDailyCounterChange)

    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGMCommand, this.onRspMainGMCommand)
end

------------------------------由服务器发来的协议响应------------------------------
function this.onNetMsgLister(protoID, protoBytes)
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
        --全局的通知在这里注册
        self:AddNotifictionLister(NoticeType.GM_Send_To_MainGateway, this.onGMSend2MainGateway)
        self:AddNotifictionLister(NoticeType.Game_Exit, this.onGameExit)
        self:AddNotifictionLister(NoticeType.Game_Open_Shop, this.onGameOpenShop)
        self:AddNotifictionLister(NoticeType.Game_Open_Shop_Lands, this.onGameOpenShopLands)
    end
    return self.notificationList
end

function this.onGMSend2MainGateway(noticeType, notice)
    local cmd = notice:GetObj()
    this.sendGM2MainGateway(cmd)
end

function this.onGameExit(noticeType, notice)
    --local gameId = notice:GetObj()
    GameManager.exitGame()
end

function this.onGameOpenShop(noticeType, notice)
    local shopType = notice:GetObj()
    RechargeManager.openShop(shopType, 3)
    --TODO
end

function this.onGameOpenShopLands(noticeType, notice)
    local shopType = notice:GetObj()
    RechargeManager.openShopLands(shopType, 3)
    --TODO
end

------------------------------发协议------------------------------

--发送设备信息
function this.sendReqDeviceLogInfo()
    local req = {}
	if IS_ANDROID then
		req.os = 2
	elseif IS_IOS then
		req.os = 1
	else
		req.os = 0
	end
    req.os_ver = DeviceUtil.GetOperatingSystem()
    req.memory = DeviceUtil.GetSystemMemorySize()
	req.mobile_operator = PlatformSDK.getSimOperator()
	req.cpu_model = DeviceUtil.GetProcessorType()
	req.disk_capacity = ""
	req.brand = DeviceUtil.GetDeviceModel()
	req.screen_size = tostring(math.floor(Main.screenWidthHeight.x)).."*"..tostring(math.floor(Main.screenWidthHeight.y))
	if UtilMethod.channel == nil then
		req.line_no = "00000000"
	else
		req.line_no = tostring(UtilMethod.channel)
	end
	req.player_id = LoginDataProxy.playerId
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqDeviceLogInfo", req)
end

--发送GM指令
function this.sendGM2MainGateway(cmd)
    local result = string.split(cmd, "|")
    if result[1] == "-pb" then
        --发送协议
        local packageName = result[2]
        local protoName = result[3]
        local req = JsonUtil.decode(result[4])
        this.sendNetMsg(GameConfig.ServerName.MainGateway, packageName, protoName, req)
    else
        --发送GM指令
        local req = {}
        req.cmd = cmd
        this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGMCommand", req)
    end
end

--请求相册图片列表（根据添加时间倒序）
function this.sendReqAlbumPicList(player_id)
    local req = {}
    req.player_id = player_id
    req.page_index = 0
    req.per_page_count = 1000
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "userinfo", "ReqAlbumPicList", req)
end

function this.sendReqGetHotGame()
    local req = {}
    --printDebug("请求热门游戏信息:" .. table.tostring(req))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetHotGame", req)
end

------------------------------收协议------------------------------

--服务器通知登录完成
function this.onNotifyLoginComplete(protoBytes)
    printDebug("服务器通知登录完成，先请求用户信息，收到用户信息返回后进入平台，再请求其他信息")
    local rsp = this.decodeProtoBytes("userinfo", "NotifyLoginComplete", protoBytes)
    this.sysFriendInfo = rsp.sys_notify_info.friend_notify_info
    this.sysMailInfo = rsp.sys_notify_info.mail_notify_info
    --用户信息
    LoginDataProxy.isGetUserInfo = true

    PlatformUserProxy:GetInstance():setUserInfo(rsp.user_info)
    PlatformUserProxy:GetInstance():setDailyData(rsp.usr_daily_update)

    if PlatformGlobalRedBagView ~= nil and PlatformGlobalRedBagView.isOpen then
        PlatformGlobalRedBagView:updateRedBagPanel()
    end

    if not GlobalOnceData.hasLoginRecord then
        printDebug("AudioManager.loginRecord")
        AudioManager.loginRecord(rsp.user_info.nick_name, rsp.user_info.player_id)
        GlobalOnceData.hasLoginRecord = true
    end
    this.LogoinInitData()
    --收到用户信息才算真正意义上的登录流程结束，这时候才关闭login的waiting界面
    ShowWaiting(false, "login")
	
	--发送设备信息
	this.sendReqDeviceLogInfo()
	
	--如果今天没签到，弹出签到界面
	if rsp.is_sign_today == 0 then
		ViewManager.open(UIViewEnum.Platform_Sign_View)
	end
end

--登录请求协议用于切换账号或登录时拉取
function this.LogoinInitData()
    --请求好友列表
    local data = {
        op = ProtoEnumFriendModule.FriendOp.FriendOpReqList,
        param1 = 0,
        param2 = 100
    }
    this.setIsLoginReqFriendList(true)
    PlatformFriendModule.onReqFriendOp(data)
    PlatformMessageModule.sendReqGetUserNotifyMsg()
    --离线消息相关处理
    if this.sysFriendInfo and this.sysFriendInfo.msg_flag == true then
        local info = {}
        info.page_index = 0
        info.per_page_count = 50
        PlatformFriendModule.onReqOfflineChat(info)
    end

    --离线好友申请相关处理
    if this.sysFriendInfo and this.sysFriendInfo.apply_flag == true then
        --请求好友申请列表
        local data = {
            op = ProtoEnumFriendModule.FriendOp.FriendOpReqApplyList,
            param1 = 0,
            param2 = 100
        }
        PlatformFriendModule.onReqFriendOp(data)
    end

    --请求邮件列表
    local data = {
        mail_type = ProtoEnumCommon.MailType.MailType_Normal,
        page_index = 0,
        per_page_count = 100
    }
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Mail, data)
end

--通知计数器重置
function this.onNotifyDailyCounterChange(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "NotifyDailyCounterChange", protoBytes)
    PlatformUserProxy:GetInstance():setDailyData(rsp)
end

--用于过滤登录时好友列表的请求
function this.setIsLoginReqFriendList(value)
    this.isLoginReqFriendList = value
end

--用于过滤登录时好友列表的请求
function this.getIsLoginReqFriendList(value)
    return this.isLoginReqFriendList
end

function this.onRspMainGMCommand(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspGMCommand", protoBytes)
    UtilMethod.SetSendGMResult(rsp.result == 0)
end