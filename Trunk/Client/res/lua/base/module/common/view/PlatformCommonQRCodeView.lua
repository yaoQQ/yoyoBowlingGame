

require "base:enum/UIViewEnum"
require "base:mid/common/Mid_platform_common_qrcode_panel"

PlatformCommonQRCodeView = BaseView:new()
local this = PlatformCommonQRCodeView
this.viewName = "PlatformCommonQRCodeView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Help_View, UIViewEnum.Platform_Common_QRCode_View, false)

--设置加载列表
this.loadOrders=
{
	"base:common/platform_common_qrcode_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

this.scanText = nil
function this:onShowHandler(msg)
	printDebug("=====================Platform_Common_QRCode_View调用完毕======================")

	if msg == nil then return end
	printDebug("=====================Platform_Common_QRCode_View  msg======== :"..table.tostring(msg))
	this.scanText = msg.scanText
	
	self.main_mid.Tile_Text.text = msg.titleText
	self.main_mid.Tip1_Text.text = msg.tip1Text
	self:updateQRCodeView()
end

function this:addEvent()
	self.main_mid.cancel_Image:AddEventListener(UIEvent.PointerClick,function ()
		ViewManager.close(UIViewEnum.Platform_Common_QRCode_View)
	end)
end

function this:updateQRCodeView()

	-- local scanText = LoginDataProxy.playerId
	this.currGlobalBaseData = PlatformUserProxy:GetInstance():getUserInfo()

	if this.currGlobalBaseData == nil then return end
	--printDebug("++++++++++++++++++++++++this.currGlobalBaseData:"..table.tostring(this.currGlobalBaseData))
	self.main_mid.name_Text.text = this.currGlobalBaseData.nick_name
	self.main_mid.sex_Icon:ChangeIcon(this.currGlobalBaseData.sex - 1)
	self.main_mid.ID_Text.text = "ID:"..this.currGlobalBaseData.player_id
	downloadUserHead(this.currGlobalBaseData.head_url,self.main_mid.head_Icon)
	printDebug("二维码信息scanText为："..this.scanText)

	local m_Texture2D = UtilMethod.GetQrCode(tostring(this.scanText))

	ImageUtil.setTexture2DImage(m_Texture2D,self.main_mid.qrcode_Image.Img)
	
end
