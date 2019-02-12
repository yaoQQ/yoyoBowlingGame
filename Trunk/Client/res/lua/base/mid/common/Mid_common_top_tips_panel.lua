﻿local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget

Mid_common_top_tips_panel={}
local this = Mid_common_top_tips_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.topTips_bg_Image=nil
this.topTips_Text=nil

function this:init(gameObject)
	self.go=gameObject
	self.topTips_bg_Image=self.go.transform:Find("topTips/topTips_bg_Image"):GetComponent(typeof(ImageWidget))
	self.topTips_Text=self.go.transform:Find("topTips/topTips_Text"):GetComponent(typeof(TextWidget))
end


