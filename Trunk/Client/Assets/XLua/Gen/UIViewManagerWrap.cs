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
    public class UIViewManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UIViewManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 16, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateUILayerPanel", _m_CreateUILayerPanel);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SwitchScreenOrientation", _m_SwitchScreenOrientation);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetCanvasMatch", _m_SetCanvasMatch);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RegisterStackButtomView", _m_RegisterStackButtomView);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RegisterView", _m_RegisterView);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Preload", _m_Preload);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Open", _m_Open);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Close", _m_Close);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseAllView", _m_CloseAllView);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SaveStackAndCloseAllView", _m_SaveStackAndCloseAllView);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseAllViewAndRevertStack", _m_CloseAllViewAndRevertStack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MoveZoomAllView", _m_MoveZoomAllView);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ZoomAllView", _m_ZoomAllView);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DestroyView", _m_DestroyView);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DestroyUIRes", _m_DestroyUIRes);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseStackTopView", _m_CloseStackTopView);
			
			
			
			
			
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
					
					UIViewManager __cl_gen_ret = new UIViewManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UIViewManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateUILayerPanel(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int layerIndex = LuaAPI.xlua_tointeger(L, 2);
                    string name = LuaAPI.lua_tostring(L, 3);
                    
                        UnityEngine.GameObject __cl_gen_ret = __cl_gen_to_be_invoked.CreateUILayerPanel( layerIndex, name );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SwitchScreenOrientation(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    bool isHorizontal = LuaAPI.lua_toboolean(L, 2);
                    
                    __cl_gen_to_be_invoked.SwitchScreenOrientation( isHorizontal );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetCanvasMatch(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int uiViewType = LuaAPI.xlua_tointeger(L, 2);
                    float value = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    __cl_gen_to_be_invoked.SetCanvasMatch( uiViewType, value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterStackButtomView(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int viewEnum = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.RegisterStackButtomView( viewEnum );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterView(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string packageName = LuaAPI.lua_tostring(L, 2);
                    LuaUIView view = (LuaUIView)translator.GetObject(L, 3, typeof(LuaUIView));
                    
                    __cl_gen_to_be_invoked.RegisterView( packageName, view );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Preload(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<System.Action>(L, 3)) 
                {
                    int viewEnum = LuaAPI.xlua_tointeger(L, 2);
                    System.Action preloadCallback = translator.GetDelegate<System.Action>(L, 3);
                    
                    __cl_gen_to_be_invoked.Preload( viewEnum, preloadCallback );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int viewEnum = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.Preload( viewEnum );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UIViewManager.Preload!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Open(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<object>(L, 3)&& translator.Assignable<System.Action>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    int viewEnum = LuaAPI.xlua_tointeger(L, 2);
                    object msg = translator.GetObject(L, 3, typeof(object));
                    System.Action openCallback = translator.GetDelegate<System.Action>(L, 4);
                    bool isPush = LuaAPI.lua_toboolean(L, 5);
                    
                    __cl_gen_to_be_invoked.Open( viewEnum, msg, openCallback, isPush );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<object>(L, 3)&& translator.Assignable<System.Action>(L, 4)) 
                {
                    int viewEnum = LuaAPI.xlua_tointeger(L, 2);
                    object msg = translator.GetObject(L, 3, typeof(object));
                    System.Action openCallback = translator.GetDelegate<System.Action>(L, 4);
                    
                    __cl_gen_to_be_invoked.Open( viewEnum, msg, openCallback );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<object>(L, 3)) 
                {
                    int viewEnum = LuaAPI.xlua_tointeger(L, 2);
                    object msg = translator.GetObject(L, 3, typeof(object));
                    
                    __cl_gen_to_be_invoked.Open( viewEnum, msg );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int viewEnum = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.Open( viewEnum );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UIViewManager.Open!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Close(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    int viewEnum = LuaAPI.xlua_tointeger(L, 2);
                    bool isDel = LuaAPI.lua_toboolean(L, 3);
                    bool isAnim = LuaAPI.lua_toboolean(L, 4);
                    
                    __cl_gen_to_be_invoked.Close( viewEnum, isDel, isAnim );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    int viewEnum = LuaAPI.xlua_tointeger(L, 2);
                    bool isDel = LuaAPI.lua_toboolean(L, 3);
                    
                    __cl_gen_to_be_invoked.Close( viewEnum, isDel );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    int viewEnum = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.Close( viewEnum );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UIViewManager.Close!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseAllView(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.CloseAllView(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SaveStackAndCloseAllView(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.SaveStackAndCloseAllView(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseAllViewAndRevertStack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.CloseAllViewAndRevertStack(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MoveZoomAllView(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 pivotV2;translator.Get(L, 2, out pivotV2);
                    UnityEngine.Vector3 value;translator.Get(L, 3, out value);
                    float time = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    __cl_gen_to_be_invoked.MoveZoomAllView( pivotV2, value, time );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ZoomAllView(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 pivotV2;translator.Get(L, 2, out pivotV2);
                    UnityEngine.Vector3 value;translator.Get(L, 3, out value);
                    
                    __cl_gen_to_be_invoked.ZoomAllView( pivotV2, value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestroyView(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int viewEnum = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.DestroyView( viewEnum );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestroyUIRes(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string packageName = LuaAPI.lua_tostring(L, 2);
                    string relativePath = LuaAPI.lua_tostring(L, 3);
                    
                    __cl_gen_to_be_invoked.DestroyUIRes( packageName, relativePath );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseStackTopView(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UIViewManager __cl_gen_to_be_invoked = (UIViewManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.CloseStackTopView(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
