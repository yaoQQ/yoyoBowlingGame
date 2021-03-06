﻿local ImageWidget=CS.ImageWidget
local EmptyImageWidget=CS.EmptyImageWidget
local TextWidget=CS.TextWidget

Mid_platform_redbag_withdraw_record_panel={}
local this = Mid_platform_redbag_withdraw_record_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.background_Image=nil
this.back_Image=nil
this.text=nil
this.more_Text=nil
--CellRecycle数组
this.cellRecycleArr={}

function this:init(gameObject)
	self.go=gameObject
	self.background_Image=self.go.transform:Find("record/background_Image"):GetComponent(typeof(ImageWidget))
	self.back_Image=self.go.transform:Find("record/mid/back_Image/back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.text=self.go.transform:Find("record/mid/text"):GetComponent(typeof(TextWidget))
	self.more_Text=self.go.transform:Find("record/mid/more_Image/more_Text"):GetComponent(typeof(TextWidget))
	self.cellRecycleArr={}
	table.insert(self.cellRecycleArr,self.new_CellRecycle(self.go.transform:Find("record/record_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.cellRecycleArr,self.new_CellRecycle(self.go.transform:Find("record/record_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.cellRecycleArr,self.new_CellRecycle(self.go.transform:Find("record/record_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.cellRecycleArr,self.new_CellRecycle(self.go.transform:Find("record/record_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.cellRecycleArr,self.new_CellRecycle(self.go.transform:Find("record/record_CellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.cellRecycleArr,self.new_CellRecycle(self.go.transform:Find("record/record_CellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.cellRecycleArr,self.new_CellRecycle(self.go.transform:Find("record/record_CellRecycleScrollPanel/content/cellitem_6").gameObject))
	table.insert(self.cellRecycleArr,self.new_CellRecycle(self.go.transform:Find("record/record_CellRecycleScrollPanel/content/cellitem_7").gameObject))
	table.insert(self.cellRecycleArr,self.new_CellRecycle(self.go.transform:Find("record/record_CellRecycleScrollPanel/content/cellitem_8").gameObject))
	table.insert(self.cellRecycleArr,self.new_CellRecycle(self.go.transform:Find("record/record_CellRecycleScrollPanel/content/cellitem_9").gameObject))
	table.insert(self.cellRecycleArr,self.new_CellRecycle(self.go.transform:Find("record/record_CellRecycleScrollPanel/content/cellitem_10").gameObject))
	table.insert(self.cellRecycleArr,self.new_CellRecycle(self.go.transform:Find("record/record_CellRecycleScrollPanel/content/cellitem_11").gameObject))
end

--CellRecycle复用单元
function this.new_CellRecycle(itemGo)
	local item = { }
	item.go = itemGo
	item.bg_Image=itemGo.transform:Find("bg_Image"):GetComponent(typeof(ImageWidget))
	item.gold_Text=itemGo.transform:Find("gold_Text"):GetComponent(typeof(TextWidget))
	item.date_Text=itemGo.transform:Find("date_Text"):GetComponent(typeof(TextWidget))
	item.time_Text=itemGo.transform:Find("time_Text"):GetComponent(typeof(TextWidget))
	return item
end

