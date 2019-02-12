local ImageWidget=CS.ImageWidget
local CircleImageWidget=CS.CircleImageWidget
local TextWidget=CS.TextWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local IconWidget=CS.IconWidget

Mid_platform_active_reward_panel={}
local this = Mid_platform_active_reward_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.closemask_Image=nil
this.back_Image=nil
this.change_type_Text=nil
this.rank_CellRecycleScrollPanel=nil
--RankCell数组
this.rankCellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.closemask_Image=self.go.transform:Find("closemask_Image"):GetComponent(typeof(ImageWidget))
	self.back_Image=self.go.transform:Find("back_Image"):GetComponent(typeof(CircleImageWidget))
	self.change_type_Text=self.go.transform:Find("change_type_Text"):GetComponent(typeof(TextWidget))
	self.rank_CellRecycleScrollPanel=self.go.transform:Find("rank_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.rankCellArr={}
	table.insert(self.rankCellArr,self.new_RankCell(self.go.transform:Find("rank_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.rankCellArr,self.new_RankCell(self.go.transform:Find("rank_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.rankCellArr,self.new_RankCell(self.go.transform:Find("rank_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.rankCellArr,self.new_RankCell(self.go.transform:Find("rank_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.rankCellArr,self.new_RankCell(self.go.transform:Find("rank_CellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.rankCellArr,self.new_RankCell(self.go.transform:Find("rank_CellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.rankCellArr,self.new_RankCell(self.go.transform:Find("rank_CellRecycleScrollPanel/content/cellitem_6").gameObject))
	table.insert(self.rankCellArr,self.new_RankCell(self.go.transform:Find("rank_CellRecycleScrollPanel/content/cellitem_7").gameObject))
	table.insert(self.rankCellArr,self.new_RankCell(self.go.transform:Find("rank_CellRecycleScrollPanel/content/cellitem_8").gameObject))
end

--RankCell复用单元
function this.new_RankCell(itemGo)
	local item = { }
	item.go = itemGo
	item.top_rank_Icon=itemGo.transform:Find("top_rank_Icon"):GetComponent(typeof(IconWidget))
	item.reward_Text=itemGo.transform:Find("reward_Text"):GetComponent(typeof(TextWidget))
	item.other_reward_Text=itemGo.transform:Find("other_reward_Text"):GetComponent(typeof(TextWidget))
	return item
end

