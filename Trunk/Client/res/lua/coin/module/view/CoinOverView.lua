---
--- Created by Lichongzhi.
--- DateTime: 2018\10\30 0030 14:31
---

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "coin:mid/Mid_coin_over_panel"

local Loger = CS.Loger
local UITools = CS.UITools

CoinOverView = BaseView:new()
local this = CoinOverView
this.viewName = "CoinOverView"
--设置面板特性
this:setViewAttribute(UIViewType.Game_1, UIViewEnum.Coin_OverView)

--设置加载列表
this.loadOrders=
{
    "coin:coin_over_panel"
}
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_coin_over_panel
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid.exit_btn:AddEventListener(UIEvent.PointerClick, this.onPointerClick)

    self:hide()
end

function this:onShowHandler(msg)
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
end

function this:showOverView(rank)
    local score = CoinGameView:GetScore()
    self.main_mid.score_text.text = tostring(score)
    self.main_mid.me_rank_text.text = string.format("当前排名: %s", rank)
end

function this.onPointerClick(eventData)
    CoinNetModule.sendReqUpdateMatchState()
end
--================================访问器=========================



