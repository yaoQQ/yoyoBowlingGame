local TextWidget=CS.TextWidget
local EmptyImageWidget=CS.EmptyImageWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local ImageWidget=CS.ImageWidget
local PanelWidget=CS.PanelWidget
local IconWidget=CS.IconWidget

Mid_platform_message_second_panel={}
local this = Mid_platform_message_second_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.top_text=nil
this.back_Image=nil
this.ignore_Image=nil
this.messagelist_CellRecycleScrollPanel=nil
this.noMessage_Image=nil
--Messagelistcell数组
this.messagelistcellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.top_text=self.go.transform:Find("top_text"):GetComponent(typeof(TextWidget))
	self.back_Image=self.go.transform:Find("back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.ignore_Image=self.go.transform:Find("ignore_Image"):GetComponent(typeof(EmptyImageWidget))
	self.messagelist_CellRecycleScrollPanel=self.go.transform:Find("messagelist_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.noMessage_Image=self.go.transform:Find("noMessage_Image"):GetComponent(typeof(ImageWidget))
	self.messagelistcellArr={}
	table.insert(self.messagelistcellArr,self.new_Messagelistcell(self.go.transform:Find("messagelist_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.messagelistcellArr,self.new_Messagelistcell(self.go.transform:Find("messagelist_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.messagelistcellArr,self.new_Messagelistcell(self.go.transform:Find("messagelist_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.messagelistcellArr,self.new_Messagelistcell(self.go.transform:Find("messagelist_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.messagelistcellArr,self.new_Messagelistcell(self.go.transform:Find("messagelist_CellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.messagelistcellArr,self.new_Messagelistcell(self.go.transform:Find("messagelist_CellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.messagelistcellArr,self.new_Messagelistcell(self.go.transform:Find("messagelist_CellRecycleScrollPanel/content/cellitem_6").gameObject))
	table.insert(self.messagelistcellArr,self.new_Messagelistcell(self.go.transform:Find("messagelist_CellRecycleScrollPanel/content/cellitem_7").gameObject))
	table.insert(self.messagelistcellArr,self.new_Messagelistcell(self.go.transform:Find("messagelist_CellRecycleScrollPanel/content/cellitem_8").gameObject))
	table.insert(self.messagelistcellArr,self.new_Messagelistcell(self.go.transform:Find("messagelist_CellRecycleScrollPanel/content/cellitem_9").gameObject))
	table.insert(self.messagelistcellArr,self.new_Messagelistcell(self.go.transform:Find("messagelist_CellRecycleScrollPanel/content/cellitem_10").gameObject))
	table.insert(self.messagelistcellArr,self.new_Messagelistcell(self.go.transform:Find("messagelist_CellRecycleScrollPanel/content/cellitem_11").gameObject))
end

--Messagelistcell复用单元
function this.new_Messagelistcell(itemGo)
	local item = { }
	item.go = itemGo
	item.second_Panel=itemGo.transform:Find("second_Panel"):GetComponent(typeof(PanelWidget))
	item.second_Image=itemGo.transform:Find("second_Panel/second_Image"):GetComponent(typeof(ImageWidget))
	item.second_Icon=itemGo.transform:Find("second_Panel/second_Icon"):GetComponent(typeof(IconWidget))
	item.title_Text=itemGo.transform:Find("second_Panel/title_Text"):GetComponent(typeof(TextWidget))
	item.second_honor_Text=itemGo.transform:Find("second_Panel/second_honor_Text"):GetComponent(typeof(TextWidget))
	return item
end

