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
    public class GlobalTimeManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GlobalTimeManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 12, 5, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SynchronizationTime", _m_SynchronizationTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetDateTimebyTimeStamp", _m_GetDateTimebyTimeStamp);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetInverseTimeSpan", _m_GetInverseTimeSpan);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CheckTimeOut", _m_CheckTimeOut);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CheackTimeArea", _m_CheackTimeArea);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PauseGame", _m_PauseGame);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "resumeGame", _m_resumeGame);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Execute", _m_Execute);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DeferExecute", _m_DeferExecute);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "resetDuplicateTime", _m_resetDuplicateTime);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetDuplicateTime", _m_GetDuplicateTime);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "timerController", _g_get_timerController);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "timeRecorderController", _g_get_timeRecorderController);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CurPauseType", _g_get_CurPauseType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DupStartTime", _g_get_DupStartTime);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "oldTicks", _g_get_oldTicks);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "oldTicks", _s_set_oldTicks);
            
			
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
					
					GlobalTimeManager __cl_gen_ret = new GlobalTimeManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GlobalTimeManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SynchronizationTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string timeStamp = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.SynchronizationTime( timeStamp );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDateTimebyTimeStamp(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    long timeStamp = LuaAPI.lua_toint64(L, 2);
                    
                        System.DateTime __cl_gen_ret = __cl_gen_to_be_invoked.GetDateTimebyTimeStamp( timeStamp );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInverseTimeSpan(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.DateTime targetDT;translator.Get(L, 2, out targetDT);
                    
                        System.TimeSpan __cl_gen_ret = __cl_gen_to_be_invoked.GetInverseTimeSpan( targetDT );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckTimeOut(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.DateTime targetDT;translator.Get(L, 2, out targetDT);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.CheckTimeOut( targetDT );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheackTimeArea(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string startTime = LuaAPI.lua_tostring(L, 2);
                    string endTime = LuaAPI.lua_tostring(L, 3);
                    
                        GlobalTimeManager.TimeAreaEnum __cl_gen_ret = __cl_gen_to_be_invoked.CheackTimeArea( startTime, endTime );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.Init(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PauseGame(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.PauseGame(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_resumeGame(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.resumeGame(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Execute(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        float __cl_gen_ret = __cl_gen_to_be_invoked.Execute(  );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeferExecute(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.DeferExecute(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_resetDuplicateTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.resetDuplicateTime(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDuplicateTime(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        float __cl_gen_ret = __cl_gen_to_be_invoked.GetDuplicateTime(  );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_timerController(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.timerController);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_timeRecorderController(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.timeRecorderController);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CurPauseType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.CurPauseType);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DupStartTime(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.DupStartTime);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_oldTicks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushint64(L, __cl_gen_to_be_invoked.oldTicks);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_oldTicks(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GlobalTimeManager __cl_gen_to_be_invoked = (GlobalTimeManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.oldTicks = LuaAPI.lua_toint64(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
