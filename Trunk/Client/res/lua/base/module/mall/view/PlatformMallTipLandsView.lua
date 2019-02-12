require "base:mid/mall/Mid_platform_mall_tip_lands_panel"
require "base:module/mall/data/PlatformMallProxy"

--商城特效界面
PlatformMallTipLandsView = BaseView:new()
local this = PlatformMallTipLandsView
this.viewName = "PlatformMallTipLandsView"

--设置面板特性（界面层级、界面枚举、是否参与界面堆栈）
this:setViewAttribute(UIViewType.Feedback_Tip, UIViewEnum.Platform_Mall_Tip_Lands_View, false)

--设置加载列表
this.loadOrders =
{
	"base:mall/platform_mall_tip_lands_panel",
}

local m_isShowView = false
--是否可以开始飞图标
local m_isCanStartIconFly = false
--是否已经开始飞图标
local m_isStartIconFly = false
local m_isDiamond = false
local m_iconCount = 0
local m_iconDiamondList = {}
local m_iconGoldList = {}
local m_countNum = 0

--override 加载UI完成回调
function this:onLoadUIEnd(uiName,gameObject)
	--下面两行默认需要调用
	
	UITools.SetParentAndAlign(gameObject, self.container)
	
	--设置UI中间代码
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	
	--添加UI事件监听
	self:addEvent()
end

function this:addEvent()
	this.main_mid.mask_Image:AddEventListener(UIEvent.PointerClick, function ()
		if m_isCanStartIconFly and not m_isStartIconFly then
			GlobalTimeManager.Instance.timerController:RemoveTimerByKey("effectDOMoveTime")
			this.iconFly()
		end
	end)
end

--override 打开UI回调
function this:onShowHandler(msg)
	if m_isShowView then
		return
	end
	self:initView()
	m_isShowView = true
	m_isCanStartIconFly = false
	m_isStartIconFly = false
	
	local tipEnum = msg.enum
	this:updateTipEnum(tipEnum, msg.successData)
end

--override 关闭UI回调
function this:onClose()	
	m_isShowView = false
	self.main_mid.success_Panel.gameObject:SetActive(false)
	self.main_mid.platform_mall_success_panel.gameObject:SetActive(false)
	--PlatformMallView:closeClickImage()
	GlobalTimeManager.Instance.timerController:RemoveTimer("effectDOScaleLandsTime")
end

--更新提示界面
function this:updateTipEnum(tipEnum, data)
	if this.tipText ~= nil then
		this.tipText.gameObject:SetActive(false)
	end

	if tipEnum == 1 then 
		--this.main_mid.bg_image.gameObject:SetActive(true)
		this.main_mid.platform_mall_success_panel.gameObject:SetActive(false)
		this.tipText = this.main_mid.cost_que_Text
	elseif tipEnum == 2 then
		--this.main_mid.bg_image.gameObject:SetActive(true)
		this.main_mid.platform_mall_success_panel.gameObject:SetActive(false)
		this.tipText = this.main_mid.cost_exchange_Text
	elseif tipEnum == 3 then
		m_isDiamond = true
		--this.main_mid.bg_image.gameObject:SetActive(false)
	
		PlatformMallView:exchangeNum(this.main_mid.show_Text, data.dest_item)
		--this.main_mid.show_Text.text="钻石"
		this:onStartTimerGameView()

		--this.tipText=this.main_mid.diamond_cost_end_Text
	elseif tipEnum == 4 then
		m_isDiamond = false
		--this.main_mid.bg_image.gameObject:SetActive(false)
		
		PlatformMallView:exchangeNum(this.main_mid.show_Text, data.dest_item)
		--this.main_mid.show_Text.text="金币"
		this:onStartTimerGameView()
	else
		printDebug("++++++++++++++++未知选项")	
	end
	--this.tipText.gameObject:SetActive(true)
	--[[local fun=tipFun
	this.main_mid.go_Button:AddEventListener(UIEvent.PointerClick,function ()
		--ViewManager.close(UIViewEnum.Platform_Mall_Tip_View)
		--fun()
	end)--]]
end

function this:onStartTimerGameView()
	m_countNum = PlatformMallProxy.getCountNum()
	this.main_mid.platform_mall_success_panel.gameObject:SetActive(true)
	this.main_mid.successeffect:Play()
	local fun = function ()

		local curObj =this.main_mid.success_Panel
		curObj.gameObject:SetActive(true)

	    local mySequence = DOTween.Sequence()
	    curObj.transform.localScale = Vector3(0.1, 0.1, 1)
	    --local scale_1 = curObj.transform:DOScale(Vector3.one * 1.2, 0.3)
	    local scale_2 = curObj.transform:DOScale(Vector3.one, 0.15)
	    --mySequence:Append(scale_1)
	    mySequence:Append(scale_2)
	    if m_isDiamond then 
		    this.main_mid.success_diamond_Image.gameObject:SetActive(true)
		else
			this.main_mid.success_gold_Image.gameObject:SetActive(true)
		end

	    this:doMoveEvent()
	end
	if m_countNum == 0 then
		GlobalTimeManager.Instance.timerController:AddTimer(
        "effectDOScaleTime",
        750,
        1,
        fun)
        PlatformMallProxy.setCountNum()
	else
		fun()
	end
