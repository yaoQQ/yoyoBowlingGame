using System.Collections.Generic;
using ProtoBufSpace;
using ProtoBuf;


public abstract class BaseModule
{

    public BaseModule()
    {
        InitRegisterNet();
    }

    public abstract ModuleEnum ModuleName();

    protected List<string> notificationList;

    protected Dictionary<uint, MessageReceive> netTProtocolIDData = new Dictionary<uint, MessageReceive>();

    public abstract List<string> GetRegisterNotificationList();


    public abstract void OnNotificationLister(string noticeType, BaseNotice notice);

    public abstract void InitRegisterNet();


    public void RegisterNetMsg(TProtocol protoID)
    {
        uint protoIDValue = (uint)protoID;
        netTProtocolIDData.Add(protoIDValue, OnNetMsgLister);
        NetworkEventManager.Instance.RegisterEventHandler(protoIDValue, OnNetMsgLister);
    }

    public void RegisterNetMsg(uint protoID)
    {
        netTProtocolIDData.Add(protoID, OnNetMsgLister);
        NetworkEventManager.Instance.RegisterEventHandler(protoID, OnNetMsgLister);
    }

    public abstract void OnNetMsgLister(uint protoID, byte[] buffer);

    public void SendNetMsg(string serverName, uint protoID, IExtensible msg)
    {
        Package package = new Package(protoID, (int)ReturnCode.Success, ProtobufTool.PSerializer(msg));
        NetworkManager.Instance.SendMessage(serverName, package);
    }

    public bool RemoveNetMsg(uint protoID)
    {
        MessageReceive mr;
        if (netTProtocolIDData.TryGetValue(protoID, out mr))
        {
            NetworkEventManager.Instance.RemoveEventHandler(protoID, mr);
            netTProtocolIDData.Remove(protoID);
            return true;
        }
        return false;
    }

    public void resetModule()
    {
        if(notificationList!=null)
        {
            notificationList.Clear();
            notificationList = null;
        }
        foreach (KeyValuePair<uint, MessageReceive> kvp in netTProtocolIDData )
        {
            NetworkEventManager.Instance.RemoveEventHandler(kvp.Key, kvp.Value);
        }
        netTProtocolIDData.Clear();
    }
}
