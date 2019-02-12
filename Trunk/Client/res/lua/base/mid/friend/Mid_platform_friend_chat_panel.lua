local ImageWidget=CS.ImageWidget
local EmptyImageWidget=CS.EmptyImageWidget
local CellRecycleScrollWidget=CS.CellRecycleScrollWidget
local ButtonWidget=CS.ButtonWidget
local IconWidget=CS.IconWidget
local TextWidget=CS.TextWidget
local CellGroupWidget=CS.CellGroupWidget
local InputFieldWidget=CS.InputFieldWidget
local CircleImageWidget=CS.CircleImageWidget
local PanelWidget=CS.PanelWidget

Mid_platform_friend_chat_panel={}
local this = Mid_platform_friend_chat_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.Chat_bg_Image=nil
this.chat=nil
this.chatCellRecycleScrollPanel=nil
this.bottom=nil
this.bottom_picture_Button=nil
this.bottom_picture_Icon=nil
this.bottom_picture_Text=nil
this.bottom_camera_Button=nil
this.bottom_camera_Icon=nil
this.bottom_camera_Text=nil
this.face=nil
this.face_CellGroup=nil
this.chatInput=nil
this.chatImput_InputField=nil
this.change_chat_type_Icon=nil
this.chat_face_Button=nil
this.chat_face_Icon=nil
this.chat_photo_Button=nil
this.chat_photo_Icon=nil
this.chat_voice_Button=nil
this.friend_config_Image=nil
this.friendchat_back_Image=nil
this.friendname_Text=nil
this.cancel_Button=nil
this.show_bg_Image=nil
this.show_Icon=nil
this.show_Text=nil
--ChatCell数组
this.chatCellArr={}
--FaceGroupCell数组
this.faceGroupCellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.Chat_bg_Image=self.go.transform:Find("Chat_bg_Image"):GetComponent(typeof(ImageWidget))
	self.chat=self.go.transform:Find("chat"):GetComponent(typeof(EmptyImageWidget))
	self.chatCellRecycleScrollPanel=self.go.transform:Find("chat/chatCellRecycleScrollPanel"):GetComponent(typeof(CellRecycleScrollWidget))
	self.bottom=self.go.transform:Find("chat/bottom"):GetComponent(typeof(ImageWidget))
	self.bottom_picture_Button=self.go.transform:Find("chat/bottom/bottom_picture_Button"):GetComponent(typeof(ButtonWidget))
	self.bottom_picture_Icon=self.go.transform:Find("chat/bottom/bottom_picture_Button/bottom_picture_Icon"):GetComponent(typeof(IconWidget))
	self.bottom_picture_Text=self.go.transform:Find("chat/bottom/bottom_picture_Button/bottom_picture_Text"):GetComponent(typeof(TextWidget))
	self.bottom_camera_Button=self.go.transform:Find("chat/bottom/bottom_camera_Button"):GetComponent(typeof(ButtonWidget))
	self.bottom_camera_Icon=self.go.transform:Find("chat/bottom/bottom_camera_Button/bottom_camera_Icon"):GetComponent(typeof(IconWidget))
	self.bottom_camera_Text=self.go.transform:Find("chat/bottom/bottom_camera_Button/bottom_camera_Text"):GetComponent(typeof(TextWidget))
	self.face=self.go.transform:Find("chat/face"):GetComponent(typeof(ImageWidget))
	self.face_CellGroup=self.go.transform:Find("chat/face/face_CellGroup"):GetComponent(typeof(CellGroupWidget))
	self.chatInput=self.go.transform:Find("chat/chatInput"):GetComponent(typeof(ImageWidget))
	self.chatImput_InputField=self.go.transform:Find("chat/chatInput/chatImput_InputField"):GetComponent(typeof(InputFieldWidget))
	self.change_chat_type_Icon=self.go.transform:Find("chat/chatInput/change_chat_type_Icon"):GetComponent(typeof(IconWidget))
	self.chat_face_Button=self.go.transform:Find("chat/chatInput/chat_face_Button"):GetComponent(typeof(ButtonWidget))
	self.chat_face_Icon=self.go.transform:Find("chat/chatInput/chat_face_Button/chat_face_Icon"):GetComponent(typeof(IconWidget))
	self.chat_photo_Button=self.go.transform:Find("chat/chatInput/chat_photo_Button"):GetComponent(typeof(ButtonWidget))
	self.chat_photo_Icon=self.go.transform:Find("chat/chatInput/chat_photo_Button/chat_photo_Icon"):GetComponent(typeof(IconWidget))
	self.chat_voice_Button=self.go.transform:Find("chat/chatInput/chat_voice_Button"):GetComponent(typeof(ButtonWidget))
	self.friend_config_Image=self.go.transform:Find("Panel/friend_config_Image"):GetComponent(typeof(ImageWidget))
	self.friendchat_back_Image=self.go.transform:Find("Panel/friendchat_back_Image"):GetComponent(typeof(ImageWidget))
	self.friendname_Text=self.go.transform:Find("Panel/friendname_Text"):GetComponent(typeof(TextWidget))
	self.cancel_Button=self.go.transform:Find("cancel_Button"):GetComponent(typeof(ButtonWidget))
	self.show_bg_Image=self.go.transform:Find("show_bg_Image"):GetComponent(typeof(ImageWidget))
	self.show_Icon=self.go.transform:Find("show_bg_Image/show_Icon"):GetComponent(typeof(IconWidget))
	self.show_Text=self.go.transform:Find("show_bg_Image/show_Text"):GetComponent(typeof(TextWidget))
	self.chatCellArr={}
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_1").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_2").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_3").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_4").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_5").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_6").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_7").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_8").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_9").gameObject))
	table.insert(self.chatCellArr,self.new_ChatCell(self.go.transform:Find("chat/chatCellRecycleScrollPanel/content/cellitem_10").gameObject))
	self.faceGroupCellArr={}
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_1").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_2").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_3").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_4").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_5").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_6").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_7").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_8").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_9").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_10").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_11").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_12").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_13").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_14").gameObject))
	table.insert(self.faceGroupCellArr,self.new_FaceGroupCell(self.go.transform:Find("chat/face/face_CellGroup/CellItem_15").gameObject))
