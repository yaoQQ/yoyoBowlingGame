require "base:manager/MapManager"

PlatformLBSModule = BaseModule:new()
local this = PlatformLBSModule
this.moduleName = "PlatformLBS"

------------------------------注册由服务器发来的协议------------------------------
function this.initRegisterNet()
    this.netFuncList = {}
    --获取附近活动
    this:AddNetLister(ProtoEnumActive.MsgIdx.MsgIdxRspFindNearActive, this.onRspFindNearActive)
    --获取附近红包
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspFindNearRedPacket, this.onRspFindNearRedPacket)
    --获取附近卡卷
    this:AddNetLister(ProtoEnumCoupon.MsgIdx.MsgIdxRspFindNearCoupon, this.onRspFindNearCoupon)
    --获取附近所有活动
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspFindNearAllActivity, this.onRspFindNearAllActivity)
    --获取官方赛事
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGetOfficalActivity, this.onRspGetOfficalActivity)
    --获取官方赛事排行
    this:AddNetLister(ProtoEnumPlatform.MsgIdx.MsgIdxRspGetOfficalActivityRank, this.onRspGetOfficalActivityRank)
	--请求推荐商家赛事返回
    this:AddNetLister(ProtoEnumActive.MsgIdx.MsgIdxRspRecommonedBusActive, this.onRspRecommonedBusActive)
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
    end
    return self.notificationList
end

------------------------------收发协议------------------------------

--请求获取附近活动信息
function this.sendReqNearActivity(lng, lat, distance)
    local req = {}
    req.lng = lng
    req.lat = lat
    req.distance = distance
    --MapManager.getCurScreenMapPos(500) --暂定1公里
    req.maxcount = 50 --暂定最多50个活動
    req.num = 50
    --printDebug("请求获取附近活动信息:"..table.tostring(req))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "active", "ReqFindNearActive", req)
end
--收到更新附近活动信息
function this.onRspFindNearActive(protoBytes)
    local rsp = this.decodeProtoBytes("active", "RspFindNearActive", protoBytes)
    if rsp.active_list == nil then
        rsp.active_list = {}
    end

    PlatformLBSDataProxy.onRspFindNearActive(rsp)
    ShowSearching(false, "activityClassify")
end

--请求获取所有活动信息
function this.sendReqNearAllActivity(lng, lat, distance, search)
    local req = {}
    req.lng = lng
    req.lat = lat
    req.search = search --搜索关键字
    req.distance = distance
    req.max_count = 50 --暂定最多50个活動
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqFindNearAllActivity", req)
end
--收到所有活动信息
function this.onRspFindNearAllActivity(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspFindNearAllActivity", protoBytes)

    if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
        PlatformLBSDataProxy.onRspFindNearAllActivity(rsp)
    else
        printError("所有活动信息错误 :" .. tostring(rsp.result))
    end
    ShowSearching(false, "activityClassify")
end

--请求获取附近紅包信息
function this.sendReqNearRedBag(lng, lat)
    local req = {}
    req.lng = lng
    req.lat = lat
    req.distance = MapManager.getCurScreenMapPos(500) --暂定1公里
    req.max_count = 50 --暂定最多1000个紅包
    --printDebug("请求获取附近红包信息:"..table.tostring(req))
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqFindNearRedPacket", req)
end
--收到更新附近红包信息
function this.onRspFindNearRedPacket(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspFindNearRedPacket", protoBytes)

    PlatformLBSDataProxy.onRspFindNearRedPacket(rsp)
    ShowSearching(false, "activityClassify")
end

--请求附近优惠卷
function this.sendReqNearCoupon(lng, lat)
    local req = {}
    req.lng = lng
    req.lat = lat
    req.distance = MapManager.getCurScreenMapPos(500) --暂定1公里
    req.max_count = 50 --暂定最多1000个紅包
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "coupon", "ReqFindNearCoupon", req)
end
--收到更新附近卡卷信息
function this.onRspFindNearCoupon(protoBytes)
    local rsp = this.decodeProtoBytes("coupon", "RspFindNearCoupon", protoBytes)

    PlatformLBSDataProxy.onRspFindNearCoupon(rsp)
    ShowSearching(false, "activityClassify")
end

--请求官方赛事
function this.sendReqGetOfficalActivity()
    local req = {}
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetOfficalActivity", req)
end
--收到官方赛事信息
function this.onRspGetOfficalActivity(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspGetOfficalActivity", protoBytes)
    PlatformLBSDataProxy.onRspGetOfficalActivity(rsp)
end

--请求官方赛事排行
function this.sendReqGetOfficalActivityRank(activity_id)
    local req = {}
    req.activity_id = activity_id
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "platform", "ReqGetOfficalActivityRank", req)
end
--收到官方赛事排行信息
function this.onRspGetOfficalActivityRank(protoBytes)
    local rsp = this.decodeProtoBytes("platform", "RspGetOfficalActivityRank", protoBytes)

    PlatformLBSDataProxy.setActiveRankData(rsp.offical_activity_rank)

    ViewManager.open(UIViewEnum.Platform_Active_Rank_View, {isofficial = true})
    -- ViewManager.open(UIViewEnum.Platform_LOCAL_OFFICIAL_RANK, rsp)
end

--请求推荐商家赛事返回
function this.sendReqRecommonedBusActive()
    local req = {}
    req.maxcount = 6
    this.sendNetMsg(GameConfig.ServerName.MainGateway, "active", "ReqRecommonedBusActive", req)
end
--请求推荐商家赛事返回
function this.onRspRecommonedBusActive(protoBytes)
    local rsp = this.decodeProtoBytes("active", "RspRecommonedBusActive", protoBytes)
    PlatformLBSDataProxy.onRspRecommonedBusActive(rsp)
end

------------------------------外部接口------------------------------

local m_lastSendOfficalTime = 0
--延迟10秒后刷新官方赛
function this.delaySendReqGetOfficalActivity()
	local curTime = TimeManager.getServerUnixTime()
	if curTime < m_lastSendOfficalTime + 10 then
		return
	end
	
	m_lastSendOfficalTime = TimeManager.getServerUnixTime()
	GlobalTimeManager.Instance.timerController:AddTimer(
		"DelaySendReqGetOfficalActivity",
		10000,
		1,
		function()
			this.sendReqGetOfficalActivity()
		end
	)
end