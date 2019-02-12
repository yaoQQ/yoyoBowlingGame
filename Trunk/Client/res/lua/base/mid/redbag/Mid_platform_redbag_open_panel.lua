﻿local ImageWidget=CS.ImageWidget
local AnimatorWidget=CS.AnimatorWidget
local TextWidget=CS.TextWidget
local CircleImageWidget=CS.CircleImageWidget
local ButtonWidget=CS.ButtonWidget
local EmptyImageWidget=CS.EmptyImageWidget

Mid_platform_redbag_open_panel={}
local this = Mid_platform_redbag_open_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.mask_image=nil
this.open_panel=nil
this.name_text=nil
this.head_circleimage=nil
this.open_btn_animator=nil
this.openredbag_btn=nil
this.remain_bg=nil
this.bg_text=nil
this.remain_text=nil
this.add_btn=nil
this.noClick_Image=nil

function this:init(gameObject)
	self.go=gameObject
	self.mask_image=self.go.transform:Find("mask_image"):GetComponent(typeof(ImageWidget))
	self.open_panel=self.go.transform:Find("open_panel"):GetComponent(typeof(AnimatorWidget))
	self.name_text=self.go.transform:Find("open_panel/money_Image/mid_Image/name_text"):GetComponent(typeof(TextWidget))
	self.head_circleimage=self.go.transform:Find("open_panel/money_Image/mid_Image/head_circleimage"):GetComponent(typeof(CircleImageWidget))
	self.open_btn_animator=self.go.transform:Find("open_panel/money_Image/open_btn_animator"):GetComponent(typeof(AnimatorWidget))
	self.openredbag_btn=self.go.transform:Find("open_panel/money_Image/open_btn_animator/openredbag_btn"):GetComponent(typeof(ButtonWidget))
	self.remain_bg=self.go.transform:Find("remain_bg"):GetComponent(typeof(ImageWidget))
	self.bg_text=self.go.transform:Find("remain_bg/bg_text"):GetComponent(typeof(TextWidget))
	self.remain_text=self.go.transform:Find("remain_bg/remain_text"):GetComponent(typeof(TextWidget))
	self.add_btn=self.go.transform:Find("remain_bg/add_btn"):GetComponent(typeof(ImageWidget))
	self.noClick_Image=self.go.transform:Find("noClick_Image"):GetComponent(typeof(EmptyImageWidget))
end


