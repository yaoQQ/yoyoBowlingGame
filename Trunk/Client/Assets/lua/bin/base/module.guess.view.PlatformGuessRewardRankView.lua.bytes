

require "base:enum/UIViewEnum"
require "base:mid/guess/Mid_platform_guess_reward_rank_panel"

PlatformGuessRewardRankView =BaseView:new()
local this = PlatformGuessRewardRankView
this.viewName = "PlatformGuessRewardRankView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_GuessBet_RewardRankView, true)

--设置加载列表
this.loadOrders=
{
	"base:guess/platform_guess_reward_rank_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

function this:addEvent()
	self.main_mid.back_image:AddEventListener(UIEvent.PointerClick,function ()
		ViewManager.close(UIViewEnum.Platform_GuessBet_RewardRankView)
	end)
end

--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
	self:updateTopPanelInfo()
	self:updateRewardRankDetail()
end

--override 关闭UI回调
function this:onClose()
	self:removeNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.Socket_Error, this.onSocketError)
	NoticeManager.Instance:AddNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateRewardView, this.updateRewardView)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Socket_Error, this.onSocketError)
	NoticeManager.Instance:RemoveNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateRewardView, this.updateRewardView)
end

function this:setReward(rewardImage, countText, itemUrl, itemCount)
	rewardImage.gameObject:SetActive(true)
	downloadImage(itemUrl, rewardImage)
	countText.text = tostring(itemCount)
end

function this:updateRewardRankDetail()
	local model = PlatformGuessBetProxy.getRewardInfo()
	if table.empty(model.reward_info) == false then
		self.main_mid.rank_scroll_list:SetCellData(model.reward_info, self.onUpdateRankList, false)
	end
end

function this:updateTopPanelInfo()
	local model = PlatformGuessBetProxy.getRewardInfo()
	if table.empty(model) then
		return
	end
	
	local currGlobalBaseData = PlatformUserProxy:GetInstance():getUserInfo()
	downloadUserHead(currGlobalBaseData.head_url, self.main_mid.head_image)
	
	local rankFormat = ""
	if model.my_rank == 0 then
		rankFormat = "未上榜"
	else
		rankFormat = string.format("第%s名", model.my_rank)
	end
	self.main_mid.rank_text.text = rankFormat
	self.main_mid.reward_click_icon:ChangeIcon(model.reward_flag)
	if model.reward_flag == 1 then
		self.main_mid.reward_click_icon:AddEventListener(UIEvent.PointerClick,function ()
			local room_id = PlatformGuessBetProxy.getCurRoomId()
			PlatformGuessBetModule.sendReqGetRoomRankReward(room_id)
		end)
	end
end

function this.onUpdateRankList(go, data, index)
	--data数据的类型为common.MsgViewRoomRankRewardInfo
	local item = this.main_mid.rankItemArr[index + 1]
	item.rank_text.text = data.rank
	if data.rank <= 3 then
		item.medal_icon:ChangeIcon(data.rank - 1)
		item.medal_icon.gameObject:SetActive(true)
		item.rank_text.gameObject:SetActive(false)
	else
		item.medal_icon.gameObject:SetActive(false)
		item.rank_text.gameObject:SetActive(true)
	end
	this:setReward(item.reward_image_1, item.count_text_1, data.item_url, data.loot_item.item_count)
	
	if data.player_base_info ~= nil then
		downloadUserHead(data.player_base_info.head_url, item.head_image)
		item.name_text.text = data.player_base_info.nick_name
	end
end

function this.onSocketError()
	--断线时关闭聊天房间
	ViewManager.close(UIViewEnum.Platform_GuessBet_RewardRankView)
	ViewManager.close(UIViewEnum.Platform_GuessBet_ChatRoomView)
end

function this.updateRewardView()
	this:onUpdateRankList()
end