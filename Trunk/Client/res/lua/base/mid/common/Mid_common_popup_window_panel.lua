local PanelWidget=CS.PanelWidget
local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget

Mid_common_popup_window_panel={}
local this = Mid_common_popup_window_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.popup_panel=nil
this.mask=nil
this.bg_image=nil
this.info_text=nil
this.title_text=nil
this.buttons_container=nil
this.btn_1=nil
this.btn_2=nil

function this:init(gameObject)
	self.go=gameObject
	self.popup_panel=self.go.transform:Find("popup_panel"):GetComponent(typeof(PanelWidget))
	self.mask=self.go.transform:Find("popup_panel/mask"):GetComponent(typeof(ImageWidget))
	self.bg_image=self.go.transform:Find("popup_panel/bg_image"):GetComponent(typeof(ImageWidget))
	self.info_text=self.go.transform:Find("popup_panel/bg_image/info_text"):GetComponent(typeof(TextWidget))
	self.title_text=self.go.transform:Find("popup_panel/bg_image/info_text/title_text"):GetComponent(typeof(TextWidget))
	self.buttons_container=self.go.transform:Find("popup_panel/bg_image/buttons_container"):GetComponent(typeof(PanelWidget))
	self.btn_1=self.go.transform:Find("popup_panel/bg_image/buttons_container/btn_1"):GetComponent(typeof(ButtonWidget))
	self.btn_2=self.go.transform:Find("popup_panel/bg_image/buttons_container/btn_2"):GetComponent(typeof(ButtonWidget))
end


