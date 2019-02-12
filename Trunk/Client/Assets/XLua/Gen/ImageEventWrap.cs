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
    public class ImageEventWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ImageEvent);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 0, 0);
			
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnCollision2DEnterAct", _e_OnCollision2DEnterAct);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnTrigger2DEnterAct", _e_OnTrigger2DEnterAct);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnCollision2DEnterActPro", _e_OnCollision2DEnterActPro);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnTriggerEnterActPro", _e_OnTriggerEnterActPro);
			
			
			
			
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
					
					ImageEvent __cl_gen_ret = new ImageEvent();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ImageEvent constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        
        
		
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnCollision2DEnterAct(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int __gen_param_count = LuaAPI.lua_gettop(L);
			ImageEvent __cl_gen_to_be_invoked = (ImageEvent)translator.FastGetCSObj(L, 1);
                System.Action<UnityEngine.Collision2D> __gen_delegate = translator.GetDelegate<System.Action<UnityEngine.Collision2D>>(L, 3);
                if (__gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need System.Action<UnityEngine.Collision2D>!");
                }
				
				if (__gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						__cl_gen_to_be_invoked.OnCollision2DEnterAct += __gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						__cl_gen_to_be_invoked.OnCollision2DEnterAct -= __gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to ImageEvent.OnCollision2DEnterAct!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnTrigger2DEnterAct(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int __gen_param_count = LuaAPI.lua_gettop(L);
			ImageEvent __cl_gen_to_be_invoked = (ImageEvent)translator.FastGetCSObj(L, 1);
                System.Action<UnityEngine.Collider2D> __gen_delegate = translator.GetDelegate<System.Action<UnityEngine.Collider2D>>(L, 3);
                if (__gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need System.Action<UnityEngine.Collider2D>!");
                }
				
				if (__gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						__cl_gen_to_be_invoked.OnTrigger2DEnterAct += __gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						__cl_gen_to_be_invoked.OnTrigger2DEnterAct -= __gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to ImageEvent.OnTrigger2DEnterAct!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnCollision2DEnterActPro(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int __gen_param_count = LuaAPI.lua_gettop(L);
			ImageEvent __cl_gen_to_be_invoked = (ImageEvent)translator.FastGetCSObj(L, 1);
                System.Action<int, int, float, float> __gen_delegate = translator.GetDelegate<System.Action<int, int, float, float>>(L, 3);
                if (__gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need System.Action<int, int, float, float>!");
                }
				
				if (__gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						__cl_gen_to_be_invoked.OnCollision2DEnterActPro += __gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						__cl_gen_to_be_invoked.OnCollision2DEnterActPro -= __gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to ImageEvent.OnCollision2DEnterActPro!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnTriggerEnterActPro(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int __gen_param_count = LuaAPI.lua_gettop(L);
			ImageEvent __cl_gen_to_be_invoked = (ImageEvent)translator.FastGetCSObj(L, 1);
                System.Action<int, int> __gen_delegate = translator.GetDelegate<System.Action<int, int>>(L, 3);
                if (__gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need System.Action<int, int>!");
                }
				
				if (__gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						__cl_gen_to_be_invoked.OnTriggerEnterActPro += __gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						__cl_gen_to_be_invoked.OnTriggerEnterActPro -= __gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to ImageEvent.OnTriggerEnterActPro!");
            return 0;
        }
        
		
		
    }
}
