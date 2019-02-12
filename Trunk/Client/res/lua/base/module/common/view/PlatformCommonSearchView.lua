

require "base:enum/UIViewEnum"
require "base:mid/common/Mid_platform_common_search_panel"
require "base:enum/PlatformFriendType"

PlatformCommonSearchView = BaseView:new()
local this = PlatformCommonSearchView
this.viewName = "PlatformCommonSearchView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Common_Search_View, false)

--设置加载列表
this.loadOrders=
{
	"base:common/platform_common_search_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

this.myConfirmFun = nil
function this:onShowHandler(msg)
	printDebug("=====================Platform_Common_Search_View调用完毕======================")

	if msg.isInstanceSearch then
		this.updateInstanceSearchView()
	else
		this.updateNotInstanceSearchView()
	end

	this.myConfirmFun = msg.myFun


	local go = self:getViewGO()
	if go == nil then return end
	go.transform:SetAsLastSibling()
end


function this:onClose()	
	this.main_mid.search_InputField.text = ""
	this.myConfirmFun = nil
end

function this:addEvent()

	self.main_mid.cancel_Button:AddEventListener(UIEvent.PointerClick,function ()

		ViewManager.close(UIViewEnum.Platform_Common_Search_View)
		NoticeManager.Instance:Dispatch(PlatformFriendType.Platform_Friend_Search_Close)

	end)

	self.main_mid.confirm_Button:AddEventListener(UIEvent.PointerClick,function ()
		--向服务器发送搜索的内容
		this.myConfirmFun(this.main_mid.search_InputField.text)
	end)
	self.main_mid.clear_Image:AddEventListener(UIEvent.PointerClick,self.clearInputTxt)

end

function this.updateNotInstanceSearchView()
	--printDebug("++++++++++++++++++++进入了updateNotInstanceSearchView")
	this.main_mid.mask_Image.gameObject:SetActive(true)
	this.main_mid.search_InputField.inputField:ActivateInputField()
	
	this.main_mid.search_InputField:OnValueChanged(function(obj)
		printDebug("++++++++++++++OnValueChanged1")

		if this.main_mid.search_InputField.text == "" or this.main_mid.search_InputField.text == nil then
			this.main_mid.confirm_Button.gameObject:SetActive(false)
			this.main_mid.cancel_Button.gameObject:SetActive(true)
		else
			this.main_mid.confirm_Button.gameObject:SetActive(true)
			this.main_mid.cancel_Button.gameObject:SetActive(false)
		end
	end)
end

----------------------------------------对外---------------------
function this:clearInputTxt()
	-- this.myConfirmFun = nil
	if this.main_mid ~= nil then
		this.main_mid.search_InputField.text = ""
	end
end