end

--ChatCell复用单元
function this.new_ChatCell(itemGo)
	local item = { }
	item.go = itemGo
	item.time_bg=itemGo.transform:Find("time_bg"):GetComponent(typeof(ImageWidget))
	item.time_Text=itemGo.transform:Find("time_bg/time_Text"):GetComponent(typeof(TextWidget))
	item.parentPanel=itemGo.transform:Find("parentPanel"):GetComponent(typeof(EmptyImageWidget))
	item.self=itemGo.transform:Find("parentPanel/self"):GetComponent(typeof(EmptyImageWidget))
	item.self_name_Text=itemGo.transform:Find("parentPanel/self/self_name_Text"):GetComponent(typeof(TextWidget))
	item.self_chat_bg_Image=itemGo.transform:Find("parentPanel/self/self_chat_bg_Image"):GetComponent(typeof(ImageWidget))
	item.self_chat_Text=itemGo.transform:Find("parentPanel/self/self_chat_bg_Image/self_chat_Text"):GetComponent(typeof(TextWidget))
	item.self_Image=itemGo.transform:Find("parentPanel/self/self_Image"):GetComponent(typeof(ImageWidget))
	item.self_head_Image=itemGo.transform:Find("parentPanel/self/self_head_Image"):GetComponent(typeof(CircleImageWidget))
	item.self_voice_Image=itemGo.transform:Find("parentPanel/self/self_voice_Image"):GetComponent(typeof(ImageWidget))
	item.self_roarer_Image=itemGo.transform:Find("parentPanel/self/self_voice_Image/self_roarer_Image"):GetComponent(typeof(ImageWidget))
	item.self_roarer_Text=itemGo.transform:Find("parentPanel/self/self_voice_Image/self_roarer_Text"):GetComponent(typeof(TextWidget))
	item.other=itemGo.transform:Find("parentPanel/other"):GetComponent(typeof(EmptyImageWidget))
	item.other_name_Text=itemGo.transform:Find("parentPanel/other/other_name_Text"):GetComponent(typeof(TextWidget))
	item.name_type=itemGo.transform:Find("parentPanel/other/name_type"):GetComponent(typeof(ImageWidget))
	item.other_Image=itemGo.transform:Find("parentPanel/other/other_Image"):GetComponent(typeof(ImageWidget))
	item.other_head_Image=itemGo.transform:Find("parentPanel/other/other_head_Image"):GetComponent(typeof(CircleImageWidget))
	item.other_chat_bg_Image=itemGo.transform:Find("parentPanel/other/other_chat_bg_Image"):GetComponent(typeof(ImageWidget))
	item.other_self_chat_Text=itemGo.transform:Find("parentPanel/other/other_chat_bg_Image/other_self_chat_Text"):GetComponent(typeof(TextWidget))
	item.redbag=itemGo.transform:Find("parentPanel/redbag"):GetComponent(typeof(PanelWidget))
	item.regbag_Image=itemGo.transform:Find("parentPanel/redbag/regbag_Image"):GetComponent(typeof(ImageWidget))
	item.get_Text=itemGo.transform:Find("parentPanel/redbag/regbag_Image/get_Text"):GetComponent(typeof(TextWidget))
	item.other_pray_Text=itemGo.transform:Find("parentPanel/redbag/regbag_Image/other_pray_Text"):GetComponent(typeof(TextWidget))
	return item
end
--FaceGroupCell复用单元
function this.new_FaceGroupCell(itemGo)
	local item = { }
	item.go = itemGo
	item.press_Image=itemGo.transform:Find("press_Image"):GetComponent(typeof(ImageWidget))
	return item
end

