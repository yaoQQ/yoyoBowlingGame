require "base:module/Shop/data/PlatformShopChatProxy"
require "base:module/Shop/data/PlatformBusinessProxy"

--商店模块
PlatformShopModule = BaseModule:new()
local this = PlatformShopModule
this.moduleName = "PlatformShop"

------------------------------注册由服务器发来的协议------------------------------
function this.initRegisterNet()
    this.netFuncList = {}
    --广播聊天消息
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxNotifySendChat, this.acceptSendChatMsg)
    --商家优惠券信息返回
    this:AddNetLister(ProtoEnumCoupon.MsgIdx.MsgIdxRspShopCouponInfo, this.receiveShopCouponMsg)
    --领取或使用商家优惠券
    --this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspUserAddCoupon,this.receiveGetOrUseShopCouponMsg)
    --请求领取活动状态返回
    this:AddNetLister(ProtoEnumActive.MsgIdx.MsgIdxRspGetActiveGameState, this.onRspGetActiveGameState)
    --是否选择开始提醒返回
    this:AddNetLister(ProtoEnumActive.MsgIdx.MsgIdxRspIsActiveStartNotify, this.onRspIsActiveStartNotify)
    --选择开始提醒返回
    this:AddNetLister(ProtoEnumActive.MsgIdx.MsgIdxRspActiveStartNotify, this.onRspActiveStartNotify)
    --请求关注赛事列表返回
    this:AddNetLister(ProtoEnumActive.MsgIdx.MsgIdxRspActiveStartNotifyList, this.onRspActiveStartListNotify)
    -- 请求查找最近参加活动返回
    this:AddNetLister(ProtoEnumActive.MsgIdx.MsgIdxRspFindRecentActive, this.onRspFindRecentActive)
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
        self.switch = {}
        self:AddNotifictionLister(PlatformGlobalNoticeType.Platform_Req_Chat_Channel_Op, this.onSendChatChannelOp)
        self:AddNotifictionLister(PlatformGlobalNoticeType.Platform_Req_Send_Chat, this.onSendBroadCastMsg)
    end
    return self.notificationList
end

---------------------------------------------收到消息(客户端或服务端发出)---------------------------
--显示用户聊天信息
function this:onShowBroadCastMsg(notice)
    local rep = notice:GetObj()
    printDebug("收到的聊天信息为：" .. table.tostring(rep))
    PlatformShopChatProxy:GetInstance():addShopChatMsgData(rep)
    if PlatformShopChatView ~= nil and PlatformShopChatView.isOpen then
        -- printDebug("收到服务器聊天协议1")
        PlatformShopChatView:updateFirstChatMsg()
    end
end

------------------------------发协议------------------------------

--给服务器发送聊天室操作
function this:onSendChatChannelOp(notice)
    local rep = notice:GetObj()
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqChatChannelOp", rep)
end

--给服务器发送聊天内容
function this:onSendBroadCastMsg(notice)
    local rep = notice:GetObj()
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqSendChat", rep)
end

--请求商家优惠券信息
function this:onSendReqShopCoupon(shop_id)
    local req = {}
    req.shop_id = shop_id
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "coupon", "ReqShopCouponInfo", req)
end

--请求获取活动游戏状态
function this.sendReqGetActiveGameState(active_id)
    local req = {}
    req.active_id = active_id
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "active", "ReqGetActiveGameState", req)
end

--请求获取活动是否选择开始提醒
function this.sendReqIsActiveStartNotify(active_id)
    local req = {}
    req.active_id = active_id
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "active", "ReqIsActiveStartNotify", req)
end
--请求设置活动开始提醒
function this.sendReqActiveStartNotify(active_id, type)
    local req = {}
    req.active_id = active_id
    req.type = type == true and 1 or 0
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "active", "ReqActiveStartNotify", req)
end
--请求获取关注活动列表
function this.sendReqActiveStartListNotify()
    local req = {}
    req.page_index = 0
    req.per_page_num = 50
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "active", "ReqActiveStartNotifyList", req)
end
--请求获取最近参赛列表
function this.sendReqFindRecentActive()
    local req = {}
    req.page_index = 0
    req.per_page_num = 50
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "active", "ReqFindRecentActive", req)
end

