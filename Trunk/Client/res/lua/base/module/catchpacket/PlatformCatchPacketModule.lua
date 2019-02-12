--接红包模块
PlatformCatchPacketModule = BaseModule:new()
local this = PlatformCatchPacketModule
this.moduleName = "PlatformCatchPacket"

------------------------------外部调用接口------------------------------
local m_maxValue = 0
local m_finishCallback = nil
function this.openCatchPacket(maxValue, finishCallback)
	m_maxValue = maxValue
	m_finishCallback = finishCallback
	ViewManager.open(UIViewEnum.CatchPacket_GameView, maxValue)
end

------------------------------注册由服务器发来的协议------------------------------
function this:initRegisterNet()
	this.netFuncList = {}
end
function this.onNetMsgLister(protoID,protoBytes)
	local nfSwitch = this.netFuncList[protoID] 
	if nfSwitch then  
		nfSwitch(protoBytes) 
	else 
		this:withoutRegistNotice(protoID)
	end
end

------------------------------注册通知------------------------------
function this:getRegisterNotificationList()
	if self.notificationList == nil then
		self.notificationList = {} 
		self.switch = {}
		--全局的通知在这里注册
		self:AddNotifictionLister(NoticeType.CatchPacket_End, this.onNoticeCatchPacketEnd)
	end
	return self.notificationList
end

function this.onNoticeCatchPacketEnd(noticeType, notice)
	local data = notice:GetObj()
	printDebug("接红包结果："..data)
	if data < 0 then
		data = 0
	elseif data > m_maxValue then
		data = m_maxValue
	end
	if m_finishCallback ~= nil then
		m_finishCallback(data)
		m_finishCallback = nil
		m_maxValue = 0
	end
end