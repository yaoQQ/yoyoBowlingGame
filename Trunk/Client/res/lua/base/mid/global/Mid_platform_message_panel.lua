local EmptyImageWidget=CS.EmptyImageWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local PanelWidget=CS.PanelWidget
local ButtonWidget=CS.ButtonWidget
local TextWidget=CS.TextWidget
local ImageWidget=CS.ImageWidget
local IconWidget=CS.IconWidget
local CircleImageWidget=CS.CircleImageWidget

Mid_platform_message_panel={}
local this = Mid_platform_message_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.ignore_Image=nil
this.chatlist_CellRecycleScrollPanel=nil
this.top_Panel=nil
this.check_Button=nil
this.top_discribe_Text=nil
this.noMessage_Image=nil
--Friendchatlistcell数组
this.friendchatlistcellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.ignore_Image=self.go.transform:Find("ignore_Image"):GetComponent(typeof(EmptyImageWidget))
	self.chatlist_CellRecycleScrollPanel=self.go.transform:Find("chatlist_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.top_Panel=self.go.transform:Find("top_Panel"):GetComponent(typeof(PanelWidget))
	self.check_Button=self.go.transform:Find("top_Panel/check_Button"):GetComponent(typeof(ButtonWidget))
	self.top_discribe_Text=self.go.transform:Find("top_Panel/top_discribe_Text"):GetComponent(typeof(TextWidget))
	self.noMessage_Image=self.go.transform:Find("noMessage_Image"):GetComponent(typeof(ImageWidget))
	self.friendchatlistcellArr={}
	table.insert(self.friendchatlistcellArr,self.new_Friendchatlistcell(self.go.transform:Find("chatlist_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.friendchatlistcellArr,self.new_Friendchatlistcell(self.go.transform:Find("chatlist_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.friendchatlistcellArr,self.new_Friendchatlistcell(self.go.transform:Find("chatlist_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.friendchatlistcellArr,self.new_Friendchatlistcell(self.go.transform:Find("chatlist_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.friendchatlistcellArr,self.new_Friendchatlistcell(self.go.transform:Find("chatlist_CellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.friendchatlistcellArr,self.new_Friendchatlistcell(self.go.transform:Find("chatlist_CellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.friendchatlistcellArr,self.new_Friendchatlistcell(self.go.transform:Find("chatlist_CellRecycleScrollPanel/content/cellitem_6").gameObject))
	table.insert(self.friendchatlistcellArr,self.new_Friendchatlistcell(self.go.transform:Find("chatlist_CellRecycleScrollPanel/content/cellitem_7").gameObject))
	table.insert(self.friendchatlistcellArr,self.new_Friendchatlistcell(self.go.transform:Find("chatlist_CellRecycleScrollPanel/content/cellitem_8").gameObject))
	table.insert(self.friendchatlistcellArr,self.new_Friendchatlistcell(self.go.transform:Find("chatlist_CellRecycleScrollPanel/content/cellitem_9").gameObject))
	table.insert(self.friendchatlistcellArr,self.new_Friendchatlistcell(self.go.transform:Find("chatlist_CellRecycleScrollPanel/content/cellitem_10").gameObject))
	table.insert(self.friendchatlistcellArr,self.new_Friendchatlistcell(self.go.transform:Find("chatlist_CellRecycleScrollPanel/content/cellitem_11").gameObject))
end

--Friendchatlistcell复用单元
function this.new_Friendchatlistcell(itemGo)
	local item = { }
	item.go = itemGo
	item.config_Panel=itemGo.transform:Find("config_Panel"):GetComponent(typeof(PanelWidget))
	item.config_image=itemGo.transform:Find("config_Panel/config_image"):GetComponent(typeof(ImageWidget))
	item.configname_Text=itemGo.transform:Find("config_Panel/configname_Text"):GetComponent(typeof(TextWidget))
	item.config_Icon=itemGo.transform:Find("config_Panel/config_Icon"):GetComponent(typeof(IconWidget))
	item.confighonor_Text=itemGo.transform:Find("config_Panel/confighonor_Text"):GetComponent(typeof(TextWidget))
	item.configredpoint_Icon=itemGo.transform:Find("config_Panel/configredpoint_Icon"):GetComponent(typeof(IconWidget))
	item.configredpoint_Text=itemGo.transform:Find("config_Panel/configredpoint_Icon/configredpoint_Text"):GetComponent(typeof(TextWidget))
	item.friend_Panel=itemGo.transform:Find("friend_Panel"):GetComponent(typeof(PanelWidget))
	item.press_Image=itemGo.transform:Find("friend_Panel/press_Image"):GetComponent(typeof(ImageWidget))
	item.name_Text=itemGo.transform:Find("friend_Panel/name_Text"):GetComponent(typeof(TextWidget))
	item.head_Image=itemGo.transform:Find("friend_Panel/head_Image"):GetComponent(typeof(CircleImageWidget))
	item.sex_Image=itemGo.transform:Find("friend_Panel/sex_Image"):GetComponent(typeof(ImageWidget))
	item.sex_Icon=itemGo.transform:Find("friend_Panel/sex_Image/sex_Icon"):GetComponent(typeof(IconWidget))
	item.level_Text=itemGo.transform:Find("friend_Panel/sex_Image/level_Text"):GetComponent(typeof(TextWidget))
	item.honor_Text=itemGo.transform:Find("friend_Panel/honor_Text"):GetComponent(typeof(TextWidget))
	item.time_Text=itemGo.transform:Find("friend_Panel/time_Text"):GetComponent(typeof(TextWidget))
	item.msg_icon=itemGo.transform:Find("friend_Panel/msg_icon"):GetComponent(typeof(IconWidget))
	item.msgcount_Text=itemGo.transform:Find("friend_Panel/msg_icon/msgcount_Text"):GetComponent(typeof(TextWidget))
	return item
end

