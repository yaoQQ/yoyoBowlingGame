

require "base:enum/UIViewEnum"
require "base:enum/NoticeType"
require "base:mid/Mid_select_server_panel"

SelectServerView=BaseView:new()
local this=SelectServerView
this.viewName="SelectServerView"


--设置面板特性
this:setViewAttribute(UIViewType.Pop_view, UIViewEnum.SelectServerView, false)

--设置加载列表
this.loadOrders=
{
	"base:select_server_panel", 
}

--override 加载UI完成回调
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self.main_mid.go:SetActive(true)	
	
	self.main_mid.btn_connect:AddEventListener(UIEvent.PointerClick, self.showBowlingGame)
	--self.main_mid.btn_token_connect:AddEventListener(UIEvent.PointerClick, self.onBtnTokenConnectClick)
	self.main_mid.btn_server1:AddEventListener(UIEvent.PointerClick, self.onBtnServer1Click)
	self.main_mid.btn_server2:AddEventListener(UIEvent.PointerClick, self.onBtnServer2Click)
	
	this.main_mid.inputfield_account.gameObject:SetActive(false)
	self.main_mid.btn_token_connect.gameObject:SetActive(false)
	local ip = "192.168.1.104"
	this.main_mid.inputfield_ip.text =ip
	this.main_mid.inputfield_port.text= "8086"
end


function this.showBowlingGame(eventData)
	if this.main_mid.inputfield_ip.text~="" then
		this.onBtnConnectClick()
		require "bowling:module/mvp/MVPGameModule"
		MVPGameModule.initRegisterNet()
		--return
	end
	
	printDebug("@@@@@@@@@@@@@@begain click!! showBowlingGame~~")

	GameManager.startGame(EnumGameID.Bowling)
--[[	require "bowling:BowlingPackage"
	BowlingPackage:init(function ()
			local gameData = {}
			gameData.game_id = EnumGameID.Bowling
			gameData.token = LoginDataProxy.token
			gameData.player_id = LoginDataProxy.playerId
			gameData.game_type = gameType
			gameData.shop_id = shopId
			gameData.room_id = activeId
			NoticeManager.Instance:Dispatch(CommonNoticeType.Game_Enter, gameData)
	end)--]]

	ViewManager.close(UIViewEnum.SelectServerView)
	printDebug("end click!! showBowlingGame~~")
	

end



--override 打开UI回调
function this:onShowHandler(msg)
--[[	this.main_mid.inputfield_ip.text = PlayerPrefs.GetString("LOGIN_IP", "")
	this.main_mid.inputfield_port.text = PlayerPrefs.GetString("LOGIN_PORT", "")
	this.main_mid.inputfield_account.text = PlayerPrefs.GetString("LOGIN_GUEST_ACCOUNT", "")--]]
	
	self:addNotice()
	
	local go = self:getViewGO()
	if go == nil then return end
	go.transform:SetAsLastSibling()
end

--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end

function this:addNotice()

end

function this:removeNotice()
end

function this.onBtnConnectClick(eventData)
	local ip = this.main_mid.inputfield_ip.text
	if ip == "" then
		showFloatTips("请填写正确的IP")
		return
	end
	local port = this.main_mid.inputfield_port.text

	if port == "" then
		showFloatTips("请填写正确的PORT")
		return
	end
	local guestAccount = this.main_mid.inputfield_account.text
	--[[if guestAccount == "" then
		showFloatTips("请填写游客账号")
		return
	end--]]
	
	PlayerPrefs.SetString("LOGIN_IP", ip)
	PlayerPrefs.SetString("LOGIN_PORT", port)
	PlayerPrefs.SetString("LOGIN_GUEST_ACCOUNT", guestAccount)
	GameConfig.loginIP = ip
	GameConfig.loginPort = tonumber(port)
	LoginDataProxy.guestAccount = guestAccount
	
	--ViewManager.open(UIViewEnum.LoginView)
	this.connectMainLoginServer(function ()
		printDebug("@@@@@@@@@@@@@@connectMainLoginServer 连接成功！！")
	end)
end
--=============================连接服务器=============================
function this.connectMainLoginServer(succeedcallBack)
    --连接login服前先断开
	local serverName = "bowling"
    NetworkManager.Instance:Disconnect(serverName)

    --[[--测试包禁止连接正式服
    if IS_TEST_SERVER and GameConfig.loginIP == GameConfig.loginIP3 and GameConfig.loginPort == GameConfig.loginPort3 then
        Alert.showAlertMsg(nil, "测试包禁止连接正式服", "确定")
        return
    end--]]

    printDebug("连接login服")
	NetworkManager.Instance:RegisterLoginServer(serverName)
    NetworkManager.Instance:Connect(
        serverName,
        GameConfig.loginIP,
        GameConfig.loginPort,
        function(result)
            if result == 0 then
                printDebug("连接login服成功")
                if succeedcallBack ~= nil then
                    succeedcallBack()
                end
		elseif result == 2 or result == 3 then --排除主动断开
                printDebug("连接login服失败：" .. result)
                ShowWaiting(false, "login")
                Alert.showAlertMsg(nil, "网络连接错误", "确定")
            end
        end
    )
end
function this.onBtnTokenConnectClick(eventData)
	local ip = this.main_mid.inputfield_ip.text
	if ip == "" then
		showFloatTips("请填写正确的IP")
		return
	end
	local port = this.main_mid.inputfield_port.text
	if port == "" then
		showFloatTips("请填写正确的PORT")
		return
	end
	local guestAccount = this.main_mid.inputfield_account.text
	--[[if guestAccount == "" then
		showFloatTips("请填写游客账号")
		return
	end--]]
	
	PlayerPrefs.SetString("LOGIN_IP", ip)
	PlayerPrefs.SetString("LOGIN_PORT", port)
	GameConfig.loginIP = ip
	GameConfig.loginPort = tonumber(port)
	LoginDataProxy.guestAccount = guestAccount
	
	LoginModule.startLoginMainLoginServer()
end

function this.onBtnServer1Click(eventData)
--	this.main_mid.inputfield_ip.text = GameConfig.loginIP1
--	this.main_mid.inputfield_port.text = tostring(GameConfig.loginPort1)
end

function this.onBtnServer2Click(eventData)
	this.main_mid.inputfield_ip.text = GameConfig.loginIPTest
	this.main_mid.inputfield_port.text = tostring(GameConfig.loginPortTest)
end