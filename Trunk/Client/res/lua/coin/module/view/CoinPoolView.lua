---
--- Created by Lichongzhi.
--- DateTime: 2018\10\30 0030 14:31
---

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "coin:mid/Mid_coin_pool_panel"

local Loger = CS.Loger
local UITools = CS.UITools

CoinPoolView = BaseView:new()
local this = CoinPoolView
this.viewName = "CoinPoolView"
--设置面板特性
this:setViewAttribute(UIViewType.Game_1, UIViewEnum.Coin_PoolView)

--设置加载列表
this.loadOrders=
{
    "coin:coin_pool_panel"
}

function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_coin_pool_panel
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    
    self:hide()
end

function this:onShowHandler(msg)
    Loger.PrintWarning(self.viewName," view加载完毕打开时可重写");

end

function this:GetCoinPrefab()
    return self.main_mid.coinItem
end

function this:GetCoin1Sprite()
    return self.main_mid.coin_icon.IconArr[0]
end

function this:GetCoin2Sprite()
    return self.main_mid.coin_icon.IconArr[1]
end

function this:GetCoin5Sprite()
    return self.main_mid.coin_icon.IconArr[2]
end

function this:GetStartTimerList()
    return self.main_mid.timer_icon.IconArr
end

--================================访问器=========================



