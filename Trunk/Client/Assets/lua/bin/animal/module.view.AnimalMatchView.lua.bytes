---
--- Created by Lichongzhi.
--- DateTime: 2018\10\30 0030 14:31
---

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "animal:mid/Mid_animal_match_panel"

local Loger = CS.Loger
local UITools = CS.UITools

AnimalMatchView = BaseView:new()
local this = AnimalMatchView
this.viewName = "AnimalMatchView"
--设置面板特性
this:setViewAttribute(UIViewType.Game_1, UIViewEnum.Animal_MatchView, true)

--设置加载列表
this.loadOrders=
{
    "animal:animal_match_panel"
}
local MatchTimer = "AnimalMatchViewMatchTimerTimer"
function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_animal_match_panel
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)

end

function this:onShowHandler(msg)
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
    self.main_mid.Slider.value = 0
end

function this:OnMatchStart(player_info)
    local item = self.main_mid.playerItemArr[1]
    local item2 = self.main_mid.playerItemArr[2]
    downloadUserHead(player_info.player_icon, item.portrait_image)
    item.name_text.text = player_info.player_name
    item2.name_bg.gameObject:SetActive(false)
    self.main_mid.match_bg_icon:ChangeIcon(0)
    self.main_mid.Slider.gameObject:SetActive(false)
    self.main_mid.load_tip_text.gameObject:SetActive(false)
    self.main_mid.Slider.gameObject:SetActive(false)
    self.main_mid.load_tip_text.gameObject:SetActive(false)
    local k = 0
    GlobalTimeManager.Instance.timerController:AddTimer(MatchTimer, 600, -1, function ()
        k = k + 1
        local str = ""
        for i = 1, k do
            str = string.concat(str, ".")
        end
        self.main_mid.match_tip_text.text = string.concat("正在匹配对手", str)
        if k == 5 then
            k = 0
        end
    end)
end

function this:OnMatchEnd(match_info_list)
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(MatchTimer)
    self.main_mid.match_tip_text.text = "匹配成功"
    for _, v in pairs(match_info_list) do
        local item
        if v.player_id == LoginDataProxy.playerId then
            item = self.main_mid.playerItemArr[1]
        else
            item = self.main_mid.playerItemArr[2]
        end
        item.name_bg.gameObject:SetActive(true)
        downloadUserHead(v.player_icon, item.portrait_image)
        item.name_text.text = v.player_name
    end
end

function this:OnLoadStart()
    self.main_mid.match_tip_text.text = "比赛即将开始"
    self.main_mid.match_bg_icon:ChangeIcon(1)
    self.main_mid.Slider.gameObject:SetActive(true)
    self.main_mid.load_tip_text.gameObject:SetActive(true)
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

function this:OnLoadStep()
    local progress = AnimalDataProxy:GetMeLoadProgress()
    self:setProgress(progress, 0.2)
    self.main_mid.load_tip_text.text = string.format("加载中%s%s", progress, "%")

end

function this:OnExitMatchView()
    if self:getIsLoaded() == false then
        return
    end
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(MatchTimer)
    coroutine.stop(self.progressCro)
    self.progressCro = nil
end
--================================访问器=========================



