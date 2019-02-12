using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIIconTool  {

    public static void SetItemImage(Image itemImage, int iconValue)
    {
        SetImage(itemImage, IconTypeEnum.Item, iconValue);
    }

    public static void SetGuildSkillImage(Image itemImage, int iconValue)
    {
        SetImage(itemImage, IconTypeEnum.GuildSkill, iconValue);
    }

    public static void SetPetSkillImage(Image itemImage, int iconValue)
    {
        SetImage(itemImage, IconTypeEnum.PetSkill, iconValue);
    }

    public static void SetPetHeadImage(Image itemImage, int iconValue)
    {
        SetImage(itemImage, IconTypeEnum.PetHead, iconValue);
    }

    public static void SetRoleSkillImage(Image itemImage, string iconValue)
    {
        SetImage(itemImage, IconTypeEnum.RoleSkill, iconValue);
    }

    public static void SetWorldTriaBossImage(Image itemImage, int iconValue)
    {
        SetImage(itemImage, IconTypeEnum.WorldTrial, iconValue);
    }

    public static void SetGrowImage(Image image, int iconValue)
    {
        SetImage(image, IconTypeEnum.Grow, iconValue);
    }

    public static void SetActivityImage(Image image, int iconValue)
    {
        SetImage(image, IconTypeEnum.Activity, iconValue);
    }

    static void SetImage(Image image, IconTypeEnum type, int iconValue)
    {
        IconResManager.Instance.LoadIcon(type, iconValue, (x) =>
        {
            if (!image.IsDestroyed())
            {
                if (x != null)
                {
                    image.sprite = x;
                    return;
                }

                IconResManager.Instance.LoadMissingIcon(image, type);
            }
        });
    }

    static void SetImage(Image image, IconTypeEnum type, string iconValue)
    {
        IconResManager.Instance.LoadIcon(type, iconValue, (x) =>
        {
            if (!image.IsDestroyed())
            {
                if (x != null)
                {
                    image.sprite = x;
                    return;
                }

                IconResManager.Instance.LoadMissingIcon(image, type);
            }
        });
    }

    


    public static void SetItemImage(ImageWidget itemImage, int iconValue)
    {
        SetImage(itemImage, IconTypeEnum.Item, iconValue);
    }

    public static void SetGuildSkillImage(ImageWidget itemImage, int iconValue)
    {
        SetImage(itemImage, IconTypeEnum.GuildSkill, iconValue);
    }

    public static void SetPetSkillImage(ImageWidget itemImage, int iconValue)
    {
        SetImage(itemImage, IconTypeEnum.PetSkill, iconValue);
    }

    public static void SetPetHeadImage(ImageWidget itemImage, int iconValue)
    {
        SetImage(itemImage, IconTypeEnum.PetHead, iconValue);
    }

    public static void SetCommonImage(ImageWidget itemImage, string iconValue)
    {
        SetImage(itemImage, IconTypeEnum.RoleSkill, iconValue);
    }

    public static void SetWorldTriaBossImage(ImageWidget itemImage, int iconValue)
    {
        SetImage(itemImage, IconTypeEnum.WorldTrial, iconValue);
    }

    public static void SetGrowImage(ImageWidget image, int iconValue)
    {
        SetImage(image, IconTypeEnum.Grow, iconValue);
    }

    public static void SetActivityImage(ImageWidget image, int iconValue)
    {
        SetImage(image, IconTypeEnum.Activity, iconValue);
    }

    public static void SetMarketStallSecondTypeImage(ImageWidget image, int iconValue)
    {
        SetImage(image, IconTypeEnum.MarketStallSecondType, iconValue);
    }

    static void SetImage(ImageWidget image, IconTypeEnum type, int iconValue)
    {
        IconResManager.Instance.LoadIcon(type, iconValue, image);
    }

    static void SetImage(ImageWidget image, IconTypeEnum type, string iconValue)
    {
        IconResManager.Instance.LoadIcon(type, iconValue, image);
    }
}
