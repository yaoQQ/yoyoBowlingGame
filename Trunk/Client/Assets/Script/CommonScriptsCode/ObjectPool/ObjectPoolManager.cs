using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
	PoolResProxy resProxy = new PoolResProxy();

	private float m_progress;
	public float Progress
	{
		get { return this.m_progress; }
		private set { this.m_progress = value; }
	}

	public Action LoadingEvent;
	public Action LoadedEvent;
	public void CreatePoolObject(string packName, string resName, Action<UnityEngine.Object> onLoadend)
	{
		packName = packName.ToLower();
		resName = resName.ToLower();
		MainThread.Instance.StartCoroutine(AsyncCreateObject(packName, resName, onLoadend));

	}

	IEnumerator AsyncCreateObject(string packName, string effectName, Action<UnityEngine.Object> onLoadend)
	{
		while (resProxy.GetPackManifest(packName) == null)
		{
			yield return 0;
		}

        string abRelativePath = UtilMethod.ConnectStrs("effect/", packName, "/prefab/", effectName, ".unity3d");
        ResLoadManager.LoadAsync(AssetType.Effect, packName, abRelativePath, (relativePath, res) =>
        {
            if (onLoadend != null)
            {
                onLoadend.Invoke(res as UnityEngine.Object);
            }
        });
	}

}
