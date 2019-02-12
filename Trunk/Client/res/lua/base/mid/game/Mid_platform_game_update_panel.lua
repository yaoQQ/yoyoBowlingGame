﻿local ImageWidget=CS.ImageWidget
local PanelWidget=CS.PanelWidget
local EmptyImageWidget=CS.EmptyImageWidget
local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget

Mid_platform_game_update_panel={}
local this = Mid_platform_game_update_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.mask_image=nil
this.game_down_panel=nil
this.close_down_image=nil
this.down_game_image=nil
this.down_process_fg=nil
this.down_game_text=nil
this.down_percent_text=nil
this.down_capacity_text=nil
this.down_cancel_btn=nil
this.update_mask_image=nil
this.update_panel=nil
this.update_text=nil
this.update_left_btn=nil
this.update_right_btn=nil

function this:init(gameObject)
	self.go=gameObject
	self.mask_image=self.go.transform:Find("mask_image"):GetComponent(typeof(ImageWidget))
	self.game_down_panel=self.go.transform:Find("game_down_panel"):GetComponent(typeof(PanelWidget))
	self.close_down_image=self.go.transform:Find("game_down_panel/close_down_image"):GetComponent(typeof(EmptyImageWidget))
	self.down_game_image=self.go.transform:Find("game_down_panel/down_game_bg/down_game_image"):GetComponent(typeof(ImageWidget))
	self.down_process_fg=self.go.transform:Find("game_down_panel/down_process_bg/down_process_fg"):GetComponent(typeof(ImageWidget))
	self.down_game_text=self.go.transform:Find("game_down_panel/down_game_text"):GetComponent(typeof(TextWidget))
	self.down_percent_text=self.go.transform:Find("game_down_panel/down_percent_text"):GetComponent(typeof(TextWidget))
	self.down_capacity_text=self.go.transform:Find("game_down_panel/down_capacity_text"):GetComponent(typeof(TextWidget))
	self.down_cancel_btn=self.go.transform:Find("game_down_panel/down_cancel_btn"):GetComponent(typeof(ButtonWidget))
	self.update_mask_image=self.go.transform:Find("update_mask_image"):GetComponent(typeof(ImageWidget))
	self.update_panel=self.go.transform:Find("update_panel"):GetComponent(typeof(PanelWidget))
	self.update_text=self.go.transform:Find("update_panel/update_text"):GetComponent(typeof(TextWidget))
	self.update_left_btn=self.go.transform:Find("update_panel/update_left_btn"):GetComponent(typeof(ButtonWidget))
	self.update_right_btn=self.go.transform:Find("update_panel/update_right_btn"):GetComponent(typeof(ButtonWidget))
end


