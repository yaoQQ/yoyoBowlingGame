

require "base:enum/UIViewEnum"
require "base:mid/guess/Mid_platform_guess_betlist_panel"

--竞猜列表界面
PlatformGuessBetListView = BaseView:new()
local this = PlatformGuessBetListView
this.viewName = "PlatformGuessBetListView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_GuessBet_BetListView, true)

--设置加载列表
this.loadOrders =
{
	"base:guess/platform_guess_betlist_panel",
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
	
	UITools.SetParentAndAlign(gameObject, self.container)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	
	self:activeGameObject(self.main_mid.rule_panel.gameObject, false)
	self:addEvent()
end

function this:addEvent()
	self.main_mid.back_image:AddEventListener(UIEvent.PointerClick, function()
		ViewManager.close(UIViewEnum.Platform_GuessBet_BetListView)
	end)
	self.main_mid.rule_back_image:AddEventListener(UIEvent.PointerClick, function()
		self:activeGameObject(self.main_mid.rule_panel.gameObject, false)
	end)
end



function this:activeGameObject(go, state)
	if go == nil then
		return
	end
	if state then
	else
		PlatformGuessBetProxy.resetCurDynamicBetInfo()
	end
	go:SetActive(state)
end

--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()

	self:updateTopDetail()
	self:addRuleEvent()
	this.updateDynamicListView()
end

--override 关闭UI回调
function this:onClose()
	self:removeNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateDynamicListView, this.updateDynamicListView)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateDynamicListView, this.updateDynamicListView)
end

function this:setDynamicQuestionView(viewItem, modelItem)

	self:setRateText(viewItem.odds_text_1, modelItem.option_info[1], viewItem.choose_state_icon_1)
	self:setRateText(viewItem.odds_text_2, modelItem.option_info[2], viewItem.choose_state_icon_2)
	self:setRateText(viewItem.odds_text_3, modelItem.option_info[3], viewItem.choose_state_icon_3)
end

function this:setRateText(textWidget, data, root)
	if table.empty(data) then
		root.gameObject:SetActive(false)
		return
	end
	root.gameObject:SetActive(true)
	textWidget.text = string.format("%s \n (%s)", data.option_des, data.rate / 100)
end


function this:updateTopDetail()
	local curRoomId = PlatformGuessBetProxy.getCurRoomId()
	local roomInfo = PlatformGuessBetProxy.getRoomInfoById(curRoomId)
	local data = PlatformGuessBetProxy.getDetailInfoByRoomId(curRoomId)
	if table.empty(data) then
		Loger.PrintError("错误, 找不到房间ID对应的比赛信息, roomId = "..curRoomId)
		return
	end
	if table.empty(data.team_list) == false then
		downloadFromOtherServer(data.team_list[1].logo, self.main_mid.home_team_image)
		self.main_mid.home_team_name_text.text = data.team_list[1].name_zh

		downloadFromOtherServer(data.team_list[2].logo, self.main_mid.guest_team_image)
		self.main_mid.guest_team_name_text.text = data.team_list[2].name_zh
	end
	self.main_mid.belong_text.text = data.match_event.name_zh
	--local dataTime = TimeManager.getDateTimeByUnixTime(roomInfo.end_time)
	--local timeFormat = string.format("结束时间:%02s-%02s %02s:%02s",dataTime.Month, dataTime.Day, dataTime.TimeOfDay.Hours, dataTime.TimeOfDay.Minutes)
	--self.main_mid.match_explain_text.text = timeFormat
	self.main_mid.match_explain_text.gameObject:SetActive(false)
end

function this:updateBetDetail()
	local model = PlatformGuessBetProxy.getBetListInfo()
	
	self.main_mid.bet_detail_scroll_list:SetCellData(model, self.onUpdateBetList, false)
end

function this:addRuleEvent()
	self.main_mid.bet_detail_scroll_list:SetCellData({1,2,3,4}, function (go, data, index)
		local item = this.main_mid.betDetailCellArr[index + 1]
		item.bet_rule_btn:AddEventListener(UIEvent.PointerClick,function ()
			this:activeGameObject(this.main_mid.rule_panel.gameObject, true)
		end)
	end, false)

end

function this.onUpdateBetList(go, data, index)
	local item = this.main_mid.betDetailCellArr[index + 1]

	local guess_info = data.guess_info
	local question_status = data.question_status
	
	item.bet_content_text.text = guess_info.des
	local dataTime = TimeManager.getDateTimeByUnixTime(guess_info.end_time)
	item.deadline_text.text = string.format("截止时间:%02s-%02s %02s:%02s",dataTime.Month, dataTime.Day, dataTime.TimeOfDay.Hours, dataTime.TimeOfDay.Minutes)
	this:setDynamicQuestionView(item, guess_info)
	item.choose_state_icon_1:AddEventListener(UIEvent.PointerClick, function ()
		PlatformGuessBetProxy.setCurDynamicBetInfo(guess_info.room_id, guess_info.question_id, 1)
		ViewManager.open(UIViewEnum.Platform_GuessBet_BetView)
	end)
	item.choose_state_icon_2:AddEventListener(UIEvent.PointerClick, function ()
		PlatformGuessBetProxy.setCurDynamicBetInfo(guess_info.room_id, guess_info.question_id, 2)
		ViewManager.open(UIViewEnum.Platform_GuessBet_BetView)
	end)
	item.choose_state_icon_3:AddEventListener(UIEvent.PointerClick, function ()
		PlatformGuessBetProxy.setCurDynamicBetInfo(guess_info.room_id, guess_info.question_id, 3)
		ViewManager.open(UIViewEnum.Platform_GuessBet_BetView)
	end)
	
	item.bet_state_icon:ChangeIcon(question_status.question_status)
	item.choose_state_icon_1:ChangeIcon(question_status.question_status)
	item.choose_state_icon_2:ChangeIcon(question_status.question_status)
	item.choose_state_icon_3:ChangeIcon(question_status.question_status)
	
	if question_status.answer_detail ~= nil then
		local isChoose1 = false
		local isChoose2 = false
		local isChoose3 = false
		for k,v in ipairs(question_status.answer_detail) do
			if v.choose == 1 then
				isChoose1 = true
			elseif v.choose == 2 then
				isChoose2 = true
			elseif v.choose == 3 then
				isChoose3 = true
			end
		end
		if isChoose1 then
			item.choose_flag_icon_1:ChangeIcon(1)
		else
			item.choose_flag_icon_1:ChangeIcon(0)
		end
		if isChoose2 then
			item.choose_flag_icon_2:ChangeIcon(1)
		else
			item.choose_flag_icon_2:ChangeIcon(0)
		end
		if isChoose3 then
			item.choose_flag_icon_3:ChangeIcon(1)
		else
			item.choose_flag_icon_3:ChangeIcon(0)
		end
	else
		item.choose_flag_icon_1:ChangeIcon(0)
		item.choose_flag_icon_2:ChangeIcon(0)
		item.choose_flag_icon_3:ChangeIcon(0)
	end
end

function this.updateDynamicListView()
	this:updateBetDetail()
end
