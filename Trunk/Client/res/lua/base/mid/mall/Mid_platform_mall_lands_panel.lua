local EmptyImageWidget=CS.EmptyImageWidget
local IconWidget=CS.IconWidget
local PanelWidget=CS.PanelWidget
local GridRecycleScrollWidget=CS.GridRecycleScrollWidget
local TextWidget=CS.TextWidget
local EffectWidget=CS.EffectWidget
local ImageWidget=CS.ImageWidget
local ButtonWidget=CS.ButtonWidget

Mid_platform_mall_lands_panel={}
local this = Mid_platform_mall_lands_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.point_Image=nil
this.close_Image=nil
this.diamond_Icon=nil
this.gold_Icon=nil
this.yo_card_Icon=nil
this.diamond_Panel=nil
this.diamond_GridRecycleScrollPanel=nil
this.diamong_diamond_Text=nil
this.diamong_gold_Text=nil
this.gold_Panel=nil
this.gold_GridRecycleScrollPanel=nil
this.gold_top_Panel=nil
this.gold_diamond_Text=nil
this.gold_gold_Text=nil
this.yo_card_Panel=nil
this.yo_card_GridRecycleScrollPanel=nil
this.yo_card_top_Panel=nil
this.yo_card_num_Text=nil
this.blend_panel=nil
this.gold_icon_pool=nil
this.diamond_icon_pool=nil
this.cost_icon_pool=nil
this.blend_grid_scroll=nil
this.top_gold_Text=nil
this.top_diamond_Text=nil
this.top_yo_card_Text=nil
this.top_packet_Text=nil
this.goldEffect=nil
this.diamondEffect=nil
this.ucardEffect=nil
this.packetEffect=nil
--Diamondcell数组
this.diamondcellArr={}
--Goldcell数组
this.goldcellArr={}
--YoCardcell数组
this.yoCardcellArr={}
--BlendCell数组
this.blendCellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.point_Image=self.go.transform:Find("point_Image"):GetComponent(typeof(EmptyImageWidget))
	self.close_Image=self.go.transform:Find("mid_Panel/Image/close_Image"):GetComponent(typeof(EmptyImageWidget))
	self.diamond_Icon=self.go.transform:Find("mid_Panel/diamond_Icon"):GetComponent(typeof(IconWidget))
	self.gold_Icon=self.go.transform:Find("mid_Panel/gold_Icon"):GetComponent(typeof(IconWidget))
	self.yo_card_Icon=self.go.transform:Find("mid_Panel/yo_card_Icon"):GetComponent(typeof(IconWidget))
	self.diamond_Panel=self.go.transform:Find("mid_Panel/diamond_Panel"):GetComponent(typeof(PanelWidget))
	self.diamond_GridRecycleScrollPanel=self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel"):GetComponent(typeof(GridRecycleScrollWidget))
	self.diamong_diamond_Text=self.go.transform:Find("mid_Panel/diamond_Panel/diamond_top_Panel/diamong_logo_Image/diamong_diamond_Text"):GetComponent(typeof(TextWidget))
	self.diamong_gold_Text=self.go.transform:Find("mid_Panel/diamond_Panel/diamond_top_Panel/golg_logo_Image/diamong_gold_Text"):GetComponent(typeof(TextWidget))
	self.gold_Panel=self.go.transform:Find("mid_Panel/gold_Panel"):GetComponent(typeof(PanelWidget))
	self.gold_GridRecycleScrollPanel=self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel"):GetComponent(typeof(GridRecycleScrollWidget))
	self.gold_top_Panel=self.go.transform:Find("mid_Panel/gold_Panel/gold_top_Panel"):GetComponent(typeof(PanelWidget))
	self.gold_diamond_Text=self.go.transform:Find("mid_Panel/gold_Panel/gold_top_Panel/diamong_logo_Image/gold_diamond_Text"):GetComponent(typeof(TextWidget))
	self.gold_gold_Text=self.go.transform:Find("mid_Panel/gold_Panel/gold_top_Panel/golg_logo_Image/gold_gold_Text"):GetComponent(typeof(TextWidget))
	self.yo_card_Panel=self.go.transform:Find("mid_Panel/yo_card_Panel"):GetComponent(typeof(PanelWidget))
	self.yo_card_GridRecycleScrollPanel=self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel"):GetComponent(typeof(GridRecycleScrollWidget))
	self.yo_card_top_Panel=self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_top_Panel"):GetComponent(typeof(PanelWidget))
	self.yo_card_num_Text=self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_top_Panel/yo_card_logo_Image/yo_card_num_Text"):GetComponent(typeof(TextWidget))
	self.blend_panel=self.go.transform:Find("mid_Panel/blend_panel"):GetComponent(typeof(PanelWidget))
	self.gold_icon_pool=self.go.transform:Find("mid_Panel/blend_panel/gold_icon_pool"):GetComponent(typeof(IconWidget))
	self.diamond_icon_pool=self.go.transform:Find("mid_Panel/blend_panel/diamond_icon_pool"):GetComponent(typeof(IconWidget))
	self.cost_icon_pool=self.go.transform:Find("mid_Panel/blend_panel/cost_icon_pool"):GetComponent(typeof(IconWidget))
	self.blend_grid_scroll=self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll"):GetComponent(typeof(GridRecycleScrollWidget))
	self.top_gold_Text=self.go.transform:Find("common_top_cost_panel/top_gold_Text"):GetComponent(typeof(TextWidget))
	self.top_diamond_Text=self.go.transform:Find("common_top_cost_panel/top_diamond_Text"):GetComponent(typeof(TextWidget))
	self.top_yo_card_Text=self.go.transform:Find("common_top_cost_panel/top_yo_card_Text"):GetComponent(typeof(TextWidget))
	self.top_packet_Text=self.go.transform:Find("common_top_cost_panel/top_packet_Text"):GetComponent(typeof(TextWidget))
	self.goldEffect=self.go.transform:Find("common_top_cost_panel/Image/goldEffect"):GetComponent(typeof(EffectWidget))
	self.diamondEffect=self.go.transform:Find("common_top_cost_panel/Image (1)/diamondEffect"):GetComponent(typeof(EffectWidget))
	self.ucardEffect=self.go.transform:Find("common_top_cost_panel/Image (2)/ucardEffect"):GetComponent(typeof(EffectWidget))
	self.packetEffect=self.go.transform:Find("common_top_cost_panel/packet_Image/packetEffect"):GetComponent(typeof(EffectWidget))
	self.diamondcellArr={}
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_0_1").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_0_2").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_0_3").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_0_4").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_1_0").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_1_1").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_1_2").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_1_3").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_1_4").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_2_0").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_2_1").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_2_2").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_2_3").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_2_4").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_3_0").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_3_1").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_3_2").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_3_3").gameObject))
	table.insert(self.diamondcellArr,self.new_Diamondcell(self.go.transform:Find("mid_Panel/diamond_Panel/diamond_GridRecycleScrollPanel/content/cellitem_3_4").gameObject))
	self.goldcellArr={}
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_0_1").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_0_2").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_0_3").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_0_4").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_1_0").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_1_1").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_1_2").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_1_3").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_1_4").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_2_0").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_2_1").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_2_2").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_2_3").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_2_4").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_3_0").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_3_1").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_3_2").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_3_3").gameObject))
	table.insert(self.goldcellArr,self.new_Goldcell(self.go.transform:Find("mid_Panel/gold_Panel/gold_GridRecycleScrollPanel/content/cellitem_3_4").gameObject))
	self.yoCardcellArr={}
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_0_1").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_0_2").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_0_3").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_1_0").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_1_1").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_1_2").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_1_3").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_2_0").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_2_1").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_2_2").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_2_3").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_3_0").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_3_1").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_3_2").gameObject))
	table.insert(self.yoCardcellArr,self.new_YoCardcell(self.go.transform:Find("mid_Panel/yo_card_Panel/yo_card_GridRecycleScrollPanel/content/cellitem_3_3").gameObject))
	self.blendCellArr={}
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_0_1").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_0_2").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_0_3").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_0_4").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_1_0").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_1_1").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_1_2").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_1_3").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_1_4").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_2_0").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_2_1").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_2_2").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_2_3").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_2_4").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_3_0").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_3_1").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_3_2").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_3_3").gameObject))
	table.insert(self.blendCellArr,self.new_BlendCell(self.go.transform:Find("mid_Panel/blend_panel/blend_grid_scroll/content/cellitem_3_4").gameObject))
