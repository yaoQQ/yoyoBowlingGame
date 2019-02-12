using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class MapManager
{
    private class MapTile
    {
        public int zoom;
        public int x;
        public int y;
        public bool isDownLoad = false;
        public Sprite sprite = null;
        public Action<int, int, int, Sprite> callback = null;
    }

    private static Dictionary<string, MapTile> m_mapTileDict = new Dictionary<string, MapTile>();
    private static List<MapTile> m_mapTileList = new List<MapTile>();

    private class MyPosTile
    {
        public int tileX;
        public int tileY;
    }

    private static Dictionary<int, MyPosTile> m_myPosTileDict = new Dictionary<int, MyPosTile>();

    public static void SetMyPosTile(int zoom, int tileX, int tileY)
    {
        if (!m_myPosTileDict.ContainsKey(zoom))
        {
            MyPosTile myPosTile = new MyPosTile();
            myPosTile.tileX = tileX;
            myPosTile.tileY = tileY;
            m_myPosTileDict.Add(zoom, myPosTile);
        }
        else
        {
            MyPosTile myPosTile = m_myPosTileDict[zoom];
            myPosTile.tileX = tileX;
            myPosTile.tileY = tileY;
        }
    }

    /// <summary>
    /// 高德地图
    /// </summary>
    public static double TileYToLat(int tileY, int zoom, float pixelY)
    {
        double size = Math.Pow(2, zoom);
        double pixelYToTileAddition = pixelY / 256.0;
        double lat = Math.Atan(Math.Sinh(Math.PI * (1 - 2 * (tileY + pixelYToTileAddition) / size))) * 180.0 / Math.PI;
        return lat;
    }

    /*/// <summary>
    /// 百度地图
    /// </summary>
    public static double TileYToLat(int tileY, int zoom, float pixelY)
    {
        int R = 6378137;
        double size = Math.Pow(2, 26 - zoom);
        double pixelYToTileAddition = pixelY / 256.0;
        double lat = Math.Atan(Math.Pow(Math.E, (tileY + pixelYToTileAddition) / R * size)) * 360 / Math.PI - 90;
        return lat;
    }*/

    /// <summary>
    /// 高德地图
    /// </summary>
    public static void LatToTileY(double lat, int zoom, out int tileY, out float pixelY)
    {
        double size = Math.Pow(2, zoom);
        double lat_rad = lat * Math.PI / 180;
        double y = (1 - Math.Log(Math.Tan(lat_rad) + 1 / Math.Cos(lat_rad)) / Math.PI) / 2;
        y = y * size;
        tileY = (int)y;
        pixelY = (float)((y - tileY) * 256);
    }

    /*/// <summary>
    /// 百度地图
    /// </summary>
    public static void LatToTileY(double lat, int zoom, out int tileY, out float pixelY)
    {
        int R = 6378137;
        double size = Math.Pow(2, 26 - zoom);
        double lat_rad = lat * Math.PI / 180;
        double y = Math.Log(Math.Tan(lat_rad) + 1 / Math.Cos(lat_rad), Math.E) * R / size;
        tileY = (int)y;
        pixelY = (float)((y - tileY) * 256);
    }*/

    /// <summary>
    /// 计算2个坐标直接的距离
    /// </summary>
    public static float GetDistance(double lng1, double lat1, double lng2, double lat2)
    {
        double Lat1 = lat1 * Math.PI / 180.0;
        double Lat2 = lat2 * Math.PI / 180.0;
        double a = Lat1 - Lat2;
        double b = (lng1 - lng2) * Math.PI / 180.0;
        double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(Lat1) * Math.Cos(Lat2) * Math.Pow(Math.Sin(b / 2), 2)));//计算两点距离的公式
        s = s * 6378137.0;
        s = Math.Round(s * 10000d) / 10000d;
        return (float)s;
    }

    private static void AddMapTile(string key, MapTile mapTile)
    {
        if (m_mapTileDict.ContainsKey(key))
        {
            MapTile mapTile2 = m_mapTileDict[key];
            m_mapTileList.Remove(mapTile2);
            m_mapTileList.Add(mapTile2);
        }
        else
        {
            if (m_mapTileList.Count >= 512)
            {
                MapTile mapTile2 = m_mapTileList[0];
                string key2 = CommonUtils.ConnectStrs(mapTile2.zoom.ToString(),
                    "_", mapTile2.x.ToString(),
                    "_", mapTile2.y.ToString());
                m_mapTileDict.Remove(key2);
                m_mapTileList.RemoveAt(0);
                UnityEngine.Object.Destroy(mapTile2.sprite.texture);
                UnityEngine.Object.Destroy(mapTile2.sprite);
                //Logger.PrintLog("删除地图瓦片");
            }

            m_mapTileDict.Add(key, mapTile);
            m_mapTileList.Add(mapTile);
            //Logger.PrintLog("地图瓦片数量：" + m_mapTileDict.Count);
        }
    }

    public static void GetMapTiles(string urlFormat, int zoom, int x, int y, int rangeX, int rangeY, Action<int, int, int, Sprite> callback)
    {
        if (callback == null)
            return;
        string key;
        StringBuilder sb;
        int minX = x - rangeX;
        int maxX = x + rangeX;
        int minY = y - rangeY;
        int maxY = y + rangeY;
        for(int i = minX; i <= maxX; ++i)
        {
            for (int j = minY; j <= maxY; ++j)
            {
                sb = new StringBuilder();
                sb.Append(zoom);
                sb.Append("_");
                sb.Append(i);
                sb.Append("_");
                sb.Append(j);
                key = sb.ToString();
                if (m_mapTileDict.ContainsKey(key))
                {
                    if (m_mapTileDict[key].isDownLoad)
                        callback(zoom, i, j, m_mapTileDict[key].sprite);
                    else
                        m_mapTileDict[key].callback += callback;
                }
                else
                {
                    MapTile mapTile = new MapTile();
                    mapTile.zoom = zoom;
                    mapTile.x = i;
                    mapTile.y = j;
                    mapTile.callback += callback;
                    AddMapTile(key, mapTile);

                    string dir = UtilMethod.ConnectStrs(CommonPathUtils.PERSISTENT_DATA_ROOT_PATH, "/mapTileImage/");
                    string fileName = UtilMethod.ConnectStrs(key, ".png");
                    string path = UtilMethod.ConnectStrs(dir, fileName);
                    if (File.Exists(path))
                    {
                        //Loger.PrintLog("加载本地瓦片：" + path);
                        byte[] bytes = File.ReadAllBytes(path);
                        Texture2D texture = new Texture2D(256, 256, TextureFormat.ARGB32, false);
                        texture.LoadImage(bytes);

                        mapTile.isDownLoad = true;
                        mapTile.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                        mapTile.callback(mapTile.zoom, mapTile.x, mapTile.y, mapTile.sprite);
                        mapTile.callback = null;
                    }
                    else
                        DownloadMapTile(urlFormat, mapTile.zoom, mapTile.x, mapTile.y, key, dir, fileName);
                }
            }
        }
    }

    public static void GetMapTile(string urlFormat, int zoom, int x, int y, Action<int, int, int, Sprite> callback)
    {
        if (callback == null)
            return;
        string key;
        StringBuilder sb;
        sb = new StringBuilder();
        sb.Append(zoom);
        sb.Append("_");
        sb.Append(x);
        sb.Append("_");
        sb.Append(y);
        key = sb.ToString();
        if (m_mapTileDict.ContainsKey(key))
        {
            if (m_mapTileDict[key].isDownLoad)
                callback(zoom, x, y, m_mapTileDict[key].sprite);
            else
                m_mapTileDict[key].callback += callback;
        }
        else
        {
            MapTile mapTile = new MapTile();
            mapTile.zoom = zoom;
            mapTile.x = x;
            mapTile.y = y;
            mapTile.callback += callback;
            AddMapTile(key, mapTile);
            
            string dir = UtilMethod.ConnectStrs(PathUtil.PERSISTENT_DATA_ROOT_PATH, "/mapTileImage/");
            string fileName = UtilMethod.ConnectStrs(key, ".png");
            string path = UtilMethod.ConnectStrs(dir, fileName);
            if (File.Exists(path))
            {
                //Loger.PrintLog("加载本地瓦片：" + path);
                byte[] bytes = File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(256, 256, TextureFormat.ARGB32, false);
                texture.LoadImage(bytes);

                mapTile.isDownLoad = true;
                mapTile.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                mapTile.callback(mapTile.zoom, mapTile.x, mapTile.y, mapTile.sprite);
                mapTile.callback = null;
            }
            else
                DownloadMapTile(urlFormat, mapTile.zoom, mapTile.x, mapTile.y, key, dir, fileName);
        }
    }

    private static void DownloadMapTile(string urlFormat, int zoom, int tileX, int tileY, string key, string saveDir, string fileName)
    {
        string url = string.Format(urlFormat, tileX, tileY, zoom);
        //Loger.PrintLog("下载瓦片：" + url);
        NetResDownloadManager.Instance.Download(url, (www) =>
        {
            //Loger.PrintLog("下载瓦片完成：" + url);
            if (m_mapTileDict.ContainsKey(key))
            {
                MapTile mapTile = m_mapTileDict[key];
                mapTile.isDownLoad = true;
                Texture2D texture = www.texture;
                mapTile.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                mapTile.callback(mapTile.zoom, mapTile.x, mapTile.y, mapTile.sprite);
                mapTile.callback = null;
            }

            //存本地
            IOUtil.CreateDirectory(saveDir);
            string savePath = UtilMethod.ConnectStrs(saveDir, fileName);
            File.WriteAllBytes(savePath, www.bytes);
        });
    }
}