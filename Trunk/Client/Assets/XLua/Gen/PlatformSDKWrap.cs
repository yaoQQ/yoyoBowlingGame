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
    public class PlatformSDKWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(PlatformSDK);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 21, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "ActiveInitializeUI", _m_ActiveInitializeUI_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetSimOperator", _m_GetSimOperator_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsWXAppInstalled", _m_IsWXAppInstalled_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WxSendAuth", _m_WxSendAuth_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WxShare", _m_WxShare_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WxPay", _m_WxPay_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AlipaySendAuth", _m_AlipaySendAuth_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AliPay", _m_AliPay_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "QQLogin", _m_QQLogin_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RestartApp", _m_RestartApp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StartLocation", _m_StartLocation_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OpenGaodeMapApp", _m_OpenGaodeMapApp_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TakePhonePhoto", _m_TakePhonePhoto_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OpenUniWebView", _m_OpenUniWebView_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PickDate", _m_PickDate_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PickTime", _m_PickTime_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PickDateTime", _m_PickDateTime_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ShowStatusBar", _m_ShowStatusBar_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetStatusBarColor", _m_SetStatusBarColor_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ScanQRCode", _m_ScanQRCode_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					PlatformSDK __cl_gen_ret = new PlatformSDK();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to PlatformSDK constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ActiveInitializeUI_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    PlatformSDK.ActiveInitializeUI(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSimOperator_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        string __cl_gen_ret = PlatformSDK.GetSimOperator(  );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsWXAppInstalled_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        bool __cl_gen_ret = PlatformSDK.IsWXAppInstalled(  );
                        LuaAPI.lua_pushboolean(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WxSendAuth_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    PlatformSDK.WxSendAuth(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WxShare_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int shareType = LuaAPI.xlua_tointeger(L, 1);
                    int mode = LuaAPI.xlua_tointeger(L, 2);
                    string url = LuaAPI.lua_tostring(L, 3);
                    string title = LuaAPI.lua_tostring(L, 4);
                    string description = LuaAPI.lua_tostring(L, 5);
                    string bmpUrl = LuaAPI.lua_tostring(L, 6);
                    System.Action succeedCallback = translator.GetDelegate<System.Action>(L, 7);
                    System.Action cancelCallback = translator.GetDelegate<System.Action>(L, 8);
                    System.Action failCallback = translator.GetDelegate<System.Action>(L, 9);
                    
                    PlatformSDK.WxShare( shareType, mode, url, title, description, bmpUrl, succeedCallback, cancelCallback, failCallback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WxPay_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string prepayId = LuaAPI.lua_tostring(L, 1);
                    string nonceStr = LuaAPI.lua_tostring(L, 2);
                    string sign = LuaAPI.lua_tostring(L, 3);
                    string timeStamp = LuaAPI.lua_tostring(L, 4);
                    string packageValue = LuaAPI.lua_tostring(L, 5);
                    System.Action<string> callback = translator.GetDelegate<System.Action<string>>(L, 6);
                    
                    PlatformSDK.WxPay( prepayId, nonceStr, sign, timeStamp, packageValue, callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AlipaySendAuth_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string authInfo = LuaAPI.lua_tostring(L, 1);
                    
                    PlatformSDK.AlipaySendAuth( authInfo );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AliPay_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string orderInfo = LuaAPI.lua_tostring(L, 1);
                    System.Action<string> callback = translator.GetDelegate<System.Action<string>>(L, 2);
                    
                    PlatformSDK.AliPay( orderInfo, callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_QQLogin_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    PlatformSDK.QQLogin(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RestartApp_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    PlatformSDK.RestartApp(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartLocation_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action<string> callback = translator.GetDelegate<System.Action<string>>(L, 1);
                    
                    PlatformSDK.StartLocation( callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenGaodeMapApp_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float fromLng = (float)LuaAPI.lua_tonumber(L, 1);
                    float fromLat = (float)LuaAPI.lua_tonumber(L, 2);
                    string fromName = LuaAPI.lua_tostring(L, 3);
                    float toLng = (float)LuaAPI.lua_tonumber(L, 4);
                    float toLat = (float)LuaAPI.lua_tonumber(L, 5);
                    string toName = LuaAPI.lua_tostring(L, 6);
                    System.Action<string> callback = translator.GetDelegate<System.Action<string>>(L, 7);
                    
                    PlatformSDK.OpenGaodeMapApp( fromLng, fromLat, fromName, toLng, toLat, toName, callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TakePhonePhoto_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 5&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& translator.Assignable<System.Action<byte[]>>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)) 
                {
                    bool isFromCamera = LuaAPI.lua_toboolean(L, 1);
                    System.Action<byte[]> callback = translator.GetDelegate<System.Action<byte[]>>(L, 2);
                    bool isCut = LuaAPI.lua_toboolean(L, 3);
                    int width = LuaAPI.xlua_tointeger(L, 4);
                    int height = LuaAPI.xlua_tointeger(L, 5);
                    
                    PlatformSDK.TakePhonePhoto( isFromCamera, callback, isCut, width, height );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 4&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& translator.Assignable<System.Action<byte[]>>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    bool isFromCamera = LuaAPI.lua_toboolean(L, 1);
                    System.Action<byte[]> callback = translator.GetDelegate<System.Action<byte[]>>(L, 2);
                    bool isCut = LuaAPI.lua_toboolean(L, 3);
                    int width = LuaAPI.xlua_tointeger(L, 4);
                    
                    PlatformSDK.TakePhonePhoto( isFromCamera, callback, isCut, width );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 3&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& translator.Assignable<System.Action<byte[]>>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    bool isFromCamera = LuaAPI.lua_toboolean(L, 1);
                    System.Action<byte[]> callback = translator.GetDelegate<System.Action<byte[]>>(L, 2);
                    bool isCut = LuaAPI.lua_toboolean(L, 3);
                    
                    PlatformSDK.TakePhonePhoto( isFromCamera, callback, isCut );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& translator.Assignable<System.Action<byte[]>>(L, 2)) 
                {
                    bool isFromCamera = LuaAPI.lua_toboolean(L, 1);
                    System.Action<byte[]> callback = translator.GetDelegate<System.Action<byte[]>>(L, 2);
                    
                    PlatformSDK.TakePhonePhoto( isFromCamera, callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to PlatformSDK.TakePhonePhoto!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenUniWebView_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string url = LuaAPI.lua_tostring(L, 1);
                    
                    PlatformSDK.OpenUniWebView( url );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PickDate_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action<string> callback = translator.GetDelegate<System.Action<string>>(L, 1);
                    
                    PlatformSDK.PickDate( callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PickTime_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action<string> callback = translator.GetDelegate<System.Action<string>>(L, 1);
                    
                    PlatformSDK.PickTime( callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PickDateTime_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action<string> callback = translator.GetDelegate<System.Action<string>>(L, 1);
                    
                    PlatformSDK.PickDateTime( callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowStatusBar_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool isShow = LuaAPI.lua_toboolean(L, 1);
                    bool isWhite = LuaAPI.lua_toboolean(L, 2);
                    
                    PlatformSDK.ShowStatusBar( isShow, isWhite );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 1&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)) 
                {
                    bool isShow = LuaAPI.lua_toboolean(L, 1);
                    
                    PlatformSDK.ShowStatusBar( isShow );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to PlatformSDK.ShowStatusBar!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetStatusBarColor_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
			    int __gen_param_count = LuaAPI.lua_gettop(L);
            
                if(__gen_param_count == 1&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 1)) 
                {
                    bool isWhite = LuaAPI.lua_toboolean(L, 1);
                    
                    PlatformSDK.SetStatusBarColor( isWhite );
                    
                    
                    
                    return 0;
                }
                if(__gen_param_count == 0) 
                {
                    
                    PlatformSDK.SetStatusBarColor(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to PlatformSDK.SetStatusBarColor!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScanQRCode_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Action<string> onScannerMessage = translator.GetDelegate<System.Action<string>>(L, 1);
                    System.Action<string> onScannerEvent = translator.GetDelegate<System.Action<string>>(L, 2);
                    System.Action<string> onDecoderMessage = translator.GetDelegate<System.Action<string>>(L, 3);
                    
                    PlatformSDK.ScanQRCode( onScannerMessage, onScannerEvent, onDecoderMessage );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
