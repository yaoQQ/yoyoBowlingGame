local EmptyImageWidget=CS.EmptyImageWidget
local IconWidget=CS.IconWidget
local TextWidget=CS.TextWidget

Mid_platform_global_shop_float_panel={}
local this = Mid_platform_global_shop_float_panel

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
this.timesbg=nil
this.times=nil

function this:init(gameObject)
	self.go=gameObject
	self.panelParent=self.go.transform:Find("panelParent"):GetComponent(typeof(EmptyImageWidget))
	self.shop_Icon=self.go.transform:Find("panelParent/shop_Icon"):GetComponent(typeof(IconWidget))
	self.timesbg=self.go.transform:Find("panelParent/timesbg"):GetComponent(typeof(EmptyImageWidget))
	self.times=self.go.transform:Find("panelParent/timesbg/times"):GetComponent(typeof(TextWidget))
end


