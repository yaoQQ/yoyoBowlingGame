﻿local TextWidget=CS.TextWidget
local EmptyImageWidget=CS.EmptyImageWidget
local PanelWidget=CS.PanelWidget
local InputFieldWidget=CS.InputFieldWidget
local ButtonWidget=CS.ButtonWidget
local IconWidget=CS.IconWidget
local ImageWidget=CS.ImageWidget

Mid_personal_single_change_panel={}
local this = Mid_personal_single_change_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.change_type_Text=nil
this.back_Image=nil
this.name_Panel=nil
this.name_InputField=nil
this.word_left_Text=nil
this.name_prompt_Text=nil
this.close_Button=nil
this.done_Button=nil
this.sex_Panel=nil
this.male_Icon=nil
this.male_check_Image=nil
this.female_Icon=nil
this.female_check_Image=nil
this.warning_Text=nil
this.done_Sex_Button=nil

function this:init(gameObject)
	self.go=gameObject
	self.change_type_Text=self.go.transform:Find("change_type_Text"):GetComponent(typeof(TextWidget))
	self.back_Image=self.go.transform:Find("back_Image/back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.name_Panel=self.go.transform:Find("name_Panel"):GetComponent(typeof(PanelWidget))
	self.name_InputField=self.go.transform:Find("name_Panel/name_InputField"):GetComponent(typeof(InputFieldWidget))
	self.word_left_Text=self.go.transform:Find("name_Panel/word_left_Text"):GetComponent(typeof(TextWidget))
	self.name_prompt_Text=self.go.transform:Find("name_Panel/name_prompt_Text"):GetComponent(typeof(TextWidget))
	self.close_Button=self.go.transform:Find("name_Panel/close_Button"):GetComponent(typeof(ButtonWidget))
	self.done_Button=self.go.transform:Find("name_Panel/done_Button"):GetComponent(typeof(ButtonWidget))
	self.sex_Panel=self.go.transform:Find("sex_Panel"):GetComponent(typeof(PanelWidget))
	self.male_Icon=self.go.transform:Find("sex_Panel/male_Icon"):GetComponent(typeof(IconWidget))
	self.male_check_Image=self.go.transform:Find("sex_Panel/male_check_Image"):GetComponent(typeof(ImageWidget))
	self.female_Icon=self.go.transform:Find("sex_Panel/female_Icon"):GetComponent(typeof(IconWidget))
	self.female_check_Image=self.go.transform:Find("sex_Panel/female_check_Image"):GetComponent(typeof(ImageWidget))
	self.warning_Text=self.go.transform:Find("sex_Panel/warning_Text"):GetComponent(typeof(TextWidget))
	self.done_Sex_Button=self.go.transform:Find("sex_Panel/done_Sex_Button"):GetComponent(typeof(ButtonWidget))
end


