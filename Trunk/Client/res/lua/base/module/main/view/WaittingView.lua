

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "base:mid/common/Mid_common_waitting_panel"

WaittingView=BaseView:new()
local this=WaittingView
this.viewName="WaittingView"

this._speed = 0
this._delaytime = 200
local onEndFunc = nil

this.reasonTable = {}

--设置面板特性
this:setViewAttribute(UIViewType.Loading_View, UIViewEnum.WaittingView, false)

--设置加载列表
this.loadOrders=
{
	"base:common/common_waitting_panel", 
}


function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self.main_mid.go:SetActive(true)	
end


function this:onShowHandler(msg)
	CSLoger.printLog("=================WaittingView完毕调用======================")
	self:startRotate()
	
	for _,v in pairs(this.reasonTable) do
		if v == true then
			return
		end
	end
	ViewManager.close(UIViewEnum.WaittingView)
end


function this:onClose()
	CSLoger.printLog("WaittingView:onClose===========>")
	onEndFunc = nil
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("rotate")
end


function this.showOrHide(isShow, reason)
	if isShow then
		this.reasonTable[reason] = true
		ViewManager.open(UIViewEnum.WaittingView)
	else
		this.reasonTable[reason] = nil
		for _,v in pairs(this.reasonTable) do
			if v == true then
				return
			end
		end
		ViewManager.close(UIViewEnum.WaittingView)
	end
end


function this.openView(showtime,onEndCall)
	onEndFunc = nil
	this.removeAllTimer()
	GlobalTimeManager.Instance.timerController:AddTimer("WaittingShow", this._delaytime, 1, this.onShowView) 
    if showtime then 
        if onEndCall then 
        	onEndFunc = onEndCall 
        end	        	
    	local showseconds = showtime +this._delaytime
		GlobalTimeManager.Instance.timerController:AddTimer("WaittingHide",this._delaytime + showseconds, 1, this.waittingEndFunc)	        	
    end	
end


function this:startRotate()
	GlobalTimeManager.Instance.timerController:AddTimer("rotate",-1, -1, self.changeRotateSpeed)
end


function this.changeRotateSpeed()  
	this._speed = this._speed + 7
	this.main_mid.image_1.gameObject.transform.localEulerAngles = Vector3(0, 0, -this._speed/2)
	this.main_mid.image_2.gameObject.transform.localEulerAngles = Vector3(0, 0, -this._speed)	
end


function this.onShowView()
	if WaittingView.isOpen then
       return
	end
	ViewManager.open(UIViewEnum.WaittingView)		
end


function this.hideView()
	onEndFunc = nil
    this.removeAllTimer()
	ViewManager.close(UIViewEnum.WaittingView)		
end


function this.waittingEndFunc()
	if onEndFunc then
	   CSLoger.printLog("WaittingView =======> onEndFunc call")
	   onEndFunc()
	end
	this.hideView()
end


function this.removeAllTimer()
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("WaittingShow")
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("WaittingHide")	
end