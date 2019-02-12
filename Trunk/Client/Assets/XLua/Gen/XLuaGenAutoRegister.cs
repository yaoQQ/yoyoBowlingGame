#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;
using System.Collections.Generic;
using System.Reflection;


namespace XLua.CSObjectWrap
{
    public class XLua_Gen_Initer_Register__
	{
	    static XLua_Gen_Initer_Register__()
        {
		    XLua.LuaEnv.AddIniter((luaenv, translator) => {
			    
				translator.DelayWrapLoader(typeof(DG.Tweening.AutoPlay), DGTweeningAutoPlayWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.AxisConstraint), DGTweeningAxisConstraintWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.Ease), DGTweeningEaseWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.LogBehaviour), DGTweeningLogBehaviourWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.LoopType), DGTweeningLoopTypeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.PathMode), DGTweeningPathModeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.PathType), DGTweeningPathTypeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.RotateMode), DGTweeningRotateModeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.ScrambleMode), DGTweeningScrambleModeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.TweenType), DGTweeningTweenTypeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.UpdateType), DGTweeningUpdateTypeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.DOTween), DGTweeningDOTweenWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.DOVirtual), DGTweeningDOVirtualWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.EaseFactory), DGTweeningEaseFactoryWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.Tweener), DGTweeningTweenerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.Tween), DGTweeningTweenWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.Sequence), DGTweeningSequenceWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.TweenParams), DGTweeningTweenParamsWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.Core.ABSSequentiable), DGTweeningCoreABSSequentiableWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>), DGTweeningCoreTweenerCore_3_UnityEngineVector3UnityEngineVector3DGTweeningPluginsOptionsVectorOptions_Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.TweenExtensions), DGTweeningTweenExtensionsWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.TweenSettingsExtensions), DGTweeningTweenSettingsExtensionsWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.ShortcutExtensions), DGTweeningShortcutExtensionsWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.ShortcutExtensions43), DGTweeningShortcutExtensions43Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.ShortcutExtensions46), DGTweeningShortcutExtensions46Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.ShortcutExtensions50), DGTweeningShortcutExtensions50Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(System.DateTime), SystemDateTimeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.KeyCode), UnityEngineKeyCodeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.TouchPhase), UnityEngineTouchPhaseWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Animator), UnityEngineAnimatorWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Camera), UnityEngineCameraWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Collider2D), UnityEngineCollider2DWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Collision2D), UnityEngineCollision2DWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Color), UnityEngineColorWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.ContactPoint2D), UnityEngineContactPoint2DWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.GameObject), UnityEngineGameObjectWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.UI.Image), UnityEngineUIImageWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Material), UnityEngineMaterialWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.MeshFilter), UnityEngineMeshFilterWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.MeshRenderer), UnityEngineMeshRendererWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Physics2D), UnityEnginePhysics2DWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.PlayerPrefs), UnityEnginePlayerPrefsWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.PolygonCollider2D), UnityEnginePolygonCollider2DWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Quaternion), UnityEngineQuaternionWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.RaycastHit), UnityEngineRaycastHitWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.RectTransform), UnityEngineRectTransformWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Renderer), UnityEngineRendererWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Rigidbody2D), UnityEngineRigidbody2DWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Sprite), UnityEngineSpriteWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.SpriteRenderer), UnityEngineSpriteRendererWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Time), UnityEngineTimeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.UI.ToggleGroup), UnityEngineUIToggleGroupWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.TrailRenderer), UnityEngineTrailRendererWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Transform), UnityEngineTransformWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Vector2), UnityEngineVector2Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Vector3), UnityEngineVector3Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(EZReplayManager), EZReplayManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(testManage), testManageWrap.__Register);
				
				translator.DelayWrapLoader(typeof(LoadManager), LoadManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DeviceUtil), DeviceUtilWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ImageEvent), ImageEventWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ImageUtil), ImageUtilWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Loger), LogerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(MeshUtil), MeshUtilWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Tween), TweenWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UITools), UIToolsWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UtilMethod), UtilMethodWrap.__Register);
				
				translator.DelayWrapLoader(typeof(GameProcessManager), GameProcessManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(PhysicGameManager), PhysicGameManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(InputManager), InputManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(SceneClickNotice), SceneClickNoticeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(LuaActionController), LuaActionControllerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(MapManager), MapManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ModuleManager), ModuleManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(NetResDownloadManager), NetResDownloadManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(AliyunOSSManager), AliyunOSSManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(HttpPostManager), HttpPostManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(JavaNetWorkManager), JavaNetWorkManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(JsonPackage), JsonPackageWrap.__Register);
				
				translator.DelayWrapLoader(typeof(NetworkManager), NetworkManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Package), PackageWrap.__Register);
				
				translator.DelayWrapLoader(typeof(NetworkEventManager), NetworkEventManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(NoticeManager), NoticeManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ObjectNotice), ObjectNoticeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ObjectPoolManager), ObjectPoolManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(PreloadManager), PreloadManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(AudioManager), AudioManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(RecordManager), RecordManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(EffectManager), EffectManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ModelManager), ModelManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ResReleaseManager), ResReleaseManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(SceneCamera), SceneCameraWrap.__Register);
				
				translator.DelayWrapLoader(typeof(SceneCell), SceneCellWrap.__Register);
				
				translator.DelayWrapLoader(typeof(SceneContainer), SceneContainerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(SceneEntity), SceneEntityWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ScenePosPoint), ScenePosPointWrap.__Register);
				
				translator.DelayWrapLoader(typeof(SceneManager), SceneManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UILoadControl), UILoadControlWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UIManager), UIManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ResDownLoadContoller), ResDownLoadContollerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(LoadingBarController), LoadingBarControllerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(PlatformSDK), PlatformSDKWrap.__Register);
				
				translator.DelayWrapLoader(typeof(SQLiteManager), SQLiteManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ScreenSnapManager), ScreenSnapManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DisableTermsManager), DisableTermsManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(GlobalTimeManager), GlobalTimeManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(TimeManager), TimeManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UIViewManager), UIViewManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ButtonWidget), ButtonWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(CellGroupWidget), CellGroupWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(CellItemWidget), CellItemWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(CellRecycleNewScrollWidget), CellRecycleNewScrollWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(CellRecycleScrollWidget), CellRecycleScrollWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(CircleImageWidget), CircleImageWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(EffectWidget), EffectWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(EmptyImageWidget), EmptyImageWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UICustomEvent), UICustomEventWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UIEvent), UIEventWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UISpineEvent), UISpineEventWrap.__Register);
				
				translator.DelayWrapLoader(typeof(GridRecycleScrollWidget), GridRecycleScrollWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(HorizontalLayoutGroupWidget), HorizontalLayoutGroupWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(IconWidget), IconWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ImageWidget), ImageWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(InputFieldWidget), InputFieldWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(MarqueeWidget), MarqueeWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(MaskWidget), MaskWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(PanelWidget), PanelWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(RawImageWidget), RawImageWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ScrollPanelWidget), ScrollPanelWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ScrollPanelWithButtonWidget), ScrollPanelWithButtonWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(SliderWidget), SliderWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(SpineWidget), SpineWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(TabPanelWidget), TabPanelWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(TextPicWidget), TextPicWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(TextWidget), TextWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ToggleWidget), ToggleWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(VerticalLayoutGroupWidget), VerticalLayoutGroupWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(AnimationWidget), AnimationWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(AnimatorWidget), AnimatorWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(BannerWidget), BannerWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(NumberPickerWidget), NumberPickerWidgetWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UIExEventTool), UIExEventToolWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UIBaseMono), UIBaseMonoWrap.__Register);
				
				
				
				translator.AddInterfaceBridgeCreator(typeof(LuaActionManager), LuaActionManagerBridge.__Create);
				
				translator.AddInterfaceBridgeCreator(typeof(LuaModule), LuaModuleBridge.__Create);
				
				translator.AddInterfaceBridgeCreator(typeof(LuaPreloadOrder), LuaPreloadOrderBridge.__Create);
				
				translator.AddInterfaceBridgeCreator(typeof(LuaScene), LuaSceneBridge.__Create);
				
				translator.AddInterfaceBridgeCreator(typeof(LuaShareSDKManager), LuaShareSDKManagerBridge.__Create);
				
				translator.AddInterfaceBridgeCreator(typeof(LuaUIView), LuaUIViewBridge.__Create);
				
			});
		}
		
		
	}
	
}
namespace XLua
{
	public partial class ObjectTranslator
	{
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ s_gen_reg_dumb_obj = new XLua.CSObjectWrap.XLua_Gen_Initer_Register__();
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ gen_reg_dumb_obj {get{return s_gen_reg_dumb_obj;}}
	}
	
	internal partial class InternalGlobals
    {
	    
	    static InternalGlobals()
		{
		    extensionMethodMap = new Dictionary<Type, IEnumerable<MethodInfo>>()
			{
			    
			};
			
			genTryArrayGetPtr = StaticLuaCallbacks.__tryArrayGet;
            genTryArraySetPtr = StaticLuaCallbacks.__tryArraySet;
		}
	}
}
