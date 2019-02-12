﻿local EmptyImageWidget=CS.EmptyImageWidget
local IconWidget=CS.IconWidget
local ImageWidget=CS.ImageWidget
local CircleImageWidget=CS.CircleImageWidget
local TextWidget=CS.TextWidget
local PanelWidget=CS.PanelWidget

Mid_platform_coupon_detail_panel={}
local this = Mid_platform_coupon_detail_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.back_Image=nil
this.coupon_bg_icon=nil
this.coupon_diy_image=nil
this.coupon_image=nil
this.shop_name_text=nil
this.coupon_name_text=nil
this.more_btn=nil
this.use_icon=nil
this.user_text=nil
this.intro_info_text=nil
this.intro_title_text=nil
this.coupon_date_text=nil
this.qr_panel=nil
this.qr_bg=nil
this.qr_image=nil
this.qr_coupon_code_text=nil
this.qr_use_tip_text=nil
--ActionItem数组
this.actionItemArr={}

function this:init(gameObject)
	self.go=gameObject
	self.back_Image=self.go.transform:Find("top/back_Image/back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.coupon_bg_icon=self.go.transform:Find("top/coupon_bg_icon"):GetComponent(typeof(IconWidget))
	self.coupon_diy_image=self.go.transform:Find("top/coupon_bg_icon/coupon_diy_image"):GetComponent(typeof(ImageWidget))
	self.coupon_image=self.go.transform:Find("top/coupon_bg_icon/coupon_image"):GetComponent(typeof(CircleImageWidget))
	self.shop_name_text=self.go.transform:Find("top/coupon_bg_icon/shop_name_text"):GetComponent(typeof(TextWidget))
	self.coupon_name_text=self.go.transform:Find("top/coupon_bg_icon/coupon_name_text"):GetComponent(typeof(TextWidget))
	self.more_btn=self.go.transform:Find("top/more_btn"):GetComponent(typeof(ImageWidget))
	self.use_icon=self.go.transform:Find("middle/use_icon"):GetComponent(typeof(IconWidget))
	self.user_text=self.go.transform:Find("middle/use_icon/user_text"):GetComponent(typeof(TextWidget))
	self.intro_info_text=self.go.transform:Find("middle/ScrollPanel/content/intro_info_text"):GetComponent(typeof(TextWidget))
	self.intro_title_text=self.go.transform:Find("middle/ScrollPanel/content/intro_info_text/intro_title_text"):GetComponent(typeof(TextWidget))
	self.coupon_date_text=self.go.transform:Find("middle/ScrollPanel/content/coupon_date_text"):GetComponent(typeof(TextWidget))
	self.qr_panel=self.go.transform:Find("qr_panel"):GetComponent(typeof(PanelWidget))
	self.qr_bg=self.go.transform:Find("qr_bg"):GetComponent(typeof(ImageWidget))
	self.qr_image=self.go.transform:Find("qr_bg/qr_image"):GetComponent(typeof(ImageWidget))
	self.qr_coupon_code_text=self.go.transform:Find("qr_bg/qr_coupon_code_text"):GetComponent(typeof(TextWidget))
	self.qr_use_tip_text=self.go.transform:Find("qr_bg/qr_use_tip_text"):GetComponent(typeof(TextWidget))
	self.actionItemArr={}
	table.insert(self.actionItemArr,self.new_ActionItem(self.go.transform:Find("middle/cellgroup/CellItem").gameObject))
	table.insert(self.actionItemArr,self.new_ActionItem(self.go.transform:Find("middle/cellgroup/CellItem_1").gameObject))
	table.insert(self.actionItemArr,self.new_ActionItem(self.go.transform:Find("middle/cellgroup/CellItem_2").gameObject))
end

--ActionItem复用单元
function this.new_ActionItem(itemGo)
	local item = { }
	item.go = itemGo
	item.action_icon=itemGo.transform:Find("action_icon"):GetComponent(typeof(IconWidget))
	item.action_text=itemGo.transform:Find("action_text"):GetComponent(typeof(TextWidget))
	return item
end
