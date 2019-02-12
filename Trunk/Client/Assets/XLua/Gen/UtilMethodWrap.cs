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
    public class UtilMethodWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UtilMethod);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 18, 5, 5);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "IsUnityEditor", _m_IsUnityEditor_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "isAndroid", _m_isAndroid_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "isIOS", _m_isIOS_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsSuperVersion", _m_IsSuperVersion_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetCurVersion", _m_GetCurVersion_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetPersistentDataRootPath", _m_GetPersistentDataRootPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CleanCache", _m_CleanCache_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ConnectStrs", _m_ConnectStrs_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Md5Sum", _m_Md5Sum_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetDeltaDataTime", _m_GetDeltaDataTime_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ParseFromFloat", _m_ParseFromFloat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetScreenWidthHeight", _m_GetScreenWidthHeight_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetQrCode", _m_GetQrCode_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMD5HashFromString", _m_GetMD5HashFromString_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetSendGMResult", _m_SetSendGMResult_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PhysicsRaycast", _m_PhysicsRaycast_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SwitchScreenOrientation", _m_SwitchScreenOrientation_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "isIncludeGame", _g_get_isIncludeGame);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "isSkipUpdate", _g_get_isSkipUpdate);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "isTestServer", _g_get_isTestServer);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "isBetaCDN", _g_get_isBetaCDN);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "channel", _g_get_channel);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "isIncludeGame", _s_set_isIncludeGame);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "isSkipUpdate", _s_set_isSkipUpdate);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "isTestServer", _s_set_isTestServer);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "isBetaCDN", _s_set_isBetaCDN);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "channel", _s_set_channel);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UtilMethod __cl_gen_ret = new UtilMethod();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UtilMethod constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsUnityEditor_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        bool __cl_gen_ret = UtilMethod.IsUnityEditor(  );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_isAndroid_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        bool __cl_gen_ret = UtilMethod.isAndroid(  );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_isIOS_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        bool __cl_gen_ret = UtilMethod.isIOS(  );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsSuperVersion_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        bool __cl_gen_ret = UtilMethod.IsSuperVersion(  );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurVersion_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        string __cl_gen_ret = UtilMethod.GetCurVersion(  );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPersistentDataRootPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        string __cl_gen_ret = UtilMethod.GetPersistentDataRootPath(  );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CleanCache_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string path = LuaAPI.lua_tostring(L, 1);
                    
                    UtilMethod.CleanCache( path );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ConnectStrs_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string[] strs = translator.GetParams<string>(L, 1);
                    
                        string __cl_gen_ret = UtilMethod.ConnectStrs( strs );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Md5Sum_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string input = LuaAPI.lua_tostring(L, 1);
                    
                        string __cl_gen_ret = UtilMethod.Md5Sum( input );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDeltaDataTime_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.DateTime left;translator.Get(L, 1, out left);
                    System.DateTime right;translator.Get(L, 2, out right);
                    
                        System.TimeSpan __cl_gen_ret = UtilMethod.GetDeltaDataTime( left, right );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ParseFromFloat_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float curTime = (float)LuaAPI.lua_tonumber(L, 1);
                    
                        System.TimeSpan __cl_gen_ret = UtilMethod.ParseFromFloat( curTime );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetScreenWidthHeight_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        UnityEngine.Vector2 __cl_gen_ret = UtilMethod.GetScreenWidthHeight(  );
                        translator.PushUnityEngineVector2(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetQrCode_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    string text = LuaAPI.lua_tostring(L, 1);
                    int size = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.Texture2D __cl_gen_ret = UtilMethod.GetQrCode( text, size );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string text = LuaAPI.lua_tostring(L, 1);
                    
                        UnityEngine.Texture2D __cl_gen_ret = UtilMethod.GetQrCode( text );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UtilMethod.GetQrCode!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMD5HashFromString_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string str = LuaAPI.lua_tostring(L, 1);
                    
                        string __cl_gen_ret = UtilMethod.GetMD5HashFromString( str );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSendGMResult_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool isSucceed = LuaAPI.lua_toboolean(L, 1);
                    
                    UtilMethod.SetSendGMResult( isSucceed );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PhysicsRaycast_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Ray ray;translator.Get(L, 1, out ray);
                    UnityEngine.RaycastHit hit;
                    
                        bool __cl_gen_ret = UtilMethod.PhysicsRaycast( ray, out hit );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    translator.Push(L, hit);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SwitchScreenOrientation_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool isHorizontal = LuaAPI.lua_toboolean(L, 1);
                    
                    UtilMethod.SwitchScreenOrientation( isHorizontal );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isIncludeGame(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UtilMethod.isIncludeGame);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isSkipUpdate(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UtilMethod.isSkipUpdate);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isTestServer(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UtilMethod.isTestServer);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isBetaCDN(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, UtilMethod.isBetaCDN);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_channel(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, UtilMethod.channel);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isIncludeGame(RealStatePtr L)
        {
		    try {
                
			    UtilMethod.isIncludeGame = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isSkipUpdate(RealStatePtr L)
        {
		    try {
                
			    UtilMethod.isSkipUpdate = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isTestServer(RealStatePtr L)
        {
		    try {
                
			    UtilMethod.isTestServer = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isBetaCDN(RealStatePtr L)
        {
		    try {
                
			    UtilMethod.isBetaCDN = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_channel(RealStatePtr L)
        {
		    try {
                
			    UtilMethod.channel = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
