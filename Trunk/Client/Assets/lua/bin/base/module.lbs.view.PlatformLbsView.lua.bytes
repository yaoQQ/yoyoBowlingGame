require "base:mid/Mid_platform_icon_panel"
require "base:mid/global/Mid_platform_my_pos_platform"
require "base:mid/global/Mid_platform_global_shop_float_panel"
require "base:mid/global/Mid_platform_global_has_receiveredpacket_panel"
require "base:module/shop/data/ActivityItem"

--LBS界面
PlatformLbsView = BaseView:new()
local this = PlatformLbsView
this.viewName = "PlatformLbsView"

--设置面板特性（界面层级、界面枚举、是否参与界面堆栈）
this:setViewAttribute(UIViewType.MapFloat_View, UIViewEnum.PlatForm_Float_View, false)

--设置加载列表
this.loadOrders = {
    "base:platform_icon_panel", --浮标框架
    "base:global/platform_my_pos_platform", --用户自己的浮标
    "base:global/platform_global_float_panel", --红包或者活动浮标
}

this.myPosGo = nil --我的位置浮标
this.redBagGoList = {} --红包位置浮标
this.isReceiveRedBagGoList = {} --已领取红包数据
this.redBagCouponGoList = {} --卡券红包位置浮标
this.activityGoList = {} --活动位置浮标
this.officialGoList = {} --官方赛浮标

this.myPosData = nil --我的位置数据table

local DEFAULT_ALPHA = 0.5
local ICON_INTERVAL = 260 * 1.2
local MAX_NEAR_COUNT = 6
local MAX_COUNT = 10

--现金红包列表(未领)
local m_redPackList = {}
--现金红包列表(已领)
local m_redPackGetList = {}
--现金红包列表(被领光)
local m_noStockRedPackList = {}
--现金红包列表(全部)
local m_allRedPackList = {}
--优惠券列表
local m_couponList = {}
--赛事列表
local m_actList = {}
--官方赛事列表
local m_officialList = {}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    local switch = {
        [self.loadOrders[1]] = function()
            self.main_mid = {}
            self:BindMonoTable(gameObject, self.main_mid)
            printDebug(self.container.name)
            UITools.SetParentAndAlign(gameObject, self.container)
        end,
		[self.loadOrders[2]] = function()
            self.myPosObj = gameObject
            UITools.SetParentAndAlign(gameObject, self.container)
            self.myPosObj.transform.localPosition = Vector3(10000, 10000, 0)
        end,
        [self.loadOrders[3]] = function()
            self.redBagObj = gameObject
            UITools.SetParentAndAlign(gameObject, self.container)
            self.redBagObj.transform.localPosition = Vector3(10000, 10000, 0)
        end,
    }

    local fSwitch = switch[uiName]

    if fSwitch then
        fSwitch()
    else --key not found
        printDebug(uiName .. " not found !")
    end
end

--override 打开UI回调
function this:onShowHandler(msg)
    --打开界面时添加UI通知监听
    self:addNotice()

    --打开界面时初始化，一般用于处理没有数据时的默认的界面显示
    self:initView()
end

--override 关闭UI回调
function this:onClose()
    --关闭界面时移除UI通知监听
    self:removeNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Search_Notice, this.onSearchLbsUpdate)
    NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Update_MyPos, this.onUpdateMyPos)
	
	--NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Update_SearchActivityListData, this.onUpdateAllListData)
	--NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Update_ActivityListData, this.onUpdateAllListData)
	NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Update_OfficialListData, this.onUpdateAllListData)
    NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Update_RedPacketListData, this.onUpdateAllListData)
    NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Update_CouponListData, this.onUpdateAllListData)
	
    GlobalTimeManager.Instance.timerController:AddTimer(
        "PlatformLbsView",
        -1,
        -1,
        function()
            this:updateIconPos(false)
        end
    )
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Search_Notice, this.onSearchLbsUpdate)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Update_MyPos, this.onUpdateMyPos)
	
	--NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Update_SearchActivityListData, this.onUpdateAllListData)
	--NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Update_ActivityListData, this.onUpdateAllListData)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Update_OfficialListData, this.onUpdateAllListData)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Update_RedPacketListData, this.onUpdateAllListData)
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Update_CouponListData, this.onUpdateAllListData)

    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("PlatformLbsView")
end

--打开界面时初始化
function this:initView()
    self:onLoadMyPos()
	this.onUpdateAllListData()
end

this.stratActivityList = {}
--更新我的位置
function this.onUpdateMyPos()
    this:onLoadMyPos()
end

local lerpCro = nil
--插值变化
local function startLerpAlpha(self, initValue, addValue, time, callback)
    if lerpCro then
        coroutine.stop(lerpCro)
        lerpCro = nil
    end
    self.main_mid.icon_Panel:SetPanelCanvasGroupAlpha(initValue)
    lerpCro =
        coroutine.start(
        function()
            for t = 0, time, Time.deltaTime do
                local alpha = initValue + t / time * addValue
                self.main_mid.icon_Panel:SetPanelCanvasGroupAlpha(alpha)
                coroutine.step(lerpCro)
            end
            self.main_mid.icon_Panel:SetPanelCanvasGroupAlpha(initValue + addValue)
            if callback then
                callback()
            end
        end
    )
end

function this.IconPanelSetAplha(alpha)
    if this.main_mid then
        this.main_mid.icon_Panel.gameObject:SetActive(alpha == 1)
        this.main_mid.icon_Panel:SetPanelCanvasGroupAlpha(alpha)
    end
end

this.alphaPlayFinish = true
--搜索更新
function this.onSearchLbsUpdate(key, rsp)
    local data = rsp:GetObj()
    if data.reason == "activityClassify" and this.main_mid.icon_Panel then
        if data.isShow then
            --startLerpAlpha(this, 1, -1, 0.6)
            this.alphaPlayFinish = true
            this.main_mid.icon_Panel.gameObject:SetActive(false)
        else
            this.main_mid.icon_Panel.gameObject:SetActive(true)
            if this.alphaPlayFinish then
                this.alphaPlayFinish = false
                startLerpAlpha(
                    this,
                    0,
                    1,
                    0.6,
                    function()
                        this.alphaPlayFinish = true
                    end
                )
            end
        end
    end
