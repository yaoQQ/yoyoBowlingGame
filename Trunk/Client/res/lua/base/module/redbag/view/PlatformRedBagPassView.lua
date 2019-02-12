require "base:enum/UIViewEnum"
require "base:mid/redbag/Mid_platform_redbag_pass_panel"

PlatformRedBagPassView = BaseView:new()
local this = PlatformRedBagPassView
this.viewName = "PlatformRedBagPassView"

--设置面板特性
this:setViewAttribute(UIViewType.Pop_view, UIViewEnum.Platform_RedBag_Pass_View, false)

--设置加载列表
this.loadOrders = {
    "base:redbag/platform_redbag_pass_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

function this:onShowHandler(msg)
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
    this.main_mid.pass_mask_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_RedBag_Pass_View)
        end
    )
    this.main_mid.right_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_RedBag_Pass_View)   
        end
    )
end


