﻿local EmptyImageWidget=CS.EmptyImageWidget

Mid_platform_sign_panel={}
local this = Mid_platform_sign_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.btn_close=nil
--Cell数组
this.cellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.btn_close=self.go.transform:Find("btn_close"):GetComponent(typeof(EmptyImageWidget))
	self.cellArr={}
	table.insert(self.cellArr,self.new_Cell(self.go.transform:Find("CellGroup/CellItem").gameObject))
	table.insert(self.cellArr,self.new_Cell(self.go.transform:Find("CellGroup/CellItem_1").gameObject))
	table.insert(self.cellArr,self.new_Cell(self.go.transform:Find("CellGroup/CellItem_2").gameObject))
	table.insert(self.cellArr,self.new_Cell(self.go.transform:Find("CellGroup/CellItem_3").gameObject))
	table.insert(self.cellArr,self.new_Cell(self.go.transform:Find("CellGroup/CellItem_4").gameObject))
	table.insert(self.cellArr,self.new_Cell(self.go.transform:Find("CellGroup/CellItem_5").gameObject))
end

--Cell复用单元
function this.new_Cell(itemGo)
	local item = { }
	item.go = itemGo
	return item
end

