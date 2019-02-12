local IconWidget=CS.IconWidget

Mid_animal_pool_panel={}
local this = Mid_animal_pool_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.animalPre=nil
this.normal_icon=nil
this.selected_icon=nil

function this:init(gameObject)
	self.go=gameObject
	self.animalPre=self.go.transform:Find("animalPre"):GetComponent(typeof(IconWidget))
	self.normal_icon=self.go.transform:Find("normal_icon"):GetComponent(typeof(IconWidget))
	self.selected_icon=self.go.transform:Find("selected_icon"):GetComponent(typeof(IconWidget))
end


