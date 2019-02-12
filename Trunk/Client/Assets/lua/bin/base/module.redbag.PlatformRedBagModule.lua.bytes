require "base:module/redbag/data/PlatformRedBagProxy"
	
--红包模块
PlatformRedBagModule = BaseModule:new()
local this = PlatformRedBagModule
this.moduleName = "PlatformRedBag"

------------------------------注册由服务器发来的协议------------------------------
function this:initRegisterNet()
	this.netFuncList = {}
	--请求偷红包返回
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspStealOnlineRedPacket, this.onRspStealOnlineRedPacket)
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspOnlineRedPacketShareRewards, this.onRspOnlineRedPacketShareRewards)
end
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
		self.switch={}
		--全局的通知在这里注册
	end
	return self.notificationList
end

------------------------------收发协议------------------------------

--请求偷红包
function this.sendReqStealOnlineRedPacket(player_id)
    local req = {}
	req.player_id = player_id
	this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqStealOnlineRedPacket", req)
end
--请求偷红包返回
function this.onRspStealOnlineRedPacket(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspStealOnlineRedPacket", protoBytes)
    rsp.isFriend = true
    PlatformRedBagProxy:GetInstance():setRedBagEndData(rsp)
    PlatformRedBagOpenView:openSuccessHandler()
    if rsp.result == ProtoEnumPlatform.StealOnlineRPResult.StealOnlineRPResult_Success then
       -- Alert.showAlertMsg(nil,"恭喜抢到了红包￥"..(rsp.money/100).."元","确定")
        NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Friend_Online_RedPacket_Info)
    elseif rsp.result == ProtoEnumPlatform.StealOnlineRPResult.StealOnlineRPResult_Stole then
        Alert.showAlertMsg(nil, "已经抢过这个红包了", "确定")
		ViewManager.close(UIViewEnum.Platform_RedBag_Open_View)
    elseif rsp.result == ProtoEnumPlatform.StealOnlineRPResult.StealOnlineRPResult_OverNum then
        Alert.showAlertMsg(nil, "今日抢红包次数已用完", "确定")
		ViewManager.close(UIViewEnum.Platform_RedBag_Open_View)
    elseif rsp.result == ProtoEnumPlatform.StealOnlineRPResult.StealOnlineRPResult_Wait then
		Alert.showAlertMsg(nil, "这个红包现在还不能抢", "确定")
		ViewManager.close(UIViewEnum.Platform_RedBag_Open_View)
    elseif rsp.result == ProtoEnumPlatform.StealOnlineRPResult.StealOnlineRPResult_ParamError then
		--参数错误
        Alert.showAlertMsg(nil, "抢红包失败", "确定")
		ViewManager.close(UIViewEnum.Platform_RedBag_Open_View)
	elseif rsp.result == ProtoEnumPlatform.StealOnlineRPResult.StealOnlineRPResult_Fail then
        --Alert.showAlertMsg(nil, "很遗憾，什么都没有抢到！", "确定")
		--ViewManager.close(UIViewEnum.Platform_RedBag_Open_View)
    end
	
	NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Friend_Online_RedPacket_Info)
end

-- 请求在线红包分享奖励
function this.sendReqOnlineRedPacketShareRewards(type)
    local req = {}
    req.type = type
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqOnlineRedPacketShareRewards", req)
end
--请求在线红包分享奖励返回
function this.onRspOnlineRedPacketShareRewards(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspOnlineRedPacketShareRewards", protoBytes)
	
    if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        printDebug("现实红包分享奖励成功")
        PlatformRedBagProxy:GetInstance():setRedBagEndData(rsp)
        ViewManager.open(UIViewEnum.Platform_Share_End_View, 2)
        NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Friend_Online_RedPacket_Info) 
        NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Req_Get_Myself_Online_RedPacket_Info) 
    elseif rsp.result == ProtoEnumCommon.ReqResult.ReqResultFail then
        Alert.showAlertMsg(nil, "失败", "确定")
        printDebug("失败")
    elseif rsp.result == ProtoEnumCommon.ReqResult.ReqResultFailNoFound then
        Alert.showAlertMsg(nil, "没有找到", "确定")
        printDebug("没有找到")
    elseif rsp.result == ProtoEnumCommon.ReqResult.ReqResultFailNoStock then
        Alert.showAlertMsg(nil, "库存不足", "确定")
        printDebug("库存不足")
    elseif rsp.result == ProtoEnumCommon.ReqResult.ReqResultFailDuplicate then
        Alert.showAlertMsg(nil, "重复", "确定")
        printDebug("重复")
    end
end