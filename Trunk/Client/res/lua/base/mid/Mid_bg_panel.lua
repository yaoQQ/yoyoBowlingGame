
Mid_bg_panel={}
local this = Mid_bg_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil

function this:init(gameObject)
	self.go=gameObject
end


