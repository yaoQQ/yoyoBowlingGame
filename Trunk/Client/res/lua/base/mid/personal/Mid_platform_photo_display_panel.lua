﻿local EmptyImageWidget=CS.EmptyImageWidget
local TextWidget=CS.TextWidget
local CellGroupWidget=CS.CellGroupWidget
local ImageWidget=CS.ImageWidget
local IconWidget=CS.IconWidget

Mid_platform_photo_display_panel={}
local this = Mid_platform_photo_display_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.back_Image=nil
this.Text=nil
this.PhotoGroup=nil
this.delete_image=nil
this.delete_confirm_icon=nil
this.delete_confirm_text=nil
--PhotoFrameItem数组
this.photoFrameItemArr={}
--PhotoItem数组
this.photoItemArr={}

function this:init(gameObject)
	self.go=gameObject
	self.back_Image=self.go.transform:Find("back_Image/back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.Text=self.go.transform:Find("Text"):GetComponent(typeof(TextWidget))
	self.PhotoGroup=self.go.transform:Find("PhotoGroup"):GetComponent(typeof(CellGroupWidget))
	self.delete_image=self.go.transform:Find("delete_image"):GetComponent(typeof(ImageWidget))
	self.delete_confirm_icon=self.go.transform:Find("delete_image/delete_confirm_icon"):GetComponent(typeof(IconWidget))
	self.delete_confirm_text=self.go.transform:Find("delete_image/delete_confirm_text"):GetComponent(typeof(TextWidget))
	self.photoFrameItemArr={}
	table.insert(self.photoFrameItemArr,self.new_PhotoFrameItem(self.go.transform:Find("PhotoFrameGroup/CellItem").gameObject))
	table.insert(self.photoFrameItemArr,self.new_PhotoFrameItem(self.go.transform:Find("PhotoFrameGroup/CellItem_1").gameObject))
	table.insert(self.photoFrameItemArr,self.new_PhotoFrameItem(self.go.transform:Find("PhotoFrameGroup/CellItem_2").gameObject))
	table.insert(self.photoFrameItemArr,self.new_PhotoFrameItem(self.go.transform:Find("PhotoFrameGroup/CellItem_3").gameObject))
	table.insert(self.photoFrameItemArr,self.new_PhotoFrameItem(self.go.transform:Find("PhotoFrameGroup/CellItem_4").gameObject))
	table.insert(self.photoFrameItemArr,self.new_PhotoFrameItem(self.go.transform:Find("PhotoFrameGroup/CellItem_5").gameObject))
	table.insert(self.photoFrameItemArr,self.new_PhotoFrameItem(self.go.transform:Find("PhotoFrameGroup/CellItem_6").gameObject))
	table.insert(self.photoFrameItemArr,self.new_PhotoFrameItem(self.go.transform:Find("PhotoFrameGroup/CellItem_7").gameObject))
	table.insert(self.photoFrameItemArr,self.new_PhotoFrameItem(self.go.transform:Find("PhotoFrameGroup/CellItem_8").gameObject))
	self.photoItemArr={}
	table.insert(self.photoItemArr,self.new_PhotoItem(self.go.transform:Find("PhotoGroup/CellItem").gameObject))
	table.insert(self.photoItemArr,self.new_PhotoItem(self.go.transform:Find("PhotoGroup/CellItem_1").gameObject))
	table.insert(self.photoItemArr,self.new_PhotoItem(self.go.transform:Find("PhotoGroup/CellItem_2").gameObject))
	table.insert(self.photoItemArr,self.new_PhotoItem(self.go.transform:Find("PhotoGroup/CellItem_3").gameObject))
	table.insert(self.photoItemArr,self.new_PhotoItem(self.go.transform:Find("PhotoGroup/CellItem_4").gameObject))
	table.insert(self.photoItemArr,self.new_PhotoItem(self.go.transform:Find("PhotoGroup/CellItem_5").gameObject))
	table.insert(self.photoItemArr,self.new_PhotoItem(self.go.transform:Find("PhotoGroup/CellItem_6").gameObject))
	table.insert(self.photoItemArr,self.new_PhotoItem(self.go.transform:Find("PhotoGroup/CellItem_7").gameObject))
	table.insert(self.photoItemArr,self.new_PhotoItem(self.go.transform:Find("PhotoGroup/CellItem_8").gameObject))
end

--PhotoFrameItem复用单元
function this.new_PhotoFrameItem(itemGo)
	local item = { }
	item.go = itemGo
	item.photo_frame_image=itemGo.transform:Find("photo_frame_image"):GetComponent(typeof(ImageWidget))
	return item
end
--PhotoItem复用单元
function this.new_PhotoItem(itemGo)
	local item = { }
	item.go = itemGo
	item.photo_image=itemGo.transform:Find("photo_image"):GetComponent(typeof(ImageWidget))
	return item
end

