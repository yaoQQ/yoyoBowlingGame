---
--- Created by Lichongzhi.
--- DateTime: 2018\10\30 0030 14:31
---

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "animal:mid/Mid_animal_pool_panel"

local Loger = CS.Loger
local UITools = CS.UITools

AnimalPoolView = BaseView:new()
local this = AnimalPoolView
this.viewName = "AnimalPoolView"
--设置面板特性
this:setViewAttribute(UIViewType.Game_1, UIViewEnum.Animal_PoolView)

--设置加载列表
this.loadOrders=
{
    "animal:animal_pool_panel"
}

function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_animal_pool_panel
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    
    self:hide()
end

function this:onShowHandler(msg)
    Loger.PrintWarning(self.viewName," view加载完毕打开时可重写");

end

function this:GetAnimalPrefab()
    return self.main_mid.animalPre.gameObject
end

function this:GetNormalSprite()
    return self.main_mid.normal_icon.IconArr
end

function this:GetSelectedSprite()
    return self.main_mid.selected_icon.IconArr
end
