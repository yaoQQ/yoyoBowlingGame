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
    public class GridRecycleScrollWidgetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(GridRecycleScrollWidget);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 7, 6);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetWidgetType", _m_GetWidgetType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetCellData", _m_SetCellData);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "StartIndex", _g_get_StartIndex);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellItemName", _g_get_cellItemName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellItemArr", _g_get_cellItemArr);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "rowValue", _g_get_rowValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "columnValue", _g_get_columnValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellSpaceValue", _g_get_cellSpaceValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellPosValue", _g_get_cellPosValue);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellItemName", _s_set_cellItemName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellItemArr", _s_set_cellItemArr);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "rowValue", _s_set_rowValue);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "columnValue", _s_set_columnValue);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellSpaceValue", _s_set_cellSpaceValue);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellPosValue", _s_set_cellPosValue);
            
			
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
					
					GridRecycleScrollWidget __cl_gen_ret = new GridRecycleScrollWidget();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to GridRecycleScrollWidget constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWidgetType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        WidgetType __cl_gen_ret = __cl_gen_to_be_invoked.GetWidgetType(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetCellData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 4&& translator.Assignable<System.Collections.Generic.List<object>>(L, 2)&& translator.Assignable<System.Action<UnityEngine.GameObject, object, int>>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    System.Collections.Generic.List<object> p_dataList = (System.Collections.Generic.List<object>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<object>));
                    System.Action<UnityEngine.GameObject, object, int> p_onUpdateCellData = translator.GetDelegate<System.Action<UnityEngine.GameObject, object, int>>(L, 3);
                    bool resetPos = LuaAPI.lua_toboolean(L, 4);
                    
                    __cl_gen_to_be_invoked.SetCellData( p_dataList, p_onUpdateCellData, resetPos );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& translator.Assignable<System.Collections.Generic.List<object>>(L, 2)&& translator.Assignable<System.Action<UnityEngine.GameObject, object, int>>(L, 3)) 
                {
                    System.Collections.Generic.List<object> p_dataList = (System.Collections.Generic.List<object>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<object>));
                    System.Action<UnityEngine.GameObject, object, int> p_onUpdateCellData = translator.GetDelegate<System.Action<UnityEngine.GameObject, object, int>>(L, 3);
                    
                    __cl_gen_to_be_invoked.SetCellData( p_dataList, p_onUpdateCellData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to GridRecycleScrollWidget.SetCellData!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StartIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.StartIndex);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cellItemName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.cellItemName);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cellItemArr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.cellItemArr);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_rowValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.rowValue);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_columnValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.columnValue);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cellSpaceValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, __cl_gen_to_be_invoked.cellSpaceValue);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cellPosValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector2(L, __cl_gen_to_be_invoked.cellPosValue);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cellItemName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.cellItemName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cellItemArr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.cellItemArr = (CellItemWidget[])translator.GetObject(L, 2, typeof(CellItemWidget[]));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_rowValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.rowValue = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_columnValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.columnValue = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cellSpaceValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.cellSpaceValue = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_cellPosValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                GridRecycleScrollWidget __cl_gen_to_be_invoked = (GridRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector2 __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.cellPosValue = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
