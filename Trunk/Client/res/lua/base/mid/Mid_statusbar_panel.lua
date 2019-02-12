
Mid_statusbar_panel={}
local this = Mid_statusbar_panel

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


