using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioResPool
{
    private static Dictionary<string, AudioClip> m_dictAudioClip = new Dictionary<string, AudioClip>();

    public static void Preload(string packageName, string resPath, Action finishCallback)
    {
        if (m_dictAudioClip.ContainsKey(resPath))
        {
            if (finishCallback != null)
                finishCallback();
            return;
        }
        
        ResLoadManager.LoadAsync(AssetType.Audio, packageName, resPath, (relativePath, res) =>
        {
            m_dictAudioClip[resPath] = res as AudioClip;
            if (finishCallback != null)
                finishCallback();
        });
    }

    public static void Clear()
    {
        m_dictAudioClip.Clear();
    }

    public static AudioClip GetAudioClip(string resPath)
    {
        if (m_dictAudioClip.ContainsKey(resPath))
            return AudioClip.Instantiate(m_dictAudioClip[resPath]);
        return null;
    }
}
