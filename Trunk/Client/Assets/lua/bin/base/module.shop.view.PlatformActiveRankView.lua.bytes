require "base:enum/UIViewEnum"
require "base:mid/shop/Mid_platform_active_rank_panel"
-- require "base:enum/PlatformFriendType"

PlatformActiveRankView = BaseView:new()
local this = PlatformActiveRankView
this.viewName = "PlatformActiveRankView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Active_Rank_View, true)

--设置加载列表
this.loadOrders = {
    "base:shop/platform_active_rank_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

function this:onShowHandler(msg)
    this.isOfficial = msg.isofficial
    this:upActiveRankInfo()
end

function this:addEvent()
    self.main_mid.back_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Active_Rank_View)
        end
    )
end

this.currActiveRankData = nil

--更新商家详细信息
function this:upActiveRankInfo()
    this.currActiveRankData = PlatformLBSDataProxy.getActiveRankData()

    if this.currActiveRankData == nil then
        self.main_mid.buttom_Panel.gameObject:SetActive(false)
        self.main_mid.none_game.gameObject:SetActive(true)
        return self.main_mid.rank_CellRecycleScrollPanel:SetCellData({}, this.updateActiveRank, false)
    end

    local target = {}
    local tempData = {}
    local hasUpdateMyRank = nil

    if this.currActiveRankData ~= nil then
        target = this.currActiveRankData

        for i = 1, #target do
            if tostring(target[i].player_id) == tostring(LoginDataProxy.playerId) then
                hasUpdateMyRank = target[i]
                break
            end
        end
    end
    if this.isOfficial and #target > 20 then
        for i = 1, 20 do
            table.insert(tempData, target[i])
        end
    else
        tempData = target
    end
    local tempRect = self.main_mid.rank_CellRecycleScrollPanel.rectTransform
    if hasUpdateMyRank ~= nil then
        self.main_mid.buttom_time_Text.text = hasUpdateMyRank.score .. "分"
        self.main_mid.buttom_rank_Text.text = "第 " .. hasUpdateMyRank.rank .. " 名"
        self.main_mid.buttom_Panel.gameObject:SetActive(true)
        tempRect.offsetMin = Vector2(tempRect.offsetMin.x, 220)
    else
        self.main_mid.buttom_Panel.gameObject:SetActive(false)
        tempRect.offsetMin = Vector2(tempRect.offsetMin.x, 0)
    end
    self.main_mid.none_game.gameObject:SetActive(#tempData <= 0)
    self.main_mid.rank_CellRecycleScrollPanel:SetCellData(tempData, this.updateActiveRank, false)
    -- self.main_mid.apply_CellRecycleScrollPanel:SetCellData(this.currShopDetailInfoData,this.updateFriendCellList,true)
end

--更新排行榜列表
function this.updateActiveRank(go, data, index)
    local item = this.main_mid.rank_CellArr[index + 1]
    downloadUserHead(data.header_url, item.head_CircleImage)

    if data.player_id == LoginDataProxy.playerId then --如果是用户本人
        item.highLight_Image.gameObject:SetActive(true)
        item.player_name_Text.text = string.concat("<color=#815b37>", data.nick_name, "</color>")
        item.use_Time_Text.text = string.concat("<color=#815b37>", data.score, "</color>")
    else
        item.highLight_Image.gameObject:SetActive(false)
        item.player_name_Text.text = data.nick_name
        item.use_Time_Text.text = data.score
    end

    if data.rank > 3 then
        item.rank_Icon:ChangeIcon(3)
        if data.player_id == LoginDataProxy.playerId then --如果是用户本人
            item.rank_Text.text = string.concat("<color=#815b37>", string.format("%02d", data.rank), "</color>")
        else
            item.rank_Text.text = string.format("%02d", data.rank)
        end
    else
        item.rank_Text.text = ""
        item.rank_Icon:ChangeIcon(data.rank - 1)
    end
end
