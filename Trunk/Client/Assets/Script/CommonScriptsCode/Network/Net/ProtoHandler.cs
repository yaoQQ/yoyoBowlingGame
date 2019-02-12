using System;
using System.Net.Sockets;
using UnityEngine;

public class ProtoHandler
{
    private enum ProtoState
    {
        head,
        body,
        close,
    }

    private NetworkConnect m_connect;
    private Socket socket;
    private BufferObj bObj = new BufferObj();

    private ProtoState protoState = ProtoState.head;

    private const int headLength = 4;

    private uint protoID;
    //private int returnCodeID=1;

    private byte[] bodyBuffer;
    private int bodyLength;
    private int bodyOffset = 0;


    private byte[] headBuffer = new byte[headLength];
    private int headOffset = 0;



    private bool isSending = false;
    private bool isReceiving = false;
    private SocketAsyncEventArgs socketReceiveAsyncEventArgs;
    private SocketAsyncEventArgs socketSendAsyncEventArgs;
    MessageReceive OnMessageReceived;

    public ProtoHandler(NetworkConnect connect, Socket socket, MessageReceive delegateMessageReceived)
    {
        m_connect = connect;
        this.socket = socket;
        OnMessageReceived = delegateMessageReceived;
    }

    public void Start()
    {
        headBuffer = new byte[headLength];
        socketReceiveAsyncEventArgs = new SocketAsyncEventArgs();
        socketReceiveAsyncEventArgs.SetBuffer(bObj.buffer, 0, bObj.buffer.Length);
        socketReceiveAsyncEventArgs.UserToken = bObj;
        socketReceiveAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ReceiveCallback);

        socketSendAsyncEventArgs = new SocketAsyncEventArgs();
        socketSendAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(SendCallback);

