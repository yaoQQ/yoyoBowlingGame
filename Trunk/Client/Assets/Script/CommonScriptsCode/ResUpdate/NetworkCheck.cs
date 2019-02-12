using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;

public enum NetworkType
{
    None=0,
    Mobile = 1,
    Wifi=2
}

public class NetworkCheck
{

    public static NetworkType GetNetworkType()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            return NetworkType.None;
        }
        else if(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            return NetworkType.Mobile;
        }
        else if(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            return NetworkType.Wifi;
        }
        return NetworkType.None;
    }



    static int outTimeNum;

    static Thread thread;

    static bool isStop;

    static int timeoutConst = 3000;

    static int timeoutNum;

    static System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
    /*public static void StartPing()
    {
        if (thread != null) return;
        outTimeNum = 0;
        isStop = false;
        string ip = ResUpdateManager.GetInstance().resIP;
        timeoutNum = 0;

        thread = new Thread(delegate ()
        {
            
            while(!isStop)
            {
                
                PingReply reply = ping.Send(ip, timeoutConst);

                if(reply.Status!= IPStatus.Success)
                {
                    timeoutNum++;
                    Loger.PrintError("連線超时 : " + reply.RoundtripTime);
                    if (timeoutNum>=20)
                    {
                        Loger.PrintError("連線失敗 : " + reply.Status.ToString());
                        StopPing();
                        NoticeManager.Instance.Dispatch(NoticeType.Update_Res_Error);
                    }
                   
                }
                else
                {
                    timeoutNum = 0;
                    Loger.PrintLog("连线成功 往返时间： " + reply.RoundtripTime);
                }
            }
        });

        thread.IsBackground = true;
        thread.Start();
    }*/

   
    public static void StopPing()
    {
        isStop = true;
        thread = null;
    }

}
