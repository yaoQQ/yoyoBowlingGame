---
--- Created by Administrator.
--- DateTime: 2017\12\29 0029 11:19
---
require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "eliminate:mid/Mid_eliminate_over_panel"

local Loger = CS.Loger
local UITools = CS.UITools
local UIEvent = CS.UIEvent

EliminateOverView = BaseView:new()
local this = EliminateOverView
this.viewName = "EliminateOverView"
--设置面板特性
this:setViewAttribute(UIViewType.Game_1, UIViewEnum.Eliminate_OverView)

--设置加载列表
this.loadOrders=
{
    "eliminate:eliminate_over_panel"
}

function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_eliminate_over_panel
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid.exit_btn:AddEventListener(UIEvent.PointerClick, self.onBtnExit)
    self:hide()
end

function this:onShowHandler(msg)
    Loger.PrintWarning(self.viewName," view加载完毕打开时可重写");
    self:programAnim()
end

function this:programAnim()
    self.main_mid.over_bg.transform.localPosition = Vector3(0, 1200, 0)
    self.main_mid.over_bg.transform:DOLocalMove(Vector3.zero, 0.25)
end

function this.onBtnExit(eventData)
    EliminateNetModule:sendReqUpdateMatchState()
end


function this:openEliminateOverView(msg)
    ViewManager.open(UIViewEnum.Eliminate_OverView,nil, function ()
        self.main_mid.me_score_text.text = string.format("%s", EliminateDataProxy:GetInstance():getMeScore())
        self.main_mid.me_rank_text.text = string.format("第%s名", msg.player_rank_info.rank)
        self.main_mid.rank_scroll_panel:SetCellData(msg.rank_info_list, this.showRankInfo, true)
    end)
end

function this.showRankInfo(go, data,index)
    local item =  this.main_mid.rangItemArr[index+1]
    item.rank_text.text = string.format("第%s名", data.rank)
    item.name_text.text = tostring(data.nick_name)
    item.score_text.text = tostring(data.score)
    item.reward_text.text = CS.System.String.Format("{0:0.##}", data.item_count / 100)
    local meColor = "#FFFC00FF"
    local otherColor = "#95DAF4FF"
    if data.player_id == EliminateDataProxy:GetInstance():GetMeUid() then
        item.rank_text.Txt.color = UIExEventTool.HexToColor(meColor)
        item.name_text.Txt.color = UIExEventTool.HexToColor(meColor)
        item.score_text.Txt.color = UIExEventTool.HexToColor(meColor)
        item.reward_text.Txt.color = UIExEventTool.HexToColor(meColor)
    else
        item.rank_text.Txt.color = UIExEventTool.HexToColor(otherColor)
        item.name_text.Txt.color = UIExEventTool.HexToColor(otherColor)
        item.score_text.Txt.color = UIExEventTool.HexToColor(otherColor)
        item.reward_text.Txt.color = UIExEventTool.HexToColor(otherColor)
    end
    if data.loot_type == ProtoEnumCommon.LootType.LootType_Cash then
        item.reward_icon:ChangeIcon(2)
    else
        item.reward_icon:ChangeIcon(0)
    end
end

function this:closeEliminateOverView()
    if self:getIsLoaded() then
        self:hide()
    end
end

