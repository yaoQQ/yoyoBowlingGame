using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testUGIPos : MonoBehaviour {

    // Use this for initialization
    public GameObject midUI;
    public Camera UICameara;
    public Text myText;
    public Text myText2;
    private RectTransform rectTran;

    void Start () {
         rectTran = midUI.transform as RectTransform;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0)) {

            Debug.Log("mouse up");
          Vector3  screenPos = UICameara.WorldToScreenPoint(rectTran.transform.position);
            Debug.Log("screenPos="+ screenPos);
             
        }
      bool isInRect=  RectTransformUtility.RectangleContainsScreenPoint(rectTran, new Vector2(Input.mousePosition.x, Input.mousePosition.y), UICameara);
        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTran, new Vector2(Input.mousePosition.x, Input.mousePosition.y), UICameara,out worldPos);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTran, new Vector2(Input.mousePosition.x, Input.mousePosition.y), UICameara, out localPoint);
        if (isInRect) {
            Debug.Log("isInRect!!!!");
        }
        myText.text = "screenPos=" + Input.mousePosition;
        myText2.text = "worldPos=" + worldPos;
        rectTran.position = worldPos;
    }
}
