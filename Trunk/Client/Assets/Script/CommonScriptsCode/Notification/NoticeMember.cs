using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NoticeMember {
	class ToAddMember
	{
		public OnNoticeLister listener;
		public EventPriority priority;

		public ToAddMember(OnNoticeLister lis,EventPriority pri)
		{
			listener = lis;
			priority = pri;
		}
	}

	LinkedList<OnNoticeLister> listenerLinkedList;
	List<OnNoticeLister> toDeleteList;
	List<ToAddMember> toAddList;
	bool isSending;

	public NoticeMember()
	{
		listenerLinkedList = new LinkedList<OnNoticeLister> ();
		toDeleteList = new List<OnNoticeLister> ();
		toAddList = new List<ToAddMember> ();
	}

	public int ListenerCount
	{
		get
		{
			if(listenerLinkedList!=null)
			{
			return listenerLinkedList.Count;
			}
			return 0;
		}
	}

	public void Add(OnNoticeLister listener, EventPriority priority = EventPriority.Low)
	{
		if (!isSending) {
			ReallyAdd (listener,priority);
		} else {
			toAddList.Add(new ToAddMember(listener,priority));
		}
			

	}

	public void Send(string noticeType, BaseNotice notice)
	{
		if (listenerLinkedList != null) {
			isSending = true;
			foreach (OnNoticeLister listener in listenerLinkedList) {
				listener (noticeType, notice);
			}
		} 
		isSending = false;

		DeleteCache ();
		AddCache ();
	}

	public void Remove(OnNoticeLister listener)
	{
		if (!isSending) {
			ReallyRemove (listener);
		} else {
			toDeleteList.Add(listener);
		}
	}

	void ReallyRemove(OnNoticeLister listener)
	{
		if (listenerLinkedList != null) {
			if (listenerLinkedList.Contains (listener)) {
				listenerLinkedList.Remove (listener);
			}
		}
	}

	void ReallyAdd(OnNoticeLister listener, EventPriority priority = EventPriority.Low)
	{
		if (listenerLinkedList != null) {
			if (!listenerLinkedList.Contains (listener)) {
				switch (priority) {
				case EventPriority.Height:
				{
					listenerLinkedList.AddFirst (listener);
				}
					break;
				case EventPriority.Low:
				{
					listenerLinkedList.AddLast (listener);
				}
					break;
				}
			}
		}
	}

	
	void DeleteCache()
	{
		if (toDeleteList.Count > 0) {
			for (int i =0; i<toDeleteList.Count; i++) {
				ReallyRemove(toDeleteList [i]);
			}
			toDeleteList.Clear ();
		}
	}
	
	void AddCache()
	{
		if (toAddList.Count > 0) {
			for (int i =0; i<toAddList.Count; i++) {
				ReallyAdd(toAddList[i].listener,toAddList[i].priority);
			}
			toAddList.Clear ();
		}
	}

}
