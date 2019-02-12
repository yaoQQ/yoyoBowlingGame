using UnityEngine;
using System.Collections;

public class ConnectSocketNotice :BaseNotice
{
    public override string GetNotificationType()
    {
        return NoticeType.ConnectSocket;
    }

    public string ip;
    public int port;
}
