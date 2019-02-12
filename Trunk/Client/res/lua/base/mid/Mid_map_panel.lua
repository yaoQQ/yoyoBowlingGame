﻿local EmptyImageWidget=CS.EmptyImageWidget
local ImageWidget=CS.ImageWidget

Mid_map_panel={}
local this = Mid_map_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.map=nil
this.tileItem=nil
this.bg=nil
this.tiles=nil

function this:init(gameObject)
	self.go=gameObject
	self.map=self.go.transform:Find("map"):GetComponent(typeof(EmptyImageWidget))
	self.tileItem=self.go.transform:Find("map/tileItem"):GetComponent(typeof(ImageWidget))
	self.bg=self.go.transform:Find("map/bg"):GetComponent(typeof(ImageWidget))
	self.tiles=self.go.transform:Find("map/tiles"):GetComponent(typeof(EmptyImageWidget))
end

