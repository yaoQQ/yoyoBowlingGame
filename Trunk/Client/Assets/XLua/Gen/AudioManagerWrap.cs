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
    public class AudioManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(AudioManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 10, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayBgMusic", _m_PlayBgMusic);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopBgMusic", _m_StopBgMusic);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAudioSource", _m_GetAudioSource);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlaySound", _m_PlaySound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopSound", _m_StopSound);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Play2D", _m_Play2D);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AdjustVolume", _m_AdjustVolume);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeBgmVolume", _m_ChangeBgmVolume);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ChangeSoundVolume", _m_ChangeSoundVolume);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Preload", _m_Preload);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "AudioManager does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayBgMusic(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AudioManager __cl_gen_to_be_invoked = (AudioManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string packName = LuaAPI.lua_tostring(L, 2);
                    string soundName = LuaAPI.lua_tostring(L, 3);
                    
                    __cl_gen_to_be_invoked.PlayBgMusic( packName, soundName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopBgMusic(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AudioManager __cl_gen_to_be_invoked = (AudioManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.StopBgMusic(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAudioSource(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AudioManager __cl_gen_to_be_invoked = (AudioManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        UnityEngine.AudioSource __cl_gen_ret = __cl_gen_to_be_invoked.GetAudioSource(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlaySound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AudioManager __cl_gen_to_be_invoked = (AudioManager)translator.FastGetCSObj(L, 1);
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    string packName = LuaAPI.lua_tostring(L, 2);
                    string soundName = LuaAPI.lua_tostring(L, 3);
                    bool isMute = LuaAPI.lua_toboolean(L, 4);
                    
                    __cl_gen_to_be_invoked.PlaySound( packName, soundName, isMute );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    string packName = LuaAPI.lua_tostring(L, 2);
                    string soundName = LuaAPI.lua_tostring(L, 3);
                    
                    __cl_gen_to_be_invoked.PlaySound( packName, soundName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to AudioManager.PlaySound!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopSound(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AudioManager __cl_gen_to_be_invoked = (AudioManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.StopSound(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Play2D(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AudioManager __cl_gen_to_be_invoked = (AudioManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string packName = LuaAPI.lua_tostring(L, 2);
                    string soundName = LuaAPI.lua_tostring(L, 3);
                    
                    __cl_gen_to_be_invoked.Play2D( packName, soundName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AdjustVolume(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AudioManager __cl_gen_to_be_invoked = (AudioManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.AdjustVolume(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeBgmVolume(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AudioManager __cl_gen_to_be_invoked = (AudioManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float value = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    __cl_gen_to_be_invoked.ChangeBgmVolume( value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeSoundVolume(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AudioManager __cl_gen_to_be_invoked = (AudioManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float value = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    __cl_gen_to_be_invoked.ChangeSoundVolume( value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Preload(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AudioManager __cl_gen_to_be_invoked = (AudioManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string packName = LuaAPI.lua_tostring(L, 2);
                    string soundName = LuaAPI.lua_tostring(L, 3);
                    System.Action finishCallback = translator.GetDelegate<System.Action>(L, 4);
                    
                    __cl_gen_to_be_invoked.Preload( packName, soundName, finishCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
