﻿local EmptyImageWidget=CS.EmptyImageWidget
local TextWidget=CS.TextWidget

Mid_platform_coupon_use_know_panel={}
local this = Mid_platform_coupon_use_know_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.back_Image=nil
this.tips_Text=nil

function this:init(gameObject)
	self.go=gameObject
	self.back_Image=self.go.transform:Find("back_Image/back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.tips_Text=self.go.transform:Find("tips_Text"):GetComponent(typeof(TextWidget))
end


