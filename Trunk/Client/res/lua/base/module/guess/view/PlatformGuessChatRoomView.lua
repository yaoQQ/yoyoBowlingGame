

require "base:enum/UIViewEnum"
require "base:enum/PlatformGuessBetType"
require "base:mid/guess/Mid_platform_guess_chatroom_panel"
require "base:mid/guess/Mid_platform_chat_item_panel"

--竞猜聊天界面
PlatformGuessChatRoomView =BaseView:new()
local this=PlatformGuessChatRoomView
this.viewName="PlatformGuessChatRoomView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_GuessBet_ChatRoomView,true)

--设置加载列表
this.loadOrders=
{
	"base:guess/platform_guess_chatroom_panel",
	"base:guess/platform_chat_item_panel",
}

local roomId = 0
local roomInfo = nil
local guessInfo = nil

--override 加载UI完成回调
function this:onLoadUIEnd(uiName,gameObject)
	
	local switch = {
	
	    [self.loadOrders[1]] = function()
			self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
			printDebug(self.container.name)
			UITools.SetParentAndAlign(gameObject, self.container)
			self:addEvent()
	    end,
		[self.loadOrders[2]] = function()
			self.item = gameObject
			self.item:SetActive(false)
			
			self.main_mid.room_chat_ScrollPanel:SetCellBase(gameObject, self.main_mid.chat_content.gameObject)
	    end,
		
	}
	local fSwitch = switch[uiName] 

	if fSwitch then 
	   fSwitch() 
	else --key not found  
		printDebug( uiName.." not found !")
	end	
end

function this:addEvent()
	self.main_mid.btn_return:AddEventListener(UIEvent.PointerClick,function ()
		PlatformGuessBetModule.sendReqGuessChatChannelLeave(roomId)
	end)
	self.main_mid.btn_my_guess:AddEventListener(UIEvent.PointerClick, function ()
		PlatformGuessBetModule.sendReqMyDynamicGuesssList(roomId)
	end)
	self.main_mid.btn_more_bet:AddEventListener(UIEvent.PointerClick, function ()
		ViewManager.open(UIViewEnum.Platform_GuessBet_BetListView)
	end)
	self.main_mid.btn_check_reward:AddEventListener(UIEvent.PointerClick, function ()
		PlatformGuessBetModule.sendReqViewRoomRankReward(roomId)
	end)
	
	self.main_mid.choose_state_icon_1:AddEventListener(UIEvent.PointerClick, function ()
		if guessInfo == nil then
			return
		end
		PlatformGuessBetProxy.setCurDynamicBetInfo(guessInfo.room_id, guessInfo.question_id, 1)
		ViewManager.open(UIViewEnum.Platform_GuessBet_BetView)
	end)
	self.main_mid.choose_state_icon_2:AddEventListener(UIEvent.PointerClick, function ()
		if guessInfo == nil then
			return
		end
		PlatformGuessBetProxy.setCurDynamicBetInfo(guessInfo.room_id, guessInfo.question_id, 2)
		ViewManager.open(UIViewEnum.Platform_GuessBet_BetView)
	end)
	self.main_mid.choose_state_icon_3:AddEventListener(UIEvent.PointerClick, function ()
		if guessInfo == nil then
			return
		end
		PlatformGuessBetProxy.setCurDynamicBetInfo(guessInfo.room_id, guessInfo.question_id, 3)
		ViewManager.open(UIViewEnum.Platform_GuessBet_BetView)
	end)
	
	self.main_mid.chatImput_InputField:OnEndEdit(self.onBtnSendPress)
end

--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
	
	roomId = msg
	roomInfo = PlatformGuessBetProxy.getRoomInfoById(roomId)
	this:initView()

	--打开界面时请求动态问题列表
	PlatformGuessBetModule.sendReqGetDynamicGuesssList(roomId)
end

