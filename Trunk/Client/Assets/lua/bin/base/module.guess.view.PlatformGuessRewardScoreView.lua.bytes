

require "base:enum/UIViewEnum"
require "base:mid/guess/Mid_platform_guess_reward_score_panel"

PlatformGuessRewardScoreView =BaseView:new()
local this = PlatformGuessRewardScoreView
this.viewName = "PlatformGuessRewardScoreView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_GuessBet_RewardScoreView, true)

--设置加载列表
this.loadOrders=
{
	"base:guess/platform_guess_reward_score_panel",
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
		ViewManager.close(UIViewEnum.Platform_GuessBet_RewardScoreView)
	end)
end

--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
	
	local currGlobalBaseData = PlatformUserProxy:GetInstance():getUserInfo()
	downloadUserHead(currGlobalBaseData.head_url, self.main_mid.head_image)
	
	self:updateRewardScoreDetail()
	this.updateScoreView()
end

--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end


function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.Socket_Error, this.onSocketError)
	NoticeManager.Instance:AddNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateRoomScore, this.updateScoreView)
	NoticeManager.Instance:AddNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateRewardView, this.updateRewardView)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Socket_Error, this.onSocketError)
	NoticeManager.Instance:RemoveNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateRoomScore, this.updateScoreView)
	NoticeManager.Instance:RemoveNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateRewardView, this.updateRewardView)
end

function this:setReward(rewardImage, countText, itemUrl, itemCount)
	rewardImage.gameObject:SetActive(true)
	downloadImage(itemUrl, rewardImage)
	countText.text = tostring(itemCount)
end

function this:updateRewardScoreDetail()
	local model = PlatformGuessBetProxy.getRewardInfo()
	if table.empty(model)then
		self.main_mid.exchange_scroll_list.gameObject:SetActive(false)
	end
	if table.empty(model.reward_info) == false then
		self.main_mid.exchange_scroll_list.gameObject:SetActive(true)
		self.main_mid.exchange_scroll_list:SetCellData(model.reward_info, self.onUpdateRankList, false)
	end

end

function this.onUpdateRankList(go, data, index)
	--data数据的类型为common.MsgViewRoomRankRewardInfo
	local item = this.main_mid.exchangeItemArr[index + 1]
	item.need_text.text = data.score
	--item.progress_text.text = string.format("%s/%s", data.have_changed, data.max_change)
	item.progress_text.gameObject:SetActive(false)
	this:setReward(item.reward_image_1, item.count_text_1, data.item_url, data.loot_item.item_count)
	local isCanExchange = false
	if data.have_changed < data.max_change then
		isCanExchange = true
	else
		isCanExchange = false
	end
	if isCanExchange  then
		item.exchange_icon:ChangeIcon(0)
	else
		item.exchange_icon:ChangeIcon(1)
	end
	item.exchange_icon:AddEventListener(UIEvent.PointerClick,function ()
		if isCanExchange == false then
			return
		else
			local room_id = PlatformGuessBetProxy.getCurRoomId()
			PlatformGuessBetModule.sendReqScoreExchangeCoupon(room_id, index - 1, 1)
		end
	end)
end

function this.onSocketError()
	--断线时关闭聊天房间
	ViewManager.close(UIViewEnum.Platform_GuessBet_RewardScoreView)
	ViewManager.close(UIViewEnum.Platform_GuessBet_ChatRoomView)
end

function this.updateScoreView()
	local data = PlatformGuessBetProxy.getCurGuessRoomMoney()
	this.main_mid.money_text.text = tostring(data)
end

function this.updateRewardView()
	this:updateRewardScoreDetail()
end