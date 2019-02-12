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
    public class EZReplayManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(EZReplayManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 21, 21);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "EZRicon", _g_get_EZRicon);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "playIcon", _g_get_playIcon);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pauseIcon", _g_get_pauseIcon);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "startRecordIcon", _g_get_startRecordIcon);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "stopRecordIcon", _g_get_stopRecordIcon);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "replayIcon", _g_get_replayIcon);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "stopIcon", _g_get_stopIcon);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "closeIcon", _g_get_closeIcon);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "rewindIcon", _g_get_rewindIcon);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "useRecordingGUI", _g_get_useRecordingGUI);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "useReplayGUI", _g_get_useReplayGUI);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "useDarkStripesInReplay", _g_get_useDarkStripesInReplay);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "saveToFileOnDefault", _g_get_saveToFileOnDefault);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "autoDeactivateLiveObjectsOnLoad", _g_get_autoDeactivateLiveObjectsOnLoad);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "recordingInterval", _g_get_recordingInterval);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gOs2propMappingsRecordingSlot", _g_get_gOs2propMappingsRecordingSlot);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gOs2propMappingsLoadingSlot", _g_get_gOs2propMappingsLoadingSlot);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gOs2propMappings", _g_get_gOs2propMappings);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "gameObjectsToRecord", _g_get_gameObjectsToRecord);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "componentsAndScriptsToKeepAtReplay", _g_get_componentsAndScriptsToKeepAtReplay);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "addTestTarget", _g_get_addTestTarget);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "EZRicon", _s_set_EZRicon);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "playIcon", _s_set_playIcon);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pauseIcon", _s_set_pauseIcon);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "startRecordIcon", _s_set_startRecordIcon);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "stopRecordIcon", _s_set_stopRecordIcon);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "replayIcon", _s_set_replayIcon);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "stopIcon", _s_set_stopIcon);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "closeIcon", _s_set_closeIcon);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "rewindIcon", _s_set_rewindIcon);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "useRecordingGUI", _s_set_useRecordingGUI);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "useReplayGUI", _s_set_useReplayGUI);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "useDarkStripesInReplay", _s_set_useDarkStripesInReplay);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "saveToFileOnDefault", _s_set_saveToFileOnDefault);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "autoDeactivateLiveObjectsOnLoad", _s_set_autoDeactivateLiveObjectsOnLoad);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "recordingInterval", _s_set_recordingInterval);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "gOs2propMappingsRecordingSlot", _s_set_gOs2propMappingsRecordingSlot);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "gOs2propMappingsLoadingSlot", _s_set_gOs2propMappingsLoadingSlot);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "gOs2propMappings", _s_set_gOs2propMappings);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "gameObjectsToRecord", _s_set_gameObjectsToRecord);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "componentsAndScriptsToKeepAtReplay", _s_set_componentsAndScriptsToKeepAtReplay);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "addTestTarget", _s_set_addTestTarget);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 28, 1, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "useRecordingSlot", _m_useRecordingSlot_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "useLoadingSlot", _m_useLoadingSlot_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "saveToFile", _m_saveToFile_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "loadFromFile", _m_loadFromFile_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "mark4Recording", _m_mark4Recording_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "getMaxFrames", _m_getMaxFrames_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "stop", _m_stop_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "record", _m_record_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "pause", _m_pause_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "play", _m_play_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "getCurrentPosition", _m_getCurrentPosition_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "getCurrentAction", _m_getCurrentAction_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "getCurrentMode", _m_getCurrentMode_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "close", _m_close_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "dispose", _m_dispose_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ACTION_RECORD", EZReplayManager.ACTION_RECORD);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ACTION_PLAY", EZReplayManager.ACTION_PLAY);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ACTION_PAUSED", EZReplayManager.ACTION_PAUSED);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ACTION_STOPPED", EZReplayManager.ACTION_STOPPED);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MODE_LIVE", EZReplayManager.MODE_LIVE);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MODE_REPLAY", EZReplayManager.MODE_REPLAY);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "S_PARENT_NAME", EZReplayManager.S_PARENT_NAME);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "S_EZR_ASSET_PATH", EZReplayManager.S_EZR_ASSET_PATH);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "showErrors", EZReplayManager.showErrors);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "showWarnings", EZReplayManager.showWarnings);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "showHints", EZReplayManager.showHints);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EZR_VERSION", EZReplayManager.EZR_VERSION);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "singleton", _g_get_singleton);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					EZReplayManager __cl_gen_ret = new EZReplayManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to EZReplayManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_useRecordingSlot_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    EZReplayManager.useRecordingSlot(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_useLoadingSlot_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    EZReplayManager.useLoadingSlot(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_saveToFile_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string filename = LuaAPI.lua_tostring(L, 1);
                    
                    EZReplayManager.saveToFile( filename );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_loadFromFile_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string filename = LuaAPI.lua_tostring(L, 1);
                    
                    EZReplayManager.loadFromFile( filename );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_mark4Recording_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& translator.Assignable<UnityEngine.GameObject>(L, 1)) 
                {
                    UnityEngine.GameObject go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    
                    EZReplayManager.mark4Recording( go );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    UnityEngine.GameObject go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    bool prepareToSaveToFile = LuaAPI.lua_toboolean(L, 2);
                    
                    EZReplayManager.mark4Recording( go, prepareToSaveToFile );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    UnityEngine.GameObject go = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    bool prepareToSaveToFile = LuaAPI.lua_toboolean(L, 2);
                    string prefabLoadPath = LuaAPI.lua_tostring(L, 3);
                    
                    EZReplayManager.mark4Recording( go, prepareToSaveToFile, prefabLoadPath );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to EZReplayManager.mark4Recording!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getMaxFrames_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& translator.Assignable<System.Collections.Generic.Dictionary<UnityEngine.GameObject, Object2PropertiesMapping>>(L, 1)) 
                {
                    System.Collections.Generic.Dictionary<UnityEngine.GameObject, Object2PropertiesMapping> go2o2pm = (System.Collections.Generic.Dictionary<UnityEngine.GameObject, Object2PropertiesMapping>)translator.GetObject(L, 1, typeof(System.Collections.Generic.Dictionary<UnityEngine.GameObject, Object2PropertiesMapping>));
                    
                        int __cl_gen_ret = EZReplayManager.getMaxFrames( go2o2pm );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                if(__gen_param_count == 1&& translator.Assignable<Object2PropertiesMapping>(L, 1)) 
                {
                    Object2PropertiesMapping o2pm = (Object2PropertiesMapping)translator.GetObject(L, 1, typeof(Object2PropertiesMapping));
                    
                        int __cl_gen_ret = EZReplayManager.getMaxFrames( o2pm );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to EZReplayManager.getMaxFrames!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_stop_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    EZReplayManager.stop(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_record_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    EZReplayManager.record(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_pause_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    EZReplayManager.pause(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_play_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    int speed = LuaAPI.xlua_tointeger(L, 1);
                    
                    EZReplayManager.play( speed );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    int speed = LuaAPI.xlua_tointeger(L, 1);
                    bool playImmediately = LuaAPI.lua_toboolean(L, 2);
                    bool backwards = LuaAPI.lua_toboolean(L, 3);
                    bool exitOnFinished = LuaAPI.lua_toboolean(L, 4);
                    
                    EZReplayManager.play( speed, playImmediately, backwards, exitOnFinished );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to EZReplayManager.play!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getCurrentPosition_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        int __cl_gen_ret = EZReplayManager.getCurrentPosition(  );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getCurrentAction_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        int __cl_gen_ret = EZReplayManager.getCurrentAction(  );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getCurrentMode_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        int __cl_gen_ret = EZReplayManager.getCurrentMode(  );
                        LuaAPI.xlua_pushinteger(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_close_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    EZReplayManager.close(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_dispose_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    EZReplayManager.dispose(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_singleton(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, EZReplayManager.singleton);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EZRicon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.EZRicon);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_playIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.playIcon);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pauseIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.pauseIcon);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_startRecordIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.startRecordIcon);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stopRecordIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.stopRecordIcon);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_replayIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.replayIcon);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_stopIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.stopIcon);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_closeIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.closeIcon);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_rewindIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.rewindIcon);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_useRecordingGUI(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.useRecordingGUI);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_useReplayGUI(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.useReplayGUI);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_useDarkStripesInReplay(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.useDarkStripesInReplay);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_saveToFileOnDefault(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.saveToFileOnDefault);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_autoDeactivateLiveObjectsOnLoad(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.autoDeactivateLiveObjectsOnLoad);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_recordingInterval(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, __cl_gen_to_be_invoked.recordingInterval);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gOs2propMappingsRecordingSlot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.gOs2propMappingsRecordingSlot);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gOs2propMappingsLoadingSlot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.gOs2propMappingsLoadingSlot);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gOs2propMappings(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.gOs2propMappings);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_gameObjectsToRecord(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.gameObjectsToRecord);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_componentsAndScriptsToKeepAtReplay(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.componentsAndScriptsToKeepAtReplay);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_addTestTarget(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.addTestTarget);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EZRicon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.EZRicon = (UnityEngine.Texture2D)translator.GetObject(L, 2, typeof(UnityEngine.Texture2D));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_playIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.playIcon = (UnityEngine.Texture2D)translator.GetObject(L, 2, typeof(UnityEngine.Texture2D));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pauseIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.pauseIcon = (UnityEngine.Texture2D)translator.GetObject(L, 2, typeof(UnityEngine.Texture2D));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_startRecordIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.startRecordIcon = (UnityEngine.Texture2D)translator.GetObject(L, 2, typeof(UnityEngine.Texture2D));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_stopRecordIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.stopRecordIcon = (UnityEngine.Texture2D)translator.GetObject(L, 2, typeof(UnityEngine.Texture2D));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_replayIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.replayIcon = (UnityEngine.Texture2D)translator.GetObject(L, 2, typeof(UnityEngine.Texture2D));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_stopIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.stopIcon = (UnityEngine.Texture2D)translator.GetObject(L, 2, typeof(UnityEngine.Texture2D));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_closeIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.closeIcon = (UnityEngine.Texture2D)translator.GetObject(L, 2, typeof(UnityEngine.Texture2D));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_rewindIcon(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.rewindIcon = (UnityEngine.Texture2D)translator.GetObject(L, 2, typeof(UnityEngine.Texture2D));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_useRecordingGUI(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.useRecordingGUI = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_useReplayGUI(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.useReplayGUI = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_useDarkStripesInReplay(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.useDarkStripesInReplay = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_saveToFileOnDefault(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.saveToFileOnDefault = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_autoDeactivateLiveObjectsOnLoad(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.autoDeactivateLiveObjectsOnLoad = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_recordingInterval(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.recordingInterval = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gOs2propMappingsRecordingSlot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.gOs2propMappingsRecordingSlot = (System.Collections.Generic.Dictionary<UnityEngine.GameObject, Object2PropertiesMapping>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<UnityEngine.GameObject, Object2PropertiesMapping>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gOs2propMappingsLoadingSlot(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.gOs2propMappingsLoadingSlot = (System.Collections.Generic.Dictionary<UnityEngine.GameObject, Object2PropertiesMapping>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<UnityEngine.GameObject, Object2PropertiesMapping>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gOs2propMappings(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.gOs2propMappings = (System.Collections.Generic.Dictionary<UnityEngine.GameObject, Object2PropertiesMapping>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<UnityEngine.GameObject, Object2PropertiesMapping>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_gameObjectsToRecord(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.gameObjectsToRecord = (System.Collections.Generic.List<UnityEngine.GameObject>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.GameObject>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_componentsAndScriptsToKeepAtReplay(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.componentsAndScriptsToKeepAtReplay = (System.Collections.Generic.List<string>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<string>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_addTestTarget(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                EZReplayManager __cl_gen_to_be_invoked = (EZReplayManager)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.addTestTarget = (System.Collections.Generic.List<UnityEngine.GameObject>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<UnityEngine.GameObject>));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
