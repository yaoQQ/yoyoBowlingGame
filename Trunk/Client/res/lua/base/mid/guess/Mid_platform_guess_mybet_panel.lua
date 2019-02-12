﻿local EmptyImageWidget=CS.EmptyImageWidget
local CircleImageWidget=CS.CircleImageWidget
local TextWidget=CS.TextWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local ImageWidget=CS.ImageWidget

Mid_platform_guess_mybet_panel={}
local this = Mid_platform_guess_mybet_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.btn_return=nil
this.head_image=nil
this.money_text=nil
this.my_bet_scroll_list=nil
--MybetCell数组
this.mybetCellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.btn_return=self.go.transform:Find("btn_return/btn_return"):GetComponent(typeof(EmptyImageWidget))
	self.head_image=self.go.transform:Find("user_data/head_image"):GetComponent(typeof(CircleImageWidget))
	self.money_text=self.go.transform:Find("user_data/money_type_text/money_bg_image/money_text"):GetComponent(typeof(TextWidget))
	self.my_bet_scroll_list=self.go.transform:Find("my_bet_scroll_list"):GetComponent(typeof(CellRecycleScrollWidget))
	self.mybetCellArr={}
	table.insert(self.mybetCellArr,self.new_MybetCell(self.go.transform:Find("my_bet_scroll_list/content/cellitem").gameObject))
	table.insert(self.mybetCellArr,self.new_MybetCell(self.go.transform:Find("my_bet_scroll_list/content/cellitem_1").gameObject))
	table.insert(self.mybetCellArr,self.new_MybetCell(self.go.transform:Find("my_bet_scroll_list/content/cellitem_2").gameObject))
	table.insert(self.mybetCellArr,self.new_MybetCell(self.go.transform:Find("my_bet_scroll_list/content/cellitem_3").gameObject))
	table.insert(self.mybetCellArr,self.new_MybetCell(self.go.transform:Find("my_bet_scroll_list/content/cellitem_4").gameObject))
	table.insert(self.mybetCellArr,self.new_MybetCell(self.go.transform:Find("my_bet_scroll_list/content/cellitem_5").gameObject))
	table.insert(self.mybetCellArr,self.new_MybetCell(self.go.transform:Find("my_bet_scroll_list/content/cellitem_6").gameObject))
	table.insert(self.mybetCellArr,self.new_MybetCell(self.go.transform:Find("my_bet_scroll_list/content/cellitem_7").gameObject))
end

--MybetCell复用单元
function this.new_MybetCell(itemGo)
	local item = { }
	item.go = itemGo
	item.bet_content_text=itemGo.transform:Find("bet_content_text"):GetComponent(typeof(TextWidget))
	item.bet_explain_text=itemGo.transform:Find("bet_explain_text"):GetComponent(typeof(TextWidget))
	item.win_image=itemGo.transform:Find("win_image"):GetComponent(typeof(ImageWidget))
	item.win_result_text=itemGo.transform:Find("win_result_text"):GetComponent(typeof(TextWidget))
	item.lose_result_text=itemGo.transform:Find("lose_result_text"):GetComponent(typeof(TextWidget))
	item.answer_text=itemGo.transform:Find("answer_text"):GetComponent(typeof(TextWidget))
	item.no_lottery_text=itemGo.transform:Find("no_lottery_text"):GetComponent(typeof(TextWidget))
	return item
end

