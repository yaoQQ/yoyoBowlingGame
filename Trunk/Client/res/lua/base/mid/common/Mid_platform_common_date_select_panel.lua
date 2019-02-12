local ImageWidget=CS.ImageWidget
local NumberPickerWidget=CS.NumberPickerWidget
local ButtonWidget=CS.ButtonWidget

Mid_platform_common_date_select_panel={}
local this = Mid_platform_common_date_select_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.mask_image=nil
this.year_picker=nil
this.cancel_btn=nil
this.confirm_btn=nil
this.month_picker=nil
this.day_picker=nil

function this:init(gameObject)
	self.go=gameObject
	self.mask_image=self.go.transform:Find("mask_image"):GetComponent(typeof(ImageWidget))
	self.year_picker=self.go.transform:Find("Image/year_picker"):GetComponent(typeof(NumberPickerWidget))
	self.cancel_btn=self.go.transform:Find("Image/cancel_btn"):GetComponent(typeof(ButtonWidget))
	self.confirm_btn=self.go.transform:Find("Image/confirm_btn"):GetComponent(typeof(ButtonWidget))
	self.month_picker=self.go.transform:Find("Image/month_picker"):GetComponent(typeof(NumberPickerWidget))
	self.day_picker=self.go.transform:Find("Image/day_picker"):GetComponent(typeof(NumberPickerWidget))
end


