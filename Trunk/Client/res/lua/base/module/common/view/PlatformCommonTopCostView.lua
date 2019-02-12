require "base:mid/common/Mid_common_top_cost_panel"

--顶部货币栏
PlatformCommonTopCostView = BaseView:new()
local this = PlatformCommonTopCostView
this.viewName = "PlatformCommonTopCostView"

--设置面板特性（界面层级、界面枚举、是否参与界面堆栈）
this:setViewAttribute(UIViewType.Main_view, UIViewEnum.Platform_Top_Cost_View, false)

--设置加载列表
this.loadOrders =
{
	"base:common/common_top_cost_panel",
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
	--下面两行默认需要调用
    

	--设置UI中间代码
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
	UITools.SetParentAndAlign(gameObject, self.container)
	
	--添加UI事件监听
	self:addEvent()
end

function this:addEvent()
	self.main_mid.top_gold_Button:AddEventListener(UIEvent.PointerClick,self.onBtnGoldEvent)
	self.main_mid.top_diamond_Button:AddEventListener(UIEvent.PointerClick,self.onBtnDiamondEvent)
end

function this:onBtnDiamondEvent()
	--RechargeManager.openShop(1, 0)
	--临时
	RechargeManager.openShop(2, 0)
end

function this:onBtnGoldEvent()
	RechargeManager.openShop(2, 0)	
end
--override 打开UI回调
function this:onShowHandler(msg)
	--打开界面时添加UI通知监听
	self:addNotice()
	local go = self:getViewGO()
	if go == nil then return end
	go.transform:SetAsLastSibling()

	--打开界面时初始化，一般用于处理没有数据时的默认的界面显示
	self:initView()
	this:updataUserHead()
    this:topGoldEffectTimer()
    this.updateDiamondAndMoney()
end
local oldValueDiamond = 0
local oldValueGold = 0
--override 关闭UI回调
function this:onClose()
	--关闭界面时移除UI通知监听
	self:removeNotice()
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("ucardRedBagTime")
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("diamondRedBagTime")
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("goldRedBagTime")
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("PlatformTopUpdateGold")
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Init_Diamond_Money, this.updateDiamondAndMoney)
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_Diamond, this.updateDiamondAndMoney)
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_Cash, this.updateDiamondAndMoney)
	--NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_Money,this.updateDiamondAndMoney)
	NoticeManager.Instance:AddNoticeLister(NoticeType.Mall_Update_Money, this.onUpdateMoney)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Init_Diamond_Money, this.updateDiamondAndMoney)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_Diamond, this.updateDiamondAndMoney)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_Cash, this.updateDiamondAndMoney)
	--NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_Money,this.updateDiamondAndMoney)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Mall_Update_Money, this.onUpdateMoney)
end


--更新顶部金币
function this.onUpdateMoney()
	local userInfo = PlatformUserProxy:GetInstance():getUserInfo()
	if userInfo == nil then return end
	if  userInfo.money > 10000 then
		this:updateDiamondAndMoney()
	else
		local count = 0
		local maxCount = 60
		local oldValue = tonumber(oldValueGold)
		local newValue = userInfo.money
		this.main_mid.top_diamond_Text.text=userInfo.diamond
		GlobalTimeManager.Instance.timerController:AddTimer(
			"PlatformTopUpdateGold",
			-1,
			maxCount,
			function()
				count = count + 1
				local curCount = count - 10
				if curCount < 0 then
					curCount = 0
				end
				this.main_mid.top_gold_Text.text = math.floor((newValue - oldValue) * curCount / (maxCount - 10)) + oldValue
			end)
	end
	this:updataSubsidy()
end
--打开界面时初始化
function this:initView()
	
end
function this:updataUserHead()
    local userInfo = PlatformUserProxy:GetInstance():getUserInfo()
	downloadUserHead(userInfo.head_url,this.main_mid.head_Icon)
	this.main_mid.press_Image:AddEventListener(UIEvent.PointerClick,function ()
		ViewManager.open(UIViewEnum.Personal_Change_Info_View)
	end)
end

function this:topGoldEffectTimer()

    GlobalTimeManager.Instance.timerController:AddTimer(
    "goldRedBagTime",
    2500,
    -1,
    function(...)
        this.main_mid.goldEffect:Play()
    end)

GlobalTimeManager.Instance.timerController:AddTimer(
    "diamondRedBagTime",
    2900,
    -1,
    function(...)
        this.main_mid.diamondEffect:Play()
    end)
GlobalTimeManager.Instance.timerController:AddTimer(
    "ucardRedBagTime",
    3500,
    -1,
    function(...)
        this.main_mid.packetEffect:Play()
    end)
end
--更新顶部金币钻石
function this.updateDiamondAndMoney()
    local currBaseData = PlatformUserProxy:GetInstance():getUserInfo()
    printDebug(currBaseData)
    if currBaseData == nil then return end
	oldValueGold = currBaseData.money
	local money = this:NumberToShow(currBaseData.money)
	local diamond = this:NumberToShow(currBaseData.diamond)
	local cash = this:redbagNumberToShow(currBaseData.cash/100)
    this.main_mid.top_gold_Text.text = tostring(money)
    this.main_mid.top_diamond_Text.text = tostring(diamond)
	this.main_mid.top_packet_Text.text = tostring(cash)
	
	local subsidyCount = PlatformUserProxy:GetInstance():getSubsidyCountDailyCount()
	if subsidyCount > 0 then	
		this:updataSubsidy()
	else
	--	Alert.showAlertMsg(nil,"今日领取的次数已到达上限!\n(每日00：00点时重置领取次数)"
		--,"确定")
	end

end


function this:NumberToShow(number)
	if number == nil then
		printDebug("数字格式错误")
	else
        if number / 10^9 >= 1 then
            return "9.99亿"
        elseif number / 10^8 >= 1 then
			number = math.floor(number / 10^6)
			local num = table.clearZero(tonumber(string.format("%.2f", number/10^2)))
            return (num.."亿")
        elseif number / 10^6 >= 1 then
			number = math.floor(number / 10^4)
            return number.."万"
        elseif number / 10^5 >= 1 then
			number = math.floor(number / 10^3)
            local num = table.clearZero(tonumber(string.format("%.1f", number/10)))
            return (num.."万")
        elseif number / 10^4 >= 1 then
			number = math.floor(number / 10^2)
			local num = (tonumber(string.format("%.2f", number/10^2)))
            return (num.."万")
		else
			return number
		end
	end
end
function this:redbagNumberToShow(number)
	if number == nil then
		printDebug("数字格式错误")
	else
		if number / 10^3 >= 1 then
			return math.floor(number)
		elseif number / 10^2 >= 1 then
			return tonumber(string.format("%.1f", number))
		else
			return number
		end
	end
end

function this:updataSubsidy()
	local userInfo = PlatformUserProxy:GetInstance():getUserInfo()
	local less_condition = TableBaseBankruptcySubsidy.data[1].less_condition
	if less_condition > userInfo.money then 
		ViewManager.open(UIViewEnum.Platform_Common_Subsidy_View)
	end

end

