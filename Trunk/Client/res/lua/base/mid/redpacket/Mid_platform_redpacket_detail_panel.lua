﻿local TextWidget=CS.TextWidget
local CircleImageWidget=CS.CircleImageWidget
local EmptyImageWidget=CS.EmptyImageWidget
local PanelWidget=CS.PanelWidget
local ButtonWidget=CS.ButtonWidget
local GridRecycleScrollWidget=CS.GridRecycleScrollWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local ImageWidget=CS.ImageWidget

Mid_platform_redpacket_detail_panel={}
local this = Mid_platform_redpacket_detail_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.bag_type_text=nil
this.head_image=nil
this.des_text=nil
this.money_text=nil
this.back_image=nil
this.remain_time_text=nil
this.shop_Panel=nil
this.intro_Text=nil
this.mainpage_Button=nil
this.pic_GridRecycleScrollPanel=nil
this.redbag_Panel=nil
this.redbag_intro_text=nil
this.bag_list_scroll=nil
--Piccell数组
this.piccellArr={}
--Redbaglistcell数组
this.redbaglistcellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.bag_type_text=self.go.transform:Find("bag_type_text"):GetComponent(typeof(TextWidget))
	self.head_image=self.go.transform:Find("top/head_image"):GetComponent(typeof(CircleImageWidget))
	self.des_text=self.go.transform:Find("top/des_text"):GetComponent(typeof(TextWidget))
	self.money_text=self.go.transform:Find("top/money_text"):GetComponent(typeof(TextWidget))
	self.back_image=self.go.transform:Find("top/back_image"):GetComponent(typeof(EmptyImageWidget))
	self.remain_time_text=self.go.transform:Find("top/remain_time_text"):GetComponent(typeof(TextWidget))
	self.shop_Panel=self.go.transform:Find("bottom/shop_Panel"):GetComponent(typeof(PanelWidget))
	self.intro_Text=self.go.transform:Find("bottom/shop_Panel/intro_Text"):GetComponent(typeof(TextWidget))
	self.mainpage_Button=self.go.transform:Find("bottom/shop_Panel/mainpage_Button"):GetComponent(typeof(ButtonWidget))
	self.pic_GridRecycleScrollPanel=self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel"):GetComponent(typeof(GridRecycleScrollWidget))
	self.redbag_Panel=self.go.transform:Find("bottom/redbag_Panel"):GetComponent(typeof(PanelWidget))
	self.redbag_intro_text=self.go.transform:Find("bottom/redbag_Panel/redbag_intro_text"):GetComponent(typeof(TextWidget))
	self.bag_list_scroll=self.go.transform:Find("bottom/redbag_Panel/bag_list_scroll"):GetComponent(typeof(CellRecycleScrollWidget))
	self.piccellArr={}
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_0_1").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_0_2").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_0_3").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_0_4").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_1_0").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_1_1").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_1_2").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_1_3").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_1_4").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_2_0").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_2_1").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_2_2").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_2_3").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_2_4").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_3_0").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_3_1").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_3_2").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_3_3").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("bottom/shop_Panel/pic_GridRecycleScrollPanel/content/cellitem_3_4").gameObject))
	self.redbaglistcellArr={}
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("bottom/redbag_Panel/bag_list_scroll/content/cellitem").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("bottom/redbag_Panel/bag_list_scroll/content/cellitem_1").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("bottom/redbag_Panel/bag_list_scroll/content/cellitem_2").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("bottom/redbag_Panel/bag_list_scroll/content/cellitem_3").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("bottom/redbag_Panel/bag_list_scroll/content/cellitem_4").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("bottom/redbag_Panel/bag_list_scroll/content/cellitem_5").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("bottom/redbag_Panel/bag_list_scroll/content/cellitem_6").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("bottom/redbag_Panel/bag_list_scroll/content/cellitem_7").gameObject))
end

--Piccell复用单元
function this.new_Piccell(itemGo)
	local item = { }
	item.go = itemGo
	item.pic_Image=itemGo.transform:Find("pic_Image"):GetComponent(typeof(ImageWidget))
	return item
end
--Redbaglistcell复用单元
function this.new_Redbaglistcell(itemGo)
	local item = { }
	item.go = itemGo
	item.head_image=itemGo.transform:Find("head_image"):GetComponent(typeof(CircleImageWidget))
	item.name_text=itemGo.transform:Find("name_text"):GetComponent(typeof(TextWidget))
	item.time_text=itemGo.transform:Find("time_text"):GetComponent(typeof(TextWidget))
	item.money_text=itemGo.transform:Find("money_text"):GetComponent(typeof(TextWidget))
	return item
end

