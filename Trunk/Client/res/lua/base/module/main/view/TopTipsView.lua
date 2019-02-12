

require "base:enum/UIViewEnum"
require "base:mid/common/Mid_common_top_tips_panel"

TopTipsView = BaseView:new()
local this = TopTipsView
this.viewName = "TopTipsView"

--设置面板特性
this:setViewAttribute(UIViewType.Feedback_Tip, UIViewEnum.TopTips_View, false)

--设置加载列表
this.loadOrders=
{
	"base:common/common_top_tips_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

function this:addEvent()

end

--override 打开UI回调
function this:onShowHandler(msg)
	printDebug("=====================TopTipsView调用完毕======================")

	local go = self:getViewGO()
	if go == nil then return end
	go.transform:SetAsLastSibling()
	
end

--override 关闭UI回调
function this:onClose()	
    
end

--------------------------------------设置数据------------------------------------


--------------------------------------按钮点击事件--------------------------------
--打开界面
function this.onOpenView(action)
    if this.main_mid then
        if not this.main_mid.go.activeSelf then
            ViewManager.open(UIViewEnum.TopTips_View)
        end
        action()
    else
	    ViewManager.open(UIViewEnum.TopTips_View, nil, action)
    end
end
--隐藏界面
function this.onHideView(eventData)
    if this.main_mid then
	    ViewManager.close(UIViewEnum.TopTips_View)
    end
end

-------------------------------------外部调用------------------------------------

function this:onShowTopTips(notice)
    local fun = function()       
        if notice then
            local tempImageRect = this.main_mid.topTips_bg_Image.transform:GetComponent(typeof(RectTransform))
            this.main_mid.topTips_Text.text = tostring(notice)
            local tempwidth =this.main_mid.topTips_Text.Txt.preferredWidth+150
            local tempHeight =165
            tempImageRect.sizeDelta = Vector2(tempwidth,tempHeight)
        end
        GlobalTimeManager.Instance.timerController:AddTimer(this.topTipsTimer, 1500, 1, this.onHideView)
    end
    if not this.topTipsTimer then
        this.topTipsTimer = "TopTipsTimer"
    end
    if not GlobalTimeManager.Instance.timerController:CheckExistByKey(this.topTipsTimer) then
        GlobalTimeManager.Instance.timerController:RemoveTimerByKey(this.topTipsTimer)
        this.onHideView()
    end
    this.onOpenView(fun)
end

-------------------------------------辅助方法------------------------------------
