﻿local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget
local InputFieldWidget=CS.InputFieldWidget

Mid_platform_guess_bet_panel={}
local this = Mid_platform_guess_bet_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.input_count_text=nil
this.bet_count_tip_text=nil
this.bet_confirm_btn=nil
this.bet_cancel_btn=nil
this.bet_input_field=nil

function this:init(gameObject)
	self.go=gameObject
	self.input_count_text=self.go.transform:Find("bet_confirm_panel/Image/input_count_text"):GetComponent(typeof(TextWidget))
	self.bet_count_tip_text=self.go.transform:Find("bet_confirm_panel/Image/bet_count_tip_text"):GetComponent(typeof(TextWidget))
	self.bet_confirm_btn=self.go.transform:Find("bet_confirm_panel/Image/bet_confirm_btn"):GetComponent(typeof(ButtonWidget))
	self.bet_cancel_btn=self.go.transform:Find("bet_confirm_panel/Image/bet_cancel_btn"):GetComponent(typeof(ButtonWidget))
	self.bet_input_field=self.go.transform:Find("bet_confirm_panel/Image/bet_input_field"):GetComponent(typeof(InputFieldWidget))
end


