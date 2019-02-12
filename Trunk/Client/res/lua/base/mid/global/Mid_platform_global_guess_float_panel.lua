local EmptyImageWidget=CS.EmptyImageWidget
local ImageWidget=CS.ImageWidget
local CircleImageWidget=CS.CircleImageWidget
local TextWidget=CS.TextWidget

Mid_platform_global_guess_float_panel={}
local this = Mid_platform_global_guess_float_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.panelParent=nil
this.guess_Image=nil
this.head_Image=nil
this.timesbg=nil
this.times=nil

function this:init(gameObject)
	self.go=gameObject
	self.panelParent=self.go.transform:Find("panelParent"):GetComponent(typeof(EmptyImageWidget))
	self.guess_Image=self.go.transform:Find("panelParent/guess_Image"):GetComponent(typeof(ImageWidget))
	self.head_Image=self.go.transform:Find("panelParent/head_Image"):GetComponent(typeof(CircleImageWidget))
	self.timesbg=self.go.transform:Find("panelParent/timesbg"):GetComponent(typeof(EmptyImageWidget))
	self.times=self.go.transform:Find("panelParent/timesbg/times"):GetComponent(typeof(TextWidget))
end


