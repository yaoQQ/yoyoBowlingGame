


PlatformGuessBetProxy = {}
local this=PlatformGuessBetProxy


-- 房间列表信息
this.roomList = nil
function this.setRoomListInfo(msg)
	this.roomList = msg
end
function this.getRoomListInfo()
	return this.roomList
end
--根据房间id获取房间信息
function this.getRoomInfoById(roomId)
	for k,v in ipairs(this.roomList.room_info) do
		if v.room_id == roomId then
			return v
		end
	end
	return nil
end

--根据房间id获取比赛信息
function this.getDetailInfoByRoomId(id)
	if table.empty(this.roomList) then
		return nil
	end
	local info = nil
	for i = 1, #this.roomList.room_info do
		if this.roomList.room_info[i].room_id == id then
			info = this.roomList.detail_info[i]
			break
		end
	end
	return info
end

-- 当前动态问题消息
this.dynamicQuestionList = {}

--msg:RspGetDynamicGuesssList
function this.setBetListInfo(msg)
	this.dynamicQuestionList = {}
	for i = 1, #msg.guess_info do
		local dq = {}
		dq.guess_info = msg.guess_info[i]
		dq.question_status = msg.question_status[i]
		this.dynamicQuestionList[i] = dq
	end
	
	NoticeManager.Instance:Dispatch(PlatformGuessBetType.Platform_GuessBet_UpdateChatRoomTopQuestion)
end
function this.getBetListInfo()
	return this.dynamicQuestionList
end

--msg:MatchGuessQuestionDetail
function this.addBetInfo(msg)
	for i = #this.dynamicQuestionList, 1 do
		this.dynamicQuestionList[i+1] = this.dynamicQuestionList[i]
	end
	local dq = {}
	dq.guess_info = msg
	dq.question_status = {}
	dq.question_id = msg.question_id
	dq.question_status.question_status = 0
	this.dynamicQuestionList[1] = dq
	
	NoticeManager.Instance:Dispatch(PlatformGuessBetType.Platform_GuessBet_UpdateChatRoomTopQuestion)
end

