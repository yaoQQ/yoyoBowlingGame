require "base:mid/redbag/Mid_platform_redbag_withdraw_panel"

--提现界面
PlatformRedBagWithDrawView= BaseView:new()
local this = PlatformRedBagWithDrawView
this.viewName = "PlatformRedBagWithDrawView"

--设置面板特性（界面层级、界面枚举、是否参与界面堆栈）
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_RedBag_WithDraw_View, true)

--设置加载列表
this.loadOrders=
{
	 "base:redbag/platform_redbag_withdraw_panel",
}

--已选择的提现额度表格信息
local m_selectedInfo = nil
--提现CD
local m_remainTime = -1
--提现类型 0.支付宝 1.微信
local m_withdrawType = -1

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
    UITools.SetParentAndAlign(gameObject, self.container)

    --设置UI中间代码
    self.main_mid = {}
    self:BindMonoTable(gameObject, self.main_mid)
    --添加UI事件监听
    self:addEvent()
end

function this:addEvent()
    self.main_mid.back_Image:AddEventListener(UIEvent.PointerClick,function ()
    	ViewManager.close(UIViewEnum.Platform_RedBag_WithDraw_View)
    end)

    self.main_mid.record_Image:AddEventListener(UIEvent.PointerClick,function ()
    	ViewManager.open(UIViewEnum.Platform_RedBag_WithDraw_Record_View)
    end)

    self.main_mid.rule_Image:AddEventListener(UIEvent.PointerClick,function ()
    	ViewManager.open(UIViewEnum.Platform_RedBag_WithDraw_Rules_View)
    end)
	
	self.main_mid.btn_bind_alipay:AddEventListener(UIEvent.PointerClick, this.onBtnBindAlipay)
	self.main_mid.btn_bind_wx:AddEventListener(UIEvent.PointerClick, this.onBtnBindWx)
	self.main_mid.toggle_alipay:AddEventListener(UIEvent.PointerClick, this.onToggleAlipay)
	self.main_mid.toggle_wx:AddEventListener(UIEvent.PointerClick, this.onToggleWx)
    self.main_mid.withdraw_Button:AddEventListener(UIEvent.PointerClick, this.withdrawBtn)
end

--override 打开UI回调
function this:onShowHandler(msg)
    --打开界面时添加UI通知监听
    self:addNotice()

    --打开界面时初始化，一般用于处理没有数据时的默认的界面显示
    self:initView()
end

--override 关闭UI回调
function this:onClose()
    --关闭界面时移除UI通知监听
    self:removeNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_Cash, this.withdrawShow)
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_BindAcount, this.onUpdateBindAcount)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_Cash, this.withdrawShow)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_BindAcount, this.onUpdateBindAcount)
	
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("RedBagWithDrawTimer")
end

--打开界面时初始化
function this:initView()
	m_withdrawType = -1
	
    local msg = PlatformRedBagProxy:GetInstance():getCDData()
    m_remainTime = msg.remain_time
	
    this.withdrawShow()
	this:updateAccount()
	
    if m_remainTime > 0 then
        local function showRemainTime(m_remainTime)
            local downTimeNew = TimeSpan.FromSeconds(m_remainTime)
            self.main_mid.withdraw_timer_text.text = string.format("%02s:%02s:%02s", downTimeNew.Hours, downTimeNew.Minutes,downTimeNew.Seconds)
        end
        showRemainTime(m_remainTime)
        self.main_mid.withdraw_timer_text.gameObject:SetActive(true)
        GlobalTimeManager.Instance.timerController:AddTimer("RedBagWithDrawTimer", 1000, -1, function ()
            if m_remainTime < 0 then
                return
            end
            m_remainTime = m_remainTime - 1
            if m_remainTime <= 0 then
                self.main_mid.withdraw_timer_text.gameObject:SetActive(false)
                this.withdrawShow()
            else
                showRemainTime(m_remainTime)
            end
        end)
    else
        self.main_mid.withdraw_timer_text.gameObject:SetActive(false)
    end
end

--刷新提现金额
function this.withdrawShow()
    local baseData = PlatformUserProxy:GetInstance():getUserInfo()
    this.main_mid.money_Text.text = baseData.cash / 100
    this:showSelectGroup(baseData.cash)
	this:isWithdrawBtnEvent()
end

function this:showSelectGroup(cash)
    m_selectedInfo = nil
    if cash == nil then
        return
    end
    for i = 1, #this.main_mid.selectItemArr do
        local tableInfo = TableBaseGetmoney.data[i]
        local item = this.main_mid.selectItemArr[i]
        if tableInfo == nil then
            item.go:SetActive(false)
        else
            item.go:SetActive(true)
            local isCanClick = cash >= tableInfo.moneylimit
            if isCanClick and m_remainTime <= 0 then
                item.bg_icon:ChangeIcon(1)
            else
                item.bg_icon:ChangeIcon(0)
            end
            item.selected_tick_toggle.Txt.text = string.format("%s元", math.floor(tableInfo.money/100))
            item.selected_tick_toggle.IsOn = false
            item.selected_bg_toggle.IsOn = false
            item.withdraw_tip_image.gameObject:SetActive(not (i == 1))
            item.bg_icon:AddEventListener(UIEvent.PointerClick, function ()
                if isCanClick == false then
                    return
                end
                if m_remainTime > 0 then
                    showTopTips("账户提现次数还在恢复当中, 请稍候提现")
                    return
                end
                m_selectedInfo = tableInfo
                item.selected_tick_toggle.IsOn = true
                item.selected_bg_toggle.IsOn = true
            end)
            local isShowCondition = false
            item.withdraw_condition_image.gameObject:SetActive(isShowCondition)
            item.withdraw_tip_image:AddEventListener(UIEvent.PointerClick, function ()
                isShowCondition = not isShowCondition
                item.withdraw_condition_image.gameObject:SetActive(isShowCondition)
                item.withdraw_condition_text.text = tableInfo.tips
            end)
        end
    end
