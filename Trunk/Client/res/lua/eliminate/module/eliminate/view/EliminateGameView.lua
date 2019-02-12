---
--- Created by lichongzhi.
--- DateTime: 2017/12/4 14:04
---
require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "eliminate:mid/Mid_eliminate_game_panel"

local Loger = CS.Loger
local NoticeManager = CS.NoticeManager
local UITools = CS.UITools
local UIEvent = CS.UIEvent
local Vector3 = CS.UnityEngine.Vector3
local DOTween = CS.DG.Tweening.DOTween

EliminateGameView = BaseView:new()
local this = EliminateGameView
this.viewName = "Eliminate_GameView"
--设置面板特性
this:setViewAttribute(UIViewType.Game_1, UIViewEnum.Eliminate_GameView, true)

this.preEvaluate = 0
this.sceneEffectObj = nil
this.uiEffectObj = nil
this.dynamicDic = nil
local DazzleTimerKey = "EliminateDazzleTimer"
--设置加载列表
this.loadOrders=
{
    "eliminate:eliminate_game_panel"
}

function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid=Mid_eliminate_game_panel
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid.exit_btn:AddEventListener(UIEvent.PointerClick, this.onBtnExit)
    self.main_mid.game_control_panel:AddEventListener(UIEvent.PointerDown, this.onPointerDown)
    self.main_mid.game_control_panel:AddEventListener(UIEvent.Drag, this.onPointerDrag)
    self.main_mid.shuffle_btn:AddEventListener(UIEvent.PointerClick, this.onShuffleBtn)
    self:hide()
    self:initDynamicUI()
end

function this:initDynamicUI()
    self.dynamicDic = {}
    local players = EliminateDataProxy:GetInstance():GetAllPlayer()
    for _, v in pairs(players) do
        local item = self.main_mid.player_dui_itemArr[v:getSeat() + 1]
        if item ~= nil then
            self.dynamicDic[v:getUid()] = item
        end
    end
    for i = 1, #self.main_mid.player_dui_itemArr do
        self.main_mid.player_dui_itemArr[i].go:SetActive(false)
    end
end

function this:onShowHandler(msg)
    Loger.PrintWarning(self.viewName," view加载完毕打开时可重写");
    self:resetGameView()
    self.main_mid.over_timer_text.text = "00:00"
end

-- 更新分数显示
function this:UpdateScoreText()
    local score = EliminateDataProxy:GetInstance():getMeScore()
    self.main_mid.playerScore_text.text = tostring(score)
end

function this:initCountBgView()
    self.main_mid.count_fg.gameObject:SetActive(true)
    self.main_mid.count_fg.Img.fillAmount = 1
    self.main_mid.playerScore_text.text = "0"
    self.main_mid.fre_left_fg.Img.fillAmount = 0
    self.main_mid.fre_right_fg.Img.fillAmount = 0
end

function this:showComboProgress(expendTime)
    local progress = EliminateGrid:GetInstance():GetComboProgress()
    --print("progress: "..progress)
    --self.main_mid.fre_left_fg.Img.fillAmount = progress
    --self.main_mid.fre_right_fg.Img.fillAmount = progress
    local progress = Mathf.Clamp(progress, 0, 1)
    local curValue = self.main_mid.fre_left_fg.Img.fillAmount
    local deltaPro = progress - curValue
    local fixedDeltaPro = deltaPro / (50 * expendTime)
    --print("curValue = "..curValue)
    --print("deltaPro = "..deltaPro)
    --print("fixedDeltaPro = "..fixedDeltaPro)
    local progressCro = nil
    progressCro = coroutine.start(function ()
        for t = 0, expendTime, Time.fixedDeltaTime do
            self.main_mid.fre_left_fg.Img.fillAmount = self.main_mid.fre_left_fg.Img.fillAmount + fixedDeltaPro
            self.main_mid.fre_right_fg.Img.fillAmount = self.main_mid.fre_right_fg.Img.fillAmount + fixedDeltaPro
            coroutine.step(progressCro)
        end
    end)
end

-- 更新计数显示
function this:updateCountView()
    local progress = EliminateDataProxy:GetInstance():getGameProgress()
    self.main_mid.count_fg.Img.fillAmount = progress

    self.main_mid.over_timer_text.gameObject:SetActive(true)
    local remain_time = EliminateDataProxy:GetInstance():getGameTimerCount()
    local downTime = TimeSpan.FromSeconds(remain_time)
    self.main_mid.over_timer_text.text = string.format("%02s:%02s", downTime.Minutes,downTime.Seconds)
end

function this:getInfo()
    return self
end

function this:openEliminateGameView(action)
    ViewManager.open(UIViewEnum.Eliminate_GameView,nil, action)
end

function this:closeEliminateGameView()
    if self:getIsLoaded() == false then
        return
    end
    self:hide()
end

