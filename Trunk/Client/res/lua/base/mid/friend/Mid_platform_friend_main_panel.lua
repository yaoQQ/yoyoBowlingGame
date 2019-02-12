﻿local EmptyImageWidget=CS.EmptyImageWidget
local CircleImageWidget=CS.CircleImageWidget
local TextWidget=CS.TextWidget
local ImageWidget=CS.ImageWidget
local IconWidget=CS.IconWidget
local GridRecycleScrollWidget=CS.GridRecycleScrollWidget
local PanelWidget=CS.PanelWidget
local ButtonWidget=CS.ButtonWidget
local InputFieldWidget=CS.InputFieldWidget

Mid_platform_friend_main_panel={}
local this = Mid_platform_friend_main_panel

function this:new(gameObject)
	local o = { }
	setmetatable(o, self)
	self.__index = self
	o:init(gameObject)
	return o
end

this.go = nil
this.back_Image=nil
this.head_Image=nil
this.repu_Text=nil
this.distance_Text=nil
this.name_Text=nil
this.id_Text=nil
this.address_Text=nil
this.sex_Image=nil
this.sexbg_Icon=nil
this.level_Text=nil
this.nopyq_Text=nil
this.single=nil
this.single_image=nil
this.pyq_GridRecycleScrollPanel=nil
this.nonfriend_noactive_Panel=nil
this.addfriend_Button=nil
this.decline_Button=nil
this.friend_Panel=nil
this.sendmsg_Button=nil
this.cancel_Button=nil
this.applied_Panel=nil
this.applied_Button=nil
this.nonfriend_active_Panel=nil
this.addactivefriend_Button=nil
this.sendtip_Panel=nil
this.msg_InputField=nil
this.send_Button=nil
this.closetip_Image=nil
this.clear_Image=nil
--PyqCell数组
this.pyqCellArr={}

function this:init(gameObject)
	self.go=gameObject
	self.back_Image=self.go.transform:Find("top_Panel/back_Image/back_Image"):GetComponent(typeof(EmptyImageWidget))
	self.head_Image=self.go.transform:Find("top_Panel/baseinfo_Panel/head_Image"):GetComponent(typeof(CircleImageWidget))
	self.repu_Text=self.go.transform:Find("top_Panel/baseinfo_Panel/head_Image/repu_Text"):GetComponent(typeof(TextWidget))
	self.distance_Text=self.go.transform:Find("top_Panel/baseinfo_Panel/head_Image/distance_Text"):GetComponent(typeof(TextWidget))
	self.name_Text=self.go.transform:Find("top_Panel/baseinfo_Panel/head_Image/name_Text"):GetComponent(typeof(TextWidget))
	self.id_Text=self.go.transform:Find("top_Panel/baseinfo_Panel/head_Image/id_Text"):GetComponent(typeof(TextWidget))
	self.address_Text=self.go.transform:Find("top_Panel/baseinfo_Panel/head_Image/address_Text"):GetComponent(typeof(TextWidget))
	self.sex_Image=self.go.transform:Find("top_Panel/baseinfo_Panel/head_Image/sex_Image"):GetComponent(typeof(ImageWidget))
	self.sexbg_Icon=self.go.transform:Find("top_Panel/baseinfo_Panel/head_Image/sex_Image/sexbg_Icon"):GetComponent(typeof(IconWidget))
	self.level_Text=self.go.transform:Find("top_Panel/baseinfo_Panel/head_Image/sex_Image/level_Text"):GetComponent(typeof(TextWidget))
	self.nopyq_Text=self.go.transform:Find("mid_Panel/nopyq_Text"):GetComponent(typeof(TextWidget))
	self.single=self.go.transform:Find("mid_Panel/single"):GetComponent(typeof(ImageWidget))
	self.single_image=self.go.transform:Find("mid_Panel/single/single_image"):GetComponent(typeof(ImageWidget))
	self.pyq_GridRecycleScrollPanel=self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel"):GetComponent(typeof(GridRecycleScrollWidget))
	self.nonfriend_noactive_Panel=self.go.transform:Find("bottom_Panel/nonfriend_noactive_Panel"):GetComponent(typeof(PanelWidget))
	self.addfriend_Button=self.go.transform:Find("bottom_Panel/nonfriend_noactive_Panel/addfriend_Button"):GetComponent(typeof(ButtonWidget))
	self.decline_Button=self.go.transform:Find("bottom_Panel/nonfriend_noactive_Panel/decline_Button"):GetComponent(typeof(ButtonWidget))
	self.friend_Panel=self.go.transform:Find("bottom_Panel/friend_Panel"):GetComponent(typeof(PanelWidget))
	self.sendmsg_Button=self.go.transform:Find("bottom_Panel/friend_Panel/sendmsg_Button"):GetComponent(typeof(ButtonWidget))
	self.cancel_Button=self.go.transform:Find("bottom_Panel/friend_Panel/cancel_Button"):GetComponent(typeof(ButtonWidget))
	self.applied_Panel=self.go.transform:Find("bottom_Panel/applied_Panel"):GetComponent(typeof(PanelWidget))
	self.applied_Button=self.go.transform:Find("bottom_Panel/applied_Panel/applied_Button"):GetComponent(typeof(ButtonWidget))
	self.nonfriend_active_Panel=self.go.transform:Find("bottom_Panel/nonfriend_active_Panel"):GetComponent(typeof(PanelWidget))
	self.addactivefriend_Button=self.go.transform:Find("bottom_Panel/nonfriend_active_Panel/addactivefriend_Button"):GetComponent(typeof(ButtonWidget))
	self.sendtip_Panel=self.go.transform:Find("sendtip_Panel"):GetComponent(typeof(PanelWidget))
	self.msg_InputField=self.go.transform:Find("sendtip_Panel/msg_InputField"):GetComponent(typeof(InputFieldWidget))
	self.send_Button=self.go.transform:Find("sendtip_Panel/send_Button"):GetComponent(typeof(ButtonWidget))
	self.closetip_Image=self.go.transform:Find("sendtip_Panel/closetip_Image"):GetComponent(typeof(ButtonWidget))
	self.clear_Image=self.go.transform:Find("sendtip_Panel/clear_Image"):GetComponent(typeof(EmptyImageWidget))
	self.pyqCellArr={}
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_0_1").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_0_2").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_0_3").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_0_4").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_1_0").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_1_1").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_1_2").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_1_3").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_1_4").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_2_0").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_2_1").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_2_2").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_2_3").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_2_4").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_3_0").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_3_1").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_3_2").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_3_3").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_3_4").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_4_0").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_4_1").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_4_2").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_4_3").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_4_4").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_5_0").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_5_1").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_5_2").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_5_3").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_5_4").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_6_0").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_6_1").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_6_2").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_6_3").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_6_4").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_7_0").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_7_1").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_7_2").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_7_3").gameObject))
	table.insert(self.pyqCellArr,self.new_PyqCell(self.go.transform:Find("mid_Panel/pyq_GridRecycleScrollPanel/content/cellitem_7_4").gameObject))
end

--PyqCell复用单元
function this.new_PyqCell(itemGo)
	local item = { }
	item.go = itemGo
	item.pic_Image=itemGo.transform:Find("pic_Image"):GetComponent(typeof(ImageWidget))
	return item
end