end

function this.iconFly()
	this.main_mid.success_diamond_Image.gameObject:SetActive(false)
	this.main_mid.success_gold_Image.gameObject:SetActive(false)
	this.main_mid.success_Panel.gameObject:SetActive(false)
	this.main_mid.successeffect:Stop()
	
	if m_isDiamond then
		NoticeManager.Instance:Dispatch(NoticeType.Mall_Update_Diamond)
	else
		NoticeManager.Instance:Dispatch(NoticeType.Mall_Update_Money)
	end
	
	m_isStartIconFly = true
	m_iconCount = 10
	local isFirst = true
	GlobalTimeManager.Instance.timerController:AddTimer(
		"PlatformMallTipView_iconFly",
		100,
		10,
		function()
			local iconGo = nil
			local iconTrans = nil
			if m_isDiamond then
				local len = #m_iconDiamondList
				if len > 0 then
					iconGo = m_iconDiamondList[len]
					m_iconDiamondList[len] = nil
				else
					iconGo = GameObject.Instantiate(this.main_mid.success_diamond_Image.gameObject)
				end
			else
				local len = #m_iconGoldList
				if len > 0 then
					iconGo = m_iconGoldList[len]
					m_iconGoldList[len] = nil
				else
					iconGo = GameObject.Instantiate(this.main_mid.success_gold_Image.gameObject)
				end
				
			end
			
			iconTrans = iconGo.transform
			iconTrans:SetParent(this.main_mid.platform_mall_success_panel.transform)
			iconTrans.localPosition = Vector3(0,0,0)
			iconTrans.localScale = Vector3.one
			--设置到最下层
			iconTrans:SetAsFirstSibling()
	
			iconGo:SetActive(true)
			local imageWidget = iconTrans:GetComponent(typeof(ImageWidget))
			if isFirst then
				imageWidget.Img.color = CSColor(1, 1, 1, 1)
				isFirst = false
			else
				imageWidget.Img.color = CSColor(1, 1, 1, 0.7)
			end
			
			local mySequence1 = DOTween.Sequence();
			local cRectm1 =  this.main_mid.point_Image.gameObject:GetComponent(typeof(RectTransform))
			local cRectm2 =  iconGo:GetComponent(typeof(RectTransform))
			if m_isDiamond then 
				move1 = iconTrans:DOLocalMove(Vector3(-512,cRectm1.localPosition.y-10,0), 0.5)
			else				
			   -- Rectm2.anchoredPosition = Vector2(0, cRectm2.localPosition.y-1050)
				move1 = iconTrans:DOLocalMove(Vector3(-114,cRectm1.localPosition.y-10,0), 0.5)
			end	
			move2=iconTrans:DOScale(Vector3.one*0.5, 0.07)
			this.main_mid.mask_Image.gameObject:SetActive(false)
			mySequence1:Append(move2)
			mySequence1:Append(move1)
			mySequence1:AppendCallback(function ()
				iconGo:SetActive(false)
				if m_isDiamond then
					m_iconDiamondList[#m_iconDiamondList + 1] = iconGo
				else
					m_iconGoldList[#m_iconGoldList + 1] = iconGo
				end
				m_iconCount = m_iconCount - 1
				if m_iconCount <= 0 then
					ViewManager.close(UIViewEnum.Platform_Mall_Tip_Lands_View)
				end
			end)
		end)
end
	
function this:doMoveEvent()
	m_isCanStartIconFly = true
	
	GlobalTimeManager.Instance.timerController:AddTimer(
		"effectDOMoveTime",
		3000,
		1,
		function(...)
			this.iconFly()
		end)
end



function this.onSetDiamond(go,data,index)


end

function this.onSetGold(go,data,index)


end

--打开界面时初始化
function this:initView()
	self.main_mid.success_gold_Image.transform.localPosition = Vector3(0,0,0)
	self.main_mid.success_gold_Image.transform.localScale=Vector3.one
	self.main_mid.success_gold_Image.gameObject:SetActive(false)
	self.main_mid.success_diamond_Image.transform.localPosition = Vector3(0,0,0)
	self.main_mid.success_diamond_Image.transform.localScale=Vector3.one
	self.main_mid.success_diamond_Image.gameObject:SetActive(false)
	self.main_mid.success_Panel.gameObject:SetActive(false)
	this.main_mid.mask_Image.gameObject:SetActive(true)

end



