﻿local PanelWidget=CS.PanelWidget
local ImageWidget=CS.ImageWidget
local CellGroupWidget=CS.CellGroupWidget
local IconWidget=CS.IconWidget
local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget
local CircleImageWidget=CS.CircleImageWidget

Mid_animal_game_panel={}
local this = Mid_animal_game_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.animal_game_panel=nil
this.map_image=nil
this.direction_group=nil
this.round_icon=nil
this.score_bg2=nil
this.score_text=nil
this.back_btn=nil
this.capitulate_btn=nil
this.me_round_panel=nil
this.me_round_me_seat_icon=nil
this.me_round_me_head_image=nil
this.me_round_me_name_bg=nil
this.me_round_me_name_text=nil
this.me_round_timer_text=nil
this.me_round_icon=nil
this.me_round_other_seat_icon=nil
this.me_round_other_head_image=nil
this.me_round_other_name_bg=nil
this.me_round_other_name_text=nil
this.other_round_panel=nil
this.other_round_me_seat_icon=nil
this.other_round_me_head_image=nil
this.other_round_me_name_bg=nil
this.other_round_me_name_text=nil
this.other_round_other_seat_icon=nil
this.other_round_timer_text=nil
this.other_round_icon=nil
this.other_round_other_head_image=nil
this.other_round_other_name_bg=nil
this.other_round_other_name_text=nil
this.tip_image=nil
this.tip_text=nil
--DirectionItem数组
this.directionItemArr={}
--MeSurviveItem数组
this.meSurviveItemArr={}
--MeRoundNextItem数组
this.meRoundNextItemArr={}
--OtherSurviveItem数组
this.otherSurviveItemArr={}

