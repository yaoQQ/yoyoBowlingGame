require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "base:mid/common/Mid_common_searching_panel"

SearchView = BaseView:new()
local this = SearchView
this.viewName = "SearchView"

this.reasonTable = {}

--设置面板特性
this:setViewAttribute(UIViewType.MapFloat_View, UIViewEnum.SearchView, false)

--设置加载列表
this.loadOrders = {
    "base:common/common_searching_panel"
}

function this:onLoadUIEnd(uiName, gameObject)
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self.main_mid.go:SetActive(true)
end

function this:onShowHandler(msg)
    self:startRotate()

    for _, v in pairs(this.reasonTable) do
        if v == true then
            return
        end
    end
end

function this:onClose()
    this.removeAllTimer()
end

function this.showOrHide(isShow, reason)
    if isShow then
        this.reasonTable[reason] = true
        this.onShadePanel(true)
    else
        this.reasonTable[reason] = nil
        for _, v in pairs(this.reasonTable) do
            if v == true then
                return
            end
        end
        this.onShadePanel(false)
    end
end

function this:startRotate()
    GlobalTimeManager.Instance.timerController:AddTimer("startRotate", 400, -1, self.onUpdateRotate)
end
this._speed = 0
this.IconIndex = 0
function this.onUpdateRotate()
    this.IconIndex = this.IconIndex >= 4 and 0 or this.IconIndex + 1
    this.main_mid.image_change:ChangeIcon(this.IconIndex)
end
local cor = nil
--搜索更新(true为打开)
function this.onShadePanel(isOPen)
    if isOPen then
        PlatformLbsView.IconPanelSetAplha(0)
        ViewManager.open(UIViewEnum.SearchView)
        NoticeManager.Instance:Dispatch(NoticeType.LBS_Search_Notice, {reason = "activityClassify", isShow = true})
    else
        if cor then
            coroutine.stop(cor)
            cor = nil
        end
        cor =
            coroutine.start(
            function()
                coroutine.wait(cor, 1000)
                ViewManager.close(UIViewEnum.SearchView)
                NoticeManager.Instance:Dispatch(
                    NoticeType.LBS_Search_Notice,
                    {reason = "activityClassify", isShow = false}
                )
            end
        )
    end
end

function this.removeAllTimer()
    GlobalTimeManager.Instance.timerController:RemoveTimerByKey("startRotate")
    if cor then
        coroutine.stop(cor)
        cor = nil
    end
    PlatformLbsView.IconPanelSetAplha(1)
end
