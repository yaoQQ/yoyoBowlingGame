#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class ScrollPanelWithButtonWidgetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ScrollPanelWithButtonWidget);
			Utils.BeginObjectRegister(type, L, translator, 0, 9, 8, 8);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetWidgetType", _m_GetWidgetType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddEventListener", _m_AddEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveEventListener", _m_RemoveEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetContentSize", _m_SetContentSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResetPos", _m_ResetPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenPageScroll", _m_OpenPageScroll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClosePageScroll", _m_ClosePageScroll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ScrollToAssignPage", _m_ScrollToAssignPage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddBannerThings", _m_AddBannerThings);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "scrollRect", _g_get_scrollRect);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "scrollRT", _g_get_scrollRT);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "contentRT", _g_get_contentRT);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseBottonPoint", _g_get_UseBottonPoint);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pointRt", _g_get_pointRt);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "mask", _g_get_mask);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BannerSample", _g_get_BannerSample);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PointBtSample", _g_get_PointBtSample);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "scrollRect", _s_set_scrollRect);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "scrollRT", _s_set_scrollRT);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "contentRT", _s_set_contentRT);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseBottonPoint", _s_set_UseBottonPoint);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pointRt", _s_set_pointRt);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "mask", _s_set_mask);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BannerSample", _s_set_BannerSample);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PointBtSample", _s_set_PointBtSample);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					ScrollPanelWithButtonWidget __cl_gen_ret = new ScrollPanelWithButtonWidget();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ScrollPanelWithButtonWidget constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWidgetType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        WidgetType __cl_gen_ret = __cl_gen_to_be_invoked.GetWidgetType(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEventListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UIEvent eventType;translator.Get(L, 2, out eventType);
                    System.Action<UnityEngine.EventSystems.PointerEventData> onEventHandler = translator.GetDelegate<System.Action<UnityEngine.EventSystems.PointerEventData>>(L, 3);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.AddEventListener( eventType, onEventHandler );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveEventListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UIEvent eventType;translator.Get(L, 2, out eventType);
                    System.Action<UnityEngine.EventSystems.PointerEventData> onEventHandler = translator.GetDelegate<System.Action<UnityEngine.EventSystems.PointerEventData>>(L, 3);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.RemoveEventListener( eventType, onEventHandler );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetContentSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    float sizeValue = (float)LuaAPI.lua_tonumber(L, 2);
                    bool resetPos = LuaAPI.lua_toboolean(L, 3);
                    
                    __cl_gen_to_be_invoked.SetContentSize( sizeValue, resetPos );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    float sizeValue = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    __cl_gen_to_be_invoked.SetContentSize( sizeValue );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to ScrollPanelWithButtonWidget.SetContentSize!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResetPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.ResetPos(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenPageScroll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Action<int>>(L, 3)) 
                {
                    int maxPage = LuaAPI.xlua_tointeger(L, 2);
                    System.Action<int> scrollPageEndFun = translator.GetDelegate<System.Action<int>>(L, 3);
                    
                    __cl_gen_to_be_invoked.OpenPageScroll( maxPage, scrollPageEndFun );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int maxPage = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.OpenPageScroll( maxPage );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to ScrollPanelWithButtonWidget.OpenPageScroll!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClosePageScroll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.ClosePageScroll(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScrollToAssignPage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int pageIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.ScrollToAssignPage( pageIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddBannerThings(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& translator.Assignable<UnityEngine.Sprite>(L, 2)) 
                {
                    UnityEngine.Sprite _bannerSpr = (UnityEngine.Sprite)translator.GetObject(L, 2, typeof(UnityEngine.Sprite));
                    
                    __cl_gen_to_be_invoked.AddBannerThings( _bannerSpr );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 1) 
                {
                    
                    __cl_gen_to_be_invoked.AddBannerThings(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to ScrollPanelWithButtonWidget.AddBannerThings!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scrollRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.scrollRect);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_scrollRT(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.scrollRT);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_contentRT(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.contentRT);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseBottonPoint(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.UseBottonPoint);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pointRt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.pointRt);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_mask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.mask);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BannerSample(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.BannerSample);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PointBtSample(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.PointBtSample);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_scrollRect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.scrollRect = (UnityEngine.UI.ScrollRect)translator.GetObject(L, 2, typeof(UnityEngine.UI.ScrollRect));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_scrollRT(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.scrollRT = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_contentRT(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.contentRT = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseBottonPoint(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.UseBottonPoint = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pointRt(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.pointRt = (UnityEngine.RectTransform)translator.GetObject(L, 2, typeof(UnityEngine.RectTransform));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_mask(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.mask = (UnityEngine.UI.RectMask2D)translator.GetObject(L, 2, typeof(UnityEngine.UI.RectMask2D));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BannerSample(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.BannerSample = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PointBtSample(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ScrollPanelWithButtonWidget __cl_gen_to_be_invoked = (ScrollPanelWithButtonWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.PointBtSample = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
