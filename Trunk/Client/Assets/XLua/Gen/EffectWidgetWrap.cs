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
    public class EffectWidgetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(EffectWidget);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 4, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddEventListener", _m_AddEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveEventListener", _m_RemoveEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetWidgetType", _m_GetWidgetType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Play", _m_Play);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PreLoad", _m_PreLoad);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetTrack", _m_SetTrack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Stop", _m_Stop);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "maskImg", _g_get_maskImg);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "packName", _g_get_packName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "effectName", _g_get_effectName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "sortingOrderOffset", _g_get_sortingOrderOffset);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "maskImg", _s_set_maskImg);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "packName", _s_set_packName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "effectName", _s_set_effectName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "sortingOrderOffset", _s_set_sortingOrderOffset);
            
			
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
					
					EffectWidget __cl_gen_ret = new EffectWidget();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to EffectWidget constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEventListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
            
            
                
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
            
            
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_Play(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.Play(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PreLoad(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.PreLoad(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetTrack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count >= 3&& translator.Assignable<EffectWidget.TrackType>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 4) || LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4))) 
                {
                    EffectWidget.TrackType type;translator.Get(L, 2, out type);
                    float runTime = (float)LuaAPI.lua_tonumber(L, 3);
                    float[] valueArr = translator.GetParams<float>(L, 4);
                    
                    __cl_gen_to_be_invoked.SetTrack( type, runTime, valueArr );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count >= 2&& translator.Assignable<EffectWidget.TrackType>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    EffectWidget.TrackType type;translator.Get(L, 2, out type);
                    float runTime = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    __cl_gen_to_be_invoked.SetTrack( type, runTime );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count >= 1&& translator.Assignable<EffectWidget.TrackType>(L, 2)) 
                {
                    EffectWidget.TrackType type;translator.Get(L, 2, out type);
                    
                    __cl_gen_to_be_invoked.SetTrack( type );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to EffectWidget.SetTrack!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Stop(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.Stop(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maskImg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.maskImg);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_packName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.packName);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_effectName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.effectName);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_sortingOrderOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.sortingOrderOffset);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maskImg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.maskImg = (UnityEngine.UI.Image)translator.GetObject(L, 2, typeof(UnityEngine.UI.Image));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_packName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.packName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_effectName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.effectName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_sortingOrderOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EffectWidget __cl_gen_to_be_invoked = (EffectWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.sortingOrderOffset = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
