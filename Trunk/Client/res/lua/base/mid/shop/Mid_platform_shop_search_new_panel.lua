local PanelWidget=CS.PanelWidget
local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget
local InputFieldWidget=CS.InputFieldWidget
local EmptyImageWidget=CS.EmptyImageWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget

Mid_platform_shop_search_new_panel={}
local this = Mid_platform_shop_search_new_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.second_Panel=nil
this.show_Image=nil
this.show_Text=nil
this.bg_images=nil
this.cancel_Button=nil
this.search_Inputfield=nil
this.hot_first_Panel=nil
this.clear_Image=nil
this.lately_first_Panel=nil
this.enterActivity=nil
this.activity_count_text=nil
this.mid_scratch_Panel=nil
this.Parent=nil
this.shop_count_do_button=nil
this.shop_activitylist=nil
this.shop_list_CellRecycleScrollPanel=nil
this.nothing_Text=nil
--Shopactivitylistcell数组
this.shopactivitylistcellArr={}
--Shoplistcell数组
this.shoplistcellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.second_Panel=self.go.transform:Find("second_Panel"):GetComponent(typeof(PanelWidget))
	self.show_Image=self.go.transform:Find("second_Panel/show_Image"):GetComponent(typeof(ImageWidget))
	self.show_Text=self.go.transform:Find("second_Panel/show_Image/show_Text"):GetComponent(typeof(TextWidget))
	self.bg_images=self.go.transform:Find("top_Panel/bg_images"):GetComponent(typeof(ImageWidget))
	self.cancel_Button=self.go.transform:Find("top_Panel/cancel_Button"):GetComponent(typeof(ButtonWidget))
	self.search_Inputfield=self.go.transform:Find("top_Panel/search_Inputfield"):GetComponent(typeof(InputFieldWidget))
	self.hot_first_Panel=self.go.transform:Find("hot_Panel/hot_show_Panel/hot_first_Panel"):GetComponent(typeof(PanelWidget))
	self.clear_Image=self.go.transform:Find("lately_Panel/clear_Image"):GetComponent(typeof(EmptyImageWidget))
	self.lately_first_Panel=self.go.transform:Find("lately_Panel/lately_show_Panel/lately_first_Panel"):GetComponent(typeof(PanelWidget))
	self.enterActivity=self.go.transform:Find("enterActivity"):GetComponent(typeof(ButtonWidget))
	self.activity_count_text=self.go.transform:Find("enterActivity/activity_count_text"):GetComponent(typeof(TextWidget))
	self.mid_scratch_Panel=self.go.transform:Find("mid_scratch_Panel"):GetComponent(typeof(PanelWidget))
	self.Parent=self.go.transform:Find("mid_scratch_Panel/Parent"):GetComponent(typeof(ImageWidget))
	self.shop_count_do_button=self.go.transform:Find("mid_scratch_Panel/Parent/shop_count_do_button"):GetComponent(typeof(ButtonWidget))
	self.shop_activitylist=self.go.transform:Find("mid_scratch_Panel/Parent/shop_activitylist"):GetComponent(typeof(CellRecycleScrollWidget))
	self.shop_list_CellRecycleScrollPanel=self.go.transform:Find("mid_scratch_Panel/Parent/shop_list_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.nothing_Text=self.go.transform:Find("mid_scratch_Panel/nothing_Text"):GetComponent(typeof(TextWidget))
	self.shopactivitylistcellArr={}
	table.insert(self.shopactivitylistcellArr,self.new_Shopactivitylistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_activitylist/content/cellitem").gameObject))
	table.insert(self.shopactivitylistcellArr,self.new_Shopactivitylistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_activitylist/content/cellitem_1").gameObject))
	table.insert(self.shopactivitylistcellArr,self.new_Shopactivitylistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_activitylist/content/cellitem_2").gameObject))
	table.insert(self.shopactivitylistcellArr,self.new_Shopactivitylistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_activitylist/content/cellitem_3").gameObject))
	table.insert(self.shopactivitylistcellArr,self.new_Shopactivitylistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_activitylist/content/cellitem_4").gameObject))
	table.insert(self.shopactivitylistcellArr,self.new_Shopactivitylistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_activitylist/content/cellitem_5").gameObject))
	table.insert(self.shopactivitylistcellArr,self.new_Shopactivitylistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_activitylist/content/cellitem_6").gameObject))
	table.insert(self.shopactivitylistcellArr,self.new_Shopactivitylistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_activitylist/content/cellitem_7").gameObject))
	table.insert(self.shopactivitylistcellArr,self.new_Shopactivitylistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_activitylist/content/cellitem_8").gameObject))
	self.shoplistcellArr={}
	table.insert(self.shoplistcellArr,self.new_Shoplistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_list_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.shoplistcellArr,self.new_Shoplistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_list_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.shoplistcellArr,self.new_Shoplistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_list_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.shoplistcellArr,self.new_Shoplistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_list_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.shoplistcellArr,self.new_Shoplistcell(self.go.transform:Find("mid_scratch_Panel/Parent/shop_list_CellRecycleScrollPanel/content/cellitem_4").gameObject))
end

--Shopactivitylistcell复用单元
function this.new_Shopactivitylistcell(itemGo)
	local item = { }
	item.go = itemGo
	return item
end
--Shoplistcell复用单元
function this.new_Shoplistcell(itemGo)
	local item = { }
	item.go = itemGo
	item.group_Image=itemGo.transform:Find("group_Image"):GetComponent(typeof(ImageWidget))
	return item
end

