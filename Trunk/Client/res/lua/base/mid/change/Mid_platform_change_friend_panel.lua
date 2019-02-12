local PanelWidget=CS.PanelWidget
local ImageWidget=CS.ImageWidget
local IconWidget=CS.IconWidget
local TextWidget=CS.TextWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local CircleImageWidget=CS.CircleImageWidget
local EmptyImageWidget=CS.EmptyImageWidget
local ButtonWidget=CS.ButtonWidget

Mid_platform_change_friend_panel={}
local this = Mid_platform_change_friend_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.mid_top_Panel=nil
this.apply_Panel=nil
this.apply_config_image=nil
this.apply_config_Icon=nil
this.apply_configredpoint_Icon=nil
this.apply_configredpoint_Text=nil
this.add_Panel=nil
this.add_config_image=nil
this.add_config_Icon=nil
this.add_configredpoint_Icon=nil
this.add_configredpoint_Text=nil
this.friend_Text=nil
this.friend_CellRecycleScrollPanel=nil
this.head_Icon=nil
this.press_Image=nil
this.friend_Button=nil
this.friendcount_Button=nil
this.button_Image=nil
this.group_Button=nil
this.pyq_Button=nil
this.discover_Button=nil
this.add_Image=nil
--Friendlistcell数组
this.friendlistcellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.mid_top_Panel=self.go.transform:Find("mid_Panel/mid_top_Panel"):GetComponent(typeof(PanelWidget))
	self.apply_Panel=self.go.transform:Find("mid_Panel/mid_top_Panel/apply_Panel"):GetComponent(typeof(PanelWidget))
	self.apply_config_image=self.go.transform:Find("mid_Panel/mid_top_Panel/apply_Panel/apply_config_image"):GetComponent(typeof(ImageWidget))
	self.apply_config_Icon=self.go.transform:Find("mid_Panel/mid_top_Panel/apply_Panel/apply_config_Icon"):GetComponent(typeof(IconWidget))
	self.apply_configredpoint_Icon=self.go.transform:Find("mid_Panel/mid_top_Panel/apply_Panel/apply_configredpoint_Icon"):GetComponent(typeof(IconWidget))
	self.apply_configredpoint_Text=self.go.transform:Find("mid_Panel/mid_top_Panel/apply_Panel/apply_configredpoint_Icon/apply_configredpoint_Text"):GetComponent(typeof(TextWidget))
	self.add_Panel=self.go.transform:Find("mid_Panel/mid_top_Panel/add_Panel"):GetComponent(typeof(PanelWidget))
	self.add_config_image=self.go.transform:Find("mid_Panel/mid_top_Panel/add_Panel/add_config_image"):GetComponent(typeof(ImageWidget))
	self.add_config_Icon=self.go.transform:Find("mid_Panel/mid_top_Panel/add_Panel/add_config_Icon"):GetComponent(typeof(IconWidget))
	self.add_configredpoint_Icon=self.go.transform:Find("mid_Panel/mid_top_Panel/add_Panel/add_configredpoint_Icon"):GetComponent(typeof(IconWidget))
	self.add_configredpoint_Text=self.go.transform:Find("mid_Panel/mid_top_Panel/add_Panel/add_configredpoint_Icon/add_configredpoint_Text"):GetComponent(typeof(TextWidget))
	self.friend_Text=self.go.transform:Find("mid_Panel/Panel/friend_Text"):GetComponent(typeof(TextWidget))
	self.friend_CellRecycleScrollPanel=self.go.transform:Find("mid_Panel/Panel/friend_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.head_Icon=self.go.transform:Find("top_Panel/head_Icon"):GetComponent(typeof(CircleImageWidget))
	self.press_Image=self.go.transform:Find("top_Panel/head_Icon/press_Image"):GetComponent(typeof(EmptyImageWidget))
	self.friend_Button=self.go.transform:Find("top_Panel/friend_Button"):GetComponent(typeof(ButtonWidget))
	self.friendcount_Button=self.go.transform:Find("top_Panel/noUse/friendcount_Button"):GetComponent(typeof(ButtonWidget))
	self.button_Image=self.go.transform:Find("top_Panel/noUse/friendcount_Button/button_Image"):GetComponent(typeof(ImageWidget))
	self.group_Button=self.go.transform:Find("top_Panel/noUse/group_Button"):GetComponent(typeof(ButtonWidget))
	self.pyq_Button=self.go.transform:Find("top_Panel/noUse/pyq_Button"):GetComponent(typeof(ButtonWidget))
	self.discover_Button=self.go.transform:Find("top_Panel/noUse/discover_Button"):GetComponent(typeof(ButtonWidget))
	self.add_Image=self.go.transform:Find("top_Panel/add_Image"):GetComponent(typeof(EmptyImageWidget))
	self.friendlistcellArr={}
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("mid_Panel/Panel/friend_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("mid_Panel/Panel/friend_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("mid_Panel/Panel/friend_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("mid_Panel/Panel/friend_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("mid_Panel/Panel/friend_CellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("mid_Panel/Panel/friend_CellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("mid_Panel/Panel/friend_CellRecycleScrollPanel/content/cellitem_6").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("mid_Panel/Panel/friend_CellRecycleScrollPanel/content/cellitem_7").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("mid_Panel/Panel/friend_CellRecycleScrollPanel/content/cellitem_8").gameObject))
	table.insert(self.friendlistcellArr,self.new_Friendlistcell(self.go.transform:Find("mid_Panel/Panel/friend_CellRecycleScrollPanel/content/cellitem_9").gameObject))
end

--Friendlistcell复用单元
function this.new_Friendlistcell(itemGo)
	local item = { }
	item.go = itemGo
	item.friend_Panel=itemGo.transform:Find("friend_Panel"):GetComponent(typeof(PanelWidget))
	item.newfriend_image=itemGo.transform:Find("friend_Panel/newfriend_image"):GetComponent(typeof(ImageWidget))
	item.name_Text=itemGo.transform:Find("friend_Panel/name_Text"):GetComponent(typeof(TextWidget))
	item.head_Image=itemGo.transform:Find("friend_Panel/head_Image"):GetComponent(typeof(CircleImageWidget))
	item.sex_Image=itemGo.transform:Find("friend_Panel/head_Image/sex_Image"):GetComponent(typeof(ImageWidget))
	item.sexbg_Icon=itemGo.transform:Find("friend_Panel/head_Image/sex_Image/sexbg_Icon"):GetComponent(typeof(IconWidget))
	item.level_Text=itemGo.transform:Find("friend_Panel/head_Image/sex_Image/level_Text"):GetComponent(typeof(TextWidget))
	item.honor_Text=itemGo.transform:Find("friend_Panel/honor_Text"):GetComponent(typeof(TextWidget))
	item.distance_Text=itemGo.transform:Find("friend_Panel/distance_Text"):GetComponent(typeof(TextWidget))
	item.friendredpoint_Icon=itemGo.transform:Find("friend_Panel/friendredpoint_Icon"):GetComponent(typeof(IconWidget))
	item.friendredpoint_Text=itemGo.transform:Find("friend_Panel/friendredpoint_Icon/friendredpoint_Text"):GetComponent(typeof(TextWidget))
	return item
end