function this:resetGameView()
    self.main_mid.item_tip_text.gameObject:SetActive(false)
    self.main_mid.mask.gameObject:SetActive(false)
    self.main_mid.shuffle_btn.gameObject:SetActive(false)
    self.main_mid.start_timer_image.gameObject:SetActive(false)
    self.main_mid.timeUp_image.gameObject:SetActive(false)
    self.main_mid.evaluate_image.Img:CrossFadeAlpha(0, 0, true)

    self.main_mid.combo_img.Img:CrossFadeAlpha(0, 0, true)
    self.main_mid.combo_symbol_text.Txt:CrossFadeAlpha(0, 0, true)
    self.main_mid.combo_text.Txt:CrossFadeAlpha(0, 0, true)
end

function this:GetDynamicUIByUid(uid)
    return self.dynamicDic[uid]
end

function this:GetControlRectTransform()
    return self.main_mid.piece_container.rectTransform
end

function this:GetScoreTextWidget()
    return self.main_mid.playerScore_text
end

function this:poolEffect()
    if self.sceneEffectObj ~= nil and self.uiEffectObj ~= nil then
        ObjectPool:GetInstance():poolObject(self.sceneEffectObj)
        ObjectPool:GetInstance():poolObject(self.uiEffectObj)
        self.sceneEffectObj = nil
        self.uiEffectObj = nil
    end
end

--=================================事件响应============================

function this:onLoadCompleteGameView()
    self:initCountBgView()
    self:resetGameView()
    self:openEliminateGameView()
end

function this:onStartTimerGameView()
    local count = EliminateDataProxy:GetInstance():getStartTimerCount()
    local timerImageList = EliminatePoolView:getStartTimerList()
    if count < 0 or count >= timerImageList.Length then
        Loger.PrintError("错误-开始倒计时计数错误, count = ", count)
        return;
    end
    local image = timerImageList[count]
    local curObj = self.main_mid.start_timer_image
    curObj.gameObject:SetActive(true)
    curObj.Img:CrossFadeAlpha(1, 0, true)
    curObj:SetPng(image)
    curObj.Img:SetNativeSize()
    local mySequence = DOTween.Sequence()
    curObj.transform.localScale = Vector3.one * 0.5
    local scale_1 = curObj.transform:DOScale(Vector3.one * 1.2, 0.3)
    local scale_2 = curObj.transform:DOScale(Vector3.one, 0.2)
    mySequence:Append(scale_1)
    mySequence:Append(scale_2)
    mySequence:AppendCallback(function ()
        curObj.Img:CrossFadeAlpha(0, 0.5, true)
    end)
end

function this:onEnteredSceneGameView()
    local stageCam = EliminateScene:GetStageCamera()
    local mat = EliminatePoolView:GetRenderMat()
    stageCam:SetTargetTexture(mat.mainTexture)
    self.main_mid.stage_render.Img.sprite = nil
    self.main_mid.stage_render.Img.material = mat
end

function this:onOverGameView()
    self:updateCountView()
end

function this:onCountDecreaseGameView()
    self:updateCountView()
end

function this:onShuffleGameView()
    self.main_mid.shuffle_btn.gameObject:SetActive(true)
    self.main_mid.tip_text.text = "没有可以消除的小魔怪了"
end

function this:hideShuffleBtn()
    self.main_mid.shuffle_btn.gameObject:SetActive(false)
end

function this:isInSection(value, min , max)
    if value >= min and value <= max then
        return true
    end
    return false
end

function this:getDeltaCountToNextEva(curEva, curCombo)
    local nextEvaNeedCount = 0
    if curEva == 0 then
        nextEvaNeedCount = EliminateConfig.ComboEvaNeedCount.First + 1
    elseif curEva == 1 then
        nextEvaNeedCount = EliminateConfig.ComboEvaNeedCount.Second + 1
    elseif curEva == 2 then
        nextEvaNeedCount = EliminateConfig.ComboEvaNeedCount.Third + 1
    elseif curEva == 3 then
        nextEvaNeedCount = EliminateConfig.ComboEvaNeedCount.Fourth + 1
    else
        Loger.PrintError("错误, 当前评价已为最高级, 不存在下一个评价")
    end
    return nextEvaNeedCount - curCombo
end

