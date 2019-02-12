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
    public class LoadingBarControllerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(LoadingBarController);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 9, 1, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "SetContainer", _m_SetContainer_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Show", _m_Show_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Hide", _m_Hide_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetLoadContent", _m_SetLoadContent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ShowProgressWindow", _m_ShowProgressWindow_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "HideProgressWindow", _m_HideProgressWindow_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetProgress", _m_SetProgress_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetVersions", _m_SetVersions_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "isAutoClose", _g_get_isAutoClose);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "isAutoClose", _s_set_isAutoClose);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					LoadingBarController __cl_gen_ret = new LoadingBarController();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to LoadingBarController constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetContainer_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject container = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    
                    LoadingBarController.SetContainer( container );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Show_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LoadingBarController.Show(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Hide_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LoadingBarController.Hide(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLoadContent_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string content = LuaAPI.lua_tostring(L, 1);
                    
                    LoadingBarController.SetLoadContent( content );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowProgressWindow_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LoadingBarController.ShowProgressWindow(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HideProgressWindow_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LoadingBarController.HideProgressWindow(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetProgress_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    float value = (float)LuaAPI.lua_tonumber(L, 1);
                    int frame = LuaAPI.xlua_tointeger(L, 2);
                    
                    LoadingBarController.SetProgress( value, frame );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    float value = (float)LuaAPI.lua_tonumber(L, 1);
                    
                    LoadingBarController.SetProgress( value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LoadingBarController.SetProgress!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetVersions_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string str = LuaAPI.lua_tostring(L, 1);
                    
                    LoadingBarController.SetVersions( str );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isAutoClose(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, LoadingBarController.isAutoClose);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isAutoClose(RealStatePtr L)
        {
		    try {
                
			    LoadingBarController.isAutoClose = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
