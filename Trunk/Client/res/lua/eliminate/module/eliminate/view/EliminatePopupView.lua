---
--- Created by lichongzhi.
--- DateTime: 2017/12/4 14:04
---
require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "eliminate:mid/Mid_eliminate_popup_panel"

local Loger = CS.Loger
local UITools = CS.UITools

EliminatePopupView = BaseView:new()
local this = EliminatePopupView
this.viewName = "Eliminate_PopupView"
--设置面板特性
this:setViewAttribute(UIViewType.Pop_view, UIViewEnum.Eliminate_PopupView)

--设置加载列表
this.loadOrders=
{
    "eliminate:eliminate_popup_panel"
}
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_eliminate_popup_panel
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:hide()
end

function this:onShowHandler(msg)
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
end

function this:onClose()
    self.main_mid.btn_1:RemoveEventListener(UIEvent.PointerClick, function () end)
    self.main_mid.btn_2:RemoveEventListener(UIEvent.PointerClick, function () end)
end
--==============================访问器==============================

function this:showPopupView(text, fun1, fun2)
    if self:getIsLoaded() == false then
        return
    end
    ViewManager.open(UIViewEnum.Eliminate_PopupView)
    self.main_mid.info_text.text = text
    self.main_mid.btn_1:AddEventListener(UIEvent.PointerClick, function ()
        ViewManager.close(UIViewEnum.Eliminate_PopupView)
        if fun1 ~= nil then
            fun1()
        end
    end)
    self.main_mid.btn_2:AddEventListener(UIEvent.PointerClick, function ()
        ViewManager.close(UIViewEnum.Eliminate_PopupView)
        if fun2 ~= nil then
            fun2()
        end
    end)
end

function this:onGameOverPopup()
    ViewManager.close(UIViewEnum.Eliminate_PopupView)
end

