using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class JavaDataProtocol
{

    public static string loginReq = "loginReq";//登录请求
    public static string loginRsp = "loginRsp";//登录返回

    public static string goodsListReq = "goodsListReq";//商品列表请求
    public static string goodsListRsp = "goodsListRsp";//商品列表返回

    public static string cargolaneReq = "cargolaneReq";//货道列表请求
    public static string cargolaneRsp = "cargolaneRsp";//货道列表返回

    public static string apkVersionRsp = "apkVersionRsp";// 发送APK版本号 返回

    public static string orderReq = "orderReq";//下单请求
    //@1 "type" : "startGame" 返回（游戏夺宝）: 游戏开始通知  @2 "type" : "deliveryGoods" 返回（直接购买）：出货结果通知
    public static string orderRsp = "orderRsp";


    public static string reportGameResultReq = "reportGameResultReq";//游戏结果上报 请求(游戏结束时调用)
    public static string reportGameResultRsp = "reportGameResultRsp";//@1返回(网络请求失败)  @2返回（游戏通关）：出货结果通知

}