--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.Socket_Error, this.onSocketError)
	NoticeManager.Instance:AddNoticeLister(PlatformGuessBetType.platform_Add_Guesschat_MSG, this.addChatThings)
	NoticeManager.Instance:AddNoticeLister(PlatformGuessBetType.platform_Update_Guesschat_Guess_MSG, this.onUpdateGuessMsg)
	NoticeManager.Instance:AddNoticeLister(PlatformGuessBetType.platform_Fresh_GuessRoom_Data_MSG, this.freshGuessRoomData)
	NoticeManager.Instance:AddNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateChatRoomTopQuestion, this.updateTopQuestion)
	NoticeManager.Instance:AddNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateRoomScore, this.updateScoreView)

end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Socket_Error, this.onSocketError)
	NoticeManager.Instance:RemoveNoticeLister(PlatformGuessBetType.platform_Add_Guesschat_MSG, this.addChatThings)
	NoticeManager.Instance:RemoveNoticeLister(PlatformGuessBetType.platform_Update_Guesschat_Guess_MSG, this.onUpdateGuessMsg)
	NoticeManager.Instance:RemoveNoticeLister(PlatformGuessBetType.platform_Fresh_GuessRoom_Data_MSG, this.freshGuessRoomData)
	NoticeManager.Instance:RemoveNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateChatRoomTopQuestion, this.updateTopQuestion)
	NoticeManager.Instance:RemoveNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateRoomScore, this.updateScoreView)

end


-- 打开页面初始化信息
function this:initView()
	local currGlobalBaseData = PlatformUserProxy:GetInstance():getUserInfo()
	downloadUserHead(currGlobalBaseData.head_url, self.main_mid.myphoto_CImage)
	this.main_mid.my_rank_sorce_Text.text = ""
	this.main_mid.bet_info.gameObject:SetActive(false)
	this.addChatThings()
	this.freshGuessRoomData()
end


function this.freshGuessRoomData()
	this.main_mid.TextPersonNum.text = tostring(PlatformGuessBetProxy.getRoomNum())
end

function this.onSocketError()
	--断线时关闭聊天房间
	ViewManager.close(UIViewEnum.Platform_GuessBet_ChatRoomView)
end