end

--Diamondcell复用单元
function this.new_Diamondcell(itemGo)
	local item = { }
	item.go = itemGo
	item.go_circle_image=itemGo.transform:Find("go_circle_image"):GetComponent(typeof(ImageWidget))
	item.go_Icon=itemGo.transform:Find("go_Icon"):GetComponent(typeof(IconWidget))
	item.mallEffect=itemGo.transform:Find("go_Icon/mallEffect"):GetComponent(typeof(EffectWidget))
	item.return_num_Text=itemGo.transform:Find("return_num_Text"):GetComponent(typeof(TextWidget))
	item.cost_Text=itemGo.transform:Find("cost_Text"):GetComponent(typeof(TextWidget))
	item.go_circle_Button=itemGo.transform:Find("go_circle_Button"):GetComponent(typeof(ButtonWidget))
	return item
end
--Goldcell复用单元
function this.new_Goldcell(itemGo)
	local item = { }
	item.go = itemGo
	item.go_circle_image=itemGo.transform:Find("go_circle_image"):GetComponent(typeof(ImageWidget))
	item.go_Image=itemGo.transform:Find("go_Image"):GetComponent(typeof(ImageWidget))
	item.go_Icon=itemGo.transform:Find("go_Icon"):GetComponent(typeof(IconWidget))
	item.mallEffect=itemGo.transform:Find("go_Icon/mallEffect"):GetComponent(typeof(EffectWidget))
	item.return_num_Text=itemGo.transform:Find("return_num_Text"):GetComponent(typeof(TextWidget))
	item.cost_Text=itemGo.transform:Find("cost_Text"):GetComponent(typeof(TextWidget))
	item.go_circle_Button=itemGo.transform:Find("go_circle_Button"):GetComponent(typeof(ButtonWidget))
	return item
