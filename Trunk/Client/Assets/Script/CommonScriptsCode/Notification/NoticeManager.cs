
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using XLua;

public enum EventPriority
{
	Height,
	Low,
}

[CSharpCallLua]
public delegate void OnNoticeLister (string noticeType, BaseNotice notice);

[LuaCallCSharp]
public class NoticeManager : Singleton<NoticeManager>
{													

	public void Dispatch (string noticeType )
	{
		
		ModuleManager.Instance.ExecuteNotificationHandle (noticeType, null);
		ExecuteHandlerList (noticeType, null);
	}
    public void Dispatch(BaseNotice notice)
    {
       
        ModuleManager.Instance.ExecuteNotificationHandle(notice.GetNotificationType(), notice);
        ExecuteHandlerList(notice.GetNotificationType(), notice);
    }

    public void Dispatch (string noticeType, System.Object notice)
	{
        BaseNotice bn;
        if(notice is BaseNotice)
        {
            bn = notice as BaseNotice;
        }
        else
        {
            bn = new ObjectNotice(noticeType, notice);
        }
		ModuleManager.Instance.ExecuteNotificationHandle (noticeType, bn);
		ExecuteHandlerList (noticeType, bn);
	}

	Dictionary<string, NoticeMember> handlerDic = new Dictionary<string, NoticeMember> ();

	public void AddNoticeLister (string noticeType, OnNoticeLister onHandler, EventPriority priority = EventPriority.Low)
	{
		if (!handlerDic.ContainsKey (noticeType)) {
			handlerDic.Add (noticeType, new NoticeMember ());
		}
			handlerDic [noticeType].Add (onHandler,priority);

	}

	public void RemoveNoticeLister (string noticeType, OnNoticeLister onHandler)
	{
		if (!handlerDic.ContainsKey (noticeType)) {
			return ;
		}
		handlerDic [noticeType].Remove (onHandler);
		NoticeMember nm= handlerDic [noticeType];
		nm.Remove (onHandler);
		CheckIfCanRemoveFromDic (noticeType, nm);
	}

	void ExecuteHandlerList (string noticeType, BaseNotice notice)
	{
		if (!handlerDic.ContainsKey (noticeType)) {
			return;
		}
		NoticeMember nm = handlerDic [noticeType];

		if (nm != null) {
			nm.Send(noticeType, notice);
			CheckIfCanRemoveFromDic (noticeType, nm);
		} 
	}

	void CheckIfCanRemoveFromDic(string noticeType, NoticeMember nm)
	{
		if (nm.ListenerCount == 0) {
			nm = null;
			handlerDic.Remove (noticeType);
		}

	}
}


