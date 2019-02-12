

require "base:enum/UIViewEnum"
require "base:mid/guess/Mid_platform_guess_question_rank_panel"

--竞猜问题排行榜界面
PlatformGuessQuestionRankView = BaseView:new()
local this = PlatformGuessQuestionRankView
this.viewName = "PlatformGuessQuestionRankView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_GuessBet_QuestionRankView, true)

--设置加载列表
this.loadOrders=
{
	"base:guess/platform_guess_question_rank_panel",
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName,gameObject)
	
	UITools.SetParentAndAlign(gameObject, self.container)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	self:addEvent()
end

function this:addEvent()
	self.main_mid.btn_return:AddEventListener(UIEvent.PointerClick,function ()
		ViewManager.close(UIViewEnum.Platform_GuessBet_QuestionRankView)
	end)
end

--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
	
	--msg数据的类型为RspGetRoomQuestionRankInfo
	self:updateData(msg)
end

--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.Socket_Error, this.onSocketError)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Socket_Error, this.onSocketError)
end


--打开页面初始化信息
function this:updateData(msg)
	--msg数据的类型为RspGetRoomQuestionRankInfo
	local question_detail = PlatformGuessBetProxy.getChatQuestionDataById(msg.question_id)
	
	self.main_mid.question_title_Text.text = question_detail.des
	self.main_mid.answer_1_Text.text = "A."..question_detail.option_info[1].option_des
	self.main_mid.answer_2_Text.text = "B."..question_detail.option_info[2].option_des
	self.main_mid.answer_3_Text.text = "C."..question_detail.option_info[3].option_des
	self.main_mid.answer_4_Text.text = "D."..question_detail.option_info[4].option_des
	
	self.main_mid.answer_1_right.gameObject:SetActive(false)
	self.main_mid.answer_2_right.gameObject:SetActive(false)
	self.main_mid.answer_3_right.gameObject:SetActive(false)
	self.main_mid.answer_4_right.gameObject:SetActive(false)
	if #msg.rank_info >= 10 then
		if msg.right_choose == 1 then
			self.main_mid.answer_1_right.gameObject:SetActive(true)
		elseif msg.right_choose == 2 then
			self.main_mid.answer_2_right.gameObject:SetActive(true)
		elseif msg.right_choose == 3 then
			self.main_mid.answer_3_right.gameObject:SetActive(true)
		elseif msg.right_choose == 4 then
			self.main_mid.answer_4_right.gameObject:SetActive(true)
		end
	end
	
	self.main_mid.rank_scroll_list:SetCellData(msg.rank_info, self.onUpdateRankItems, false)
end

--更新单个数据
function this.onUpdateRankItems(go, data, index)
	--data数据的类型为common.MsgMatchGuessQuestionRankInfo
	local item = this.main_mid.rank_CellArr[index + 1]
	
	if data.player_id == LoginDataProxy.playerId then
		item.highLight_Image.gameObject:SetActive(true)
	else
		item.highLight_Image.gameObject:SetActive(false)
	end
	
	if data.rank == 1 then
		item.rank_Text.gameObject:SetActive(false)
		item.rank_Icon.gameObject:SetActive(true)
		item.rank_Icon:ChangeIcon(0)
	elseif data.rank == 2 then
		item.rank_Text.gameObject:SetActive(false)
		item.rank_Icon.gameObject:SetActive(true)
		item.rank_Icon:ChangeIcon(1)
	elseif data.rank == 3 then
		item.rank_Text.gameObject:SetActive(false)
		item.rank_Icon.gameObject:SetActive(true)
		item.rank_Icon:ChangeIcon(2)
	else
		item.rank_Icon.gameObject:SetActive(false)
		item.rank_Text.gameObject:SetActive(true)
		item.rank_Text.text = tostring(data.rank)
	end
	
	downloadFromOtherServer(data.player_head_url, item.head_CircleImage)
	
	item.player_name_Text.text = data.nick_name
	
	item.use_Time_Text.text = tostring(data.use_time/1000)
end

function this.onSocketError()
	--断线时关闭聊天房间
	ViewManager.close(UIViewEnum.Platform_GuessBet_QuestionRankView)
	ViewManager.close(UIViewEnum.Platform_GuessBet_ChatRoomView)
end