function this:onComboGameView()
    self:showComboProgress(0.1)
    local comboCount = EliminateGrid:GetInstance():getComboCount()
    if comboCount == 0 then
        return
    end
    local function getEvaluateByCombo(comboCount)
        if self:isInSection(comboCount, 1, EliminateConfig.ComboEvaNeedCount.First) then
            return EliminateConfig.ComboEvaLevel.First
        elseif self:isInSection(comboCount, EliminateConfig.ComboEvaNeedCount.First + 1, EliminateConfig.ComboEvaNeedCount.Second) then
            return EliminateConfig.ComboEvaLevel.Second
        elseif self:isInSection(comboCount, EliminateConfig.ComboEvaNeedCount.Second + 1, EliminateConfig.ComboEvaNeedCount.Third) then
            return EliminateConfig.ComboEvaLevel.Third
        elseif self:isInSection(comboCount,EliminateConfig.ComboEvaNeedCount.Third + 1, EliminateConfig.ComboEvaNeedCount.Fourth) then
            return EliminateConfig.ComboEvaLevel.Fourth
        else
            return EliminateConfig.ComboEvaLevel.Fifth
        end
    end
    local function doScaleText(textWidget)
        textWidget.Txt:CrossFadeAlpha(1, 0, true)
        textWidget.transform.localScale = Vector3.one * 0.5
        textWidget.transform:DOScale(Vector3.one * 1.5, 0.2):OnComplete(function ()
            textWidget.transform:DOScale(Vector3.one , 0.2)
        end)
    end

    self.main_mid.combo_text.text = tostring(comboCount)
    doScaleText(self.main_mid.combo_text)
    self.main_mid.combo_img.Img:CrossFadeAlpha(1, 0, true)
    self.main_mid.combo_symbol_text.Txt:CrossFadeAlpha(1, 0, true)

    self.main_mid.combo_text.Txt:CrossFadeAlpha(0, 1.5, true)
    self.main_mid.combo_img.Img:CrossFadeAlpha(0, 1.5, true)
    self.main_mid.combo_symbol_text.Txt:CrossFadeAlpha(0, 1.5, true)

    local evaluateList = EliminatePoolView:getEvaluateList()
    local curEvaluate = getEvaluateByCombo(comboCount)
    self.main_mid.evaluate_image:SetPng(evaluateList[curEvaluate])
    self.main_mid.evaluate_image.Img:CrossFadeAlpha(1, 0, true)
    self.main_mid.evaluate_image.Img:SetNativeSize()
    self.main_mid.evaluate_image.Img:CrossFadeAlpha(0, 1, true)
    self.main_mid.evaluate_image.transform.localScale = Vector3.zero
    self.main_mid.evaluate_image.transform:DOScale(Vector3.one, 0.3)

    local deltaCountToNextEva = 0
    if curEvaluate ~= EliminateConfig.ComboEvaLevel.Fifth then
        deltaCountToNextEva = self:getDeltaCountToNextEva(self.preEvaluate, comboCount)
    else
        deltaCountToNextEva = (comboCount - EliminateConfig.ComboEvaNeedCount.Fourth) % 5
    end
    AudioManager.playSound("eliminate", string.format("evaluate_%s", tostring(deltaCountToNextEva)))
    if curEvaluate == 0 then
        return
    end
    if self.preEvaluate~= 0 and curEvaluate == self.preEvaluate then
        return
    end
    self:poolEffect()

    if self.preEvaluate ~= EliminateConfig.ComboEvaLevel.Fifth and curEvaluate == EliminateConfig.ComboEvaLevel.Fifth then
        NoticeManager.Instance:Dispatch(EliminateNoticeType.DazzleMomentStart)
    end
    self.preEvaluate = curEvaluate

end

function this:onComboBreakGameView()
    if self.preEvaluate == EliminateConfig.ComboEvaLevel.Fifth then
        NoticeManager.Instance:Dispatch(EliminateNoticeType.DazzleMomentOver)
    end
    self.preEvaluate = 0
    self:poolEffect()
    self:showComboProgress(0.1)

end

function this:OnDazzleStartGameView()
    local danceEffect = ObjectPool:GetInstance():getObject("fx_dance_moment")
    danceEffect.transform.position = Vector3.zero
    self.fireWorksCro = nil
    self.fireWorksCro = coroutine.start(function ()
        coroutine.wait(self.fireWorksCro , 1000)
        ObjectPool:GetInstance():poolObject(danceEffect)
    end)
end

function this:onAccountClearGameView()
    self.main_mid.mask.gameObject:SetActive(false)
    self.main_mid.timeUp_image.gameObject:SetActive(false)
end

function this:onItemActiveGameView(dec)
    --local dec = ItemDataBase.query(itemType).activeDec
    self.main_mid.item_tip_text.gameObject:SetActive(true)
    self.main_mid.item_tip_text.text = dec
    self.main_mid.item_tip_text.Txt:CrossFadeAlpha(1, 0, true)
    self.main_mid.item_tip_text.Txt:CrossFadeAlpha(0, 2, true)
end

function this:onTimeUpGameView()
    self:poolEffect()
    self.main_mid.mask.gameObject:SetActive(true)
    self.main_mid.timeUp_image.gameObject:SetActive(true)
    self.main_mid.timeUp_image.transform.localScale = Vector3.zero
    self.main_mid.timeUp_image.transform:DOScale(Vector3.one , 0.2)
end

function this:onExitGameView()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey(DazzleTimerKey)
    coroutine.stop(self.fireWorksCro)
    self.fireWorksCro = nil
    self:poolEffect()
    self:closeEliminateGameView()
end

function this.onBtnExit(eventData)
    EliminatePopupView:showPopupView("是否退出当前游戏?", nil, function ()
        EliminateDataProxy:GetInstance():onExitDataProxy()
        EliminateGrid:GetInstance():onExitReq()
        EliminateNetModule:sendReqUpdateMatchState()
    end)
end

function this.onPointerDrag(eventData)
    NoticeManager.Instance:Dispatch(EliminateNoticeType.PointedDrag, {screenPosition = eventData.position, delta = eventData.delta})
end

function this.onPointerDown(eventData)
    NoticeManager.Instance:Dispatch(EliminateNoticeType.PointedDown, {screenPosition = eventData.position})
end

function this.onShuffleBtn()

end