require "base:enum/UIViewEnum"
require "base:mid/shop/Mid_platform_shop_activity_end_panel"

PlatformActiveEndRewardView = BaseView:new()
local this = PlatformActiveEndRewardView
this.viewName = "PlatformActiveEndRewardView"

--设置面板特性
this:setViewAttribute(UIViewType.Pop_view, UIViewEnum.Platform_Active_EndReward_View, true)

--设置加载列表
this.loadOrders = {
    "base:shop/platform_shop_activity_end_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

function this:onShowHandler()
    local go = self:getViewGO()
    go.transform:SetAsLastSibling()
    self:addNotice()
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end

function this:addNotice()
end

function this:removeNotice()
end

function this:addEvent()
    this.main_mid.left_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Active_EndReward_View)
        end
    )
    this.main_mid.right_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Active_EndReward_View)
        end
    )
end