--监听接受聊天信息
function this.addChatThings(notice, data)
	local curDataList = PlatformGuessBetProxy.getRoomChatMsgList() --先看聊天的信息长度

	this.main_mid.room_chat_ScrollPanel:SetCellData(curDataList, function (go, chatData, index)
		local item = Mid_platform_chat_item_panel:new(go)
		
		item.go:SetActive(true)
		item.self.gameObject:SetActive(false)
		item.other.gameObject:SetActive(false)
		item.question.gameObject:SetActive(false)
		item.live.gameObject:SetActive(false)
		--设置数据内容
		local curBaseData = chatData.user_base_info
		local curChatInfo = chatData.chat_info
		--答题
		if curChatInfo.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_MatchGuess then
			item.question.gameObject:SetActive(true)
				
			item.question_name_Text.text = curBaseData.nick_name
			downloadUserHead(curBaseData.head_url ,item.question_head_Image)

			local chatQuestionData = PlatformGuessBetProxy.getChatQuestionDataById(curChatInfo.msg.question_id)
			if chatQuestionData == nil then
				return
			end
			
			item.question_title_Text.text = chatQuestionData.des
			item.answer_1_Text.text = "A."..chatQuestionData.option_info[1].option_des
			item.answer_2_Text.text = "B."..chatQuestionData.option_info[2].option_des
			item.answer_3_Text.text = "C."..chatQuestionData.option_info[3].option_des
			item.answer_4_Text.text = "D."..chatQuestionData.option_info[4].option_des
			
			item.btn_answer_1:ChangeIcon(0)
			item.btn_answer_2:ChangeIcon(0)
			item.btn_answer_3:ChangeIcon(0)
			item.btn_answer_4:ChangeIcon(0)
			item.answer_1_right.gameObject:SetActive(false)
			item.answer_1_wrong.gameObject:SetActive(false)
			item.answer_2_right.gameObject:SetActive(false)
			item.answer_2_wrong.gameObject:SetActive(false)
			item.answer_3_right.gameObject:SetActive(false)
			item.answer_3_wrong.gameObject:SetActive(false)
			item.answer_4_right.gameObject:SetActive(false)
			item.answer_4_wrong.gameObject:SetActive(false)
			if chatQuestionData.chooseIndex == 1 then
				item.btn_answer_1:ChangeIcon(1)
				if chatQuestionData.isRight == true then
					item.answer_1_right.gameObject:SetActive(true)
				elseif chatQuestionData.isRight == false then
					item.answer_1_wrong.gameObject:SetActive(true)
				end
			elseif chatQuestionData.chooseIndex == 2 then
				item.btn_answer_2:ChangeIcon(1)
				if chatQuestionData.isRight == true then
					item.answer_2_right.gameObject:SetActive(true)
				elseif chatQuestionData.isRight == false then
					item.answer_2_wrong.gameObject:SetActive(true)
				end
			elseif chatQuestionData.chooseIndex == 3 then
				item.btn_answer_3:ChangeIcon(1)
				if chatQuestionData.isRight == true then
					item.answer_3_right.gameObject:SetActive(true)
				elseif chatQuestionData.isRight == false then
					item.answer_3_wrong.gameObject:SetActive(true)
				end
			elseif chatQuestionData.chooseIndex == 4 then
				item.btn_answer_4:ChangeIcon(1)
				if chatQuestionData.isRight == true then
					item.answer_4_right.gameObject:SetActive(true)
				elseif chatQuestionData.isRight == false then
					item.answer_4_wrong.gameObject:SetActive(true)
				end
			end	
			
			if chatQuestionData.isRight == nil then
				item.btn_answer_1:AddEventListener(UIEvent.PointerClick,function ()
					if chatQuestionData.isRight == nil then
						PlatformGuessBetModule.sendReqAnswerQuestionInfo(roomId, curChatInfo.msg.question_id, 1)
					end
				end)
				item.btn_answer_2:AddEventListener(UIEvent.PointerClick,function ()
					if chatQuestionData.isRight == nil then
						PlatformGuessBetModule.sendReqAnswerQuestionInfo(roomId, curChatInfo.msg.question_id, 2)
					end
				end)
				item.btn_answer_3:AddEventListener(UIEvent.PointerClick,function ()
					if chatQuestionData.isRight == nil then
						PlatformGuessBetModule.sendReqAnswerQuestionInfo(roomId, curChatInfo.msg.question_id, 3)
					end
				end)
				item.btn_answer_4:AddEventListener(UIEvent.PointerClick,function ()
					if chatQuestionData.isRight == nil then
						PlatformGuessBetModule.sendReqAnswerQuestionInfo(roomId, curChatInfo.msg.question_id, 4)
					end
				end)
			end
			item.rank_Text:AddEventListener(UIEvent.PointerClick,function ()
				PlatformGuessBetModule.sendReqGetRoomQuestionRankInfo(roomId, curChatInfo.msg.question_id)
			end)
			
			this.main_mid.room_chat_ScrollPanel:SetCellHeightAtIndex(index, 500)
		-- 直播
		elseif curChatInfo.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_MatchGuessEventIive then
			printDebug("curChatInfo = "..table.tostring(curChatInfo))

			item.live.gameObject:SetActive(true)
			local eachLineMax = 18
			local lineCount = math.ceil(string.len(curChatInfo.msg) / eachLineMax)
			local deltaH = (lineCount - 1) * 50
			local height = 100 + deltaH
			item.live_event_text.text = string.format("%s - %s", "","")
			--设置大小
			local tempImageRect = item.live_bg_image.transform:GetComponent(typeof(RectTransform))
			tempImageRect.sizeDelta = Vector2(700, height)
			tempImageRect.anchoredPosition = Vector2(435, 0 - deltaH / 2)
			this.main_mid.room_chat_ScrollPanel:SetCellHeightAtIndex(index, height)
		else
			local playerId = LoginDataProxy.playerId
			if curChatInfo.player_id == playerId then
				item.self.gameObject:SetActive(true)
				
				item.self_name_Text.text = curBaseData.nick_name
				downloadUserHead(curBaseData.head_url, item.self_head_Image)
				item.self_chat_bg_Image.gameObject:SetActive(false)
				item.self_Image.gameObject:SetActive(false)
				item.self_regbag_Image.gameObject:SetActive(false) 
				item.self_voice_Image.gameObject:SetActive(false)

				--文字
				if curChatInfo.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_Text then
					item.self_chat_bg_Image.gameObject:SetActive(true) 
					item.self_chat_Text.text = curChatInfo.msg
					--设置大小
					local tempImageRect = item.self_chat_bg_Image.transform:GetComponent(typeof(RectTransform))
					tempImageRect.sizeDelta = Vector2(600, 400)
					local preferredWidth = item.self_chat_Text.Txt.preferredWidth
					if preferredWidth < 540 then
						item.self_chat_Text.Txt.alignment = TextAnchor.UpperRight
						tempwidth = item.self_chat_Text.Txt.preferredWidth + 60
					else
						item.self_chat_Text.Txt.alignment = TextAnchor.UpperLeft
						tempwidth = 540 + 60
					end
					tempHeight = item.self_chat_Text.Txt.preferredHeight + 40
					if tempHeight > 600 then
						tempHeight = 600
					end
					tempImageRect.sizeDelta = Vector2(tempwidth, tempHeight)
					
					this.main_mid.room_chat_ScrollPanel:SetCellHeightAtIndex(index, tempHeight + 100)
				else
					this.main_mid.room_chat_ScrollPanel:SetCellHeightAtIndex(index, 300)
				end
			else
				item.other.gameObject:SetActive(true)
				
				item.other_name_Text.text = curBaseData.nick_name
				downloadUserHead(curBaseData.head_url, item.other_head_Image)
				item.other_chat_bg_Image.gameObject:SetActive(false)  
				item.other_Image.gameObject:SetActive(false)
				item.other_regbag_Image.gameObject:SetActive(false)  
				item.other_voice_Image .gameObject:SetActive(false) 
				
				--文字
				if curChatInfo.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_Text then
					item.other_chat_bg_Image.gameObject:SetActive(true) 
					item.other_chat_Text.text = curChatInfo.msg
					--设置大小
					local tempImageRect = item.other_chat_bg_Image.transform:GetComponent(typeof(RectTransform))
					tempImageRect.sizeDelta = Vector2(600, 400)
					local preferredWidth = item.other_chat_Text.Txt.preferredWidth
					if preferredWidth < 540 then
						tempwidth = item.other_chat_Text.Txt.preferredWidth + 60
					else
						tempwidth = 540 + 60
					end
					tempHeight = item.other_chat_Text.Txt.preferredHeight + 40
					if tempHeight > 600 then
						tempHeight = 600
					end
					tempImageRect.sizeDelta = Vector2(tempwidth,tempHeight)
					
					this.main_mid.room_chat_ScrollPanel:SetCellHeightAtIndex(index, tempHeight + 100)
				--红包
				elseif curChatInfo.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_Redpacket then
					item.other_regbag_Image.gameObject:SetActive(true)
					--curChatInfo.msg:common.MsgActiveCashRedPacketSt
					item.other_pray_Text.text = curChatInfo.msg.redpacket_name
					item.other_regbag_Image:AddEventListener(UIEvent.PointerClick,function (eventData)
						local msg = {}
						msg.activeId = curChatInfo.msg.active_id
						msg.redpacketId = curChatInfo.msg.red_packet_id
						msg.redpacketType = ProtoEnumCommon.RedPacketType.RedPacketType_MatchGuess
						msg.isFromChat = true
						msg.headUrl = curBaseData.head_url
						msg.name = curBaseData.nick_name
						msg.title = curChatInfo.msg.redpacket_name
						msg.describe = curChatInfo.msg.discribe
						msg.is_official = false
						PlatformRedPacketProxy.SetOpenLBSPacketData("RedPacket_Open_Data",msg)
						PlatformLBSRedPacketOpenView.openLBSRedPacketOpenView()
					end)
					
					this.main_mid.room_chat_ScrollPanel:SetCellHeightAtIndex(index, 300)
				--优惠券
				elseif curChatInfo.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_Coupon then
					item.other_regbag_Image.gameObject:SetActive(true)
					--curChatInfo.msg:common.MsgActiveCouponRedPacketSt
					item.other_pray_Text.text = curChatInfo.msg.redpacket_name
					item.other_regbag_Image:AddEventListener(UIEvent.PointerClick,function (eventData)
						local msg = {}
						msg.activeId = curChatInfo.msg.active_id
						msg.redpacketId = curChatInfo.msg.red_packet_id
						msg.redpacketType = ProtoEnumCommon.RedPacketType.RedPacketType_MatchGuess
						msg.isFromChat = true
						msg.isCoupon = true
						msg.coupon_id = curChatInfo.msg.coupon_id
						msg.headUrl = curBaseData.head_url
						msg.name = curBaseData.nick_name
						msg.title = curChatInfo.msg.redpacket_name
						PlatformRedPacketProxy.SetOpenLBSPacketData("Coupon_Open_Data", msg)
						PlatformLBSCouponOpenView.openPlatformLBSCouponOpenView(false)
					end)
					this.main_mid.room_chat_ScrollPanel:SetCellHeightAtIndex(index, 300)
				else
					this.main_mid.room_chat_ScrollPanel:SetCellHeightAtIndex(index, 300)
				end
			end
		end
	end)
