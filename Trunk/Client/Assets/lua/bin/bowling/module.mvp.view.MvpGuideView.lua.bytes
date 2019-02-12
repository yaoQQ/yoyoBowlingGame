require "base:enum/UIViewEnum"
local UIExEventTool = CS.UIExEventTool

--主界面
MvpGuideView = BaseView:new()
local this = MvpGuideView
this.viewName = "MvpGuideView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Help_View, UIViewEnum.MvpGame_Guide_View, false)

--设置加载列表
this.loadOrders = {
    "mvpgame:guide_view"
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    self:endInit()
    self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
    printDebug(self.container.name)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end
local m_goodsPayData = {}
function this:onShowHandler(msg)
    local go = self:getViewGO()
    if go == nil then
        return
    end
    go.transform:SetAsLastSibling()
    self:initView()
    self:addNotice()
end

function this:addEvent()
   -- self.main_mid.back_Button:AddEventListener(UIEvent.PointerClick, self.close)
end
--override 关闭UI回调
function this:onClose()
    self:removeNotice()
  --  GlobalTimeManager.Instance.timerController:RemoveTimerByKey("PayReturnClock")
end

function this:addNotice()
  --  NoticeManager.Instance:AddNoticeLister(MVPGlobalNoticeType.GetPayUrlData, this.updataPayPanel)
end

function this:removeNotice()
  --   NoticeManager.Instance:RemoveNoticeLister(MVPGlobalNoticeType.GetPayUrlData, this.updataPayPanel)
end
--打开界面时初始化
function this:initView()

end


function this.close()
    ViewManager.close(UIViewEnum.MvpGame_Guide_View)
end


-----------------------------------------------------------------------------------
