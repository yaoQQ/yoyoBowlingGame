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
    public class MapManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(MapManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 7, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "SetMyPosTile", _m_SetMyPosTile_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TileYToLat", _m_TileYToLat_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LatToTileY", _m_LatToTileY_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetDistance", _m_GetDistance_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMapTiles", _m_GetMapTiles_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetMapTile", _m_GetMapTile_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					MapManager __cl_gen_ret = new MapManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to MapManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetMyPosTile_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int zoom = LuaAPI.xlua_tointeger(L, 1);
                    int tileX = LuaAPI.xlua_tointeger(L, 2);
                    int tileY = LuaAPI.xlua_tointeger(L, 3);
                    
                    MapManager.SetMyPosTile( zoom, tileX, tileY );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TileYToLat_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int tileY = LuaAPI.xlua_tointeger(L, 1);
                    int zoom = LuaAPI.xlua_tointeger(L, 2);
                    float pixelY = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        double __cl_gen_ret = MapManager.TileYToLat( tileY, zoom, pixelY );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LatToTileY_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    double lat = LuaAPI.lua_tonumber(L, 1);
                    int zoom = LuaAPI.xlua_tointeger(L, 2);
                    int tileY;
                    float pixelY;
                    
                    MapManager.LatToTileY( lat, zoom, out tileY, out pixelY );
                    LuaAPI.xlua_pushinteger(L, tileY);
                        
                    LuaAPI.lua_pushnumber(L, pixelY);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetDistance_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    double lng1 = LuaAPI.lua_tonumber(L, 1);
                    double lat1 = LuaAPI.lua_tonumber(L, 2);
                    double lng2 = LuaAPI.lua_tonumber(L, 3);
                    double lat2 = LuaAPI.lua_tonumber(L, 4);
                    
                        float __cl_gen_ret = MapManager.GetDistance( lng1, lat1, lng2, lat2 );
                        LuaAPI.lua_pushnumber(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMapTiles_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string urlFormat = LuaAPI.lua_tostring(L, 1);
                    int zoom = LuaAPI.xlua_tointeger(L, 2);
                    int x = LuaAPI.xlua_tointeger(L, 3);
                    int y = LuaAPI.xlua_tointeger(L, 4);
                    int rangeX = LuaAPI.xlua_tointeger(L, 5);
                    int rangeY = LuaAPI.xlua_tointeger(L, 6);
                    System.Action<int, int, int, UnityEngine.Sprite> callback = translator.GetDelegate<System.Action<int, int, int, UnityEngine.Sprite>>(L, 7);
                    
                    MapManager.GetMapTiles( urlFormat, zoom, x, y, rangeX, rangeY, callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMapTile_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string urlFormat = LuaAPI.lua_tostring(L, 1);
                    int zoom = LuaAPI.xlua_tointeger(L, 2);
                    int x = LuaAPI.xlua_tointeger(L, 3);
                    int y = LuaAPI.xlua_tointeger(L, 4);
                    System.Action<int, int, int, UnityEngine.Sprite> callback = translator.GetDelegate<System.Action<int, int, int, UnityEngine.Sprite>>(L, 5);
                    
                    MapManager.GetMapTile( urlFormat, zoom, x, y, callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
