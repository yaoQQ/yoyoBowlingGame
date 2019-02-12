using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProtoBufSpace;

public class CommonModule : BaseModule
{

    public override ModuleEnum ModuleName()
    {
        return ModuleEnum.Common;
    }
    public override void InitRegisterNet()
    {
       
    }
    public override void OnNetMsgLister(uint protoID, byte[] buffer)
    {
       
    }

    public override List<string> GetRegisterNotificationList()
    {
        if (notificationList == null)
        {
            notificationList = new List<string>();
            notificationList.Add(NoticeType.Normal_QuitGame);
        }
        return notificationList;
    }

    public override void OnNotificationLister(string noticeType, BaseNotice notice)
    {
        ObjectNotice onValue = notice as ObjectNotice;
        switch (noticeType)
        {
            case NoticeType.Normal_QuitGame:
                Driver.Instance.QuitGame();
                break;
        }
    }
}
