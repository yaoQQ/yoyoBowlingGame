UtilMethod = CS.UtilMethod
IS_UNITY_EDITOR = UtilMethod.IsUnityEditor()
print("IS_UNITY_EDITOR:"..tostring(IS_UNITY_EDITOR))
IS_ANDROID = CS.UnityEngine.Application.platform == CS.UnityEngine.RuntimePlatform.Android
print("IS_ANDROID:"..tostring(IS_ANDROID))
IS_IOS = CS.UnityEngine.Application.platform == CS.UnityEngine.RuntimePlatform.IPhonePlayer
print("IS_IOS:"..tostring(IS_IOS))
IS_INCLUDE_GAME = UtilMethod.isIncludeGame
print("IS_INCLUDE_GAME:"..tostring(IS_INCLUDE_GAME))
IS_TEST_SERVER = UtilMethod.isTestServer
print("IS_TEST_SERVER:"..tostring(IS_TEST_SERVER))
IS_SUPER_VERSION = UtilMethod.IsSuperVersion()
print("IS_SUPER_VERSION:"..tostring(IS_SUPER_VERSION))

require "base:common/CSDefine"
require "base:common/CSLoger"
require "base:common/GameConfig"
require "base:common/enum/CommonNoticeType"
require "base:common/enum/UIViewType"
require "base:common/collections/Queue"
require "base:common/util/Coroutine"
require "base:common/util/JsonUtil"
require "base:common/util/Math"
require "base:common/util/MethodUtil"
require "base:common/util/PlatformSDK"
require "base:common/mvc/BaseModule"
require "base:common/mvc/BaseScene"
require "base:common/mvc/BaseView"
require "base:common/manager/ActionManager"
require "base:common/manager/AudioManager"
require "base:common/manager/GameManager"
require "base:common/manager/ModelManager"
require "base:common/manager/PhotoManager"
require "base:common/manager/ProtobufManager"
require "base:common/manager/RechargeManager"
require "base:common/manager/SQLiteManager"
require "base:common/manager/TimeManager"
require "base:common/manager/ViewManager"
require "base:common/manager/PlatformPicManagerProxy"
require "base:common/data/CommonDataProxy"
require "base:common/data/LoginDataProxy"
require "base:common/AbstractPackage"
require "base:BasePackage"
require "base:BasePreloadOrder"

Main={}

local this=Main

this.is3DMap = false
this.screenWidthHeight = UtilMethod.GetScreenWidthHeight()
this.isInGame = false

function this.start()
	CSLoger.debug(Color.Yellow,"lua main 启动","成功")
	this.createLayerPanel()
	GameManager.init()
	ActionManager:init()
	BasePackage:init(function ()
		local disableList = {}
		for _, v in pairs(TableBaseDisableTermsData.data) do
			table.insert(disableList, v.name)
		end
		DisableTermsManager.Instance:Init(disableList)
		TableBaseDisableTermsData.clear()

		CS.PreloadManager.Instance:ExecuteOrder(BasePreloadOrder)
		this.enter()
	end)
end

function this.createLayerPanel()
	ViewManager.createUILayerPanel(UIViewType.Global_View, "LayerPanel_Global")
	ViewManager.createUILayerPanel(UIViewType.Map_View, "LayerPanel_Map")
	ViewManager.createUILayerPanel(UIViewType.MapFloat_View, "LayerPanel_MapFloat")
	ViewManager.createUILayerPanel(UIViewType.Main_view, "LayerPanel_Main")
	ViewManager.createUILayerPanel(UIViewType.Platform_Second_View, "LayerPanel_Platform_Second")
	ViewManager.createUILayerPanel(UIViewType.Platform_Help_View, "LayerPanel_Platform_Help")
	
	ViewManager.createUILayerPanel(UIViewType.Game_1, "LayerPanel_Game_1")
	ViewManager.createUILayerPanel(UIViewType.Game_2, "LayerPanel_Game_2")
	ViewManager.createUILayerPanel(UIViewType.Game_3, "LayerPanel_Game_3")
	ViewManager.createUILayerPanel(UIViewType.Game_4, "LayerPanel_Game_4")
	ViewManager.createUILayerPanel(UIViewType.Game_5, "LayerPanel_Game_5")
	ViewManager.createUILayerPanel(UIViewType.Game_6, "LayerPanel_Game_6")
	ViewManager.createUILayerPanel(UIViewType.Game_7, "LayerPanel_Game_7")
	ViewManager.createUILayerPanel(UIViewType.Game_8, "LayerPanel_Game_8")
	ViewManager.createUILayerPanel(UIViewType.Game_9, "LayerPanel_Game_9")
	ViewManager.createUILayerPanel(UIViewType.Game_10, "LayerPanel_Game_10")
	
	ViewManager.createUILayerPanel(UIViewType.Pop_view, "LayerPanel_Pop")
	ViewManager.createUILayerPanel(UIViewType.Alert_box, "LayerPanel_Alert")
	ViewManager.createUILayerPanel(UIViewType.Feedback_Tip, "LayerPanel_Feedback")
	ViewManager.createUILayerPanel(UIViewType.Mask_View, "LayerPanel_Mask")
	ViewManager.createUILayerPanel(UIViewType.NoviceGuide_View, "LayerPanel_NoviceGuide")
	ViewManager.createUILayerPanel(UIViewType.Plot_View, "LayerPanel_Plot")
end

function this.enter()
	--注册栈底界面
	ViewManager.registerStackButtomView(UIViewEnum.Platform_Global_Personal_View)
	ViewManager.registerStackButtomView(UIViewEnum.Platform_Global_RedBag_View)
	ViewManager.registerStackButtomView(UIViewEnum.Platform_Global_Shop_View)
	ViewManager.registerStackButtomView(UIViewEnum.Platform_Global_Message_View)
	ViewManager.registerStackButtomView(UIViewEnum.Platform_Global_Game_View)
	
--[[	if IS_UNITY_EDITOR then
		--显示上方状态栏
		ViewManager.open(UIViewEnum.StatusbarView)
	end--]]
	
	--预加载Waitting界面
	--ViewManager.open(UIViewEnum.WaittingView)
	
	--先打开主界面
	--ViewManager.open(UIViewEnum.Platform_Global_Shop_View)

	
	--if IS_UNITY_EDITOR or IS_TEST_SERVER then
		printDebug("show UIViewEnum.SelectServerView()")
		ViewManager.open(UIViewEnum.SelectServerView)
		ViewManager.open(UIViewEnum.BgView)
	--else
	--	LoginModule.startLoginMainLoginServer()
	--end
end

--启动
this.start()

