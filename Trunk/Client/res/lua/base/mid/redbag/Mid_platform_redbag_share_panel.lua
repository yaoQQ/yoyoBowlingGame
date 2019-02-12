local ImageWidget=CS.ImageWidget
local EmptyImageWidget=CS.EmptyImageWidget
local CircleImageWidget=CS.CircleImageWidget
local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget
local PanelWidget=CS.PanelWidget

Mid_platform_redbag_share_panel={}
local this = Mid_platform_redbag_share_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.mask_Image=nil
this.back_Image=nil
this.bg_Image=nil
this.head_Image=nil
this.area_Image=nil
this.title_Text=nil
this.packet_Text=nil
this.yoyo_Text=nil
this.dis_Text=nil
this.QR_Image=nil
this.left_button=nil
this.right_Button=nil
this.Image=nil
this.cancel_Image=nil
this.fail_Panel=nil
this.close_Image=nil

function this:init(gameObject)
	self.go=gameObject
	self.mask_Image=self.go.transform:Find("mask_Image"):GetComponent(typeof(ImageWidget))
	self.back_Image=self.go.transform:Find("top/back_Image/back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.bg_Image=self.go.transform:Find("share_ScrollPanel/content/panel/bg_Image"):GetComponent(typeof(ImageWidget))
	self.head_Image=self.go.transform:Find("share_ScrollPanel/content/panel/bg_Image/Panel/head_Image"):GetComponent(typeof(CircleImageWidget))
	self.area_Image=self.go.transform:Find("share_ScrollPanel/content/panel/bg_Image/area_Image"):GetComponent(typeof(EmptyImageWidget))
	self.title_Text=self.go.transform:Find("share_ScrollPanel/content/panel/bg_Image/title_Text"):GetComponent(typeof(TextWidget))
	self.packet_Text=self.go.transform:Find("share_ScrollPanel/content/panel/bg_Image/packet_Image/packet_Text"):GetComponent(typeof(TextWidget))
	self.yoyo_Text=self.go.transform:Find("share_ScrollPanel/content/panel/bg_Image/yoyo_Text"):GetComponent(typeof(TextWidget))
	self.dis_Text=self.go.transform:Find("share_ScrollPanel/content/panel/bg_Image/dis_Text"):GetComponent(typeof(TextWidget))
	self.QR_Image=self.go.transform:Find("share_ScrollPanel/content/panel/bg_Image/QR_Image"):GetComponent(typeof(ImageWidget))
	self.left_button=self.go.transform:Find("share_ScrollPanel/content/left_button"):GetComponent(typeof(ButtonWidget))
	self.right_Button=self.go.transform:Find("share_ScrollPanel/content/right_Button"):GetComponent(typeof(ButtonWidget))
	self.Image=self.go.transform:Find("share_ScrollPanel/content/cancel_Text/Image"):GetComponent(typeof(ImageWidget))
	self.cancel_Image=self.go.transform:Find("share_ScrollPanel/content/cancel_Text/cancel_Image"):GetComponent(typeof(EmptyImageWidget))
	self.fail_Panel=self.go.transform:Find("fail_Panel"):GetComponent(typeof(PanelWidget))
	self.close_Image=self.go.transform:Find("fail_Panel/bg_Image/close_Image"):GetComponent(typeof(EmptyImageWidget))
end