end

--附近所有活动更新
function this.onUpdateAllListData()
	m_officialList = {}
	m_redPackList = {}
	m_redPackGetList = {}
	m_noStockRedPackList = {}
	m_allRedPackList = {}
	m_couponList = {}
	m_actList = {}
	
	local officialList = table.clone(PlatformLBSDataProxy.getOfficialList())
	local redPackList = table.clone(PlatformLBSDataProxy.getAllRedBagData())
	local noStockRedPackList = table.clone(PlatformLBSDataProxy.getAllSplendRedBagData())
	local couponList = table.clone(PlatformLBSDataProxy.getAllRedBagCouponData())
	local actList = table.clone(PlatformLBSDataProxy.getAllActivityData())
	local recommendActList = table.clone(PlatformLBSDataProxy.getAllRecommendActivityData())
	local redPackGetList = {}
	
	--全部列表
	local allList = {}
	--无定位列表
	local allNoPosList = {}
	
	--分离出已领红包
	local index = 1
	while index <= #redPackList do
		if redPackList[index].has_received == 1 then
			table.insert(redPackGetList, redPackList[index])
			table.remove(redPackList, index)
		else
			index = index + 1
		end
	end
	
	--过滤推荐赛事
	index = 1
	while index <= #recommendActList do
		for i = 1, #actList do
			if recommendActList[index].active_info.active_id == actList[i].active_info.active_id then
				table.remove(recommendActList, index)
				index = index - 1
				break
			end
		end
		index = index + 1
	end
	
	local count = 0
	--商圈内最多MAX_NEAR_COUNT个
	local nearDistance = TableBaseParameter.data[22].parameter
	--优先显示满2个优惠券，2个比赛，2个红包
	for i = 1, 2 do
		if #couponList > 0 then
			if couponList[1].distance > nearDistance then
				break
			end
			table.insert(m_couponList, couponList[1])
			table.insert(allList, couponList[1])
			table.remove(couponList, 1)
			count = count + 1
		else
			break
		end
	end
	for i = 1, 2 do
		if #recommendActList > 0 then
			table.insert(m_actList, recommendActList[1])
			table.insert(allNoPosList, recommendActList[1])
			table.remove(recommendActList, 1)
			count = count + 1
		else
			if #actList > 0 then
				if actList[1].distance > nearDistance then
					break
				end
				table.insert(m_actList, actList[1])
				table.insert(allList, actList[1])
				table.remove(actList, 1)
				count = count + 1
			else
				break
			end
		end
	end
	for i = 1, 2 do
		if #redPackList > 0 then
			if redPackList[1].distance > nearDistance then
				break
			end
			table.insert(m_redPackList, redPackList[1])
			table.insert(allList, redPackList[1])
			table.remove(redPackList, 1)
			count = count + 1
		else
			break
		end
	end
	--不够MAX_NEAR_COUNT个补推荐赛事
	while count < MAX_NEAR_COUNT and #recommendActList > 0 do
		table.insert(m_actList, recommendActList[1])
		table.insert(allNoPosList, recommendActList[1])
		table.remove(recommendActList, 1)
		count = count + 1
	end
	--不够MAX_NEAR_COUNT个补优惠券
	while count < MAX_NEAR_COUNT and #couponList > 0 do
		if couponList[1].distance > nearDistance then
			break
		end
		table.insert(m_couponList, couponList[1])
		table.insert(allList, couponList[1])
		table.remove(couponList, 1)
		count = count + 1
	end
	--不够MAX_NEAR_COUNT个补赛事
	while count < MAX_NEAR_COUNT and #actList > 0 do
		if actList[1].distance > nearDistance then
			break
		end
		table.insert(m_actList, actList[1])
		table.insert(allList, actList[1])
		table.remove(actList, 1)
		count = count + 1
	end
	--不够MAX_NEAR_COUNT个补红包
	while count < MAX_NEAR_COUNT and #redPackList > 0 do
		if redPackList[1].distance > nearDistance then
			break
		end
		table.insert(m_redPackList, redPackList[1])
		table.insert(allList, redPackList[1])
		table.remove(redPackList, 1)
		count = count + 1
	end
	--不够MAX_NEAR_COUNT个补官方赛
	while count < MAX_NEAR_COUNT and #officialList > 0 do
		table.insert(m_officialList, officialList[1])
		table.insert(allNoPosList, officialList[1])
		table.remove(officialList, 1)
		count = count + 1
	end
	--不够MAX_NEAR_COUNT个补领过现金红包
	while count < MAX_NEAR_COUNT and #redPackGetList > 0 do
		if redPackGetList[1].distance > nearDistance then
			break
		end
		table.insert(m_redPackGetList, redPackGetList[1])
		table.insert(allList, redPackGetList[1])
		table.remove(redPackGetList, 1)
		count = count + 1
	end
	--不够MAX_NEAR_COUNT个补领光现金红包
	while count < MAX_NEAR_COUNT and #noStockRedPackList > 0 do
		if noStockRedPackList[1].distance > nearDistance then
			break
		end
		table.insert(m_noStockRedPackList, noStockRedPackList[1])
		table.insert(allList, noStockRedPackList[1])
		table.remove(noStockRedPackList, 1)
		count = count + 1
	end
	
	--删除其他商圈内的
	while #redPackList > 0 do
		if redPackList[1].distance > nearDistance then
			break
		end
		table.remove(redPackList, 1)
	end
	while #redPackGetList > 0 do
		if redPackGetList[1].distance > nearDistance then
			break
		end
		table.remove(redPackGetList, 1)
	end
	while #noStockRedPackList > 0 do
		if noStockRedPackList[1].distance > nearDistance then
			break
		end
		table.remove(noStockRedPackList, 1)
	end
	while #couponList > 0 do
		if couponList[1].distance > nearDistance then
			break
		end
		table.remove(couponList, 1)
	end
	while #actList > 0 do
		if actList[1].distance > nearDistance then
			break
		end
		table.remove(actList, 1)
	end
	
	--商圈外最多MAX_COUNT个
	--优先显示1个比赛，1个红包，1个优惠券
	for i = 1, 1 do
		if #actList > 0 then
			table.insert(m_actList, actList[1])
			table.insert(allList, actList[1])
			table.remove(actList, 1)
			count = count + 1
		end
	end
	for i = 1, 1 do
		if #redPackList > 0 then
			table.insert(m_redPackList, redPackList[1])
			table.insert(allList, redPackList[1])
			table.remove(redPackList, 1)
			count = count + 1
		end
	end
	for i = 1, 1 do
		if #couponList > 0 then
			table.insert(m_couponList, couponList[1])
			table.insert(allList, couponList[1])
			table.remove(couponList, 1)
			count = count + 1
		end
	end
	--不够MAX_COUNT个补优惠券
	while count < MAX_COUNT and #couponList > 0 do
		table.insert(m_couponList, couponList[1])
		table.insert(allList, couponList[1])
		table.remove(couponList, 1)
		count = count + 1
	end
	--不够MAX_COUNT个补赛事
	while count < MAX_COUNT and #actList > 0 do
		table.insert(m_actList, actList[1])
		table.insert(allList, actList[1])
		table.remove(actList, 1)
		count = count + 1
	end
	--不够MAX_COUNT个补红包
	while count < MAX_COUNT and #redPackList > 0 do
		table.insert(m_redPackList, redPackList[1])
		table.insert(allList, redPackList[1])
		table.remove(redPackList, 1)
		count = count + 1
	end
	--不够MAX_COUNT个补领光现金红包
	while count < MAX_COUNT and #redPackGetList > 0 do
		table.insert(m_redPackGetList, redPackGetList[1])
		table.insert(allList, redPackGetList[1])
		table.remove(redPackGetList, 1)
		count = count + 1
	end
	--不够MAX_COUNT个补领光现金红包
	while count < MAX_COUNT and #noStockRedPackList > 0 do
		table.insert(m_noStockRedPackList, noStockRedPackList[1])
		table.insert(allList, noStockRedPackList[1])
		table.remove(noStockRedPackList, 1)
		count = count + 1
	end
	
	--把无定位列表并入全部列表
	for i = 1, #allNoPosList do
		table.insert(allList, allNoPosList[i])
	end
	
	this:updateMapPos(allList)
	this.onUpdateOfficial()
    this.onUpdateCouponListData()
	this.onUpdateRedPacketListData()
	this.onUpdateActivityListData()
    -- this.SortAllIcon()
