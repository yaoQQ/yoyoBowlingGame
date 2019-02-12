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
    public class SceneContainerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(SceneContainer);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 4, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Init", _m_Init);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddCell", _m_AddCell);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CheckInitSign", _m_CheckInitSign);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DynamicAddCell", _m_DynamicAddCell);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DynamicRemoveCell", _m_DynamicRemoveCell);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Reset", _m_Reset);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "containerName", _g_get_containerName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellArr", _g_get_cellArr);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "containerInfo", _g_get_containerInfo);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "dynamicCellList", _g_get_dynamicCellList);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "containerName", _s_set_containerName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellArr", _s_set_cellArr);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "containerInfo", _s_set_containerInfo);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "dynamicCellList", _s_set_dynamicCellList);
            
			
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
					
					SceneContainer __cl_gen_ret = new SceneContainer();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to SceneContainer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Init(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int num = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.Init( num );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddCell(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    SceneCell cell = (SceneCell)translator.GetObject(L, 3, typeof(SceneCell));
                    
                    __cl_gen_to_be_invoked.AddCell( index, cell );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckInitSign(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.CheckInitSign(  );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DynamicAddCell(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    SceneCell cell = (SceneCell)translator.GetObject(L, 2, typeof(SceneCell));
                    
                    __cl_gen_to_be_invoked.DynamicAddCell( cell );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DynamicRemoveCell(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    SceneCell cell = (SceneCell)translator.GetObject(L, 2, typeof(SceneCell));
                    
                    __cl_gen_to_be_invoked.DynamicRemoveCell( cell );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Reset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.Reset(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_containerName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.containerName);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cellArr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.cellArr);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_containerInfo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.containerInfo);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_dynamicCellList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.dynamicCellList);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_containerName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.containerName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cellArr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.cellArr = (SceneCell[])translator.GetObject(L, 2, typeof(SceneCell[]));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_containerInfo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.containerInfo = (SceneContainerInfo)translator.GetObject(L, 2, typeof(SceneContainerInfo));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_dynamicCellList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SceneContainer __cl_gen_to_be_invoked = (SceneContainer)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.dynamicCellList = (System.Collections.Generic.List<SceneCell>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<SceneCell>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
