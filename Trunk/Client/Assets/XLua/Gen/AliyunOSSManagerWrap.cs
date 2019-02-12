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
    public class AliyunOSSManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(AliyunOSSManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateClient", _m_CreateClient);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UploadImage", _m_UploadImage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DownloadImage", _m_DownloadImage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DownloadResizeImage", _m_DownloadResizeImage);
			
			
			
			
			
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
					
					AliyunOSSManager __cl_gen_ret = new AliyunOSSManager();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to AliyunOSSManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateClient(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AliyunOSSManager __cl_gen_to_be_invoked = (AliyunOSSManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string endpoint = LuaAPI.lua_tostring(L, 2);
                    string accessKeyId = LuaAPI.lua_tostring(L, 3);
                    string accessKeySecret = LuaAPI.lua_tostring(L, 4);
                    
                        Aliyun.OSS.OssClient __cl_gen_ret = __cl_gen_to_be_invoked.CreateClient( endpoint, accessKeyId, accessKeySecret );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UploadImage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AliyunOSSManager __cl_gen_to_be_invoked = (AliyunOSSManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string name = LuaAPI.lua_tostring(L, 2);
                    byte[] photoBytes = LuaAPI.lua_tobytes(L, 3);
                    string type = LuaAPI.lua_tostring(L, 4);
                    System.Action<bool> finishCallback = translator.GetDelegate<System.Action<bool>>(L, 5);
                    
                    __cl_gen_to_be_invoked.UploadImage( name, photoBytes, type, finishCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DownloadImage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AliyunOSSManager __cl_gen_to_be_invoked = (AliyunOSSManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string name = LuaAPI.lua_tostring(L, 2);
                    int uid = LuaAPI.xlua_tointeger(L, 3);
                    System.Action<UnityEngine.Texture2D, string> finishCallback = translator.GetDelegate<System.Action<UnityEngine.Texture2D, string>>(L, 4);
                    
                    __cl_gen_to_be_invoked.DownloadImage( name, uid, finishCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DownloadResizeImage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AliyunOSSManager __cl_gen_to_be_invoked = (AliyunOSSManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string name = LuaAPI.lua_tostring(L, 2);
                    string resizeType = LuaAPI.lua_tostring(L, 3);
                    string process = LuaAPI.lua_tostring(L, 4);
                    int uid = LuaAPI.xlua_tointeger(L, 5);
                    System.Action<UnityEngine.Texture2D, string> finishCallback = translator.GetDelegate<System.Action<UnityEngine.Texture2D, string>>(L, 6);
                    
                    __cl_gen_to_be_invoked.DownloadResizeImage( name, resizeType, process, uid, finishCallback );
                    
                    
                    
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
			    translator.Push(L, AliyunOSSManager.Instance);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
