local EmptyImageWidget=CS.EmptyImageWidget
local IconWidget=CS.IconWidget

Mid_platform_global_has_receiveredpacket_panel={}
local this = Mid_platform_global_has_receiveredpacket_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.panelParent=nil
this.shop_Icon=nil
this.pressBg=nil

function this:init(gameObject)
	self.go=gameObject
	self.panelParent=self.go.transform:Find("panelParent"):GetComponent(typeof(EmptyImageWidget))
	self.shop_Icon=self.go.transform:Find("panelParent/shop_Icon"):GetComponent(typeof(IconWidget))
	self.pressBg=self.go.transform:Find("panelParent/pressBg"):GetComponent(typeof(EmptyImageWidget))
end


