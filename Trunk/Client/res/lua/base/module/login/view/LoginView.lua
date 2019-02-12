

require "base:enum/UIViewEnum"
require "base:mid/Mid_login_panel"
require "base:enum/NoticeType"
require "base:module/login/LoginModule"

LoginView = BaseView:new()
local this = LoginView
this.viewName = "LoginView"

this.getAuthcodeTime = 0
this.getAuthcodeCount = 0

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.LoginView, false)

--设置加载列表
this.loadOrders=
{
	"base:login_panel"
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	--.静态方法
	UITools.SetParentAndAlign(gameObject, self.container)
	
	--主界面
	self.main_mid.BtnMainLoginAlipay:AddEventListener(UIEvent.PointerClick, self.onBtnMainLoginAlipay)
	self.main_mid.BtnMainLoginWx:AddEventListener(UIEvent.PointerClick, self.onBtnMainLoginWx)
	self.main_mid.BtnMainLoginPhone:AddEventListener(UIEvent.PointerClick, self.onBtnMainLoginPhone)
	self.main_mid.BtnMainRegister:AddEventListener(UIEvent.PointerClick, self.onBtnMainRegister)
	self.main_mid.BtnUserAgreement:AddEventListener(UIEvent.PointerClick, self.onBtnUserAgreement)
	
	--注册界面
	self.main_mid.BtnRegisterAuth:AddEventListener(UIEvent.PointerClick, self.onBtnRegisterAuth)
	self.main_mid.BtnRegisterSee:AddEventListener(UIEvent.PointerClick, self.onBtnRegisterSee)
	self.main_mid.BtnRegisterRegister:AddEventListener(UIEvent.PointerClick, self.onBtnRegisterRegister)
	self.main_mid.BtnRegisterBack:AddEventListener(UIEvent.PointerClick, self.onBtnRegisterBack)
	
	--登录界面
	self.main_mid.BtnLoginSee:AddEventListener(UIEvent.PointerClick, self.onBtnLoginSee)
	self.main_mid.BtnLoginReset:AddEventListener(UIEvent.PointerClick, self.onBtnLoginReset)
	--self.main_mid.BtnLoginRegister:AddEventListener(UIEvent.PointerClick, self.onBtnLoginRegister)
	self.main_mid.BtnLoginLogin:AddEventListener(UIEvent.PointerClick, self.onBtnLoginLogin)
	--self.main_mid.BtnLoginBack:AddEventListener(UIEvent.PointerClick, self.onBtnLoginBack)
	
	--重置密码界面
	self.main_mid.BtnResetAuth:AddEventListener(UIEvent.PointerClick, self.onBtnResetAuth)
	self.main_mid.BtnResetSee:AddEventListener(UIEvent.PointerClick, self.onBtnResetSee)
	self.main_mid.BtnResetReset:AddEventListener(UIEvent.PointerClick, self.onBtnResetReset)
	self.main_mid.BtnResetBack:AddEventListener(UIEvent.PointerClick, self.onBtnResetBack)
	
	
	--微信未安装提示界面
	self.main_mid.BtnWeChatClose:AddEventListener(UIEvent.PointerClick, self.onBtnWeChatClose)
	
	if IS_IOS and IS_SUPER_VERSION then
		--iOS提审屏蔽第三方登录
		self.main_mid.bottom_panel.gameObject:SetActive(false)
	end
end

--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
	
	ViewManager.close(UIViewEnum.SelectServerView)
	
	local go = self:getViewGO()
	if go == nil then return end
	go.transform:SetAsLastSibling()
	
	self.main_mid.PageMain.gameObject:SetActive(true)
	self.main_mid.PageRegister.gameObject:SetActive(false)
	self.main_mid.PageLogin.gameObject:SetActive(true)
	self.main_mid.PageReset.gameObject:SetActive(false)
end

--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.Login_SMSAuthSucceed, this.onGetAuthcodeSucceed)
	NoticeManager.Instance:AddNoticeLister(NoticeType.Login_RegisterSucceed, this.onRegisterSucceed)
	NoticeManager.Instance:AddNoticeLister(NoticeType.Login_ResetSucceed, this.onResetSucceed)
	
	local curTime = Time.realtimeSinceStartup
	if this.getAuthcodeTime > 0 and curTime + 1 < this.getAuthcodeTime + 60 then
		this.getAuthcodeCount = this.getAuthcodeTime + 60 - curTime
	end
	GlobalTimeManager.Instance.timerController:AddTimer("Password", 1000, this.getAuthcodeCount, this.onResetGetAuthcode)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Login_SMSAuthSucceed, this.onGetAuthcodeSucceed)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Login_RegisterSucceed, this.onRegisterSucceed)
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Login_ResetSucceed, this.onResetSucceed)
	
	this.getAuthcodeCount = 0
	GlobalTimeManager.Instance.timerController:RemoveTimerByKey("Password")
