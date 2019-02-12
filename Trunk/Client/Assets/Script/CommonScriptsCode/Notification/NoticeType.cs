
using UnityEngine;
using System.Collections;

public class NoticeType
{
    public const string Focus_TimeOut = "Focus_TimeOut";

    #region 资源更新

    //热更时出错
    public const string Update_Res_Error = "Update_Res_Error";

    #endregion

    public const string GM_Send_To_MainGateway = "GM_Send_To_MainGateway";
    public const string GM_Send_To_BusinessGateway = "GM_Send_To_BusinessGateway";

    public const string Scene_Click_Event = "Scene_Click_Event";

    public const string Loading_Bar_Show = "Loading_Bar_Show";
    public const string Loading_Bar_Hide = "Loading_Bar_Hide";

    public const string Loading_Annulus_Show = "Loading_Annulus_Show";
    public const string Loading_Annulus_Hide = "Loading_Annulus_Hide";

    #region 心跳

    public const string HeartBeat_Send = "HeartBeat_Send";
    public const string HeartBeat_TimeOut = "HeartBeat_TimeOut";


    #endregion

    #region 登陆

    //debug版本 用于弹出选择服务器 
    public const string Popup_Select_Socket = "Popup_Select_Socket";
    public const string Lua_ConnectSocket = "Lua_ConnectSocket";

    public const string Normal_QuitGame = "Normal_QuitGame";


    //登录
    /// <summary>微信授权成功</summary>
    public const string Login_WxAuthSucceed = "Login_WxAuthSucceed";
    /// <summary>微信授权失败</summary>
    public const string Login_WxAuthFail = "Login_WxAuthFail";
    /// <summary>支付宝授权成功</summary>
    public const string Login_AlipayAuthSucceed = "Login_AlipayAuthSucceed";
    /// <summary>支付宝授权失败</summary>
    public const string Login_AlipayAuthFail = "Login_AlipayAuthFail";

    public const string ConnectSocket = "ConnectSocket";

    public const string Login_SendToRegist = "Login_SendToRegist";

    public const string Login_SendToSverer = "Login_SendToSverer";

    public const string Login_SendForWX = "Login_SendForWX";

    public const string Login_AuthForWX = "Login_AuthForWX";

    public const string Login_SendForWXError = "Login_SendForWXError";
    #endregion;


    public const string Panel_Opened_or_Closed_Window = "Panel_Opened_or_Closed_Window";

    public const string Socket_Server_Connect_Shut_Down = "Socket_Server_Connect_Shut_Down";
    
    /// <summary>录音结束回调</summary>
    public const string Record_End = "Record_End";

    /// <summary>游戏更新进度</summary>
    public const string Game_Update_Progress = "Game_Update_Progress";
    /// <summary>游戏退出</summary>
    public const string Game_Exit = "Game_Exit";
    /// <summary>游戏打开竖版商城</summary>
    public const string Game_Open_Shop = "Game_Open_Shop";
    /// <summary>游戏打开横版商城</summary>
    public const string Game_Open_Shop_Lands = "Game_Open_Shop_Lands";
    /// <summary>点击手机返回按键</summary>
    public const string Device_ReturnEvent = "Device_ReturnEvent";
}
