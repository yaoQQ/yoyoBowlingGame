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
    public class SQLiteManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(SQLiteManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 15, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateOrOpenDb", _m_CreateOrOpenDb);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InsertValues", _m_InsertValues);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InsertOrUpdateValues", _m_InsertOrUpdateValues);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InsertOrIgnoreValues", _m_InsertOrIgnoreValues);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InsertStringInto", _m_InsertStringInto);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseConnection", _m_CloseConnection);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DeleteValuesOR", _m_DeleteValuesOR);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DeleteValuesAND", _m_DeleteValuesAND);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UpdateData", _m_UpdateData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadTable", _m_ReadTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadFullTableData", _m_ReadFullTableData);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DeleteFullTable", _m_DeleteFullTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DeleteTable", _m_DeleteTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateTable", _m_CreateTable);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "isHaveTable", _m_isHaveTable);
			
			
			
			
			
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
					
					SQLiteManager __cl_gen_ret = new SQLiteManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to SQLiteManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateOrOpenDb(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string dbName = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.CreateOrOpenDb( dbName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InsertValues(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string tableName = LuaAPI.lua_tostring(L, 2);
                    string[] values = (string[])translator.GetObject(L, 3, typeof(string[]));
                    
                    __cl_gen_to_be_invoked.InsertValues( tableName, values );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InsertOrUpdateValues(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string tableName = LuaAPI.lua_tostring(L, 2);
                    string[] values = (string[])translator.GetObject(L, 3, typeof(string[]));
                    
                    __cl_gen_to_be_invoked.InsertOrUpdateValues( tableName, values );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InsertOrIgnoreValues(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string tableName = LuaAPI.lua_tostring(L, 2);
                    string[] values = (string[])translator.GetObject(L, 3, typeof(string[]));
                    
                    __cl_gen_to_be_invoked.InsertOrIgnoreValues( tableName, values );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InsertStringInto(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string table_name = LuaAPI.lua_tostring(L, 2);
                    string[] values = (string[])translator.GetObject(L, 3, typeof(string[]));
                    
                    __cl_gen_to_be_invoked.InsertStringInto( table_name, values );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseConnection(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.CloseConnection(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteValuesOR(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string tableName = LuaAPI.lua_tostring(L, 2);
                    string[] colNames = (string[])translator.GetObject(L, 3, typeof(string[]));
                    string[] operations = (string[])translator.GetObject(L, 4, typeof(string[]));
                    string[] colValues = (string[])translator.GetObject(L, 5, typeof(string[]));
                    
                    __cl_gen_to_be_invoked.DeleteValuesOR( tableName, colNames, operations, colValues );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteValuesAND(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string tableName = LuaAPI.lua_tostring(L, 2);
                    string[] colNames = (string[])translator.GetObject(L, 3, typeof(string[]));
                    string[] operations = (string[])translator.GetObject(L, 4, typeof(string[]));
                    string[] colValues = (string[])translator.GetObject(L, 5, typeof(string[]));
                    
                    __cl_gen_to_be_invoked.DeleteValuesAND( tableName, colNames, operations, colValues );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string tableName = LuaAPI.lua_tostring(L, 2);
                    string[] colNames = (string[])translator.GetObject(L, 3, typeof(string[]));
                    string[] colValues = (string[])translator.GetObject(L, 4, typeof(string[]));
                    string key = LuaAPI.lua_tostring(L, 5);
                    string operation = LuaAPI.lua_tostring(L, 6);
                    string value = LuaAPI.lua_tostring(L, 7);
                    
                    __cl_gen_to_be_invoked.UpdateData( tableName, colNames, colValues, key, operation, value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string tableName = LuaAPI.lua_tostring(L, 2);
                    string[] items = (string[])translator.GetObject(L, 3, typeof(string[]));
                    string[] colNames = (string[])translator.GetObject(L, 4, typeof(string[]));
                    string[] operations = (string[])translator.GetObject(L, 5, typeof(string[]));
                    string[] colValues = (string[])translator.GetObject(L, 6, typeof(string[]));
                    
                        System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>> __cl_gen_ret = __cl_gen_to_be_invoked.ReadTable( tableName, items, colNames, operations, colValues );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadFullTableData(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string table_name = LuaAPI.lua_tostring(L, 2);
                    
                        System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>> __cl_gen_ret = __cl_gen_to_be_invoked.ReadFullTableData( table_name );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteFullTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string table_name = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.DeleteFullTable( table_name );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DeleteTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string table_name = LuaAPI.lua_tostring(L, 2);
                    
                    __cl_gen_to_be_invoked.DeleteTable( table_name );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string tableName = LuaAPI.lua_tostring(L, 2);
                    string[] colNames = (string[])translator.GetObject(L, 3, typeof(string[]));
                    string[] colTypes = (string[])translator.GetObject(L, 4, typeof(string[]));
                    
                    __cl_gen_to_be_invoked.CreateTable( tableName, colNames, colTypes );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_isHaveTable(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SQLiteManager __cl_gen_to_be_invoked = (SQLiteManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string tableName = LuaAPI.lua_tostring(L, 2);
                    
                        bool __cl_gen_ret = __cl_gen_to_be_invoked.isHaveTable( tableName );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
