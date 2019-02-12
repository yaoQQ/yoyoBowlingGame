﻿local PanelWidget=CS.PanelWidget
local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget
local CellGroupWidget=CS.CellGroupWidget
local ButtonWidget=CS.ButtonWidget
local IconWidget=CS.IconWidget

Mid_platform_redbag_end_panel={}
local this = Mid_platform_redbag_end_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.platform_redbag_end_panel=nil
this.main_Panel=nil
this.mask_Image=nil
this.title_Text=nil
this.dis_Text=nil
this.redbag_CellGroup=nil
this.left_Button=nil
this.right_Button=nil
this.number_Text=nil
this.pass_Panel=nil
this.pass_mask_Image=nil
this.nextOne_Button=nil
--RedbagCell数组
this.redbagCellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.platform_redbag_end_panel=self.go.transform:Find(""):GetComponent(typeof(PanelWidget))
	self.main_Panel=self.go.transform:Find("main_Panel"):GetComponent(typeof(PanelWidget))
	self.mask_Image=self.go.transform:Find("main_Panel/mask_Image"):GetComponent(typeof(ImageWidget))
	self.title_Text=self.go.transform:Find("main_Panel/title_Text"):GetComponent(typeof(TextWidget))
	self.dis_Text=self.go.transform:Find("main_Panel/dis_Text"):GetComponent(typeof(TextWidget))
	self.redbag_CellGroup=self.go.transform:Find("main_Panel/redbag_CellGroup"):GetComponent(typeof(CellGroupWidget))
	self.left_Button=self.go.transform:Find("main_Panel/left_Button"):GetComponent(typeof(ButtonWidget))
	self.right_Button=self.go.transform:Find("main_Panel/right_Button"):GetComponent(typeof(ButtonWidget))
	self.number_Text=self.go.transform:Find("main_Panel/right_Button/number_Text"):GetComponent(typeof(TextWidget))
	self.pass_Panel=self.go.transform:Find("pass_Panel"):GetComponent(typeof(PanelWidget))
	self.pass_mask_Image=self.go.transform:Find("pass_Panel/pass_mask_Image"):GetComponent(typeof(ImageWidget))
	self.nextOne_Button=self.go.transform:Find("pass_Panel/nextOne_Button"):GetComponent(typeof(ButtonWidget))
	self.redbagCellArr={}
	table.insert(self.redbagCellArr,self.new_RedbagCell(self.go.transform:Find("main_Panel/redbag_CellGroup/CellItem").gameObject))
	table.insert(self.redbagCellArr,self.new_RedbagCell(self.go.transform:Find("main_Panel/redbag_CellGroup/CellItem_1").gameObject))
end

--RedbagCell复用单元
function this.new_RedbagCell(itemGo)
	local item = { }
	item.go = itemGo
	item.left_gold_Icon=itemGo.transform:Find("left_bg_Image/left_gold_Icon"):GetComponent(typeof(IconWidget))
	item.left_gold_Text=itemGo.transform:Find("left_bg_Image/left_gold_Text"):GetComponent(typeof(TextWidget))
	return item
end
