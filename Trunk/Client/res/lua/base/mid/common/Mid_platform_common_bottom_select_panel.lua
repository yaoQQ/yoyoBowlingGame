local ImageWidget=CS.ImageWidget
local PanelWidget=CS.PanelWidget
local ButtonWidget=CS.ButtonWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget

Mid_platform_common_bottom_select_panel={}
local this = Mid_platform_common_bottom_select_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.mask_Image=nil
this.photo_selection_Panel=nil
this.two_first_Image=nil
this.two_second_Image=nil
this.close_photo_Button=nil
this.close_Button=nil
this.tips_items=nil
this.content_Panel=nil
--TipItem数组
this.tipItemArr={}

function this:init(gameObject)
	self.go=gameObject
	self.mask_Image=self.go.transform:Find("mask_Image"):GetComponent(typeof(ImageWidget))
	self.photo_selection_Panel=self.go.transform:Find("photo_selection_Panel"):GetComponent(typeof(PanelWidget))
	self.two_first_Image=self.go.transform:Find("photo_selection_Panel/two_first_Image"):GetComponent(typeof(ImageWidget))
	self.two_second_Image=self.go.transform:Find("photo_selection_Panel/two_second_Image"):GetComponent(typeof(ImageWidget))
	self.close_photo_Button=self.go.transform:Find("photo_selection_Panel/close_photo_Button"):GetComponent(typeof(ButtonWidget))
	self.close_Button=self.go.transform:Find("close_Button"):GetComponent(typeof(ButtonWidget))
	self.tips_items=self.go.transform:Find("selct_tips_Panel/tips_items"):GetComponent(typeof(CellRecycleScrollWidget))
	self.content_Panel=self.go.transform:Find("selct_tips_Panel/tips_items/content_Panel"):GetComponent(typeof(PanelWidget))
	self.tipItemArr={}
	table.insert(self.tipItemArr,self.new_TipItem(self.go.transform:Find("selct_tips_Panel/tips_items/content_Panel/tipcellitem").gameObject))
	table.insert(self.tipItemArr,self.new_TipItem(self.go.transform:Find("selct_tips_Panel/tips_items/content_Panel/tipcellitem_1").gameObject))
	table.insert(self.tipItemArr,self.new_TipItem(self.go.transform:Find("selct_tips_Panel/tips_items/content_Panel/tipcellitem_2").gameObject))
	table.insert(self.tipItemArr,self.new_TipItem(self.go.transform:Find("selct_tips_Panel/tips_items/content_Panel/tipcellitem_3").gameObject))
	table.insert(self.tipItemArr,self.new_TipItem(self.go.transform:Find("selct_tips_Panel/tips_items/content_Panel/tipcellitem_4").gameObject))
	table.insert(self.tipItemArr,self.new_TipItem(self.go.transform:Find("selct_tips_Panel/tips_items/content_Panel/tipcellitem_5").gameObject))
	table.insert(self.tipItemArr,self.new_TipItem(self.go.transform:Find("selct_tips_Panel/tips_items/content_Panel/tipcellitem_6").gameObject))
	table.insert(self.tipItemArr,self.new_TipItem(self.go.transform:Find("selct_tips_Panel/tips_items/content_Panel/tipcellitem_7").gameObject))
end

--TipItem复用单元
function this.new_TipItem(itemGo)
	local item = { }
	item.go = itemGo
	item.Button=itemGo.transform:Find("Button"):GetComponent(typeof(ButtonWidget))
	return item
end

