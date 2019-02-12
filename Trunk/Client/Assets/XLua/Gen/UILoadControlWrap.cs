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
    public class UILoadControlWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UILoadControl);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateUI", _m_CreateUI);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AsyncCreateUI", _m_AsyncCreateUI);
			
			
			
			
			
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
					
					UILoadControl __cl_gen_ret = new UILoadControl();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UILoadControl constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UILoadControl __cl_gen_to_be_invoked = (UILoadControl)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<LuaUIView>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)&& translator.Assignable<LuaPreloadOrder>(L, 6)) 
                {
                    string packName = LuaAPI.lua_tostring(L, 2);
                    string name = LuaAPI.lua_tostring(L, 3);
                    LuaUIView uiView = (LuaUIView)translator.GetObject(L, 4, typeof(LuaUIView));
                    bool isInstantiation = LuaAPI.lua_toboolean(L, 5);
                    LuaPreloadOrder order = (LuaPreloadOrder)translator.GetObject(L, 6, typeof(LuaPreloadOrder));
                    
                    __cl_gen_to_be_invoked.CreateUI( packName, name, uiView, isInstantiation, order );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<LuaUIView>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    string packName = LuaAPI.lua_tostring(L, 2);
                    string name = LuaAPI.lua_tostring(L, 3);
                    LuaUIView uiView = (LuaUIView)translator.GetObject(L, 4, typeof(LuaUIView));
                    bool isInstantiation = LuaAPI.lua_toboolean(L, 5);
                    
                    __cl_gen_to_be_invoked.CreateUI( packName, name, uiView, isInstantiation );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<LuaUIView>(L, 4)) 
                {
                    string packName = LuaAPI.lua_tostring(L, 2);
                    string name = LuaAPI.lua_tostring(L, 3);
                    LuaUIView uiView = (LuaUIView)translator.GetObject(L, 4, typeof(LuaUIView));
                    
                    __cl_gen_to_be_invoked.CreateUI( packName, name, uiView );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UILoadControl.CreateUI!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AsyncCreateUI(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UILoadControl __cl_gen_to_be_invoked = (UILoadControl)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<LuaUIView>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)&& translator.Assignable<LuaPreloadOrder>(L, 6)) 
                {
                    string packageName = LuaAPI.lua_tostring(L, 2);
                    string name = LuaAPI.lua_tostring(L, 3);
                    LuaUIView uiView = (LuaUIView)translator.GetObject(L, 4, typeof(LuaUIView));
                    bool isInstantiation = LuaAPI.lua_toboolean(L, 5);
                    LuaPreloadOrder order = (LuaPreloadOrder)translator.GetObject(L, 6, typeof(LuaPreloadOrder));
                    
                        System.Collections.IEnumerator __cl_gen_ret = __cl_gen_to_be_invoked.AsyncCreateUI( packageName, name, uiView, isInstantiation, order );
                        translator.PushAny(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<LuaUIView>(L, 4)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 5)) 
                {
                    string packageName = LuaAPI.lua_tostring(L, 2);
                    string name = LuaAPI.lua_tostring(L, 3);
                    LuaUIView uiView = (LuaUIView)translator.GetObject(L, 4, typeof(LuaUIView));
                    bool isInstantiation = LuaAPI.lua_toboolean(L, 5);
                    
                        System.Collections.IEnumerator __cl_gen_ret = __cl_gen_to_be_invoked.AsyncCreateUI( packageName, name, uiView, isInstantiation );
                        translator.PushAny(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UILoadControl.AsyncCreateUI!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
