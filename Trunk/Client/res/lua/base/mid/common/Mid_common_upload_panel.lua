local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget
local EffectWidget=CS.EffectWidget

Mid_common_upload_panel={}
local this = Mid_common_upload_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.mask_iamge=nil
this.tip_image=nil
this.title_text=nil
this.upload_effect=nil

function this:init(gameObject)
	self.go=gameObject
	self.mask_iamge=self.go.transform:Find("mask_iamge"):GetComponent(typeof(ImageWidget))
	self.tip_image=self.go.transform:Find("tip_image"):GetComponent(typeof(ImageWidget))
	self.title_text=self.go.transform:Find("tip_image/title_text"):GetComponent(typeof(TextWidget))
	self.upload_effect=self.go.transform:Find("tip_image/title_text/upload_effect"):GetComponent(typeof(EffectWidget))
end


