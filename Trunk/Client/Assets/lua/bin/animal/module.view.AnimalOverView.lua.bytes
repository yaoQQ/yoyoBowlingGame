---
--- Created by Lichongzhi.
--- DateTime: 2018\10\30 0030 14:31
---

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "animal:mid/Mid_animal_over_panel"

local UITools = CS.UITools
local DOTween = CS.DG.Tweening.DOTween

AnimalOverView = BaseView:new()
local this = AnimalOverView
this.viewName = "AnimalOverView"
--设置面板特性
this:setViewAttribute(UIViewType.Game_1, UIViewEnum.Animal_OverView)

--设置加载列表
this.loadOrders=
{
    "animal:animal_over_panel"
}

function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_animal_over_panel
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:hide()
    self.main_mid.exit_btn:AddEventListener(UIEvent.PointerClick, this.onBtnExit)
end

function this:onShowHandler(msg)
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
end

function this:OnAnimalOverView(game_result)
    ViewManager.open(UIViewEnum.Animal_OverView, nil , function ()
        self:programAnimation(game_result)
    end)
end

function this:programAnimation(game_result)
    self.main_mid.result_icon:ChangeIcon(game_result)
    self.main_mid.result_icon.transform.localScale = Vector3.zero
    self.main_mid.bg_image.rectTransform.anchoredPosition = Vector2(0, 1800)
    self.main_mid.result_icon.transform:DOScale(Vector3.one, 0.4):OnComplete(function ()
        --print("icon动画播放完毕")
        AnimalNetModule.ReqGameRank()
    end)
    if game_result == 0 then
        AudioManager.playSound("animal", "win")
    else
        AudioManager.playSound("animal", "fail_tie")
    end
end

function this:OnGameRank(info)
    AudioManager.playSound("animal", "account")
    self.main_mid.bg_image.transform:DOLocalMove(Vector3(0, 120, 0), 0.4)
    AnimalUtility.CountScroll(0, info.score, self.main_mid.score_text, 0.8)
    self.main_mid.rank_text.text = string.format("当前排名: %s", info.rank)
end

function this:OnExitOverView()
    if self:getIsLoaded() == false then
        return
    end
end

function this.onBtnExit(eventData)
    AnimalNetModule.ReqUpdateMatchState()
end
--================================访问器=========================



