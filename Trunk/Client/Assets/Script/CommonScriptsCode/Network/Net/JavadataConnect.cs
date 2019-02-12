using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using XLua;

public class JavadataConnect
{
    public enum ConnectError
    {
        None,           //成功
        Connected,      //已连接过了
        NotReachable,   //未连通网络
        SocketError,    //Socket连接出错
        Cancel          //主动取消连接
    }

    ///// <summary>IP</summary>
    //private string m_ip;
    ///// <summary>端口</summary>
    //private int m_port;
    //private Socket m_socket;
    //private ProtoHandler m_protoHandler;
    private Socket m_socket;
    private ProtoHandler m_protoHandle;
    /// <summary>是否正在连接</summary>
    private bool m_isConnecting = false;
    /// <summary>是否已连接</summary>
    private bool m_isConnected = true;
    public bool IsConnected
    {
        get { return m_isConnected; }
    }

    /// <summary>是否主动断开连接</summary>
    public bool isInitiativeClose = false;

    private Action<ConnectError> m_connectCallback;

    /// <summary>发协议列表</summary>
    private List<JsonPackage> m_sendMessageList = new List<JsonPackage>();
    /// <summary>收协议队列</summary>
    private MessageQueues<JsonPackage> m_recieveMessageQueues = new MessageQueues<JsonPackage>();

    public bool isCurSocket(Socket socket) {
        return true;
    }

    public void Connect(string ip, int port, Action<ConnectError> connectCallback) {
        //m_ip = ip;
        //m_port = port;

    }
    public ProtoHandler protoHandle
    {
        get
        {
            return m_protoHandle;
        }
        set
        {
            m_protoHandle = value;
        }
    }
    public Socket socket
    {
        get
        {
            return m_socket;
        }
        set
        {
            m_socket = value;
          //  accept();
           //  m_protoHandler = new ProtoHandler(this, m_socket, RecievePackage);
           // Receive();
         
           Logger.PrintColor("blue","JavadataConnect() set m_socket="+ m_socket);
        }

    }
    
  
    
   
 

    private BufferObj bObj = new BufferObj();
    private SocketAsyncEventArgs socketReceiveAsyncEventArgs;
  
    private bool IsIP(string str) {
        for (int i = 0, len = str.Length; i < len; ++i) {
            if (str[i] == '.' || (str[i] >= '0' && str[i] <= '9'))
                continue;
            return false;
        }
        return true;
    }

    private void ConnectCallback(IAsyncResult ar) {
        try {
            m_isConnecting = false;
            m_isConnected = true;
            Socket handler = (Socket)ar.AsyncState;
            handler.EndConnect(ar);
          //  m_protoHandler = new ProtoHandler(this, m_socket, RecievePackage);
            StartReceive();
        }
        catch (SocketException ex) {
            m_isConnecting = false;
            m_isConnected = false;
            Debug.LogError("Socket连接出错(" + ex.SocketErrorCode + "):" + ex.Message);

            DispatchSocketError();
        }
        if (m_isConnected) {
            ThreadManager.RunMainThread(() => {
                Loger.PrintLog("JavadataConnect Socket_Connect_Succeed");
                if (m_connectCallback != null) {
                    m_connectCallback(ConnectError.None);
                   // this.accept();
                }
            });
        }
    }

    private void StartReceive() {
        //if (m_protoHandler != null)
        //    m_protoHandler.Start();
        //else
        //    Debug.Log("m_protoHandler 未初始化,无法接收");
    }

    /// <summary>
    /// 收协议
    /// </summary>
    public void RecievePackage(string protoID, string jsonData) {
        //ThreadManager.RunMainThread(() => {
        //    if (protoID != 11006)
        //        Loger.PrintLog(CommonUtils.ConnectStrs("收到协议 ====> ", protoID.ToString()));
        //});
        m_recieveMessageQueues.Enqueue(new JsonPackage(protoID, 1, jsonData));
    }

    /// <summary>
    /// 发协议
    /// </summary>
    public void SendPackage(JsonPackage package) {
        if (!m_isConnected) {
            //if (package.ProtoID != 11005)   //过滤心跳包
            //    DispatchSocketError();
            return;
        }
        m_sendMessageList.Add(package);
    }

    /// <summary>
    /// 发送json数据到java端
    /// </summary>
    /// <param name="package"></param>
    /// <returns></returns>
    public bool ProcessSendPackage(JsonPackage package) {
        //if (m_protoHandler == null) {
        //    Debug.Log("@@@Java JsonData m_protoHandler 未初始化，无法发送");
        //    return false;
        //}

        string id = package.ProtoID;
        Loger.PrintLog(CommonUtils.ConnectStrs("@@@ProcessSendPackage Java JsonData 发送协议 ====> ", id.ToString()));
        if (string.IsNullOrEmpty(package.jsonData)) {
            Loger.PrintError(CommonUtils.ConnectStrs("@@@ProcessSendPackage Java JsonData 协议内容为空 ====> ", id.ToString()));
            return false;
        }

       // string info = package.jsonData;
       // Debug.Log ("ProcessSendPackage() info发送 package=" + package + "    package.jsonData=" + package.jsonData);

        //  NetworkManager.Instance.SendMsgToJavaMessage(package);
       return sendJson(package);
        //return AndroidSDK.Instance.setU3DToJava(package.jsonData);
    }

    private bool sendJson(JsonPackage package) {
        byte[] buffer;
       // Logger.PrintColor("blue", "@@@@@@  sendJson() package.jsonData=" + package.jsonData);
        buffer = System.Text.Encoding.Default.GetBytes(package.jsonData);
        //    socket.Send(buffer);
        if (m_protoHandle != null) {
            m_protoHandle.Send(buffer);
        }
        else {
            Debug.Log("<color='green'>@@@@@@@@sendJson(JsonPackage package) m_protoHandle=null</color>");
            Debug.Log("<color='green'>@@@@@@@@sendJson(JsonPackage package) package.jsonData=" + package.jsonData+ "</color>");
        }
        return true;
    }

    public void OnProcess() {
        if (m_isConnected) {
            if (m_sendMessageList.Count > 0) {
                JsonPackage req = m_sendMessageList[0];
                if (req == null) {
                    m_sendMessageList.RemoveAt(0);
                }
                //  if (ProcessSendPackage(req))  java通信不管成功失败都发送一次
                ProcessSendPackage(req);
                m_sendMessageList.RemoveAt(0);
            }
        }
        while (m_recieveMessageQueues.Count > 0) {
            JsonPackage response = m_recieveMessageQueues.Dequeue();
            if (response == null)
                break;
            JavaNetWorkManager.Instance.InvokeCallBack(response.ProtoID, response.jsonData);
        }
    }

    public void Close(bool isInitiative) {
        m_isConnecting = false;
        m_isConnected = false;
        //if (m_socket != null) {
        //    Debug.Log("CLOSE Socket");
        //    this.isInitiativeClose = isInitiative;
        //    m_socket.Close();
        //}
    }

    public void Reconnect(Action<ConnectError> connectCallback) {
        //重连用回之前的IP与端口
      //  Connect(m_ip, m_port, connectCallback);
    }

    public void DispatchSocketError() {
        ThreadManager.RunMainThread(() => {
         //   Loger.PrintLog(CommonUtils.ConnectStrs("Socket连接错误:", serverName));
            if (m_connectCallback != null)
                m_connectCallback(ConnectError.SocketError);
        });
    }
}