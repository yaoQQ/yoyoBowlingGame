local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget

Mid_animal_popup_panel={}
local this = Mid_animal_popup_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.bg_image=nil
this.info_text=nil
this.btn_1=nil
this.btn_2=nil

function this:init(gameObject)
	self.go=gameObject
	self.bg_image=self.go.transform:Find("bg_image"):GetComponent(typeof(ImageWidget))
	self.info_text=self.go.transform:Find("bg_image/info_text"):GetComponent(typeof(TextWidget))
	self.btn_1=self.go.transform:Find("bg_image/btn_1"):GetComponent(typeof(ButtonWidget))
	self.btn_2=self.go.transform:Find("bg_image/btn_2"):GetComponent(typeof(ButtonWidget))
end


