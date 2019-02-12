using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class AudioManager : Singleton<AudioManager>
{
    private Dictionary<string, List<string>> m_dict = new Dictionary<string, List<string>>();

    AudioManager()
	{
		//立体声;
		AudioSettings.speakerMode = AudioSpeakerMode.Stereo;
	}


	#region 所有声源对象池

	private const int GC_Limit = 10;
    /// <summary>
    /// 通过音源文件名来保存;
    /// </summary>
    private Dictionary<string, Dictionary<string, SoundObject>> soundObjectPool = new Dictionary<string, Dictionary<string, SoundObject>>();
    private float m_volumeBgm = 1f;
    private float m_volumeSound = 1f;

    [BlackList]
    public void AddAudioAsset(string packName, string abRelativePath)
    {
        if (!m_dict.ContainsKey(packName))
        {
            m_dict.Add(packName, new List<string>());
            m_dict[packName].Add(abRelativePath);
        }
        else
        {
            if (!m_dict[packName].Contains(abRelativePath))
                m_dict[packName].Add(abRelativePath);
        }
    }

    [BlackList]
	public SoundObject GetSoundObject(string packName, string soundName)
	{
		string resPath = PathUtil.Instance.GetAudioPath(packName, soundName);
		Dictionary<string, SoundObject> packDic = null;
		soundObjectPool.TryGetValue(resPath, out packDic);
		if (packDic == null)
		{
			packDic = new Dictionary<string, SoundObject>();
			soundObjectPool.Add(resPath, packDic);
		}
		SoundObject so = null;
		packDic.TryGetValue(resPath, out so);
		if (so == null)
		{
			//创建;
			so = new SoundObject(packName, resPath);
			so.AddReference();
			packDic.Add(resPath, so);
		}
		return so;
	}


	#endregion



	#region 频道对象池处理(2D音)

	AudioItem bgAudioItem;

    [BlackList]
    public AudioItem BgAudioItem
	{
		get { return bgAudioItem; }
	}

	List<AudioItem> item2DList = new List<AudioItem>();
	List<AudioItem> unused2DList = new List<AudioItem>();

	AudioItem Creat2DItem()
	{
		AudioItem item;
		if (unused2DList.Count > 0)
		{
			item = unused2DList[unused2DList.Count - 1];
			unused2DList.RemoveAt(unused2DList.Count - 1);
		}
		else
		{
			item = new AudioItem(audioRoot.AddComponent<AudioSource>());
		}
		item2DList.Add(item);
		return item;
	}

    [BlackList]
    public void Remove2DItem(AudioItem item)
	{
		for (int i = 0; i < item2DList.Count; i++)
		{
			AudioItem tempItem = item2DList[i];
			if (tempItem == item)
			{
				if (bgAudioItem == item)
				{
					bgAudioItem = null;
				}
				//清理;
				item.Clear();
				item2DList.Remove(item);
				unused2DList.Add(item);
				return;
			}
		}
	}

    [BlackList]
    public void Remove2DItem(SoundInfo info)
	{
		for (int i = 0; i < item2DList.Count; i++)
		{
			AudioItem tempItem = item2DList[i];
			if (tempItem.SoundClip.info == info)
			{
				if (bgAudioItem == tempItem)
				{
					bgAudioItem = null;
				}
				//清理;
				tempItem.Clear();
				item2DList.Remove(tempItem);
				unused2DList.Add(tempItem);
				return;
			}
		}
	}
	#endregion



	bool isInit = false;

	GameObject audioRoot;

    #region 对外接口
    [BlackList]
    /// <summary>
    /// 初始化音频控制器;
    /// </summary>
    public void Init(GameObject root)
	{
		isInit = true;
		if (audioRoot == null)
		{
			audioRoot = new GameObject("[AudioRoot]");
			audioRoot.transform.parent = root.transform;
		}
	}
    
	public void PlayBgMusic(string packName, string soundName)
	{
		if (!isInit) return;
		if (bgAudioItem == null)
		{
			bgAudioItem = new AudioItem(audioRoot.AddComponent<AudioSource>());
		}

		SoundInfo soundInfo = new SoundInfo();
		soundInfo.packName = packName;
		soundInfo.soundName = soundName;
		soundInfo.soundType = SoundTypeEnum.BG;
		soundInfo.volume = m_volumeBgm;
		soundInfo.isLoop = true;
		if (bgAudioItem.CheckEquals(soundInfo))
		{
			Debug.Log("RecoverSoundRecoverSoundRecoverSoundRecoverSoundRecoverSoundRecoverSoundRecoverSoundRecoverSoundRecoverSoundRecoverSoundRecoverSound");
			bgAudioItem.RecoverSound();
		}
		else
		{
			bgAudioItem.Play(soundInfo);
		}
	}

	public void StopBgMusic()
	{
		if (bgAudioItem != null)
		{
			bgAudioItem.StopSound();
		}
	}

	/// <summary>
	/// 返回音源组件
	/// </summary>
	/// <returns></returns>
	public AudioSource GetAudioSource()
	{
		return bgAudioItem.AudioSource;
	}

	/// <summary>
	/// 播放音乐
	/// </summary>
	/// <param name="packName"></param>
	/// <param name="soundName"></param>
	public void PlaySound(string packName, string soundName, bool isMute = false)
	{
		if (!isInit) return;
		if (bgAudioItem == null)
		{
			bgAudioItem = new AudioItem(audioRoot.AddComponent<AudioSource>());
		}

		SoundInfo soundInfo = new SoundInfo();
		soundInfo.packName = packName;
		soundInfo.soundName = soundName;
		soundInfo.soundType = SoundTypeEnum.BG;
		soundInfo.volume = m_volumeSound;
		soundInfo.isMute = isMute;
		soundInfo.isLoop = true;
		bgAudioItem.Play(soundInfo);
	}

	/// <summary>
	/// 停止音乐
	/// </summary>
	public void StopSound()
	{
		if (bgAudioItem != null && bgAudioItem.AudioSource != null)
		{
			bgAudioItem.AudioSource.Stop();
		}
	}

    [BlackList]
    /// <summary>
    /// 暂停音乐
    /// </summary>
    public void PauseSound()
	{
		if (bgAudioItem != null)
		{
			bgAudioItem.PauseSound();
		}
	}

    [BlackList]
    public void CleanBgMusic()
	{
		if (bgAudioItem != null)
		{
			bgAudioItem.Clear();
		}
	}
    
	public void Play2D(string packName, string soundName)
	{
		if (!isInit) return;
		AudioItem audioItem = Creat2DItem();
		SoundInfo soundInfo = new SoundInfo();

		soundInfo.packName = packName;
		soundInfo.soundName = soundName;
		soundInfo.soundType = SoundTypeEnum.Human;
		soundInfo.volume = m_volumeSound;
		audioItem.Play(soundInfo);
	}
    #endregion

    /// <summary>
    /// update里调用;
    /// </summary>
    public void AdjustVolume()
    {
        if (!isInit) return;
        Adjust2DVolume();
    }

    void Adjust2DVolume()
    {
        for (int i = item2DList.Count - 1; i >= 0; i--)
        {
            item2DList[i].AdjustVolume();
        }
        if (bgAudioItem != null)
        {
            bgAudioItem.AdjustVolume();
        }
    }

    /// <summary>
    /// 调节音量
    /// </summary>
    public void ChangeBgmVolume(float value)
	{
        if (value > 1f)
            value = 1f;
        else if (value < 0f)
            value = 0f;
        if (m_volumeBgm == value)
            return;
        m_volumeBgm = value;
        if (bgAudioItem != null)
            bgAudioItem.AudioSource.volume = m_volumeBgm;
    }

    /// <summary>
    /// 调节音量
    /// </summary>
    public void ChangeSoundVolume(float value)
    {
        if (value > 1f)
            value = 1f;
        else if (value < 0f)
            value = 0f;
        if (m_volumeSound == value)
            return;
        m_volumeSound = value;
        for (int i = item2DList.Count - 1; i >= 0; --i)
        {
            item2DList[i].AudioSource.volume = m_volumeSound;
        }
    }

    public void Preload(string packName, string soundName, Action finishCallback)
	{
		string resPath = PathUtil.Instance.GetAudioPath(packName, soundName);
		AudioResPool.Preload(packName, resPath, finishCallback);
	}

    [BlackList]
    /// <summary>
    /// 销毁一个包的所有音效
    /// </summary>
    /// <param name="packageName"></param>
    public void DestroyPackageAudio(string packageName)
    {
        if (!m_dict.ContainsKey(packageName))
            return;
        for (int i = 0, count = m_dict[packageName].Count; i < count; ++i)
        {
            AssetNodeManager.ReleaseNode(AssetType.Audio, packageName, m_dict[packageName][i]);
        }
    }
}
