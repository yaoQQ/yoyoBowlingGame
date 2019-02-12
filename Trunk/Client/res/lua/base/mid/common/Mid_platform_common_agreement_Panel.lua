local PanelWidget=CS.PanelWidget
local ButtonWidget=CS.ButtonWidget
local ScrollPanelWidget=CS.ScrollPanelWidget

Mid_platform_common_agreement_panel={}
local this = Mid_platform_common_agreement_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.platform_common_agreement_panel=nil
this.BtnAgreementBack=nil
this.ScrollPanelAgreement=nil

function this:init(gameObject)
	self.go=gameObject
	self.platform_common_agreement_panel=self.go.transform:Find(""):GetComponent(typeof(PanelWidget))
	self.BtnAgreementBack=self.go.transform:Find("BtnAgreementBack"):GetComponent(typeof(ButtonWidget))
	self.ScrollPanelAgreement=self.go.transform:Find("ScrollPanelAgreement"):GetComponent(typeof(ScrollPanelWidget))
end


