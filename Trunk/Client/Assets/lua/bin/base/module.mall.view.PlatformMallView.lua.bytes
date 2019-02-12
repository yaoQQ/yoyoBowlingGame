require "base:mid/mall/Mid_platform_mall_panel"

PlatformMallView = BaseView:new()
local this = PlatformMallView
this.viewName = "PlatformMallView"

--设置面板特性
this:setViewAttribute(UIViewType.Pop_view, UIViewEnum.Platform_Mall_View, false)

--设置加载列表
this.loadOrders=
{
	"base:mall/platform_mall_panel",
}

-- 显示的商城类型
this.MallType =
{
	Diamond 	= 1,	-- 钻石
	Gold 		= 2,	-- 金币
	YoCard 		= 3,	-- 优卡
	Blend 		= 4,	-- 混合
}

--来源类型 0.平台 >0.游戏id
local m_fromType = 0
--从平台或游戏传入的充值兑换成功回调
local m_succeedCallback = nil

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	UITools.SetParentAndAlign(gameObject, self.container)
	self.main_mid = Mid_platform_mall_panel
	self:BindMonoTable(gameObject, self.main_mid)
	self:addEvent()
	self:hidePoolUI()
end

function this:hidePoolUI()
	this.main_mid.gold_icon_pool.gameObject:SetActive(false)
	this.main_mid.diamond_icon_pool.gameObject:SetActive(false)
	this.main_mid.cost_icon_pool.gameObject:SetActive(false)
end

function this:addEvent()
	self.main_mid.close_Image:AddEventListener(UIEvent.PointerClick,function ()
		ViewManager.close(UIViewEnum.Platform_Mall_View)
	end)
	--临时关闭
	--self.main_mid.diamond_Icon:AddEventListener(UIEvent.PointerClick,function ()
		--this:updateMallEnum(1)
	--end)
	--self.main_mid.gold_Icon:AddEventListener(UIEvent.PointerClick,function ()
		--this:updateMallEnum(2)
	--end)
	
	--self.main_mid.yo_card_Icon:AddEventListener(UIEvent.PointerClick,function ()
		--this:updateMallEnum(3)
 	--	showFloatTips("功能还在开发中，敬请期待！")
  
	--end)
	this.main_mid.press_Image:AddEventListener(UIEvent.PointerClick,function ()
		--ViewManager.open(UIViewEnum.Personal_Change_Info_View)
	end)

	this.main_mid.blend_panel.gameObject:SetActive(false)
end

--override 打开UI回调
function this:onShowHandler(msg)
	self:initView()
	self:addNotice()
	m_fromType = msg.fromType
	m_succeedCallback = msg.succeedCallback
	this:updateMallEnum(msg.shopType)
end
local oldValueDiamond = 0
local oldValueGold = 0
--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("ucardEffectTime")
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("diamondEffectTime")
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("goldEffectTime")
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("PlatformMallUpdateDiamond")
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("PlatformMallUpdateGold")

end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Init_Diamond_Money, this.onInitDiamondAndMoney)
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_Cash, this.updateCash)
	NoticeManager.Instance:AddNoticeLister(NoticeType.Mall_Update_Diamond, this.onUpdateDiamond)
	NoticeManager.Instance:AddNoticeLister(NoticeType.Mall_Update_Money, this.onUpdateMoney)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Init_Diamond_Money, this.onInitDiamondAndMoney)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_Cash, this.updateCash)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Mall_Update_Diamond, this.onUpdateDiamond)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Mall_Update_Money, this.onUpdateMoney)
end

