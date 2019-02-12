local SliderWidget=CS.SliderWidget
local EffectWidget=CS.EffectWidget

Mid_eliminate_load_panel={}
local this = Mid_eliminate_load_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.Slider=nil
this.handleeffect=nil

function this:init(gameObject)
	self.go=gameObject
	self.Slider=self.go.transform:Find("Slider"):GetComponent(typeof(SliderWidget))
	self.handleeffect=self.go.transform:Find("Slider/Handle Slide Area/Handle/handleeffect"):GetComponent(typeof(EffectWidget))
end