end

--注册界面初始化
function this.initPageRegister()
	this.main_mid.BtnRegisterBack.gameObject:SetActive(true)
	this.main_mid.BtnResetBack.gameObject:SetActive(false)
	this.main_mid.InputFieldRegisterAccount.text = ""
	this.main_mid.InputFieldRegisterAuth.text = ""
	this.main_mid.InputFieldRegisterPassword.text = ""
	this.main_mid.top_Text.text = "新用户注册"
end

--登录界面初始化
function this.initPageLogin()
	this.main_mid.top_Text.text = "登录"
	this.main_mid.InputFieldLoginAccount.text = PlayerPrefs.GetString("LOGIN_NUM", "")
	if IS_UNITY_EDITOR or IS_TEST_SERVER then
		this.main_mid.InputFieldLoginPassword.text = PlayerPrefs.GetString("LOGIN_PASSWORD", "")
	else
		this.main_mid.InputFieldLoginPassword.text = ""
	end
	
end

--重置密码界面初始化
function this.initPageReset()
	this.main_mid.InputFieldResetAccount.text = ""
	this.main_mid.InputFieldResetAuth.text = ""
	this.main_mid.InputFieldResetPassword.text = ""
	this.main_mid.top_Text.text = "找回密码"
end


--主界面支付宝登录
function this.onBtnMainLoginAlipay(eventData)
	ShowWaiting(true, "login")
	LoginModule.connectMainLoginServer(
        function()
			PlatformSDK.aliPayLogin(function (authCode)
				--支付宝登录
				printDebug("支付宝登录")
				LoginModule.connectMainLoginServer(
					function()
						LoginModule.sendReqMainLogin(ProtoEnumCommon.LoginType.LoginTypeAliPay, "", 0, authCode)
					end
				)
			end, function (errCode)
				if errCode == "4000" then
					showFloatTips("系统异常")
				elseif errCode == "6001" then
					showFloatTips("取消授权")
				elseif errCode == "6002" then
					showFloatTips("网络连接出错")
				else
					showFloatTips("支付宝授权失败:"..errCode)
				end
				ShowWaiting(false, "login")
			end)
        end
    )
end

--主界面微信登录
function this.onBtnMainLoginWx(eventData)
	PlatformSDK.wxLogin(function (authCode)
		printDebug("微信登录")
		ShowWaiting(true, "login")
		LoginModule.connectMainLoginServer(
			function()
				LoginModule.sendReqMainLogin(ProtoEnumCommon.LoginType.LoginTypeWeChat, "", 0, authCode)
			end
		)
	end, function (errCode)
		showFloatTips("微信授权失败"..errCode)
	end)
end

--主界面手机登录
function this.onBtnMainLoginPhone(eventData)
	--打开手机登录界面
	this.main_mid.PageMain.gameObject:SetActive(false)
	this.main_mid.PageLogin.gameObject:SetActive(true)
	this.initPageLogin()
end


