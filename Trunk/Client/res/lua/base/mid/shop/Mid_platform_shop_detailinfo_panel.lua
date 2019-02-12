local ImageWidget=CS.ImageWidget
local CircleImageWidget=CS.CircleImageWidget
local TextWidget=CS.TextWidget
local EmptyImageWidget=CS.EmptyImageWidget
local CellGroupWidget=CS.CellGroupWidget

Mid_platform_shop_detailinfo_panel={}
local this = Mid_platform_shop_detailinfo_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.closemask_Image=nil
this.head_CircleImage=nil
this.name_Text=nil
this.close_Image=nil
this.intro_Text=nil
this.thing_Text=nil
this.thing_CellGroup=nil
--Thingcell数组
this.thingcellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.closemask_Image=self.go.transform:Find("closemask_Image"):GetComponent(typeof(ImageWidget))
	self.head_CircleImage=self.go.transform:Find("head_CircleImage"):GetComponent(typeof(CircleImageWidget))
	self.name_Text=self.go.transform:Find("name_Text"):GetComponent(typeof(TextWidget))
	self.close_Image=self.go.transform:Find("close_Image/close_Image"):GetComponent(typeof(EmptyImageWidget))
	self.intro_Text=self.go.transform:Find("intro_Text"):GetComponent(typeof(TextWidget))
	self.thing_Text=self.go.transform:Find("thing_Image/thing_Text"):GetComponent(typeof(TextWidget))
	self.thing_CellGroup=self.go.transform:Find("thing_CellGroup"):GetComponent(typeof(CellGroupWidget))
	self.thingcellArr={}
	table.insert(self.thingcellArr,self.new_Thingcell(self.go.transform:Find("thing_CellGroup/CellItem").gameObject))
	table.insert(self.thingcellArr,self.new_Thingcell(self.go.transform:Find("thing_CellGroup/CellItem_1").gameObject))
	table.insert(self.thingcellArr,self.new_Thingcell(self.go.transform:Find("thing_CellGroup/CellItem_2").gameObject))
	table.insert(self.thingcellArr,self.new_Thingcell(self.go.transform:Find("thing_CellGroup/CellItem_3").gameObject))
	table.insert(self.thingcellArr,self.new_Thingcell(self.go.transform:Find("thing_CellGroup/CellItem_4").gameObject))
	table.insert(self.thingcellArr,self.new_Thingcell(self.go.transform:Find("thing_CellGroup/CellItem_5").gameObject))
	table.insert(self.thingcellArr,self.new_Thingcell(self.go.transform:Find("thing_CellGroup/CellItem_6").gameObject))
	table.insert(self.thingcellArr,self.new_Thingcell(self.go.transform:Find("thing_CellGroup/CellItem_7").gameObject))
end

--Thingcell复用单元
function this.new_Thingcell(itemGo)
	local item = { }
	item.go = itemGo
	item.shopthing_Image=itemGo.transform:Find("shopthing_Image"):GetComponent(typeof(ImageWidget))
	item.shopthing_Text=itemGo.transform:Find("shopthing_Image/shopthing_Text"):GetComponent(typeof(TextWidget))
	return item
end

