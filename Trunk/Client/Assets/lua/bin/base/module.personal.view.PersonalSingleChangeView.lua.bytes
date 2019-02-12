
require "base:enum/UIViewEnum"
require "base:mid/personal/Mid_personal_single_change_panel"
require "base:enum/PlatformFriendType"
require "base:module/friend/data/FriendChatDataProxy"

PersonalSingleChangeView = BaseView:new()
local this = PersonalSingleChangeView
this.viewName = "PersonalSingleChangeView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Single_Change_Info_View, true)

--设置加载列表
this.loadOrders=
{
	"base:personal/personal_single_change_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

this.changeType = 0
function this:onShowHandler(msg)
	printDebug("=====================Single_Change_Info_View调用完毕======================")
	
	if msg ~= nil then 
		this.changeType = msg.changeType
	end
	self:initView()
	this:addNotice()
	this:updateSingleChange()
	
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.User_Update_UserBaseInfo, this.onChangeSuccess)
end


function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.User_Update_UserBaseInfo, this.onChangeSuccess)
end

function this:initView()
	self.main_mid.male_check_Image.gameObject:SetActive(false)
	self.main_mid.female_check_Image.gameObject:SetActive(false)
	this.main_mid.male_Icon:ChangeIcon(0)
	this.main_mid.male_Icon.gameObject:SetActive(true)
	this.main_mid.female_Icon:ChangeIcon(0)
	this.main_mid.female_Icon.gameObject:SetActive(true)
	m_isClick = true
end

--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end


function this.onChangeSuccess()
	printDebug("更新了哦")
	this:updateSingleChange()
	if this.changeType == 1 then
		showFloatTips("昵称修改成功")
		
	end
	--ViewManager.close(UIViewEnum.Single_Change_Info_View)
end

function this:addEvent()

	self.main_mid.back_Image:AddEventListener(UIEvent.PointerClick,function ()
		ViewManager.close(UIViewEnum.Single_Change_Info_View)
	end)

	self.main_mid.done_Button:AddEventListener(UIEvent.PointerClick,function ()
		--点击了完成按钮，向服务器发送修改资料消息
		ViewManager.close(UIViewEnum.Single_Change_Info_View)
		PersonalChangeInfoView:changeNameEnd()
		local data = {}
		if this.updateNickName ~= nil then
			data.nick_name = this.updateNickName
		end
		PlatformUserModule.sendReqChangeUserBaseInfo(data)
	end)
	self.main_mid.done_Sex_Button:AddEventListener(UIEvent.PointerClick,function ()
		--点击了完成按钮，向服务器发送修改资料消息
		this:setSexBtnEvent()
	end)
end


this.currSingleChangeData = nil

this.updateNickName = nil
this.updateSex = nil
this.preInputText = nil
local m_isClick = true

