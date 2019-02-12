using UnityEngine;

[DisallowMultipleComponent]
public class DynamicUIWidget : MonoBehaviour
{

    public Transform Target;
    public Vector3 OffsetPos;

    private RectTransform rectTransform;
    private Camera m_mainCamera;
    public Camera RenderCamera
    {
        get
        {
            return this.m_mainCamera;
        }
        set
        {
            this.m_mainCamera = value;
        }
    }
    private Vector2 m_standarHeight;
    public Vector2 StandScrren
    {
        get
        {
            return this.m_standarHeight;
        }
        set
        {
            this.m_standarHeight = value;
        }
    }
    /// <summary>
    /// 是否按照高度适配(目前只有按照宽或者高度适配)
    /// </summary>
    private bool isMatchByHeight;
    void Start()
    {
        this.rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 动态UI的初始化
    /// </summary>
    /// <param name="target">目标物体</param>
    /// <param name="camera">渲染该动态UI的摄像机</param>
    /// <param name="offsetPos">偏移补偿, 世界坐标系</param>
    /// <param name="standScreen">标准屏幕尺寸</param>
    /// <param name="isMatchHeight">是否按高度匹配 = true</param>
    public void InitDynamicUI(Transform target, Camera camera, Vector3 offsetPos, Vector2 standScreen, bool isMatchHeight = true)
    {
        this.Target = target;
        this.m_mainCamera = camera;
        this.OffsetPos = offsetPos;
        this.m_standarHeight = standScreen;
        this.isMatchByHeight = isMatchHeight;
        this.UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (this.Target == null || this.rectTransform == null)
            return;
        if (this.m_mainCamera == null)
            return;
        var worldPos = Target.transform.position + this.OffsetPos;
        var screenPos = RectTransformUtility.WorldToScreenPoint(this.m_mainCamera, worldPos);
        var ratio = this.isMatchByHeight ? (this.m_standarHeight.y / Screen.height) : (this.m_standarHeight.x / Screen.width);
        var position = new Vector2(screenPos.x - Screen.width / 2, screenPos.y - Screen.height / 2) * ratio;
        rectTransform.localPosition = position;
    }

    void LateUpdate()
    {
        this.UpdatePosition();
    }
}

