using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class SceneCamera :MonoBehaviour
{

    public string cameraName;

    public SceneCameraInfo cameraInfo;

    public Camera cam;

    Vector2 viewableSize = Vector2.zero;

    float curAspect;

    //相机orthographicSize的最小变化值
    float offsetValue;

    //相机orthographicSize变化后的最小改变量
    float stepValue;

    public void SetViewableSize(float w, float h)
    {
        viewableSize = new Vector2(w, h);
        UpdateViewableSize();
    }
    public void SetViewableSize(Vector2 v2)
    {
        viewableSize = v2;
        UpdateViewableSize();
    }

    void Awake()
    {      
       
    }

    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    void UpdateViewableSize()
    {      
        if (cam == null) { return; }

        if (cam.orthographic)
        {
            float orthographicSize = cam.orthographicSize;

            float cameraHeight = orthographicSize * 2;
            float cameraWidth = cameraHeight * curAspect;


            orthographicSize = viewableSize.x / (2 * curAspect);
            if(orthographicSize != 0)
            {
                cam.orthographicSize = orthographicSize;
            }
           
        }
    }

    public void SetTargetTexture(RenderTexture tex)
    {
        this.cam.targetTexture = tex;
    }
    public void SetSceneCameraPos(float offset,float step)
    {
        offsetValue = offset;
        stepValue = step;
        UpdateSceneCamreaPos();       
    }

    void UpdateSceneCamreaPos()
    {
        if (offsetValue == 0 || cam == null)
            return;

        float v = cameraInfo.posVector.y + (cam.orthographicSize - cameraInfo.orthographicSize)/ offsetValue * stepValue;      
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, v, gameObject.transform.position.z);
    }

    void Update()
    {
        if (cam == null) return;
        if (curAspect != cam.aspect)
        {
            curAspect = cam.aspect;
            UpdateViewableSize();
            UpdateSceneCamreaPos();
        }
    }

    public Texture2D GetSnapshot(int w, int h)
    {
        if (cam == null) return null;
        RenderTexture tempRT = RenderTexture.GetTemporary(w, h);
        cam.targetTexture = tempRT;
        cam.Render();
        Texture2D tex2d = new Texture2D(w, h, TextureFormat.RGBA32, false);
        RenderTexture.active = cam.targetTexture;
        tex2d.ReadPixels(new Rect(0, 0, w, h), 0, 0);
        tex2d.Apply();
        cam.targetTexture = null;
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(tempRT);
        tempRT = null;
        return tex2d;
    }

    public Texture2D GetSnapshot(int w)
    {
        if (cam == null) return null;
        int h = Mathf.FloorToInt((float)w / cam.aspect);
        RenderTexture tempRT = RenderTexture.GetTemporary(w, h);
        cam.targetTexture = tempRT;
        cam.Render();
        Texture2D tex2d = new Texture2D(w, h, TextureFormat.RGBA32, false);
        RenderTexture.active = cam.targetTexture;
        tex2d.ReadPixels(new Rect(0, 0, w, h), 0, 0);
        tex2d.Apply();
        cam.targetTexture = null;
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(tempRT);
        tempRT = null;
        return tex2d;

    }
    public void Reset()
    {
        this.gameObject.SetActive(cameraInfo.cameraEnable);
        this.transform.position = cameraInfo.posVector;
        this.transform.rotation = Quaternion.Euler(cameraInfo.rotationVector);
    }
}