end

--更新地图位置（防重叠）
function this:updateMapPos(allList)
	for i = 1, #allList do
		if allList[i].pos == nil then
			allList[i].pos = MapPos:new(allList[i].lng, allList[i].lat)
		end
	end
	
	if #allList <= 1 then
		return
	end
	
	local x1, y1, x2, y2, dx, dy, absX, absY, absOffsetX, absOffsetY
	local offsetX = 0
	local offsetY = 0
		
    for i = 2, #allList do
		offsetX = 0
		offsetY = 0
		x1, y1 = allList[i].pos:getScreenPosFromCenter()
		
		local j = 1
		while j < i do
			x2, y2 = allList[j].pos:getScreenPosFromCenter()
		
			dx = x1 + offsetX - x2
			dy = y1 + offsetY - y2
			
			--位置相同随机偏移
			if dx == 0 and dy == 0 then
				offsetX = offsetX + math.random(1, ICON_INTERVAL) - ICON_INTERVAL / 2
				offsetY = offsetY + math.random(1, ICON_INTERVAL) - ICON_INTERVAL / 2
				dx = x1 + offsetX - x2
				dy = y1 + offsetY - y2
			end
			
			absX = math.abs(dx)
			absY = math.abs(dy)
			absOffsetX = math.abs(offsetX)
			absOffsetY = math.abs(offsetY)
			if absX < ICON_INTERVAL and absY < ICON_INTERVAL then
				if absX == absY then
					if absOffsetX == absOffsetY then
						if absOffsetX == 0 then
							local rand = math.random(1, 4)
							if rand <= 1 then
								offsetX = x2 - x1 + ICON_INTERVAL
							elseif rand == 2 then
								offsetX = x2 - x1 - ICON_INTERVAL
							elseif rand == 3 then
								offsetY = y2 - y1 + ICON_INTERVAL
							else
								offsetY = y2 - y1 - ICON_INTERVAL
							end
						else
							local rand = math.random(1, 2)
							if rand <= 1 then
								offsetX = x2 - x1 + offsetX / absOffsetX * ICON_INTERVAL
							else
								offsetY = y2 - y1 + offsetY / absOffsetY * ICON_INTERVAL
							end
						end
					elseif absOffsetX > absOffsetY then
						if absOffsetY == 0 then
							local rand = math.random(1, 2)
							if rand <= 1 then
								offsetY = y2 - y1 + ICON_INTERVAL
							else
								offsetY = y2 - y1 - ICON_INTERVAL
							end
						else
							offsetY = y2 - y1 + offsetY / absOffsetY * ICON_INTERVAL
						end
					else
						if absOffsetX == 0 then
							local rand = math.random(1, 2)
							if rand <= 1 then
								offsetX = x2 - x1 + ICON_INTERVAL
							else
								offsetX = x2 - x1 - ICON_INTERVAL
							end
						else
							offsetX = x2 - x1 + offsetX / absOffsetX * ICON_INTERVAL
						end
					end
				elseif absX > absY then
					if absOffsetX == 0 then
						offsetX = x2 - x1 + dx / absX * ICON_INTERVAL
					else
						offsetX = x2 - x1 + offsetX / absOffsetX * ICON_INTERVAL
					end
				else
					if absOffsetY == 0 then
						offsetY = y2 - y1 + dy / absY * ICON_INTERVAL
					else
						offsetY = y2 - y1 + offsetY / absOffsetY * ICON_INTERVAL
					end
				end
				j = 0
			end
			j = j + 1
		end
		allList[i].pos:setOffset(offsetX, offsetY)
	end