--主界面注册
function this.onBtnMainRegister(eventData)
	--打开注册界面
	this.main_mid.PageMain.gameObject:SetActive(false)
	this.main_mid.PageRegister.gameObject:SetActive(true)
	this.main_mid.BtnRegisterBack.gameObject:SetActive(true)
	this.initPageRegister()
end

--主界面用户协议
function this.onBtnUserAgreement(eventData)
	--打开用户协议界面
	ViewManager.open(UIViewEnum.Platform_Common_Agreement_View)
end

--手机登录界面登录
function this.onBtnLoginLogin(eventData)
	local account = this.main_mid.InputFieldLoginAccount.text
	--[[if account == "" or string.byte(account) ~= 49 or #account ~= 11 then
		Alert.showAlertMsg("无效号码","请输入正确的手机号码", "好的")
		--AlertLoginWindowView:showLoginAlertMsg("无效号码","请确认输入的手机号码无误", "好的",nil)
		return
	end--]]
	
	local password = this.main_mid.InputFieldLoginPassword.text
	--[[if checkPassword(password) == false then
		return
	end--]]
	
	--保存手机号
	PlayerPrefs.SetString("LOGIN_NUM", account)
	--保存密码
	if IS_UNITY_EDITOR or IS_TEST_SERVER then
		PlayerPrefs.SetString("LOGIN_PASSWORD", password)
	end
	
	--手机号登录
	printDebug("手机号登录")
	ShowWaiting(true, "login")
	LoginModule.connectMainLoginServer(function ()
		LoginModule.sendReqMainLogin(ProtoEnumCommon.LoginType.LoginTypeMobile, account, nil, password)
	end)
end

--手机登录界面返回
function this.onBtnLoginBack(eventData)
	--打开主界面
	this.main_mid.PageMain.gameObject:SetActive(true)
	this.main_mid.PageLogin.gameObject:SetActive(false)
end
--注册界面发送验证码
function this.onBtnRegisterAuth(eventData)
	local account = this.main_mid.InputFieldRegisterAccount.text
	if account == "" or string.byte(account) ~= 49 or #account ~= 11 then
		Alert.showAlertMsg("无效号码","请输入正确的手机号码", "好的")
		--AlertLoginWindowView:showLoginAlertMsg("无效号码","请确认输入的手机号码无误", "好的",nil)
		return
	end
	
	--发送验证码
	printDebug("发送验证码")
	ShowWaiting(true, "login")
	LoginModule.connectMainLoginServer(function ()
		LoginModule.sendReqMainSMSAuth(account, 0)
	end)
end

--注册界面显示隐藏密码
function this.onBtnRegisterSee(eventData)
	local inputField = this.main_mid.InputFieldRegisterPassword.inputField
	local icon = this.main_mid.IconRegisterSee
	if icon.initIndex == 1 then
		inputField.contentType = 0
		inputField:ForceLabelUpdate()
		icon:ChangeIcon(0)
	else
		inputField.contentType = 7
		inputField:ForceLabelUpdate()
		icon:ChangeIcon(1)
	end
end

--注册界面注册
function this.onBtnRegisterRegister(eventData)
	local account = this.main_mid.InputFieldRegisterAccount.text
	if account == "" or string.byte(account) ~= 49 or #account ~= 11 then
		Alert.showAlertMsg("无效号码","请输入正确的手机号码", "好的")
		--AlertLoginWindowView:showLoginAlertMsg("无效号码","请确认输入的手机号码无误", "好的",nil)
		return
	end
	
	local auth = this.main_mid.InputFieldRegisterAuth.text
	if auth == "" then
		Alert.showAlertMsg("验证码错误","请输入正确的验证码", "好的")
		--AlertLoginWindowView:showLoginAlertMsg("验证码错误","请确认验证码并重新输入", "好的",nil)
		return
	end
	
	local password = this.main_mid.InputFieldRegisterPassword.text
	if checkPassword(password) == false then
		return
	end
	
	--注册
	printDebug("注册")
	ShowWaiting(true, "login")
	LoginModule.connectMainLoginServer(function ()
		LoginModule.sendReqRegister(account, auth, password)
	end)
