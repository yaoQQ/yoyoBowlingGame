local EmptyImageWidget=CS.EmptyImageWidget
local ImageWidget=CS.ImageWidget
local TextWidget=CS.TextWidget
local ToggleWidget=CS.ToggleWidget
local ButtonWidget=CS.ButtonWidget
local IconWidget=CS.IconWidget

Mid_platform_redbag_withdraw_panel={}
local this = Mid_platform_redbag_withdraw_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.back_Image=nil
this.record_Image=nil
this.nowithdraw_Image=nil
this.btn_bind_alipay=nil
this.text_name_alipay=nil
this.toggle_alipay=nil
this.btn_bind_wx=nil
this.text_name_wx=nil
this.toggle_wx=nil
this.withdraw_Button=nil
this.Image=nil
this.rule_Image=nil
this.money_Text=nil
this.prompt_Text=nil
this.withdraw_timer_text=nil
--SelectItem数组
this.selectItemArr={}

function this:init(gameObject)
	self.go=gameObject
	self.back_Image=self.go.transform:Find("top/back_Image/back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.record_Image=self.go.transform:Find("top/record_Image"):GetComponent(typeof(ImageWidget))
	self.nowithdraw_Image=self.go.transform:Find("mid/mid_Panel/nowithdraw_Image"):GetComponent(typeof(ImageWidget))
	self.btn_bind_alipay=self.go.transform:Find("mid/mid_Panel/btn_bind_alipay"):GetComponent(typeof(TextWidget))
	self.text_name_alipay=self.go.transform:Find("mid/mid_Panel/text_name_alipay"):GetComponent(typeof(TextWidget))
	self.toggle_alipay=self.go.transform:Find("mid/mid_Panel/toggle_alipay"):GetComponent(typeof(ToggleWidget))
	self.btn_bind_wx=self.go.transform:Find("mid/mid_Panel/btn_bind_wx"):GetComponent(typeof(TextWidget))
	self.text_name_wx=self.go.transform:Find("mid/mid_Panel/text_name_wx"):GetComponent(typeof(TextWidget))
	self.toggle_wx=self.go.transform:Find("mid/mid_Panel/toggle_wx"):GetComponent(typeof(ToggleWidget))
	self.withdraw_Button=self.go.transform:Find("mid/mid_Panel/withdraw_Button"):GetComponent(typeof(ButtonWidget))
	self.Image=self.go.transform:Find("mid/mid_Panel/rules_Text/Image"):GetComponent(typeof(ImageWidget))
	self.rule_Image=self.go.transform:Find("mid/mid_Panel/rules_Text/rule_Image"):GetComponent(typeof(EmptyImageWidget))
	self.money_Text=self.go.transform:Find("mid/mid_Panel/money_Text"):GetComponent(typeof(TextWidget))
	self.prompt_Text=self.go.transform:Find("mid/mid_Panel/prompt_Text"):GetComponent(typeof(TextWidget))
	self.withdraw_timer_text=self.go.transform:Find("mid/mid_Panel/withdraw_timer_text"):GetComponent(typeof(TextWidget))
	self.selectItemArr={}
	table.insert(self.selectItemArr,self.new_SelectItem(self.go.transform:Find("mid/mid_Panel/select_panel/select_group/CellItem").gameObject))
	table.insert(self.selectItemArr,self.new_SelectItem(self.go.transform:Find("mid/mid_Panel/select_panel/select_group/CellItem_1").gameObject))
end

--SelectItem复用单元
function this.new_SelectItem(itemGo)
	local item = { }
	item.go = itemGo
	item.bg_icon=itemGo.transform:Find("bg_icon"):GetComponent(typeof(IconWidget))
	item.selected_tick_toggle=itemGo.transform:Find("bg_icon/selected_tick_toggle"):GetComponent(typeof(ToggleWidget))
	item.selected_bg_toggle=itemGo.transform:Find("bg_icon/selected_bg_toggle"):GetComponent(typeof(ToggleWidget))
	item.withdraw_tip_image=itemGo.transform:Find("bg_icon/withdraw_tip_image"):GetComponent(typeof(EmptyImageWidget))
	item.withdraw_condition_image=itemGo.transform:Find("bg_icon/withdraw_condition_image"):GetComponent(typeof(ImageWidget))
	item.withdraw_condition_text=itemGo.transform:Find("bg_icon/withdraw_condition_image/withdraw_condition_text"):GetComponent(typeof(TextWidget))
	return item
end

