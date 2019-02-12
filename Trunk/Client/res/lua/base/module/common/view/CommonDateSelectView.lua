

require "base:enum/UIViewEnum"
require "base:mid/common/Mid_platform_common_date_select_panel"
require "base:enum/PlatformFriendType"

CommonDateSelectView = BaseView:new()
local this = CommonDateSelectView
this.viewName = "CommonDateSelectView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Common_Date_Select_View, false)

--设置加载列表
this.loadOrders=
{
	"base:common/platform_common_date_select_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

this.changeType = 0
function this:onShowHandler(msg)
	printDebug("=====================Common_Date_Select_View调用完毕======================")
	
	--this:updateDateSelect(msg)
	this:addNotice()
end


function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_UserBaseInfo, this.onChangeSuccess)
end


function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_UserBaseInfo, this.onChangeSuccess)
end



function this.onChangeSuccess()
	ViewManager.close(UIViewEnum.Common_Date_Select_View)
end


--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end

function this:addEvent()
	local function closeView()
		ViewManager.close(UIViewEnum.Common_Date_Select_View)
	end
	self.main_mid.mask_image:AddEventListener(UIEvent.PointerClick,function ()
		closeView()
	end)
	self.main_mid.cancel_btn:AddEventListener(UIEvent.PointerClick,function ()
		closeView()
	end)

	self.main_mid.confirm_btn:AddEventListener(UIEvent.PointerClick,function ()
		closeView()
		local y = self.main_mid.year_picker
		local m = self.main_mid.month_picker
		local d = self.main_mid.day_picker
		local birthday = os.time({year = y:GetCurData(), month = m:GetCurData(), day = d:GetCurData(), hour =0, min =0, sec = 0})
		--返回birthday参数作为形参并且调用传过来的函数
		if this.confirmFunc ~= nil then
			--printDebug("++++++++++++++++++birthday confirmFunc2:"..tostring(birthday))
			this.confirmFunc(birthday)
		end

	end)
end

this.confirmFunc = nil
function this:showDateView(info)
	print("info = "..table.tostring(info))
	local yearList = {}
	local monthList = {}
	local dayList = {}
	local y = self.main_mid.year_picker
	local m = self.main_mid.month_picker
	local d = self.main_mid.day_picker
	for i = info.minYear, info.maxYear do
		table.insert(yearList, i)
	end
	for i = 1, 12 do
		table.insert(monthList, i)
	end
	for i = 1, 30 do
		table.insert(dayList, i)
	end
	local showYearIndex = info.defaultYear - info.minYear
	if showYearIndex < 0 then
		printError("错误, 显示的默认年数小于最小年")
		showYearIndex = 0
	end
	y:SetScrollPageData(yearList, showYearIndex, function (index)
		return string.format("%s年",  yearList[1] + index)
	end)
	m:SetScrollPageData(monthList, info.defaultMonth - 1, function (index)
		return string.format("%s月",  monthList[1] + index)
	end)
	d:SetScrollPageData(dayList, info.defaultDay - 1,function (index)
		return string.format("%s日",  dayList[1] + index)
	end)
	local function dayCountChangedHandler(index)
		local preCount = #dayList
		local count = DateTime.DaysInMonth(y:GetCurData(), m:GetCurData())
		if preCount ~= count then
			dayList = {}
			for i = 1, count do
				table.insert(dayList, i)
			end
			d:ChangeCount(dayList, count)
		end
	end
	y.onEndSelect = dayCountChangedHandler
	m.onEndSelect = dayCountChangedHandler
	this.confirmFunc = info.confirmFunc
end