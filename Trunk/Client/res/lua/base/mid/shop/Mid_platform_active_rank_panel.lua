﻿local CircleImageWidget=CS.CircleImageWidget
local TextWidget=CS.TextWidget
local PanelWidget=CS.PanelWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local ImageWidget=CS.ImageWidget
local IconWidget=CS.IconWidget

Mid_platform_active_rank_panel={}
local this = Mid_platform_active_rank_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.back_Image=nil
this.change_type_Text=nil
this.rank_title_Text=nil
this.player_title_Text=nil
this.time_title_Text =nil
this.Mid_Panel=nil
this.rank_CellRecycleScrollPanel=nil
this.buttom_Panel=nil
this.buttom_rank_Text=nil
this.buttom_time_Text=nil
--Rank_Cell数组
this.rank_CellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.back_Image=self.go.transform:Find("Panel/back_Image"):GetComponent(typeof(CircleImageWidget))
	self.change_type_Text=self.go.transform:Find("Panel/change_type_Text"):GetComponent(typeof(TextWidget))
	self.rank_title_Text=self.go.transform:Find("Panel/rank_title_Text"):GetComponent(typeof(TextWidget))
	self.player_title_Text=self.go.transform:Find("Panel/player_title_Text"):GetComponent(typeof(TextWidget))
	self.time_title_Text =self.go.transform:Find("Panel/time_title_Text "):GetComponent(typeof(TextWidget))
	self.Mid_Panel=self.go.transform:Find("Mid_Panel"):GetComponent(typeof(PanelWidget))
	self.rank_CellRecycleScrollPanel=self.go.transform:Find("Mid_Panel/rank_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.buttom_Panel=self.go.transform:Find("buttom_Panel"):GetComponent(typeof(PanelWidget))
	self.buttom_rank_Text=self.go.transform:Find("buttom_Panel/buttom_rank_Text"):GetComponent(typeof(TextWidget))
	self.buttom_time_Text=self.go.transform:Find("buttom_Panel/buttom_time_Text"):GetComponent(typeof(TextWidget))
	self.rank_CellArr={}
	table.insert(self.rank_CellArr,self.new_Rank_Cell(self.go.transform:Find("Mid_Panel/rank_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.rank_CellArr,self.new_Rank_Cell(self.go.transform:Find("Mid_Panel/rank_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.rank_CellArr,self.new_Rank_Cell(self.go.transform:Find("Mid_Panel/rank_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.rank_CellArr,self.new_Rank_Cell(self.go.transform:Find("Mid_Panel/rank_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.rank_CellArr,self.new_Rank_Cell(self.go.transform:Find("Mid_Panel/rank_CellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.rank_CellArr,self.new_Rank_Cell(self.go.transform:Find("Mid_Panel/rank_CellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.rank_CellArr,self.new_Rank_Cell(self.go.transform:Find("Mid_Panel/rank_CellRecycleScrollPanel/content/cellitem_6").gameObject))
	table.insert(self.rank_CellArr,self.new_Rank_Cell(self.go.transform:Find("Mid_Panel/rank_CellRecycleScrollPanel/content/cellitem_7").gameObject))
	table.insert(self.rank_CellArr,self.new_Rank_Cell(self.go.transform:Find("Mid_Panel/rank_CellRecycleScrollPanel/content/cellitem_8").gameObject))
	table.insert(self.rank_CellArr,self.new_Rank_Cell(self.go.transform:Find("Mid_Panel/rank_CellRecycleScrollPanel/content/cellitem_9").gameObject))
	table.insert(self.rank_CellArr,self.new_Rank_Cell(self.go.transform:Find("Mid_Panel/rank_CellRecycleScrollPanel/content/cellitem_10").gameObject))
	table.insert(self.rank_CellArr,self.new_Rank_Cell(self.go.transform:Find("Mid_Panel/rank_CellRecycleScrollPanel/content/cellitem_11").gameObject))
end

--Rank_Cell复用单元
function this.new_Rank_Cell(itemGo)
	local item = { }
	item.go = itemGo
	item.highLight_Image=itemGo.transform:Find("highLight_Image"):GetComponent(typeof(ImageWidget))
	item.rank_Icon=itemGo.transform:Find("rank_Icon"):GetComponent(typeof(IconWidget))
	item.rank_Text=itemGo.transform:Find("rank_Text"):GetComponent(typeof(TextWidget))
	item.head_CircleImage=itemGo.transform:Find("head_CircleImage"):GetComponent(typeof(CircleImageWidget))
	item.player_name_Text=itemGo.transform:Find("player_name_Text"):GetComponent(typeof(TextWidget))
	item.use_Time_Text=itemGo.transform:Find("use_Time_Text"):GetComponent(typeof(TextWidget))
	return item
end

