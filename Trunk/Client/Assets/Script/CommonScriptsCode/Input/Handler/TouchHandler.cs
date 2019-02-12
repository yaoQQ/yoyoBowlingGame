using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TouchHandler 
{

	public void Execute()
	{
		
        

		for (int i =0;i<Input.touches.Length;i++) {
			Touch touch = Input.touches[i];
			Vector3 inputPos = touch.position;
			Vector2 percent = new Vector2(inputPos.x / Screen.width, inputPos.y / Screen.height);

			if(touch.phase==TouchPhase.Began)
			{
				//UIManager.Instance.PlayerControl.JoystrickControl.OnTouchDown(percent,touch.fingerId);
			}
			else if(touch.phase==TouchPhase.Moved)
			{
				//UIManager.Instance.PlayerControl.JoystrickControl.OnTouchMove(percent,touch.fingerId);
			}
			else if(touch.phase==TouchPhase.Ended)
			{
				//UIManager.Instance.PlayerControl.JoystrickControl.OnTouchRelease(percent,touch.fingerId);
			}
			else
			{
				//UIManager.Instance.PlayerControl.JoystrickControl.OnTouchMove(percent,touch.fingerId);
			}
		}
	}

    public void OnTouchDown(PointerEventData eventData)
    {
        Vector2 percent = new Vector2(eventData.pointerCurrentRaycast.screenPosition.x / Screen.width, eventData.pointerCurrentRaycast.screenPosition.y / Screen.height);
        //UIManager.Instance.PlayerControl.JoystrickControl.OnTouchDown(percent, eventData.pointerId);
    }

    public void OnTouchMove(PointerEventData eventData)
    {
        Vector2 percent = new Vector2(eventData.pointerCurrentRaycast.screenPosition.x / Screen.width, eventData.pointerCurrentRaycast.screenPosition.y / Screen.height);
        //UIManager.Instance.PlayerControl.JoystrickControl.OnTouchMove(percent, eventData.pointerId);
    }
    public void OnTouchRelease(PointerEventData eventData)
    {
        Vector2 percent = new Vector2(eventData.pointerCurrentRaycast.screenPosition.x / Screen.width, eventData.pointerCurrentRaycast.screenPosition.y / Screen.height);
        //UIManager.Instance.PlayerControl.JoystrickControl.OnTouchRelease(percent, eventData.pointerId);
    }
}
