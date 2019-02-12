using UnityEngine;
using System.Collections;

public class PanelChangeNotice : BaseNotice
{
    public enum ChangeType
    {
        open,
        close
    }
    public override string GetNotificationType()
    {
        return NoticeType.Panel_Opened_or_Closed_Window;
    }

    public ChangeType changeType;

    public UIViewType panelType;

    public UIPanelEnum panelEnum;

}
