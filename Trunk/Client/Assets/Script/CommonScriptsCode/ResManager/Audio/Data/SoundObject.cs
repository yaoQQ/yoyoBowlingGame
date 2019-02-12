using UnityEngine;

/// <summary>
/// 音源对象，包括assetbundle和声音信息SoundInfo
/// </summary>
public class SoundObject:ABObject
{
    private string m_packageName;
    string name = string.Empty;

    string resName;

    private AudioClip m_audioClip = null;
    
    public string GetName()
    {
        return name;
    }

    public SoundObject(string packageName, string p_name)
    {
        m_packageName = packageName;
        name = p_name;
        LoadAB();
    }

    void LoadAB()
    {
        curLoadState = LoadState.Loading;
        m_audioClip = AudioResPool.GetAudioClip(name);
        if (m_audioClip == null)
        {
            //LoadManager.Instance.AddOrder(LoaderType.AssetBundle, name, LoadABEnd);
            ResLoadManager.LoadAsync(AssetType.Audio, m_packageName, name, (relativePath, res) =>
            {
                AudioManager.Instance.AddAudioAsset(m_packageName, name);
                m_audioClip = res as AudioClip;
                curLoadState = LoadState.Loaded;
            });
        }
        else
            curLoadState = LoadState.Loaded;
    }

    public AudioClip GetAudioClip()
    {
        return m_audioClip;
    }

    /// <summary>
    /// 响应GC，销毁AB;
    /// </summary>
    public void Destroy()
    {
		name = null;
        ab.Unload(true);
        Debug.Log("删AB");
    }
}
