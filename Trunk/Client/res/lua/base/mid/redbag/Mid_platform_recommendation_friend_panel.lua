local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget
local CellGroupWidget=CS.CellGroupWidget
local ButtonWidget=CS.ButtonWidget
local CircleImageWidget=CS.CircleImageWidget
local IconWidget=CS.IconWidget
local EmptyImageWidget=CS.EmptyImageWidget

Mid_platform_recommendation_friend_panel={}
local this = Mid_platform_recommendation_friend_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.recommendation_friend_bg_Image=nil
this.recommendation_friend_close_Image=nil
this.recommendation_friend_explain_Text=nil
this.recommendationFriendCellRecycleScrollPanel=nil
this.add_all_Button=nil
this.no_all_Button=nil
this.in_batch_Button=nil
--RecommendationFriendCell数组
this.recommendationFriendCellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.recommendation_friend_bg_Image=self.go.transform:Find("recommendation_friend/recommendation_friend_bg_Image"):GetComponent(typeof(ImageWidget))
	self.recommendation_friend_close_Image=self.go.transform:Find("recommendation_friend/recommendation_friend_middle_Image/recommendation_friend_close_Image"):GetComponent(typeof(ImageWidget))
	self.recommendation_friend_explain_Text=self.go.transform:Find("recommendation_friend/recommendation_friend_middle_Image/recommendation_friend_explain_Text"):GetComponent(typeof(TextWidget))
	self.recommendationFriendCellRecycleScrollPanel=self.go.transform:Find("recommendation_friend/recommendation_friend_middle_Image/recommendationFriendCellRecycleScrollPanel"):GetComponent(typeof(CellGroupWidget))
	self.add_all_Button=self.go.transform:Find("recommendation_friend/add_all_Button"):GetComponent(typeof(ButtonWidget))
	self.no_all_Button=self.go.transform:Find("recommendation_friend/no_all_Button"):GetComponent(typeof(ButtonWidget))
	self.in_batch_Button=self.go.transform:Find("recommendation_friend/in_batch_Button"):GetComponent(typeof(ButtonWidget))
	self.recommendationFriendCellArr={}
	table.insert(self.recommendationFriendCellArr,self.new_RecommendationFriendCell(self.go.transform:Find("recommendation_friend/recommendation_friend_middle_Image/recommendationFriendCellRecycleScrollPanel/cellitem").gameObject))
	table.insert(self.recommendationFriendCellArr,self.new_RecommendationFriendCell(self.go.transform:Find("recommendation_friend/recommendation_friend_middle_Image/recommendationFriendCellRecycleScrollPanel/cellitem_1").gameObject))
	table.insert(self.recommendationFriendCellArr,self.new_RecommendationFriendCell(self.go.transform:Find("recommendation_friend/recommendation_friend_middle_Image/recommendationFriendCellRecycleScrollPanel/cellitem_2").gameObject))
	table.insert(self.recommendationFriendCellArr,self.new_RecommendationFriendCell(self.go.transform:Find("recommendation_friend/recommendation_friend_middle_Image/recommendationFriendCellRecycleScrollPanel/cellitem_3").gameObject))
	table.insert(self.recommendationFriendCellArr,self.new_RecommendationFriendCell(self.go.transform:Find("recommendation_friend/recommendation_friend_middle_Image/recommendationFriendCellRecycleScrollPanel/cellitem_4").gameObject))
end

--RecommendationFriendCell复用单元
function this.new_RecommendationFriendCell(itemGo)
	local item = { }
	item.go = itemGo
	item.head_Icon=itemGo.transform:Find("head_Icon"):GetComponent(typeof(CircleImageWidget))
	item.sex_bg_Icon=itemGo.transform:Find("sex_bg_Icon"):GetComponent(typeof(IconWidget))
	item.sex_Icon=itemGo.transform:Find("sex_bg_Icon/sex_Icon"):GetComponent(typeof(IconWidget))
	item.name_Text=itemGo.transform:Find("name_Text"):GetComponent(typeof(TextWidget))
	item.introduce_Text=itemGo.transform:Find("introduce_Text"):GetComponent(typeof(TextWidget))
	item.press_Image=itemGo.transform:Find("press_Image"):GetComponent(typeof(EmptyImageWidget))
	item.add_friend_Button=itemGo.transform:Find("add_friend_Button"):GetComponent(typeof(ButtonWidget))
	item.distance_Text=itemGo.transform:Find("distance_Text"):GetComponent(typeof(TextWidget))
	item.add_end_Image=itemGo.transform:Find("add_end_Image"):GetComponent(typeof(ImageWidget))
	return item
end

