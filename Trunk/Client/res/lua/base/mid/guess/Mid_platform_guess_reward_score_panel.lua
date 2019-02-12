local CircleImageWidget=CS.CircleImageWidget
local TextWidget=CS.TextWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local EmptyImageWidget=CS.EmptyImageWidget
local ImageWidget=CS.ImageWidget
local IconWidget=CS.IconWidget

Mid_platform_guess_reward_score_panel={}
local this = Mid_platform_guess_reward_score_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.head_image=nil
this.money_text=nil
this.exchange_scroll_list=nil
this.back_image=nil
--ExchangeItem数组
this.exchangeItemArr={}

function this:init(gameObject)
	self.go=gameObject
	self.head_image=self.go.transform:Find("user_data/head_image"):GetComponent(typeof(CircleImageWidget))
	self.money_text=self.go.transform:Find("user_data/money_type_text/money_bg_image/money_text"):GetComponent(typeof(TextWidget))
	self.exchange_scroll_list=self.go.transform:Find("exchange_scroll_list"):GetComponent(typeof(CellRecycleScrollWidget))
	self.back_image=self.go.transform:Find("back_image/back_image"):GetComponent(typeof(EmptyImageWidget))
	self.exchangeItemArr={}
	table.insert(self.exchangeItemArr,self.new_ExchangeItem(self.go.transform:Find("exchange_scroll_list/content/cellitem").gameObject))
	table.insert(self.exchangeItemArr,self.new_ExchangeItem(self.go.transform:Find("exchange_scroll_list/content/cellitem_1").gameObject))
	table.insert(self.exchangeItemArr,self.new_ExchangeItem(self.go.transform:Find("exchange_scroll_list/content/cellitem_2").gameObject))
	table.insert(self.exchangeItemArr,self.new_ExchangeItem(self.go.transform:Find("exchange_scroll_list/content/cellitem_3").gameObject))
	table.insert(self.exchangeItemArr,self.new_ExchangeItem(self.go.transform:Find("exchange_scroll_list/content/cellitem_4").gameObject))
	table.insert(self.exchangeItemArr,self.new_ExchangeItem(self.go.transform:Find("exchange_scroll_list/content/cellitem_5").gameObject))
	table.insert(self.exchangeItemArr,self.new_ExchangeItem(self.go.transform:Find("exchange_scroll_list/content/cellitem_6").gameObject))
	table.insert(self.exchangeItemArr,self.new_ExchangeItem(self.go.transform:Find("exchange_scroll_list/content/cellitem_7").gameObject))
	table.insert(self.exchangeItemArr,self.new_ExchangeItem(self.go.transform:Find("exchange_scroll_list/content/cellitem_8").gameObject))
	table.insert(self.exchangeItemArr,self.new_ExchangeItem(self.go.transform:Find("exchange_scroll_list/content/cellitem_9").gameObject))
end

--ExchangeItem复用单元
function this.new_ExchangeItem(itemGo)
	local item = { }
	item.go = itemGo
	item.need_text=itemGo.transform:Find("need_text"):GetComponent(typeof(TextWidget))
	item.reward_image_1=itemGo.transform:Find("reward_image_1"):GetComponent(typeof(ImageWidget))
	item.count_image_1=itemGo.transform:Find("count_image_1"):GetComponent(typeof(ImageWidget))
	item.count_text_1=itemGo.transform:Find("count_image_1/count_text_1"):GetComponent(typeof(TextWidget))
	item.exchange_icon=itemGo.transform:Find("exchange_icon"):GetComponent(typeof(IconWidget))
	item.progress_text=itemGo.transform:Find("exchange_icon/progress_text"):GetComponent(typeof(TextWidget))
	return item
end

