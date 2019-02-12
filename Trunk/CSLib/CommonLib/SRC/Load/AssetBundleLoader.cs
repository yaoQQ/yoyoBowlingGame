using UnityEngine;

public class AssetBundleLoader {
    public static T LoadAsset<T>(AssetBundle ab) where T : Object
    {
        var obj = ab.mainAsset;
        if(obj != null && obj is T) {
            return obj as T;
        }
		float start = Time.realtimeSinceStartup;
        var objs = ab.LoadAllAssets<T>();
		totalLoadedSeconds += Time.realtimeSinceStartup - start;
        if(objs.Length > 0) {
			for ( int i = 1; i < objs.Length; i++)
			{
				//GCManager.Instance.UnloadAsset (objs[i]);
			}
            return objs[0];
        }

        return null;
    }

	public static void Clear ()
	{
		loadedTextureAmount = 0;
		loadedAniclipSeconds = 0;
		totalLoadedSeconds = 0f;
	}


	public static int loadedTextureAmount = 0;
	public static float loadedAniclipSeconds = 0f;
	public static float totalLoadedSeconds = 0f;
	//public static bool CanLoadTexture ()
	//{
	//	if (LevelManager.Instance.IsSceneDataRuning)
	//		return loadedTextureAmount < 1;
	//	return loadedTextureAmount < 10;
	//}

	//public static bool CanLoadMesh ()
	//{
	//	if (LevelManager.Instance.IsSceneDataRuning)
	//		return totalLoadedSeconds < 15f;
	//	return totalLoadedSeconds < 200f;
	//}

	//public static bool CanLoadAniclip ()
	//{
	//	if (LevelManager.Instance.IsSceneDataRuning)
	//		return totalLoadedSeconds < 15f;
	//	return totalLoadedSeconds < 200f;
	//}
}