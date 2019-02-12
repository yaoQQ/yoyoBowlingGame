local PanelWidget=CS.PanelWidget
local EmptyImageWidget=CS.EmptyImageWidget
local ImageWidget=CS.ImageWidget
local EffectWidget=CS.EffectWidget
local TextWidget=CS.TextWidget
local IconWidget=CS.IconWidget

Mid_platform_mall_tip_lands_panel={}
local this = Mid_platform_mall_tip_lands_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.platform_mall_tip_lands_panel=nil
this.noClick_Image=nil
this.point_Image=nil
this.mask_Image=nil
this.platform_mall_success_panel=nil
this.successeffect=nil
this.success_diamond_Image=nil
this.success_gold_Image=nil
this.success_Panel=nil
this.show_Image=nil
this.show_Text=nil
this.logoType_Icon=nil

function this:init(gameObject)
	self.go=gameObject
	self.platform_mall_tip_lands_panel=self.go.transform:Find(""):GetComponent(typeof(PanelWidget))
	self.noClick_Image=self.go.transform:Find("noClick_Image"):GetComponent(typeof(EmptyImageWidget))
	self.point_Image=self.go.transform:Find("point_Image"):GetComponent(typeof(EmptyImageWidget))
	self.mask_Image=self.go.transform:Find("mask_Image"):GetComponent(typeof(ImageWidget))
	self.platform_mall_success_panel=self.go.transform:Find("platform_mall_success_panel"):GetComponent(typeof(PanelWidget))
	self.successeffect=self.go.transform:Find("platform_mall_success_panel/Area/Image/successeffect"):GetComponent(typeof(EffectWidget))
	self.success_diamond_Image=self.go.transform:Find("platform_mall_success_panel/success_diamond_Image"):GetComponent(typeof(ImageWidget))
	self.success_gold_Image=self.go.transform:Find("platform_mall_success_panel/success_gold_Image"):GetComponent(typeof(ImageWidget))
	self.success_Panel=self.go.transform:Find("platform_mall_success_panel/success_Panel"):GetComponent(typeof(PanelWidget))
	self.show_Image=self.go.transform:Find("platform_mall_success_panel/success_Panel/show_Image"):GetComponent(typeof(ImageWidget))
	self.show_Text=self.go.transform:Find("platform_mall_success_panel/success_Panel/show_Image/show_Text"):GetComponent(typeof(TextWidget))
	self.logoType_Icon=self.go.transform:Find("platform_mall_success_panel/success_Panel/show_Image/show_Text/logoType_Icon"):GetComponent(typeof(IconWidget))
end