function this:init(gameObject)
	self.go=gameObject
	self.animal_game_panel=self.go.transform:Find(""):GetComponent(typeof(PanelWidget))
	self.map_image=self.go.transform:Find("map_image"):GetComponent(typeof(ImageWidget))
	self.direction_group=self.go.transform:Find("map_image/direction_group"):GetComponent(typeof(CellGroupWidget))
	self.round_icon=self.go.transform:Find("round_icon"):GetComponent(typeof(IconWidget))
	self.score_bg2=self.go.transform:Find("score_bg1/score_bg2"):GetComponent(typeof(ImageWidget))
	self.score_text=self.go.transform:Find("score_bg1/score_bg2/score_text"):GetComponent(typeof(TextWidget))
	self.back_btn=self.go.transform:Find("back_btn"):GetComponent(typeof(ButtonWidget))
	self.capitulate_btn=self.go.transform:Find("capitulate_btn"):GetComponent(typeof(ButtonWidget))
	self.me_round_panel=self.go.transform:Find("me_round_panel"):GetComponent(typeof(PanelWidget))
	self.me_round_me_seat_icon=self.go.transform:Find("me_round_panel/me_round_me_seat_icon"):GetComponent(typeof(IconWidget))
	self.me_round_me_head_image=self.go.transform:Find("me_round_panel/me_round_me_seat_icon/Image/me_round_me_head_image"):GetComponent(typeof(CircleImageWidget))
	self.me_round_me_name_bg=self.go.transform:Find("me_round_panel/me_round_me_seat_icon/me_round_me_name_bg"):GetComponent(typeof(ImageWidget))
	self.me_round_me_name_text=self.go.transform:Find("me_round_panel/me_round_me_seat_icon/me_round_me_name_bg/me_round_me_name_text"):GetComponent(typeof(TextWidget))
	self.me_round_timer_text=self.go.transform:Find("me_round_panel/me_round_me_seat_icon/me_round_clock_image/me_round_timer_text"):GetComponent(typeof(TextWidget))
	self.me_round_icon=self.go.transform:Find("me_round_panel/me_round_me_seat_icon/me_round_clock_image/me_round_icon"):GetComponent(typeof(IconWidget))
	self.me_round_other_seat_icon=self.go.transform:Find("me_round_panel/me_round_other_seat_icon"):GetComponent(typeof(IconWidget))
	self.me_round_other_head_image=self.go.transform:Find("me_round_panel/me_round_other_seat_icon/Image/me_round_other_head_image"):GetComponent(typeof(CircleImageWidget))
	self.me_round_other_name_bg=self.go.transform:Find("me_round_panel/me_round_other_seat_icon/me_round_other_name_bg"):GetComponent(typeof(ImageWidget))
	self.me_round_other_name_text=self.go.transform:Find("me_round_panel/me_round_other_seat_icon/me_round_other_name_bg/me_round_other_name_text"):GetComponent(typeof(TextWidget))
	self.other_round_panel=self.go.transform:Find("other_round_panel"):GetComponent(typeof(PanelWidget))
	self.other_round_me_seat_icon=self.go.transform:Find("other_round_panel/other_round_me_seat_icon"):GetComponent(typeof(IconWidget))
	self.other_round_me_head_image=self.go.transform:Find("other_round_panel/other_round_me_seat_icon/Image/other_round_me_head_image"):GetComponent(typeof(CircleImageWidget))
	self.other_round_me_name_bg=self.go.transform:Find("other_round_panel/other_round_me_seat_icon/other_round_me_name_bg"):GetComponent(typeof(ImageWidget))
	self.other_round_me_name_text=self.go.transform:Find("other_round_panel/other_round_me_seat_icon/other_round_me_name_bg/other_round_me_name_text"):GetComponent(typeof(TextWidget))
	self.other_round_other_seat_icon=self.go.transform:Find("other_round_panel/other_round_other_seat_icon"):GetComponent(typeof(IconWidget))
	self.other_round_timer_text=self.go.transform:Find("other_round_panel/other_round_other_seat_icon/other_round_clock_image/other_round_timer_text"):GetComponent(typeof(TextWidget))
	self.other_round_icon=self.go.transform:Find("other_round_panel/other_round_other_seat_icon/other_round_clock_image/other_round_icon"):GetComponent(typeof(IconWidget))
	self.other_round_other_head_image=self.go.transform:Find("other_round_panel/other_round_other_seat_icon/Image/other_round_other_head_image"):GetComponent(typeof(CircleImageWidget))
	self.other_round_other_name_bg=self.go.transform:Find("other_round_panel/other_round_other_seat_icon/other_round_other_name_bg"):GetComponent(typeof(ImageWidget))
	self.other_round_other_name_text=self.go.transform:Find("other_round_panel/other_round_other_seat_icon/other_round_other_name_bg/other_round_other_name_text"):GetComponent(typeof(TextWidget))
	self.tip_image=self.go.transform:Find("tip_image"):GetComponent(typeof(ImageWidget))
	self.tip_text=self.go.transform:Find("tip_image/tip_text"):GetComponent(typeof(TextWidget))
	self.directionItemArr={}
	table.insert(self.directionItemArr,self.new_DirectionItem(self.go.transform:Find("map_image/direction_group/CellItem").gameObject))
	table.insert(self.directionItemArr,self.new_DirectionItem(self.go.transform:Find("map_image/direction_group/CellItem_1").gameObject))
	table.insert(self.directionItemArr,self.new_DirectionItem(self.go.transform:Find("map_image/direction_group/CellItem_2").gameObject))
	table.insert(self.directionItemArr,self.new_DirectionItem(self.go.transform:Find("map_image/direction_group/CellItem_3").gameObject))
	self.meSurviveItemArr={}
	table.insert(self.meSurviveItemArr,self.new_MeSurviveItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/animal_group/CellItem").gameObject))
	table.insert(self.meSurviveItemArr,self.new_MeSurviveItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/animal_group/CellItem_1").gameObject))
	table.insert(self.meSurviveItemArr,self.new_MeSurviveItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/animal_group/CellItem_2").gameObject))
	table.insert(self.meSurviveItemArr,self.new_MeSurviveItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/animal_group/CellItem_3").gameObject))
	table.insert(self.meSurviveItemArr,self.new_MeSurviveItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/animal_group/CellItem_4").gameObject))
	table.insert(self.meSurviveItemArr,self.new_MeSurviveItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/animal_group/CellItem_5").gameObject))
	table.insert(self.meSurviveItemArr,self.new_MeSurviveItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/animal_group/CellItem_6").gameObject))
	table.insert(self.meSurviveItemArr,self.new_MeSurviveItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/animal_group/CellItem_7").gameObject))
	table.insert(self.meSurviveItemArr,self.new_MeSurviveItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/animal_group/CellItem_8").gameObject))
	self.meRoundNextItemArr={}
	table.insert(self.meRoundNextItemArr,self.new_MeRoundNextItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/next_group/CellItem").gameObject))
	table.insert(self.meRoundNextItemArr,self.new_MeRoundNextItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/next_group/CellItem_1").gameObject))
	table.insert(self.meRoundNextItemArr,self.new_MeRoundNextItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/next_group/CellItem_2").gameObject))
	table.insert(self.meRoundNextItemArr,self.new_MeRoundNextItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/next_group/CellItem_3").gameObject))
	table.insert(self.meRoundNextItemArr,self.new_MeRoundNextItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/next_group/CellItem_4").gameObject))
	table.insert(self.meRoundNextItemArr,self.new_MeRoundNextItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/next_group/CellItem_5").gameObject))
	table.insert(self.meRoundNextItemArr,self.new_MeRoundNextItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/next_group/CellItem_6").gameObject))
	table.insert(self.meRoundNextItemArr,self.new_MeRoundNextItem(self.go.transform:Find("me_round_panel/me_round_me_seat_icon/next_group/CellItem_7").gameObject))
	self.otherSurviveItemArr={}
	table.insert(self.otherSurviveItemArr,self.new_OtherSurviveItem(self.go.transform:Find("other_round_panel/other_round_other_seat_icon/animal_group/CellItem").gameObject))
	table.insert(self.otherSurviveItemArr,self.new_OtherSurviveItem(self.go.transform:Find("other_round_panel/other_round_other_seat_icon/animal_group/CellItem_1").gameObject))
	table.insert(self.otherSurviveItemArr,self.new_OtherSurviveItem(self.go.transform:Find("other_round_panel/other_round_other_seat_icon/animal_group/CellItem_2").gameObject))
	table.insert(self.otherSurviveItemArr,self.new_OtherSurviveItem(self.go.transform:Find("other_round_panel/other_round_other_seat_icon/animal_group/CellItem_3").gameObject))
	table.insert(self.otherSurviveItemArr,self.new_OtherSurviveItem(self.go.transform:Find("other_round_panel/other_round_other_seat_icon/animal_group/CellItem_4").gameObject))
	table.insert(self.otherSurviveItemArr,self.new_OtherSurviveItem(self.go.transform:Find("other_round_panel/other_round_other_seat_icon/animal_group/CellItem_5").gameObject))
	table.insert(self.otherSurviveItemArr,self.new_OtherSurviveItem(self.go.transform:Find("other_round_panel/other_round_other_seat_icon/animal_group/CellItem_6").gameObject))
	table.insert(self.otherSurviveItemArr,self.new_OtherSurviveItem(self.go.transform:Find("other_round_panel/other_round_other_seat_icon/animal_group/CellItem_7").gameObject))
	table.insert(self.otherSurviveItemArr,self.new_OtherSurviveItem(self.go.transform:Find("other_round_panel/other_round_other_seat_icon/animal_group/CellItem_8").gameObject))
end

--DirectionItem复用单元
function this.new_DirectionItem(itemGo)
	local item = { }
	item.go = itemGo
	item.direction_icon=itemGo.transform:Find("direction_icon"):GetComponent(typeof(IconWidget))
	return item
end
--MeSurviveItem复用单元
function this.new_MeSurviveItem(itemGo)
	local item = { }
	item.go = itemGo
	item.animal_icon=itemGo.transform:Find("animal_icon"):GetComponent(typeof(IconWidget))
	return item
end
--MeRoundNextItem复用单元
function this.new_MeRoundNextItem(itemGo)
	local item = { }
	item.go = itemGo
	item.next_image=itemGo.transform:Find("next_image"):GetComponent(typeof(ImageWidget))
	return item
end
--OtherSurviveItem复用单元
function this.new_OtherSurviveItem(itemGo)
	local item = { }
	item.go = itemGo
	item.animal_icon=itemGo.transform:Find("animal_icon"):GetComponent(typeof(IconWidget))
	return item
end

