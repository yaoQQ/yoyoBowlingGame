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
    public class CellGroupWidgetWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(CellGroupWidget);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 2, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddEventListener", _m_AddEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveEventListener", _m_RemoveEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddCustomEventListener", _m_AddCustomEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddMoreCustomEventListener", _m_AddMoreCustomEventListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetCellData", _m_SetCellData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetWidgetType", _m_GetWidgetType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "setWidth", _m_setWidth);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellItemName", _g_get_cellItemName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "cellItemArr", _g_get_cellItemArr);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellItemName", _s_set_cellItemName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "cellItemArr", _s_set_cellItemArr);
            
			
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
					
					CellGroupWidget __cl_gen_ret = new CellGroupWidget();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to CellGroupWidget constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEventListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellGroupWidget __cl_gen_to_be_invoked = (CellGroupWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UIEvent eventType;translator.Get(L, 2, out eventType);
                    System.Action<UnityEngine.EventSystems.PointerEventData> onEventHandler = translator.GetDelegate<System.Action<UnityEngine.EventSystems.PointerEventData>>(L, 3);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.AddEventListener( eventType, onEventHandler );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveEventListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellGroupWidget __cl_gen_to_be_invoked = (CellGroupWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UIEvent eventType;translator.Get(L, 2, out eventType);
                    System.Action<UnityEngine.EventSystems.PointerEventData> onEventHandler = translator.GetDelegate<System.Action<UnityEngine.EventSystems.PointerEventData>>(L, 3);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.RemoveEventListener( eventType, onEventHandler );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddCustomEventListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellGroupWidget __cl_gen_to_be_invoked = (CellGroupWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UICustomEvent eventType;translator.Get(L, 2, out eventType);
                    System.Action<object> onEventHandler = translator.GetDelegate<System.Action<object>>(L, 3);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.AddCustomEventListener( eventType, onEventHandler );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddMoreCustomEventListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellGroupWidget __cl_gen_to_be_invoked = (CellGroupWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UIEvent eventType;translator.Get(L, 2, out eventType);
                    System.Action<System.Collections.Generic.List<object>> onEventHandler = translator.GetDelegate<System.Action<System.Collections.Generic.List<object>>>(L, 3);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.AddMoreCustomEventListener( eventType, onEventHandler );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
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
            
            
                CellGroupWidget __cl_gen_to_be_invoked = (CellGroupWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Collections.Generic.List<object> p_dataList = (System.Collections.Generic.List<object>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<object>));
                    System.Action<UnityEngine.GameObject, object, int> p_onUpdateCellData = translator.GetDelegate<System.Action<UnityEngine.GameObject, object, int>>(L, 3);
                    
                    __cl_gen_to_be_invoked.SetCellData( p_dataList, p_onUpdateCellData );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetWidgetType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellGroupWidget __cl_gen_to_be_invoked = (CellGroupWidget)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_setWidth(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                CellGroupWidget __cl_gen_to_be_invoked = (CellGroupWidget)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float width = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    __cl_gen_to_be_invoked.setWidth( width );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_cellItemName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                CellGroupWidget __cl_gen_to_be_invoked = (CellGroupWidget)translator.FastGetCSObj(L, 1);
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
			
                CellGroupWidget __cl_gen_to_be_invoked = (CellGroupWidget)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.cellItemArr);
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
			
                CellGroupWidget __cl_gen_to_be_invoked = (CellGroupWidget)translator.FastGetCSObj(L, 1);
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
			
                CellGroupWidget __cl_gen_to_be_invoked = (CellGroupWidget)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.cellItemArr = (CellItemWidget[])translator.GetObject(L, 2, typeof(CellItemWidget[]));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