end

function this:isWithdrawBtnEvent()
    local baseData = PlatformUserProxy:GetInstance():getUserInfo()
    if baseData.cash/100 < 1 or m_remainTime > 0 then
        this.main_mid.nowithdraw_Image.gameObject:SetActive(true)
        this.main_mid.withdraw_Button.gameObject:SetActive(false)
    else
        this.main_mid.nowithdraw_Image.gameObject:SetActive(false)
        this.main_mid.withdraw_Button.gameObject:SetActive(true)
    end
end

function this.onUpdateBindAcount()
	this:updateAccount()
end

--刷新账户信息
function this:updateAccount()
	local baseData = PlatformUserProxy:GetInstance():getUserInfo()
	if baseData.alipay_nick_name == nil or baseData.alipay_nick_name == "" then
		this.main_mid.btn_bind_alipay.gameObject:SetActive(true)
		this.main_mid.text_name_alipay.gameObject:SetActive(false)
		this.main_mid.toggle_alipay.gameObject:SetActive(false)
	else
		this.main_mid.btn_bind_alipay.gameObject:SetActive(false)
		this.main_mid.text_name_alipay.gameObject:SetActive(true)
		this.main_mid.text_name_alipay.text = baseData.alipay_nick_name
		this.main_mid.toggle_alipay.gameObject:SetActive(true)
		if m_withdrawType == 0 then
			this.main_mid.toggle_alipay.toggle.isOn = true
		else
			this.main_mid.toggle_alipay.toggle.isOn = false
		end
	end
	if baseData.wechat_nick_name == nil or baseData.wechat_nick_name == "" then
		this.main_mid.btn_bind_wx.gameObject:SetActive(true)
		this.main_mid.text_name_wx.gameObject:SetActive(false)
		this.main_mid.toggle_wx.gameObject:SetActive(false)
	else
		this.main_mid.btn_bind_wx.gameObject:SetActive(false)
		this.main_mid.text_name_wx.gameObject:SetActive(true)
		this.main_mid.text_name_wx.text = baseData.wechat_nick_name
		this.main_mid.toggle_wx.gameObject:SetActive(true)
		if m_withdrawType == 1 then
			this.main_mid.toggle_wx.toggle.isOn = true
		else
			this.main_mid.toggle_wx.toggle.isOn = false
		end
	end
end

--绑定支付宝
function this.onBtnBindAlipay()
	PlatformSDK.aliPayBind(function (authCode)
		printDebug("支付宝绑定")
		PlatformUserModule.sendReqBindThirdPartyAccount(0, authCode)
	end, function (errCode)
		if errCode == "4000" then
			showFloatTips("系统异常")
		elseif errCode == "6001" then
			showFloatTips("取消授权")
		elseif errCode == "6002" then
			showFloatTips("网络连接出错")
		else
			showFloatTips("支付宝授权失败:"..errCode)
		end
	end)
end

--绑定微信
function this.onBtnBindWx()
	PlatformSDK.wxBind(function (authCode)
		printDebug("微信绑定")
		PlatformUserModule.sendReqBindThirdPartyAccount(1, authCode)
	end, function (errCode)
		showFloatTips("微信绑定失败"..errCode)
	end)
end

function this.onToggleAlipay()
	if m_withdrawType == 0 then
		return
	end
	m_withdrawType = 0
	this:updateAccount()
end

function this.onToggleWx()
	if m_withdrawType == 1 then
		return
	end
	m_withdrawType = 1
	this:updateAccount()
end

--提现按钮点击响应
function this.withdrawBtn()
    if m_selectedInfo == nil then
        showFloatTips("请选择提现金额")
        return
    end
	
	local baseData = PlatformUserProxy:GetInstance():getUserInfo()
	if m_withdrawType == -1 then
		showFloatTips("请先绑定并选择提现支付宝或微信")
		return
	end
    
	if m_withdrawType == 0 then
		Alert.showVerifyMsg("确定提现到支付宝账户:",this.main_mid.text_name_alipay.text.."?", "取消", nil, "确定", function()
			PlatformUserModule.sendReqGetMoney(ProtoEnumCommon.PayType.PayType_AliPay, m_selectedInfo.id)
		end)
	elseif m_withdrawType == 1 then
		Alert.showVerifyMsg("确定提现到微信账户:",this.main_mid.text_name_wx.text.."?", "取消", nil, "确定", function()
			PlatformUserModule.sendReqGetMoney(ProtoEnumCommon.PayType.PayType_WeChatPay, m_selectedInfo.id)
		end)
	end
end