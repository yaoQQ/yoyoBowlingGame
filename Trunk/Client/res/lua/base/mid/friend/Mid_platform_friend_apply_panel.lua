local EmptyImageWidget=CS.EmptyImageWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local TextWidget=CS.TextWidget
local ImageWidget=CS.ImageWidget
local CircleImageWidget=CS.CircleImageWidget
local IconWidget=CS.IconWidget
local ButtonWidget=CS.ButtonWidget

Mid_platform_friend_apply_panel={}
local this = Mid_platform_friend_apply_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.back_Image=nil
this.apply_CellRecycleScrollPanel=nil
this.noapply_Text=nil
--Applycell数组
this.applycellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.back_Image=self.go.transform:Find("back_Image/back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.apply_CellRecycleScrollPanel=self.go.transform:Find("apply_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.noapply_Text=self.go.transform:Find("noapply_Text"):GetComponent(typeof(TextWidget))
	self.applycellArr={}
	table.insert(self.applycellArr,self.new_Applycell(self.go.transform:Find("apply_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.applycellArr,self.new_Applycell(self.go.transform:Find("apply_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.applycellArr,self.new_Applycell(self.go.transform:Find("apply_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.applycellArr,self.new_Applycell(self.go.transform:Find("apply_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.applycellArr,self.new_Applycell(self.go.transform:Find("apply_CellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.applycellArr,self.new_Applycell(self.go.transform:Find("apply_CellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.applycellArr,self.new_Applycell(self.go.transform:Find("apply_CellRecycleScrollPanel/content/cellitem_6").gameObject))
	table.insert(self.applycellArr,self.new_Applycell(self.go.transform:Find("apply_CellRecycleScrollPanel/content/cellitem_7").gameObject))
	table.insert(self.applycellArr,self.new_Applycell(self.go.transform:Find("apply_CellRecycleScrollPanel/content/cellitem_8").gameObject))
	table.insert(self.applycellArr,self.new_Applycell(self.go.transform:Find("apply_CellRecycleScrollPanel/content/cellitem_9").gameObject))
end

--Applycell复用单元
function this.new_Applycell(itemGo)
	local item = { }
	item.go = itemGo
	item.press_Image=itemGo.transform:Find("press_Image"):GetComponent(typeof(ImageWidget))
	item.head_Image=itemGo.transform:Find("head_Image"):GetComponent(typeof(CircleImageWidget))
	item.sex_Image=itemGo.transform:Find("head_Image/sex_Image"):GetComponent(typeof(ImageWidget))
	item.sexbg_Icon=itemGo.transform:Find("head_Image/sex_Image/sexbg_Icon"):GetComponent(typeof(IconWidget))
	item.level_Text=itemGo.transform:Find("head_Image/sex_Image/level_Text"):GetComponent(typeof(TextWidget))
	item.intro_Text=itemGo.transform:Find("intro_Text"):GetComponent(typeof(TextWidget))
	item.added_Text=itemGo.transform:Find("added_Text"):GetComponent(typeof(TextWidget))
	item.add_Button=itemGo.transform:Find("add_Button"):GetComponent(typeof(ButtonWidget))
	item.name_Text=itemGo.transform:Find("name_Text"):GetComponent(typeof(TextWidget))
	return item
end

