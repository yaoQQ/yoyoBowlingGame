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
    public class ResDownLoadContollerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ResDownLoadContoller);
			Utils.BeginObjectRegister(type, L, translator, 0, 11, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DownLoadResPack", _m_DownLoadResPack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CancelDownLoadResPack", _m_CancelDownLoadResPack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTotalProgress", _m_GetTotalProgress);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetTotalSize", _m_GetTotalSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearCompleter", _m_ClearCompleter);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnApplicationQuit", _m_OnApplicationQuit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddResRecordDown", _m_AddResRecordDown);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetUpdateRecordsProgress", _m_GetUpdateRecordsProgress);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResRecordDownEndSign", _m_ResRecordDownEndSign);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetFailComparisonRecordList", _m_GetFailComparisonRecordList);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearRecordDown", _m_ClearRecordDown);
			
			
			
			
			
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
					
					ResDownLoadContoller __cl_gen_ret = new ResDownLoadContoller();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ResDownLoadContoller constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DownLoadResPack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ResDownLoadContoller __cl_gen_to_be_invoked = (ResDownLoadContoller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    ResVersions.ResPack resPack = (ResVersions.ResPack)translator.GetObject(L, 2, typeof(ResVersions.ResPack));
                    
                    __cl_gen_to_be_invoked.DownLoadResPack( resPack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CancelDownLoadResPack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ResDownLoadContoller __cl_gen_to_be_invoked = (ResDownLoadContoller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.CancelDownLoadResPack(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTotalProgress(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ResDownLoadContoller __cl_gen_to_be_invoked = (ResDownLoadContoller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        float __cl_gen_ret = __cl_gen_to_be_invoked.GetTotalProgress(  );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTotalSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ResDownLoadContoller __cl_gen_to_be_invoked = (ResDownLoadContoller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        long __cl_gen_ret = __cl_gen_to_be_invoked.GetTotalSize(  );
                        LuaAPI.lua_pushint64(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearCompleter(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ResDownLoadContoller __cl_gen_to_be_invoked = (ResDownLoadContoller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.ClearCompleter(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnApplicationQuit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ResDownLoadContoller __cl_gen_to_be_invoked = (ResDownLoadContoller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.OnApplicationQuit(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddResRecordDown(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ResDownLoadContoller __cl_gen_to_be_invoked = (ResDownLoadContoller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    ResVersions.ComparisonRecord comparisonRecord = (ResVersions.ComparisonRecord)translator.GetObject(L, 2, typeof(ResVersions.ComparisonRecord));
                    
                    __cl_gen_to_be_invoked.AddResRecordDown( comparisonRecord );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUpdateRecordsProgress(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ResDownLoadContoller __cl_gen_to_be_invoked = (ResDownLoadContoller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        float __cl_gen_ret = __cl_gen_to_be_invoked.GetUpdateRecordsProgress(  );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResRecordDownEndSign(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ResDownLoadContoller __cl_gen_to_be_invoked = (ResDownLoadContoller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.ResRecordDownEndSign(  );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFailComparisonRecordList(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ResDownLoadContoller __cl_gen_to_be_invoked = (ResDownLoadContoller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        ResVersions.ComparisonRecord[] __cl_gen_ret = __cl_gen_to_be_invoked.GetFailComparisonRecordList(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearRecordDown(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ResDownLoadContoller __cl_gen_to_be_invoked = (ResDownLoadContoller)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.ClearRecordDown(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
