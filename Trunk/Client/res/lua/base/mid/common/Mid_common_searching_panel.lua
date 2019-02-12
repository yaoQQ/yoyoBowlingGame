local PanelWidget=CS.PanelWidget
local IconWidget=CS.IconWidget

Mid_common_searching_panel={}
local this = Mid_common_searching_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.waitting_panel=nil
this.image_change=nil

function this:init(gameObject)
	self.go=gameObject
	self.waitting_panel=self.go.transform:Find("waitting_panel"):GetComponent(typeof(PanelWidget))
	self.image_change=self.go.transform:Find("waitting_panel/image_change"):GetComponent(typeof(IconWidget))
end


