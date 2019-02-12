local ImageWidget=CS.ImageWidget
local EmptyImageWidget=CS.EmptyImageWidget

Mid_platform_redbag_withdraw_rules_panel={}
local this = Mid_platform_redbag_withdraw_rules_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.mask_Image=nil
this.background_Image=nil
this.close_Image=nil

function this:init(gameObject)
	self.go=gameObject
	self.mask_Image=self.go.transform:Find("record/mask_Image"):GetComponent(typeof(ImageWidget))
	self.background_Image=self.go.transform:Find("record/background_Image"):GetComponent(typeof(ImageWidget))
	self.close_Image=self.go.transform:Find("record/background_Image/close_Image/close_Image"):GetComponent(typeof(EmptyImageWidget))
end


