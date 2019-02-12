using UnityEngine;
using System;
using System.Collections;
//using ProtoBufSpace;

public delegate void TimeOutCallBack();

public class HeartBeat
{
    const int Send_HeartBeatID=10003;
    const int Receive_HeartBeatID = 20003;

    private int timeout;

    //单位是秒
	//3s发一次 6秒没返回为timeout
	const int interval = 10;

	float lastUpdateTime;
	float lastwaitTimeresetTime;
	float waitTime;
	
	public TimeOutCallBack OnTimeOut;

    bool sign = false;
	
	public HeartBeat( TimeOutCallBack delegateTimeOut)
	{

		this.OnTimeOut = delegateTimeOut;
	}
	

	internal void resetTimeout()
	{
		this.timeout = 0;
		lastUpdateTime = 0;
		waitTime = 0;
		lastwaitTimeresetTime = Time.time;
	}
	

	public void Start()
	{
        if(!sign)
        {
            sign = true;
            Loger.PrintLog("==================注册心跳侦听=================");
            NetworkEventManager.Instance.RegisterEventHandler(Receive_HeartBeatID, OnReceveHander);
            timeout = 0;
            waitTime = 0;
            lastUpdateTime = 0;
            lastwaitTimeresetTime = Time.time;
        }
        
	}        
	

	public void Stop()
	{
        if(sign)
        {
            sign = false;
            NetworkEventManager.Instance.RemoveEventHandler(Receive_HeartBeatID, OnReceveHander);
        }
       
    }

	public void OnProcess()
	{
        if(sign)
        {
            if (Time.time - lastUpdateTime >= interval)
            {
                SendHeartBeat();
                lastUpdateTime = Time.time;
            }
        }
		
	}
	

	private void SendHeartBeat()
	{
        if (sign)
        {
            Loger.PrintLog("===================SendHeartBeat==================");
            waitTime = Time.time - lastwaitTimeresetTime;
            if (waitTime > interval * 2)
            {
                //超时不会再发心跳
                if (OnTimeOut != null) OnTimeOut();
                return;
            }

            //Package p = new Package (TProtocol.PingReq, null);
            //NetworkManager.Instance.SendMessage (p);

            //lua那边发送心跳
            NoticeManager.Instance.Dispatch(NoticeType.HeartBeat_Send);
        }
           

    }

	private void ReceiveHeartBeat()
	{
        Loger.PrintLog("===================Receive HeartBeat==================");
        timeout = 0;
		waitTime = 0;
		lastwaitTimeresetTime = Time.time;
	}

    void OnReceveHander(uint protoID, byte[] buffer)
    {
       
        ReceiveHeartBeat();
    }
}
