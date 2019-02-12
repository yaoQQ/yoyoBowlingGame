local ImageWidget=CS.ImageWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local ButtonWidget=CS.ButtonWidget
local TextWidget=CS.TextWidget
local IconWidget=CS.IconWidget

Mid_eliminate_over_panel={}
local this = Mid_eliminate_over_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.over_bg=nil
this.rank_scroll_panel=nil
this.exit_btn=nil
this.me_score_text=nil
this.me_rank_text=nil
--RangItem数组
this.rangItemArr={}

function this:init(gameObject)
	self.go=gameObject
	self.over_bg=self.go.transform:Find("over_bg"):GetComponent(typeof(ImageWidget))
	self.rank_scroll_panel=self.go.transform:Find("over_bg/rank_scroll_panel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.exit_btn=self.go.transform:Find("over_bg/exit_btn"):GetComponent(typeof(ButtonWidget))
	self.me_score_text=self.go.transform:Find("over_bg/me_score_text"):GetComponent(typeof(TextWidget))
	self.me_rank_text=self.go.transform:Find("over_bg/me_rank_text"):GetComponent(typeof(TextWidget))
	self.rangItemArr={}
	table.insert(self.rangItemArr,self.new_RangItem(self.go.transform:Find("over_bg/rank_scroll_panel/content/cellitem").gameObject))
	table.insert(self.rangItemArr,self.new_RangItem(self.go.transform:Find("over_bg/rank_scroll_panel/content/cellitem_1").gameObject))
	table.insert(self.rangItemArr,self.new_RangItem(self.go.transform:Find("over_bg/rank_scroll_panel/content/cellitem_2").gameObject))
	table.insert(self.rangItemArr,self.new_RangItem(self.go.transform:Find("over_bg/rank_scroll_panel/content/cellitem_3").gameObject))
	table.insert(self.rangItemArr,self.new_RangItem(self.go.transform:Find("over_bg/rank_scroll_panel/content/cellitem_4").gameObject))
	table.insert(self.rangItemArr,self.new_RangItem(self.go.transform:Find("over_bg/rank_scroll_panel/content/cellitem_5").gameObject))
	table.insert(self.rangItemArr,self.new_RangItem(self.go.transform:Find("over_bg/rank_scroll_panel/content/cellitem_6").gameObject))
end

--RangItem复用单元
function this.new_RangItem(itemGo)
	local item = { }
	item.go = itemGo
	item.rank_text=itemGo.transform:Find("rank_text"):GetComponent(typeof(TextWidget))
	item.name_text=itemGo.transform:Find("name_text"):GetComponent(typeof(TextWidget))
	item.score_text=itemGo.transform:Find("score_text"):GetComponent(typeof(TextWidget))
	item.reward_text=itemGo.transform:Find("reward_text"):GetComponent(typeof(TextWidget))
	item.reward_icon=itemGo.transform:Find("reward_text/reward_icon"):GetComponent(typeof(IconWidget))
	return item
end

