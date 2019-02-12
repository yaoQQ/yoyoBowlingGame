---
--- Created by Lichongzhi.
--- DateTime: 2018\8\7 0007 10:30
---
---@class
require "base:common/util/MethodUtil"
require "base:module/Shop/data/ShopGlobalConfig"
ActivityItem = {}
local this = ActivityItem

function ActivityItem:new()
    local o = {}
    setmetatable(o, self)
    self.__index = self
    return o
end

-- 活动类型
this.ActivityType = {
    RedPacket = 1, -- 红包
    Coupon = 2, -- 优惠券
    Competition = 3, -- 赛事
    All = 4 -- 所有
}

this.ActivityRewardType = {
    Coupon = 0, -- 优惠券
    Gold = 1, -- 金币
    Cash = 2, -- 现金
    Diamond = 3, -- 钻石
    UTicket = 4, -- U券
    None = 5
}
this.JoinState = {
    None = 0, -- 无
    BeforeStart = 1, -- 未开始
    CanJoin = 2, -- 可参赛
    AlreadyJoin = 3 -- 已参赛
}

this.type = 0 -- 活动类型(1红包, 2优惠券, 3赛事)
this.title = "" -- 活动标题
this.gameId = 0 -- 活动(赛事)游戏ID
this.state = 0 -- 活动状态(0未开始, 1可参加, 2已结束)
this.id = 0 -- 活动id（即红包ID/券ID/赛事ID）
this.describe = "" -- 活动描述
this.couponId = 0 -- 优惠券ID(可选)
this.describeImageList = {} -- 描述图片列表, 例如商家的券包里带着商店的商品图片列表
this.packetStyle = 0 -- 红包或优惠券的样式
this.iconUrl = "" -- 红包或优惠券的自定义图片

this.shopImageUrl = nil -- 活动商店的图片地址
this.shopName = "" -- 活动商店名称
this.shopId = 0 -- 商店ID
this.rewardType = this.ActivityRewardType.None -- 奖励类型(1金币, 2现金, 3钻石, 4U券, 5优惠券)
this.rewardDescribe = ""
-- 奖励描述
this.rewardCount = 0 -- 奖励数额
this.rewardCouponCount = 0 -- 奖励优惠券数额(只用于赛事-优惠券时)
this.startTime = nil -- 开始时间 System.DateTime
this.endTime = nil -- 结束时间 System.DateTime
this.sortWeight = 0 -- 排序权重
this.joinState = this.JoinState.None -- 赛事参与状态
this.is_official = false -- 是否为官方赛事

---@param _data MsgMapCashRedPacketSt
---@param _shopUrl string
---@param _shopName string
ActivityItem.createByRedPacket = function(_data, shopData)
    local item = ActivityItem:new()
    item.id = _data.red_packet_id
    item.type = this.ActivityType.RedPacket
    item.title = _data.title
    item.describe = _data.discribe
    item.describeImageList = _data.imgs

    item.startTime = TimeManager.getDateTimeByUnixTime(_data.start_time)
    item.endTime = TimeManager.getDateTimeByUnixTime(_data.end_time)

    --todo用shopId判断是否为官方赛事
    item.is_official = shopData.shop_id < ShopGlobalConfig.OfficialShopId

    item.shopImageUrl = shopData.headurl
    item.shopName = shopData.name
    item.rewardType = this.ActivityRewardType.Cash
    item.rewardCount = _data.money
    item.packetStyle = _data.red_packer_style
    item.sortWeight = 120
	
	item.positionLimitType = _data.position_limit_type
	item.cityCode = _data.city_code
	item.lng = _data.lng
	item.lat = _data.lat
	
    return item
end

---@param _data MsgCouponPropagandaRedPacketSt
---@param _shopUrl string
---@param _shopName string
ActivityItem.createByCoupon = function(_data, shopData)
    local item = ActivityItem:new()
    item.type = this.ActivityType.Coupon
    item.title = _data.title
    item.describe = _data.text
    item.describeImageList = _data.imgs
    item.id = _data.red_packet_id

    item.startTime = TimeManager.getDateTimeByUnixTime(_data.start_time)
    item.endTime = TimeManager.getDateTimeByUnixTime(_data.end_time)

    --todo用shopId判断是否为官方赛事
    item.is_official = shopData.shop_id < ShopGlobalConfig.OfficialShopId

    item.shopImageUrl = shopData.headurl
    item.shopName = shopData.name
    item.couponId = _data.coupon_id
    item.rewardType = this.ActivityRewardType.Coupon
    item.rewardDescribe = _data.coupon_name
    item.rewardCount = _data.num
    item.packetStyle = _data.style
    item.iconUrl = _data.icon
    item.sortWeight = 110
    return item
