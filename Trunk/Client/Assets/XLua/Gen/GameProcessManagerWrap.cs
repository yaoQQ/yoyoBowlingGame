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
    public class GameProcessManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GameProcessManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 16, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "RegisterPackageName", _m_RegisterPackageName_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegisterCheckDownloadFunc", _m_RegisterCheckDownloadFunc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegisterStartDownloadFunc", _m_RegisterStartDownloadFunc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegisterCancelDownloadFunc", _m_RegisterCancelDownloadFunc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegisterStartGameFunc", _m_RegisterStartGameFunc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RegisterDestroyGameFunc", _m_RegisterDestroyGameFunc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CheckDownload", _m_CheckDownload_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StartDownload", _m_StartDownload_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CancelDownload", _m_CancelDownload_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateDownloadProgress", _m_UpdateDownloadProgress_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StartGame", _m_StartGame_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ExitGame", _m_ExitGame_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DestroyGame", _m_DestroyGame_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OpenShop", _m_OpenShop_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OpenShopLands", _m_OpenShopLands_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					GameProcessManager __cl_gen_ret = new GameProcessManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GameProcessManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterPackageName_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int gameID = LuaAPI.xlua_tointeger(L, 1);
                    string packageName = LuaAPI.lua_tostring(L, 2);
                    
                    GameProcessManager.RegisterPackageName( gameID, packageName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterCheckDownloadFunc_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int gameID = LuaAPI.xlua_tointeger(L, 1);
                    CheckGameDownloadFunc checkGameDownloadFunc = translator.GetDelegate<CheckGameDownloadFunc>(L, 2);
                    
                    GameProcessManager.RegisterCheckDownloadFunc( gameID, checkGameDownloadFunc );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterStartDownloadFunc_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int gameID = LuaAPI.xlua_tointeger(L, 1);
                    System.Action startGameDownloadFunc = translator.GetDelegate<System.Action>(L, 2);
                    
                    GameProcessManager.RegisterStartDownloadFunc( gameID, startGameDownloadFunc );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterCancelDownloadFunc_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int gameID = LuaAPI.xlua_tointeger(L, 1);
                    System.Action cancelGameDownloadFunc = translator.GetDelegate<System.Action>(L, 2);
                    
                    GameProcessManager.RegisterCancelDownloadFunc( gameID, cancelGameDownloadFunc );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterStartGameFunc_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int gameID = LuaAPI.xlua_tointeger(L, 1);
                    System.Action<object> startGameFunc = translator.GetDelegate<System.Action<object>>(L, 2);
                    
                    GameProcessManager.RegisterStartGameFunc( gameID, startGameFunc );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterDestroyGameFunc_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int gameID = LuaAPI.xlua_tointeger(L, 1);
                    System.Action destroyGameFunc = translator.GetDelegate<System.Action>(L, 2);
                    
                    GameProcessManager.RegisterDestroyGameFunc( gameID, destroyGameFunc );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckDownload_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int gameID = LuaAPI.xlua_tointeger(L, 1);
                    System.Action<uint> checkCallback = translator.GetDelegate<System.Action<uint>>(L, 2);
                    
                    GameProcessManager.CheckDownload( gameID, checkCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartDownload_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int gameID = LuaAPI.xlua_tointeger(L, 1);
                    
                    GameProcessManager.StartDownload( gameID );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CancelDownload_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int gameID = LuaAPI.xlua_tointeger(L, 1);
                    
                    GameProcessManager.CancelDownload( gameID );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateDownloadProgress_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int gameID = LuaAPI.xlua_tointeger(L, 1);
                    float value = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    GameProcessManager.UpdateDownloadProgress( gameID, value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartGame_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int gameID = LuaAPI.xlua_tointeger(L, 1);
                    object param = translator.GetObject(L, 2, typeof(object));
                    
                    GameProcessManager.StartGame( gameID, param );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ExitGame_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int gameID = LuaAPI.xlua_tointeger(L, 1);
                    bool isNeedSwitchScene = LuaAPI.lua_toboolean(L, 2);
                    
                    GameProcessManager.ExitGame( gameID, isNeedSwitchScene );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestroyGame_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int gameID = LuaAPI.xlua_tointeger(L, 1);
                    
                    GameProcessManager.DestroyGame( gameID );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenShop_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int shopType = LuaAPI.xlua_tointeger(L, 1);
                    
                    GameProcessManager.OpenShop( shopType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenShopLands_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int shopType = LuaAPI.xlua_tointeger(L, 1);
                    
                    GameProcessManager.OpenShopLands( shopType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
