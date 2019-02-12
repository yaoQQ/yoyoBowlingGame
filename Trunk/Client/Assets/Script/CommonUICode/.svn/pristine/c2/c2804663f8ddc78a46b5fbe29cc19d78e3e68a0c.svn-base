using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIParticleScale : MonoBehaviour
{
    private List<ScaleData> scaleDatas = null;
    //是否计算出来的结果原有基础减少0.1大小
    public bool ReduceScale = false;
    //是否计算出来的结果原有基础加0.1大小
    public bool AddScale = false;
    float designWidth = 1920;//开发时分辨率宽
    float designHeight = 1080;//开发时分辨率高
    void Awake()
    {
        scaleDatas = new List<ScaleData>();
        foreach (ParticleSystem p in transform.GetComponentsInChildren<ParticleSystem>(true))
        {
            scaleDatas.Add(new ScaleData() { transform = p.transform, beginScale = p.transform.localScale });
        }
    }
    void Start()
    {
        OnInitParticleScale();
    }
    //初始化
    public void OnInitParticleScale()
    {
        float designScale = designWidth / designHeight;
        float scaleRate = (float)Screen.width / (float)Screen.height;
        foreach (ScaleData scale in scaleDatas)
        {
            if (scale.transform != null)
            {
                if (scaleRate < designScale)
                {
                    float scaleFactor = scaleRate / designScale;
                    if (ReduceScale)
                    {
                        scaleFactor = scaleFactor - 0.075f;
                    }
                    if (AddScale)
                    {
                        scaleFactor = scaleFactor + 0.1f;
                    }
                    scale.transform.localScale = scale.beginScale * scaleFactor;
                }
                else
                {
                    scale.transform.localScale = scale.beginScale;
                }
            }
        }
    }
    void Update()
    {
        OnInitParticleScale(); //Editor下修改屏幕的大小实时预览缩放效果
    }
    class ScaleData
    {
        public Transform transform;
        public Vector3 beginScale = Vector3.one;
    }
}