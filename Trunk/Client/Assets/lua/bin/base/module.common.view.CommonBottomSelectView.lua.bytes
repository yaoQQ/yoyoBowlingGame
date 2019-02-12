

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"

require "base:mid/common/Mid_platform_common_bottom_select_panel"

CommonBottomSelectView = BaseView:new()
local this = CommonBottomSelectView
this.viewName = "CommonBottomSelectView"


--设置面板特性
this:setViewAttribute(UIViewType.Alert_box, UIViewEnum.Common_Bottom_Select_View, false)

--设置加载列表
this.loadOrders =
{
	"base:common/platform_common_bottom_select_panel", 
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()		
end

--override 打开UI回调
function this:onShowHandler(msg)
	--printDebug("PopupWindowView ============> onShowHandler() ")
end

--override 关闭UI回调
function this:onClose()
	this.main_mid.photo_selection_Panel.gameObject:SetActive(false)
end


function this:addEvent()
    self.main_mid.close_photo_Button:AddEventListener(UIEvent.PointerClick,self.onMaskClose)
	self.main_mid.mask_Image:AddEventListener(UIEvent.PointerClick,self.onMaskClose)
end

function this.updateTipsCell(go, info, index)
	this.main_mid.photo_selection_Panel.gameObject:SetActive(false)
	local item = this.main_mid.tipItemArr[index + 1]
	local dataIndex = this.main_mid.tips_items.StartIndex + index
	local data = info
	if dataIndex ==this.methodGroupCount-1 then
		item.Button.Txt.color = CSColor(0.043, 0.71, 0.659)
		this.main_mid.tips_items.gameObject:SetActive(true)
		printDebug(dataIndex)
	else
		item.Button.Txt.color = CSColor(0.31, 0.31, 0.31)
	end
	item.Button.Txt.text=info.title_Text

	local funTemp=function()
		info.funEvent(index)
		--this.closeView()
	end

	item.Button:AddEventListener(UIEvent.PointerClick, funTemp)
end 



--------------------------------------按钮点击事件--------------------------------
--打开界面
function this.onOpenView(action)
    if this.main_mid then
        if not this.main_mid.go.activeSelf then
            ViewManager.open(UIViewEnum.Common_Bottom_Select_View)
        end
        if action then
            action()
        end
    else
	    ViewManager.open(UIViewEnum.Common_Bottom_Select_View, nil, action)
    end
end

function this:updatePhotoWindow(msg)
	self.main_mid.tips_items.gameObject:SetActive(false)
	self.main_mid.photo_selection_Panel.gameObject:SetActive(true)
	self.main_mid.close_Button.gameObject:SetActive(false)

	self.main_mid.two_first_Image:AddEventListener(UIEvent.PointerClick, function()
		ViewManager.close(UIViewEnum.Common_Bottom_Select_View)
		msg.btnFunc1()
	end)
	self.main_mid.two_second_Image:AddEventListener(UIEvent.PointerClick, function()
		ViewManager.close(UIViewEnum.Common_Bottom_Select_View)
		msg.btnFunc2()
	end)
end 


function this.onMaskClose(eventData)
	ViewManager.close(UIViewEnum.Common_Bottom_Select_View)
end


-------------------------------------外部调用------------------------------------
function this:showSelectTipsDownView(methodGroup)
	--printDebug("+++++++++++++++++++++++++我来啦："..table.tostring(methodGroup))
	local fun=function()

		local cancel={title_Text="取消",funEvent=this.onMaskClose}
		table.insert(methodGroup,cancel)--插到最后的一个取消按钮
		self.methodGroupCount=#methodGroup
		self.main_mid.tips_items.transform.sizeDelta = Vector2(0, #methodGroup * 124)--根据列出可选项目多少设置背景
		self.main_mid.tips_items:SetCellData(methodGroup, this.updateTipsCell, true)--刷出button
    end

    this.onOpenView(fun)

end	