function this.updateDynamicItem(question_id, choose, bet_score)
	if table.empty(this.dynamicQuestionList) then
		return
	end
	local list = this.dynamicQuestionList
	local item = nil
	for i = 1, #list do
		if list[i].question_status.question_id == question_id then
			item = list[i].question_status
			break
		end
	end
	if table.empty(item) then
		Loger.PrintError("错误, 客户端动态问题列表没有该问题, question_id = ", question_id)
		return
	else
		item.question_status = 1
		if item.answer_detail == nil then
			item.answer_detail = {}
		end
		local detail = {}
		detail.choose = choose
		detail.bet_score = bet_score
		item.answer_detail[#item.answer_detail+1] = detail
		--printDebug("updateDynamicItem, item = "..table.tostring(item))
	end
	
	NoticeManager.Instance:Dispatch(PlatformGuessBetType.Platform_GuessBet_UpdateChatRoomTopQuestion)
end

this.curGuessRoomMoney = 0
function this.setCurGuessRoomMoney(msg)
	this.curGuessRoomMoney = msg
	NoticeManager.Instance:Dispatch(PlatformGuessBetType.Platform_GuessBet_UpdateRoomScore)
end
function this.getCurGuessRoomMoney()
	return this.curGuessRoomMoney
end

-- 获取动态问题的最后一条问题消息
function this.getLastDynamicQuestionInfo()
	if #this.dynamicQuestionList == 0 then
		return nil
	end
	return this.dynamicQuestionList[#this.dynamicQuestionList]
end

-- 当前动态投注信息
this.curDynamicBetInfo = nil
function this.setCurDynamicBetInfo(room_id, question_id, choose)
	this.curDynamicBetInfo = {}
	this.curDynamicBetInfo.room_id = room_id
	this.curDynamicBetInfo.question_id = question_id
	this.curDynamicBetInfo.choose = choose
end
function this.setCurDynamicBetScore(score)
	if table.empty(this.curDynamicBetInfo) then
		return
	end
	this.curDynamicBetInfo.bet_score = score
end
function this.getCurDynamicBetInfo()
	return this.curDynamicBetInfo
end
function this.resetCurDynamicBetInfo()
	this.curDynamicBetInfo = nil
end


--奖励信息
this.rewardInfo = nil
--msg:RspViewRoomRankReward
function this.setRewardInfo(msg)
	this.rewardInfo = msg
end
function this.getRewardInfo()
	return this.rewardInfo
end

this.curExchangeInfo = nil
function this.setCurExchangeInfo(msg)
	this.curExchangeInfo = msg
end
function this.getCurExchangeInfo()
	return this.curExchangeInfo
end
function this.updateScoreRewardItem(index, have_changed)
	if table.empty(this.rewardInfo) then
		return
	end
	local list = this.rewardInfo.reward_info
	if table.empty(list)== false  then
		local item = nil
		for i = 1, #list do
			if list[i].rank == index then
				item = list[i]
				break
			end
		end
		if table.empty(item) then
			Loger.PrintError("错误, 客户端奖励列表没有该数据, index = ", index)
			return
		else
			item.have_changed = have_changed
		end
	end
end

function this.updateRankRewardItem()
	if table.empty(this.rewardInfo) then
		return
	end
	this.rewardInfo.reward_flag = 2
end

--当前进入房间房间ID
this.roomId = 0
function this.setCurRoomId(id)
	printDebug("设置CurRoomId = "..id)
	this.roomId = id
end

function this.getCurRoomId()
	return this.roomId
end

------------------------------竞猜聊天数据------------------------------

--竞猜房间聊天数据
local curRoomChatMsgList = {}
--聊天问题索引数据
local chatQuestionIndexDict = {}

function this.clearRoomChatMsgList()
	curRoomChatMsgList = {}
	chatQuestionIndexDict = {}
	chatQuestionDataDict = {}
end

function this.addRoomChatMsg(singleData)
	if curRoomChatMsgList == nil then
		curRoomChatMsgList  = {}
	end
	printDebug("===========新增聊天信息========="..table.tostring(singleData))
	table.insert(curRoomChatMsgList, singleData)
	
	if singleData.chat_info.chat_msg_type == ProtoEnumCommon.ChatMsgType.ChatMsgType_MatchGuess then
		chatQuestionIndexDict[singleData.chat_info.msg.question_id] = #curRoomChatMsgList - 1
	end
	
	NoticeManager.Instance:Dispatch(PlatformGuessBetType.platform_Add_Guesschat_MSG)
end

function this.getRoomChatMsgList()
	return curRoomChatMsgList
end

--聊天问题数据
local chatQuestionDataDict = {}

--设置聊天问题数据
--[[数据示例：
msg = table: 00000000560497C0
{
  question_id = 59
  option_info = table: 0000000056049800
  {
	[1] = table: 0000000056049840
	{
	  option_des = "3"
	}
	[2] = table: 0000000056049880
	{
	  option_des = "4"
	}
	[3] = table: 00000000560498C0
	{
	  option_des = "5"
	}
	[4] = table: 0000000056049900
	{
	  option_des = "6"
	}
  }
  score = 5
  des = "巴西拿了多少次世界杯冠军？"
  room_id = 7
  question_type = "MatchGuessQuestionType_Static"
}--]]
function this.addChatQuestionData(msg)
	chatQuestionDataDict[msg.question_id] = msg
end

function this.updateChatQuestionChooseData(id, chooseIndex)
	local msg = chatQuestionDataDict[id]
	if msg ~= nil then
		msg.chooseIndex = chooseIndex
	end
end

function this.updateChatQuestionAnswerData(id, isRight)
	local msg = chatQuestionDataDict[id]
	if msg ~= nil then
		msg.isRight = isRight
	end
	
	NoticeManager.Instance:Dispatch(PlatformGuessBetType.platform_Update_Guesschat_Guess_MSG, chatQuestionIndexDict[id])
end

function this.getChatQuestionDataById(id)
	return chatQuestionDataDict[id]
end

------------------------------竞猜聊天数据end------------------------------

---竞猜聊天额外信息--

this.roomNum = 0 
function this.setRoomNum(allNum)
	this.roomNum  = allNum
end

function this.addRoomNum(addnum)
	this.roomNum  = this.roomNum  + addnum
end

function this.getRoomNum()
	return this.roomNum 
end




