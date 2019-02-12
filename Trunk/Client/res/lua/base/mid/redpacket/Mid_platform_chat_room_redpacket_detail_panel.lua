﻿local EmptyImageWidget=CS.EmptyImageWidget
local TextWidget=CS.TextWidget
local CircleImageWidget=CS.CircleImageWidget
local ImageWidget=CS.ImageWidget
local ButtonWidget=CS.ButtonWidget
local ToggleWidget=CS.ToggleWidget
local PanelWidget=CS.PanelWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget

Mid_platform_chat_room_redpacket_detail_panel={}
local this = Mid_platform_chat_room_redpacket_detail_panel

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
this.shop_head_image=nil
this.shop_name_text=nil
this.official_image=nil
this.money_text=nil
this.shop_main_page_btn=nil
this.get_toggle=nil
this.bag_num_text=nil
this.redbag_Panel=nil
this.bag_list_scroll=nil
--Redbaglistcell数组
this.redbaglistcellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.back_image=self.go.transform:Find("back_image"):GetComponent(typeof(EmptyImageWidget))
	self.bag_type_text=self.go.transform:Find("bag_type_text"):GetComponent(typeof(TextWidget))
	self.remain_time_text=self.go.transform:Find("remain_time_text"):GetComponent(typeof(TextWidget))
	self.shop_head_image=self.go.transform:Find("shop_head_image"):GetComponent(typeof(CircleImageWidget))
	self.shop_name_text=self.go.transform:Find("shop_name_text"):GetComponent(typeof(TextWidget))
	self.official_image=self.go.transform:Find("shop_name_text/official_image"):GetComponent(typeof(ImageWidget))
	self.money_text=self.go.transform:Find("money_bg/money_text"):GetComponent(typeof(TextWidget))
	self.shop_main_page_btn=self.go.transform:Find("shop_main_page_btn"):GetComponent(typeof(ButtonWidget))
	self.get_toggle=self.go.transform:Find("get_toggle"):GetComponent(typeof(ToggleWidget))
	self.bag_num_text=self.go.transform:Find("bag_num_text"):GetComponent(typeof(TextWidget))
	self.redbag_Panel=self.go.transform:Find("redbag_Panel"):GetComponent(typeof(PanelWidget))
	self.bag_list_scroll=self.go.transform:Find("redbag_Panel/bag_list_scroll"):GetComponent(typeof(CellRecycleScrollWidget))
	self.redbaglistcellArr={}
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("redbag_Panel/bag_list_scroll/content/cellitem").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("redbag_Panel/bag_list_scroll/content/cellitem_1").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("redbag_Panel/bag_list_scroll/content/cellitem_2").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("redbag_Panel/bag_list_scroll/content/cellitem_3").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("redbag_Panel/bag_list_scroll/content/cellitem_4").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("redbag_Panel/bag_list_scroll/content/cellitem_5").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("redbag_Panel/bag_list_scroll/content/cellitem_6").gameObject))
	table.insert(self.redbaglistcellArr,self.new_Redbaglistcell(self.go.transform:Find("redbag_Panel/bag_list_scroll/content/cellitem_7").gameObject))
end

--Redbaglistcell复用单元
function this.new_Redbaglistcell(itemGo)
	local item = { }
	item.go = itemGo
	item.is_self_image=itemGo.transform:Find("is_self_image"):GetComponent(typeof(ImageWidget))
	item.head_image=itemGo.transform:Find("head_image"):GetComponent(typeof(CircleImageWidget))
	item.name_text=itemGo.transform:Find("name_text"):GetComponent(typeof(TextWidget))
	item.time_text=itemGo.transform:Find("time_text"):GetComponent(typeof(TextWidget))
	item.money_text=itemGo.transform:Find("money_text"):GetComponent(typeof(TextWidget))
	return item
end
