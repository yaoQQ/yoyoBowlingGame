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
    public class testManageWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(testManage);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 7, 7);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "showStage", _m_showStage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnRenderObject", _m_OnRenderObject);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "addScanPos", _m_addScanPos);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "lineMaterial", _g_get_lineMaterial);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "isDraw3D", _g_get_isDraw3D);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "begainObj", _g_get_begainObj);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "endObj", _g_get_endObj);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "posText", _g_get_posText);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "stageList", _g_get_stageList);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "posList", _g_get_posList);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "lineMaterial", _s_set_lineMaterial);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "isDraw3D", _s_set_isDraw3D);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "begainObj", _s_set_begainObj);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "endObj", _s_set_endObj);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "posText", _s_set_posText);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "stageList", _s_set_stageList);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "posList", _s_set_posList);
            
			
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
					
					testManage __cl_gen_ret = new testManage();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to testManage constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_showStage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.showStage(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnRenderObject(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.OnRenderObject(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_addScanPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 pos;translator.Get(L, 2, out pos);
                    
                    __cl_gen_to_be_invoked.addScanPos( pos );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_lineMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.lineMaterial);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isDraw3D(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.isDraw3D);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_begainObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.begainObj);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_endObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.endObj);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_posText(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.posText);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stageList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.stageList);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_posList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.posList);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_lineMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.lineMaterial = (UnityEngine.Material)translator.GetObject(L, 2, typeof(UnityEngine.Material));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isDraw3D(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.isDraw3D = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_begainObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.begainObj = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_endObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.endObj = (UnityEngine.GameObject)translator.GetObject(L, 2, typeof(UnityEngine.GameObject));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_posText(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.posText = (UnityEngine.UI.Text)translator.GetObject(L, 2, typeof(UnityEngine.UI.Text));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_stageList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.stageList = (System.Collections.Generic.List<UnityEngine.Vector3>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Vector3>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_posList(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                testManage __cl_gen_to_be_invoked = (testManage)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.posList = (System.Collections.Generic.List<UnityEngine.Vector3>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.Vector3>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
