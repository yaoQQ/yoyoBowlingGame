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
    public class HttpPostManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(HttpPostManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendHttp", _m_SendHttp);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PostToHttpService", _m_PostToHttpService);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendHttpProtobuf", _m_SendHttpProtobuf);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PostToHttpServiceProtobuf", _m_PostToHttpServiceProtobuf);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PostPhotoToHttpService", _m_PostPhotoToHttpService);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DownloadPhoto", _m_DownloadPhoto);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 0);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Instance", _g_get_Instance);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					HttpPostManager __cl_gen_ret = new HttpPostManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to HttpPostManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendHttp(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HttpPostManager __cl_gen_to_be_invoked = (HttpPostManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string url = LuaAPI.lua_tostring(L, 2);
                    string param = LuaAPI.lua_tostring(L, 3);
                    System.Action<string> callback = translator.GetDelegate<System.Action<string>>(L, 4);
                    
                    __cl_gen_to_be_invoked.SendHttp( url, param, callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PostToHttpService(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HttpPostManager __cl_gen_to_be_invoked = (HttpPostManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string url = LuaAPI.lua_tostring(L, 2);
                    string param = LuaAPI.lua_tostring(L, 3);
                    
                        string __cl_gen_ret = __cl_gen_to_be_invoked.PostToHttpService( url, param );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendHttpProtobuf(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HttpPostManager __cl_gen_to_be_invoked = (HttpPostManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string url = LuaAPI.lua_tostring(L, 2);
                    byte[] protoBytes = LuaAPI.lua_tobytes(L, 3);
                    System.Action<byte[]> callback = translator.GetDelegate<System.Action<byte[]>>(L, 4);
                    
                    __cl_gen_to_be_invoked.SendHttpProtobuf( url, protoBytes, callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PostToHttpServiceProtobuf(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HttpPostManager __cl_gen_to_be_invoked = (HttpPostManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string url = LuaAPI.lua_tostring(L, 2);
                    byte[] protoBytes = LuaAPI.lua_tobytes(L, 3);
                    
                        byte[] __cl_gen_ret = __cl_gen_to_be_invoked.PostToHttpServiceProtobuf( url, protoBytes );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PostPhotoToHttpService(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HttpPostManager __cl_gen_to_be_invoked = (HttpPostManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string url = LuaAPI.lua_tostring(L, 2);
                    string fileName = LuaAPI.lua_tostring(L, 3);
                    byte[] photoBytes = LuaAPI.lua_tobytes(L, 4);
                    System.Action<bool> finishCallback = translator.GetDelegate<System.Action<bool>>(L, 5);
                    
                    __cl_gen_to_be_invoked.PostPhotoToHttpService( url, fileName, photoBytes, finishCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DownloadPhoto(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HttpPostManager __cl_gen_to_be_invoked = (HttpPostManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string url = LuaAPI.lua_tostring(L, 2);
                    bool isAllowCache = LuaAPI.lua_toboolean(L, 3);
                    System.Action<UnityEngine.Texture2D, string> finishCallback = translator.GetDelegate<System.Action<UnityEngine.Texture2D, string>>(L, 4);
                    
                    __cl_gen_to_be_invoked.DownloadPhoto( url, isAllowCache, finishCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, HttpPostManager.Instance);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
