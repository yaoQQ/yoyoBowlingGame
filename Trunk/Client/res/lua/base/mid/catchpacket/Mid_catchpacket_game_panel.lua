﻿local PanelWidget=CS.PanelWidget
local ImageWidget=CS.ImageWidget
local EffectWidget=CS.EffectWidget
local IconWidget=CS.IconWidget
local SpineWidget=CS.SpineWidget
local ButtonWidget=CS.ButtonWidget
local TextWidget=CS.TextWidget

Mid_catchpacket_game_panel={}
local this = Mid_catchpacket_game_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.map_panel=nil
this.itemPre=nil
this.flash_effect=nil
this.item_pool_icon=nil
this.mammon_spine=nil
this.pig_spine=nil
this.collider_panel=nil
this.collider_effect=nil
this.paralyzed_effect=nil
this.left_btn=nil
this.right_btn=nil
this.left_image=nil
this.right_image=nil
this.fog_image=nil
this.exit_btn=nil
this.timer_bg=nil
this.timer_text=nil
this.start_timer_icon=nil
this.over_panel=nil
this.over_money_text=nil
this.over_exit_btn=nil
this.popup_panel=nil
this.exit_confirm_btn=nil
this.exit_continue_btn=nil

function this:init(gameObject)
	self.go=gameObject
	self.map_panel=self.go.transform:Find("map_panel"):GetComponent(typeof(PanelWidget))
	self.itemPre=self.go.transform:Find("itemPre"):GetComponent(typeof(ImageWidget))
	self.flash_effect=self.go.transform:Find("itemPre/flash_effect"):GetComponent(typeof(EffectWidget))
	self.item_pool_icon=self.go.transform:Find("item_pool_icon"):GetComponent(typeof(IconWidget))
	self.mammon_spine=self.go.transform:Find("mammon_spine"):GetComponent(typeof(SpineWidget))
	self.pig_spine=self.go.transform:Find("pig_spine"):GetComponent(typeof(SpineWidget))
	self.collider_panel=self.go.transform:Find("pig_spine/collider_panel"):GetComponent(typeof(PanelWidget))
	self.collider_effect=self.go.transform:Find("pig_spine/collider_panel/collider_effect"):GetComponent(typeof(EffectWidget))
	self.paralyzed_effect=self.go.transform:Find("pig_spine/collider_panel/paralyzed_effect"):GetComponent(typeof(EffectWidget))
	self.left_btn=self.go.transform:Find("left_btn"):GetComponent(typeof(ButtonWidget))
	self.right_btn=self.go.transform:Find("right_btn"):GetComponent(typeof(ButtonWidget))
	self.left_image=self.go.transform:Find("left_image"):GetComponent(typeof(ImageWidget))
	self.right_image=self.go.transform:Find("right_image"):GetComponent(typeof(ImageWidget))
	self.fog_image=self.go.transform:Find("fog_image"):GetComponent(typeof(ImageWidget))
	self.exit_btn=self.go.transform:Find("exit_btn"):GetComponent(typeof(ButtonWidget))
	self.timer_bg=self.go.transform:Find("timer_bg"):GetComponent(typeof(ImageWidget))
	self.timer_text=self.go.transform:Find("timer_bg/timer_text"):GetComponent(typeof(TextWidget))
	self.start_timer_icon=self.go.transform:Find("start_timer_icon"):GetComponent(typeof(IconWidget))
	self.over_panel=self.go.transform:Find("over_panel"):GetComponent(typeof(PanelWidget))
	self.over_money_text=self.go.transform:Find("over_panel/Image (2)/over_money_text"):GetComponent(typeof(TextWidget))
	self.over_exit_btn=self.go.transform:Find("over_panel/Image (2)/over_exit_btn"):GetComponent(typeof(ButtonWidget))
	self.popup_panel=self.go.transform:Find("popup_panel"):GetComponent(typeof(PanelWidget))
	self.exit_confirm_btn=self.go.transform:Find("popup_panel/bg/exit_confirm_btn"):GetComponent(typeof(ButtonWidget))
	self.exit_continue_btn=self.go.transform:Find("popup_panel/bg/exit_continue_btn"):GetComponent(typeof(ButtonWidget))
end