--更新好友申请列表
function this:updateSingleChange()
	this.updateNickName = nil
	this.updateSex = nil

	this.currSingleChangeData = PlatformUserProxy:GetInstance():getUserInfo()

	if this.currSingleChangeData == nil then return end

	printDebug("=======================更新单个修改界面！！！！"..table.tostring(this.currSingleChangeData))

	if this.changeType == 1 then--表示修改昵称

		self.main_mid.name_Panel.gameObject:SetActive(true)
		self.main_mid.sex_Panel.gameObject:SetActive(false)
	


		self.main_mid.change_type_Text.text = "设置昵称"
		--首先要拿到用户原来的昵称
		self.main_mid.name_InputField.text = this.currSingleChangeData.nick_name
		this.main_mid.name_prompt_Text.text=""
		--this:GetTextCount()
		self.main_mid.name_InputField:OnValueChanged(function ()	
				this:GetTextCount()	
		end)
		
		self.main_mid.close_Button:AddEventListener(UIEvent.PointerClick,function ()
			this.updateNickName = nil
			self.main_mid.name_InputField.text = ""
		end)

	elseif this.changeType ==2 then--表示修改性别

		self.main_mid.name_Panel.gameObject:SetActive(false)
		self.main_mid.sex_Panel.gameObject:SetActive(true)

		self.main_mid.change_type_Text.text = "设置性别"
		--this.updateSex = 2
		--首先要拿到用户原来的性别
		if  this.currSingleChangeData.modified_sex then
			
			self.main_mid.warning_Text.text = "您已经修改过性别了，不能再修改了！"
			m_isClick = false
			if this.currSingleChangeData.sex == 1 then
				this.main_mid.male_check_Image.gameObject:SetActive(false)
				this.main_mid.female_Icon.gameObject:SetActive(false)
				this.main_mid.female_check_Image.gameObject:SetActive(true)
				this.main_mid.male_Icon:ChangeIcon(1)
					
				--self.main_mid.female_mask_Image.gameObject:SetActive(true)
				--self.main_mid.male_mask_Image.gameObject:SetActive(false)
			elseif this.currSingleChangeData.sex == 2 then
				this.main_mid.male_check_Image.gameObject:SetActive(true)
				this.main_mid.male_Icon.gameObject:SetActive(false)
				this.main_mid.female_check_Image.gameObject:SetActive(false)
				this.main_mid.female_Icon:ChangeIcon(1)
				--self.main_mid.female_mask_Image.gameObject:SetActive(false)
				--self.main_mid.male_mask_Image.gameObject:SetActive(true)
			end
			self.main_mid.done_Sex_Button.gameObject:SetActive(false)
		else

			self.main_mid.warning_Text.text = "系统仅提供一次修改机会，请谨慎选择"
			m_isClick = true

			if this.currSingleChangeData.sex == 1 then
				self.main_mid.male_check_Image.gameObject:SetActive(false)
				self.main_mid.female_check_Image.gameObject:SetActive(false)
				self.main_mid.male_Icon:ChangeIcon(1)
			elseif this.currSingleChangeData.sex == 2 then
				self.main_mid.male_check_Image.gameObject:SetActive(false)
				self.main_mid.female_check_Image.gameObject:SetActive(false)
				self.main_mid.female_Icon:ChangeIcon(1)
			elseif this.currSingleChangeData.sex == 0 then
				this.main_mid.done_Sex_Button.gameObject:SetActive(false)
			end
			
		end
		self.main_mid.male_Icon:AddEventListener(UIEvent.PointerClick,function ()
			this.updateSex = 1
			self.main_mid.male_Icon:ChangeIcon(1)
			self.main_mid.female_Icon:ChangeIcon(0)
			if m_isClick then
				self.main_mid.done_Sex_Button.gameObject:SetActive(true)
			end
		end)

		self.main_mid.female_Icon:AddEventListener(UIEvent.PointerClick,function ()
			this.updateSex = 2
			self.main_mid.male_Icon:ChangeIcon(0)
			self.main_mid.female_Icon:ChangeIcon(1)
			if m_isClick then
				self.main_mid.done_Sex_Button.gameObject:SetActive(true)
			end
		end)

	end
end

function this:GetTextCount()
	if DisableTermsManager.Instance:IsMatch(this.main_mid.name_InputField.text) then
		Alert.showAlertMsg(nil,"您输入的文字包含违规内容，请修改后再尝试","确定")
		this.main_mid.name_InputField.text = ""
	else
		local count = string.len(this.main_mid.name_InputField.text)
		local result = count > 24 and 24 or count

		if count >24 then 
			result = 8
			this.main_mid.name_InputField.text = this.preInputText
			this.main_mid.name_prompt_Text.text="（昵称长度超出允许长度，最多为8个汉字）"
		else
			result = math.floor(count/3)
			this.preInputText = this.main_mid.name_InputField.text
			this.main_mid.name_prompt_Text.text=""
		end

		this.main_mid.word_left_Text.text = result.."/8"
		this.updateNickName = this.main_mid.name_InputField.text

		if this.main_mid.name_InputField.text~="" and self.main_mid.name_InputField.text~=nil then
			this.main_mid.close_Button.gameObject:SetActive(true)
		else
			this.main_mid.close_Button.gameObject:SetActive(false)
		end
	end
	
end

function this:setSexBtnEvent()
	local data = {}
	local fun = function()
		if this.updateSex ~= nil then
			data.sex = this.updateSex
		end
		if this.updateSex == 1 then 
			self.main_mid.male_check_Image.gameObject:SetActive(false)
			self.main_mid.female_check_Image.gameObject:SetActive(true)
			self.main_mid.female_Icon.gameObject:SetActive(false)
		elseif this.updateSex == 2 then
			self.main_mid.male_check_Image.gameObject:SetActive(true)
			self.main_mid.female_check_Image.gameObject:SetActive(false)
			self.main_mid.male_Icon.gameObject:SetActive(false)
		else
			printError("未知性别类型")
		end
		PlatformUserModule.sendReqChangeUserBaseInfo(data)
		m_isClick = false
	end
	Alert.showAlertVerify(nil,"性别修改后将不能再次修改，请确认是否继续修改", "确定", fun, true)
end
