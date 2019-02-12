local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget
local CellGroupWidget=CS.CellGroupWidget
local ButtonWidget=CS.ButtonWidget
local IconWidget=CS.IconWidget

Mid_platform_share_end_panel={}
local this = Mid_platform_share_end_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.mask_Image=nil
this.title_Text=nil
this.share_end_CellGroup=nil
this.left_Button=nil
this.right_Button=nil
--EndshareCell数组
this.endshareCellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.mask_Image=self.go.transform:Find("mask_Image"):GetComponent(typeof(ImageWidget))
	self.title_Text=self.go.transform:Find("title_Text"):GetComponent(typeof(TextWidget))
	self.share_end_CellGroup=self.go.transform:Find("share_end_CellGroup"):GetComponent(typeof(CellGroupWidget))
	self.left_Button=self.go.transform:Find("left_Button"):GetComponent(typeof(ButtonWidget))
	self.right_Button=self.go.transform:Find("right_Button"):GetComponent(typeof(ButtonWidget))
	self.endshareCellArr={}
	table.insert(self.endshareCellArr,self.new_EndshareCell(self.go.transform:Find("share_end_CellGroup/CellItem").gameObject))
	table.insert(self.endshareCellArr,self.new_EndshareCell(self.go.transform:Find("share_end_CellGroup/CellItem_1").gameObject))
end

--EndshareCell复用单元
function this.new_EndshareCell(itemGo)
	local item = { }
	item.go = itemGo
	item.left_gold_Icon=itemGo.transform:Find("left_bg_Image/left_gold_Icon"):GetComponent(typeof(IconWidget))
	item.left_gold_Text=itemGo.transform:Find("left_bg_Image/left_gold_Text"):GetComponent(typeof(TextWidget))
	return item
end

