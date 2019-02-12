using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovieClip : ScriptableObject
{
    public int spaceTime = 100;
    public Sprite[] frameSprites;


    public int loopNum = -1;



    public int GetPlayNum()
    {
        int num = loopNum * frameSprites.Length;
        num = Mathf.Max(-1, num);
        return num;
    }

    public Sprite GetSprite(int index)
    {
        index = index % frameSprites.Length;
        if(index>=0&& index<frameSprites.Length)
        {
            return frameSprites[index];
        }
        return null;
    }
}
