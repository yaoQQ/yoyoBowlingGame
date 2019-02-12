--签到界面
PlatformSignView = BaseView:new()
local this = PlatformSignView
this.viewName = "PlatformSignView"

--设置面板特性（界面层级、界面枚举、是否参与界面堆栈）
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Sign_View, false)

--设置加载列表
this.loadOrders = {
    "base:sign/platform_sign_panel"
}

--7天签到IconList
local m_weekIconList = {}
--月签到IconList
local m_monthIconList = {}

--当前月份
local m_curMonth = 1
--今天有没有签到
local m_is_sign_today = 0
--7天累计签到次数
local m_week_sign_num = 0
--月签到累计次数
local m_mon_sign_num = 0
--月签到奖励已经领取列表（天数列表)
local m_mon_sin_has_rcv_list = {}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    UITools.SetParentAndAlign(gameObject, self.container)

    --设置UI中间代码
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
	
    --添加UI事件监听
    self:addEvent()
	
	m_weekIconList[1] = self.main_mid.icon_day_1
	m_weekIconList[2] = self.main_mid.icon_day_2
	m_weekIconList[3] = self.main_mid.icon_day_3
	m_weekIconList[4] = self.main_mid.icon_day_4
	m_weekIconList[5] = self.main_mid.icon_day_5
	m_weekIconList[6] = self.main_mid.icon_day_6
	m_weekIconList[7] = self.main_mid.icon_day_7
	
	m_monthIconList[1] = self.main_mid.icon_month_1
	m_monthIconList[2] = self.main_mid.icon_month_2
	m_monthIconList[3] = self.main_mid.icon_month_3
	
	self.main_mid.text_month_1.Txt.text = TableBaseSigninmonth.data[1].Day.."天"
	self.main_mid.text_month_2.Txt.text = TableBaseSigninmonth.data[2].Day.."天"
	self.main_mid.text_month_3.Txt.text = TableBaseSigninmonth.data[3].Day.."天"
	
	local x1 = TableBaseSigninmonth.data[1].Day / TableBaseSigninmonth.data[3].Day * 760 - 380
	local x2 = TableBaseSigninmonth.data[2].Day / TableBaseSigninmonth.data[3].Day * 760 - 380
	self.main_mid.btn_month_1.transform.localPosition = Vector3(x1, 0, 0)
	self.main_mid.btn_month_2.transform.localPosition = Vector3(x2, 0, 0)
end

function this:addEvent()
    self.main_mid.btn_close:AddEventListener(UIEvent.PointerClick, this.onBtnClose)
	
	self.main_mid.icon_day_1:AddEventListener(UIEvent.PointerClick, this.onBtnSign1)
	self.main_mid.icon_day_2:AddEventListener(UIEvent.PointerClick, this.onBtnSign2)
	self.main_mid.icon_day_3:AddEventListener(UIEvent.PointerClick, this.onBtnSign3)
	self.main_mid.icon_day_4:AddEventListener(UIEvent.PointerClick, this.onBtnSign4)
	self.main_mid.icon_day_5:AddEventListener(UIEvent.PointerClick, this.onBtnSign5)
	self.main_mid.icon_day_6:AddEventListener(UIEvent.PointerClick, this.onBtnSign6)
	self.main_mid.icon_day_7:AddEventListener(UIEvent.PointerClick, this.onBtnSign7)
	
	self.main_mid.icon_month_1:AddEventListener(UIEvent.PointerClick, this.onBtnMonthSign1)
	self.main_mid.icon_month_2:AddEventListener(UIEvent.PointerClick, this.onBtnMonthSign2)
	self.main_mid.icon_month_3:AddEventListener(UIEvent.PointerClick, this.onBtnMonthSign3)
end

function this.onBtnClose()
    ViewManager.close(UIViewEnum.Platform_Sign_View)
end

function this.onBtnSign1()
    this.sign(1)
end
function this.onBtnSign2()
    this.sign(2)
end
function this.onBtnSign3()
	this.sign(3)
end
function this.onBtnSign4()
    this.sign(4)
end
function this.onBtnSign5()
    this.sign(5)
end
function this.onBtnSign6()
    this.sign(6)
end
function this.onBtnSign7()
	if IS_TEST_SERVER then
		if m_is_sign_today > 0 or 7 ~= m_week_sign_num + 1 then
			Alert.showVerifyMsg(nil, "是否要用测试包测试7天签到接红包？", "取消", nil, "确定", function()
				PlatformCatchPacketModule.openCatchPacket(TableBaseSigninday.data[7].money1, function(catchValue)
					--this.sign(7, catchValue)
				end)
			end)
			return
		end
		
		PlatformCatchPacketModule.openCatchPacket(TableBaseSigninday.data[7].money1, function(catchValue)
			this.sign(7, catchValue)
		end)
	else
		if m_is_sign_today > 0 or 7 ~= m_week_sign_num + 1 then
			return
		end
		
		PlatformCatchPacketModule.openCatchPacket(TableBaseSigninday.data[7].money1, function(catchValue)
			this.sign(7, catchValue)
		end)
	end
end

function this.onBtnMonthSign1()
	this.monthSign(1)
end
function this.onBtnMonthSign2()
	this.monthSign(2)
end
function this.onBtnMonthSign3()
	this.monthSign(3)
end

--override 打开UI回调
function this:onShowHandler(msg)
    --打开界面时添加UI通知监听
    self:addNotice()

    --打开界面时初始化，一般用于处理没有数据时的默认的界面显示
    self:initView()
