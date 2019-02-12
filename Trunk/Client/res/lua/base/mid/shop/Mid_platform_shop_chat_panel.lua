local TextWidget=CS.TextWidget
local EmptyImageWidget=CS.EmptyImageWidget
local CircleImageWidget=CS.CircleImageWidget
local ImageWidget=CS.ImageWidget
local IconWidget=CS.IconWidget
local ButtonWidget=CS.ButtonWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local InputFieldWidget=CS.InputFieldWidget
local PanelWidget=CS.PanelWidget

Mid_platform_shop_chat_panel={}
local this = Mid_platform_shop_chat_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.Top_title_text=nil
this.title_text=nil
this.back_Image=nil
this.head_Image=nil
this.game_Icon=nil
this.gamename_Text=nil
this.shopname_Text=nil
this.checkreward_Image=nil
this.checkrank_Text=nil
this.checkRule_Text=nil
this.activitybegintimeBG_Image=nil
this.activitybegintime_Text=nil
this.noticeToStart_Text=nil
this.noticeToStart_Image_Toggle=nil
this.cellitem1_shop_image=nil
this.cellitem4_shop_image=nil
this.cellitem2_shop_image=nil
this.cellitem5_shop_image=nil
this.cellitem3_shop_image=nil
this.cellitem6_shop_image=nil
this.enter_game_Button=nil
this.enter_game_time=nil
this.chat=nil
this.haveCoupon=nil
this.haveRedBag=nil
this.OPenChat_Window=nil
this.chatCellRecycleScrollPanel=nil
this.chatInput=nil
this.chatImput_InputField=nil
this.rule_Panel=nil
this.rule_title_Text=nil
this.rule_CellRecycleScrollPanel=nil
this.go_Button=nil
--ChatCell数组
this.chatCellArr={}
--RuleItem数组
this.ruleItemArr={}

