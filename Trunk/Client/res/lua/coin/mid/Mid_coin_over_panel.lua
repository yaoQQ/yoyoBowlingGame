﻿local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget

Mid_coin_over_panel={}
local this = Mid_coin_over_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.score_text=nil
this.me_rank_text=nil
this.exit_btn=nil

function this:init(gameObject)
	self.go=gameObject
	self.score_text=self.go.transform:Find("Image/score_text"):GetComponent(typeof(TextWidget))
	self.me_rank_text=self.go.transform:Find("Image/me_rank_text"):GetComponent(typeof(TextWidget))
	self.exit_btn=self.go.transform:Find("Image/exit_btn"):GetComponent(typeof(ButtonWidget))
end


