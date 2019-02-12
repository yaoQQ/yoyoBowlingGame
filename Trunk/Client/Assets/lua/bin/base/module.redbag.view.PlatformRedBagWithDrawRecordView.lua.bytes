
require "base:enum/UIViewEnum"
require "base:mid/redbag/Mid_platform_redbag_withdraw_record_panel"
require "base:module/redbag/data/PlatformNewRedBagProxy"

PlatformRedBagWithDrawRecordView= BaseView:new()
local this = PlatformRedBagWithDrawRecordView
this.viewName = "PlatformRedBagWithDrawRecordView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_RedBag_WithDraw_Record_View,true)

--设置加载列表
this.loadOrders=
{
	"base:redbag/platform_redbag_withdraw_record_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

function this:onShowHandler(msg)
	printDebug("=====================PlatformRedBagWithDrawRecordView调用完毕======================")

	local go = self:getViewGO()
	if go == nil then return end
	go.transform:SetAsLastSibling()
    self:addNotice()
end

function this:onClose()
    self:removeNotice()
end

function this:addNotice()
end
    

 function this:removeNotice()
 
 end

---------------------------------------------------------------------------
function this:addEvent()
    self.main_mid.back_Image:AddEventListener(UIEvent.PointerClick,self.geMyRedBag)

end
--------------------------------------------------------------------------
--点击事件
function this:geMyRedBag()
	--ViewManager.open(UIViewEnum.Platform_RedBag_WithDraw_View)
     ViewManager.close(UIViewEnum.Platform_RedBag_WithDraw_Record_View)
     
end