function this:init(gameObject)
	self.go=gameObject
	self.Top_title_text=self.go.transform:Find("shop_info/Top_title_text"):GetComponent(typeof(TextWidget))
	self.title_text=self.go.transform:Find("shop_info/title_text"):GetComponent(typeof(TextWidget))
	self.back_Image=self.go.transform:Find("shop_info/back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.head_Image=self.go.transform:Find("shop_info/shopinfo_Panel/head_Image"):GetComponent(typeof(CircleImageWidget))
	self.game_Icon=self.go.transform:Find("shop_info/shopinfo_Panel/game_Icon"):GetComponent(typeof(ImageWidget))
	self.gamename_Text=self.go.transform:Find("shop_info/shopinfo_Panel/gamename_Text"):GetComponent(typeof(TextWidget))
	self.shopname_Text=self.go.transform:Find("shop_info/shopinfo_Panel/shopname_Text"):GetComponent(typeof(TextWidget))
	self.checkreward_Image=self.go.transform:Find("shop_info/shopinfo_Panel/checkreward_Image"):GetComponent(typeof(ImageWidget))
	self.checkrank_Text=self.go.transform:Find("shop_info/shopinfo_Panel/checkrank_Text"):GetComponent(typeof(TextWidget))
	self.checkRule_Text=self.go.transform:Find("shop_info/shopinfo_Panel/checkRule_Text"):GetComponent(typeof(TextWidget))
	self.activitybegintimeBG_Image=self.go.transform:Find("shop_info/shopinfo_Panel/activitybegintimeBG_Image"):GetComponent(typeof(IconWidget))
	self.activitybegintime_Text=self.go.transform:Find("shop_info/shopinfo_Panel/activitybegintimeBG_Image/activitybegintime_Text"):GetComponent(typeof(TextWidget))
	self.noticeToStart_Text=self.go.transform:Find("shop_info/shopinfo_Panel/noticeToStart_Text"):GetComponent(typeof(TextWidget))
	self.noticeToStart_Image_Toggle=self.go.transform:Find("shop_info/shopinfo_Panel/noticeToStart_Text/noticeToStart_Image_Toggle"):GetComponent(typeof(IconWidget))
	self.cellitem1_shop_image=self.go.transform:Find("shop_info/shoppng_CellRecycleScrollPanel/cellitem1_shop_image"):GetComponent(typeof(ImageWidget))
	self.cellitem4_shop_image=self.go.transform:Find("shop_info/shoppng_CellRecycleScrollPanel/cellitem4_shop_image"):GetComponent(typeof(ImageWidget))
	self.cellitem2_shop_image=self.go.transform:Find("shop_info/shoppng_CellRecycleScrollPanel/cellitem2_shop_image"):GetComponent(typeof(ImageWidget))
	self.cellitem5_shop_image=self.go.transform:Find("shop_info/shoppng_CellRecycleScrollPanel/cellitem5_shop_image"):GetComponent(typeof(ImageWidget))
	self.cellitem3_shop_image=self.go.transform:Find("shop_info/shoppng_CellRecycleScrollPanel/cellitem3_shop_image"):GetComponent(typeof(ImageWidget))
	self.cellitem6_shop_image=self.go.transform:Find("shop_info/shoppng_CellRecycleScrollPanel/cellitem6_shop_image"):GetComponent(typeof(ImageWidget))
	self.enter_game_Button=self.go.transform:Find("shop_info/enter_game_Button"):GetComponent(typeof(ButtonWidget))
	self.enter_game_time=self.go.transform:Find("shop_info/enter_game_time"):GetComponent(typeof(TextWidget))
	self.chat=self.go.transform:Find("chat"):GetComponent(typeof(ImageWidget))
	self.haveCoupon=self.go.transform:Find("chat/haveCoupon"):GetComponent(typeof(ImageWidget))
	self.haveRedBag=self.go.transform:Find("chat/haveRedBag"):GetComponent(typeof(ImageWidget))
	self.OPenChat_Window=self.go.transform:Find("chat/OPenChat_Window"):GetComponent(typeof(ButtonWidget))
	self.chatCellRecycleScrollPanel=self.go.transform:Find("chat/chatCellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.chatInput=self.go.transform:Find("chatInput"):GetComponent(typeof(ImageWidget))
	self.chatImput_InputField=self.go.transform:Find("chatInput/chatImput_InputField"):GetComponent(typeof(InputFieldWidget))
	self.rule_Panel=self.go.transform:Find("rule_Panel"):GetComponent(typeof(PanelWidget))
	self.rule_title_Text=self.go.transform:Find("rule_Panel/rule_title_Text"):GetComponent(typeof(TextWidget))
	self.rule_CellRecycleScrollPanel=self.go.transform:Find("rule_Panel/rule_CellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.go_Button=self.go.transform:Find("rule_Panel/go_Button"):GetComponent(typeof(ButtonWidget))
	self.chatCellArr={}
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_6").gameObject))
	self.ruleItemArr={}
	table.insert(self.ruleItemArr,self.new_RuleItem(self.go.transform:Find("rule_Panel/rule_CellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.ruleItemArr,self.new_RuleItem(self.go.transform:Find("rule_Panel/rule_CellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.ruleItemArr,self.new_RuleItem(self.go.transform:Find("rule_Panel/rule_CellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.ruleItemArr,self.new_RuleItem(self.go.transform:Find("rule_Panel/rule_CellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.ruleItemArr,self.new_RuleItem(self.go.transform:Find("rule_Panel/rule_CellRecycleScrollPanel/content/cellitem_4").gameObject))
end

--ChatCell复用单元
function this.new_ChatCell(itemGo)
	local item = { }
	item.go = itemGo
	item.chat_text=itemGo.transform:Find("chat_text"):GetComponent(typeof(TextWidget))
	item.name_type=itemGo.transform:Find("name_type"):GetComponent(typeof(ImageWidget))
	return item
end
--RuleItem复用单元
function this.new_RuleItem(itemGo)
	local item = { }
	item.go = itemGo
	item.title_text=itemGo.transform:Find("title_text"):GetComponent(typeof(TextWidget))
	item.number_Text=itemGo.transform:Find("number_Text"):GetComponent(typeof(TextWidget))
	return item
end

