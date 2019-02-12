
require "base:enum/UIViewEnum"
--require "base:mid/redbag/Mid_platform_redbag_withdraw_prompt_panel"
require "base:module/redbag/data/PlatformNewRedBagProxy"

PlatformRedBagWithDrawPromptView= BaseView:new()
local this = PlatformRedBagWithDrawPromptView
this.viewName = "PlatformRedBagWithDrawPromptView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_RedBag_WithDraw_Prompt_View,false)

--设置加载列表
this.loadOrders=
{
	"base:redbag/platform_redbag_withdraw_prompt_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end
this.withDrawText= nil
function this:onShowHandler(msg)
	printDebug("=====================PlatformRedBagWithDrawPromptView调用完毕======================")

	local go = self:getViewGO()
	if go == nil then return end
	go.transform:SetAsLastSibling()
    self:addNotice()
    self:updateRedBagWithDrawPromptPanel()
end

function this:onClose()
    self:removeNotice()
end

function this:addNotice()
end
    

 function this:removeNotice()
 
 end

 function this:updateRedBagWithDrawPromptPanel()

    local time = PlayerPrefs.GetString("TIME_NUM","")
    local num = PlayerPrefs.GetString("COUNt_NUM","")
    printDebug("aaaaaaaaaaaaaaaaaaaaaaaaa"..num)

        if os.date("%Y/%m/%d %H:%M")==time then
            Alert.showAlertMsg(nil,"提现成功","确定")
            PlayerPrefs.SetString("COUNt_NUM",num+1)
            PlayerPrefs.SetString("TIME_NUM",os.date("%Y/%m/%d"))
            printDebug("aaaaaaaaaaaaaaaa协议这里为"..tonumber(PlayerPrefs.GetString("COUNt_NUM","")).."次")
        else
            PlayerPrefs.SetString("COUNt_NUM",0)
        end

end

---------------------------------------------------------------------------
function this:addEvent()
    self.main_mid.mask_Image:AddEventListener(UIEvent.PointerClick,self.geMyRedBag)

end
--------------------------------------------------------------------------
--点击事件
function this:geMyRedBag()
     ViewManager.close(UIViewEnum.Platform_RedBag_WithDraw_Prompt_View)
     
end
