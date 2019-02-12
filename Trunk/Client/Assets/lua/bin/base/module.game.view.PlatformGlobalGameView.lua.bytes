

require "base:enum/UIViewEnum"
require "base:mid/game/Mid_platform_game_panel"
require "base:module/game/data/PlatformGameProxy"
require "base:table/TableBaseGameList"

local UIExEventTool = CS.UIExEventTool
--主界面：游戏
PlatformGlobalGameView = BaseView:new()
local this = PlatformGlobalGameView
this.viewName = "PlatformGlobalGameView"

--设置面板特性
this:setViewAttribute(UIViewType.Main_view, UIViewEnum.Platform_Global_Game_View, true)

--设置加载列表
this.loadOrders=
{
	"base:game/platform_game_panel",
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName,gameObject)
	self.main_mid = Mid_platform_game_panel
	self:BindMonoTable(gameObject, self.main_mid)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
	self:initOriginSprite()
	self.main_mid.hot_game_panel.gameObject:SetActive(false)
end

function this:addEvent()

end

--temp
this.originSpriteList = nil
local gameList = {}
function this:initOriginSprite()
	if table.empty(self.originSpriteList) then
		self.originSpriteList = {}
	end
	local banner = self.main_mid.head_banner
	for i = 0, banner.viewList.Length - 1 do
		local image = banner.viewList[i]:GetComponent(typeof(ImageWidget))
		self.originSpriteList[i] = image.Img.sprite
	end
end

--override 打开UI回调
function this:onShowHandler(msg)
	printDebug("=====================PlatformGlobalGameView调用完毕======================")
	local go = self:getViewGO()
	if go == nil then return end
	go.transform:SetAsLastSibling()
	self:addNotice()	
	self:reqFind()
	gameList = {}
	for _, v in pairs(TableBaseGameList.data) do
		if IS_TEST_SERVER then
			table.insert(gameList, v)
		else
			if v.isStableOpen == 1 then
				table.insert(gameList, v)
			end
		end
	end
	table.sort(gameList, function (a, b)
		local left = a.id
		local right = b.id
		if left == nil or right  == nil then
			return false
		end
		if left == right then
			return false
		end
		return left < right
	end)
	local banner = self.main_mid.head_banner
	banner.content = banner.viewList[1]
	banner:SetBannerData(gameList, 0, function (go, data, index)
		--print(string.format("index: %s, banner_name: %s", index, data.banner_name))
		local viewList = self.main_mid.head_banner.viewList
		self:showGameBanner(gameList[index], viewList[0])		-- left
		self:showGameBanner(gameList[index + 1], viewList[1])			-- cur
		self:showGameBanner(gameList[index + 2], viewList[2])		-- right
	end)
	self.main_mid.near_more_image.gameObject:SetActive(false)
	--self.main_mid.near_more_text:AddEventListener(UIEvent.PointerClick, function ()
	--	showFloatTips("功能还在开发中，敬请期待！")
	--end)
	self.main_mid.head_banner:AddExEventListener(UIEvent.PointerShortClick, this.onPointerShortClick)

	--打开子界面
	ViewManager.open(UIViewEnum.Platform_Top_Cost_View)
	ViewManager.open(UIViewEnum.Platform_Global_View, UIViewEnum.Platform_Global_Game_View)
end

function this:showGameBanner(data, view)
	if data == nil then
		return
	end
	local image = view:GetComponent(typeof(CS.ImageWidget))
	downloadGameIcon(data.banner_name, image)
	--print("尝试下载网络banner图: "..data.banner_name)
end

function this.onPointerShortClick(go)
	if not IS_TEST_SERVER then
		return
	end
	local index = this.main_mid.head_banner.dataIndex
	GameManager.enterGame(gameList[index + 1].id, EnumGameType.Hall, -1, 6500)
end

--override 关闭UI回调
function this:onClose()
	self:removeNotice()

	--关闭子界面
	ViewManager.close(UIViewEnum.Platform_Top_Cost_View)
	ViewManager.close(UIViewEnum.Platform_Global_View)
end

