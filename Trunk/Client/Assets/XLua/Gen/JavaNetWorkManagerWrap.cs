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
    public class JavaNetWorkManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(JavaNetWorkManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 2, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RegisterEventHandler", _m_RegisterEventHandler);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveEventHandler", _m_RemoveEventHandler);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "accept", _m_accept);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InvokeCallBack", _m_InvokeCallBack);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ContainsKey", _m_ContainsKey);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Count", _g_get_Count);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "m_jsonConnect", _g_get_m_jsonConnect);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "m_jsonConnect", _s_set_m_jsonConnect);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 0);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Instance", _g_get_Instance);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					JavaNetWorkManager __cl_gen_ret = new JavaNetWorkManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to JavaNetWorkManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RegisterEventHandler(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                JavaNetWorkManager __cl_gen_to_be_invoked = (JavaNetWorkManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string code = LuaAPI.lua_tostring(L, 2);
                    JavadataMessageReceive handler = translator.GetDelegate<JavadataMessageReceive>(L, 3);
                    
                    __cl_gen_to_be_invoked.RegisterEventHandler( code, handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveEventHandler(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                JavaNetWorkManager __cl_gen_to_be_invoked = (JavaNetWorkManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string code = LuaAPI.lua_tostring(L, 2);
                    JavadataMessageReceive handler = translator.GetDelegate<JavadataMessageReceive>(L, 3);
                    
                    __cl_gen_to_be_invoked.RemoveEventHandler( code, handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_accept(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                JavaNetWorkManager __cl_gen_to_be_invoked = (JavaNetWorkManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    byte[] data = LuaAPI.lua_tobytes(L, 2);
                    
                    __cl_gen_to_be_invoked.accept( data );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InvokeCallBack(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                JavaNetWorkManager __cl_gen_to_be_invoked = (JavaNetWorkManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string protoID = LuaAPI.lua_tostring(L, 2);
                    string jsonData = LuaAPI.lua_tostring(L, 3);
                    
                    __cl_gen_to_be_invoked.InvokeCallBack( protoID, jsonData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ContainsKey(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                JavaNetWorkManager __cl_gen_to_be_invoked = (JavaNetWorkManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string code = LuaAPI.lua_tostring(L, 2);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.ContainsKey( code );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, JavaNetWorkManager.Instance);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Count(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                JavaNetWorkManager __cl_gen_to_be_invoked = (JavaNetWorkManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.Count);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_jsonConnect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                JavaNetWorkManager __cl_gen_to_be_invoked = (JavaNetWorkManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.m_jsonConnect);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_jsonConnect(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                JavaNetWorkManager __cl_gen_to_be_invoked = (JavaNetWorkManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.m_jsonConnect = (JavadataConnect)translator.GetObject(L, 2, typeof(JavadataConnect));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
