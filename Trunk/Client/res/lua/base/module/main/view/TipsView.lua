

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"

require "base:mid/common/Mid_common_tip_panel"

TipsView = BaseView:new()
local this=TipsView
this.viewName="TipsView"


--设置面板特性
this:setViewAttribute(UIViewType.Pop_view,UIViewEnum.TipsView, false)


--设置加载列表
this.loadOrders=
{
	"base:common/common_tip_panel", 
}

this.msgList = nil
this.num = 1

function this:onLoadUIEnd(uiName,gameObject)
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	UITools.SetParentAndAlign(gameObject, self.container)	
	self.main_mid.go:SetActive(true)		
	self:resetView()
end


function this:onShowHandler(msg)
	printDebug("TipsView ============> onShowHandler() ")
	self.main_mid.go:SetActive(true)	
	self:openTipsView()
end

function this:resetView()
	if not TipsView.isInit then
		return
	end	
	self.main_mid.go:SetActive(false)
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("TipCountDown") 
	self.main_mid.tip_text.text = ""
	self.msgList = nil
	self.num = 1
	
end

function this:showTip(msg)
	self.msgList = {}
	table.insert(self.msgList, msg.."." )
	table.insert(self.msgList, msg..".." )
	table.insert(self.msgList, msg.."..." )
	self.num = 1
	if TipsView.isInit then
		self:openTipsView()
	else
    	ViewManager.open(UIViewEnum.TipsView,msg)
	end
end	


function this:openTipsView()
	if self.msgList == nil then
		self.main_mid.go:SetActive(false)
		return
	else
		self.main_mid.go:SetActive(true)
	end
	
	self.main_mid.tip_text.text = self.msgList[1]
	if #self.msgList > 1 then
		GlobalTimeManager.Instance.timerController:RemoveTimerByKey("TipCountDown") 
		GlobalTimeManager.Instance.timerController:AddTimer("TipCountDown",500,-1, this.setMsgContent)
	end
  
end

function this.setMsgContent()
	if this.msgList ~= nil then
		this.num = this.num + 1
		if this.num > #this.msgList then
			this.num = 1
		end
		this.main_mid.tip_text.text = this.msgList[this.num]
	end
end

function this:closeTipVew()
	self:resetView()
end