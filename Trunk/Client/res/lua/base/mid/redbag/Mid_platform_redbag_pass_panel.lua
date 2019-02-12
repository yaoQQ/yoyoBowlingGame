﻿local PanelWidget=CS.PanelWidget
local ImageWidget=CS.ImageWidget
local ButtonWidget=CS.ButtonWidget

Mid_platform_redbag_pass_panel={}
local this = Mid_platform_redbag_pass_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.platform_redbag_pass_panel=nil
this.pass_mask_Image=nil
this.left_Button=nil
this.right_Button=nil

function this:init(gameObject)
	self.go=gameObject
	self.platform_redbag_pass_panel=self.go.transform:Find(""):GetComponent(typeof(PanelWidget))
	self.pass_mask_Image=self.go.transform:Find("pass_mask_Image"):GetComponent(typeof(ImageWidget))
	self.left_Button=self.go.transform:Find("left_Button"):GetComponent(typeof(ButtonWidget))
	self.right_Button=self.go.transform:Find("right_Button"):GetComponent(typeof(ButtonWidget))
end