        Receive();
   
    }

    void Close()
    {
        protoState = ProtoState.close;

        socketReceiveAsyncEventArgs.Dispose();
        socketSendAsyncEventArgs.Dispose();
        headBuffer = null;
    }

    public bool Send(byte[] data)
    {
        try
        {
            isSending = true;
            socketSendAsyncEventArgs.SetBuffer(data, 0, data.Length);
            bool sendSuceed = socket.SendAsync(socketSendAsyncEventArgs);// (data, 0, data.Length, SocketFlags.None, new AsyncCallback (SendCallback), socket);
            if (!sendSuceed)
            {
                Loger.PrintWarning("发送错误");
                return false;
            }
        }
        catch (Exception e)
        {
            Loger.PrintError("发送错误:" + e.Message);
            m_connect.Close(false);
            m_connect.DispatchSocketError();
            return false;
        }
        return true;
    }

    void SendCallback(object sender, SocketAsyncEventArgs e)
    {
        //		int length = socket.EndSend (asyncSend);
        isSending = false;
        if (e.SocketError == SocketError.Success)
            Loger.PrintLog("发送成功");
        else
            Loger.PrintLog("SendCallback:" + e.SocketError);
    }

    void Receive()
    {
        isReceiving = true;
        bool succeed = socket.ReceiveAsync(socketReceiveAsyncEventArgs);// (bObj.buffer, 0, bObj.buffer.Length, SocketFlags.None, new AsyncCallback (ReceiveCallback), bObj);
       // Logger.PrintColor("blue", "Receive() bObj.buffer.Length socketReceiveAsyncEventArgs=" + socketReceiveAsyncEventArgs);
      //  Logger.PrintColor("blue", "Receive() succeed=" + succeed);
        if (!succeed)
        {
            Loger.PrintWarning("接收错误");
        }
    }

    void ReceiveCallback(object sender, SocketAsyncEventArgs e)
    {
        if (e.SocketError == SocketError.Success)
        {
            if (sender is Socket)
            {
                if (!m_connect.isCurSocket(sender as Socket))
                    return;
            }
            int length = e.BytesTransferred;
            if (length == 0)
            {
                //服务器要求断开
                PrintLog(UtilMethod.ConnectStrs(m_connect.serverName, " 服务器要求断开"));
                m_connect.Close(false);
                if (!NetworkManager.Instance.isLoginServer(m_connect.serverName))
                    m_connect.DispatchSocketError();
                return;
            }
            //Debug.LogError("length   " + length);
            //Debug.LogError("bObj.buffer.Length   " + bObj.buffer.Length);
       //     Logger.PrintColor("blue", " bObj.buffer.Length=" + bObj.buffer.Length);
            if (length > 0 && length <= bObj.buffer.Length)
            {
                BufferObj bObj = e.UserToken as BufferObj;
                HandleData(bObj.buffer, 0, length);
                isReceiving = false;
                if (protoState != ProtoState.close)
                {
                   
                    // Receive();
                 //  JavaNetWorkManager.Instance.accept(bObj.buffer);
                    Receive();
                }
            }
            else
            {
                PrintError(UtilMethod.ConnectStrs(m_connect.serverName, " 接收数据长度有问题"));
            }
        }
        else if (e.SocketError == SocketError.Interrupted ||
            e.SocketError == SocketError.NotSocket ||
            e.SocketError == SocketError.ConnectionAborted ||
            e.SocketError == SocketError.NotConnected)
        {
            //连接已断开
            PrintLog(UtilMethod.ConnectStrs(m_connect.serverName, " 连接已断开"));
            if (m_connect.isInitiativeClose)
                m_connect.isInitiativeClose = false;
            else
            {
                m_connect.Close(false);
                m_connect.DispatchSocketError();
            }
        }
        else
        {
            PrintError(UtilMethod.ConnectStrs(m_connect.serverName, " Disconnect:", e.SocketError.ToString()));
            m_connect.Close(false);
            if (!NetworkManager.Instance.isLoginServer(m_connect.serverName))
                m_connect.DispatchSocketError();
        }
    }
   
    private void PrintLog(string msg)
    {
        ThreadManager.RunMainThread(() =>
        {
            Loger.PrintLog(msg);
        });
    }

    private void PrintError(string msg)
    {
        ThreadManager.RunMainThread(() =>
        {
            Loger.PrintError(msg);
        });
    }

    void HandleData(byte[] data, int offset, int validLen)
    {
       // Logger.PrintColor("blue", "HandleData() offset=" + offset);
      //  Logger.PrintColor("blue", "HandleData() validLen=" + validLen);
        if (offset >= validLen) return;

        if (protoState == ProtoState.head)
        {
            //Debug.Log("ReadHead ===========================>>>");
            ReadHead(data, offset, validLen);
        }
        else if (protoState == ProtoState.body)
        {
            //Debug.Log("ReadBody ===========================>>>>>");
            ReadBody(data, offset, validLen);
        }
      
    }

    void ReadHead(byte[] data, int offset, int validLen)
    {
     //   Logger.PrintColor("blue", "ReadHead() offset=" + offset);
      //  Logger.PrintColor("blue", "ReadHead() validLen=" + validLen);
        //还没读到的数据长度
        int length = validLen - offset;

        //头需要的长度
        int needLen = headLength - headOffset;

        int readLen = Mathf.Min(needLen, length);
        for (int i = 0; i < readLen; i++)
        {
            headBuffer[headOffset + i] = data[offset + i];
        }
        headOffset += readLen;
        offset += readLen;
     //   Logger.PrintColor("blue", "ReadHead() headLength=" + headLength);
        if (headOffset == headLength)
        {
            //  long len5 = BitConverter.ToInt64(headBuffer, 0);

              bodyLength = BitConverter.ToInt32(headBuffer, 0);
          //  int len = getByteLen(headBuffer);
           // int len = BitConverter.ToInt32(headBuffer, 0);
         // bodyLength = len;
            //ulong len3 = BitConverter.ToUInt64(headBuffer, 0);
            //ushort len4 = BitConverter.ToUInt16(headBuffer, 0);
         //   long len5 = BitConverter.ToInt64(headBuffer, 0);
            //头已读完
        //    Logger.PrintColor("blue", "ReadHead() bodyLength=" + bodyLength);
            headOffset = 0;
            bodyOffset = 0;
            bodyBuffer = new byte[bodyLength];
            this.protoState = ProtoState.body;

            HandleData(data, offset, validLen);
        }
    }

    private int getByteLen(byte[] bytes) {
        int te = 0;
        int count = bytes.Length - 1;
        for (int i = count; i >= 0; i--) {
            int index= bytes[i];
            int curr = count - i;
             int pow = index*paw(curr);
            te = te + pow;
        }

        return te;
    }
    private int paw(int num) {
        int te = 1;
        for (int i = 0; i < num; i++) {
            te = 256 * te;
        }
        return te;
    }

    void ReadBody(byte[] data, int offset, int validLen)
    {
      //  Logger.PrintColor("blue", "ReadBody() offset=" + offset);
     //   Logger.PrintColor("blue", "ReadBody() validLen=" + validLen);
        //还没读到的数据长度
        int length = validLen - offset;

        //body需要的长度
        int needLen = bodyLength - bodyOffset;

        int readLen = Mathf.Min(needLen, length);

        for (int i = 0; i < readLen; i++)
        {
            bodyBuffer[bodyOffset + i] = data[offset + i];
        }
        bodyOffset += readLen;
        offset += readLen;
       
        if (bodyOffset == bodyLength)
        {
            //body已读完
          //  protoID = BitConverter.ToUInt16(GetProtoIDBuff(bodyBuffer), 0);
          //  byte[] protoData = GetProtoDataBuff(bodyBuffer);
           // OnMessageReceived.Invoke(protoID, protoData);
           // Logger.PrintColor("blue", "ReadBody() OnMessageReceived.Invoke  protoData.Length=" + data.Length);
            JavaNetWorkManager.Instance.accept(bodyBuffer);
            headOffset = 0;
            bodyOffset = 0;
            this.protoState = ProtoState.head;
            HandleData(data, offset, validLen);
        }
    }

    void WriteBytes(byte[] data, int start, int lengthLimited, int offset, byte[] target)
    {
        for (int i = 0; i < lengthLimited; i++)
        {
            target[offset + i] = data[start + i];
        }
    }




    byte[] GetProtoIDBuff(byte[] body)
    {
        byte[] target = new byte[2];
        for (int i = 0; i < 2; i++)
        {
            target[i] = body[i];
        }
        return target;
    }
    byte[] GetProtoDataBuff(byte[] body)
    {
        byte[] target = new byte[body.Length-2];
        for (int i = 2; i < body.Length; i++)
        {
            target[i-2] = body[i];
        }
        return target;
    }



}
