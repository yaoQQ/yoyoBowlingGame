﻿local ImageWidget=CS.ImageWidget
local AnimatorWidget=CS.AnimatorWidget
local TextWidget=CS.TextWidget
local CircleImageWidget=CS.CircleImageWidget
local EmptyImageWidget=CS.EmptyImageWidget
local ButtonWidget=CS.ButtonWidget
local EffectWidget=CS.EffectWidget

Mid_platform_chat_room_redpacket_open_panel={}
local this = Mid_platform_chat_room_redpacket_open_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.mask_image=nil
this.red_packet_animator=nil
this.top_open_image=nil
this.name_text=nil
this.official_image=nil
this.title_text=nil
this.head_circleImage=nil
this.close_image=nil
this.open_btn_animator=nil
this.openredbag_btn=nil
this.has_receive_Image=nil
this.get_receive_redbag_text=nil
this.open_effect=nil

function this:init(gameObject)
	self.go=gameObject
	self.mask_image=self.go.transform:Find("mask_image"):GetComponent(typeof(ImageWidget))
	self.red_packet_animator=self.go.transform:Find("red_packet_animator"):GetComponent(typeof(AnimatorWidget))
	self.top_open_image=self.go.transform:Find("red_packet_animator/top_open_image"):GetComponent(typeof(ImageWidget))
	self.name_text=self.go.transform:Find("red_packet_animator/mid_image/name_text"):GetComponent(typeof(TextWidget))
	self.official_image=self.go.transform:Find("red_packet_animator/mid_image/name_text/official_image"):GetComponent(typeof(ImageWidget))
	self.title_text=self.go.transform:Find("red_packet_animator/mid_image/title_text"):GetComponent(typeof(TextWidget))
	self.head_circleImage=self.go.transform:Find("red_packet_animator/mid_image/headBg/head_circleImage"):GetComponent(typeof(CircleImageWidget))
	self.close_image=self.go.transform:Find("red_packet_animator/mid_image/close_image"):GetComponent(typeof(EmptyImageWidget))
	self.open_btn_animator=self.go.transform:Find("red_packet_animator/mid_image/open_btn_animator"):GetComponent(typeof(AnimatorWidget))
	self.openredbag_btn=self.go.transform:Find("red_packet_animator/mid_image/open_btn_animator/openredbag_btn"):GetComponent(typeof(ButtonWidget))
	self.has_receive_Image=self.go.transform:Find("red_packet_animator/mid_image/has_receive_Image"):GetComponent(typeof(ImageWidget))
	self.get_receive_redbag_text=self.go.transform:Find("red_packet_animator/mid_image/get_receive_redbag_text"):GetComponent(typeof(TextWidget))
	self.open_effect=self.go.transform:Find("open_effect"):GetComponent(typeof(EffectWidget))
end