function this:reqFind()
	self.main_mid.hot_bg_image:ActiveLoadImage(true)
	self.main_mid.near_bg_image:ActiveLoadImage(true)
	self.main_mid.hot_group.gameObject:SetActive(false)
	self.main_mid.near_scroll_panel.gameObject:SetActive(false)
	self.main_mid.hot_no_text.gameObject:SetActive(false)
	self.main_mid.near_no_text.gameObject:SetActive(false)
	PlatformLBSModule.sendReqNearActivity(MapManager.userLng, MapManager.userLat,10000)
	MainModule.sendReqGetHotGame()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Rsp_Find_Hot_Game, this.onRspHotGame)
	NoticeManager.Instance:AddNoticeLister(NoticeType.LBS_Update_ActivityListData, this.onRspCompetition)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(PlatformGlobalNoticeType.Platform_Rsp_Find_Hot_Game, this.onRspHotGame)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.LBS_Update_ActivityListData, this.onRspCompetition)
end

function this:onHotGameInfoChangeHandler(dataList)
	self.main_mid.hot_bg_image:ActiveLoadImage(false)
	self.main_mid.hot_group.gameObject:SetActive(true)
	self.main_mid.hot_no_text.gameObject:SetActive(#dataList == 0)
	for i = 1, #self.main_mid.hotGameItemArr do
		local data = dataList[i]
		local item = self.main_mid.hotGameItemArr[i]
		if data == nil then
			item.go:SetActive(false)
		else
			item.go:SetActive(true)
			if i == 2 then
				item.bg_icon:ChangeIcon(1)
			end
			item.game_name_text.text = TableBaseGameList.data[data].name
			item.game_introduce_text.text = TableBaseGameList.data[data].GameSketch
			downloadGameIcon(data, item.game_image)
		end
		item.bg_icon:AddEventListener(UIEvent.PointerClick, function ()
			local msg = {}
			msg.gameId = data
			msg.gameType = EnumGameType.Hall
			msg.shopId = -1
			msg.roomId = -1
			ViewManager.open(UIViewEnum.Platform_Game_Rule_View, msg)
		end)
	end
end

this.officialTimeTextList = {}
this.sendHeartBeatTimes = 0
this.official_timer = 0
local function CountDowm(self)
    local curtimer = this.official_timer - TimeManager.getServerUnixTime()
    if not table.empty(this.officialTimeTextList) then
        for _, targettext in pairs(this.officialTimeTextList) do
            if targettext then
				if curtimer <= 0 then
					targettext.text = "活动已结束"
					PlatformLBSModule.delaySendReqGetOfficalActivity()
				else
					targettext.text =  string.concat("结束倒计时: ", GetTimeStr(curtimer))
				end
            end
        end
    end
end
this.initOfficialTimer = false
function this.CalTimerCount(self)
    if this.initOfficialTimer then
        return
    end
    this.initOfficialTimer = true
    CountDowm(self)
    GlobalTimeManager.Instance.timerController:AddTimer(
        "GlobalOfficialTimeCountDown",
        1000,
        -1,
        function()
            CountDowm(self)
        end
    )
end

function this:onCompetitionChangeHandler(dataList)
	self.main_mid.near_bg_image:ActiveLoadImage(false)
	--self.main_mid.near_scroll_panel.gameObject:SetActive(true)
	self.main_mid.near_competition_panel.gameObject:SetActive(true)
	self.main_mid.near_no_text.gameObject:SetActive(#dataList == 0)
	--print("dataList = "..table.tostring(dataList))
	local function showCanJoin(item)
		item.bg_icon:ChangeIcon(0)
		item.activity_state_icon:ChangeIcon(0)
		item.game_state_text.text = "游戏进行中"
		item.game_name_text.Txt.color = UIExEventTool.HexToColor("#FFFFFFFF")
		item.activity_name_text.Txt.color = UIExEventTool.HexToColor("#FFFFFFFF")
		item.sponsor_name_text.Txt.color = UIExEventTool.HexToColor("#FFF78AFF")
		item.game_state_text.Txt.color = UIExEventTool.HexToColor("#FFFFFFFF")
	end
	--ChangeCell
	self.main_mid.near_list_CellRecycleScrollPanel:SetCellData(dataList, self.onSetDetailInfo, true)
	
	--




--[[
	local now = TimeManager.getServerDateTime()
	for i = 1, #self.main_mid.nearItemArr do
		local data = dataList[i]
		local item = self.main_mid.nearItemArr[i]
		if data == nil then
			item.go:SetActive(false)
		else
			item.go:SetActive(true)
			local timerKey = string.format("platform_game_%s", data.id)
			GlobalTimeManager.Instance.timerController:RemoveTimerByKey(timerKey)
			downloadGameIcon(data.gameId, item.game_image)
			item.game_name_text.text = TableBaseGameList.data[data.gameId].name
			item.activity_name_text.text = data.title
			if data.isLocalOfficial then
				--官方赛的商家头像特殊处理
				downloadMerchantHead("MerchantHead/official", item.shop_head_image)
			else
				downloadMerchantHead(data.shopImageUrl, item.shop_head_image)
			end
			item.sponsor_name_text.text = string.format("赞助商: %s", data.shopName)
			if data.joinState == ActivityItem.JoinState.CanJoin then
				showCanJoin(item)
			else
				item.bg_icon:ChangeIcon(1)
				item.activity_state_icon:ChangeIcon(1)
				item.game_name_text.Txt.color = UIExEventTool.HexToColor("#424242FF")
				item.activity_name_text.Txt.color = UIExEventTool.HexToColor("#424242FF")
				item.sponsor_name_text.Txt.color = UIExEventTool.HexToColor("#9A9A9AFF")
				item.game_state_text.Txt.color = UIExEventTool.HexToColor("#0DACE2FF")
				local downTime = data.startTime - now
				local totalSeconds = downTime.TotalSeconds
				item.game_state_text.text = string.format("离开始时间: %02s:%02s:%02s", downTime.Hours, downTime.Minutes,downTime.Seconds)
				GlobalTimeManager.Instance.timerController:AddTimer(timerKey, 1000, -1, function ()
					if totalSeconds < 0 then
						return
					end
					totalSeconds = totalSeconds - 1
					if totalSeconds <= 0 then
						showCanJoin(item)
					else
						local downTimeNew = TimeSpan.FromSeconds(totalSeconds)
						item.game_state_text.text = string.format("离开始时间: %02s:%02s:%02s", downTimeNew.Hours, downTimeNew.Minutes,downTimeNew.Seconds)
					end
				end)
			end
			item.bg_icon:AddEventListener(UIEvent.PointerClick, function ()
				PlatformGlobalShopChatView.showPlatformGlobalShopChatView(data.id)
			end)
		end
	end
	self.main_mid.near_scroll_panel.contentRT.anchoredPosition = Vector2.zero
	local cellSize = Vector2(491, 339)
	local space = Vector2(20, 0)
	local columnCount = 2
	local rowCount = math.ceil(#dataList / columnCount)
	local groupW = cellSize.x * columnCount + space.x * (columnCount - 1)
	local groupH = cellSize.y * rowCount + space.y * (rowCount - 1)
	local a = self.main_mid.near_group.rectTransform
	local b = self.main_mid.bg_image.rectTransform
	a.sizeDelta = Vector2(groupW, groupH)
	self.main_mid.near_scroll_panel.contentRT.sizeDelta = Vector2(groupW, groupH + cellSize.y)
	self.main_mid.near_scroll_panel.scrollRect.vertical = UIExEventTool.IsDown(a, b)
	--]]
end



--设置活动详情信息
function this.onSetDetailInfo(go, data, index)
    local item = this.main_mid.nearlistcellArr[index + 1]
    this:setDetailInfo(item, data)
end

function this:setDetailInfo(item, data)
	local function onClickItemHandler(data)
        if data.type == ActivityItem.ActivityType.Competition then
            if data.isLocalOfficial then
                ViewManager.open(
                    UIViewEnum.Platform_LOCAL_OFFICIAL_RULE,
                    {match_type = data.match_type, id = data.id, endTime = data.endTime, game_state = data.game_state}
                )
            else
				PlatformGlobalShopChatView.showPlatformGlobalShopChatView(data.id)
            end
        else
            local msg = {}
            msg.redpacketId = data.id
            msg.redpacketType = ProtoEnumCommon.RedPacketType.RedPacketType_Active
            msg.isFromChat = false
            msg.headUrl = data.shopImageUrl
            msg.name = data.shopName
            msg.title = data.title
            msg.describe = data.describe
            msg.describeImageList = data.describeImageList
            msg.packetStyle = data.packetStyle
            msg.is_official = data.is_official
			
			msg.positionLimitType = data.positionLimitType
			msg.cityCode = data.cityCode
			msg.lng = data.lng
			msg.lat = data.lat
			
            local isCoupon = data.type == ActivityItem.ActivityType.Coupon
            if isCoupon then
                msg.coupon_id = data.couponId
                msg.couponName = data.rewardDescribe
                msg.iconUrl = data.iconUrl
                PlatformRedPacketProxy.SetOpenLBSPacketData("Coupon_Open_Data", msg)
				PlatformLBSCouponOpenView.openPlatformLBSCouponOpenView(true)
            else
                PlatformRedPacketProxy.SetOpenLBSPacketData("RedPacket_Open_Data", msg)
                PlatformLBSRedPacketOpenView.openLBSRedPacketOpenView()
            end
        end
    end
	
	item.pressbg:AddEventListener(
        UIEvent.PointerClick,
        function(eventData)
            onClickItemHandler(data)
        end
    )
	--商店名
    item.shop_name_text.text = data.isLocalOfficial and "官方赛" or data.shopName
	--商店头像
	if data.isLocalOfficial then
		--官方赛的商家头像特殊处理
		downloadMerchantHead("MerchantHead/official", item.shop_head_image)
	else
		downloadMerchantHead(data.shopImageUrl, item.shop_head_image)
	end
	
    if this.officialTimeTextList[item.count_time] then
        this.officialTimeTextList[item.count_time] = nil
    end
	
	--可参赛状态
    item.join_state_icon:ChangeIcon(data.joinState)
	
    if data.type == ActivityItem.ActivityType.Competition then
		--赛事
		
		--游戏图标
		item.redbag_icon.gameObject:SetActive(false)
		item.coupon_icon.gameObject:SetActive(false)
        item.game_icon.gameObject:SetActive(true)
        downloadGameIcon(data.gameId, item.game_icon)
		
        item.get_state_icon.gameObject:SetActive(false)

		--奖励类型
		item.reward_icon.gameObject:SetActive(true)
		item.reward_icon:ChangeIcon(data.rewardType)
	
        if data.isLocalOfficial then
			--官方赛
            local official_config = TableBaseOfficalMatch.data[data.match_type]
            local game_config = TableBaseGameList.data[data.gameId]
			
            item.shop_intro_text.text = official_config.title
			
			--结束时间
            item.shop_opentime_text.text = "结束时间:  "..os.date("%H:%M", data.endTime)
			
			--倒计时
			item.shop_reward_bg.gameObject:SetActive(false)
			item.time_bg.gameObject:SetActive(true)
			
            this.officialTimeTextList[item.count_time] = item.count_time
            downloadGameIcon(data.gameId, item.officialGameIcon)
            if this.official_timer ~= data.endTime then
                this.official_timer = data.endTime
            end
            this.CalTimerCount(self)
			return
        end
    else
		item.game_icon.gameObject:SetActive(false)
		if data.type == ActivityItem.ActivityType.RedPacket then
			--红包
			item.redbag_icon.gameObject:SetActive(true)
			item.coupon_icon.gameObject:SetActive(false)
		elseif data.type == ActivityItem.ActivityType.Coupon then
			--优惠券
			item.redbag_icon.gameObject:SetActive(false)
			item.coupon_icon.gameObject:SetActive(true)
		end
			
        item.get_state_icon.gameObject:SetActive(true)
		item.reward_icon.gameObject:SetActive(false)
    end
	
	--活动名
	item.shop_intro_text.text = data.title
	
	--奖励
	item.time_bg.gameObject:SetActive(false)
	item.shop_reward_bg.gameObject:SetActive(true)
    if data.rewardType == ActivityItem.ActivityRewardType.None then
        item.shop_reward_text.text = tostring(data.rewardCount)
    elseif data.rewardType == ActivityItem.ActivityRewardType.Gold then
        item.shop_reward_text.text = tostring(data.rewardCount)
    elseif data.rewardType == ActivityItem.ActivityRewardType.Cash then
        local text = string.format("<size=24>%s</size>%s", "￥", math.floor(data.rewardCount / 100))
        item.shop_reward_text.text = text
    elseif data.rewardType == ActivityItem.ActivityRewardType.UTicket then
        item.shop_reward_text.text = tostring(data.rewardCount)
    elseif data.rewardType == ActivityItem.ActivityRewardType.Coupon then
        item.shop_reward_text.text = data.rewardDescribe
    else
        printError("错误, 未知奖励类型: " .. data.rewardType)
    end

    if data.rewardType == ActivityItem.ActivityRewardType.Coupon then
		local width = item.shop_reward_text.Txt.preferredWidth
		if width < 300 then
			width = 300
		elseif width > 500 then
			width = 500
		end
        item.shop_reward_bg.rectTransform.sizeDelta = Vector2(width, 68)
        item.shop_reward_text.rectTransform.offsetMin = Vector2(0, 0)
        item.shop_reward_text.rectTransform.offsetMax = Vector2(0, 0)
    else
        item.shop_reward_bg.rectTransform.sizeDelta = Vector2(300, 68)
        item.shop_reward_text.rectTransform.offsetMin = Vector2(80, 0)
        item.shop_reward_text.rectTransform.offsetMax = Vector2(0, 0)
    end

    item.shop_opentime_text.text =
        string.format(
        "时间: %s-%s %02s:%02s 至 %s-%s %02s:%02s",
        data.startTime.Month,
        data.startTime.Day,
        data.startTime.Hour,
        data.startTime.Minute,
        data.endTime.Month,
        data.endTime.Day,
        data.endTime.Hour,
        data.endTime.Minute
    )
end








function this.onRspHotGame(notice, rsp)
	local info = rsp:GetObj()
	if info.id == nil then
		info.id = {}
	end
	this:onHotGameInfoChangeHandler(info.id)
end

function this.onRspCompetition(notice, rsp)
	local competitionList = PlatformLBSDataProxy.getAllActivityData()
	local dataList = {}
	if not table.empty(competitionList) then
		for i = 1, #competitionList do
			local activeItem = ActivityItem.createByCompetition(competitionList[i])
			if activeItem.rewardType == ActivityItem.ActivityRewardType.Cash
					and (activeItem.joinState == ActivityItem.JoinState.CanJoin or activeItem.joinState == ActivityItem.JoinState.BeforeStart) then
				table.insert(dataList, activeItem)
			end
		end
	end
	local function sortFun3(a, b)
		local left = a.startTime
		local right = b.startTime
		if left == nil or right == nil then
			return false
		end
		if left == right then
			return false
		end
		return left < right
	end

	local function sortFun2(a, b)
		local left = 0
		local right = 0
		if a.type == ActivityItem.ActivityType.Competition and a.rewardType == ActivityItem.ActivityRewardType.Coupon then
			left = a.rewardCouponCount
			right = b.rewardCouponCount
		else
			left = a.rewardCount
			right = b.rewardCount
		end

		if left == nil or right == nil then
			return false
		end
		if left == right then
			return sortFun3(a, b)
		end
		return left > right
	end
	table.sort(dataList,function(a, b)
		local left = a.sortWeight
		local right = b.sortWeight
		if left == nil or right == nil then
			return false
		end
		if left == right then
			return sortFun2(a, b)
		end
		return left > right
	end)
	local partDataList = {}
	if #dataList <= 6 then
		partDataList = dataList
	else
		for i = 1, 6 do
			table.insert(partDataList, dataList[i])
		end
	end

	this:onCompetitionChangeHandler(partDataList)
end