end

---@param _data MsgShopActivityInfo
ActivityItem.createByCompetition =
    function(_data)
    local item = ActivityItem:new()
    item.type = ActivityItem.ActivityType.Competition

    --本地官方赛事的特殊处理
    if _data.isLocalOfficial then
        item.isLocalOfficial = true
        item.id = _data.active_id
        item.match_type = _data.match_type
        local official_config = TableBaseOfficalMatch.data[_data.match_type] or TableBaseOfficalMatch.data[1]
        item.gameId = official_config.gameid
        item.startTime = _data.start_time
        item.endTime = _data.end_time
        item.game_state = _data.game_state
        if _data.game_state == ProtoEnumCommon.AactiveGameState.AactiveGameState_CanJion then
            item.joinState = this.JoinState.CanJoin
        elseif _data.game_state == ProtoEnumCommon.AactiveGameState.AactiveGameState_UnCanJion then
            item.joinState = this.JoinState.AlreadyJoin
        end

        return item
    end

    item.id = _data.active_info.active_id
    item.title = _data.active_info.apply.subject
    item.startTime = TimeManager.getDateTimeByUnixTime(_data.active_info.apply.start_time)
    item.endTime = TimeManager.getDateTimeByUnixTime(_data.active_info.apply.extra_time)
	
	local shopInfo = PlatformLBSDataProxy.getRedBagShopDataByShopId(_data.active_info.shop_id)
	
    item.shopId = shopInfo.shop_id
    item.shopImageUrl = shopInfo.headurl
    item.shopName = shopInfo.name

    --todo用shopId判断是否为官方赛事
    item.is_official = shopInfo.shop_id < ShopGlobalConfig.OfficialShopId

    local time = TimeManager.getServerUnixTime()
    if time < _data.active_info.apply.start_time then
        item.joinState = this.JoinState.BeforeStart
    elseif time >= _data.active_info.apply.start_time and time <= _data.active_info.apply.extra_time then
        if _data.game_state == ProtoEnumCommon.AactiveGameState.AactiveGameState_CanJion then
            item.joinState = this.JoinState.CanJoin
        elseif _data.game_state == ProtoEnumCommon.AactiveGameState.AactiveGameState_UnCanJion then
            item.joinState = this.JoinState.AlreadyJoin
        else
            printError("错误, 参赛状态错误")
        end
    else
        item.joinState = this.JoinState.None -- temp, 已结束, 理论上不会有该状态
    end

    if _data.active_info.apply.reward_list.reward_type == ProtoEnumCommon.LootType.LootType_Cash then
        item.rewardType = this.ActivityRewardType.Cash
        item.rewardCount = _data.active_info.apply.reward_total
        if item.joinState == this.JoinState.CanJoin then
            item.sortWeight = 100
        elseif item.joinState == this.JoinState.BeforeStart then
            item.sortWeight = 80
        elseif item.joinState == this.JoinState.AlreadyJoin then
            item.sortWeight = 40
        else
            -- printError("错误, 参赛状态错误")
            item.sortWeight = 100
        end
    elseif _data.active_info.apply.reward_list.reward_type == ProtoEnumCommon.LootType.LootType_Coupon then
        item.rewardType = this.ActivityRewardType.Coupon
        local sum = 0
        for _, v in pairs(_data.active_info.apply.reward_list.rewards) do
            for i = v.start_rank, v.end_rank do
                sum = sum + v.active_reward.item_count
            end
        end
        item.rewardCouponCount = sum
        if item.joinState == this.JoinState.CanJoin then
            item.sortWeight = 90
        elseif item.joinState == this.JoinState.BeforeStart then
            item.sortWeight = 70
        elseif item.joinState == this.JoinState.AlreadyJoin then
            item.sortWeight = 30
        else
            -- printError("错误, 参赛状态错误")
            item.sortWeight = 90
        end
    elseif _data.active_info.apply.reward_list.reward_type == ProtoEnumCommon.LootType.LootType_Diamond then
        item.rewardType = this.ActivityRewardType.Diamond
        item.rewardCount = _data.active_info.apply.reward_total
    elseif _data.active_info.apply.reward_list.reward_type == ProtoEnumCommon.LootType.LootType_Money then
        item.rewardType = this.ActivityRewardType.Gold
        item.rewardCount = _data.active_info.apply.reward_total
    else
        printError("错误, 未知的赛事奖励类型")
    end
    item.gameId = _data.active_info.apply.game_type
    item.rewardDescribe = _data.active_info.apply.reward_list.rewards[1].active_reward.item_name

    return item
end
