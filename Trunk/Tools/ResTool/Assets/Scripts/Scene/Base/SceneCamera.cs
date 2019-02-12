using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneCamera : MonoBehaviour
{
    public string cameraName;


    Camera cam;

    Vector2 viewableSize=Vector2.zero;

    float curAspect;

    public void SetViewableSize(float w, float h)
    {
        viewableSize = new Vector2(w,h);
        UpdateViewableSize();
    }
    public void SetViewableSize(Vector2 v2)
    {
        viewableSize = v2;
        UpdateViewableSize();
    }

    void Awake()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    void UpdateViewableSize()
    {
        if (cam == null) return;
        float orthographicSize = cam.orthographicSize;

        float cameraHeight = orthographicSize * 2;
        float cameraWidth = cameraHeight * curAspect;

        
        orthographicSize = viewableSize.x / (2 * curAspect);
        cam.orthographicSize = orthographicSize;
    }

    void Update()
    {
        if (cam == null) return;
        if (curAspect!= cam.aspect)
        {
            curAspect = cam.aspect;
            UpdateViewableSize();
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
}