end

function this.onUpdateGuessMsg(notice, data)
	local index = data:GetObj()
	this.main_mid.room_chat_ScrollPanel:UpdateCell(index)
end


--发送文字聊天
function this.onBtnSendPress(eventData)
	if this.main_mid.chatImput_InputField.text == "" then
		return
	end

	local channel_id = roomId
	local chat_info = {}
	chat_info.msg = this.main_mid.chatImput_InputField.text
	chat_info.chat_msg_type = ProtoEnumCommon.ChatMsgType.ChatMsgType_Text
	
	PlatformGuessBetModule.sendReqGuessSendChat(channel_id, chat_info)

	this.main_mid.chatImput_InputField.text = ""
end

function this:setRateText(textWidget, data, root)
	if table.empty(data) then
		root.gameObject:SetActive(false)
		return
	end
	root.gameObject:SetActive(true)
	textWidget.text = string.format("%s \n (%s)", data.option_des, data.rate / 100)
end

--更新顶部问题
function this.updateTopQuestion()
	local data = PlatformGuessBetProxy.getLastDynamicQuestionInfo()
	if data == nil then
		this.main_mid.bet_info.gameObject:SetActive(false)
		return
	end
	
	this.main_mid.bet_info.gameObject:SetActive(true)
	
	guessInfo = data.guess_info
	local question_status = data.question_status
	
	this.main_mid.bet_content_text.text = guessInfo.des
	this:setRateText(this.main_mid.odds_text_1, guessInfo.option_info[1], this.main_mid.choose_state_icon_1)
	this:setRateText(this.main_mid.odds_text_2, guessInfo.option_info[2], this.main_mid.choose_state_icon_2)
	this:setRateText(this.main_mid.odds_text_3, guessInfo.option_info[3], this.main_mid.choose_state_icon_3)
	
	this.main_mid.choose_state_icon_1:ChangeIcon(question_status.question_status)
	this.main_mid.choose_state_icon_2:ChangeIcon(question_status.question_status)
	this.main_mid.choose_state_icon_3:ChangeIcon(question_status.question_status)
	
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
			this.main_mid.choose_flag_icon_1:ChangeIcon(1)
		else
			this.main_mid.choose_flag_icon_1:ChangeIcon(0)
		end
		if isChoose2 then
			this.main_mid.choose_flag_icon_2:ChangeIcon(1)
		else
			this.main_mid.choose_flag_icon_2:ChangeIcon(0)
		end
		if isChoose3 then
			this.main_mid.choose_flag_icon_3:ChangeIcon(1)
		else
			this.main_mid.choose_flag_icon_3:ChangeIcon(0)
		end
	else
		this.main_mid.choose_flag_icon_1:ChangeIcon(0)
		this.main_mid.choose_flag_icon_2:ChangeIcon(0)
		this.main_mid.choose_flag_icon_3:ChangeIcon(0)
	end
end

function this.updateScoreView()
	local data = PlatformGuessBetProxy.getCurGuessRoomMoney()
	this.main_mid.my_rank_sorce_Text.text = tostring(data)
end