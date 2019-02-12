local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget
local PanelWidget=CS.PanelWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local EmptyImageWidget=CS.EmptyImageWidget

Mid_platform_game_rule_panel={}
local this = Mid_platform_game_rule_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.mask_image=nil
this.bg_image=nil
this.rule_title_text=nil
this.rule_content_text=nil
this.enter_game_btn=nil
this.game_shot_panel=nil
this.game_shot_scroll_panel=nil
this.close_rule_image=nil
--ShotItem数组
this.shotItemArr={}

function this:init(gameObject)
	self.go=gameObject
	self.mask_image=self.go.transform:Find("mask_image"):GetComponent(typeof(ImageWidget))
	self.bg_image=self.go.transform:Find("bg_image"):GetComponent(typeof(ImageWidget))
	self.rule_title_text=self.go.transform:Find("bg_image/rule_title_text"):GetComponent(typeof(TextWidget))
	self.rule_content_text=self.go.transform:Find("bg_image/rule_content_text"):GetComponent(typeof(TextWidget))
	self.enter_game_btn=self.go.transform:Find("bg_image/enter_game_btn"):GetComponent(typeof(ButtonWidget))
	self.game_shot_panel=self.go.transform:Find("bg_image/game_shot_panel"):GetComponent(typeof(PanelWidget))
	self.game_shot_scroll_panel=self.go.transform:Find("bg_image/game_shot_panel/game_shot_scroll_panel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.close_rule_image=self.go.transform:Find("bg_image/close_rule_image"):GetComponent(typeof(EmptyImageWidget))
	self.shotItemArr={}
	table.insert(self.shotItemArr,self.new_ShotItem(self.go.transform:Find("bg_image/game_shot_panel/game_shot_scroll_panel/content/cellitem").gameObject))
	table.insert(self.shotItemArr,self.new_ShotItem(self.go.transform:Find("bg_image/game_shot_panel/game_shot_scroll_panel/content/cellitem_1").gameObject))
	table.insert(self.shotItemArr,self.new_ShotItem(self.go.transform:Find("bg_image/game_shot_panel/game_shot_scroll_panel/content/cellitem_2").gameObject))
	table.insert(self.shotItemArr,self.new_ShotItem(self.go.transform:Find("bg_image/game_shot_panel/game_shot_scroll_panel/content/cellitem_3").gameObject))
end

--ShotItem复用单元
function this.new_ShotItem(itemGo)
	local item = { }
	item.go = itemGo
	item.game_shot_image=itemGo.transform:Find("game_shot_image"):GetComponent(typeof(ImageWidget))
	return item
end