this.currMallData = {"1","2"}
--更新商城界面
function this:updateMallEnum(mallEnum)
	this.currMallBaseData = PlatformUserProxy:GetInstance():getUserInfo()
	downloadUserHead(this.currMallBaseData.head_url,this.main_mid.head_Icon)

	--mallEnum = PlatformMallView.MallType.Blend -- temp
	if this.mallIcon ~= nil then
		this.mallIcon:ChangeIcon(0)
	end
	if this.mallPanel ~= nil then 
		this.mallPanel.gameObject:SetActive(false)
	end
	
	if	mallEnum == 1 then
		this.mallIcon = this.main_mid.diamond_Icon
	    this.mallPanel = this.main_mid.diamond_Panel
	    this.currMallData = TableBaseRechargeShop.data
	    this.main_mid.diamond_GridRecycleScrollPanel:SetCellData(this.currMallData, this.onSetDiamond,true)
    elseif mallEnum == 2 then
    	this.mallIcon = this.main_mid.gold_Icon
	    this.mallPanel = this.main_mid.gold_Panel
	    this.currMallData = TableBaseShopExchange.data
	    this.main_mid.gold_GridRecycleScrollPanel:SetCellData(this.currMallData, this.onSetGold,true)
	elseif mallEnum == 3 then
	    this.mallIcon = this.main_mid.yo_card_Icon
	    this.mallPanel = this.main_mid.yo_card_Panel
	elseif mallEnum == PlatformMallView.MallType.Blend then
		this.mallIcon = this.main_mid.gold_Icon
		this.mallPanel = this.main_mid.blend_panel
		this.currMallData = TableBaseShopExchange.data
		this.main_mid.blend_grid_scroll:SetCellData(this.currMallData, this.onSetBlendInfo, true)
	else
		printDebug("+++++++++++++未知选项")
	end
	this.mallIcon:ChangeIcon(1)
	this.mallPanel.gameObject:SetActive(true)
	this:updateCash()
	this:onInitDiamondAndMoney()
	this:topGoldEffectTimer()

end

function this:topGoldEffectTimer()
		GlobalTimeManager.Instance.timerController:AddTimer(
        "goldEffectTime",
        2500,
        -1,
        function(...)
			this.main_mid.goldEffect:Play()
        end)

		GlobalTimeManager.Instance.timerController:AddTimer(
	        "diamondEffectTime",
	        2900,
	        -1,
	        function(...)
				this.main_mid.diamondEffect:Play()
	        end)
		GlobalTimeManager.Instance.timerController:AddTimer(
	        "ucardEffectTime",
	        3500,
	        -1,
	        function(...)
				this.main_mid.packetEffect:Play()
	        end)
end

function this.onInitDiamondAndMoney()
	local userInfo = PlatformUserProxy:GetInstance():getUserInfo()
	if userInfo == nil then return end
	oldValueDiamond = userInfo.diamond
	oldValueGold = userInfo.money
	local money = this:NumberToShow(userInfo.money)
	local diamond = this:NumberToShow(userInfo.diamond)
	printDebug("userInfo"..table.tostring(userInfo))
    this.main_mid.top_gold_Text.text = tostring(money)
    this.main_mid.top_diamond_Text.text = tostring(diamond)

end

 function this.updateCash()
	local userInfo = PlatformUserProxy:GetInstance():getUserInfo()
	if userInfo == nil then return end
	local cash = this:redbagNumberToShow(userInfo.cash/100)
	this.main_mid.top_packet_Text.text = tostring(cash)
end

--更新顶部钻石

function this.onUpdateDiamond()
	local userInfo = PlatformUserProxy:GetInstance():getUserInfo()
	if userInfo == nil then return end
	if  userInfo.diamond > 10000 then
		this:onInitDiamondAndMoney()
	else
		local count = 0
		local maxCount = 60
		local oldValue = tonumber(oldValueDiamond)
		local newValue = userInfo.diamond
		GlobalTimeManager.Instance.timerController:AddTimer(
			"PlatformMallUpdateDiamond",
			-1,
			maxCount,
			function()
				count = count + 1
				this.main_mid.top_diamond_Text.text = math.floor((newValue - oldValue) * count / maxCount) + oldValue
			end)
	end
