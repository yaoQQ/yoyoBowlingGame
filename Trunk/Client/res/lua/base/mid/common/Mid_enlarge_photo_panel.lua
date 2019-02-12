local BannerWidget=CS.BannerWidget
local ImageWidget=CS.ImageWidget
local EmptyImageWidget=CS.EmptyImageWidget
local TextWidget=CS.TextWidget

Mid_enlarge_photo_panel={}
local this = Mid_enlarge_photo_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.Banner=nil
this.top_bg=nil
this.back_image=nil
this.progress_text=nil
this.down_image=nil

function this:init(gameObject)
	self.go=gameObject
	self.Banner=self.go.transform:Find("Banner"):GetComponent(typeof(BannerWidget))
	self.top_bg=self.go.transform:Find("top_bg"):GetComponent(typeof(ImageWidget))
	self.back_image=self.go.transform:Find("top_bg/back_image"):GetComponent(typeof(EmptyImageWidget))
	self.progress_text=self.go.transform:Find("top_bg/progress_text"):GetComponent(typeof(TextWidget))
	self.down_image=self.go.transform:Find("top_bg/down_image"):GetComponent(typeof(EmptyImageWidget))
end


