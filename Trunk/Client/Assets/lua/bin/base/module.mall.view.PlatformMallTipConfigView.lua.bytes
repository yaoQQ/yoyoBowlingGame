require "base:mid/mall/Mid_platform_mall_tip_config_panel"

PlatformMallTipConfigView = BaseView:new()
local this = PlatformMallTipConfigView
this.viewName = "PlatformMallTipConfigView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Mall_Tip_Config_View, false)

--设置加载列表
this.loadOrders=
{
	"base:mall/platform_mall_tip_config_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
end

function this:addEvent()

	
end


function this:onShowHandler(msg)
	self:addEvent()
	self:initView()
	local tipEnum=msg
	this:updateTipEnum(tipEnum)
	
	
end

--更新商城界面
function this:updateTipEnum(tipEnum)
	if this.tipPanel ~=nil then
		this.tipPanel.gameObject:SetActive(false)
	end


	if tipEnum==1 then 
		this.tipPanel=this.main_mid.cost_que_Panel
		elseif tipEnum==2 then
		this.tipPanel=this.main_mid.gold_cost_end_Panel
		elseif tipEnum==3 then
		this.tipPanel=this.main_mid.diamond_cost_end_Panel
		elseif tipEnum==4 then
		this.tipPanel=this.main_mid.cost_exchange_Panel
		else
		printDebug("++++++++++++++++未知选项")	
	end
	this.tipPanel.gameObject:SetActive(true)
	
end
function this.onSetDiamond(go,data,index)

end

function this.onSetGold(go,data,index)

end

--打开界面时初始化
function this:initView()


end