end

--注册界面返回
function this.onBtnRegisterBack(eventData)
	--打开主界面
	this.main_mid.PageMain.gameObject:SetActive(true)
	this.main_mid.PageRegister.gameObject:SetActive(false)
	this.main_mid.BtnRegisterBack.gameObject:SetActive(false)
	this.initPageLogin()
end

--手机登录界面显示隐藏密码
function this.onBtnLoginSee(eventData)
	local inputField = this.main_mid.InputFieldLoginPassword.inputField
	local icon = this.main_mid.IconLoginSee
	if icon.initIndex == 1 then
		inputField.contentType = 0
		inputField:ForceLabelUpdate()
		icon:ChangeIcon(0)
	else
		inputField.contentType = 7
		inputField:ForceLabelUpdate()
		icon:ChangeIcon(1)
	end
end

--手机登录界面重置
function this.onBtnLoginReset(eventData)
	--打开重置界面
	this.main_mid.PageLogin.gameObject:SetActive(false)
	this.main_mid.PageMain.gameObject:SetActive(false)
	this.main_mid.PageReset.gameObject:SetActive(true)
	this.main_mid.BtnResetBack.gameObject:SetActive(true)
	this.initPageReset()
end
--[[
--手机登录界面注册
function this.onBtnLoginRegister(eventData)
	--打开注册界面
	this.main_mid.PageLogin.gameObject:SetActive(false)
	this.main_mid.PageRegister.gameObject:SetActive(true)
	this.initPageRegister()
end
--]]


--重置密码界面发送验证码
function this.onBtnResetAuth(eventData)
	local account = this.main_mid.InputFieldResetAccount.text
	if account == "" or string.byte(account) ~= 49 or #account ~= 11 then
		Alert.showAlertMsg("无效号码","请输入正确的手机号码", "好的")
		--AlertLoginWindowView:showLoginAlertMsg("无效号码","请确认输入的手机号码无误", "好的",nil)
		return
	end
	
	--发送验证码
	printDebug("发送验证码")
	ShowWaiting(true, "login")
	LoginModule.connectMainLoginServer(function ()
		LoginModule.sendReqMainSMSAuth(account, 1)
	end)
end

--重置密码界面显示隐藏密码
function this.onBtnResetSee(eventData)
	local inputField = this.main_mid.InputFieldResetPassword.inputField
	local icon = this.main_mid.IconResetSee
	if icon.initIndex == 1 then
		inputField.contentType = 0
		inputField:ForceLabelUpdate()
		icon:ChangeIcon(0)
	else
		inputField.contentType = 7
		inputField:ForceLabelUpdate()
		icon:ChangeIcon(1)
	end
end

--重置密码界面重置
function this.onBtnResetReset(eventData)
	local account = this.main_mid.InputFieldResetAccount.text
	if account == "" or string.byte(account) ~= 49 or #account ~= 11 then
		Alert.showAlertMsg("无效号码","请输入正确的手机号码", "好的")
		--AlertLoginWindowView:showLoginAlertMsg("无效号码","请确认输入的手机号码无误", "好的",nil)
		return
	end
	
	local auth = this.main_mid.InputFieldResetAuth.text
	if auth == "" then
		Alert.showAlertMsg("验证码错误","请输入正确的验证码", "好的")
		--AlertLoginWindowView:showLoginAlertMsg("验证码错误","请确认验证码并重新输入", "好的",nil)
		return
	end
	
	local password = this.main_mid.InputFieldResetPassword.text
	if checkPassword(password) == false then
		return
	end
	
	--重置
	printDebug("重置")
	ShowWaiting(true, "login")
	LoginModule.connectMainLoginServer(function ()
		LoginModule.sendReqResetPwd(account, auth, password)
	end)
end

