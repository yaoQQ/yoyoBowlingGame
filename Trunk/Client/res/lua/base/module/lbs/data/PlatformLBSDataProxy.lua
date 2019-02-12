PlatformLBSDataProxy = {}
local this = PlatformLBSDataProxy

--官方赛事列表
local m_officialList = {}
--商店字典（LBS活动相关的商店）
local m_shopDict = {}
--现金红包列表
local m_redPackList = {}
--被领光现金红包列表
local m_noStockRedPackList = {}
--优惠券列表
local m_couponList = {}
--赛事列表
local m_actList = {}
--推荐赛事列表
local m_recommendActList = {}

--清除缓存数据
function this.clearAllActivityData()
	--m_shopDict = {}
	m_redPackList = {}
	m_noStockRedPackList = {}
	m_couponList = {}
	m_officialList = {}
	m_actList = {}
	m_recommendActList = {}
	NoticeManager.Instance:Dispatch(NoticeType.LBS_Update_SearchActivityListData)
end

--获取官方赛事列表
function this.getOfficialList()
    return m_officialList
end

--获取商家数据
function this.getRedBagShopDataByShopId(shop_id)
	if m_shopDict[shop_id] == nil then
		m_shopDict[shop_id] = nil
	end
    return m_shopDict[shop_id]
end

--获取现金红包列表
function this.getAllRedBagData()
    return m_redPackList
end

--获取被领光现金红包列表
function this.getAllSplendRedBagData()
    return m_noStockRedPackList
end

--获取优惠券列表
function this.getAllRedBagCouponData()
    return m_couponList
end

--获取赛事列表
function this.getAllActivityData()
    return m_actList
end

--获取推荐赛事列表
function this.getAllRecommendActivityData()
    return m_recommendActList
end

--设置官方赛事
function this.onRspGetOfficalActivity(rsp)
    m_officialList = {}
    if not table.empty(rsp.activity_list) then
        for k, v in pairs(rsp.activity_list) do
            v.isLocalOfficial = true
			v.lng = MapManager.userLng + math.random(-80, 80) / 10000
			v.lat = MapManager.userLat + math.random(-80, 80) / 10000
            table.insert(m_officialList, v)
        end
    end
    NoticeManager.Instance:Dispatch(NoticeType.LBS_Update_OfficialListData)
end

--刷新官方赛事位置
function this.updateOfficalActivityLngLat()
	for k, v in ipairs(m_officialList) do
		v.lng = MapManager.userLng + math.random(-80, 80) / 10000
		v.lat = MapManager.userLat + math.random(-80, 80) / 10000
	end
    NoticeManager.Instance:Dispatch(NoticeType.LBS_Update_OfficialListData)
end

--官方活动和全部活动搜索返回
function this.onRspFindNearAllActivity(rsp)
    --商店数据处理
	this.setShopList(rsp.shop_list)
	
	-- flag: 0现金红包列表+商店列表，1被领光现金红包列表+商店列表，2优惠券红包列表+商店列表，3活动列表+商店列表
    if rsp.flag == 0 then
		this.setRedPackList(rsp.red_packet_list)
    elseif rsp.flag == 1 then
		this.setNoStockRedPackList(rsp.no_stock_red_packet_list)
    elseif rsp.flag == 2 then
		this.setCouponList(rsp.coupon_red_packet_list)
    elseif rsp.flag == 3 then
		this.setActList(rsp.active_list)
        NoticeManager.Instance:Dispatch(NoticeType.LBS_Update_SearchActivityListData)
    end
end

--请求推荐商家赛事返回
function this.onRspRecommonedBusActive(rsp)
	if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
		this.setShopList(rsp.shop_list)
		this.setRecommendActList(rsp.active_list)
	end
end

--获取附近红包返回
function this.onRspFindNearRedPacket(rsp)
	if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
		this.setShopList(rsp.shop_list)
		this.setRedPackList(rsp.red_packet_list)
		this.setNoStockRedPackList(rsp.no_stock_red_packet_list)
		NoticeManager.Instance:Dispatch(NoticeType.LBS_Update_RedPacketListData)
	end
end

--获取附近优惠券返回
function this.onRspFindNearCoupon(rsp)
	if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
		this.setShopList(rsp.shop_list)
		this.setCouponList(rsp.coupon_red_packet_list)
		NoticeManager.Instance:Dispatch(NoticeType.LBS_Update_CouponListData)
	end
end

--获取附近赛事返回
function this.onRspFindNearActive(rsp)
	if rsp.result == ProtoEnumCommon.ReqResult.ReqResultSuccess then
		this.setShopList(rsp.shop_list)
		this.setActList(rsp.active_list)
		NoticeManager.Instance:Dispatch(NoticeType.LBS_Update_ActivityListData)
	end
end

--设置LBS相关商店数据
function this.setShopList(shop_list)
	if not table.empty(shop_list) then
        for _, v in pairs(shop_list) do
            if not table.empty(v) then
                m_shopDict[v.shop_id] = v
            end
        end
    end
end
	
--设置LBS红包数据
function this.setRedPackList(red_packet_list)
	m_redPackList = {}
	if not table.empty(red_packet_list) then
		for k, v in pairs(red_packet_list) do
			if not table.empty(v) and this.getRedBagShopDataByShopId(v.shop_id) then
				table.insert(m_redPackList, v)
			end
		end
	end
