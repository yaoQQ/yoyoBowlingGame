using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;
using System;

public delegate void LoadSpriteCallBackFunction(Sprite sprite);

class LoadIconRequest
{
    public LoadSpriteCallBackFunction callback;
    public string spriteName;
}

public class IconResManager : Singleton<IconResManager>
{
    string ICON_ASSET_BUNDLE_FILE_DIR = PathUtil.Instance.GetIconResDirPath() + "/png";
    string ICON_ASSET_BUNDLE_MAP_FILE = PathUtil.Instance.GetIconResDirPath() + "/bundleinfomap";

    Dictionary<string/*iconName*/, string/*spriteName*/> iconToSpriteNameMap = new Dictionary<string, string>();
    Dictionary<string/*spriteName*/, string/*ABFileName*/> spriteToBundleNameMap = new Dictionary<string, string>();
    Dictionary<string/*ABFileName*/, AssetBundle> assetBundleMap = new Dictionary<string, AssetBundle>();

    Dictionary<IconTypeEnum, string> iconTypeStringMap = new Dictionary<IconTypeEnum, string>();
    List<LoadIconRequest> requestList = new List<LoadIconRequest>();
    bool isLoadingABFile = false;

    public IconResManager()
    {
        Init();
    }

    public bool Init()
    {
        spriteToBundleNameMap.Clear();
        assetBundleMap.Clear();
        iconTypeStringMap.Clear();

        InitIconTypeStringMap();
        return LoadConfigFile();
    }

    void InitIconTypeStringMap()
    {
        // 确保Icon名字符串小写，这一步在导入打包Icon AB文件的时候做的处理，原因是：有些平台机器解析的大写错误。
        iconTypeStringMap.Add(IconTypeEnum.Activity, "activity_icon".ToLower());
        iconTypeStringMap.Add(IconTypeEnum.Grow, "grow_icon".ToLower());
        iconTypeStringMap.Add(IconTypeEnum.GuildSkill, "ui_guild_skill_icon".ToLower());
        iconTypeStringMap.Add(IconTypeEnum.Item, "item_icon".ToLower());
        iconTypeStringMap.Add(IconTypeEnum.PetHead, "ui_pet_icon".ToLower());
        iconTypeStringMap.Add(IconTypeEnum.PetSkill, "ui_pet_skill_icon".ToLower());
        iconTypeStringMap.Add(IconTypeEnum.RoleSkill, "ui_role_skill".ToLower());
        iconTypeStringMap.Add(IconTypeEnum.RoleSkySkill, "ui_role_sky_skill".ToLower());
        iconTypeStringMap.Add(IconTypeEnum.ShapeShifting, "ui_role_bianshen_skill".ToLower());
        iconTypeStringMap.Add(IconTypeEnum.WorldTrial, "worldtria_boss_icon".ToLower());

        iconTypeStringMap.Add(IconTypeEnum.MarketStallSecondType, "market_stall_second_type_icon".ToLower());
    }

    public void Unload()
    {
        foreach (var item in assetBundleMap)
        {
            item.Value.Unload(true);
        }

        assetBundleMap.Clear();
    }

    bool LoadConfigFile()
    {
        var data = File.ReadAllText(ICON_ASSET_BUNDLE_MAP_FILE, Encoding.Unicode);

        var lines = data.Split('\n');
        foreach (var line in lines)
        {
            // 行存储格式: IconFileName:SpriteName:ABFileName
            string[] fields = line.Split(':');

            string iconName = fields[0];
            string spriteName = fields[1];
            string abFileName = fields[2];

            iconToSpriteNameMap.Add(iconName, spriteName);
            spriteToBundleNameMap.Add(spriteName, abFileName);
        }

        return true;
    }

    IEnumerator WWWLoadABFile(string abFileName)
    {
        var assetBundleFilePath = string.Format("{0}/{1}", ICON_ASSET_BUNDLE_FILE_DIR, abFileName);

        WWW contant = new WWW("file:///" + assetBundleFilePath);
        yield return contant;

        if (contant.isDone)
        {
            assetBundleMap[abFileName] = contant.assetBundle;
        }
        else
        {
            Debug.Log("加载AssetBundle失败，path:" + assetBundleFilePath);
        }

        OnLoadABFileFinished(abFileName);
    }

    void OnLoadABFileFinished(string abFileName)
    {
        isLoadingABFile = false;

        // 遍历需要删除的request
        List<LoadIconRequest> needToDelectRequest = new List<LoadIconRequest>();
        needToDelectRequest.Clear();

        foreach (var request in requestList)
        {
            if (!spriteToBundleNameMap.ContainsKey(request.spriteName))
            {
                needToDelectRequest.Add(request);
                continue;
            }

            var name = spriteToBundleNameMap[request.spriteName];
            if (name == abFileName)
            {
                needToDelectRequest.Add(request);
            }
        }

        foreach (var request in needToDelectRequest)
        {
            var sprite = GetItemFromABMap(request.spriteName);
            request.callback(sprite);

            requestList.Remove(request);
        }

        StartLoadABFile();
    }

