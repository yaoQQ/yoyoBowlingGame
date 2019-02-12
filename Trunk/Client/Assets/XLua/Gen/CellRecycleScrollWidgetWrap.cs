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
    public class CellRecycleScrollWidgetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(CellRecycleScrollWidget);
			Utils.BeginObjectRegister(type, L, translator, 0, 9, 5, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetWidgetType", _m_GetWidgetType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetDataByIndex", _m_GetDataByIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetCellHeightOffSetByIndex", _m_SetCellHeightOffSetByIndex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetCellData", _m_SetCellData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetContentPos", _m_SetContentPos);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetToContentBottom", _m_SetToContentBottom);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LocateCellPosition", _m_LocateCellPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HoldAppearScroll", _m_HoldAppearScroll);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AppearScroll", _m_AppearScroll);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "StartIndex", _g_get_StartIndex);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellItemName", _g_get_cellItemName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellItemArr", _g_get_cellItemArr);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellSpaceValue", _g_get_cellSpaceValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellPosValue", _g_get_cellPosValue);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellItemName", _s_set_cellItemName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellItemArr", _s_set_cellItemArr);
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
					
					CellRecycleScrollWidget __cl_gen_ret = new CellRecycleScrollWidget();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to CellRecycleScrollWidget constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWidgetType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_GetDataByIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    
                        object __cl_gen_ret = __cl_gen_to_be_invoked.GetDataByIndex( index );
                        translator.PushAny(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetCellHeightOffSetByIndex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int index = LuaAPI.xlua_tointeger(L, 2);
                    float height = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    __cl_gen_to_be_invoked.SetCellHeightOffSetByIndex( index, height );
                    
                    
                    
                    return 0;
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
            
            
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 4&& translator.Assignable<System.Collections.Generic.List<object>>(L, 2)&& translator.Assignable<System.Action<UnityEngine.GameObject, object, int, int>>(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    System.Collections.Generic.List<object> pDataList = (System.Collections.Generic.List<object>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<object>));
                    System.Action<UnityEngine.GameObject, object, int, int> pOnUpdateCellData = translator.GetDelegate<System.Action<UnityEngine.GameObject, object, int, int>>(L, 3);
                    bool resetPos = LuaAPI.lua_toboolean(L, 4);
                    
                    __cl_gen_to_be_invoked.SetCellData( pDataList, pOnUpdateCellData, resetPos );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& translator.Assignable<System.Collections.Generic.List<object>>(L, 2)&& translator.Assignable<System.Action<UnityEngine.GameObject, object, int, int>>(L, 3)) 
                {
                    System.Collections.Generic.List<object> pDataList = (System.Collections.Generic.List<object>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<object>));
                    System.Action<UnityEngine.GameObject, object, int, int> pOnUpdateCellData = translator.GetDelegate<System.Action<UnityEngine.GameObject, object, int, int>>(L, 3);
                    
                    __cl_gen_to_be_invoked.SetCellData( pDataList, pOnUpdateCellData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to CellRecycleScrollWidget.SetCellData!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetContentPos(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float value = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    __cl_gen_to_be_invoked.SetContentPos( value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetToContentBottom(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.SetToContentBottom(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LocateCellPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int dataIndex = LuaAPI.xlua_tointeger(L, 2);
                    
                    __cl_gen_to_be_invoked.LocateCellPosition( dataIndex );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HoldAppearScroll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float scale = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    __cl_gen_to_be_invoked.HoldAppearScroll( scale );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AppearScroll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float holdTime = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    __cl_gen_to_be_invoked.AppearScroll( holdTime );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StartIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
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
			
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
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
			
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.cellItemArr);
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
			
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.cellSpaceValue);
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
			
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.cellPosValue);
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
			
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
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
			
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.cellItemArr = (CellItemWidget[])translator.GetObject(L, 2, typeof(CellItemWidget[]));
            
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
			
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.cellSpaceValue = (float)LuaAPI.lua_tonumber(L, 2);
            
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
			
                CellRecycleScrollWidget __cl_gen_to_be_invoked = (CellRecycleScrollWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.cellPosValue = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
