local CircleImageWidget=CS.CircleImageWidget
local TextWidget=CS.TextWidget
local ButtonWidget=CS.ButtonWidget
local PanelWidget=CS.PanelWidget
local IconWidget=CS.IconWidget
local ImageWidget=CS.ImageWidget
local CellRecycleNewScrollWidget=CS.CellRecycleNewScrollWidget
local InputFieldWidget=CS.InputFieldWidget
local EmptyImageWidget=CS.EmptyImageWidget

Mid_platform_guess_chatroom_panel={}
local this = Mid_platform_guess_chatroom_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.myphoto_CImage=nil
this.my_rank_sorce_Text=nil
this.btn_my_guess=nil
this.bet_info=nil
this.bet_content_text=nil
this.choose_state_icon_1=nil
this.odds_text_1=nil
this.choose_flag_icon_1=nil
this.choose_state_icon_2=nil
this.odds_text_2=nil
this.choose_flag_icon_2=nil
this.choose_state_icon_3=nil
this.odds_text_3=nil
this.choose_flag_icon_3=nil
this.TextPersonNum=nil
this.btn_check_reward=nil
this.btn_more_bet=nil
this.room_chat_ScrollPanel=nil
this.chat_content=nil
this.chatInput=nil
this.chatImput_InputField=nil
this.change_chat_type_Icon=nil
this.chat_face_Button=nil
this.chat_photo_Button=nil
this.chat_voice_Button=nil
this.talking_map_Panel=nil
this.signal_Image=nil
this.btn_return=nil

function this:init(gameObject)
	self.go=gameObject
	self.myphoto_CImage=self.go.transform:Find("Top_Panel/photo_frame_Image/myphoto_CImage"):GetComponent(typeof(CircleImageWidget))
	self.my_rank_sorce_Text=self.go.transform:Find("Top_Panel/photo_frame_Image/my_rank_val_img/my_rank_sorce_Text"):GetComponent(typeof(TextWidget))
	self.btn_my_guess=self.go.transform:Find("Top_Panel/btn_my_guess"):GetComponent(typeof(ButtonWidget))
	self.bet_info=self.go.transform:Find("Mid_Panel/tip_Panel/bet_info"):GetComponent(typeof(PanelWidget))
	self.bet_content_text=self.go.transform:Find("Mid_Panel/tip_Panel/bet_info/bet_content_text"):GetComponent(typeof(TextWidget))
	self.choose_state_icon_1=self.go.transform:Find("Mid_Panel/tip_Panel/bet_info/choose_state_icon_1"):GetComponent(typeof(IconWidget))
	self.odds_text_1=self.go.transform:Find("Mid_Panel/tip_Panel/bet_info/choose_state_icon_1/odds_text_1"):GetComponent(typeof(TextWidget))
	self.choose_flag_icon_1=self.go.transform:Find("Mid_Panel/tip_Panel/bet_info/choose_state_icon_1/choose_flag_icon_1"):GetComponent(typeof(IconWidget))
	self.choose_state_icon_2=self.go.transform:Find("Mid_Panel/tip_Panel/bet_info/choose_state_icon_2"):GetComponent(typeof(IconWidget))
	self.odds_text_2=self.go.transform:Find("Mid_Panel/tip_Panel/bet_info/choose_state_icon_2/odds_text_2"):GetComponent(typeof(TextWidget))
	self.choose_flag_icon_2=self.go.transform:Find("Mid_Panel/tip_Panel/bet_info/choose_state_icon_2/choose_flag_icon_2"):GetComponent(typeof(IconWidget))
	self.choose_state_icon_3=self.go.transform:Find("Mid_Panel/tip_Panel/bet_info/choose_state_icon_3"):GetComponent(typeof(IconWidget))
	self.odds_text_3=self.go.transform:Find("Mid_Panel/tip_Panel/bet_info/choose_state_icon_3/odds_text_3"):GetComponent(typeof(TextWidget))
	self.choose_flag_icon_3=self.go.transform:Find("Mid_Panel/tip_Panel/bet_info/choose_state_icon_3/choose_flag_icon_3"):GetComponent(typeof(IconWidget))
	self.TextPersonNum=self.go.transform:Find("Mid_Panel/tip_Panel/person_num/TextPersonNum"):GetComponent(typeof(TextWidget))
	self.btn_check_reward=self.go.transform:Find("Mid_Panel/tip_Panel/btn_check_reward"):GetComponent(typeof(ImageWidget))
	self.btn_more_bet=self.go.transform:Find("Mid_Panel/tip_Panel/btn_more_bet"):GetComponent(typeof(ImageWidget))
	self.room_chat_ScrollPanel=self.go.transform:Find("Mid_Panel/room_chat_ScrollPanel"):GetComponent(typeof(CellRecycleNewScrollWidget))
	self.chat_content=self.go.transform:Find("Mid_Panel/room_chat_ScrollPanel/chat_content"):GetComponent(typeof(PanelWidget))
	self.chatInput=self.go.transform:Find("chatInput"):GetComponent(typeof(ImageWidget))
	self.chatImput_InputField=self.go.transform:Find("chatInput/chatImput_InputField"):GetComponent(typeof(InputFieldWidget))
	self.change_chat_type_Icon=self.go.transform:Find("chatInput/change_chat_type_Icon"):GetComponent(typeof(IconWidget))
	self.chat_face_Button=self.go.transform:Find("chatInput/chat_face_Button"):GetComponent(typeof(ButtonWidget))
	self.chat_photo_Button=self.go.transform:Find("chatInput/chat_photo_Button"):GetComponent(typeof(ButtonWidget))
	self.chat_voice_Button=self.go.transform:Find("chatInput/chat_voice_Button"):GetComponent(typeof(ButtonWidget))
	self.talking_map_Panel=self.go.transform:Find("chatInput/talking_map_Panel"):GetComponent(typeof(PanelWidget))
	self.signal_Image=self.go.transform:Find("chatInput/talking_map_Panel/signal_Image"):GetComponent(typeof(ImageWidget))
	self.btn_return=self.go.transform:Find("btn_return/btn_return"):GetComponent(typeof(EmptyImageWidget))
end


