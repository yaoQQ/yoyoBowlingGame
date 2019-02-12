require "base:module/redpacket/data/PlatformRedPacketProxy"

PlatformRedpacketModule = BaseModule:new()
local this = PlatformRedpacketModule
this.moduleName = "PlatformRedpacket"

------------------------------注册由服务器发来的协议------------------------------
function this.initRegisterNet()
    this.netFuncList = {}
    this:AddNetLister(
        ProtoEnumActive.MsgIdx.MsgIdxRspRcvActiveCashRedPacket,
         this.onRspRcvActiveCashRedPacket
        )
 
    this:AddNetLister(
        ProtoEnumActive.MsgIdx.MsgIdxRspActiveCashRedPacketState,
         this.onRspActiveCashRedPacketState
        )

    this:AddNetLister(
        ProtoEnumActive.MsgIdx.MsgIdxRspGetActiveCashRedPacketRcvHistory,
        this.onRspGetActiveCashRedPacketRcvHistory
    )
    --收到地图拆红包通知
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspRcvMapRedPacket, this.onReceiveMapRedPacket)
    this:AddNetLister(
        ProtoEnumActive.MsgIdx.MsgIdxRspReceiveActiveCouponRedpacket,
        this.onRspReceiveActiveCouponRedpacket
    )
    this:AddNetLister(ProtoEnumCoupon.MsgIdx.MsgIdxRspRcvCoupon, this.rspPlayerGetNewCoupon)
    --收到地图拆红包历史记录通知
    --function 为nil注释  this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspMapRedPacketRcvHistory,this.onRspMapRedPacketRcvHistory)
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
    end
    return self.notificationList
end

------------------------------通知响应------------------------------
function this:onNotificationLister(noticeType, notice)
    local switch = {}
    local fSwitch = switch[noticeType] --switch func
    if fSwitch then --key exists
        fSwitch() --do func
    else --key not found
        --用于报错提醒
        self:withoutRegistNotice(noticeType)
    end
end

------------------------------发协议------------------------------

--请求领取红包
function this.sendReqRcvActiveCashRedPacket(type, active_id, redpacket_id)
    local req = {}
    req.type = type
    req.active_id = active_id
    req.redpacket_id = redpacket_id
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "active", "ReqRcvActiveCashRedPacket", req)
end

--请求活动现金红包状态
function this.sendReqActiveCashRedPacketState(type, active_id, redpacket_id)
    local req = {}
    req.type = type
    req.active_id = active_id
    req.redpacket_id = redpacket_id
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "active", "ReqActiveCashRedPacketState", req)
end


--请求领取地图红包
function this.sendReqRcvMapRedPacket(type, red_packet_id)
    local req = {}
    req.type = type
    req.red_packet_id = red_packet_id
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqRcvMapRedPacket", req)
end

--请求获取红包领取历史记录
function this.sendReqGetActiveCashRedPacketRcvHistory(type, active_id, redpacket_id)
    local req = {}
    req.type = type
    req.active_id = active_id
    req.redpacket_id = redpacket_id
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "active", "ReqGetActiveCashRedPacketRcvHistory", req)
end
--请求领取优惠券
function this.sendReqReceiveActiveCouponRedpacket(type, active_id, red_packet_id, coupon_id)
    local req = {}
    req.type = type
    req.active_id = active_id
    req.red_packet_id = red_packet_id
    req.coupon_id = coupon_id
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "active", "ReqReceiveActiveCouponRedpacket", req)
end

--请求领取地图优惠券
function this.sendReqRcvCoupon(type, red_packet_id, coupon_id)
    local req = {}
    req.type = type
    req.red_packet_id = red_packet_id
    req.coupon_id = coupon_id
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "coupon", "ReqRcvCoupon", req)
end

------------------------------收协议------------------------------

