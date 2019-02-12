require "base:enum/NoticeType"

LoadModule=BaseModule:new()
local this=LoadModule

this.moduleName="Load"


function this:getRegisterNotificationList()
	if self.notificationList == nil then
		self.notificationList = {}
		self.switch={}
		self:AddNotifictionLister(PlatformGlobalNoticeType.Loading_Annulus_Show,this.onShowAnnulusView)
		self:AddNotifictionLister(PlatformGlobalNoticeType.Loading_Annulus_Hide,this.onHideAnnulusView)
	end
    return self.notificationList
end


function this:onShowAnnulusView(notice)
	ShowWaitMask(true)
end

function this:onHideAnnulusView(notice)
	ShowWaitMask(false)
end