end

--override 关闭UI回调
function this:onClose()
    --关闭界面时移除UI通知监听
    self:removeNotice()
end

function this:addNotice()
    NoticeManager.Instance:AddNoticeLister(NoticeType.Sign_Init_SignInfo, this.onInitSignInfo)
	NoticeManager.Instance:AddNoticeLister(NoticeType.Sign_Update_SignInfo, this.onUpdateSignInfo)
	NoticeManager.Instance:AddNoticeLister(NoticeType.Sign_Update_MonthInfo, this.onUpdateMonthInfo)
end

function this:removeNotice()
    NoticeManager.Instance:RemoveNoticeLister(NoticeType.Sign_Init_SignInfo, this.onInitSignInfo)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Sign_Update_SignInfo, this.onUpdateSignInfo)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Sign_Update_MonthInfo, this.onUpdateMonthInfo)
end

--打开界面时初始化
function this:initView()
    this.main_mid.main_panel.gameObject:SetActive(false)
	
	m_curMonth = TimeManager.getServerDateTime().Month
	
	m_is_sign_today = 0
	m_week_sign_num = 0
	m_mon_sign_num = 0
	m_mon_sin_has_rcv_list = {}
	
	--请求获取签到信息
	PlatformSignModule.sendReqGetSignInfo()
end

function this.onInitSignInfo(notice, data)
	--rsp:RspGetSignInfo
	local rsp = data:GetObj()
    printDebug("获取初始化签到信息:"..table.tostring(rsp))
	
	m_is_sign_today = rsp.is_sign_today
	m_week_sign_num = rsp.week_sign_num
	m_mon_sign_num = rsp.mon_sign_num
	m_mon_sin_has_rcv_list = rsp.mon_sin_has_rcv_list
	if m_mon_sin_has_rcv_list == nil then
		m_mon_sin_has_rcv_list = {}
	end
	
	this.main_mid.main_panel.gameObject:SetActive(true)
	this.updateWeekSign()
	this.updateMonthSign()
end

function this.onUpdateSignInfo(notice, data)
	--rsp:RspSign
	local rsp = data:GetObj()
    printDebug("获取更新签到信息:"..table.tostring(rsp))
	
	m_is_sign_today = 1
	m_week_sign_num = rsp.week_sign_num
	m_mon_sign_num = rsp.mon_sign_num
	
	this.updateWeekSign()
	this.updateMonthSign()
end

function this.onUpdateMonthInfo(notice, data)
	--rsp:RspRcvMonSignReward
	local rsp = data:GetObj()
    printDebug("获取更新月签到奖励信息:"..table.tostring(rsp))
	
	for i = 1, 3 do
		if rsp.day == TableBaseSigninmonth.data[i].Day then
			m_mon_sin_has_rcv_list[i] = 1
			break
		end
	end
	
	this.updateMonthSign()
end

function this.updateWeekSign()
	for i = 1, 7 do
		if i <= m_week_sign_num then
			m_weekIconList[i]:ChangeIcon(2)
			if i == 7 then
				this.main_mid.Text_7.gameObject:SetActive(false)
			end
		elseif m_is_sign_today == 0 and i == m_week_sign_num + 1 then
			m_weekIconList[i]:ChangeIcon(1)
			if i == 7 then
				this.main_mid.Text_7.gameObject:SetActive(false)
			end
		else
			m_weekIconList[i]:ChangeIcon(0)
			if i == 7 then
				this.main_mid.Text_7.gameObject:SetActive(true)
			end
		end
	end
end

function this.updateMonthSign()
	if m_mon_sign_num >= TableBaseSigninmonth.data[3].Day then
		this.main_mid.Image_progress_value.Img.fillAmount = 1
	else
		this.main_mid.Image_progress_value.Img.fillAmount = m_mon_sign_num / TableBaseSigninmonth.data[3].Day
	end
	
	local noGetDay = 0
	for i = 1, 3 do
		if m_mon_sign_num < TableBaseSigninmonth.data[i].Day then
			m_monthIconList[i]:ChangeIcon(0)
			if noGetDay == 0 then
				noGetDay = TableBaseSigninmonth.data[i].Day
			end
		elseif m_mon_sin_has_rcv_list[i] ~= 1 then
			m_monthIconList[i]:ChangeIcon(1)
			if noGetDay == 0 then
				noGetDay = TableBaseSigninmonth.data[i].Day
			end
		else
			m_monthIconList[i]:ChangeIcon(2)
		end
	end
	if noGetDay == 0 then
		noGetDay = TableBaseSigninmonth.data[3].Day
	end
	this.main_mid.text_month.Txt.text = "<color=#dd3e45>"..m_curMonth.."月</color>签到满<color=#dd3e45>"..noGetDay.."天</color>可获得额外奖励，当前已累计：<color=#dd3e45>"..m_mon_sign_num.."天</color>"
end

function this.sign(index, catchValue)
	if m_is_sign_today > 0 then
		return
	end
	if index ~= m_week_sign_num + 1 then
		return
	end
	
	--签到
	PlatformSignModule.sendReqSign(catchValue)
end

function this.monthSign(index)
	if m_mon_sign_num < TableBaseSigninmonth.data[index].Day then
		return
	end
	if m_mon_sin_has_rcv_list[index] == 1 then
		return
	end
	
	--领取月签到奖励
	PlatformSignModule.sendReqRcvMonSignReward(TableBaseSigninmonth.data[index].Day)
end