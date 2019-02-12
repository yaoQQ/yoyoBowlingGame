﻿local ImageWidget=CS.ImageWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local ButtonWidget=CS.ButtonWidget
local IconWidget=CS.IconWidget
local TextWidget=CS.TextWidget

Mid_platform_message_reward_end_panel={}
local this = Mid_platform_message_reward_end_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.end_bg_Image=nil
this.end_CellRecycleScrollPanel=nil
this.end_Button=nil
--EndCell数组
this.endCellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.end_bg_Image=self.go.transform:Find("end_bg_Image"):GetComponent(typeof(ImageWidget))
	self.end_CellRecycleScrollPanel=self.go.transform:Find("end_bg_Image/end_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.end_Button=self.go.transform:Find("end_bg_Image/end_Button"):GetComponent(typeof(ButtonWidget))
	self.endCellArr={}
	table.insert(self.endCellArr,self.new_EndCell(self.go.transform:Find("end_bg_Image/end_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.endCellArr,self.new_EndCell(self.go.transform:Find("end_bg_Image/end_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.endCellArr,self.new_EndCell(self.go.transform:Find("end_bg_Image/end_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.endCellArr,self.new_EndCell(self.go.transform:Find("end_bg_Image/end_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.endCellArr,self.new_EndCell(self.go.transform:Find("end_bg_Image/end_CellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.endCellArr,self.new_EndCell(self.go.transform:Find("end_bg_Image/end_CellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.endCellArr,self.new_EndCell(self.go.transform:Find("end_bg_Image/end_CellRecycleScrollPanel/content/cellitem_6").gameObject))
	table.insert(self.endCellArr,self.new_EndCell(self.go.transform:Find("end_bg_Image/end_CellRecycleScrollPanel/content/cellitem_7").gameObject))
end

--EndCell复用单元
function this.new_EndCell(itemGo)
	local item = { }
	item.go = itemGo
	item.noget_reward_icon=itemGo.transform:Find("bg_Image/noget_reward_icon"):GetComponent(typeof(IconWidget))
	item.reward_Text=itemGo.transform:Find("reward_Text"):GetComponent(typeof(TextWidget))
	return item
end

