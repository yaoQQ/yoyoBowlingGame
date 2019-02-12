local PanelWidget=CS.PanelWidget
local ImageWidget=CS.ImageWidget
local EffectWidget=CS.EffectWidget
local TextWidget=CS.TextWidget
local EmptyImageWidget=CS.EmptyImageWidget
local CircleImageWidget=CS.CircleImageWidget
local IconWidget=CS.IconWidget
local CellGroupWidget=CS.CellGroupWidget
local ButtonWidget=CS.ButtonWidget

Mid_platform_personal_panel={}
local this = Mid_platform_personal_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.top=nil
this.top_diamond=nil
this.diamondEffect=nil
this.top_diamond_Text=nil
this.top_diamond_Button=nil
this.top_gold=nil
this.goldEffect=nil
this.top_gold_Text=nil
this.top_gold_Button=nil
this.top_yo_card=nil
this.ucardEffect=nil
this.top_yo_card_Text=nil
this.top_yo_card_Button=nil
this.top_packet=nil
this.packetEffect=nil
this.top_packet_Text=nil
this.top_packet_Button=nil
this.basic_info_Panel=nil
this.head_Image=nil
this.head_Icon=nil
this.otherinfor_Panel=nil
this.name_Text=nil
this.sex_Icon=nil
this.ID_Text=nil
this.enter_Image=nil
this.press_Image=nil
this.add_Image=nil
this.pic_CellGroup=nil
this.sign_Button=nil
this.friend_Button=nil
this.friend_rp_Image=nil
this.friend_rp_Text=nil
this.card_Button=nil
this.rank_Button=nil
this.config_Button=nil
this.mall_Button=nil
this.honor_Button=nil
this.task_Button=nil
this.backpack_Button=nil
this.collect_Button=nil
--Piccell数组
this.piccellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.top=self.go.transform:Find("top"):GetComponent(typeof(PanelWidget))
	self.top_diamond=self.go.transform:Find("top/common_top_cost_panel/top_diamond"):GetComponent(typeof(ImageWidget))
	self.diamondEffect=self.go.transform:Find("top/common_top_cost_panel/top_diamond/Image/top_diamond_Image/diamondEffect"):GetComponent(typeof(EffectWidget))
	self.top_diamond_Text=self.go.transform:Find("top/common_top_cost_panel/top_diamond/Image/top_diamond_Text"):GetComponent(typeof(TextWidget))
	self.top_diamond_Button=self.go.transform:Find("top/common_top_cost_panel/top_diamond/top_diamond_Button"):GetComponent(typeof(EmptyImageWidget))
	self.top_gold=self.go.transform:Find("top/common_top_cost_panel/top_gold"):GetComponent(typeof(ImageWidget))
	self.goldEffect=self.go.transform:Find("top/common_top_cost_panel/top_gold/Image/top_gold_Image/goldEffect"):GetComponent(typeof(EffectWidget))
	self.top_gold_Text=self.go.transform:Find("top/common_top_cost_panel/top_gold/Image/top_gold_Text"):GetComponent(typeof(TextWidget))
	self.top_gold_Button=self.go.transform:Find("top/common_top_cost_panel/top_gold/top_gold_Button"):GetComponent(typeof(EmptyImageWidget))
	self.top_yo_card=self.go.transform:Find("top/common_top_cost_panel/top_yo_card"):GetComponent(typeof(ImageWidget))
	self.ucardEffect=self.go.transform:Find("top/common_top_cost_panel/top_yo_card/Image/top_yo_card_Image/ucardEffect"):GetComponent(typeof(EffectWidget))
	self.top_yo_card_Text=self.go.transform:Find("top/common_top_cost_panel/top_yo_card/Image/top_yo_card_Text"):GetComponent(typeof(TextWidget))
	self.top_yo_card_Button=self.go.transform:Find("top/common_top_cost_panel/top_yo_card/top_yo_card_Button"):GetComponent(typeof(EmptyImageWidget))
	self.top_packet=self.go.transform:Find("top/common_top_cost_panel/top_packet"):GetComponent(typeof(ImageWidget))
	self.packetEffect=self.go.transform:Find("top/common_top_cost_panel/top_packet/Image/top_yo_card_Image/packetEffect"):GetComponent(typeof(EffectWidget))
	self.top_packet_Text=self.go.transform:Find("top/common_top_cost_panel/top_packet/Image/top_packet_Text"):GetComponent(typeof(TextWidget))
	self.top_packet_Button=self.go.transform:Find("top/common_top_cost_panel/top_packet/top_packet_Button"):GetComponent(typeof(EmptyImageWidget))
	self.basic_info_Panel=self.go.transform:Find("top/basic_info_Panel"):GetComponent(typeof(PanelWidget))
	self.head_Image=self.go.transform:Find("top/basic_info_Panel/head_Image"):GetComponent(typeof(ImageWidget))
	self.head_Icon=self.go.transform:Find("top/basic_info_Panel/head_Icon"):GetComponent(typeof(CircleImageWidget))
	self.otherinfor_Panel=self.go.transform:Find("top/basic_info_Panel/otherinfor_Panel"):GetComponent(typeof(PanelWidget))
	self.name_Text=self.go.transform:Find("top/basic_info_Panel/otherinfor_Panel/name_Text"):GetComponent(typeof(TextWidget))
	self.sex_Icon=self.go.transform:Find("top/basic_info_Panel/otherinfor_Panel/sex_Icon"):GetComponent(typeof(IconWidget))
	self.ID_Text=self.go.transform:Find("top/basic_info_Panel/otherinfor_Panel/ID_Text"):GetComponent(typeof(TextWidget))
	self.enter_Image=self.go.transform:Find("top/basic_info_Panel/otherinfor_Panel/enter_Image"):GetComponent(typeof(ImageWidget))
	self.press_Image=self.go.transform:Find("top/basic_info_Panel/press_Image"):GetComponent(typeof(EmptyImageWidget))
	self.add_Image=self.go.transform:Find("top/panel/add_Image"):GetComponent(typeof(ImageWidget))
	self.pic_CellGroup=self.go.transform:Find("top/panel/pic_CellGroup"):GetComponent(typeof(CellGroupWidget))
	self.sign_Button=self.go.transform:Find("mid/ScrollPanel/content/btn_panel/sign_Button"):GetComponent(typeof(ButtonWidget))
	self.friend_Button=self.go.transform:Find("mid/ScrollPanel/content/btn_panel/friend_Button"):GetComponent(typeof(ButtonWidget))
	self.friend_rp_Image=self.go.transform:Find("mid/ScrollPanel/content/btn_panel/friend_Button/friend_rp_Image"):GetComponent(typeof(ImageWidget))
	self.friend_rp_Text=self.go.transform:Find("mid/ScrollPanel/content/btn_panel/friend_Button/friend_rp_Image/friend_rp_Text"):GetComponent(typeof(TextWidget))
	self.card_Button=self.go.transform:Find("mid/ScrollPanel/content/btn_panel/card_Button"):GetComponent(typeof(ButtonWidget))
	self.rank_Button=self.go.transform:Find("mid/ScrollPanel/content/btn_panel/rank_Button"):GetComponent(typeof(ButtonWidget))
	self.config_Button=self.go.transform:Find("mid/ScrollPanel/content/btn_panel/config_Button"):GetComponent(typeof(ButtonWidget))
	self.mall_Button=self.go.transform:Find("mid/ScrollPanel/content/btn_panel/mall_Button"):GetComponent(typeof(ButtonWidget))
	self.honor_Button=self.go.transform:Find("mid/ScrollPanel/content/btn_panel/honor_Button"):GetComponent(typeof(ButtonWidget))
	self.task_Button=self.go.transform:Find("mid/ScrollPanel/content/btn_panel/task_Button"):GetComponent(typeof(ButtonWidget))
	self.backpack_Button=self.go.transform:Find("mid/ScrollPanel/content/btn_panel/backpack_Button"):GetComponent(typeof(ButtonWidget))
	self.collect_Button=self.go.transform:Find("mid/ScrollPanel/content/btn_panel/collect_Button"):GetComponent(typeof(ButtonWidget))
	self.piccellArr={}
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("top/panel/pic_CellGroup/cellitem").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("top/panel/pic_CellGroup/cellitem_1").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("top/panel/pic_CellGroup/cellitem_2").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("top/panel/pic_CellGroup/cellitem_3").gameObject))
	table.insert(self.piccellArr,self.new_Piccell(self.go.transform:Find("top/panel/pic_CellGroup/cellitem_4").gameObject))
end

--Piccell复用单元
function this.new_Piccell(itemGo)
	local item = { }
	item.go = itemGo
	item.item_Image=itemGo.transform:Find("item_Image"):GetComponent(typeof(ImageWidget))
	return item
end

