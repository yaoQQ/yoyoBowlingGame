﻿local TextWidget=CS.TextWidget
local ImageWidget=CS.ImageWidget
local CircleImageWidget=CS.CircleImageWidget
local PanelWidget=CS.PanelWidget
local IconWidget=CS.IconWidget

Mid_platform_chat_item_panel={}
local this = Mid_platform_chat_item_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.time_Text=nil
this.self=nil
this.self_head_Image=nil
this.self_name_Text=nil
this.self_chat_bg_Image=nil
this.self_chat_Text=nil
this.self_Image=nil
this.self_regbag_Image=nil
this.pray_Text=nil
this.self_voice_Image=nil
this.self_roarer_Image=nil
this.other=nil
this.other_head_Image=nil
this.other_name_Text=nil
this.other_chat_bg_Image=nil
this.other_chat_Text=nil
this.other_Image=nil
this.other_regbag_Image=nil
this.other_pray_Text=nil
this.other_voice_Image=nil
this.other_roarer_Image=nil
this.question=nil
this.question_head_Image=nil
this.question_name_Text=nil
this.question_title_Text=nil
this.btn_answer_1=nil
this.answer_1_Text=nil
this.answer_1_right=nil
this.answer_1_wrong=nil
this.btn_answer_2=nil
this.answer_2_Text=nil
this.answer_2_wrong=nil
this.answer_2_right=nil
this.btn_answer_3=nil
this.answer_3_Text=nil
this.answer_3_right=nil
this.answer_3_wrong=nil
this.btn_answer_4=nil
this.answer_4_Text=nil
this.answer_4_wrong=nil
this.answer_4_right=nil
this.rank_Text=nil
this.live=nil
this.live_event_icon=nil
this.live_bg_image=nil
this.live_event_text=nil

