---
--- Created by lichongzhi.
--- DateTime: 2017/12/4 14:04
---


require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "eliminate:mid/Mid_eliminate_pool_panel"

local Loger = CS.Loger
local NoticeManager = CS.NoticeManager
local UITools = CS.UITools
local Vector3 = CS.UnityEngine.Vector3

EliminatePoolView = BaseView:new()
local this = EliminatePoolView
this.viewName = "Eliminate_PoolView"
--设置面板特性
this:setViewAttribute(UIViewType.Game_1, UIViewEnum.Eliminate_PoolView)

--设置加载列表
this.loadOrders=
{
    "eliminate:eliminate_pool_panel"
}

function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_eliminate_pool_panel
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    
    self:hide()
end

function this:onShowHandler(msg)
    Loger.PrintWarning(self.viewName," view加载完毕打开时可重写");

end

--重置poolView的对象
function this:resetPoolObject(obj)
    obj.transform:SetParent(self.main_mid.go.transform)
    obj.transform.localPosition = Vector3.zero
    obj.transform.localScale = Vector3.one
end

--=========================================访问器=========================
function this:getInfo()
    return self
end

function this:getNormalList()
    return self.main_mid.normalList.IconArr
end

function this:getSelectedList()
    return self.main_mid.selectedList.IconArr
end

function this:getEliminateList()
    return self.main_mid.eliminateList.IconArr
end

function this:getSpecialList()
    return self.main_mid.specialList.IconArr
end

function this:getEvaluateList()
    return self.main_mid.evaluateList.IconArr
end

function this:getStartTimerList()
    return self.main_mid.timerList.IconArr
end

function this:getItemList()
    return self.main_mid.itemList.IconArr
end

function this:GetRenderMat()
    return self.main_mid.mat_image.Img.material
end
--=========================================事件响应=========================


