

require "base:enum/UIViewEnum"
require "base:mid/guess/Mid_platform_guess_mybet_panel"

--我的竞猜界面
PlatformGuessMyBetView = BaseView:new()
local this = PlatformGuessMyBetView
this.viewName = "PlatformGuessMyBetView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_GuessBet_MyBetView, true)

--设置加载列表
this.loadOrders=
{
	"base:guess/platform_guess_mybet_panel",
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName,gameObject)
	
	UITools.SetParentAndAlign(gameObject, self.container)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	self:addEvent()
end

function this:addEvent()
	self.main_mid.btn_return:AddEventListener(UIEvent.PointerClick, function ()
		ViewManager.close(UIViewEnum.Platform_GuessBet_MyBetView)
	end)
end

--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
	
	--msg数据的类型为repeated common.MatchGuessMyAnswerInfo
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
	--msg数据的类型为repeated common.MatchGuessMyAnswerInfo
	local currGlobalBaseData = PlatformUserProxy:GetInstance():getUserInfo()
	downloadUserHead(currGlobalBaseData.head_url, self.main_mid.head_image)
	
	local data = PlatformGuessBetProxy.getCurGuessRoomMoney()
	this.main_mid.money_text.text = tostring(data)
	
	self.main_mid.my_bet_scroll_list:SetCellData(msg, self.onUpdateMyBetItems, false)
end

--更新单个数据
function this.onUpdateMyBetItems(go, data, index)
	--data数据的类型为common.MatchGuessMyAnswerInfo
	local item = this.main_mid.mybetCellArr[index + 1]
	
	local option = data.guess_info.option_info[data.choose]
	
	item.bet_content_text.text = data.guess_info.des
	item.bet_explain_text.text = "已下注："..option.option_des.."（赔率"..(option.rate / 100).."）"..data.bet_score.."积分"
	
	if data.guess_status == 0 then
		--未开奖
		item.no_lottery_text.gameObject:SetActive(true)
		item.win_image.gameObject:SetActive(false)
		item.win_result_text.gameObject:SetActive(false)
		item.lose_result_text.gameObject:SetActive(false)
	else
		--已开奖
		item.no_lottery_text.gameObject:SetActive(false)
		if data.is_right == 1 then
			item.win_image.gameObject:SetActive(true)
			item.win_result_text.text = "" 	--TODO
			item.win_result_text.gameObject:SetActive(true)
			item.lose_result_text.gameObject:SetActive(false)
		else
			item.win_image.gameObject:SetActive(false)
			item.win_result_text.gameObject:SetActive(false)
			item.lose_result_text.gameObject:SetActive(true)
		end
	end
end

function this.onSocketError()
	--断线时关闭聊天房间
	ViewManager.close(UIViewEnum.Platform_GuessBet_MyBetView)
	ViewManager.close(UIViewEnum.Platform_GuessBet_ChatRoomView)
end