--重置密码界面返回
function this.onBtnResetBack(eventData)
	--打开登录界面
	this.main_mid.PageMain.gameObject:SetActive(true)
	this.main_mid.PageLogin.gameObject:SetActive(true)
	this.main_mid.PageReset.gameObject:SetActive(false)
	this.main_mid.BtnResetBack.gameObject:SetActive(false)
	this.initPageLogin()
end




--请求短信验证成功返回
function this.onGetAuthcodeSucceed(notice, data)
	local rsp = data:GetObj()
	showFloatTips("验证码已发送到"..rsp.tel)
	this.getAuthcodeTime = Time.realtimeSinceStartup
	this.getAuthcodeCount = 60
	GlobalTimeManager.Instance.timerController:AddTimer("MainAuthcode", 1000, this.getAuthcodeCount, this.onResetGetAuthcode)
end

--注册成功返回
function this.onRegisterSucceed(notice, data)
	--showFloatTips("注册成功")
	
	this.main_mid.PageLogin.gameObject:SetActive(true)
	this.main_mid.PageRegister.gameObject:SetActive(false)
	this.initPageLogin()
	
	local account = this.main_mid.InputFieldRegisterAccount.text
	local password = this.main_mid.InputFieldRegisterPassword.text
	
	--保存手机号
	PlayerPrefs.SetString("LOGIN_NUM", account)
	--保存密码
	if IS_UNITY_EDITOR or IS_TEST_SERVER then
		PlayerPrefs.SetString("LOGIN_PASSWORD", password)
	end
	
	printDebug("注册成功自动手机号登录")
	ShowWaiting(true, "login")
	LoginModule.connectMainLoginServer(function ()
		LoginModule.sendReqMainLogin(ProtoEnumCommon.LoginType.LoginTypeMobile, account, nil, password)
	end)
end

--重置密码成功返回
function this.onResetSucceed(notice, data)
	showFloatTips("重置密码成功")
	
	this.main_mid.PageLogin.gameObject:SetActive(true)
	this.main_mid.PageReset.gameObject:SetActive(false)
	this.initPageLogin()
	
	local account = this.main_mid.InputFieldResetAccount.text
	local password = this.main_mid.InputFieldResetPassword.text
	
	--保存手机号
	PlayerPrefs.SetString("LOGIN_NUM", account)
	--保存密码
	if IS_UNITY_EDITOR or IS_TEST_SERVER then
		PlayerPrefs.SetString("LOGIN_PASSWORD", password)
	end
	
	printDebug("重置密码成功自动手机号登录")
	ShowWaiting(true, "login")
	LoginModule.connectMainLoginServer(function ()
		LoginModule.sendReqMainLogin(ProtoEnumCommon.LoginType.LoginTypeMobile, account, nil, password)
	end)
end

--刷新验证码按钮显示
function this.onResetGetAuthcode()
	this.getAuthcodeCount = this.getAuthcodeCount - 1
	if this.getAuthcodeCount > 0 then
		this.main_mid.BtnRegisterAuth.Btn.interactable = false
		this.main_mid.BtnRegisterAuth.Txt.text ="<color=#D2D2D2FF>已发送("..this.getAuthcodeCount.."s)</color>"
		this.main_mid.BtnResetAuth.Btn.interactable = false
		this.main_mid.BtnResetAuth.Txt.text = "<color=#D2D2D2FF>已发送("..this.getAuthcodeCount.."s)</color>"
	else
		this.main_mid.BtnRegisterAuth.Btn.interactable = true
		this.main_mid.BtnRegisterAuth.Txt.text = "发送验证码"
		this.main_mid.BtnResetAuth.Btn.interactable = true
		this.main_mid.BtnResetAuth.Txt.text = "发送验证码"
		GlobalTimeManager.Instance.timerController:RemoveTimerByKey("MainAuthcode")
	end
end

--关闭微信未安装提示界面
function this.onBtnWeChatClose()
	this.main_mid.PageWeChat.gameObject:SetActive(false)
end