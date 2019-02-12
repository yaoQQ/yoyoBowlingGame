local EmptyImageWidget=CS.EmptyImageWidget
local TextWidget=CS.TextWidget
local ImageWidget=CS.ImageWidget
local CircleImageWidget=CS.CircleImageWidget
local ButtonWidget=CS.ButtonWidget
local ToggleWidget=CS.ToggleWidget
local ScrollPanelWidget=CS.ScrollPanelWidget
local CellGroupWidget=CS.CellGroupWidget

Mid_platform_lbs_redpacket_detail_panel={}
local this = Mid_platform_lbs_redpacket_detail_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.back_image=nil
this.bag_type_text=nil
this.remain_time_text=nil
this.shop_name_text=nil
this.official_image=nil
this.shop_head_image=nil
this.money_text=nil
this.shop_main_page_btn=nil
this.get_toggle=nil
this.intro_scroll_panel=nil
this.intro_text=nil
this.cellgroup=nil
this.single_image=nil
this.single_publicity_image=nil
this.bag_title_text=nil
--PublicityItem数组
this.publicityItemArr={}

function this:init(gameObject)
	self.go=gameObject
	self.back_image=self.go.transform:Find("back_image"):GetComponent(typeof(EmptyImageWidget))
	self.bag_type_text=self.go.transform:Find("bag_type_text"):GetComponent(typeof(TextWidget))
	self.remain_time_text=self.go.transform:Find("remain_time_text"):GetComponent(typeof(TextWidget))
	self.shop_name_text=self.go.transform:Find("shop_name_text"):GetComponent(typeof(TextWidget))
	self.official_image=self.go.transform:Find("shop_name_text/official_image"):GetComponent(typeof(ImageWidget))
	self.shop_head_image=self.go.transform:Find("shadow_image/shop_head_image"):GetComponent(typeof(CircleImageWidget))
	self.money_text=self.go.transform:Find("money_bg/money_text"):GetComponent(typeof(TextWidget))
	self.shop_main_page_btn=self.go.transform:Find("shop_main_page_btn"):GetComponent(typeof(ButtonWidget))
	self.get_toggle=self.go.transform:Find("get_toggle"):GetComponent(typeof(ToggleWidget))
	self.intro_scroll_panel=self.go.transform:Find("intro_scroll_panel"):GetComponent(typeof(ScrollPanelWidget))
	self.intro_text=self.go.transform:Find("intro_scroll_panel/content/intro_text"):GetComponent(typeof(TextWidget))
	self.cellgroup=self.go.transform:Find("intro_scroll_panel/content/cellgroup"):GetComponent(typeof(CellGroupWidget))
	self.single_image=self.go.transform:Find("intro_scroll_panel/content/single_image"):GetComponent(typeof(ImageWidget))
	self.single_publicity_image=self.go.transform:Find("intro_scroll_panel/content/single_image/single_publicity_image"):GetComponent(typeof(ImageWidget))
	self.bag_title_text=self.go.transform:Find("bag_title_text"):GetComponent(typeof(TextWidget))
	self.publicityItemArr={}
	table.insert(self.publicityItemArr,self.new_PublicityItem(self.go.transform:Find("intro_scroll_panel/content/cellgroup/CellItem").gameObject))
	table.insert(self.publicityItemArr,self.new_PublicityItem(self.go.transform:Find("intro_scroll_panel/content/cellgroup/CellItem_1").gameObject))
	table.insert(self.publicityItemArr,self.new_PublicityItem(self.go.transform:Find("intro_scroll_panel/content/cellgroup/CellItem_2").gameObject))
	table.insert(self.publicityItemArr,self.new_PublicityItem(self.go.transform:Find("intro_scroll_panel/content/cellgroup/CellItem_3").gameObject))
	table.insert(self.publicityItemArr,self.new_PublicityItem(self.go.transform:Find("intro_scroll_panel/content/cellgroup/CellItem_4").gameObject))
	table.insert(self.publicityItemArr,self.new_PublicityItem(self.go.transform:Find("intro_scroll_panel/content/cellgroup/CellItem_5").gameObject))
	table.insert(self.publicityItemArr,self.new_PublicityItem(self.go.transform:Find("intro_scroll_panel/content/cellgroup/CellItem_6").gameObject))
	table.insert(self.publicityItemArr,self.new_PublicityItem(self.go.transform:Find("intro_scroll_panel/content/cellgroup/CellItem_7").gameObject))
	table.insert(self.publicityItemArr,self.new_PublicityItem(self.go.transform:Find("intro_scroll_panel/content/cellgroup/CellItem_8").gameObject))
end

--PublicityItem复用单元
function this.new_PublicityItem(itemGo)
	local item = { }
	item.go = itemGo
	item.publicity_image=itemGo.transform:Find("publicity_image"):GetComponent(typeof(ImageWidget))
	return item
end

