local PanelWidget=CS.PanelWidget
local ImageWidget=CS.ImageWidget

Mid_common_waitting_panel={}
local this = Mid_common_waitting_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.waitting_panel=nil
this.image_1=nil
this.image_2=nil

function this:init(gameObject)
	self.go=gameObject
	self.waitting_panel=self.go.transform:Find("waitting_panel"):GetComponent(typeof(PanelWidget))
	self.image_1=self.go.transform:Find("waitting_panel/image_1"):GetComponent(typeof(ImageWidget))
	self.image_2=self.go.transform:Find("waitting_panel/image_2"):GetComponent(typeof(ImageWidget))
end