end
--YoCardcell复用单元
function this.new_YoCardcell(itemGo)
	local item = { }
	item.go = itemGo
	item.go_circle_image=itemGo.transform:Find("go_circle_image"):GetComponent(typeof(ImageWidget))
	item.go_Image=itemGo.transform:Find("go_Image"):GetComponent(typeof(ImageWidget))
	item.go_Icon=itemGo.transform:Find("go_Icon"):GetComponent(typeof(IconWidget))
	item.return_num_text=itemGo.transform:Find("return_num_text"):GetComponent(typeof(TextWidget))
	item.cost_text=itemGo.transform:Find("cost_text"):GetComponent(typeof(TextWidget))
	item.go_circle_Button=itemGo.transform:Find("go_circle_Button"):GetComponent(typeof(ButtonWidget))
	return item
end
--BlendCell复用单元
function this.new_BlendCell(itemGo)
	local item = { }
	item.go = itemGo
	item.return_image=itemGo.transform:Find("return_image"):GetComponent(typeof(ImageWidget))
	item.return_btn=itemGo.transform:Find("return_btn"):GetComponent(typeof(ButtonWidget))
	item.return_num_text=itemGo.transform:Find("return_num_text"):GetComponent(typeof(TextWidget))
	item.cost_text=itemGo.transform:Find("cost_text"):GetComponent(typeof(TextWidget))
	item.cost_image=itemGo.transform:Find("cost_text/cost_image"):GetComponent(typeof(ImageWidget))
	item.mall_effect=itemGo.transform:Find("mall_effect"):GetComponent(typeof(EffectWidget))
	return item
end

