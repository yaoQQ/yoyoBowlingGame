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
    public class PhysicGameManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(PhysicGameManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 10, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "addPhysic", _m_addPhysic);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "addPhysicTrigger", _m_addPhysicTrigger);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "addEnableFun", _m_addEnableFun);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "addUpdateFun", _m_addUpdateFun);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "removeUpdateFun", _m_removeUpdateFun);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "addLateUpdateFun", _m_addLateUpdateFun);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LateUpdate", _m_LateUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "clear", _m_clear);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "getEnumNumber", _m_getEnumNumber);
			
			
			
			
			
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
					
					PhysicGameManager __cl_gen_ret = new PhysicGameManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to PhysicGameManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_addPhysic(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PhysicGameManager __cl_gen_to_be_invoked = (PhysicGameManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject obj = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    OnCollisionCallBack enter = translator.GetDelegate<OnCollisionCallBack>(L, 3);
                    OnCollisionCallBack exit = translator.GetDelegate<OnCollisionCallBack>(L, 4);
                    OnCollisionCallBack stay = translator.GetDelegate<OnCollisionCallBack>(L, 5);
                    
                    __cl_gen_to_be_invoked.addPhysic( obj, enter, exit, stay );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_addPhysicTrigger(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PhysicGameManager __cl_gen_to_be_invoked = (PhysicGameManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject obj = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    OnTriggerCallBack triggerEnter = translator.GetDelegate<OnTriggerCallBack>(L, 3);
                    OnTriggerCallBack triggerStay = translator.GetDelegate<OnTriggerCallBack>(L, 4);
                    OnTriggerCallBack triggerExit = translator.GetDelegate<OnTriggerCallBack>(L, 5);
                    
                    __cl_gen_to_be_invoked.addPhysicTrigger( obj, triggerEnter, triggerStay, triggerExit );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_addEnableFun(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PhysicGameManager __cl_gen_to_be_invoked = (PhysicGameManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.GameObject obj = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
                    System.Action OnEnableFun = translator.GetDelegate<System.Action>(L, 3);
                    System.Action OnDisableFun = translator.GetDelegate<System.Action>(L, 4);
                    OnCollisionCallBack stay = translator.GetDelegate<OnCollisionCallBack>(L, 5);
                    
                    __cl_gen_to_be_invoked.addEnableFun( obj, OnEnableFun, OnDisableFun, stay );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_addUpdateFun(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PhysicGameManager __cl_gen_to_be_invoked = (PhysicGameManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action luaFun = translator.GetDelegate<System.Action>(L, 2);
                    
                    __cl_gen_to_be_invoked.addUpdateFun( luaFun );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_removeUpdateFun(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PhysicGameManager __cl_gen_to_be_invoked = (PhysicGameManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action luaFun = translator.GetDelegate<System.Action>(L, 2);
                    
                    __cl_gen_to_be_invoked.removeUpdateFun( luaFun );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_addLateUpdateFun(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PhysicGameManager __cl_gen_to_be_invoked = (PhysicGameManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action luaFun = translator.GetDelegate<System.Action>(L, 2);
                    
                    __cl_gen_to_be_invoked.addLateUpdateFun( luaFun );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PhysicGameManager __cl_gen_to_be_invoked = (PhysicGameManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LateUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PhysicGameManager __cl_gen_to_be_invoked = (PhysicGameManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.LateUpdate(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_clear(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PhysicGameManager __cl_gen_to_be_invoked = (PhysicGameManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.clear(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getEnumNumber(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                PhysicGameManager __cl_gen_to_be_invoked = (PhysicGameManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Enum targetEnum = (System.Enum)translator.GetObject(L, 2, typeof(System.Enum));
                    
                        int __cl_gen_ret = __cl_gen_to_be_invoked.getEnumNumber( targetEnum );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
