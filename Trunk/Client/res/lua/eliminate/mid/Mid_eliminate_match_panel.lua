local CellGroupWidget=CS.CellGroupWidget
local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local InputFieldWidget=CS.InputFieldWidget
local ImageWidget=CS.ImageWidget
local CircleImageWidget=CS.CircleImageWidget

Mid_eliminate_match_panel={}
local this = Mid_eliminate_match_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.player_group=nil
this.room_id_txt=nil
this.ready_btn=nil
this.start_btn=nil
this.back_btn=nil
this.chatCellRecycleScrollPanel=nil
this.chat_input=nil
this.send_btn=nil
--PlayerItem数组
this.playerItemArr={}
--ChatCell数组
this.chatCellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.player_group=self.go.transform:Find("mask/bg_image/ref_image/player_group"):GetComponent(typeof(CellGroupWidget))
	self.room_id_txt=self.go.transform:Find("mask/bg_image/room_id_txt"):GetComponent(typeof(TextWidget))
	self.ready_btn=self.go.transform:Find("mask/bg_image/ready_btn"):GetComponent(typeof(ButtonWidget))
	self.start_btn=self.go.transform:Find("mask/bg_image/start_btn"):GetComponent(typeof(ButtonWidget))
	self.back_btn=self.go.transform:Find("back_btn"):GetComponent(typeof(ButtonWidget))
	self.chatCellRecycleScrollPanel=self.go.transform:Find("chat_drag_bg/chatCellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.chat_input=self.go.transform:Find("chat_input"):GetComponent(typeof(InputFieldWidget))
	self.send_btn=self.go.transform:Find("send_btn"):GetComponent(typeof(ButtonWidget))
	self.playerItemArr={}
	table.insert(self.playerItemArr,self.new_PlayerItem(self.go.transform:Find("mask/bg_image/ref_image/player_group/CellItem").gameObject))
	table.insert(self.playerItemArr,self.new_PlayerItem(self.go.transform:Find("mask/bg_image/ref_image/player_group/CellItem_1").gameObject))
	table.insert(self.playerItemArr,self.new_PlayerItem(self.go.transform:Find("mask/bg_image/ref_image/player_group/CellItem_2").gameObject))
	table.insert(self.playerItemArr,self.new_PlayerItem(self.go.transform:Find("mask/bg_image/ref_image/player_group/CellItem_3").gameObject))
	self.chatCellArr={}
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat_drag_bg/chatCellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat_drag_bg/chatCellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat_drag_bg/chatCellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat_drag_bg/chatCellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat_drag_bg/chatCellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat_drag_bg/chatCellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat_drag_bg/chatCellRecycleScrollPanel/content/cellitem_6").gameObject))
end

--PlayerItem复用单元
function this.new_PlayerItem(itemGo)
	local item = { }
	item.go = itemGo
	item.house_image=itemGo.transform:Find("house_image"):GetComponent(typeof(ImageWidget))
	item.feature_img=itemGo.transform:Find("feature_img"):GetComponent(typeof(CircleImageWidget))
	item.name_txt=itemGo.transform:Find("name_txt"):GetComponent(typeof(TextWidget))
	item.readyState_txt=itemGo.transform:Find("readyState_txt"):GetComponent(typeof(TextWidget))
	item.invite_btn=itemGo.transform:Find("invite_btn"):GetComponent(typeof(ButtonWidget))
	item.kick_btn=itemGo.transform:Find("kick_btn"):GetComponent(typeof(ButtonWidget))
	return item
end
--ChatCell复用单元
function this.new_ChatCell(itemGo)
	local item = { }
	item.go = itemGo
	item.say_text=itemGo.transform:Find("say_text"):GetComponent(typeof(TextWidget))
	return item
end

