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
    public class ObjectPoolManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ObjectPoolManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 3, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreatePoolObject", _m_CreatePoolObject);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Progress", _g_get_Progress);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadingEvent", _g_get_LoadingEvent);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LoadedEvent", _g_get_LoadedEvent);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "LoadingEvent", _s_set_LoadingEvent);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LoadedEvent", _s_set_LoadedEvent);
            
			
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
					
					ObjectPoolManager __cl_gen_ret = new ObjectPoolManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ObjectPoolManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreatePoolObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                ObjectPoolManager __cl_gen_to_be_invoked = (ObjectPoolManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string packName = LuaAPI.lua_tostring(L, 2);
                    string resName = LuaAPI.lua_tostring(L, 3);
                    System.Action<UnityEngine.Object> onLoadend = translator.GetDelegate<System.Action<UnityEngine.Object>>(L, 4);
                    
                    __cl_gen_to_be_invoked.CreatePoolObject( packName, resName, onLoadend );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Progress(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ObjectPoolManager __cl_gen_to_be_invoked = (ObjectPoolManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.Progress);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadingEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ObjectPoolManager __cl_gen_to_be_invoked = (ObjectPoolManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.LoadingEvent);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LoadedEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ObjectPoolManager __cl_gen_to_be_invoked = (ObjectPoolManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.LoadedEvent);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LoadingEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ObjectPoolManager __cl_gen_to_be_invoked = (ObjectPoolManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.LoadingEvent = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LoadedEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ObjectPoolManager __cl_gen_to_be_invoked = (ObjectPoolManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.LoadedEvent = translator.GetDelegate<System.Action>(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