end

--更新官方赛事
function this.onUpdateOfficial()
    this:onLoadOfficialActivity()
end

--更新附近活动列表
function this.onUpdateActivityListData()
    this:onLoadActivity()
end

--更新附近红包列表
function this.onUpdateRedPacketListData()
    this:onLoadRedBag()
end
--更新附近卷包列表
function this.onUpdateCouponListData()
    this:onLoadReBagCoupon()
end

function this:updateIconPos(needSetAtd)
    --红包位置偏移
    for i = 1, #m_allRedPackList do
        if m_allRedPackList[i] ~= nil then
            local loc1, loc2 = m_allRedPackList[i].pos:getScreenPosFromCenter()
            self.redBagGoList[i].go.transform.localPosition = Vector3(loc1, loc2, 0)
            --self.redBagGoList[i].loc1 = loc1
            --self.redBagGoList[i].loc2 = loc2

            --特效随机播放 //todo待优化
            if m_allRedPackList[i].red_packer_style == 4 and self.redBagGoList[i].effectrandomseed and self.redBagGoList[i].randombase then
                if self.redBagGoList[i].effectrandomseed - self.redBagGoList[i].randombase <= 0 then
                    self.redBagGoList[i].randombase = 0
                    self.redBagGoList[i].animation_panel:Play()
                else
                    self.redBagGoList[i].randombase = self.redBagGoList[i].randombase + 0.02
                end
            end
        end
    end

    --活动位置偏移
    for i = 1, #m_actList do
        if m_actList[i] ~= nil then
			local activityGo = self.activityGoList[i]
			local activity_info = m_actList[i]
			
            local loc1, loc2 = activity_info.pos:getScreenPosFromCenter()
            activityGo.go.transform.localPosition = Vector3(loc1, loc2, 0)
			
			--结束倒计时
			local curTime = TimeManager.getServerUnixTime()
			if curTime < activity_info.active_info.apply.start_time then
				activityGo.activity_time_text.text = "预热中..."
			elseif curTime > activity_info.active_info.apply.extra_time then
				activityGo.activity_time_text.text = "已结束"
			else
				activityGo.activity_time_text.text = TimeManager.formatHourMinSecFromSec(activity_info.active_info.apply.extra_time - curTime)
			end
        end
    end
    local scale = (MapManager.scale / MapManager.defaultScale) / (2 ^ (MapManager.maxZoom - MapManager.zoom))
    --官方赛事位置偏移
    for i = 1, #m_officialList do
        if m_officialList[i] ~= nil then
			local officialGo = self.officialGoList[i]
			local official_info = m_officialList[i]
			
			local loc1, loc2 = official_info.pos:getScreenPosFromCenter()
			
            officialGo.go.transform.localPosition = Vector3(loc1, loc2, 0)
			--officialGo.loc1 = loc1
            --officialGo.loc2 = loc2
			
			--结束倒计时
			local curTime = TimeManager.getServerUnixTime()
			if curTime < official_info.start_time then
				officialGo.activity_time_text.text = "预热中..."
			elseif curTime > official_info.end_time then
				officialGo.activity_time_text.text = "已结束"
			else
				officialGo.activity_time_text.text = TimeManager.formatHourMinSecFromSec(official_info.end_time - curTime)
			end
        end
    end
    --卡券红包位置偏移
    for i = 1, #m_couponList do
        if m_couponList[i] ~= nil then
            local loc1, loc2 = m_couponList[i].pos:getScreenPosFromCenter()
            self.redBagCouponGoList[i].go.transform.localPosition = Vector3(loc1, loc2, 0)
            if needSetAtd then
                self.redBagCouponGoList[i].go.name = tostring(loc2)
            end
            --self.redBagCouponGoList[i].loc1 = loc1
            --self.redBagCouponGoList[i].loc2 = loc2
        end
    end

    --我的位置浮标位置偏移
    if self.myPosData ~= nil and self.myPosGo ~= nil then
        self.myPosData.pos:updateLnglat(MapManager.userLng, MapManager.userLat)
        local loc1, loc2 = self.myPosData.pos:getScreenPosFromCenter()
        self.myPosGo.go.transform.localPosition = Vector3(loc1, loc2, 0)
        self.myPosGo.go.transform.localScale = self.myPosData.pos:getScale()
		
		--我的范围
		if MapManager.zoom >= 17 then
			self.myPosGo.my_range.gameObject:SetActive(false)
		else
			self.myPosGo.my_range.gameObject:SetActive(true)
			local rangeSize = TableBaseParameter.data[22].parameter * 3.65 * MapManager.scale / (2 ^ (17 - MapManager.zoom + 1))
			self.myPosGo.my_range.transform.localScale = Vector3(rangeSize, rangeSize, 0)
		end
		
        --我的位置椭圆
        if needSetAtd then
            self.myPosGo.go.name = tostring(loc2)
        end
    end
end

--------------------------------------解析数据并实例化预制体----------------------------------

----------合并浮标相关----------------

--[[function this.CombineIcon()
    this:onLoadActivity()
    this:onLoadRedBag()
    this:onLoadReBagCoupon()
    this:onLoadOfficialActivity()
end--]]

local distanceCombine = 200 ^ 2 + 200 ^ 2 --250像素的距离

function this:UpdateCombineIcon(curIcon, lastIcon)
    if not lastIcon then
        return false
    end
    if not curIcon then
        return false
    end
    local dis = distanseNotSqrt(curIcon.loc1, curIcon.loc2, lastIcon.loc1, lastIcon.loc2)

    return dis < distanceCombine
