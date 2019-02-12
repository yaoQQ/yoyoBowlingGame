local ImageWidget=CS.ImageWidget
local EmptyImageWidget=CS.EmptyImageWidget
local TextWidget=CS.TextWidget
local ScrollPanelWidget=CS.ScrollPanelWidget
local ButtonWidget=CS.ButtonWidget

Mid_platform_official_reward_panel={}
local this = Mid_platform_official_reward_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.mask=nil
this.gameIcon=nil
this.back_Image=nil
this.titleGamename=nil
this.gamedscri=nil
this.ScrollPanel=nil
this.reward_parent=nil
this.rewardTitle=nil
this.rewardTxt =nil
this.ruletitle=nil
this.ruleTxt=nil
this.timeCount_txt=nil
this.enterGameBtn=nil
this.enter_game_text=nil

function this:init(gameObject)
	self.go=gameObject
	self.mask=self.go.transform:Find("mask"):GetComponent(typeof(ImageWidget))
	self.gameIcon=self.go.transform:Find("gameIcon bg/gameIcon"):GetComponent(typeof(ImageWidget))
	self.back_Image=self.go.transform:Find("back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.titleGamename=self.go.transform:Find("titleGamename"):GetComponent(typeof(TextWidget))
	self.gamedscri=self.go.transform:Find("gamedscri"):GetComponent(typeof(TextWidget))
	self.ScrollPanel=self.go.transform:Find("ScrollPanel"):GetComponent(typeof(ScrollPanelWidget))
	self.reward_parent=self.go.transform:Find("ScrollPanel/content/parent/reward_parent"):GetComponent(typeof(EmptyImageWidget))
	self.rewardTitle=self.go.transform:Find("ScrollPanel/content/parent/reward_parent/rewardTitle"):GetComponent(typeof(TextWidget))
	self.rewardTxt =self.go.transform:Find("ScrollPanel/content/parent/reward_parent/rewardTitle/rewardTxt "):GetComponent(typeof(TextWidget))
	self.ruletitle=self.go.transform:Find("ScrollPanel/content/parent/ruletitle"):GetComponent(typeof(TextWidget))
	self.ruleTxt=self.go.transform:Find("ScrollPanel/content/parent/ruletitle/ruleTxt"):GetComponent(typeof(TextWidget))
	self.timeCount_txt=self.go.transform:Find("timeCount_txt"):GetComponent(typeof(TextWidget))
	self.enterGameBtn=self.go.transform:Find("enterGameBtn"):GetComponent(typeof(ButtonWidget))
	self.enter_game_text=self.go.transform:Find("enterGameBtn/enter_game_text"):GetComponent(typeof(TextWidget))
end


