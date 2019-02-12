﻿local TextWidget=CS.TextWidget

Mid_common_tip_panel={}
local this = Mid_common_tip_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.tip_text=nil

function this:init(gameObject)
	self.go=gameObject
	self.tip_text=self.go.transform:Find("tip_text"):GetComponent(typeof(TextWidget))
end

