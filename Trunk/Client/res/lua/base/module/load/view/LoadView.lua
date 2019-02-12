

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "base:mid/Mid_loading_panel"

LoadView=BaseView:new()
local this=LoadView
this.viewName="LoadView"

this.Navgation_mid=nil

--设置面板特性
this:setViewAttribute(UIViewType.Mask_View,UIViewEnum.Load_loadView)

--设置加载列表
this.loadOrders=
{
	--"base:loading_panel"
}



function this:onLoadUIEnd(uiName,gameObject)
	local switch = {
	    [self.loadOrders[1]] = function()
		   self:onLoadLoadUI(gameObject)
	    end,  	    
	} 
   
	local fSwitch = switch[uiName] 

	if fSwitch then 
	   fSwitch() 
	else --key not found  
		printDebug( uiName.." not found !")
	end

end


function this:onLoadLoadUI(gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	--.静态方法

	UITools.SetParentAndAlign(gameObject, self.container)
end


function this:onShowHandler(msg)
	printDebug("=====================LoadView完毕调用======================")
	--NoticeManager.Instance:Dispatch(NoticeType.Mahjong_Load_Scene)
end


