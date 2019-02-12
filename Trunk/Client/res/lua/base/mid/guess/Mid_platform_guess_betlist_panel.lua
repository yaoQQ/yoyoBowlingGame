local CircleImageWidget=CS.CircleImageWidget
local TextWidget=CS.TextWidget
local IconWidget=CS.IconWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local PanelWidget=CS.PanelWidget
local ImageWidget=CS.ImageWidget
local ButtonWidget=CS.ButtonWidget

Mid_platform_guess_betlist_panel={}
local this = Mid_platform_guess_betlist_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.guest_team_image=nil
this.guest_team_name_text=nil
this.home_team_image=nil
this.home_team_name_text=nil
this.match_explain_text=nil
this.bets_state_icon=nil
this.bet_detail_scroll_list=nil
this.belong_text=nil
this.rule_panel=nil
this.rule_back_image=nil
--BetDetailCell数组
this.betDetailCellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.guest_team_image=self.go.transform:Find("Top_Panel/guest_bg/guest_team_image"):GetComponent(typeof(CircleImageWidget))
	self.guest_team_name_text=self.go.transform:Find("Top_Panel/guest_bg/guest_team_image/guest_team_name_text"):GetComponent(typeof(TextWidget))
	self.home_team_image=self.go.transform:Find("Top_Panel/home_bg/home_team_image"):GetComponent(typeof(CircleImageWidget))
	self.home_team_name_text=self.go.transform:Find("Top_Panel/home_bg/home_team_image/home_team_name_text"):GetComponent(typeof(TextWidget))
	self.match_explain_text=self.go.transform:Find("Top_Panel/match_explain_text"):GetComponent(typeof(TextWidget))
	self.bets_state_icon=self.go.transform:Find("Top_Panel/bets_state_icon"):GetComponent(typeof(IconWidget))
	self.bet_detail_scroll_list=self.go.transform:Find("Mid_panel/bet_detail_scroll_list"):GetComponent(typeof(CellRecycleScrollWidget))
	self.belong_text=self.go.transform:Find("belong_text"):GetComponent(typeof(TextWidget))
	self.rule_panel=self.go.transform:Find("rule_panel"):GetComponent(typeof(PanelWidget))
	self.rule_back_image=self.go.transform:Find("rule_panel/bg/rule_back_image"):GetComponent(typeof(ImageWidget))
	self.betDetailCellArr={}
	table.insert(self.betDetailCellArr,self.new_BetDetailCell(self.go.transform:Find("Mid_panel/bet_detail_scroll_list/content/cellitem").gameObject))
	table.insert(self.betDetailCellArr,self.new_BetDetailCell(self.go.transform:Find("Mid_panel/bet_detail_scroll_list/content/cellitem_1").gameObject))
	table.insert(self.betDetailCellArr,self.new_BetDetailCell(self.go.transform:Find("Mid_panel/bet_detail_scroll_list/content/cellitem_2").gameObject))
	table.insert(self.betDetailCellArr,self.new_BetDetailCell(self.go.transform:Find("Mid_panel/bet_detail_scroll_list/content/cellitem_3").gameObject))
	table.insert(self.betDetailCellArr,self.new_BetDetailCell(self.go.transform:Find("Mid_panel/bet_detail_scroll_list/content/cellitem_4").gameObject))
	table.insert(self.betDetailCellArr,self.new_BetDetailCell(self.go.transform:Find("Mid_panel/bet_detail_scroll_list/content/cellitem_5").gameObject))
	table.insert(self.betDetailCellArr,self.new_BetDetailCell(self.go.transform:Find("Mid_panel/bet_detail_scroll_list/content/cellitem_6").gameObject))
	table.insert(self.betDetailCellArr,self.new_BetDetailCell(self.go.transform:Find("Mid_panel/bet_detail_scroll_list/content/cellitem_7").gameObject))
end

--BetDetailCell复用单元
function this.new_BetDetailCell(itemGo)
	local item = { }
	item.go = itemGo
	item.bet_state_icon=itemGo.transform:Find("bet_state_icon"):GetComponent(typeof(IconWidget))
	item.bet_content_text=itemGo.transform:Find("bet_content_text"):GetComponent(typeof(TextWidget))
	item.segment_image=itemGo.transform:Find("segment_image"):GetComponent(typeof(ImageWidget))
	item.deadline_text=itemGo.transform:Find("deadline_text"):GetComponent(typeof(TextWidget))
	item.choose_state_icon_1=itemGo.transform:Find("choose_state_icon_1"):GetComponent(typeof(IconWidget))
	item.odds_text_1=itemGo.transform:Find("choose_state_icon_1/odds_text_1"):GetComponent(typeof(TextWidget))
	item.choose_flag_icon_1=itemGo.transform:Find("choose_state_icon_1/choose_flag_icon_1"):GetComponent(typeof(IconWidget))
	item.choose_state_icon_2=itemGo.transform:Find("choose_state_icon_2"):GetComponent(typeof(IconWidget))
	item.odds_text_2=itemGo.transform:Find("choose_state_icon_2/odds_text_2"):GetComponent(typeof(TextWidget))
	item.choose_flag_icon_2=itemGo.transform:Find("choose_state_icon_2/choose_flag_icon_2"):GetComponent(typeof(IconWidget))
	item.choose_state_icon_3=itemGo.transform:Find("choose_state_icon_3"):GetComponent(typeof(IconWidget))
	item.odds_text_3=itemGo.transform:Find("choose_state_icon_3/odds_text_3"):GetComponent(typeof(TextWidget))
	item.choose_flag_icon_3=itemGo.transform:Find("choose_state_icon_3/choose_flag_icon_3"):GetComponent(typeof(IconWidget))
	item.bet_rule_btn=itemGo.transform:Find("bet_rule_btn"):GetComponent(typeof(ButtonWidget))
	return item
end

