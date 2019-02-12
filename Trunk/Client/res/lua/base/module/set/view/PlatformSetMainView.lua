
require "base:enum/UIViewEnum"
require "base:mid/set/Mid_platform_set_main_panel"

PlatformSetMainView =BaseView:new()
local this=PlatformSetMainView
this.viewName="PlatformSetMainView"

--设置面板特性
this:setViewAttribute(UIViewType.Platform_Second_View, UIViewEnum.Platform_Set_MainView, true)

--设置加载列表
this.loadOrders=
{
	"base:set/platform_set_main_panel",
}

--初始化预制体，给main_mid赋值
function this:onLoadUIEnd(uiName,gameObject)
	
	self.main_mid={} 
	self:BindMonoTable(gameObject, self.main_mid)
	printDebug(self.container.name)
	UITools.SetParentAndAlign(gameObject, self.container)
	self:addEvent()
end

--override 打开UI回调
function this:onShowHandler(msg)
	self:addNotice()
end

--override 关闭UI回调
function this:onClose()	
	self:removeNotice()
end


function this:addNotice()
end

function this:removeNotice()
end

function this:addEvent()
	self.main_mid.back_Image:AddEventListener(UIEvent.PointerClick,function ()
		ViewManager.close(UIViewEnum.Platform_Set_MainView)
	end )
	self.main_mid.accountButton:AddEventListener(UIEvent.PointerClick,self.onOpenAccountPage)
	self.main_mid.messegeButton:AddEventListener(UIEvent.PointerClick,self.onOpenMessegePage)
	self.main_mid.aboutButton:AddEventListener(UIEvent.PointerClick,self.onOpenAboutPage)
	self.main_mid.helpButton:AddEventListener(UIEvent.PointerClick,self.onOpenHelpPage)
	self.main_mid.cleanButton:AddEventListener(UIEvent.PointerClick,self.onOpenClean)
	self.main_mid.exitButton:AddEventListener(UIEvent.PointerClick,self.onOpenExit)

end


--override 打开UI回调
function this:onShowHandler(msg)
	this.main_mid.cleanButton.Txt.text = ""
	printDebug("lost cach size !!"..self.main_mid.cleanButton.Txt.text)
end

--打开玩家账号设置
function this.onOpenAccountPage(eventData)
	showFloatTips("功能开发中敬请期待！")
end

--打开消息设置
function this.onOpenMessegePage(eventData)
	showFloatTips("功能开发中敬请期待！")
end

--打开关于界面
function this.onOpenAboutPage(eventData)
	printDebug("go about page ")
	ViewManager.open(UIViewEnum.Platform_Set_AboutView)
end

--打开帮助界面
function this.onOpenHelpPage(eventData)
	printDebug("go help page lost")
	ViewManager.open(UIViewEnum.Platform_Set_Help_View)
end

--打开清除缓存二级界面
function this.onOpenClean(eventData)
	Alert.showVerifyMsg(nil,"您确定要清除本地缓存？","取消",nil, "确定",function ()
		this.doCleanCache()
	end )
end

--打开退出二级界面
function this.onOpenExit(eventData)
	printDebug("!")
	Alert.showVerifyMsg(nil,"您确定要退出登录吗？","取消",nil, "确定",function ()
		this.doGCReturnLogin()
	end) 
end

function this.doCleanCache()
	--暂时直接清除全部游戏
	local persistentDataRootPath = UtilMethod.GetPersistentDataRootPath()
	UtilMethod.CleanCache(persistentDataRootPath.."/eliminate")
	UtilMethod.CleanCache(persistentDataRootPath.."/mahjonghul")
	UtilMethod.CleanCache(persistentDataRootPath.."/marbles")
	UtilMethod.CleanCache(persistentDataRootPath.."/doodle")
	--showFloatTips("本地游戏缓存已清除")
	Alert.showAlertMsg(nil, "本地游戏缓存已清除,需要重新启动应用", "立即重启", function ()
		PlatformSDK.restartApp()
	end) 
end

function this.doGCReturnLogin()
	printDebug("正在退出!")
	LoginDataProxy.logout()
end
