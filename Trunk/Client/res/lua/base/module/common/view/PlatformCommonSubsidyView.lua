require "base:enum/UIViewEnum"
require "base:mid/common/Mid_platform_common_subsidy_panel"

PlatformCommonSubsidyView = BaseView:new()
local this = PlatformCommonSubsidyView
this.viewName = "PlatformCommonSubsidyView"

--设置面板特性
this:setViewAttribute(UIViewType.Pop_view, UIViewEnum.Platform_Common_Subsidy_View, false)

--设置加载列表
this.loadOrders = {
    "base:common/platform_common_subsidy_panel"
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName, gameObject)
    
    self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
    UITools.SetParentAndAlign(gameObject, self.container)
    self:addEvent()
end

function this:onShowHandler()
	self:initView()
	self:addNotice()
	this:onSubsidyCountHandler()
	this:updataSubSidyPanel()
	
end

--override 关闭UI回调
function this:onClose()
    self:removeNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_OnlineRedPacketShareCounter, this.onSubsidyCountHandler)
	NoticeManager.Instance:AddNoticeLister(PlatformGlobalNoticeType.Platform_Rsp_Get_Bankruptcy_Subsidy, this.updateSubsidyPanel)
	
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_OnlineRedPacketShareCounter, this.onSubsidyCountHandler)
	NoticeManager.Instance:RemoveNoticeLister(PlatformGlobalNoticeType.Platform_Rsp_Get_Bankruptcy_Subsidy, this.updateSubsidyPanel)
end

function this:addEvent()
    this.main_mid.left_Button:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Common_Subsidy_View)
        end
    )
    this.main_mid.mask_Image:AddEventListener(
        UIEvent.PointerClick,
        function()
            ViewManager.close(UIViewEnum.Platform_Common_Subsidy_View)
        end
    )
    this.main_mid.right_Button:AddEventListener(
        UIEvent.PointerClick,
		function()
			local subsidyCount = PlatformUserProxy:GetInstance():getSubsidyCountDailyCount()
			if subsidyCount > 0 then
				local data = {}
				ViewManager.close(UIViewEnum.Platform_Common_Subsidy_View)	
				PlatformGlobalModule.sendReqGetBankruptcySubsidy(data)
			else
				Alert.showAlertMsg(nil,"今日领取的次数已到达上限!\n(每日00：00点时重置领取次数)"
				,"确定")
			end
        end
	)
	
	this.main_mid.end_mask_Image:AddEventListener(
		UIEvent.PointerClick, 
		function ()
			
		end
	)
end

function this:updataSubSidyPanel()
	printDebug("wahahahhahaha")
	this.main_mid.subsidy_Panel.gameObject:SetActive(true)
    local award_money = TableBaseBankruptcySubsidy.data[1].award_money
	this.main_mid.left_gold_Text.text ="x"..(tonumber(award_money)) 
end

function this.onSubsidyCountHandler()
	if PlatformCommonSubsidyView.isOpen then
		local subsidyCount = PlatformUserProxy:GetInstance():getSubsidyCountDailyCount()
		--printDebug("++++++++++++++++++++我是补助的数量啊"..subsidyCount)
		this.main_mid.number_Text.text = string.format(subsidyCount)
	end
end

function this:updateSubsidyPanel()
	
	--this.main_mid.end_Panel.gameObject:SetActive(true)
	ViewManager.close(UIViewEnum.Platform_Common_Subsidy_View)
	local data = {}
	data.dest_item = TableBaseBankruptcySubsidy.data[1].award_money
	ViewManager.open(UIViewEnum.Platform_Mall_Tip_View,{enum = 4,successData = data})
--	this:onStartTimerGameView()
end

function this:initView()
	this.main_mid.subsidy_Panel.gameObject:SetActive(false)
end

