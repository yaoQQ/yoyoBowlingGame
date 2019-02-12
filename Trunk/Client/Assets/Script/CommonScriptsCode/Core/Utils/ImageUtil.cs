using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using XLua;

[LuaCallCSharp]
public class ImageUtil
{
    public static void setNetImage(string url, Image image)
    {
        if (string.IsNullOrEmpty(url)) return;
        MainThread.Instance.StartCoroutine(DownloadImage(url, image));
    }

    static IEnumerator DownloadImage(string url, Image image, int size = 96)
    {

        if (url[url.Length - 1].Equals('0'))
        {
            url = url.Remove(url.Length - 1);
            url += size;
        }
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
        {
            Loger.PrintError("加载不到图片： " + url);
        }
        else
        {
            if (image != null)
            {
                Texture2D tex2d = www.texture;
                //以后可以做缓存
                Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));
                image.sprite = m_sprite;
            }
        }
    }

    public static void setTexture2DImage(Texture2D texture, object image)
    {

        if (texture != null && image != null)
        {
            if (image.GetType() == typeof(Image))
            {
                //以后可以做缓存
                Sprite m_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                (image as Image).sprite = m_sprite;
            }
            else if (image.GetType() == typeof(CircleImage))
            {
                //以后可以做缓存
                //Debug.Log("==============================收到的图像信息image名称为：" + (image as CircleImage).name);
                Sprite mSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                ((CircleImage) image).sprite = mSprite;
            }
        }
    }

    public static Sprite CreateSpriteByTexture(Texture2D texture)
    {
       return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public static void setTexture2DCircleImage(Texture2D texture, CircleImage image)
    {
        if (texture != null && image != null)
        {
            //以后可以做缓存
            Sprite mSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            image.sprite = mSprite;
        }
    }
}
