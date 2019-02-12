require "base:mid/game/Mid_platform_game_update_panel"

--平台示例界面
PlatformGameUpdateView = BaseView:new()
local this = PlatformGameUpdateView
this.viewName = "PlatformGameUpdateView"

--设置面板特性（界面层级、界面枚举、是否参与界面堆栈）
this:setViewAttribute(UIViewType.Alert_box, UIViewEnum.Platform_Game_Update_View, false)

--设置加载列表
this.loadOrders =
{
	"base:game/platform_game_update_panel",
}

local gameId = 0
local gameType = 0
local size = 0
local shopId = 0
local roomId = 0

local confirmContent = "当前游戏有新版更新后才能正常进入,是否进行更新"
local cancelContent = "快要下载完了, 确定中止下载吗?"

--override 加载UI完成回调
function this:onLoadUIEnd(uiName, gameObject)
	--下面两行默认需要调用
	
	UITools.SetParentAndAlign(gameObject, self.container)
	
	--设置UI中间代码
	self.main_mid = Mid_platform_game_update_panel
	self:BindMonoTable(gameObject, self.main_mid)
	--添加UI事件监听
	self:addEvent()
end

function this:addEvent()
	self.main_mid.close_down_image:AddEventListener(UIEvent.PointerClick, this.showCancelPanel)
end

--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
	gameId = msg.gameId
	gameType = msg.gameType
	shopId = msg.shopId
	roomId = msg.roomId
	size = msg.size
	self:initGameUpdateView()
end

--override 关闭UI回调
function this:onClose()
	--关闭界面时移除UI通知监听
	self:removeNotice()
end

function this:addNotice()
	NoticeManager.Instance:AddNoticeLister(NoticeType.Game_Update_Progress, this.onGameUpdateProgress)
end

function this:removeNotice()
	NoticeManager.Instance:RemoveNoticeLister(NoticeType.Game_Update_Progress, this.onGameUpdateProgress)
end

--打开界面时初始化
function this:initGameUpdateView()
	self:showUpdatePanel()
end

function this:showUpdatePanel()
	self.main_mid.update_panel.gameObject:SetActive(true)
	self.main_mid.game_down_panel.gameObject:SetActive(false)
	self.main_mid.update_mask_image.gameObject:SetActive(false)
	self.main_mid.down_process_fg.Img.fillAmount = 0
	local showSize =  math.floor((size /(1024 * 1024)) * 100)
	showSize = showSize / 100
	if showSize < 0.01 then
		showSize = 0.01
	end
	self.main_mid.update_text.text = string.format("%s(%sM)", confirmContent, showSize)
	self.main_mid.update_left_btn.Txt.text = "下次吧"
	self.main_mid.update_right_btn.Txt.text = "确定更新"
	self.main_mid.update_left_btn:AddEventListener(UIEvent.PointerClick, this.closeGameUpdateView)
	self.main_mid.update_right_btn:AddEventListener(UIEvent.PointerClick, this.confirmUpdateHandler)
end

function this:confirmUpdateHandler()
	CSGameProcessManager.StartDownload(gameId)
	this.main_mid.update_panel.gameObject:SetActive(false)
	this.main_mid.game_down_panel.gameObject:SetActive(true)
	downloadGameIcon(gameId ,this.main_mid.down_game_image)
	this.main_mid.down_game_text.text = string.format("正在下载《%s》",TableBaseGameList.data[gameId].name)
	this.main_mid.down_cancel_btn:AddEventListener(UIEvent.PointerClick, this.showCancelPanel)
end

function this.cancelUpdateHandler()
	CSGameProcessManager.CancelDownload(gameId)
	this:closeGameUpdateView()
end

function this.showCancelPanel()
	this.main_mid.update_panel.gameObject:SetActive(true)
	this.main_mid.game_down_panel.gameObject:SetActive(true)
	this.main_mid.update_mask_image.gameObject:SetActive(true)
	this.main_mid.update_text.text = string.format("%s", cancelContent)
	this.main_mid.update_left_btn.Txt.text = "狠心中止"
	this.main_mid.update_right_btn.Txt.text = "继续下载"
	this.main_mid.update_left_btn:AddEventListener(UIEvent.PointerClick, this.cancelUpdateHandler)
	this.main_mid.update_right_btn:AddEventListener(UIEvent.PointerClick, function ()
		this.main_mid.update_mask_image.gameObject:SetActive(false)
		this.main_mid.update_panel.gameObject:SetActive(false)
	end)
end

function this.onGameUpdateProgress(noticeType, notice)
	local process = notice:GetObj()
	if process >= 1 then
		this:closeGameUpdateView()
		GameManager.startGame(gameId, gameType, shopId, roomId)
	else
		this.main_mid.down_process_fg.Img.fillAmount = process
		this.main_mid.down_capacity_text.text = string.format("%sK/%sK",math.floor((size * process) / 1024), math.floor(size / 1024))
		this.main_mid.down_percent_text.text = string.format("%0.2f", process * 100).."%"
	end
end

function this:closeGameUpdateView()
	ViewManager.close(UIViewEnum.Platform_Game_Update_View)
end