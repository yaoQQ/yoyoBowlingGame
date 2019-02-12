﻿local ImageWidget=CS.ImageWidget

Mid_base_control_panel={}
local this = Mid_base_control_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.control_img=nil

function this:init(gameObject)
	self.go=gameObject
	self.control_img=self.go.transform:Find("control_img"):GetComponent(typeof(ImageWidget))
end

