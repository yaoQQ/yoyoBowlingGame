﻿local PanelWidget=CS.PanelWidget
local ButtonWidget=CS.ButtonWidget
local InputFieldWidget=CS.InputFieldWidget
local IconWidget=CS.IconWidget
local ImageWidget=CS.ImageWidget

Mid_login_panel={}
local this = Mid_login_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.top_Panel=nil
this.BtnRegisterBack=nil
this.BtnResetBack=nil
this.PageMain=nil
this.PageLogin=nil
this.InputFieldLoginAccount=nil
this.InputFieldLoginPassword=nil
this.BtnLoginSee=nil
this.IconLoginSee=nil
this.BtnLoginReset=nil
this.BtnMainRegister=nil
this.BtnLoginLogin=nil
this.bottom_panel=nil
this.BtnMainLoginAlipay=nil
this.BtnMainLoginWx=nil
this.BtnMainLoginPhone=nil
this.BtnUserAgreement=nil
this.PageRegister=nil
this.InputFieldRegisterAccount=nil
this.InputFieldRegisterAuth=nil
this.InputFieldRegisterPassword=nil
this.BtnRegisterSee=nil
this.IconRegisterSee=nil
this.BtnRegisterAuth=nil
this.BtnRegisterRegister=nil
this.PageReset=nil
this.InputFieldResetAccount=nil
this.InputFieldResetAuth=nil
this.InputFieldResetPassword=nil
this.BtnResetSee=nil
this.IconResetSee=nil
this.BtnResetAuth=nil
this.BtnResetReset=nil
this.pagewechat=nil
this.BtnWeChatClose=nil

function this:init(gameObject)
	self.go=gameObject
	self.top_Panel=self.go.transform:Find("top_Panel"):GetComponent(typeof(PanelWidget))
	self.BtnRegisterBack=self.go.transform:Find("top_Panel/BtnRegisterBack"):GetComponent(typeof(ButtonWidget))
	self.BtnResetBack=self.go.transform:Find("top_Panel/BtnResetBack"):GetComponent(typeof(ButtonWidget))
	self.PageMain=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain"):GetComponent(typeof(PanelWidget))
	self.PageLogin=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain/mid_Panel/PageLogin"):GetComponent(typeof(PanelWidget))
	self.InputFieldLoginAccount=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain/mid_Panel/PageLogin/Panel/InputFieldLoginAccount"):GetComponent(typeof(InputFieldWidget))
	self.InputFieldLoginPassword=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain/mid_Panel/PageLogin/Panel/InputFieldLoginPassword"):GetComponent(typeof(InputFieldWidget))
	self.BtnLoginSee=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain/mid_Panel/PageLogin/Panel/InputFieldLoginPassword/BtnLoginSee"):GetComponent(typeof(ButtonWidget))
	self.IconLoginSee=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain/mid_Panel/PageLogin/Panel/InputFieldLoginPassword/BtnLoginSee/IconLoginSee"):GetComponent(typeof(IconWidget))
	self.BtnLoginReset=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain/mid_Panel/PageLogin/Panel/BtnLoginReset"):GetComponent(typeof(ButtonWidget))
	self.BtnMainRegister=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain/mid_Panel/PageLogin/Panel/BtnMainRegister"):GetComponent(typeof(ButtonWidget))
	self.BtnLoginLogin=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain/mid_Panel/PageLogin/Panel/BtnLoginLogin"):GetComponent(typeof(ButtonWidget))
	self.bottom_panel=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain/bottom_panel"):GetComponent(typeof(PanelWidget))
	self.BtnMainLoginAlipay=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain/bottom_panel/BtnMainLoginAlipay"):GetComponent(typeof(ButtonWidget))
	self.BtnMainLoginWx=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain/bottom_panel/BtnMainLoginWx"):GetComponent(typeof(ButtonWidget))
	self.BtnMainLoginPhone=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain/bottom_panel/BtnMainLoginPhone"):GetComponent(typeof(ButtonWidget))
	self.BtnUserAgreement=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageMain/Text/BtnUserAgreement"):GetComponent(typeof(ButtonWidget))
	self.PageRegister=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageRegister"):GetComponent(typeof(PanelWidget))
	self.InputFieldRegisterAccount=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageRegister/Panel/InputFieldRegisterAccount"):GetComponent(typeof(InputFieldWidget))
	self.InputFieldRegisterAuth=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageRegister/Panel/InputFieldRegisterAuth"):GetComponent(typeof(InputFieldWidget))
	self.InputFieldRegisterPassword=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageRegister/Panel/InputFieldRegisterPassword"):GetComponent(typeof(InputFieldWidget))
	self.BtnRegisterSee=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageRegister/Panel/InputFieldRegisterPassword/BtnRegisterSee"):GetComponent(typeof(ButtonWidget))
	self.IconRegisterSee=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageRegister/Panel/InputFieldRegisterPassword/BtnRegisterSee/IconRegisterSee"):GetComponent(typeof(IconWidget))
	self.BtnRegisterAuth=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageRegister/Panel/BtnRegisterAuth"):GetComponent(typeof(ButtonWidget))
	self.BtnRegisterRegister=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageRegister/Panel/BtnRegisterRegister"):GetComponent(typeof(ButtonWidget))
	self.PageReset=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageReset"):GetComponent(typeof(PanelWidget))
	self.InputFieldResetAccount=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageReset/Panel/InputFieldResetAccount"):GetComponent(typeof(InputFieldWidget))
	self.InputFieldResetAuth=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageReset/Panel/InputFieldResetAuth"):GetComponent(typeof(InputFieldWidget))
	self.InputFieldResetPassword=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageReset/Panel/InputFieldResetPassword"):GetComponent(typeof(InputFieldWidget))
	self.BtnResetSee=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageReset/Panel/InputFieldResetPassword/BtnResetSee"):GetComponent(typeof(ButtonWidget))
	self.IconResetSee=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageReset/Panel/InputFieldResetPassword/BtnResetSee/IconResetSee"):GetComponent(typeof(IconWidget))
	self.BtnResetAuth=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageReset/Panel/BtnResetAuth"):GetComponent(typeof(ButtonWidget))
	self.BtnResetReset=self.go.transform:Find("ScrollPanel/content/mid_Panel/PageReset/Panel/BtnResetReset"):GetComponent(typeof(ButtonWidget))
	self.pagewechat=self.go.transform:Find("ScrollPanel/content/mid_Panel/pagewechat"):GetComponent(typeof(ImageWidget))
	self.BtnWeChatClose=self.go.transform:Find("ScrollPanel/content/mid_Panel/pagewechat/BtnWeChatClose"):GetComponent(typeof(ButtonWidget))
end

