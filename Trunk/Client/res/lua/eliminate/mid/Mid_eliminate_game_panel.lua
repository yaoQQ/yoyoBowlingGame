local PanelWidget=CS.PanelWidget
local ImageWidget=CS.ImageWidget
local ButtonWidget=CS.ButtonWidget
local TextWidget=CS.TextWidget
local IconWidget=CS.IconWidget

Mid_eliminate_game_panel={}
local this = Mid_eliminate_game_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.game_control_panel=nil
this.stage_render=nil
this.adv_image=nil
this.question_btn=nil
this.exit_btn=nil
this.game_bg=nil
this.playerScore_text=nil
this.count_fg=nil
this.fre_left_fg=nil
this.fre_right_fg=nil
this.over_timer_text=nil
this.tip_icon=nil
this.combo_img=nil
this.combo_symbol_text=nil
this.combo_text=nil
this.pieceBg_container=nil
this.piece_container=nil
this.shuffle_btn=nil
this.tip_text=nil
this.evaluate_image=nil
this.start_timer_image=nil
this.timeUp_image=nil
this.mask=nil
this.item_container=nil
this.item_tip_text=nil
--Player_dui_item数组
this.player_dui_itemArr={}

function this:init(gameObject)
	self.go=gameObject
	self.game_control_panel=self.go.transform:Find("game_control_panel"):GetComponent(typeof(PanelWidget))
	self.stage_render=self.go.transform:Find("stage_render"):GetComponent(typeof(ImageWidget))
	self.adv_image=self.go.transform:Find("Image/adv_image"):GetComponent(typeof(ImageWidget))
	self.question_btn=self.go.transform:Find("question_btn"):GetComponent(typeof(ButtonWidget))
	self.exit_btn=self.go.transform:Find("exit_btn"):GetComponent(typeof(ButtonWidget))
	self.game_bg=self.go.transform:Find("game_bg"):GetComponent(typeof(ImageWidget))
	self.playerScore_text=self.go.transform:Find("game_bg/playerScore_text"):GetComponent(typeof(TextWidget))
	self.count_fg=self.go.transform:Find("game_bg/count_fg"):GetComponent(typeof(ImageWidget))
	self.fre_left_fg=self.go.transform:Find("game_bg/fre_left_fg"):GetComponent(typeof(ImageWidget))
	self.fre_right_fg=self.go.transform:Find("game_bg/fre_right_fg"):GetComponent(typeof(ImageWidget))
	self.over_timer_text=self.go.transform:Find("game_bg/over_timer_text"):GetComponent(typeof(TextWidget))
	self.tip_icon=self.go.transform:Find("game_bg/tip_icon"):GetComponent(typeof(IconWidget))
	self.combo_img=self.go.transform:Find("game_bg/combo_img"):GetComponent(typeof(ImageWidget))
	self.combo_symbol_text=self.go.transform:Find("game_bg/combo_img/combo_symbol_text"):GetComponent(typeof(TextWidget))
	self.combo_text=self.go.transform:Find("game_bg/combo_img/combo_text"):GetComponent(typeof(TextWidget))
	self.pieceBg_container=self.go.transform:Find("pieceBg_container"):GetComponent(typeof(PanelWidget))
	self.piece_container=self.go.transform:Find("piece_container"):GetComponent(typeof(PanelWidget))
	self.shuffle_btn=self.go.transform:Find("shuffle_btn"):GetComponent(typeof(ButtonWidget))
	self.tip_text=self.go.transform:Find("shuffle_btn/tip_text"):GetComponent(typeof(TextWidget))
	self.evaluate_image=self.go.transform:Find("evaluate_image"):GetComponent(typeof(ImageWidget))
	self.start_timer_image=self.go.transform:Find("start_timer_image"):GetComponent(typeof(ImageWidget))
	self.timeUp_image=self.go.transform:Find("timeUp_image"):GetComponent(typeof(ImageWidget))
	self.mask=self.go.transform:Find("mask"):GetComponent(typeof(ImageWidget))
	self.item_container=self.go.transform:Find("item_container"):GetComponent(typeof(PanelWidget))
	self.item_tip_text=self.go.transform:Find("item_tip_text"):GetComponent(typeof(TextWidget))
	self.player_dui_itemArr={}
	table.insert(self.player_dui_itemArr,self.new_Player_dui_item(self.go.transform:Find("player_dui_group/CellItem").gameObject))
	table.insert(self.player_dui_itemArr,self.new_Player_dui_item(self.go.transform:Find("player_dui_group/CellItem_1").gameObject))
	table.insert(self.player_dui_itemArr,self.new_Player_dui_item(self.go.transform:Find("player_dui_group/CellItem_2").gameObject))
	table.insert(self.player_dui_itemArr,self.new_Player_dui_item(self.go.transform:Find("player_dui_group/CellItem_3").gameObject))
end

--Player_dui_item复用单元
function this.new_Player_dui_item(itemGo)
	local item = { }
	item.go = itemGo
	item.name_text=itemGo.transform:Find("name_text"):GetComponent(typeof(TextWidget))
	item.score_text=itemGo.transform:Find("score_text"):GetComponent(typeof(TextWidget))
	item.arrow_image=itemGo.transform:Find("arrow_image"):GetComponent(typeof(ImageWidget))
	return item
end