--------------------------------------------收到协议(服务端发出)-------------------------------

--广播聊天消息
function this.acceptSendChatMsg(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "NotifySendChat", protoBytes)
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Notify_Send_Chat, rsp)
end

--请求聊天频道操作返回
function this.acceptChatChannelOp(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspChatChannelOp", protoBytes)
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Rsp_Chat_Channel_Op, rsp)
end

--广播聊天室变化
function this.acceptChatChannelChange(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "NotifyChatChannelChange", protoBytes)
    NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Notify_Chat_Channel_Change, rsp)
end

--收到商家优惠券信息
function this.receiveShopCouponMsg(protoBytes)
    local rsp = this.decodeProtoBytes("coupon", "RspShopCouponInfo", protoBytes)

    PlatformBusinessProxy:GetInstance():setShopMarketData(rsp)

    if PlatformShopCouponView and PlatformShopCouponView.isOpen then
        PlatformShopCouponView:updateMarketData()
    else
        ViewManager.open(UIViewEnum.PlatForm_Shop_Coupon_View)
    end
end

--请求领取或使用券返回
function this.receiveGetOrUseShopCouponMsg(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspUserAddCoupon", protoBytes)

    if rsp.state == 0 then -- 成功
        showFloatTips("领取优惠券成功!")

        if PlatformShopCouponView and PlatformShopCouponView.isOpen then
            PlatformShopCouponView:updateShopMarket()
        end
    else -- 失败
        showFloatTips("领取优惠券失败!")
    end
end

--请求获取活动游戏状态返回
function this.onRspGetActiveGameState(protoBytes)
    local rsp = this.decodeProtoBytes("active", "RspGetActiveGameState", protoBytes)
    if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        NoticeManager.Instance:Dispatch(NoticeType.Activity_Update_ActiveGameState, rsp.state)
    else
        NoticeManager.Instance:Dispatch(
            NoticeType.Activity_Update_ActiveGameState,
            ProtoEnumCommon.AactiveGameState_UnCanJion
        )
    end
end
--是否选择活动开始提醒返回
function this.onRspIsActiveStartNotify(protoBytes)
    local rsp = this.decodeProtoBytes("active", "RspIsActiveStartNotify", protoBytes)
    if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        NoticeManager.Instance:Dispatch(NoticeType.Activity_Update_ActiveIsStart, rsp.select)
    else
        showFloatTips("活动错误")
    end
end
--请求关注赛事回复
function this.onRspActiveStartNotify(protoBytes)
    local rsp = this.decodeProtoBytes("active", "RspActiveStartNotify", protoBytes)
    if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        NoticeManager.Instance:Dispatch(NoticeType.Activity_ActiveStart)
    else
        showFloatTips("活动错误")
    end
end
--请求关注赛事列表回复
function this.onRspActiveStartListNotify(protoBytes)
    local rsp = this.decodeProtoBytes("active", "RspActiveStartNotifyList", protoBytes)
    if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        PlatformLBSDataProxy.setActiveStartList(rsp)
        NoticeManager.Instance:Dispatch(NoticeType.Activity_ActiveStart_List)
    else
        showFloatTips("活动错误")
    end
end
--请求最近参赛赛事列表回复
function this.onRspFindRecentActive(protoBytes)
    local rsp = this.decodeProtoBytes("active", "RspFindRecentActive", protoBytes)
    if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        PlatformLBSDataProxy.setRecentGameList(rsp)
        NoticeManager.Instance:Dispatch(NoticeType.Activity_RecentGame_List)
    else
        showFloatTips("最近参赛赛事列表错误")
    end
end
