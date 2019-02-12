using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using XLua;

[CSharpCallLua]
public delegate void MessageReceive(uint protoID, byte[] info);
[CSharpCallLua]
public delegate void JavadataMessageReceive(string protoID, string jsonData);

public class NetworkConnect
{
    public enum ConnectError
    {
        None,           //成功
        Connected,      //已连接过了
        NotReachable,   //未连通网络
        SocketError,    //Socket连接出错
        Cancel          //主动取消连接
    }

    /// <summary>服务器名</summary>
    public readonly string serverName;
    /// <summary>IP</summary>
    private string m_ip;
    /// <summary>端口</summary>
    private int m_port;
    private Socket m_socket;
    private ProtoHandler m_protoHandler;

    /// <summary>是否正在连接</summary>
    private bool m_isConnecting = false;
    /// <summary>是否已连接</summary>
    private bool m_isConnected = false;
    public bool IsConnected
    {
        get { return m_isConnected; }
    }

    /// <summary>是否主动断开连接</summary>
    public bool isInitiativeClose = false;

    private Action<ConnectError> m_connectCallback;

    /// <summary>发协议列表</summary>
    private List<Package> m_sendMessageList = new List<Package>();
    /// <summary>收协议队列</summary>
    private MessageQueues<Package> m_recieveMessageQueues = new MessageQueues<Package>();

    public NetworkConnect(string serverName) {
        this.serverName = serverName;
    }

    public bool isCurSocket(Socket socket) {
        return socket == m_socket;
    }
    public Socket getCurrSocket() {
        return m_socket;
    }
    public ProtoHandler getProtoHandler() {
        return m_protoHandler;
    }

    public void Connect(string ip, int port, Action<ConnectError> connectCallback) {
        Debug.Log("begain=================NetworkConnect  Connect");
        m_ip = ip;
        m_port = port;

        if (Application.internetReachability == NetworkReachability.NotReachable) {
            m_isConnecting = false;
            m_isConnected = false;
            Loger.PrintLog("没连通网络");
            if (connectCallback != null)
                connectCallback(ConnectError.NotReachable);
                 return;
        }
        if (m_isConnected || m_isConnecting) {
            Loger.PrintLog("已经连接，无需再次连接");
            if (connectCallback != null)
                connectCallback(ConnectError.Connected);
                return;
        }
        m_connectCallback = connectCallback;
        m_isConnecting = true;
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_socket.NoDelay = true;

        if (IsIP(m_ip)) {
            //用IP连接
            IPEndPoint ePoint = new IPEndPoint(IPAddress.Parse(m_ip), m_port);
            Loger.PrintLog(CommonUtils.ConnectStrs("m_socket.BeginConnect ", m_ip, ":", m_port.ToString()));
            m_socket.BeginConnect(ePoint, ConnectCallback, m_socket);
        }
        else {
            //用域名连接
            IPHostEntry hostInfo = Dns.GetHostEntry(m_ip);
            IPAddress ipAddress = hostInfo.AddressList[0];
            Loger.PrintLog("m_socket.BeginConnect " + m_ip + ":" + m_port);
            m_socket.BeginConnect(ipAddress, m_port, ConnectCallback, m_socket);
        }
        Debug.Log("end=================NetworkConnect  Connect");
    }

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
            m_protoHandler = new ProtoHandler(this, m_socket, RecievePackage);
            JavaNetWorkManager.Instance.m_jsonConnect.protoHandle = m_protoHandler;
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
                Loger.PrintLog("NetworkConnect  Socket_Connect_Succeed");
                if (m_connectCallback != null) {
                    m_connectCallback(ConnectError.None);
                  //  this.accept();
                }
            });
        }
    }
 

    private void StartReceive() {
        if (m_protoHandler != null)
            m_protoHandler.Start();
        else
            Debug.Log("m_protoHandler 未初始化,无法接收");
    }

    /// <summary>
    /// 收协议
    /// </summary>
    public void RecievePackage(uint protoID, byte[] buffer) {
        ThreadManager.RunMainThread(() => {
            if (protoID != 11006)
                Loger.PrintLog(CommonUtils.ConnectStrs("收到协议 ====> ", protoID.ToString()));
        });
        Loger.PrintLog(CommonUtils.ConnectStrs("收到协议 ====> ", protoID.ToString()));
        m_recieveMessageQueues.Enqueue(new Package(protoID, 1, buffer));
    }

    /// <summary>
    /// 发协议
    /// </summary>
    public void SendPackage(Package package) {
        if (!m_isConnected) {
            if (package.ProtoID != 11005)   //过滤心跳包
                DispatchSocketError();
            return;
        }
        m_sendMessageList.Add(package);
    }

    

    public bool ProcessSendPackage(Package package) {
        if (m_protoHandler == null) {
            Debug.Log("m_protoHandler 未初始化，无法发送");
            return false;
        }

        ushort id = (ushort)package.ProtoID;
        if (id != 11005)
            Loger.PrintLog(CommonUtils.ConnectStrs("发送协议 ====> ", id.ToString()));

        byte[] idByte = BitConverter.GetBytes(id);

        int length = 2;
        if (package.ByteArray == null) {
            Loger.PrintError(CommonUtils.ConnectStrs("协议内容为空 ====> ", id.ToString()));
            return false;
        }
        //长度加2
        length = package.ByteArray.Length + 2;

        byte[] lengthByte = BitConverter.GetBytes(length);

        List<byte> all = new List<byte>();

        all.AddRange(lengthByte);

        all.AddRange(idByte);

        if (package.ByteArray != null) {
            all.AddRange(package.ByteArray);
        }
        byte[] info = all.ToArray();
        //		Debug.Log ("#######发送" + id+ "    "+ all.Count);
        return m_protoHandler.Send(info);
    }

    public void OnProcess() {
        if (m_isConnected) {
            if (m_sendMessageList.Count > 0) {
                Package req = m_sendMessageList[0];
                if (req == null) {
                    m_sendMessageList.RemoveAt(0);
                }
                if (ProcessSendPackage(req))
                    m_sendMessageList.RemoveAt(0);
            }
        }
        while (m_recieveMessageQueues.Count > 0) {
            Package response = m_recieveMessageQueues.Dequeue();
            if (response == null)
                break;
            NetworkEventManager.Instance.InvokeCallBack(response.ProtoID, response.ByteArray);
        }
    }

    public void Close(bool isInitiative) {
        m_isConnecting = false;
        m_isConnected = false;
        if (m_socket != null) {
            Debug.Log("CLOSE Socket");
            this.isInitiativeClose = isInitiative;
            m_socket.Close();
        }
    }

    public void Reconnect(Action<ConnectError> connectCallback) {
        //重连用回之前的IP与端口
        Connect(m_ip, m_port, connectCallback);
    }

    public void DispatchSocketError() {
        ThreadManager.RunMainThread(() => {
            Loger.PrintLog(CommonUtils.ConnectStrs("Socket连接错误:", serverName));
            if (m_connectCallback != null)
                m_connectCallback(ConnectError.SocketError);
        });
    }
}