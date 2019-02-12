using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class MouseHandler 
{

	public void Execute()
    {

		
        Vector3 inputPos = Input.mousePosition;
        Vector2 percent = new Vector2(inputPos.x / Screen.width, inputPos.y / Screen.height);

        if (Input.GetMouseButton(0))
        {

            if (Input.GetMouseButtonDown(0))
            {
                //UIManager.Instance.PlayerControl.JoystrickControl.OnPressDown(percent);
            }
            else
            {
				//UIManager.Instance.PlayerControl.JoystrickControl.OnPressMove(percent);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
			//UIManager.Instance.PlayerControl.JoystrickControl.OnPressRelease(percent);
        }
    }
    public void OnPressDown(PointerEventData eventData)
    {
        Vector2 percent = new Vector2(eventData.pointerCurrentRaycast.screenPosition.x / Screen.width, eventData.pointerCurrentRaycast.screenPosition.y / Screen.height);
        //UIManager.Instance.PlayerControl.JoystrickControl.OnPressDown(percent);
    }

    public void OnPressMove(PointerEventData eventData)
    {
        Vector2 percent = new Vector2(eventData.pointerCurrentRaycast.screenPosition.x / Screen.width, eventData.pointerCurrentRaycast.screenPosition.y / Screen.height);
        //UIManager.Instance.PlayerControl.JoystrickControl.OnPressMove(percent);
    }
    public void OnPressRelease(PointerEventData eventData)
    {
        Vector2 percent = new Vector2(eventData.pointerCurrentRaycast.screenPosition.x / Screen.width, eventData.pointerCurrentRaycast.screenPosition.y / Screen.height);
        //UIManager.Instance.PlayerControl.JoystrickControl.OnPressRelease(percent);
    }
}
