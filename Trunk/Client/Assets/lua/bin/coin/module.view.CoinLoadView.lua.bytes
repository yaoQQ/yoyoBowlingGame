---
--- Created by Lichongzhi.
--- DateTime: 2018\10\30 0030 14:31
---

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "coin:mid/Mid_coin_load_panel"

local Loger = CS.Loger
local Mathf = CS.UnityEngine.Mathf
local Time = CS.UnityEngine.Time


CoinLoadView = BaseView:new()
local this = CoinLoadView
this.viewName = "CoinLoadView"
--设置面板特性
this:setViewAttribute(UIViewType.Loading_View, UIViewEnum.Coin_LoadView)

--设置加载列表
this.loadOrders=
{
    "coin:coin_load_panel"
}
this.totalPreLoadCount = 0
this.curPreLoadCount = 0
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_coin_load_panel
    self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid.Slider.value = 0
end

function this:onShowHandler(msg)
    Loger.PrintWarning(self.viewName, "view加载完毕打开时可重写")
end

function this:OpenCoinLoadView(action)
    local preCount = CoinPreload:getPreLoadCount()
    local poolCount = CoinObjectPool:GetInstance():GetPoolCount()
    self.totalPreLoadCount = preCount + poolCount

    ViewManager.open(UIViewEnum.Coin_LoadView, nil, action)
end

this.isProSuccess = false
function this:setProgress(progress, expendTime)
    if self:getIsLoaded() == false then
        return
    end
    if expendTime <= 0 then
        return
    end
    self.isProSuccess = false
    local progress = Mathf.Clamp(progress, 0, 1)
    local curValue = self.main_mid.Slider.value
    local deltaPro = progress - curValue
    local fixedDeltaPro = deltaPro / (50 * expendTime)
    self.progressCro = nil
    self.progressCro = coroutine.start(function ()
        for t = 0, expendTime, Time.fixedDeltaTime do
            self.main_mid.Slider.value = self.main_mid.Slider.value + fixedDeltaPro
            coroutine.step(self.progressCro)
        end
        self.isProSuccess = true
    end)
end

function this:GetIsProSuccess()
    return self.isProSuccess
end

function this:OnLoadingLoadView()
    self.curPreLoadCount = self.curPreLoadCount + 1
    local progress = math.floor((self.curPreLoadCount / self.totalPreLoadCount) * 100)
    self:setProgress(progress, 0.2)
    if self.curPreLoadCount >= self.totalPreLoadCount then
        print("加载完成")
        NoticeManager.Instance:Dispatch(CoinNoticeType.LoadComplete)
    end
end

function this:OnExitLoadView()
    if self:getIsLoaded() == false then
        return
    end
    self.progressCro = nil
end


