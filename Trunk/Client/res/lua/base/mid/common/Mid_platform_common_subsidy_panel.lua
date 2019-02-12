local PanelWidget=CS.PanelWidget
local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget
local IconWidget=CS.IconWidget
local ButtonWidget=CS.ButtonWidget
local EffectWidget=CS.EffectWidget
local EmptyImageWidget=CS.EmptyImageWidget

Mid_platform_common_subsidy_panel={}
local this = Mid_platform_common_subsidy_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.subsidy_Panel=nil
this.mask_Image=nil
this.title_Text=nil
this.left_gold_Icon=nil
this.left_gold_Text=nil
this.left_Button=nil
this.right_Button=nil
this.number_Text=nil
this.end_Panel=nil
this.platform_mall_success_panel=nil
this.successeffect=nil
this.success_diamond_Image=nil
this.success_gold_Image=nil
this.success_Panel=nil
this.show_Image=nil
this.show_Text=nil
this.logoType_Icon=nil
this.end_mask_Image=nil
this.point_Image=nil
this.noClick_Image=nil

function this:init(gameObject)
	self.go=gameObject
	self.subsidy_Panel=self.go.transform:Find("subsidy_Panel"):GetComponent(typeof(PanelWidget))
	self.mask_Image=self.go.transform:Find("subsidy_Panel/mask_Image"):GetComponent(typeof(ImageWidget))
	self.title_Text=self.go.transform:Find("subsidy_Panel/title_Text"):GetComponent(typeof(TextWidget))
	self.left_gold_Icon=self.go.transform:Find("subsidy_Panel/left_bg_Image/left_gold_Icon"):GetComponent(typeof(IconWidget))
	self.left_gold_Text=self.go.transform:Find("subsidy_Panel/left_bg_Image/left_gold_Text"):GetComponent(typeof(TextWidget))
	self.left_Button=self.go.transform:Find("subsidy_Panel/left_Button"):GetComponent(typeof(ButtonWidget))
	self.right_Button=self.go.transform:Find("subsidy_Panel/right_Button"):GetComponent(typeof(ButtonWidget))
	self.number_Text=self.go.transform:Find("subsidy_Panel/number_Text"):GetComponent(typeof(TextWidget))
	self.end_Panel=self.go.transform:Find("end_Panel"):GetComponent(typeof(PanelWidget))
	self.platform_mall_success_panel=self.go.transform:Find("end_Panel/platform_mall_success_panel"):GetComponent(typeof(PanelWidget))
	self.successeffect=self.go.transform:Find("end_Panel/platform_mall_success_panel/Area/Image/successeffect"):GetComponent(typeof(EffectWidget))
	self.success_diamond_Image=self.go.transform:Find("end_Panel/platform_mall_success_panel/success_diamond_Image"):GetComponent(typeof(ImageWidget))
	self.success_gold_Image=self.go.transform:Find("end_Panel/platform_mall_success_panel/success_gold_Image"):GetComponent(typeof(ImageWidget))
	self.success_Panel=self.go.transform:Find("end_Panel/platform_mall_success_panel/success_Panel"):GetComponent(typeof(PanelWidget))
	self.show_Image=self.go.transform:Find("end_Panel/platform_mall_success_panel/success_Panel/show_Image"):GetComponent(typeof(ImageWidget))
	self.show_Text=self.go.transform:Find("end_Panel/platform_mall_success_panel/success_Panel/show_Image/show_Text"):GetComponent(typeof(TextWidget))
	self.logoType_Icon=self.go.transform:Find("end_Panel/platform_mall_success_panel/success_Panel/show_Image/show_Text/logoType_Icon"):GetComponent(typeof(IconWidget))
	self.end_mask_Image=self.go.transform:Find("end_Panel/end_mask_Image"):GetComponent(typeof(ImageWidget))
	self.point_Image=self.go.transform:Find("end_Panel/point_Image"):GetComponent(typeof(EmptyImageWidget))
	self.noClick_Image=self.go.transform:Find("end_Panel/noClick_Image"):GetComponent(typeof(EmptyImageWidget))
end


