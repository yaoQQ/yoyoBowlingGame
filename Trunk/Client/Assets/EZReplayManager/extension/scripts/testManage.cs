using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;

[LuaCallCSharp]
public class testManage : MonoBehaviour
{

    // Use this for initialization
    // public GameObject firstCamera;
    public Material lineMaterial;
    public bool isDraw3D = false;
    public GameObject begainObj;
    public GameObject endObj;
    void Start() {
        //  EZReplayManager.mark4Recording(gameObject, false, "asdas");
        // this.transform.localEulerAngles
         initStageView();
    }
    private bool isinit = false;
    public void showStage() {
        initStageView();
    }
    private void initStageView() {
        if (isinit) {
            return;
        }
     // this.transform.parent.gameObject.SetActive(false) = 

        stageList.Add(Vector3.zero);
        stageList.Add(new Vector3(Screen.width, 0, 0));
        stageList.Add(new Vector3(Screen.width, Screen.height, 0));
        stageList.Add(new Vector3(0, Screen.height, 0));
        stageList.Add(Vector3.zero);
        stageList.Add(new Vector3(0, 0, 100));
        stageList.Add(new Vector3(Screen.width, 0, 100));
        stageList.Add(new Vector3(Screen.width, Screen.height, 100));
        stageList.Add(new Vector3(0, Screen.height, 100));
        stageList.Add(new Vector3(Screen.width, Screen.height, 100));

        //int count = stageList.Count - 1;
        //for (int i = 0; i <= count; i++) {
        //    Text te = getText();
        //    te.transform.position = stageList[i];
        //    te.text = (stageList[i]).ToString();
        //}
        isinit = true;
    }
    public Text posText;
    private Text getText() {
        Text clone=null;
        if (posText) {
            clone = GameObject.Instantiate(posText);
            clone.transform.parent = posText.transform.parent;
        }
        return clone;
    }

    public List<Vector3> stageList = new List<Vector3>();
    //构建模拟数据场景
    private void showDrawStage() {
        int count = stageList.Count - 1;
        for (int i = 0; i < count; i++) {
            Vector3 pos = stageList[i];
            GL.Vertex3(pos.x, pos.y, pos.z);
            GL.Vertex3(stageList[i + 1].x, stageList[i + 1].y, stageList[i + 1].z);
        }
    }
    public void OnRenderObject() {
        if (stageList.Count <= 0) {
            return;
        }
        drawLine();
    }



    private int inter = 1;
    private void drawLine() {
        //  if (isPress) {
        GL.PushMatrix();

        lineMaterial.SetPass(0);
        if (!isDraw3D) {
            GL.LoadOrtho();
        }
        GL.Begin(GL.LINES);
        showDrawStage();
        int count = posList.Count - 1;
        for (int i = 0; i < count; i++) {
            Vector3 pos = posList[i];
            GL.Vertex3(pos.x, pos.y, pos.z);
            GL.Vertex3(posList[i + 1].x, posList[i + 1].y, posList[i + 1].z);
        }
        if (count >= 0) {
            begainObj.transform.position = posList[0];
            endObj.transform.position = posList[count];
        }
        GL.End();
        GL.PopMatrix();

        // }
    }

    // Update is called once per frame
    private bool isPress = false;
    private Vector3 curPos = Vector3.zero;
    public List<Vector3> posList = new List<Vector3>();
    private void clearLines() {
        curPos = Vector3.zero;
        isPress = false;
        int count = posList.Count;
        int middle = (int)(count / 2);

        Vector3 begainVec = posList[0];
        Vector3 middleVec = posList[middle];
        Vector3 endVec = posList[posList.Count - 1];

        float dis = Vector3.Distance(begainVec, endVec) / 2;
        float mDis = Vector3.Distance(begainVec, middleVec);

        float angel = Vector3.Angle(begainVec, middleVec);
        float angel3 = Mathf.Asin(dis / mDis) * Mathf.Rad2Deg;

        Debug.Log("posList.count=" + count);
        Debug.Log("<color='red'>posList.Count=" + count + "</color>");
        Debug.Log("<color='red'>begain Vec=" + begainVec + "</color>");
        Debug.Log("<color='red'>begain Vec=" + middleVec + "</color>");
        Debug.Log("<color='red'>end Vec=" + endVec + "</color>");
        Debug.Log("<color='red'>end angel=" + angel + "</color>");
        Debug.Log("<color='red'>end angel3=" + angel3 + "</color>");

       
        posList.Clear();
    }
    public void addScanPos(Vector3 pos) {
        posList.Add(pos);
    }
    private void addMovePos() {
        if (Vector3.Distance(Input.mousePosition, curPos) > inter) {
            curPos = Input.mousePosition;
            Vector3 vec;
            if (!isDraw3D) {
                vec = new Vector3(curPos.x / Screen.width, curPos.y / Screen.height, 0);
            }
            else {
                vec = new Vector3(curPos.x, curPos.y, posList.Count);
            }
            posList.Add(vec);
            Debug.Log(" posList.Add() vec=" + vec);

            //Sequence sequ = DOTween.Sequence();
            //sequ.SetDelay(1);
            //this.transform.localPosition
            Tweener test=this.transform.DOLocalRotate(Vector3.zero,1);
           RectTransform rect= this.transform.GetComponent<RectTransform>();
            //rect.
            //test.SetLoops(-1);
            test.CompletedLoops();
         //   this.transform.rotation.z = 
            Tweener test2 = this.transform.DORotate(Vector3.zero, 1);
             Sequence  test3=  DOTween.Sequence();
            RectTransform test4;
         //   this.transform.localPosition =
            //test3.onUpdate()
           // test3.Append()
            //test2.SetEase(26);
            //  this.transform.do
            // this.transform.GetComponent<RectTransform>();
            // RectTransformUtility.ScreenPointToLocalPointInRectangle(
            //sequ.onComplete
            // Tweener test = this.transform.DOLocalMove(Vector3.zero, 0);

            // test.SetDelay
            // sequ.SetDelay
            // sequ.onComplete
            // sequ.AppendCallback()
        }
    }
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            isPress = true;
        }
        if (Input.GetKeyUp("c")) {
            clearLines();
        }
        if (Input.GetMouseButtonUp(0)) {
            isPress = false;
            //clearLines();
        }
        if (isPress) {

        //    addMovePos();

        }

    }
}
