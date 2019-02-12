local EmptyImageWidget=CS.EmptyImageWidget

Mid_platform_my_pos_platform={}
local this = Mid_platform_my_pos_platform

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.official_parent=nil

function this:init(gameObject)
	self.go=gameObject
	self.official_parent=self.go.transform:Find("official_parent"):GetComponent(typeof(EmptyImageWidget))
end


