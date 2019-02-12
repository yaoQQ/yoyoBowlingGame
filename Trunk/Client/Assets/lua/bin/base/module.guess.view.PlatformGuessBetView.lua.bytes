

require "base:enum/UIViewEnum"
require "base:mid/guess/Mid_platform_guess_bet_panel"
local Mathf = CS.UnityEngine.Mathf

--下注界面
PlatformGuessBetView = BaseView:new()
local this = PlatformGuessBetView
this.viewName = "PlatformGuessBetView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Help_View, UIViewEnum.Platform_GuessBet_BetView, false)

--设置加载列表
this.loadOrders =
{
	"base:guess/platform_guess_bet_panel",
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
	
	UITools.SetParentAndAlign(gameObject, self.container)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	
	self:addEvent()
end

function this:addEvent()
	self.main_mid.bet_input_field.inputField.onValueChanged:AddListener(this.onValueChangedHandler)

	self.main_mid.bet_confirm_btn:AddEventListener(UIEvent.PointerClick, function()
		local score = self:getCurInputNumber()
		PlatformGuessBetProxy.setCurDynamicBetScore(score)
		if score ~= 0 then
			local data = PlatformGuessBetProxy.getCurDynamicBetInfo()
			PlatformGuessBetModule.sendReqAnswerDynamicGuesssInfo(data.room_id, data.question_id, data.choose, data.bet_score)
			ViewManager.close(UIViewEnum.Platform_GuessBet_BetView)
		end
	end)
	self.main_mid.bet_cancel_btn:AddEventListener(UIEvent.PointerClick, function()
		ViewManager.close(UIViewEnum.Platform_GuessBet_BetView)
	end)
end

--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()

	self.main_mid.bet_input_field.text = ""
	
	this.updateScoreView()
end

--override 关闭UI回调
function this:onClose()
	self:removeNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.Socket_Error, this.onSocketError)
	NoticeManager.Instance:AddNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateRoomScore, this.updateScoreView)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Socket_Error, this.onSocketError)
	NoticeManager.Instance:RemoveNoticeLister(PlatformGuessBetType.Platform_GuessBet_UpdateRoomScore, this.updateScoreView)
end


function this:getCurInputNumber()
	local number = 0
	local str = self.main_mid.bet_input_field.text
	if str ~= "" then
		 number = tonumber(str)
	end
	return number
end

function this.onValueChangedHandler(str)
	if str ~= "" then
		local number = tonumber(str)
		number = Mathf.Clamp(number, 1, 200)
		--this.main_mid.bet_input_field.text = tostring(number)
	end
end

function this.updateScoreView()
	local data = PlatformGuessBetProxy.getCurGuessRoomMoney()
	this.main_mid.input_count_text.text = string.format("请输入积分数量(可用积分%s)", data)
end

function this.onSocketError()
	--断线时关闭
	ViewManager.close(UIViewEnum.Platform_GuessBet_BetView)
end