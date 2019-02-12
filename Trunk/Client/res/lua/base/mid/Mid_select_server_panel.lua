﻿local InputFieldWidget=CS.InputFieldWidget
local ButtonWidget=CS.ButtonWidget

Mid_select_server_panel={}
local this = Mid_select_server_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.inputfield_ip=nil
this.inputfield_port=nil
this.inputfield_account=nil
this.btn_connect=nil
this.btn_server1=nil
this.btn_server2=nil
this.btn_token_connect=nil

function this:init(gameObject)
	self.go=gameObject
	self.inputfield_ip=self.go.transform:Find("Panel/inputfield_ip"):GetComponent(typeof(InputFieldWidget))
	self.inputfield_port=self.go.transform:Find("Panel/inputfield_port"):GetComponent(typeof(InputFieldWidget))
	self.inputfield_account=self.go.transform:Find("Panel/inputfield_account"):GetComponent(typeof(InputFieldWidget))
	self.btn_connect=self.go.transform:Find("btn_connect"):GetComponent(typeof(ButtonWidget))
	self.btn_server1=self.go.transform:Find("btn_server1"):GetComponent(typeof(ButtonWidget))
	self.btn_server2=self.go.transform:Find("btn_server2"):GetComponent(typeof(ButtonWidget))
	self.btn_token_connect=self.go.transform:Find("btn_token_connect"):GetComponent(typeof(ButtonWidget))
end

