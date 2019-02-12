using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIExLongClickListener : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public Action<PointerEventData> onHandler;
    public static UIExLongClickListener Get(GameObject go)
    {
        UIExLongClickListener listener = go.GetComponent<UIExLongClickListener>();
        if (listener == null) listener = go.AddComponent<UIExLongClickListener>();
        return listener;
    }

    [Tooltip("长按界定时间")]
    public const float longDefinition = 0.5f;
    private bool isLongPressTriggered = false;
    private bool isPointerDown = false;
    private float timePressStarted;
    private PointerEventData m_eventData;

    private bool m_isDragging = false;
    public bool isDragging
    {
        get { return m_isDragging; }
        set { m_isDragging = value; }
    }

    public bool IsLongPressTriggered
    {
        get { return isLongPressTriggered; }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        m_eventData = eventData;
        timePressStarted = Time.unscaledTime;
        isLongPressTriggered = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        m_eventData = null;
        timePressStarted = 0f;
    }

    void Update()
    {
        if (isPointerDown && !isLongPressTriggered)
        {
            var curTime = Time.unscaledTime;
            if (curTime - timePressStarted > longDefinition)
            {
                isLongPressTriggered = true;

                if (onHandler != null && !m_isDragging && m_eventData != null)
                    onHandler.Invoke(m_eventData);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("鼠标离开");
        m_eventData = null;
    }

}
