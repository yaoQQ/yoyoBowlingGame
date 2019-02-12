using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !TOOL
using XLua;
[CSharpCallLua]
#endif
public class PinchEventData
{
    public PinchEventData(Vector2 _position, float _delta)
    {
        this.centerPosition = _position;
        this.delta = _delta;
    }

    public Vector2 centerPosition;
    public float delta;
}


public class UIExPinchListener : MonoBehaviour
{
    public static UIExPinchListener Get(GameObject go)
    {
        UIExPinchListener listener = go.GetComponent<UIExPinchListener>();
        if (listener == null) listener = go.AddComponent<UIExPinchListener>();
        return listener;
    }



    public Action<PinchEventData> onPinchInHandler;
    public Action<PinchEventData> onPinchOutHandler;


    void Update()
    {
        ProcessPinchEvent();
    }
    void ProcessPinchEvent()
    {
        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            var center = new Vector2(Mathf.Abs(touchZero.position.x - touchOne.position.x) / 2, Mathf.Abs(touchZero.position.y - touchOne.position.y) / 2);
            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            //张开时, delta为负数, 收紧时为正数
            if (deltaMagnitudeDiff > 0)
            {
                if (onPinchInHandler != null)
                {
                    onPinchInHandler.Invoke(new PinchEventData(center, deltaMagnitudeDiff));
                }
            }
            else if(deltaMagnitudeDiff < 0)
            {
                if (onPinchOutHandler != null)
                {
                    onPinchOutHandler.Invoke(new PinchEventData(center, deltaMagnitudeDiff));
                }
            }

        }
    }
}
