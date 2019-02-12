local EmptyImageWidget=CS.EmptyImageWidget
local PanelWidget=CS.PanelWidget
local ButtonWidget=CS.ButtonWidget

Mid_platform_set_main_panel={}
local this = Mid_platform_set_main_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.back_Image=nil
this.buttonlistpanel=nil
this.accountButton=nil
this.messegeButton=nil
this.aboutButton=nil
this.helpButton=nil
this.cleanButton=nil
this.exitButton=nil

function this:init(gameObject)
	self.go=gameObject
	self.back_Image=self.go.transform:Find("top_panel/back_Image/back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.buttonlistpanel=self.go.transform:Find("mid_panel/buttonlistpanel"):GetComponent(typeof(PanelWidget))
	self.accountButton=self.go.transform:Find("mid_panel/buttonlistpanel/accountButton"):GetComponent(typeof(ButtonWidget))
	self.messegeButton=self.go.transform:Find("mid_panel/buttonlistpanel/messegeButton"):GetComponent(typeof(ButtonWidget))
	self.aboutButton=self.go.transform:Find("mid_panel/buttonlistpanel/aboutButton"):GetComponent(typeof(ButtonWidget))
	self.helpButton=self.go.transform:Find("mid_panel/buttonlistpanel/helpButton"):GetComponent(typeof(ButtonWidget))
	self.cleanButton=self.go.transform:Find("mid_panel/buttonlistpanel/cleanButton"):GetComponent(typeof(ButtonWidget))
	self.exitButton=self.go.transform:Find("mid_panel/buttonlistpanel/exitButton"):GetComponent(typeof(ButtonWidget))
end


