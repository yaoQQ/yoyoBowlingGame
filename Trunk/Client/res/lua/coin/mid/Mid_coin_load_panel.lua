﻿local SliderWidget=CS.SliderWidget

Mid_coin_load_panel={}
local this = Mid_coin_load_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.Slider=nil

function this:init(gameObject)
	self.go=gameObject
	self.Slider=self.go.transform:Find("Slider"):GetComponent(typeof(SliderWidget))
end