end

--本地官方赛事
function this:onLoadOfficialActivity()
    if self.redBagObj == nil then
        return
    end
    for i = 1, #self.officialGoList do
        if self.officialGoList[i] ~= nil then
            self.officialGoList[i].panelParent:SetFarAway(true)
        end
    end
    self.officialData = {}
	
    for i = 1, #m_officialList do
		local official_info = m_officialList[i]
        local official_config = TableBaseOfficalMatch.data[official_info.match_type]
        local officialGo = nil
        local reload = false
        --如果列表里面有该物体，则循环使用，无则new一个
        if self.officialGoList[i] ~= nil then
            officialGo = self.officialGoList[i]
        else
            reload = true
            local go = GameObject.Instantiate(self.redBagObj)
            officialGo = {}
            self:BindMonoTable(go, officialGo)
			local sortingOrderOffset = 20 + i
            if officialGo.panelParent.sortingOrderOffset ~= sortingOrderOffset then
                officialGo.panelParent.sortingOrderOffset = sortingOrderOffset
            end
            --浮标位置
			officialGo.go.transform:SetParent(self.main_mid.icon_Panel.transform)
			officialGo.go.transform.localScale = Vector3(1.2, 1.2, 1)
        end

        officialGo.panelParent:SetFarAway(false)
		
		officialGo.timesbg.gameObject:SetActive(false)
		officialGo.redbag.gameObject:SetActive(false)
		officialGo.activity.gameObject:SetActive(true)
		officialGo.activity_arrow_image.gameObject:SetActive(false)
		officialGo.activity_official_title_image.gameObject:SetActive(true)
		--游戏图标
		downloadGameIcon(official_config.gameid, officialGo.activity_game_image)
        officialGo.activity_game_image.name = official_info.match_type
		--赛事名
        officialGo.activity_name_text.text = official_config.title
		--官方赛的商家头像特殊处理
		downloadMerchantHead("MerchantHead/official", officialGo.activity_shop_image)

        --按钮注册
        officialGo.pressbg:AddEventListener(
            UIEvent.PointerClick,
            function(eventData)
                if eventData == nil then
                    return
                end
                if not table.empty(official_info.subset) then
                    NoticeManager.Instance:Dispatch(
                        NoticeType.LBS_Change_ViewState,
                        PlatformGlobalShopView.ShowPanelState.StateEnum.BottomPanelShortOpen
                    )
                    local activityInfo = {}
                    for j = 1, #official_info.subset do
                        local activeItem = ActivityItem.createByCompetition(official_info.subset[j])
                        table.insert(activityInfo, activeItem)
                    end
                    activityInfo.activityType = ActivityItem.ActivityType.Competition
                    return NoticeManager.Instance:Dispatch(NoticeType.LBS_ShopView_ActivityList, activityInfo)
                end
                ViewManager.open(
                    UIViewEnum.Platform_LOCAL_OFFICIAL_RULE,
                    {
                        match_type = official_info.match_type,
                        id = official_info.active_id,
                        endTime = official_info.end_time,
                        game_state = official_info.game_state
                    }
                )
            end
        )
        if official_info.subset and #official_info.subset > 0 then
            officialGo.timesbg.gameObject:SetActive(true)
            officialGo.times.text = #official_info.subset
        else
            officialGo.timesbg.gameObject:SetActive(false)
        end
        if reload then
            table.insert(self.officialGoList, officialGo)
        end
    end
end
--解析我的位置数据
function this:onLoadMyPos()
    if self.myPosObj == nil then
        return
    end

    local data = PlatformUserProxy:GetInstance():getUserInfo()

    if data == nil then
        return
    end
    --如果有该物体，则循环使用，无则new一个
    if self.myPosGo == nil then
        local go = GameObject.Instantiate(self.myPosObj)
        self.myPosGo = Mid_platform_my_pos_platform
        self:BindMonoTable(go, self.myPosGo)
    end
    
    --暂时处理
    data.pos = MapPos:new(MapManager.userLng, MapManager.userLat)

    --浮标位置
    self.myPosGo.go.transform:SetParent(self.main_mid.icon_Panel.transform)
    local loc1, loc2 = data.pos:getScreenPosFromCenter()
    -- printDebug("我的浮标位置loc1="..loc1..",我的浮标位置loc2="..loc2)
    self.myPosGo.go.transform.localPosition = Vector3(loc1, loc2, 0)
    self.myPosGo.go.transform.localScale = data.pos:getScale()
    self.myPosData = data
end

