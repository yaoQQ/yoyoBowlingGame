﻿local EffectWidget=CS.EffectWidget
local CellGroupWidget=CS.CellGroupWidget
local ButtonWidget=CS.ButtonWidget
local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget
local IconWidget=CS.IconWidget
local EmptyImageWidget=CS.EmptyImageWidget
local SpineWidget=CS.SpineWidget
local PanelWidget=CS.PanelWidget
local CircleImageWidget=CS.CircleImageWidget

Mid_platform_newredbag_panel={}
local this = Mid_platform_newredbag_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.add_Effect=nil
this.friendlist_CellGroup=nil
this.add_friends_Button=nil
this.go_guess_Image=nil
this.change_Text=nil
this.cat_image=nil
this.redbag_Icon=nil
this.countdown_Text=nil
this.countdown_Image=nil
this.spine=nil
this.add_friend_tip_panel=nil
this.secretary_head_image=nil
this.secretary_name_text=nil
this.add_friend_tip_bg=nil
this.add_friend_tip_text=nil
this.money_text=nil
this.withdraw_Button=nil
--Friendlistcell数组
this.friendlistcellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.add_Effect=self.go.transform:Find("Panel/ScrollPanel/content/mid/add_Effect"):GetComponent(typeof(EffectWidget))
	self.friendlist_CellGroup=self.go.transform:Find("Panel/ScrollPanel/content/mid/Panel/friendlist_CellGroup"):GetComponent(typeof(CellGroupWidget))
	self.add_friends_Button=self.go.transform:Find("Panel/ScrollPanel/content/mid/Panel/add_friends_Button"):GetComponent(typeof(ButtonWidget))
	self.go_guess_Image=self.go.transform:Find("Panel/ScrollPanel/content/bottom/go_guess_Image"):GetComponent(typeof(ImageWidget))
	self.change_Text=self.go.transform:Find("Panel/ScrollPanel/content/bottom/go_guess_Image/change_Text"):GetComponent(typeof(TextWidget))
	self.cat_image=self.go.transform:Find("Panel/ScrollPanel/content/cat_image"):GetComponent(typeof(ImageWidget))
	self.redbag_Icon=self.go.transform:Find("Panel/ScrollPanel/content/cat_image/redbag_Icon"):GetComponent(typeof(IconWidget))
	self.countdown_Text=self.go.transform:Find("Panel/ScrollPanel/content/cat_image/redbag_Icon/countdown_Text"):GetComponent(typeof(TextWidget))
	self.countdown_Image=self.go.transform:Find("Panel/ScrollPanel/content/cat_image/countdown_Image"):GetComponent(typeof(EmptyImageWidget))
	self.spine=self.go.transform:Find("Panel/ScrollPanel/content/cat_image/spine"):GetComponent(typeof(SpineWidget))
	self.add_friend_tip_panel=self.go.transform:Find("Panel/ScrollPanel/content/add_friend_tip_panel"):GetComponent(typeof(PanelWidget))
	self.secretary_head_image=self.go.transform:Find("Panel/ScrollPanel/content/add_friend_tip_panel/secretary_head_image"):GetComponent(typeof(CircleImageWidget))
	self.secretary_name_text=self.go.transform:Find("Panel/ScrollPanel/content/add_friend_tip_panel/secretary_head_image/secretary_name_text"):GetComponent(typeof(TextWidget))
	self.add_friend_tip_bg=self.go.transform:Find("Panel/ScrollPanel/content/add_friend_tip_panel/secretary_head_image/add_friend_tip_bg"):GetComponent(typeof(ImageWidget))
	self.add_friend_tip_text=self.go.transform:Find("Panel/ScrollPanel/content/add_friend_tip_panel/secretary_head_image/add_friend_tip_bg/add_friend_tip_text"):GetComponent(typeof(TextWidget))
	self.money_text=self.go.transform:Find("top/money_text"):GetComponent(typeof(TextWidget))
	self.withdraw_Button=self.go.transform:Find("top/withdraw_Button"):GetComponent(typeof(ButtonWidget))
	self.friendlistcellArr={}
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("Panel/ScrollPanel/content/mid/Panel/friendlist_CellGroup/CellItem").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("Panel/ScrollPanel/content/mid/Panel/friendlist_CellGroup/CellItem_1").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("Panel/ScrollPanel/content/mid/Panel/friendlist_CellGroup/CellItem_2").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("Panel/ScrollPanel/content/mid/Panel/friendlist_CellGroup/CellItem_3").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("Panel/ScrollPanel/content/mid/Panel/friendlist_CellGroup/CellItem_4").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("Panel/ScrollPanel/content/mid/Panel/friendlist_CellGroup/CellItem_5").gameObject))
end

--Friendlistcell复用单元
function this.new_Friendlistcell(itemGo)
	local item = { }
	item.go = itemGo
	item.head_Icon=itemGo.transform:Find("head_Icon"):GetComponent(typeof(CircleImageWidget))
	item.name_Text=itemGo.transform:Find("head_Icon/name_Text"):GetComponent(typeof(TextWidget))
	item.btn_Icon=itemGo.transform:Find("btn_Icon"):GetComponent(typeof(IconWidget))
	item.btn_get_Text=itemGo.transform:Find("btn_Icon/btn_get_Text"):GetComponent(typeof(TextWidget))
	item.btn_Image=itemGo.transform:Find("btn_Icon/btn_Image"):GetComponent(typeof(ImageWidget))
	return item
end

