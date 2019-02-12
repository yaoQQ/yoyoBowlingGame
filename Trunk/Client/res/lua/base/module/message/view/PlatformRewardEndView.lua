require "base:mid/message/Mid_platform_message_reward_end_panel"
require "base:module/message/data/PlatformMessageProxy"

--主界面：消息
PlatformRewardEndView = BaseView:new()
local this = PlatformRewardEndView
this.viewName = "PlatformRewardEndView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Message_Reward_View, false)

--设置加载列表
this.loadOrders = {
    "base:message/platform_message_reward_end_panel"
}
local m_messageType = nil
local m_rewardListData = {}
--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
    this.main_mid.end_bg_Image.gameObject:SetActive(false)
end

--override 打开UI回调
function this:onShowHandler(msg)
    local go = self:getViewGO()
    if go == nil then
        return
    end
    go.transform:SetAsLastSibling()
    self:addNotice()
	
	m_rewardListData = msg
    self:updateRewardList()
end

--override 关闭UI回调
function this:onClose()
    --关闭子界面
end

function this:addNotice()
    --NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Get_Loot, this.updateRewardList)
end

function this:removeNotice()
    -- NoticeManager.Instance:RemoveNoticeLister(PlatformGlobalNoticeType.Platform_Get_Loot, this.updateRewardList)
end

function this:addEvent()
    self.main_mid.end_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Message_Reward_View)
            NoticeManager.Instance:Dispatch(PlatformGlobalNoticeType.Platform_Get_All_Mail)
        end
    )
end
local SortLootType = {
    -- 现金
    LootType_Cash = 1,
    -- 钻石
    LootType_Diamond = 2,
    -- 金币
    LootType_Money = 3,
    -- 优卡
    LootType_UCard = 4,
    -- 优惠券
    LootType_Coupon = 5
}
local couponNum = 0
function this.updateRewardList()
    if m_rewardListData == nil or m_rewardListData == "" or m_rewardListData.loot_item_list == nil then
        return ViewManager.close(UIViewEnum.Platform_Message_Reward_View)
    end
    local cRectm = this.main_mid.end_bg_Image.rectTransform
    local count = m_rewardListData.loot_item_list and #m_rewardListData.loot_item_list or 0
    local panelHeight = 230 * count + 350
    if panelHeight > 940 then
        panelHeight = 940
    end
    cRectm.sizeDelta = Vector2(808, panelHeight)

    --后端好像已经排好序了，这里暂时注释,若需要则打开
    -- table.sort(
    --     m_rewardListData.loot_item_list,
    --     function(a, b)
    --         if SortLootType[a.loot_type] and SortLootType[b.loot_type] then
    --             if a.loot_type ~= b.loot_type then
    --                 return SortLootType[a.loot_type] < SortLootType[b.loot_type]
    --             end
    --         end
    --         return false
    --     end
    -- )
    

    local listData = {}
    for i, v in pairs(m_rewardListData.loot_item_list) do
        --过滤其它奖励类型不显示
        if SortLootType[v.loot_type] then
            table.insert(listData, v)
        end
    end
    
    this.main_mid.end_CellRecycleScrollPanel:SetCellData(listData, this.onUpdateRewardList, true)
end

function this.onUpdateRewardList(go, data, index)
    local item = this.main_mid.endCellArr[index + 1]
    if data.loot_type == ProtoEnumCommon.LootType.LootType_Coupon then
        item.noget_reward_icon:ChangeIcon(0)
        printDebug("+++++++++++++++++++++++++所获取的优惠券数据= "..table.tostring(data))
        item.reward_Text.text = data.item_name.."  x"..data.item_count
    elseif data.loot_type == ProtoEnumCommon.LootType.LootType_Cash then
        item.noget_reward_icon:ChangeIcon(1)
        item.reward_Text.text = "红包" .. (data.item_count / 100) .. "元"
    elseif data.loot_type == ProtoEnumCommon.LootType.LootType_Diamond then
        item.noget_reward_icon:ChangeIcon(2)
        item.reward_Text.text = "钻石 x" .. data.item_count
    elseif data.loot_type == ProtoEnumCommon.LootType.LootType_Money then
        item.noget_reward_icon:ChangeIcon(3)
        item.reward_Text.text = "金币 x" .. data.item_count
    elseif data.loot_type == ProtoEnumCommon.LootType.LootType_UCard then
        item.noget_reward_icon:ChangeIcon(4)
        item.reward_Text.text = "优卡 x" .. data.item_count
    else
        --未知奖励类型
        item.noget_reward_icon:ChangeIcon(4)
        item.reward_Text.text = "其他奖励 x" .. data.item_count
    end

    this.main_mid.end_bg_Image.gameObject:SetActive(true)
end
