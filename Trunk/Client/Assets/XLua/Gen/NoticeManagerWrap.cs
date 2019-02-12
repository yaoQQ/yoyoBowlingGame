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
    public class NoticeManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(NoticeManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Dispatch", _m_Dispatch);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddNoticeLister", _m_AddNoticeLister);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveNoticeLister", _m_RemoveNoticeLister);
			
			
			
			
			
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
					
					NoticeManager __cl_gen_ret = new NoticeManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to NoticeManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Dispatch(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                NoticeManager __cl_gen_to_be_invoked = (NoticeManager)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string noticeType = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.Dispatch( noticeType );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& translator.Assignable<BaseNotice>(L, 2)) 
                {
                    BaseNotice notice = (BaseNotice)translator.GetObject(L, 2, typeof(BaseNotice));
                    
                    __cl_gen_to_be_invoked.Dispatch( notice );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<object>(L, 3)) 
                {
                    string noticeType = LuaAPI.lua_tostring(L, 2);
                    object notice = translator.GetObject(L, 3, typeof(object));
                    
                    __cl_gen_to_be_invoked.Dispatch( noticeType, notice );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to NoticeManager.Dispatch!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddNoticeLister(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                NoticeManager __cl_gen_to_be_invoked = (NoticeManager)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<OnNoticeLister>(L, 3)&& translator.Assignable<EventPriority>(L, 4)) 
                {
                    string noticeType = LuaAPI.lua_tostring(L, 2);
                    OnNoticeLister onHandler = translator.GetDelegate<OnNoticeLister>(L, 3);
                    EventPriority priority;translator.Get(L, 4, out priority);
                    
                    __cl_gen_to_be_invoked.AddNoticeLister( noticeType, onHandler, priority );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<OnNoticeLister>(L, 3)) 
                {
                    string noticeType = LuaAPI.lua_tostring(L, 2);
                    OnNoticeLister onHandler = translator.GetDelegate<OnNoticeLister>(L, 3);
                    
                    __cl_gen_to_be_invoked.AddNoticeLister( noticeType, onHandler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to NoticeManager.AddNoticeLister!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveNoticeLister(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                NoticeManager __cl_gen_to_be_invoked = (NoticeManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string noticeType = LuaAPI.lua_tostring(L, 2);
                    OnNoticeLister onHandler = translator.GetDelegate<OnNoticeLister>(L, 3);
                    
                    __cl_gen_to_be_invoked.RemoveNoticeLister( noticeType, onHandler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