function this:init(gameObject)
	self.go=gameObject
	self.time_Text=self.go.transform:Find("time/time_Text"):GetComponent(typeof(TextWidget))
	self.self=self.go.transform:Find("self"):GetComponent(typeof(ImageWidget))
	self.self_head_Image=self.go.transform:Find("self/self_head_Image"):GetComponent(typeof(CircleImageWidget))
	self.self_name_Text=self.go.transform:Find("self/self_name_Text"):GetComponent(typeof(TextWidget))
	self.self_chat_bg_Image=self.go.transform:Find("self/self_chat_bg_Image"):GetComponent(typeof(ImageWidget))
	self.self_chat_Text=self.go.transform:Find("self/self_chat_bg_Image/self_chat_Text"):GetComponent(typeof(TextWidget))
	self.self_Image=self.go.transform:Find("self/self_Image"):GetComponent(typeof(ImageWidget))
	self.self_regbag_Image=self.go.transform:Find("self/self_regbag_Image"):GetComponent(typeof(ImageWidget))
	self.pray_Text=self.go.transform:Find("self/self_regbag_Image/pray_Text"):GetComponent(typeof(TextWidget))
	self.self_voice_Image=self.go.transform:Find("self/self_voice_Image"):GetComponent(typeof(ImageWidget))
	self.self_roarer_Image=self.go.transform:Find("self/self_voice_Image/self_roarer_Image"):GetComponent(typeof(ImageWidget))
	self.other=self.go.transform:Find("other"):GetComponent(typeof(ImageWidget))
	self.other_head_Image=self.go.transform:Find("other/other_head_Image"):GetComponent(typeof(CircleImageWidget))
	self.other_name_Text=self.go.transform:Find("other/other_name_Text"):GetComponent(typeof(TextWidget))
	self.other_chat_bg_Image=self.go.transform:Find("other/other_chat_bg_Image"):GetComponent(typeof(ImageWidget))
	self.other_chat_Text=self.go.transform:Find("other/other_chat_bg_Image/other_chat_Text"):GetComponent(typeof(TextWidget))
	self.other_Image=self.go.transform:Find("other/other_Image"):GetComponent(typeof(ImageWidget))
	self.other_regbag_Image=self.go.transform:Find("other/other_regbag_Image"):GetComponent(typeof(ImageWidget))
	self.other_pray_Text=self.go.transform:Find("other/other_regbag_Image/other_pray_Text"):GetComponent(typeof(TextWidget))
	self.other_voice_Image=self.go.transform:Find("other/other_voice_Image"):GetComponent(typeof(ImageWidget))
	self.other_roarer_Image=self.go.transform:Find("other/other_voice_Image/other_roarer_Image"):GetComponent(typeof(ImageWidget))
	self.question=self.go.transform:Find("question"):GetComponent(typeof(PanelWidget))
	self.question_head_Image=self.go.transform:Find("question/question_head_Image"):GetComponent(typeof(CircleImageWidget))
	self.question_name_Text=self.go.transform:Find("question/question_name_Text"):GetComponent(typeof(TextWidget))
	self.question_title_Text=self.go.transform:Find("question/question_title_Text"):GetComponent(typeof(TextWidget))
	self.btn_answer_1=self.go.transform:Find("question/btn_answer_1"):GetComponent(typeof(IconWidget))
	self.answer_1_Text=self.go.transform:Find("question/btn_answer_1/answer_1_Text"):GetComponent(typeof(TextWidget))
	self.answer_1_right=self.go.transform:Find("question/btn_answer_1/answer_1_right"):GetComponent(typeof(ImageWidget))
	self.answer_1_wrong=self.go.transform:Find("question/btn_answer_1/answer_1_wrong"):GetComponent(typeof(ImageWidget))
	self.btn_answer_2=self.go.transform:Find("question/btn_answer_2"):GetComponent(typeof(IconWidget))
	self.answer_2_Text=self.go.transform:Find("question/btn_answer_2/answer_2_Text"):GetComponent(typeof(TextWidget))
	self.answer_2_wrong=self.go.transform:Find("question/btn_answer_2/answer_2_wrong"):GetComponent(typeof(ImageWidget))
	self.answer_2_right=self.go.transform:Find("question/btn_answer_2/answer_2_right"):GetComponent(typeof(ImageWidget))
	self.btn_answer_3=self.go.transform:Find("question/btn_answer_3"):GetComponent(typeof(IconWidget))
	self.answer_3_Text=self.go.transform:Find("question/btn_answer_3/answer_3_Text"):GetComponent(typeof(TextWidget))
	self.answer_3_right=self.go.transform:Find("question/btn_answer_3/answer_3_right"):GetComponent(typeof(ImageWidget))
	self.answer_3_wrong=self.go.transform:Find("question/btn_answer_3/answer_3_wrong"):GetComponent(typeof(ImageWidget))
	self.btn_answer_4=self.go.transform:Find("question/btn_answer_4"):GetComponent(typeof(IconWidget))
	self.answer_4_Text=self.go.transform:Find("question/btn_answer_4/answer_4_Text"):GetComponent(typeof(TextWidget))
	self.answer_4_wrong=self.go.transform:Find("question/btn_answer_4/answer_4_wrong"):GetComponent(typeof(ImageWidget))
	self.answer_4_right=self.go.transform:Find("question/btn_answer_4/answer_4_right"):GetComponent(typeof(ImageWidget))
	self.rank_Text=self.go.transform:Find("question/rank_Text"):GetComponent(typeof(TextWidget))
	self.live=self.go.transform:Find("live"):GetComponent(typeof(PanelWidget))
	self.live_event_icon=self.go.transform:Find("live/live_event_icon"):GetComponent(typeof(IconWidget))
	self.live_bg_image=self.go.transform:Find("live/live_event_icon/live_bg_image"):GetComponent(typeof(ImageWidget))
	self.live_event_text=self.go.transform:Find("live/live_event_icon/live_bg_image/live_event_text"):GetComponent(typeof(TextWidget))
end


