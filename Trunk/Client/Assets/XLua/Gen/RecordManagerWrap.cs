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
    public class RecordManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(RecordManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 8, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Init", _m_Init_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoginRecord", _m_LoginRecord_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StartRecord", _m_StartRecord_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StopRecord", _m_StopRecord_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StartPlayRecordByFilePath", _m_StartPlayRecordByFilePath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StartPlayRecordByUrl", _m_StartPlayRecordByUrl_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StopPlayRecord", _m_StopPlayRecord_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					RecordManager __cl_gen_ret = new RecordManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to RecordManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    RecordManager.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoginRecord_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string nickname = LuaAPI.lua_tostring(L, 1);
                    string uid = LuaAPI.lua_tostring(L, 2);
                    
                    RecordManager.LoginRecord( nickname, uid );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartRecord_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string fileName = LuaAPI.lua_tostring(L, 1);
                    
                    RecordManager.StartRecord( fileName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopRecord_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    RecordManager.StopRecord(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartPlayRecordByFilePath_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string filePath = LuaAPI.lua_tostring(L, 1);
                    
                    RecordManager.StartPlayRecordByFilePath( filePath );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartPlayRecordByUrl_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string url = LuaAPI.lua_tostring(L, 1);
                    
                    RecordManager.StartPlayRecordByUrl( url );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopPlayRecord_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    RecordManager.StopPlayRecord(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
