local PanelWidget=CS.PanelWidget
local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget

Mid_platform_shop_activity_end_panel={}
local this = Mid_platform_shop_activity_end_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.platform_shop_activity_end_panel=nil
this.title_Text=nil
this.left_gold_Text=nil
this.right_gold_Text=nil
this.left_Button=nil
this.right_Button=nil

function this:init(gameObject)
	self.go=gameObject
	self.platform_shop_activity_end_panel=self.go.transform:Find(""):GetComponent(typeof(PanelWidget))
	self.title_Text=self.go.transform:Find("title_Text"):GetComponent(typeof(TextWidget))
	self.left_gold_Text=self.go.transform:Find("left_bg_Image/left_gold_Text"):GetComponent(typeof(TextWidget))
	self.right_gold_Text=self.go.transform:Find("right_bg_Image/right_gold_Text"):GetComponent(typeof(TextWidget))
	self.left_Button=self.go.transform:Find("left_Button"):GetComponent(typeof(ButtonWidget))
	self.right_Button=self.go.transform:Find("right_Button"):GetComponent(typeof(ButtonWidget))
end


