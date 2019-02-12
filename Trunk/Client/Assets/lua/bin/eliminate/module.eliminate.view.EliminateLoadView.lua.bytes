---
--- Created by Lichongzhi.
--- DateTime: 2018\3\19 0019 17:30
---
require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "eliminate:mid/Mid_eliminate_load_panel"

local Loger = CS.Loger
local UITools = CS.UITools
local Mathf = CS.UnityEngine.Mathf
local Time = CS.UnityEngine.Time


EliminateLoadView = BaseView:new()
local this = EliminateLoadView
this.viewName = "Eliminate_LoadView"
--设置面板特性
this:setViewAttribute(UIViewType.Loading_View, UIViewEnum.Eliminate_LoadView, true)

--设置加载列表
this.loadOrders=
{
    "eliminate:eliminate_load_panel"
}

function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
end

function this:onShowHandler(msg)
    Loger.PrintWarning(self.viewName, "view加载完毕打开时可重写")
    self.main_mid.Slider.value = 0
    self.main_mid.handleeffect:Play()
end

function this:openEliminateLoadView(action)
    ViewManager.open(UIViewEnum.Eliminate_LoadView, nil, action)
end

this.isProSuccess = false
function this:setProgress(progress, expendTime, action)
    if self:getIsLoaded() == false then
        print(string.format("%s未初始化",self))
        return
    end
    if expendTime <= 0 then
        return
    end
    local progress = Mathf.Clamp(progress, 0, 1)
    local curValue = self.main_mid.Slider.value
    local deltaPro = progress - curValue
    local fixedDeltaPro = deltaPro / (50 * expendTime)
    --print("curValue = "..curValue)
    --print("deltaPro = "..deltaPro)
    --print("fixedDeltaPro = "..fixedDeltaPro)
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
--==================================事件===================================
function this:onExitLoadView()
    if self:getIsLoaded() == false then
        return
    end
    coroutine.stop(self.progressCro)
    self.progressCro = nil
    self.main_mid.Slider.value = 0
end



function this:OnLoadStep()
    local progress = EliminateDataProxy:GetInstance():GetMeLoadProgress()
    self:setProgress(progress / 100, 0.1)
end
