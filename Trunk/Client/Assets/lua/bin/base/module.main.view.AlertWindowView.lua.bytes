require "base:enum/AlertWindowType"
require "base:mid/common/Mid_common_popup_window_panel"

AlertWindowView = BaseView:new()
local this = AlertWindowView
this.viewName = "AlertWindowView"


--设置面板特性
this:setViewAttribute(UIViewType.Alert_box, UIViewEnum.AlertWindow, false)



--设置加载列表
this.loadOrders =
{
	"base:common/common_popup_window_panel", 
}

this.confirmFun = nil
this.cancelFun = nil
this.isAllowClose = false

--override 加载UI完成回调
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:setEaseContainer(self.main_mid.bg_image.gameObject)
	self:addEvent()		
end

--override 打开UI回调
function this:onShowHandler(msg)
	--printDebug("PopupWindowView ============> onShowHandler() ")
	self:openMsgView(msg)
end

--override 关闭UI回调
function this:onClose()
   self.confirmFun = nil
   self.cancelFun = nil
end


function this:addEvent()
	self.main_mid.btn_1:AddEventListener(UIEvent.PointerClick, this.onConfirmFun)
    self.main_mid.btn_2:AddEventListener(UIEvent.PointerClick, this.onCancelFun)
	self.main_mid.mask:AddEventListener(UIEvent.PointerClick, this.onMaskClose)
end

function this:showAlertMsg(title,msg, btnName, onBtnfunc)
	local alertWindowInfo = {}
    alertWindowInfo.msgType = AlertWindowType.Alert
    alertWindowInfo.title = title
	alertWindowInfo.msg = msg
	alertWindowInfo.btnName = btnName
	alertWindowInfo.onBtnfunc = onBtnfunc
	ViewManager.open(UIViewEnum.AlertWindow, alertWindowInfo)
end

function this:showAlertVerifyWindow(title, info, btnName, onBtnfunc, isAllowClose)
	local msg = {}
	msg.msgType = AlertWindowType.AlertVerify
	msg.title = title
	msg.info = info
	msg.btnName = btnName
	msg.onBtnfunc = onBtnfunc
	msg.isAllowClose = isAllowClose
	ViewManager.open(UIViewEnum.AlertWindow, msg)
end

function this:showVerifyMsg(title,msg, btnName1, onBtnfunc1, btnName2, onBtnfunc2, isAllowClose)
	local alertWindowInfo = {}
    alertWindowInfo.msgType = AlertWindowType.Verify
    alertWindowInfo.title = title
	alertWindowInfo.msg = msg
	alertWindowInfo.btnName1 = btnName1
	alertWindowInfo.onBtnfunc1 = onBtnfunc1
	alertWindowInfo.btnName2 = btnName2
	alertWindowInfo.onBtnfunc2 = onBtnfunc2
	alertWindowInfo.isAllowClose = isAllowClose
	ViewManager.open(UIViewEnum.AlertWindow, alertWindowInfo)
end	

function this:openMsgView(msg)
	local switch = {
	    [AlertWindowType.Alert] = function()
		   self:updateAlertWindow(msg)
	    end,  	    
	    [AlertWindowType.Verify] = function()	   
		   self:updateVerifyWindow(msg)
	    end,
		[AlertWindowType.AlertVerify] = function()
			self:updateAlertVerifyWindow(msg)
		end,
	} 
  
	local fSwitch = switch[msg.msgType] 
	if fSwitch then 
		fSwitch() 	   
	else 
		CSLoger.debug(Color.Yellow, msgType.." not found !")
	end
end

function this:updateAlertWindow(msg)
    self.main_mid.title_text.text = msg.title
	self.main_mid.info_text.text = msg.msg
	this:bgBestView(msg.title)
	self.main_mid.btn_1.Txt.text = msg.btnName
	self.main_mid.btn_1.gameObject:SetActive(true)
	self.main_mid.btn_2.gameObject:SetActive(false)
	self.confirmFun = msg.onBtnfunc
	self.isAllowClose = false
	self.main_mid.go:SetActive(true)
end

function this:updateVerifyWindow(msg)
	self.main_mid.title_text.text =msg.title
	self.main_mid.info_text.text = msg.msg
	this:bgBestView(msg.title)
	self.main_mid.btn_1.Txt.text = msg.btnName1
	self.main_mid.btn_1.gameObject:SetActive(true)
	self.main_mid.btn_2.Txt.text = msg.btnName2
	self.main_mid.btn_2.gameObject:SetActive(true)
	self.confirmFun = msg.onBtnfunc1
	self.cancelFun = msg.onBtnfunc2
	self.isAllowClose = msg.isAllowClose
	self.main_mid.go:SetActive(true)
end

function this:updateAlertVerifyWindow(msg)
	self.main_mid.title_text.text = msg.title
	self.main_mid.info_text.text = msg.info
	this:bgBestView(msg.title)
	self.main_mid.btn_2.Txt.text = msg.btnName
	self.main_mid.btn_1.gameObject:SetActive(false)
	self.main_mid.btn_2.gameObject:SetActive(true)
	self.cancelFun = msg.onBtnfunc
	self.isAllowClose = msg.isAllowClose
	self.main_mid.go:SetActive(true)
end

function this:bgBestView(isTitleNil)
	tempImageRect = self.main_mid.bg_image.transform:GetComponent(typeof(RectTransform))
	local tempwidth =710
	local tempHeight = self.main_mid.info_text.Txt.preferredHeight+430
	if isTitleNil==nil then
		tempHeight=tempHeight-88
	end
	tempImageRect.sizeDelta = Vector2(tempwidth,tempHeight)
end


function this.onConfirmFun(eventData)
	if this.confirmFun then
		GlobalTimeManager.Instance.timerController:AddTimer("AlertWindowView", 100, 1, this.confirmFun)
	end
	ViewManager.close(UIViewEnum.AlertWindow)
end


function this.onCancelFun(eventData)
	if this.cancelFun then
		GlobalTimeManager.Instance.timerController:AddTimer("AlertWindowView", 100, 1, this.cancelFun)
	end
	ViewManager.close(UIViewEnum.AlertWindow)
end

function this.onMaskClose(eventData)
	print("尝试关闭警告窗口")
	if this.isAllowClose then
		ViewManager.close(UIViewEnum.AlertWindow)
	end
end