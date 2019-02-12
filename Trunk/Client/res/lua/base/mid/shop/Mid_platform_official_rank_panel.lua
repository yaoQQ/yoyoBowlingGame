local ImageWidget=CS.ImageWidget
local CircleImageWidget=CS.CircleImageWidget
local EmptyImageWidget=CS.EmptyImageWidget
local TextWidget=CS.TextWidget

Mid_platform_official_rank_panel={}
local this = Mid_platform_official_rank_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.mask=nil
this.user_head_image=nil
this.back_Image=nil
this.title_rank=nil
this.name=nil
this.cur_rank=nil
this.game_title=nil

function this:init(gameObject)
	self.go=gameObject
	self.mask=self.go.transform:Find("mask"):GetComponent(typeof(ImageWidget))
	self.user_head_image=self.go.transform:Find("user_head_image"):GetComponent(typeof(CircleImageWidget))
	self.back_Image=self.go.transform:Find("back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.title_rank=self.go.transform:Find("title_rank"):GetComponent(typeof(TextWidget))
	self.name=self.go.transform:Find("name"):GetComponent(typeof(TextWidget))
	self.cur_rank=self.go.transform:Find("cur_rank"):GetComponent(typeof(TextWidget))
	self.game_title=self.go.transform:Find("game_title"):GetComponent(typeof(TextWidget))
end