    void StartLoadABFile()
    {
        if (requestList.Count > 0)
        {
            var req = requestList[0];

            var name = spriteToBundleNameMap[req.spriteName];
            LoadABFile(name);
        }
    }

    void LoadABFile(string abFileName)
    {
        if (!isLoadingABFile)
        {
            isLoadingABFile = true;
            MainThread.Instance.StartCoroutine(WWWLoadABFile(abFileName));
        }
    }


    // 该接口只是暂时用于标记那些地方会需要加载Missing图片
    public void LoadMissingIcon(Image image, IconTypeEnum type)
    {
        switch (type)
        {
            case IconTypeEnum.Item:
                image.sprite = Resources.Load<Sprite>("UI/item_icon_none");
                break;
            case IconTypeEnum.PetSkill:
                image.sprite = Resources.Load<Sprite>("UI/ui_pet_skill_icon_none");
                break;
            default:
                break;
        }

    }

    public void LoadIcon(IconTypeEnum type, int iconID, LoadSpriteCallBackFunction callback)
    {
        try
        {
            if (callback == null)
            {
                return;
            }

            string spriteName = GetSpriteName(type, iconID);
            if (spriteName == null)
            {
                callback(null);
                return;
            }

            if (!spriteToBundleNameMap.ContainsKey(spriteName))
            {
                
                Debug.LogError("没有找到sprite与AB文件的关联信息，SpriteName：" + spriteName);
                callback(null);
                return;
            }

            Sprite sprite = GetItemFromABMap(spriteName);
            if (sprite != null)
            {
                callback(sprite);
                return;
            }

            RsyncLoadIcon(spriteName, callback);
        }
        catch (Exception ex)
        {
            Debug.Log("加载icon精灵异常:" + ex.ToString());
        }
    }

    public void LoadIcon(IconTypeEnum type, string iconID, LoadSpriteCallBackFunction callback)
    {
        try
        {
            if (callback == null)
            {
                return;
            }
            if (iconID == null)
            {
                callback(null);
                return;
            }

            string spriteName = GetSpriteName(type,iconID);
            if (!spriteToBundleNameMap.ContainsKey(spriteName))
            {
                Debug.LogError("没有找到sprite与AB文件的关联信息，SpriteName：" + iconID);
                callback(null);
                return;
            }

            Sprite sprite = GetItemFromABMap(spriteName);
            if (sprite != null)
            {
                callback(sprite);
                return;
            }

            RsyncLoadIcon(spriteName, callback);
        }
        catch (Exception ex)
        {
            Debug.Log("加载icon精灵异常:" + ex.ToString());
        }
    }


    public void LoadIcon(IconTypeEnum type, int iconID, ImageWidget image)
    {
        LoadIcon(type, iconID, (x) => {
            if (image != null)
            {
                image.SetPng(x);
            }
        });
    }

    public void LoadIcon(IconTypeEnum type, string iconID, ImageWidget image)
    {
        LoadIcon(type, iconID, (x) =>
        {
            if (image != null)
            {
                image.SetPng(x);
            }
        });
    }

    void RsyncLoadIcon(string spriteName, LoadSpriteCallBackFunction callback)
    {
        requestList.Add(new LoadIconRequest() { callback = callback, spriteName = spriteName });

        StartLoadABFile();
    }

    string GetSpriteName(IconTypeEnum type, int iconID)
    {
        string iconName = GetIconName(type, iconID);
        if (iconName == null)
        {
            Debug.LogError("拼接IconName失败：IconTypeEnum:" + type.ToString() + "iconID:" + iconID);
            return null;
        }

        if (!iconToSpriteNameMap.ContainsKey(iconName))
        {
            Debug.LogError("没有找到Icon文件在资源包中的Sprite信息，IconName:" + iconName);
            return null;
        }

        return iconToSpriteNameMap[iconName];
    }

    string GetSpriteName(IconTypeEnum type, string iconName)
    {

        if (!iconToSpriteNameMap.ContainsKey(iconName))
        {
            Debug.LogError("没有找到Icon文件在资源包中的Sprite信息，IconName:" + iconName);
            return null;
        }

        return iconToSpriteNameMap[iconName];
    }


    string GetIconName(IconTypeEnum type, int iconID)
    {
        if (iconTypeStringMap.ContainsKey(type))
        {
            return string.Format("{0}_{1}.png", iconTypeStringMap[type], iconID);
        }
        return null;
    }



    Sprite GetItemFromABMap(string spriteName)
    {
        Sprite sprite = null;

        var abFileName = spriteToBundleNameMap[spriteName];
        if (assetBundleMap.ContainsKey(abFileName))
        {
            sprite = assetBundleMap[abFileName].LoadAsset(spriteName, typeof(Sprite)) as Sprite;
        }

        return sprite;
    }
}
