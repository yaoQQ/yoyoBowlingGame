

require "base:enum/UIViewEnum"
require "base:enum/proto/ProtoEnumPlatform"
require "base:enum/PlatformGuessBetType"
require "base:mid/guess/Mid_platform_guess_roomlist_panel"

PlatformGuessBetRoomListView =BaseView:new()
local this=PlatformGuessBetRoomListView
this.viewName="PlatformGuessBetRoomListView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_GuessBet_RoomListView, true)

--设置加载列表
this.loadOrders=
{
	"base:guess/platform_guess_roomlist_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

function this:addEvent()
	self.main_mid.back_image:AddEventListener(UIEvent.PointerClick, function ()
		ViewManager.close(UIViewEnum.Platform_GuessBet_RoomListView)
	end)
end


function this:onShowHandler(msg)
	self:addNotice()
	self:updateGuessList()
end


function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(PlatformGuessBetType.Platform_Update_GuessRoomList, this.updateGuessList)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(PlatformGuessBetType.Platform_Update_GuessRoomList, this.updateGuessList)
end

--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end




function this:updateGuessList()
	local roomListModel = PlatformGuessBetProxy.getRoomListInfo()
	self.main_mid.bets_scroll_list:SetCellData(roomListModel.room_info, self.onUpdateGuessItems, false)
end

--更新单个数据
function this.onUpdateGuessItems(go, data, index)
	if table.empty(data) then
		return
	end
	
	local room_info = data
	local detail_info = PlatformGuessBetProxy.getDetailInfoByRoomId(room_info.room_id)
	
	--printDebug("更新单个数据, data = "..table.tostring(data))
	local item = this.main_mid.encounterItemArr[index + 1]
	if table.empty(detail_info.team_list) == false then
		downloadFromOtherServer(detail_info.team_list[1].logo, item.home_team_image)
		item.home_team_name_text.text = detail_info.team_list[1].name_zh

		downloadFromOtherServer(detail_info.team_list[2].logo, item.guest_team_image)
		item.guest_team_name_text.text = detail_info.team_list[2].name_zh
	end
	item.belong_text.text = detail_info.match_event.name_zh
	local dataTime = TimeManager.getDateTimeByUnixTime(room_info.end_time)
	item.match_time_text.text = string.format("结束时间:%02s-%02s %02s:%02s",dataTime.Month, dataTime.Day, dataTime.TimeOfDay.Hours, dataTime.TimeOfDay.Minutes)
	item.bg_icon:AddEventListener(UIEvent.PointerClick, function ()
		--body, 请求进入房间
		local roomListModel = PlatformGuessBetProxy.getRoomListInfo()
		local roomId = roomListModel.room_info[index].room_id
		PlatformGuessBetModule.sendReqGuessChatChannelJoin(roomId)
	end)
end

