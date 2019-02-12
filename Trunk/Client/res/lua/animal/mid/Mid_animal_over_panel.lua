local IconWidget=CS.IconWidget
local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget

Mid_animal_over_panel={}
local this = Mid_animal_over_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.result_icon=nil
this.bg_image=nil
this.score_text=nil
this.rank_text=nil
this.exit_btn=nil

function this:init(gameObject)
	self.go=gameObject
	self.result_icon=self.go.transform:Find("result_icon"):GetComponent(typeof(IconWidget))
	self.bg_image=self.go.transform:Find("bg_image"):GetComponent(typeof(ImageWidget))
	self.score_text=self.go.transform:Find("bg_image/score_text"):GetComponent(typeof(TextWidget))
	self.rank_text=self.go.transform:Find("bg_image/rank_text"):GetComponent(typeof(TextWidget))
	self.exit_btn=self.go.transform:Find("bg_image/exit_btn"):GetComponent(typeof(ButtonWidget))
end