--请求领取非LBS红包返回
function this.onRspRcvActiveCashRedPacket(protoBytes)
    local rsp = this.decodeProtoBytes("active", "RspRcvActiveCashRedPacket", protoBytes)

    if rsp.result == ProtoEnumCommon.ReceiveRedPacket_Result.ReceiveRedPacket_Success then
        PlatformRedPacketProxy.UpdatePacketData("ChatRoom_RedPacket_Open_Data", rsp, PacketGetState.FirstGot, false)
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
        ViewManager.open(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
        NoticeManager.Instance:Dispatch(NoticeType.User_Update_Cash)
    elseif rsp.result == ProtoEnumCommon.ReceiveRedPacket_Result.ReceiveRedPacket_HasReceived then
        PlatformRedPacketProxy.UpdatePacketData("ChatRoom_RedPacket_Open_Data", rsp, PacketGetState.HasGot, false)
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
        ViewManager.open(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
    elseif rsp.result == ProtoEnumCommon.ReceiveRedPacket_Result.ReceiveRedPacket_Empty then
        PlatformRedPacketProxy.UpdatePacketData("ChatRoom_RedPacket_Open_Data", rsp, PacketGetState.Empty, false)
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
        ViewManager.open(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
    else
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
    end
end

--请求活动现金红包状态返回
function this.onRspActiveCashRedPacketState(protoBytes)
    local rsp = this.decodeProtoBytes("active", "RspActiveCashRedPacketState", protoBytes)
    printDebug("++++++++++++++++++++++++活动现金红包状态返回rsp ="..table.tostring(rsp))
    if rsp.result == ProtoEnumCommon.ChatRedPacketStatus.ChatRedPacketStatus_CanRcv then
        PlatformRedPacketProxy.UpdatePacketData("ChatRoom_RedPacket_Open_Data", rsp, PacketGetState.FirstGot, false)
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
        ViewManager.open(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
    elseif rsp.result == ProtoEnumCommon.ChatRedPacketStatus.ChatRedPacketStatus_Rcved then
        PlatformRedPacketProxy.UpdatePacketData("ChatRoom_RedPacket_Open_Data", rsp, PacketGetState.HasGot, false)
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
        ViewManager.open(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
    elseif rsp.result == ProtoEnumCommon.ChatRedPacketStatus.ChatRedPacketStatus_Empty then
        PlatformRedPacketProxy.UpdatePacketData("ChatRoom_RedPacket_Open_Data", rsp, PacketGetState.Empty, false)
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
        ViewManager.open(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
    else
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
    end
   
end


--请求获取红包领取历史记录返回
function this.onRspGetActiveCashRedPacketRcvHistory(protoBytes)
    local rsp = this.decodeProtoBytes("active", "RspGetActiveCashRedPacketRcvHistory", protoBytes)
    if rsp.rcv_user_list == nil then
        rsp.rcv_user_list = {}
    end
    NoticeManager.Instance:Dispatch(PlatformNoticeType.Receive_RedBag_List, rsp)
end

--请求领取非LBS优惠券红包返回
function this.onRspReceiveActiveCouponRedpacket(protoBytes)
    local rsp = this.decodeProtoBytes("active", "RspReceiveActiveCouponRedpacket", protoBytes)
    if rsp.couponrcv_list == nil then
        rsp.couponrcv_list = {}
    end
    if rsp.result == ProtoEnumCommon.ReceiveRedPacket_Result.ReceiveRedPacket_Success then
        PlatformRedPacketProxy.UpdatePacketData("ChatRoom_RedPacket_Open_Data", rsp, PacketGetState.FirstGot, false)
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
        ViewManager.open(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
    elseif rsp.result == ProtoEnumCommon.ReceiveRedPacket_Result.ReceiveRedPacket_HasReceived then
        PlatformRedPacketProxy.UpdatePacketData("ChatRoom_RedPacket_Open_Data", rsp, PacketGetState.HasGot, false)
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
        ViewManager.open(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
    elseif rsp.result == ProtoEnumCommon.ReceiveRedPacket_Result.ReceiveRedPacket_Empty then
        PlatformRedPacketProxy.UpdatePacketData("ChatRoom_RedPacket_Open_Data", rsp, PacketGetState.Empty, false)
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
        ViewManager.open(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
    else
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Detail_View)
        ViewManager.close(UIViewEnum.Platform_Chat_Room_RedPacket_Open_View)
    end
end

--请求LBS领取优惠券返回
function this.rspPlayerGetNewCoupon(protoBytes)
    local rsp = this.decodeProtoBytes("coupon", "RspRcvCoupon", protoBytes)

    if rsp.result == ProtoEnumCommon.ReceiveRedPacket_Result.ReceiveRedPacket_Success then
        PlatformRedPacketProxy.UpdatePacketData("Coupon_Open_Data", rsp.coupon_red_packet, false)
        PlatformLBSCouponOpenView:openSuccessHandler()
        PlatformLBSDataProxy.setCouponBagDataById(rsp.red_packet_id)
    else
        print("领取不成功")
        PlatformLBSCouponOpenView.close()
    end
end

--收到LBS领红包通知返回
function this.onReceiveMapRedPacket(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspRcvMapRedPacket", protoBytes)
    if rsp.result == ProtoEnumCommon.ReceiveRedPacket_Result.ReceiveRedPacket_Success then
        PlatformRedPacketProxy.UpdatePacketData("RedPacket_Open_Data", rsp, PacketGetState.FirstGot, false)
        PlatformLBSRedPacketOpenView:openSuccessHandler()
        PlatformLBSDataProxy.setRedBagDataById(rsp.red_packet_id)
    elseif rsp.result == ProtoEnumCommon.ReceiveRedPacket_Result.ReceiveRedPacket_HasReceived then
        PlatformRedPacketProxy.UpdatePacketData("RedPacket_Open_Data", rsp, PacketGetState.HasGot, false)
        PlatformLBSRedPacketOpenView.close(true)
        ViewManager.open(UIViewEnum.Platform_LBS_RedPacket_Detail_View)
    elseif rsp.result == ProtoEnumCommon.ReceiveRedPacket_Result.ReceiveRedPacket_Empty then
        PlatformRedPacketProxy.UpdatePacketData("RedPacket_Open_Data", rsp, PacketGetState.Empty, false)
        PlatformLBSRedPacketOpenView.close(true)
        ViewManager.open(UIViewEnum.Platform_LBS_RedPacket_Detail_View)
    else
        PlatformLBSRedPacketOpenView.close(true)
        print("领取不成功")
    end
end
