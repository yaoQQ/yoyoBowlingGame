local ButtonWidget=CS.ButtonWidget
local TextWidget=CS.TextWidget

Mid_platform_example_xxx_panel={}
local this = Mid_platform_example_xxx_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.Button=nil
this.Text=nil

function this:init(gameObject)
	self.go=gameObject
	self.Button=self.go.transform:Find("Button"):GetComponent(typeof(ButtonWidget))
	self.Text=self.go.transform:Find("Image/Text"):GetComponent(typeof(TextWidget))
end


