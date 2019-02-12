using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBowling : MonoBehaviour
{

    // Use this for initialization
    void Start() {
        //Screen.height 
    }

    // Update is called once per frame
    private Vector3 currPos = Vector3.zero;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            currPos = Input.mousePosition;
            Debug.Log("Input.mousePosition=" + Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0)) {
            Vector3 pos = currPos;
            Debug.Log("pos=" + pos);
            Vector3 Vec2 = new Vector3(0, 1, 0);
            float angel = Vector3.Angle(Vec2, pos);
            Debug.Log("angel =" + angel);
            Debug.Log("Vector3.up =" + Vector3.up);
            Debug.Log("Vector3.forward =" + Vector3.forward);
        }
    }

    public void OnGUI() {
        if (GUI.Button(new Rect(0, 0, 100, 100), "testAngel1")) {
            if(Input.GetMouseButtonUp(0)){
                Vector3 pos = Input.mousePosition;
                    Debug.Log("pos=" + pos);
                Vector3 Vec2 = new Vector3(0, 1,0);
                float angel = Vector3.Angle(Vec2, pos);
                Debug.Log("angel =" + angel);
                Debug.Log("Vector3.up =" + Vector3.up);
                Debug.Log("Vector3.forward =" + Vector3.forward);
            }
        }
        if (GUI.Button(new Rect(0, 100, 100, 100), "testAngel2")) {
            Vector3 pos = Input.mousePosition;
            Debug.Log("pos=" + pos);
            Vector3 Vec2 = new Vector3(0, 1, 0);
            float angel = Vector3.Angle(pos, Vec2);
            Debug.Log("angel =" + angel);
        }
        if (GUI.Button(new Rect(0, 200, 100, 100), "testAngel3")) {

        }
    }
}
