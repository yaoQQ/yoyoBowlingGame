﻿local EmptyImageWidget=CS.EmptyImageWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local IconWidget=CS.IconWidget
local ImageWidget=CS.ImageWidget
local CircleImageWidget=CS.CircleImageWidget
local TextWidget=CS.TextWidget

Mid_platform_guess_roomlist_panel={}
local this = Mid_platform_guess_roomlist_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.back_image=nil
this.bets_scroll_list=nil
--EncounterItem数组
this.encounterItemArr={}

function this:init(gameObject)
	self.go=gameObject
	self.back_image=self.go.transform:Find("back_image/back_image"):GetComponent(typeof(EmptyImageWidget))
	self.bets_scroll_list=self.go.transform:Find("bets_scroll_list"):GetComponent(typeof(CellRecycleScrollWidget))
	self.encounterItemArr={}
	table.insert(self.encounterItemArr,self.new_EncounterItem(self.go.transform:Find("bets_scroll_list/content/cellitem").gameObject))
	table.insert(self.encounterItemArr,self.new_EncounterItem(self.go.transform:Find("bets_scroll_list/content/cellitem_1").gameObject))
	table.insert(self.encounterItemArr,self.new_EncounterItem(self.go.transform:Find("bets_scroll_list/content/cellitem_2").gameObject))
	table.insert(self.encounterItemArr,self.new_EncounterItem(self.go.transform:Find("bets_scroll_list/content/cellitem_3").gameObject))
	table.insert(self.encounterItemArr,self.new_EncounterItem(self.go.transform:Find("bets_scroll_list/content/cellitem_4").gameObject))
	table.insert(self.encounterItemArr,self.new_EncounterItem(self.go.transform:Find("bets_scroll_list/content/cellitem_5").gameObject))
	table.insert(self.encounterItemArr,self.new_EncounterItem(self.go.transform:Find("bets_scroll_list/content/cellitem_6").gameObject))
	table.insert(self.encounterItemArr,self.new_EncounterItem(self.go.transform:Find("bets_scroll_list/content/cellitem_7").gameObject))
end

--EncounterItem复用单元
function this.new_EncounterItem(itemGo)
	local item = { }
	item.go = itemGo
	item.bg_icon=itemGo.transform:Find("bg_icon"):GetComponent(typeof(IconWidget))
	item.belong_image=itemGo.transform:Find("belong_image"):GetComponent(typeof(ImageWidget))
	item.home_team_image=itemGo.transform:Find("home_bg/home_team_image"):GetComponent(typeof(CircleImageWidget))
	item.home_team_name_text=itemGo.transform:Find("home_bg/home_team_image/home_team_name_text"):GetComponent(typeof(TextWidget))
	item.guest_team_image=itemGo.transform:Find("guest_bg/guest_team_image"):GetComponent(typeof(CircleImageWidget))
	item.guest_team_name_text=itemGo.transform:Find("guest_bg/guest_team_image/guest_team_name_text"):GetComponent(typeof(TextWidget))
	item.bets_state_icon=itemGo.transform:Find("bets_state_icon"):GetComponent(typeof(IconWidget))
	item.belong_text=itemGo.transform:Find("belong_text"):GetComponent(typeof(TextWidget))
	item.match_time_text=itemGo.transform:Find("match_time_text"):GetComponent(typeof(TextWidget))
	return item
end