end
--更新顶部金币
function this.onUpdateMoney()
	local userInfo = PlatformUserProxy:GetInstance():getUserInfo()
	if userInfo == nil then return end
	if  userInfo.money > 10000 then
		this:onInitDiamondAndMoney()
	else
		local count = 0
		local maxCount = 60
		local oldValue = tonumber(oldValueGold)
		local newValue = userInfo.money
		this.main_mid.top_diamond_Text.text=userInfo.diamond
		GlobalTimeManager.Instance.timerController:AddTimer(
			"PlatformMallUpdateGold",
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
end


function this.onSetDiamond(go,data,index)
	local item = this.main_mid.diamondcellArr[index+1]

	this:exchangeNum(item.cost_Text, data.cash)
	this:exchangeNum(item.return_num_Text, data.item)

	if data.id > 6 then 
		item.go_Icon:ChangeIcon(5)
	else
		item.go_Icon:ChangeIcon(data.id-1)
	end

	if data.id > 3 then
		GlobalTimeManager.Instance.timerController:AddTimer("TimeDiamondRandom"..data.id,
		800*data.id,
		-1,
		function()
		item.mallEffect:Play()	
		end)
	end

	item.go_circle_Button:AddEventListener(UIEvent.PointerClick,function ()
		
		if IS_UNITY_EDITOR then
			showFloatTips("UNITY_EDITOR不能充值")
			return
		end
		local succeedCallback = nil
		if m_fromType == 0 then
			succeedCallback = function ()
				ViewManager.open(UIViewEnum.Platform_Mall_Tip_View,{enum = 3,successData = data})
			end
		else
			succeedCallback = function ()
				ViewManager.open(UIViewEnum.Platform_Mall_Tip_View,{enum = 3,successData = data})
				m_succeedCallback()
			end
		end
		--充值
		RechargeManager.recharge("0", LoginDataProxy.token, tostring(data.id), tostring(LoginDataProxy.playerId), "1", succeedCallback)
	end)
end

function this.onSetGold(go,data,index)
	local item = this.main_mid.goldcellArr[index + 1]
	
	--this:exchangeNum(item.cost_Text, data.src_item)
	--this:exchangeNum(item.return_num_Text, data.dest_item)
	
	if data.id > 6 then 
		item.go_Icon:ChangeIcon(5)
	else
		item.go_Icon:ChangeIcon(data.id-1)
	end
	if data.id > 3 then
	math.randomseed(tostring(os.time()):reverse():sub(1, 7)) 
	GlobalTimeManager.Instance.timerController:AddTimer("TimeRandom"..data.id,
		math.random(500, 800)*index,
		-1,
		function()
		item.mallEffect:Play()	
		end)
	end
	item.go_circle_Button:AddEventListener(UIEvent.PointerClick,function ()

		local userInfo = PlatformUserProxy:GetInstance():getUserInfo()
		if data.src_item > userInfo.diamond then--钻石不足
			Alert.showVerifyMsg("钻石不足", "是否立即充值", "去充值?", function ()
				this:updateMallEnum(1)
			end, "取消", nil)
		else

			local fun = function()
				local succeedCallback = nil
				if m_fromType == 0 then
					succeedCallback = function ()
						ViewManager.open(UIViewEnum.Platform_Mall_Tip_View,{enum = 4,successData = data})
					end
				else
					succeedCallback = function ()
						ViewManager.open(UIViewEnum.Platform_Mall_Tip_View,{enum = 4,successData = data})
						m_succeedCallback()
					end
				end
				--兑换
				RechargeManager.exchange("0", LoginDataProxy.token, tostring(data.id), tostring(LoginDataProxy.playerId), succeedCallback)
			end
			
			Alert.showVerifyMsg("兑换金币提示","确定使用"..data.src_item.."钻石兑换?"..data.dest_item.."金币", "取消", nil,"确定",fun)
			--this.main_mid.noClick_Image.gameObject:SetActive(true)
			
		end
	end)

end

function this.onSetBlendInfo(go,data,index)
	local item = this.main_mid.blendCellArr[index + 1]
	--printDebug("aaaaaaaaaaaaaa"..data.dest_item)
	this:exchangeNum(item.return_num_text, data.dest_item)
	-- 消耗物
	if data.src_type == 1 then
		-- 现金
		item.cost_image:SetPng(this.main_mid.cost_icon_pool.IconArr[1])
		item.cost_text.text = CS.System.String.Format("{0:0.##}", data.src_item / 100)
	elseif data.src_type == 2 then
		-- 钻石
		item.cost_image:SetPng(this.main_mid.cost_icon_pool.IconArr[0])
		item.cost_text.text = string.format("%s", data.src_item)
	else
		Loger.PrintError("错误,商城兑换表配置错误")
	end
	-- 兑换物
	if data.dest_type == 2 then
		-- 钻石
		local index = Mathf.Clamp(data.dest_level, 0, this.main_mid.diamond_icon_pool.IconArr.Length - 1)
		item.return_image:SetPng(this.main_mid.diamond_icon_pool.IconArr[index])
	elseif data.dest_type == 3 then
		-- 金币
		local index = Mathf.Clamp(data.dest_level, 0, this.main_mid.gold_icon_pool.IconArr.Length - 1)
		item.return_image:SetPng(this.main_mid.gold_icon_pool.IconArr[index])
	else
		Loger.PrintError("错误,商城兑换表配置错误")
	end
	GlobalTimeManager.Instance.timerController:AddTimer("TimeBlendRandom"..data.id, 800*data.id, -1, function()
		item.mall_effect:Play()
	end)
	item.return_btn:AddEventListener(UIEvent.PointerClick,function ()
		local userInfo = PlatformUserProxy:GetInstance():getUserInfo()
		if data.src_type == 1 then -- 使用现金兑换钻石
			if data.src_item > userInfo.cash then
				local tip = string.format("%s","红包余额不足")
				Alert.showAlertMsg(nil,tip,"确定")
			else
				local tip = string.format("<color=#000000FF>%s</color><color=#f34b4bFF>%s</color><color=#000000FF>%s</color><color=#f34b4bFF>%s</color><color=#000000FF>%s</color>"
				,"确定用 ",CS.System.String.Format("{0:0.##}", data.src_item / 100).."元 ", "兑换 ",data.dest_item.."钻石 ", "吗?")
				Alert.showVerifyMsg(nil,tip,"取消", nil,"确定",function ()
					local succeedCallback = nil
					if m_fromType == 0 then
						succeedCallback = function ()
							ViewManager.open(UIViewEnum.Platform_Mall_Tip_View,{enum = 3,successData = data})
						end
					else
						succeedCallback = function ()
							ViewManager.open(UIViewEnum.Platform_Mall_Tip_View,{enum = 3,successData = data})
							m_succeedCallback()
						end
					end
					--兑换
					RechargeManager.exchange("0", LoginDataProxy.token, tostring(data.id), tostring(LoginDataProxy.playerId), succeedCallback)
				end)
			end
		elseif data.src_type == 2 then --使用钻石兑换金币
			if data.src_item > userInfo.diamond then --钻石不足
				local tip = string.format("%s","钻石余额不足")
				Alert.showAlertMsg(nil,tip,"确定")
			else
				local fun = function()
					local succeedCallback = nil
					if m_fromType == 0 then
						succeedCallback = function ()
							ViewManager.open(UIViewEnum.Platform_Mall_Tip_View,{enum = 4,successData = data})
						end
					else
						succeedCallback = function ()
							ViewManager.open(UIViewEnum.Platform_Mall_Tip_View,{enum = 4,successData = data})
							m_succeedCallback()
						end
					end
					--兑换
					RechargeManager.exchange("0", LoginDataProxy.token, tostring(data.id), tostring(LoginDataProxy.playerId), succeedCallback)
				end
				local destNumStr = ""
				if data.dest_item >= 10000 then
					destNumStr = math.floor(data.dest_item / 10000).."万"
				else
					destNumStr = tostring(data.dest_item)
				end
				local tip = string.format("<color=#000000FF>%s</color><color=#f34b4bFF>%s</color><color=#000000FF>%s</color><color=#f34b4bFF>%s</color><color=#000000FF>%s</color>"
				,"确定用 ", data.src_item.."钻石 ", "兑换 ", destNumStr.."金币 ", "吗?")
				Alert.showVerifyMsg(nil,tip, "取消", nil,"确定",fun)
			end
		else
			Loger.PrintError("错误,商城兑换表配置错误")
		end
	end)
end

function this.timeRandom(item, data)

end

--打开界面时初始化
function this:initView()
	this.main_mid.diamond_Panel.gameObject:SetActive(false)
	this.main_mid.gold_Panel.gameObject:SetActive(false)
	this.main_mid.yo_card_Panel.gameObject:SetActive(false)
	--this.main_mid.noClick_Image.gameObject:SetActive(false)
	this.main_mid.top_yo_card_Text.text = "0"
	this.main_mid.top_gold_Text.text = "0"
	this.main_mid.top_diamond_Text.text = "0"

end

--关闭点击遮罩
function this:closeClickImage()
	--this.main_mid.noClick_Image.gameObject:SetActive(false)
end

--价格汉字转换
function this:exchangeNum(Text, num)
	if num == nil or num == "" then return end
	if num >= 10000 then
		Text.text = math.floor(num/10000).."万"
	else
		Text.text = num
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



