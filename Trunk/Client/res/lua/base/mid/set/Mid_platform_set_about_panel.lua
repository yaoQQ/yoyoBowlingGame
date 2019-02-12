local EmptyImageWidget=CS.EmptyImageWidget
local TextWidget=CS.TextWidget

Mid_platform_set_about_panel={}
local this = Mid_platform_set_about_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.back_Image=nil
this.edition_Text=nil
this.bottom_Text=nil
this.agreement_Image=nil

function this:init(gameObject)
	self.go=gameObject
	self.back_Image=self.go.transform:Find("top_panel/back_Image/back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.edition_Text=self.go.transform:Find("mid_panel/edition_Text"):GetComponent(typeof(TextWidget))
	self.bottom_Text=self.go.transform:Find("bottom_Panel/bottom_Text"):GetComponent(typeof(TextWidget))
	self.agreement_Image=self.go.transform:Find("bottom_Panel/agreement_Image"):GetComponent(typeof(EmptyImageWidget))
end