end

--设置LBS被领光红包数据
function this.setNoStockRedPackList(no_stock_red_packet_list)
	m_noStockRedPackList = {}
	if not table.empty(no_stock_red_packet_list) then
		for k, v in pairs(no_stock_red_packet_list) do
			if not table.empty(v) and this.getRedBagShopDataByShopId(v.shop_id) then
				table.insert(m_noStockRedPackList, v)
			end
		end
	end
end

--设置LBS优惠券数据
function this.setCouponList(coupon_red_packet_list)
	m_couponList = {}
	if not table.empty(coupon_red_packet_list) then
		for k, v in pairs(coupon_red_packet_list) do
			if not table.empty(v) and this.getRedBagShopDataByShopId(v.shop_id) then
				v.lng = v.longitude
				v.lat = v.latitude
				table.insert(m_couponList, v)
			end
		end
	end
end

--设置LBS赛事数据
function this.setActList(active_list)
	m_actList = {}
	if not table.empty(active_list) then
		for k, v in pairs(active_list) do
			if not table.empty(v) then
				v.lng = v.active_info.apply.lng
				v.lat = v.active_info.apply.lat
				table.insert(m_actList, v)
			end
		end
	end
end

--设置LBS推荐赛事数据
function this.setRecommendActList(active_list)
	m_recommendActList = {}
	if not table.empty(active_list) then
		for k, v in pairs(active_list) do
			if not table.empty(v) then
				v.lng = MapManager.userLng + math.random(-80, 80) / 10000
				v.lat = MapManager.userLat + math.random(-80, 80) / 10000
				table.insert(m_recommendActList, v)
			end
		end
	end
end


--当前打开的赛事
this.currActData = nil

--通过活动id来获取活动数据
function this.setActivityDataById(activityId)
    if this.currActData and tostring(this.currActData.active_info.active_id) == tostring(activityId) then
        return
    end
    --关注赛事列表的赛事信息特殊处理
    local startActivityList = PlatformLBSDataProxy.getActiveStartList()
    if not table.empty(startActivityList) and startActivityList[tonumber(activityId)] then
        this.currActData = startActivityList[tonumber(activityId)]
        return
    end
    for i = 1, #m_actList do
        if tostring(m_actList[i].active_info.active_id) == tostring(activityId) then
            this.currActData = m_actList[i]
            return
        end
    end
	for i = 1, #m_recommendActList do
        if tostring(m_recommendActList[i].active_info.active_id) == tostring(activityId) then
            this.currActData = m_recommendActList[i]
            return
        end
    end
    for i = 1, #this.recentGameList do
        if this.recentGameList[i].active_info then
            if tostring(this.recentGameList[i].active_info.active_id) == tostring(activityId) then
                this.currActData = this.recentGameList[i]
                return
            end
        end
    end
end

--获取单个活动数据
function this.getActivitySingleData()
    return this.currActData.active_info
end

this.currActiveRankData = nil
--设置排行榜数据
function this.setActiveRankData(data)
    -- body

    this.currActiveRankData = data
end

--获取排行榜数据
function this.getActiveRankData()
    -- body
    return this.currActiveRankData
end

this.activeStartList = {}
--设置关注赛事数据
function this.setActiveStartList(data)
	this.setShopList(data.shop_list)
    this.activeStartList = {}
    if not table.empty(data.active_list) then
        for k, v in pairs(data.active_list) do
            this.activeStartList[v.active_info.active_id] = v
        end
    end
end
--关注赛事数据
function this.getActiveStartList(data)
    return this.activeStartList
end

this.recentGameList = {}
--设置最近参赛数据
function this.setRecentGameList(data)
	this.setShopList(data.shop_list)
    this.recentGameList = {}
    if not table.empty(data) then
        if not table.empty(data.offical_active_list) then
            for k, v in pairs(data.offical_active_list) do
                v.isLocalOfficial = true
                table.insert(this.recentGameList, v)
            end
        end
        if not table.empty(data.active_list) then
            for k, v in pairs(data.active_list) do
                table.insert(this.recentGameList, v)
            end
        end
        table.sort(
            this.recentGameList,
            function(a, b)
                if a.join_time ~= b.join_time then
                    return a.join_time > b.join_time
                end
                return false
            end
        )
    end
end
--最近参赛数据
function this.getRecentGameList(data)
    return this.recentGameList
end

--通过红包id改变领取成功的状态
function this.setRedBagDataById(redBagId)
    if table.empty(m_redPackList) or not redBagId then
        return
    end
    local count = #m_redPackList
    for i = 1, count do
        if tostring(m_redPackList[i].red_packet_id) == tostring(redBagId) then
            m_redPackList[i].has_received = 1
            break
        end
    end
    NoticeManager.Instance:Dispatch(NoticeType.LBS_Update_RedPacketListData)
end
--通过券包id改变领取成功的状态
function this.setCouponBagDataById(couponBagId)
    if table.empty(m_couponList) or not couponBagId then
        return
    end
    local count = #m_couponList
    for i = 1, count do
        if tostring(m_couponList[i].red_packet_id) == tostring(couponBagId) then
            table.remove(m_couponList, i)
            break
        end
    end
    NoticeManager.Instance:Dispatch(NoticeType.LBS_Update_CouponListData)
end