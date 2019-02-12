require "base:enum/UIViewEnum"
require "base:mid/shop/Mid_platform_active_reward_panel"
require "base:enum/PlatformFriendType"

PlatformActiveRewardView = BaseView:new()
local this = PlatformActiveRewardView
this.viewName = "PlatformActiveRewardView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Active_Reward_View, false)

--设置加载列表
this.loadOrders = {
    "base:shop/platform_active_reward_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

function this:onShowHandler(msg)
    this:upActiveRewardInfo()
end

function this:addEvent()
    self.main_mid.back_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Active_Reward_View)
        end
    )
    self.main_mid.closemask_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Active_Reward_View)
        end
    )
end

this.currActiveRewardData = nil

--更新商家详细信息
function this:upActiveRewardInfo()
    this.currActiveRewardData = PlatformLBSDataProxy.getActivitySingleData()

    if
        this.currActiveRewardData == nil or this.currActiveRewardData.apply == nil or
            this.currActiveRewardData.apply.reward_list == nil
     then
        return
    end

    self.main_mid.rank_CellRecycleScrollPanel:SetCellData(
        this.currActiveRewardData.apply.reward_list.rewards,
        this.updateActiveReward,
        false
    )
    -- self.main_mid.apply_CellRecycleScrollPanel:SetCellData(this.currShopDetailInfoData,this.updateFriendCellList,true)
end

--更新活动奖励列表
function this.updateActiveReward(go, data, index)
    local item = this.main_mid.rankCellArr[index + 1]
    local start_rank = data.start_rank
    local end_rank = data.end_rank
    if start_rank == end_rank then
        if start_rank > 3 then
            item.top_rank_Icon.gameObject:SetActive(false)
            item.other_reward_Text.gameObject:SetActive(true)
            item.other_reward_Text.text = string.concat("第", start_rank, "名")
        else
            item.top_rank_Icon.gameObject:SetActive(true)
            item.other_reward_Text.gameObject:SetActive(false)
            item.top_rank_Icon:ChangeIcon(start_rank - 1)
        end
    else
        item.top_rank_Icon.gameObject:SetActive(false)
        item.other_reward_Text.gameObject:SetActive(true)
        item.other_reward_Text.text = string.concat("第", start_rank, "~", end_rank, "名")
    end

    if data.active_reward.loot_type == ProtoEnumCommon.LootType.LootType_Cash then
        local money = data.active_reward.item_count / 100
        item.reward_Text.text = string.concat("红包", money, "元")
    elseif data.active_reward.loot_type == ProtoEnumCommon.LootType.LootType_Coupon then
        item.reward_Text.text = data.active_reward.item_name.." x "..data.active_reward.item_count
    end
end
