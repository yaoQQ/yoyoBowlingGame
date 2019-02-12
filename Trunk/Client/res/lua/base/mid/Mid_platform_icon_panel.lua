local PanelWidget=CS.PanelWidget

Mid_platform_icon_panel={}
local this = Mid_platform_icon_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.platform_icon_panel=nil
this.other=nil
this.icon_Panel=nil

function this:init(gameObject)
	self.go=gameObject
	self.platform_icon_panel=self.go.transform:Find(""):GetComponent(typeof(PanelWidget))
	self.other=self.go.transform:Find("other"):GetComponent(typeof(PanelWidget))
	self.icon_Panel=self.go.transform:Find("icon_Panel"):GetComponent(typeof(PanelWidget))
end


