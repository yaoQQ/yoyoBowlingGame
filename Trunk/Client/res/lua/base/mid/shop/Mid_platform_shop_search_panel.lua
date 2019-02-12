local EmptyImageWidget=CS.EmptyImageWidget
local PanelWidget=CS.PanelWidget
local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget
local InputFieldWidget=CS.InputFieldWidget
local HorizontalLayoutGroupWidget=CS.HorizontalLayoutGroupWidget
local GridLayoutGroupWidget=CS.GridLayoutGroupWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget

Mid_platform_shop_search_panel={}
local this = Mid_platform_shop_search_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.close_Image=nil
this.activity_Image=nil
this.second_Panel=nil
this.show_Image=nil
this.show_Text=nil
this.mid_Panel=nil
this.hot_Panel=nil
this.hot_first_Panel=nil
this.lately_Panel=nil
this.clear_Image=nil
this.lately_first_Panel=nil
this.bottom_Image=nil
this.bg_images=nil
this.city_Button=nil
this.city_arrow=nil
this.cancel_Button=nil
this.search_Button=nil
this.search_Inputfield=nil
this.clear_search_Image=nil
this.cityPanel=nil
this.cityItem=nil
this.Normalcitybg=nil
this.Normalcityname=nil
this.Choosecitybg=nil
this.Choosecityname=nil
this.Citypressimage=nil
this.historyCityGroup=nil
this.HotCityGroup=nil
this.search_end_Panel=nil
this. result_Text=nil
this.shop_Button=nil
this.mid_scratch_Panel=nil
this.bg5=nil
this.shop_count_do_Button=nil
this.shop_list_CellRecycleScrollPanel=nil
this.nothing_Text=nil
--ShopCell数组
this.shopCellArr={}
--PlaceCell数组
this.placeCellArr={}
--Shoplistcell数组
this.shoplistcellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.close_Image=self.go.transform:Find("close_Image"):GetComponent(typeof(EmptyImageWidget))
	self.activity_Image=self.go.transform:Find("activity_Image"):GetComponent(typeof(EmptyImageWidget))
	self.second_Panel=self.go.transform:Find("second_Panel"):GetComponent(typeof(PanelWidget))
	self.show_Image=self.go.transform:Find("second_Panel/show_Image"):GetComponent(typeof(ImageWidget))
	self.show_Text=self.go.transform:Find("second_Panel/show_Image/show_Text"):GetComponent(typeof(TextWidget))
	self.mid_Panel=self.go.transform:Find("mid_Panel"):GetComponent(typeof(PanelWidget))
	self.hot_Panel=self.go.transform:Find("mid_Panel/hot_Panel"):GetComponent(typeof(PanelWidget))
	self.hot_first_Panel=self.go.transform:Find("mid_Panel/hot_Panel/hot_show_Panel/hot_first_Panel"):GetComponent(typeof(PanelWidget))
	self.lately_Panel=self.go.transform:Find("mid_Panel/lately_Panel"):GetComponent(typeof(PanelWidget))
	self.clear_Image=self.go.transform:Find("mid_Panel/lately_Panel/clear_Image"):GetComponent(typeof(EmptyImageWidget))
	self.lately_first_Panel=self.go.transform:Find("mid_Panel/lately_Panel/lately_show_Panel/lately_first_Panel"):GetComponent(typeof(PanelWidget))
	self.bottom_Image=self.go.transform:Find("mid_Panel/bottom_Image"):GetComponent(typeof(ImageWidget))
	self.bg_images=self.go.transform:Find("top_Panel/bg_images"):GetComponent(typeof(ImageWidget))
	self.city_Button=self.go.transform:Find("top_Panel/city_Button"):GetComponent(typeof(ButtonWidget))
	self.city_arrow=self.go.transform:Find("top_Panel/city_Button/city_arrow"):GetComponent(typeof(ImageWidget))
	self.cancel_Button=self.go.transform:Find("top_Panel/cancel_Button"):GetComponent(typeof(ButtonWidget))
	self.search_Button=self.go.transform:Find("top_Panel/search_Button"):GetComponent(typeof(ButtonWidget))
	self.search_Inputfield=self.go.transform:Find("top_Panel/search_Inputfield"):GetComponent(typeof(InputFieldWidget))
	self.clear_search_Image=self.go.transform:Find("top_Panel/clear_search_Image"):GetComponent(typeof(EmptyImageWidget))
	self.cityPanel=self.go.transform:Find("cityPanel"):GetComponent(typeof(PanelWidget))
	self.cityItem=self.go.transform:Find("cityPanel/cityItem"):GetComponent(typeof(EmptyImageWidget))
	self.Normalcitybg=self.go.transform:Find("cityPanel/cityItem/Normalcitybg"):GetComponent(typeof(ImageWidget))
	self.Normalcityname=self.go.transform:Find("cityPanel/cityItem/Normalcitybg/Normalcityname"):GetComponent(typeof(TextWidget))
	self.Choosecitybg=self.go.transform:Find("cityPanel/cityItem/Choosecitybg"):GetComponent(typeof(ImageWidget))
	self.Choosecityname=self.go.transform:Find("cityPanel/cityItem/Choosecitybg/Choosecityname"):GetComponent(typeof(TextWidget))
	self.Citypressimage=self.go.transform:Find("cityPanel/cityItem/Citypressimage"):GetComponent(typeof(EmptyImageWidget))
	self.historyCityGroup=self.go.transform:Find("cityPanel/historyCityGroup"):GetComponent(typeof(HorizontalLayoutGroupWidget))
	self.HotCityGroup=self.go.transform:Find("cityPanel/HotCityGroup"):GetComponent(typeof(GridLayoutGroupWidget))
	self.search_end_Panel=self.go.transform:Find("search_end_Panel"):GetComponent(typeof(PanelWidget))
	self. result_Text=self.go.transform:Find("search_end_Panel/ result_Text"):GetComponent(typeof(TextWidget))
	self.shop_Button=self.go.transform:Find("search_end_Panel/shop_Button"):GetComponent(typeof(EmptyImageWidget))
	self.mid_scratch_Panel=self.go.transform:Find("mid_scratch_Panel"):GetComponent(typeof(PanelWidget))
	self.bg5=self.go.transform:Find("mid_scratch_Panel/bg5"):GetComponent(typeof(ImageWidget))
	self.shop_count_do_Button=self.go.transform:Find("mid_scratch_Panel/shop_count_do_Button"):GetComponent(typeof(ButtonWidget))
	self.shop_list_CellRecycleScrollPanel=self.go.transform:Find("mid_scratch_Panel/shop_list_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.nothing_Text=self.go.transform:Find("nothing_Text"):GetComponent(typeof(TextWidget))
	self.shopCellArr={}
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("search_end_Panel/shop_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("search_end_Panel/shop_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("search_end_Panel/shop_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("search_end_Panel/shop_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("search_end_Panel/shop_CellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("search_end_Panel/shop_CellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("search_end_Panel/shop_CellRecycleScrollPanel/content/cellitem_6").gameObject))
	table.insert(self.shopCellArr,self.new_ShopCell(self.go.transform:Find("search_end_Panel/shop_CellRecycleScrollPanel/content/cellitem_7").gameObject))
	self.placeCellArr={}
	table.insert(self.placeCellArr,self.new_PlaceCell(self.go.transform:Find("search_end_Panel/place_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.placeCellArr,self.new_PlaceCell(self.go.transform:Find("search_end_Panel/place_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.placeCellArr,self.new_PlaceCell(self.go.transform:Find("search_end_Panel/place_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.placeCellArr,self.new_PlaceCell(self.go.transform:Find("search_end_Panel/place_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.placeCellArr,self.new_PlaceCell(self.go.transform:Find("search_end_Panel/place_CellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.placeCellArr,self.new_PlaceCell(self.go.transform:Find("search_end_Panel/place_CellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.placeCellArr,self.new_PlaceCell(self.go.transform:Find("search_end_Panel/place_CellRecycleScrollPanel/content/cellitem_6").gameObject))
	table.insert(self.placeCellArr,self.new_PlaceCell(self.go.transform:Find("search_end_Panel/place_CellRecycleScrollPanel/content/cellitem_7").gameObject))
	table.insert(self.placeCellArr,self.new_PlaceCell(self.go.transform:Find("search_end_Panel/place_CellRecycleScrollPanel/content/cellitem_8").gameObject))
	table.insert(self.placeCellArr,self.new_PlaceCell(self.go.transform:Find("search_end_Panel/place_CellRecycleScrollPanel/content/cellitem_9").gameObject))
	table.insert(self.placeCellArr,self.new_PlaceCell(self.go.transform:Find("search_end_Panel/place_CellRecycleScrollPanel/content/cellitem_10").gameObject))
	table.insert(self.placeCellArr,self.new_PlaceCell(self.go.transform:Find("search_end_Panel/place_CellRecycleScrollPanel/content/cellitem_11").gameObject))
	self.shoplistcellArr={}
	table.insert(self.shoplistcellArr,self.new_Shoplistcell(self.go.transform:Find("mid_scratch_Panel/shop_list_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.shoplistcellArr,self.new_Shoplistcell(self.go.transform:Find("mid_scratch_Panel/shop_list_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.shoplistcellArr,self.new_Shoplistcell(self.go.transform:Find("mid_scratch_Panel/shop_list_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.shoplistcellArr,self.new_Shoplistcell(self.go.transform:Find("mid_scratch_Panel/shop_list_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.shoplistcellArr,self.new_Shoplistcell(self.go.transform:Find("mid_scratch_Panel/shop_list_CellRecycleScrollPanel/content/cellitem_4").gameObject))
end

--ShopCell复用单元
function this.new_ShopCell(itemGo)
	local item = { }
	item.go = itemGo
	item.title_Text=itemGo.transform:Find("shop_show_Image/title_Text"):GetComponent(typeof(TextWidget))
	item.explain_Text=itemGo.transform:Find("shop_show_Image/explain_Text"):GetComponent(typeof(TextWidget))
	item.distance_Text=itemGo.transform:Find("place_Image/distance_Text"):GetComponent(typeof(TextWidget))
	item.left_Button=itemGo.transform:Find("left_Button"):GetComponent(typeof(EmptyImageWidget))
	item.right_Button=itemGo.transform:Find("right_Button"):GetComponent(typeof(EmptyImageWidget))
	return item
end
--PlaceCell复用单元
function this.new_PlaceCell(itemGo)
	local item = { }
	item.go = itemGo
	item.title_Text=itemGo.transform:Find("title_Text"):GetComponent(typeof(TextWidget))
	item.explain_Text=itemGo.transform:Find("explain_Text"):GetComponent(typeof(TextWidget))
	item.distance_Text=itemGo.transform:Find("place_Image/distance_Text"):GetComponent(typeof(TextWidget))
	item.left_Button=itemGo.transform:Find("left_Button"):GetComponent(typeof(EmptyImageWidget))
	item.right_Button=itemGo.transform:Find("right_Button"):GetComponent(typeof(EmptyImageWidget))
	return item
end
--Shoplistcell复用单元
function this.new_Shoplistcell(itemGo)
	local item = { }
	item.go = itemGo
	item.press_Image=itemGo.transform:Find("press_Image"):GetComponent(typeof(ImageWidget))
	item.shop_head_Image=itemGo.transform:Find("shop_head_Image"):GetComponent(typeof(ImageWidget))
	item.group_Image=itemGo.transform:Find("group_Image"):GetComponent(typeof(ImageWidget))
	item.shop_name_Text=itemGo.transform:Find("Image/shop_name_Text"):GetComponent(typeof(TextWidget))
	item.star_Panel=itemGo.transform:Find("Image/star_Panel"):GetComponent(typeof(PanelWidget))
	item.star5=itemGo.transform:Find("Image/star_Panel/star5"):GetComponent(typeof(ImageWidget))
	item.star4=itemGo.transform:Find("Image/star_Panel/star4"):GetComponent(typeof(ImageWidget))
	item.star3=itemGo.transform:Find("Image/star_Panel/star3"):GetComponent(typeof(ImageWidget))
	item.star2=itemGo.transform:Find("Image/star_Panel/star2"):GetComponent(typeof(ImageWidget))
	item.star1=itemGo.transform:Find("Image/star_Panel/star1"):GetComponent(typeof(ImageWidget))
	item.shop_intro_Text=itemGo.transform:Find("Image/shop_intro_Text"):GetComponent(typeof(TextWidget))
	item.shop_distance_Text=itemGo.transform:Find("Image/shop_distance_Text"):GetComponent(typeof(TextWidget))
	item.shop_add_Text=itemGo.transform:Find("Image/shop_add_Text"):GetComponent(typeof(TextWidget))
	item.shop_walk_text=itemGo.transform:Find("Image/shop_walk_text"):GetComponent(typeof(TextWidget))
	return item
end

