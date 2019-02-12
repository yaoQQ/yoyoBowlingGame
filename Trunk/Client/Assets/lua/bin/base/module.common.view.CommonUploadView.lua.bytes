---
--- Created by Administrator.
--- DateTime: 2018\9\18 0018 13:47
---

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "base:mid/common/Mid_common_upload_panel"

CommonUploadView = BaseView:new()
local this = CommonUploadView
this.viewName = "CommonUploadView"

--设置面板特性
this:setViewAttribute(UIViewType.Alert_box, UIViewEnum.CommonUploadView, false)

--设置加载列表
this.loadOrders = {
    "base:common/common_upload_panel"
}

function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = Mid_common_upload_panel
    self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:hide()
end

function this:onShowHandler()
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
end

function this:activeUpdateTip(state, msg)
    ViewManager.open(UIViewEnum.CommonUploadView, nil, function ()
        self.main_mid.title_text.text = msg
        if state then
            self.main_mid.upload_effect:Play()
        else
            self.main_mid.upload_effect:Stop()
            GlobalTimeManager.Instance.timerController:RemoveTimerByKey("CommonUploadViewTimer")
            GlobalTimeManager.Instance.timerController:AddTimer("CommonUploadViewTimer", 500, 1, function ()
                ViewManager.close(UIViewEnum.CommonUploadView)
            end)
        end
    end)
end








