using System;
using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using XLua;
// ReSharper disable PossibleLossOfFraction

[LuaCallCSharp]
public class ScreenSnapManager : Singleton<ScreenSnapManager>
{
    private const string SnapPicPath = "/ScreenSnapPic.png";

    public string GetSnapPath()
    {
        return Application.persistentDataPath + SnapPicPath;
    }
    private Rect _targetRect;

    //传入目标RectTransform，然后根据Rect范围截图，CallBack方法为截图完成后调用
    public void StartScreenSnap(RectTransform rect, Action snapCallBackAction)
    {
        if (rect != null)
        {
            Vector3 vect = RectTransformUtility.WorldToScreenPoint(UIManager.Instance.UICamera, rect.gameObject.transform.position);
            //适配
            float radio = Screen.width / UIManager.Instance.UIRootWidthValue;
            float x = vect.x - rect.rect.width * rect.pivot.x * radio;
            float y = vect.y - rect.rect.height * rect.pivot.x * radio;

            _targetRect = new Rect(x, y, rect.rect.width * radio, rect.rect.height * radio);
            MainThread.Instance.StartCoroutine(ScreenSnap(snapCallBackAction));
        }

    }

    private IEnumerator ScreenSnap(Action snapCallBackAction)
    {
        yield return new WaitForEndOfFrame();
        Texture2D tex = new Texture2D((int)_targetRect.width, (int)_targetRect.height, TextureFormat.RGB24, false);

        tex.ReadPixels(new Rect((int)_targetRect.x, (int)_targetRect.y, (int)_targetRect.width, (int)_targetRect.height), 0, 0);
        tex.Apply();

        string path = GetSnapPath();
        File.WriteAllBytes(path, tex.EncodeToPNG());
        if (snapCallBackAction != null) snapCallBackAction();
    }


}