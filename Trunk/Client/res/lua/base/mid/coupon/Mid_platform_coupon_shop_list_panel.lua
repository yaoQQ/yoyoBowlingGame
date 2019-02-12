local EmptyImageWidget=CS.EmptyImageWidget
local TextWidget=CS.TextWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local ImageWidget=CS.ImageWidget

Mid_platform_coupon_shop_list_panel={}
local this = Mid_platform_coupon_shop_list_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.back_Image=nil
this.how_many_Text=nil
this.shop_CellRecycleScrollPanel=nil
--ShopCell数组
this.shopCellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.back_Image=self.go.transform:Find("back_Image/back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.how_many_Text=self.go.transform:Find("how_many_Text"):GetComponent(typeof(TextWidget))
	self.shop_CellRecycleScrollPanel=self.go.transform:Find("shop_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.shopCellArr={}
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("shop_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("shop_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("shop_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("shop_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("shop_CellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("shop_CellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("shop_CellRecycleScrollPanel/content/cellitem_6").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("shop_CellRecycleScrollPanel/content/cellitem_7").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("shop_CellRecycleScrollPanel/content/cellitem_8").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("shop_CellRecycleScrollPanel/content/cellitem_9").gameObject))
end

--ShopCell复用单元
function this.new_ShopCell(itemGo)
	local item = { }
	item.go = itemGo
	item.press_Image=itemGo.transform:Find("press_Image"):GetComponent(typeof(ImageWidget))
	item.shop_name_Text=itemGo.transform:Find("shop_name_Text"):GetComponent(typeof(TextWidget))
	item.shop_add_Text=itemGo.transform:Find("shop_add_Text"):GetComponent(typeof(TextWidget))
	item.distance_Text=itemGo.transform:Find("distance_Text"):GetComponent(typeof(TextWidget))
	item.navi_Image=itemGo.transform:Find("navi_Image"):GetComponent(typeof(ImageWidget))
	return item
end