--解析红包数据
function this:onLoadRedBag()
    if self.redBagObj == nil then
        printDebug("redBagObj  = nil ！")
        return
    end

    for i = 1, #self.redBagGoList do
        if self.redBagGoList[i] ~= nil then
            self.redBagGoList[i].panelParent:SetFarAway(true)
        end
    end
	
    --data处理合并收拢点数据

    for i = 1, #m_redPackList do
        local tempData = table.clone(m_redPackList[i])
        --tempData.loc1, tempData.loc2 = tempData.pos:getScreenPosFromCenter()
        tempData.subset = {}
        tempData.shopdata = PlatformLBSDataProxy.getRedBagShopDataByShopId(m_redPackList[i].shop_id)
		tempData.isNoStock = false
        table.insert(m_allRedPackList, tempData)
    end
	for i = 1, #m_redPackGetList do
        local tempData = table.clone(m_redPackGetList[i])
        --tempData.loc1, tempData.loc2 = tempData.pos:getScreenPosFromCenter()
        tempData.subset = {}
        tempData.shopdata = PlatformLBSDataProxy.getRedBagShopDataByShopId(m_redPackGetList[i].shop_id)
		tempData.isNoStock = false
        table.insert(m_allRedPackList, tempData)
    end
	for i = 1, #m_noStockRedPackList do
        local tempData = table.clone(m_noStockRedPackList[i])
        --tempData.loc1, tempData.loc2 = tempData.pos:getScreenPosFromCenter()
        tempData.subset = {}
        tempData.shopdata = PlatformLBSDataProxy.getRedBagShopDataByShopId(m_noStockRedPackList[i].shop_id)
		tempData.isNoStock = true
        table.insert(m_allRedPackList, tempData)
    end
	
	local newdata = m_allRedPackList
    for i = 1, #newdata do
        local redBagGo = nil
        local reload = false
        --如果列表里面有该物体，则循环使用，无则new一个
        if self.redBagGoList[i] ~= nil then
            redBagGo = self.redBagGoList[i]
        else
            reload = true
            local go = GameObject.Instantiate(self.redBagObj)
            --redBagGo = Mid_platform_global_float_panel:new(go)
            --UIMono方式绑定UI对象
            redBagGo = {}
            self:BindMonoTable(go, redBagGo)
            --local sortOrder = (#self.redBagGoList * 4) + 4
            --sortOrder = sortOrder >= 2000 and 1999 or sortOrder
            --if redBagGo.panelParent.sortingOrderOffset ~= sortOrder then
            --    redBagGo.panelParent.sortingOrderOffset = sortOrder
            --end
        end
        redBagGo.panelParent:SetFarAway(false)
        --浮标位置
        redBagGo.go.transform:SetParent(self.main_mid.icon_Panel.transform)
        --newdata[i].loc1, newdata[i].loc2 = newdata[i].pos:getScreenPosFromCenter()
        --redBagGo.go.transform.localPosition = Vector3(newdata[i].loc1, newdata[i].loc2, 0)
		
		local redbag_info = newdata[i]
		
		redBagGo.timesbg.gameObject:SetActive(false)
		redBagGo.activity.gameObject:SetActive(false)
		redBagGo.redbag.gameObject:SetActive(true)
		redBagGo.coupon_bg_image.gameObject:SetActive(false)
		redBagGo.redbag_bg_image.gameObject:SetActive(true)
        --商家头像
		downloadMerchantHead(redbag_info.shopdata.headurl, redBagGo.redbag_shop_image)
        --红包金额
        redBagGo.redbag_text.text = tostring(math.floor(redbag_info.money / 100))
		
		--派发范围决定透明度
		local isTrans = false
		if redbag_info.isNoStock or redbag_info.has_received == 1 then
			isTrans = true
		else
			if redbag_info.position_limit_type == ProtoEnumCommon.LBSPositionLimitType.LBSPositionLimitType_Near then
				--判断商圈范围
				local dis = MapManager.getDistance(MapManager.userLng, MapManager.userLat, redbag_info.lng, redbag_info.lat)
				if dis > TableBaseParameter.data[22].parameter then
					isTrans = true
				end
			elseif redbag_info.position_limit_type == ProtoEnumCommon.LBSPositionLimitType.LBSPositionLimitType_City then
				--判断城市
				if MapManager.userCityCode ~= redbag_info.city_code then
					--判断商圈范围
					local dis = MapManager.getDistance(MapManager.userLng, MapManager.userLat, redbag_info.lng, redbag_info.lat)
					if dis > TableBaseParameter.data[22].parameter then
						isTrans = true
					end
				end
			end
		end
		if isTrans then
			redBagGo.go.transform.localScale = Vector3(0.8, 0.8, 1)
			redBagGo.panelParent:SetPanelCanvasGroupAlpha(DEFAULT_ALPHA)
			local sortingOrderOffset = 10 + i
            if redBagGo.panelParent.sortingOrderOffset ~= sortingOrderOffset then
                redBagGo.panelParent.sortingOrderOffset = sortingOrderOffset
            end
		else
			redBagGo.go.transform.localScale = Vector3(1.2, 1.2, 1)
			redBagGo.panelParent:SetPanelCanvasGroupAlpha(1)
			local sortingOrderOffset = 30 + i
            if redBagGo.panelParent.sortingOrderOffset ~= sortingOrderOffset then
                redBagGo.panelParent.sortingOrderOffset = sortingOrderOffset
            end
		end
		
        --按钮注册
        redBagGo.pressbg:AddEventListener(
            UIEvent.PointerClick,
            function(eventData)
                --printDebug("+++++++++++++++++点击了红包1！")
                if eventData == nil then
                    return
                end
                if not table.empty(newdata[i].subset) then
                    NoticeManager.Instance:Dispatch(
                        NoticeType.LBS_Change_ViewState,
                        PlatformGlobalShopView.ShowPanelState.StateEnum.BottomPanelShortOpen
                    )
                    local activityInfo = {}
                    for j = 1, #newdata[i].subset do
                        local activeItem =
                            ActivityItem.createByRedPacket(newdata[i].subset[j], newdata[i].subset[j].shopdata)
                        table.insert(activityInfo, activeItem)
                    end
                    activityInfo.activityType = ActivityItem.ActivityType.RedPacket
                    return NoticeManager.Instance:Dispatch(NoticeType.LBS_ShopView_ActivityList, activityInfo)
                end
                local msg = {}
                msg.redpacketId = newdata[i].red_packet_id
                msg.redpacketType = ProtoEnumCommon.RedPacketType.RedPacketType_Active
                msg.isFromChat = false
                msg.headUrl = newdata[i].shopdata.headurl
                msg.name = newdata[i].shopdata.name
                msg.title = newdata[i].title
                msg.describeImageList = newdata[i].imgs
                msg.describe = newdata[i].discribe
                msg.packetStyle = newdata[i].red_packer_style
                msg.shop_id = newdata[i].shopdata.shop_id
				msg.money = newdata[i].has_received_money
                msg.is_official = newdata[i].player_id < ShopGlobalConfig.OfficialShopId
				
				msg.positionLimitType = newdata[i].position_limit_type
				msg.cityCode = newdata[i].city_code
				msg.lng = newdata[i].lng
				msg.lat = newdata[i].lat
				
                PlatformRedPacketProxy.SetOpenLBSPacketData("RedPacket_Open_Data", msg)
                if newdata[i].has_received == 1 then
                    --不请求直接打开界面
					PlatformRedPacketProxy.UpdatePacketData(
						"RedPacket_Open_Data",
						msg,
						PacketGetState.HasGot,
						false
					)
					ViewManager.open(UIViewEnum.Platform_LBS_RedPacket_Detail_View)
                else
                    PlatformLBSRedPacketOpenView.openLBSRedPacketOpenView()
                end
            end
        )
        local msg = {}
        if newdata[i].subset and #newdata[i].subset > 0 then
            redBagGo.timesbg.gameObject:SetActive(true)
            redBagGo.times.text = #newdata[i].subset
        else
            redBagGo.timesbg.gameObject:SetActive(false)
        end
        if reload then
            table.insert(self.redBagGoList, redBagGo)
        end
    end
    --this:manhattanDistance(self.redBagGoList)
end

--解析红包卡券数据
function this:onLoadReBagCoupon()
    if self.redBagObj == nil then
        return
    end
    local data = m_couponList
    if data == nil then
        return
    end

    for i = 1, #self.redBagCouponGoList do
        if self.redBagCouponGoList[i] ~= nil then
            self.redBagCouponGoList[i].panelParent:SetFarAway(true)
        end
    end

    local newdata = {}
    for i = 1, #data do
        local tempData = table.clone(data[i])
        --tempData.loc1, tempData.loc2 = tempData.pos:getScreenPosFromCenter()
        tempData.subset = {}
        tempData.shopdata = PlatformLBSDataProxy.getRedBagShopDataByShopId(data[i].shop_id)
        table.insert(newdata, tempData)
    end

    for i = 1, #newdata do
        local redBagCpGo = nil
        local reload = false
        --如果列表里面有该物体，则循环使用，无则new一个
        if self.redBagCouponGoList[i] ~= nil then
            redBagCpGo = self.redBagCouponGoList[i]
        else
            reload = true
            local go = GameObject.Instantiate(self.redBagObj)
            --redBagCpGo = Mid_platform_global_float_panel:new(go)
            redBagCpGo = {}
            go:GetComponent(typeof(CS.UIBaseMono)):BindMonoTable(redBagCpGo)
			local sortingOrderOffset = 50 + i
            if redBagCpGo.panelParent.sortingOrderOffset ~= sortingOrderOffset then
                redBagCpGo.panelParent.sortingOrderOffset = sortingOrderOffset
            end
        end

        redBagCpGo.panelParent:SetFarAway(false)
        --浮标位置
        redBagCpGo.go.transform:SetParent(self.main_mid.icon_Panel.transform)
        --newdata[i].loc1, newdata[i].loc2 = newdata[i].pos:getScreenPosFromCenter()
        --redBagCpGo.go.transform.localPosition = Vector3(newdata[i].loc1, newdata[i].loc2, 0)
        redBagCpGo.go.transform.localScale = Vector3(1.2, 1.2, 1)
		
		local redbag_info = newdata[i]
		
		redBagCpGo.timesbg.gameObject:SetActive(false)
		redBagCpGo.activity.gameObject:SetActive(false)
		redBagCpGo.redbag.gameObject:SetActive(true)
		redBagCpGo.redbag_bg_image.gameObject:SetActive(false)
		redBagCpGo.coupon_bg_image.gameObject:SetActive(true)
        --商家头像
		downloadMerchantHead(redbag_info.shopdata.headurl, redBagCpGo.redbag_shop_image)
		
        --按钮注册
        redBagCpGo.pressbg:AddEventListener(
            UIEvent.PointerClick,
            function(eventData)
                if eventData == nil then
                    return
                end
                if not table.empty(newdata[i].subset) then
                    NoticeManager.Instance:Dispatch(
                        NoticeType.LBS_Change_ViewState,
                        PlatformGlobalShopView.ShowPanelState.StateEnum.BottomPanelShortOpen
                    )
                    local activityInfo = {}
                    for j = 1, #newdata[i].subset do
                        local activeItem =
                            ActivityItem.createByCoupon(newdata[i].subset[j], newdata[i].subset[j].shopdata)
                        table.insert(activityInfo, activeItem)
                    end
                    activityInfo.activityType = ActivityItem.ActivityType.Coupon
                    return NoticeManager.Instance:Dispatch(NoticeType.LBS_ShopView_ActivityList, activityInfo)
                end
                --print("newdata[i] = "..table.tostring(newdata[i]))
                local msg = {}
                msg.redpacketId = newdata[i].red_packet_id
                msg.redpacketType = ProtoEnumCommon.RedPacketType.RedPacketType_Active
                msg.isFromChat = false
                msg.coupon_id = newdata[i].coupon_id
                msg.headUrl = newdata[i].shopdata.headurl
                msg.name = newdata[i].shopdata.name
                msg.title = newdata[i].title
                msg.coupon_redpacket_data = newdata[i]
                msg.describe = newdata[i].text
                msg.couponName = newdata[i].coupon_name
                msg.packetStyle = newdata[i].style
                msg.iconUrl = newdata[i].icon
                msg.describeImageList = newdata[i].imgs
				
				msg.lng = newdata[i].longitude
				msg.lat = newdata[i].latitude

                PlatformRedPacketProxy.SetOpenLBSPacketData("Coupon_Open_Data", msg)
                PlatformLBSCouponOpenView.openPlatformLBSCouponOpenView(true)
            end
        )
        if newdata[i].subset and #newdata[i].subset > 0 then
            redBagCpGo.timesbg.gameObject:SetActive(true)
            redBagCpGo.times.text = #newdata[i].subset
        else
            redBagCpGo.timesbg.gameObject:SetActive(false)
        end
        if reload then
            table.insert(self.redBagCouponGoList, redBagCpGo)
        end
        -- end
    end
end

this.ActivityGameRewardType = {}
this.ActivityGameRewardType[ProtoEnumCommon.LootType.LootType_Diamond] = 3
this.ActivityGameRewardType[ProtoEnumCommon.LootType.LootType_Money] = 1
this.ActivityGameRewardType[ProtoEnumCommon.LootType.LootType_Cash] = 2
this.ActivityGameRewardType[ProtoEnumCommon.LootType.LootType_Coupon] = 4
--解析者活动数据
function this:onLoadActivity()
    if self.redBagObj == nil then
        return
    end
    
    this.stratActivityList = PlatformLBSDataProxy.getActiveStartList()
    for i = 1, #self.activityGoList do
        if self.activityGoList[i] ~= nil then
            self.activityGoList[i].panelParent:SetFarAway(true)
        end
    end
    
    for i = 1, #m_actList do
        m_actList[i].subset = {}
        m_actList[i].isActiveArrow = this.stratActivityList[m_actList[i].active_info.active_id] and true or false
    end
	
	local newdata = m_actList
    for i = 1, #newdata do
        local activityGo = nil
        local reload = false
        --如果列表里面有该物体，则循环使用，无则new一个
        if self.activityGoList[i] ~= nil then
            activityGo = self.activityGoList[i]
        else
            reload = true
            local go = GameObject.Instantiate(self.redBagObj)
            --activityGo = Mid_platform_global_float_panel:new(go)
            activityGo = {}
            self:BindMonoTable(go, activityGo)
			local sortingOrderOffset = 40 + i
            if activityGo.panelParent.sortingOrderOffset ~= sortingOrderOffset then
                activityGo.panelParent.sortingOrderOffset = sortingOrderOffset
            end
        end

        activityGo.panelParent:SetFarAway(false)
		activityGo.go.transform:SetAsLastSibling()

        local active_info = newdata[i].active_info
        local shop_info = PlatformLBSDataProxy.getRedBagShopDataByShopId(active_info.shop_id)

        --浮标位置
        activityGo.go.transform:SetParent(self.main_mid.icon_Panel.transform)
        --active_info.loc1, active_info.loc2 = newdata[i].pos:getScreenPosFromCenter()
        --activityGo.go.transform.localPosition = Vector3(active_info.loc1, active_info.loc2, 0)
        activityGo.go.transform.localScale = Vector3(1.2, 1.2, 1)
		if IS_UNITY_EDITOR then
			activityGo.go.name = "act_"..active_info.apply.subject
		end
		
		activityGo.timesbg.gameObject:SetActive(false)
		activityGo.redbag.gameObject:SetActive(false)
		activityGo.activity.gameObject:SetActive(true)
		activityGo.activity_arrow_image.gameObject:SetActive(newdata[i].isActiveArrow)
		activityGo.activity_official_title_image.gameObject:SetActive(false)
		--游戏图标
		downloadGameIcon(active_info.apply.game_type, activityGo.activity_game_image)
        activityGo.activity_game_image.name = active_info.active_id
		--赛事名
        activityGo.activity_name_text.text = active_info.apply.subject
		--商家头像
		downloadMerchantHead(active_info.shop_head_url, activityGo.activity_shop_image)
		
		--派发范围决定透明度
		local isTrans = false
		if newdata[i].game_state == ProtoEnumCommon.AactiveGameState.AactiveGameState_UnCanJion then
			isTrans = true
		else
			if active_info.apply.reward_list.reward_type == ProtoEnumCommon.LootType.LootType_Cash then
				if active_info.position_limit_type == ProtoEnumCommon.LBSPositionLimitType.LBSPositionLimitType_Near then
					--判断商圈范围
					local dis = MapManager.getDistance(MapManager.userLng, MapManager.userLat, active_info.lng, active_info.lat)
					if dis > TableBaseParameter.data[22].parameter then
						isTrans = true
					end
				elseif active_info.position_limit_type == ProtoEnumCommon.LBSPositionLimitType.LBSPositionLimitType_City then
					--判断城市
					if MapManager.userCityCode ~= active_info.city_code then
						--判断商圈范围
						local dis = MapManager.getDistance(MapManager.userLng, MapManager.userLat, active_info.lng, active_info.lat)
						if dis > TableBaseParameter.data[22].parameter then
							isTrans = true
						end
					end
				end
			end
		end
		if isTrans then
			activityGo.go.transform.localScale = Vector3(0.8, 0.8, 1)
			activityGo.panelParent:SetPanelCanvasGroupAlpha(DEFAULT_ALPHA)
		else
			activityGo.go.transform.localScale = Vector3(1.2, 1.2, 1)
			activityGo.panelParent:SetPanelCanvasGroupAlpha(1)
		end
		
        --按钮注册
        activityGo.pressbg:AddEventListener(
            UIEvent.PointerClick,
            function(eventData)
                if eventData == nil then
                    return
                end
                if not table.empty(newdata[i].subset) then
                    NoticeManager.Instance:Dispatch(
                        NoticeType.LBS_Change_ViewState,
                        PlatformGlobalShopView.ShowPanelState.StateEnum.BottomPanelShortOpen
                    )
                    local activityInfo = {}
                    for j = 1, #newdata[i].subset do
                        local activeItem = ActivityItem.createByCompetition(newdata[i].subset[j])
                        table.insert(activityInfo, activeItem)
                    end
                    activityInfo.activityType = ActivityItem.ActivityType.Competition
                    return NoticeManager.Instance:Dispatch(NoticeType.LBS_ShopView_ActivityList, activityInfo)
                end
				PlatformGlobalShopChatView.showPlatformGlobalShopChatView(active_info.active_id)
            end
        )
        if newdata[i].subset and #newdata[i].subset > 0 then
            activityGo.timesbg.gameObject:SetActive(true)
            activityGo.times.text = #newdata[i].subset
        else
            activityGo.timesbg.gameObject:SetActive(false)
        end
        if reload then
            table.insert(self.activityGoList, activityGo)
        end
    end
    --this:manhattanDistance(self.activityGoList)
end