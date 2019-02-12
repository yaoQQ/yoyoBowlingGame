﻿local ImageWidget=CS.ImageWidget
local ButtonWidget=CS.ButtonWidget
local PanelWidget=CS.PanelWidget
local ToggleWidget=CS.ToggleWidget
local IconWidget=CS.IconWidget
local InputFieldWidget=CS.InputFieldWidget

Mid_eliminate_lobby_panel={}
local this = Mid_eliminate_lobby_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.title_image=nil
this.quick_enter_btn=nil
this.create_btn=nil
this.enter_btn=nil
this.exit_btn=nil
this.quick_enter_mask=nil
this.quick_enter_back_btn=nil
this.create_mask=nil
this.toggle_1=nil
this.toggle_2=nil
this.toggle_3=nil
this.create_back_btn=nil
this.create_confirm_btn=nil
this.enter_mask=nil
this.title_icon=nil
this.inputField=nil
this.enter_back_btn=nil

function this:init(gameObject)
	self.go=gameObject
	self.title_image=self.go.transform:Find("bg/title_image"):GetComponent(typeof(ImageWidget))
	self.quick_enter_btn=self.go.transform:Find("quick_enter_btn"):GetComponent(typeof(ButtonWidget))
	self.create_btn=self.go.transform:Find("create_btn"):GetComponent(typeof(ButtonWidget))
	self.enter_btn=self.go.transform:Find("enter_btn"):GetComponent(typeof(ButtonWidget))
	self.exit_btn=self.go.transform:Find("exit_btn"):GetComponent(typeof(ButtonWidget))
	self.quick_enter_mask=self.go.transform:Find("quick_enter_mask"):GetComponent(typeof(ImageWidget))
	self.quick_enter_back_btn=self.go.transform:Find("quick_enter_mask/mask/quick_enter_back_btn"):GetComponent(typeof(ButtonWidget))
	self.create_mask=self.go.transform:Find("create_mask"):GetComponent(typeof(PanelWidget))
	self.toggle_1=self.go.transform:Find("create_mask/bg_image/toggle_group/toggle_1"):GetComponent(typeof(ToggleWidget))
	self.toggle_2=self.go.transform:Find("create_mask/bg_image/toggle_group/toggle_2"):GetComponent(typeof(ToggleWidget))
	self.toggle_3=self.go.transform:Find("create_mask/bg_image/toggle_group/toggle_3"):GetComponent(typeof(ToggleWidget))
	self.create_back_btn=self.go.transform:Find("create_mask/create_back_btn"):GetComponent(typeof(ButtonWidget))
	self.create_confirm_btn=self.go.transform:Find("create_mask/create_confirm_btn"):GetComponent(typeof(ButtonWidget))
	self.enter_mask=self.go.transform:Find("enter_mask"):GetComponent(typeof(ImageWidget))
	self.title_icon=self.go.transform:Find("enter_mask/input_bg/title_icon"):GetComponent(typeof(IconWidget))
	self.inputField=self.go.transform:Find("enter_mask/input_bg/inputField"):GetComponent(typeof(InputFieldWidget))
	self.enter_back_btn=self.go.transform:Find("enter_mask/input_bg/enter_back_btn"):GetComponent(typeof(ButtonWidget))
end


