local PanelWidget=CS.PanelWidget
local AnimationWidget=CS.AnimationWidget
local ImageWidget=CS.ImageWidget
local IconWidget=CS.IconWidget
local EffectWidget=CS.EffectWidget
local TextWidget=CS.TextWidget
local EmptyImageWidget=CS.EmptyImageWidget

Mid_platform_global_float_panel={}
local this = Mid_platform_global_float_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.panelParent=nil
this.animation_panel=nil
this.bg_Image=nil
this.official_title=nil
this.arrowImage=nil
this.gameImage=nil
this.rewardImage=nil
this.redbagtopeffect=nil
this.titlebg=nil
this.titletxt=nil
this.toppanel=nil
this.redbag_Image=nil
this.timesbg=nil
this.times=nil
this.pressbg=nil

function this:init(gameObject)
	self.go=gameObject
	self.panelParent=self.go.transform:Find("panelParent"):GetComponent(typeof(PanelWidget))
	self.animation_panel=self.go.transform:Find("panelParent/animation_panel"):GetComponent(typeof(AnimationWidget))
	self.bg_Image=self.go.transform:Find("panelParent/animation_panel/bg_Image"):GetComponent(typeof(ImageWidget))
	self.official_title=self.go.transform:Find("panelParent/animation_panel/official_title"):GetComponent(typeof(IconWidget))
	self.arrowImage=self.go.transform:Find("panelParent/animation_panel/arrowImage"):GetComponent(typeof(ImageWidget))
	self.gameImage=self.go.transform:Find("panelParent/animation_panel/gameImage"):GetComponent(typeof(ImageWidget))
	self.rewardImage=self.go.transform:Find("panelParent/animation_panel/rewardImage"):GetComponent(typeof(IconWidget))
	self.redbagtopeffect=self.go.transform:Find("panelParent/animation_panel/redbagtopeffect"):GetComponent(typeof(EffectWidget))
	self.titlebg=self.go.transform:Find("panelParent/animation_panel/titlebg"):GetComponent(typeof(ImageWidget))
	self.titletxt=self.go.transform:Find("panelParent/animation_panel/titlebg/titletxt"):GetComponent(typeof(TextWidget))
	self.toppanel=self.go.transform:Find("panelParent/animation_panel/toppanel"):GetComponent(typeof(PanelWidget))
	self.redbag_Image=self.go.transform:Find("panelParent/animation_panel/toppanel/redbag_Image"):GetComponent(typeof(IconWidget))
	self.timesbg=self.go.transform:Find("panelParent/animation_panel/toppanel/timesbg"):GetComponent(typeof(ImageWidget))
	self.times=self.go.transform:Find("panelParent/animation_panel/toppanel/timesbg/times"):GetComponent(typeof(TextWidget))
	self.pressbg=self.go.transform:Find("panelParent/animation_panel/pressbg"):GetComponent(typeof(EmptyImageWidget))
end


