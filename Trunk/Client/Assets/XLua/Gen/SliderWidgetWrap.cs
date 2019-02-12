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
    public class SliderWidgetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(SliderWidget);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 6, 6);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddEventListener", _m_AddEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveEventListener", _m_RemoveEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetWidgetType", _m_GetWidgetType);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "value", _g_get_value);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "handleSign", _g_get_handleSign);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "slider", _g_get_slider);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "bgImg", _g_get_bgImg);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "fillImg", _g_get_fillImg);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "handleRange", _g_get_handleRange);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "value", _s_set_value);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "handleSign", _s_set_handleSign);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "slider", _s_set_slider);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "bgImg", _s_set_bgImg);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "fillImg", _s_set_fillImg);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "handleRange", _s_set_handleRange);
            
			
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
					
					SliderWidget __cl_gen_ret = new SliderWidget();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to SliderWidget constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEventListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_GetWidgetType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _g_get_value(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.value);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_handleSign(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.handleSign);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_slider(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.slider);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_bgImg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.bgImg);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_fillImg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.fillImg);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_handleRange(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, __cl_gen_to_be_invoked.handleRange);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_value(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.value = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_handleSign(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.handleSign = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_slider(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.slider = (UnityEngine.UI.Slider)translator.GetObject(L, 2, typeof(UnityEngine.UI.Slider));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_bgImg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.bgImg = (UnityEngine.UI.Image)translator.GetObject(L, 2, typeof(UnityEngine.UI.Image));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_fillImg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.fillImg = (UnityEngine.UI.Image)translator.GetObject(L, 2, typeof(UnityEngine.UI.Image));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_handleRange(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SliderWidget __cl_gen_to_be_invoked = (SliderWidget)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.handleRange = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
