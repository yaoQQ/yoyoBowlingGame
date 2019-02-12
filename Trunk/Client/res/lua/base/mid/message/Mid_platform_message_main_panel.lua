﻿local PanelWidget=CS.PanelWidget
local EmptyImageWidget=CS.EmptyImageWidget
local TextWidget=CS.TextWidget
local ScrollPanelWidget=CS.ScrollPanelWidget
local ImageWidget=CS.ImageWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local ButtonWidget=CS.ButtonWidget
local IconWidget=CS.IconWidget

Mid_platform_message_main_panel={}
local this = Mid_platform_message_main_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.answer_Panel=nil
this.answer_back_Image=nil
this.pagename_Text=nil
this.answer_title_Text=nil
this.time_Text=nil
this.answer_ScrollPanel=nil
this.answer_text=nil
this.mid_reward_Panel=nil
this.mask_Image=nil
this.bg_Image=nil
this.reward_CellRecycleScrollPanel=nil
this.Text=nil
this.closereward_Button=nil
this.buttom_Panel=nil
this.go_active_Panel=nil
this.go_activity_Button=nil
this.reward_Panel=nil
this.see_reward_Image=nil
this.achieve_Button=nil
this.get_Image=nil
--RewardCelll数组
this.rewardCelllArr={}

function this:init(gameObject)
	self.go=gameObject
	self.answer_Panel=self.go.transform:Find("answer_Panel"):GetComponent(typeof(PanelWidget))
	self.answer_back_Image=self.go.transform:Find("answer_Panel/top_panel/answer_back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.pagename_Text=self.go.transform:Find("answer_Panel/top_panel/pagename_Text"):GetComponent(typeof(TextWidget))
	self.answer_title_Text=self.go.transform:Find("answer_Panel/answer_title_Text"):GetComponent(typeof(TextWidget))
	self.time_Text=self.go.transform:Find("answer_Panel/time_Text"):GetComponent(typeof(TextWidget))
	self.answer_ScrollPanel=self.go.transform:Find("answer_Panel/answer_ScrollPanel"):GetComponent(typeof(ScrollPanelWidget))
	self.answer_text=self.go.transform:Find("answer_Panel/answer_ScrollPanel/content/answer_text"):GetComponent(typeof(TextWidget))
	self.mid_reward_Panel=self.go.transform:Find("mid_reward_Panel"):GetComponent(typeof(PanelWidget))
	self.mask_Image=self.go.transform:Find("mid_reward_Panel/mask_Image"):GetComponent(typeof(ImageWidget))
	self.bg_Image=self.go.transform:Find("mid_reward_Panel/bg_Image"):GetComponent(typeof(ImageWidget))
	self.reward_CellRecycleScrollPanel=self.go.transform:Find("mid_reward_Panel/bg_Image/reward_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.Text=self.go.transform:Find("mid_reward_Panel/bg_Image/Text"):GetComponent(typeof(TextWidget))
	self.closereward_Button=self.go.transform:Find("mid_reward_Panel/bg_Image/closereward_Button"):GetComponent(typeof(ButtonWidget))
	self.buttom_Panel=self.go.transform:Find("buttom_Panel"):GetComponent(typeof(PanelWidget))
	self.go_active_Panel=self.go.transform:Find("buttom_Panel/go_active_Panel"):GetComponent(typeof(ImageWidget))
	self.go_activity_Button=self.go.transform:Find("buttom_Panel/go_active_Panel/go_activity_Button"):GetComponent(typeof(ButtonWidget))
	self.reward_Panel=self.go.transform:Find("buttom_Panel/reward_Panel"):GetComponent(typeof(PanelWidget))
	self.see_reward_Image=self.go.transform:Find("buttom_Panel/reward_Panel/see_reward_Image"):GetComponent(typeof(EmptyImageWidget))
	self.achieve_Button=self.go.transform:Find("buttom_Panel/reward_Panel/achieve_Button"):GetComponent(typeof(ButtonWidget))
	self.get_Image=self.go.transform:Find("buttom_Panel/reward_Panel/get_Image"):GetComponent(typeof(ImageWidget))
	self.rewardCelllArr={}
	table.insert(self.rewardCelllArr,self.new_RewardCelll(self.go.transform:Find("mid_reward_Panel/bg_Image/reward_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.rewardCelllArr,self.new_RewardCelll(self.go.transform:Find("mid_reward_Panel/bg_Image/reward_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.rewardCelllArr,self.new_RewardCelll(self.go.transform:Find("mid_reward_Panel/bg_Image/reward_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.rewardCelllArr,self.new_RewardCelll(self.go.transform:Find("mid_reward_Panel/bg_Image/reward_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.rewardCelllArr,self.new_RewardCelll(self.go.transform:Find("mid_reward_Panel/bg_Image/reward_CellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.rewardCelllArr,self.new_RewardCelll(self.go.transform:Find("mid_reward_Panel/bg_Image/reward_CellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.rewardCelllArr,self.new_RewardCelll(self.go.transform:Find("mid_reward_Panel/bg_Image/reward_CellRecycleScrollPanel/content/cellitem_6").gameObject))
	table.insert(self.rewardCelllArr,self.new_RewardCelll(self.go.transform:Find("mid_reward_Panel/bg_Image/reward_CellRecycleScrollPanel/content/cellitem_7").gameObject))
end

--RewardCelll复用单元
function this.new_RewardCelll(itemGo)
	local item = { }
	item.go = itemGo
	item.reward_Text=itemGo.transform:Find("reward_Text"):GetComponent(typeof(TextWidget))
	item.noget_reward_Icon=itemGo.transform:Find("noget_reward_Icon"):GetComponent(typeof(IconWidget))
	item.get_reward_Icon=itemGo.transform:Find("get_reward_Icon"):GetComponent(typeof(IconWidget))
	return item
end

