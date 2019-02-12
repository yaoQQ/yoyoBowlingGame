using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using XLua;

[LuaCallCSharp]
public class SceneEntity : MonoBehaviour
{
	bool initSign = false;

	public bool InitSign
	{
		get
		{
			return initSign;
		}
	}


	Dictionary<string, SceneContainer> containerDic = new Dictionary<string, SceneContainer>();

	Dictionary<string, SceneCamera> cameraDic = new Dictionary<string, SceneCamera>();
    SceneCamera[] allSceneCameraArr;

    Dictionary<string, Vector3> posPointDic = new Dictionary<string, Vector3>();

	string sceneName;

    public void Reset()
    {
        var container = containerDic.GetEnumerator();
        while (container.MoveNext())
        {
            container.Current.Value.Reset();
        }
        if(allSceneCameraArr!=null)
        {
            for(int i=0;i< allSceneCameraArr.Length;i++)
            {
                SceneCamera cam = allSceneCameraArr[i];
                cam.Reset();
            }
        }
       
    }

    public void Init(string scene_name, SceneData sceneData)
	{
		sceneName = scene_name.ToLower();

        SetSceneLight(sceneData);

        List<SceneCamera> allSceneCameraList = new List<SceneCamera>();
        foreach (SceneCameraInfo cameraInfo in sceneData.cameraInfoArr)
		{
            allSceneCameraList.Add(CreateCamera(cameraInfo));
		}
        allSceneCameraArr = allSceneCameraList.ToArray();

        foreach (SceneContainerInfo containerInfo in sceneData.containerInfoArr)
		{
			CreateContainer(containerInfo);
		}
		foreach (ScenePosPointInfo posPointInfo in sceneData.posPointInfoArr)
		{
			CreatePosPoint(posPointInfo);
		}
		MainThread.Instance.StartCoroutine(AsynInit());
	}

    SceneLightInfo lightInfo;
    public SceneLightInfo GetLightInfo()
    {
        return lightInfo;
    }

    //设置灯光信息
    void SetSceneLight(SceneData sceneData)
    {
        if(sceneData.lightInfo!=null)
        {
            lightInfo = sceneData.lightInfo;
            GameObject lightGO = new GameObject("Directional Light");
            lightGO.transform.parent = this.transform;

            Light light =lightGO.AddComponent<Light>();
            light.type = LightType.Directional;
            lightGO.transform.position = sceneData.lightInfo.posVector;
            lightGO.transform.rotation = Quaternion.Euler(sceneData.lightInfo.rotationVector);
        }
    }


    IEnumerator AsynInit()
	{
		var container = containerDic.GetEnumerator();
		while (container.MoveNext())
		{
            while (!container.Current.Value.CheckInitSign())
			{
				yield return 0;
			}
		}
		initSign = true;
	}

    SceneCamera CreateCamera(SceneCameraInfo cameraInfo)
	{
		GameObject cameraGO = new GameObject(cameraInfo.cameraName + " [camera]");
		SceneCamera sceneCamera = cameraGO.AddComponent<SceneCamera>();
		cameraGO.transform.parent = this.transform;
		cameraGO.transform.position = cameraInfo.posVector;
		cameraGO.transform.rotation = Quaternion.Euler(cameraInfo.rotationVector);
		Camera camera = cameraGO.AddComponent<Camera>();
		camera.clearFlags = (CameraClearFlags)cameraInfo.clearFlags;
		camera.orthographic = cameraInfo.orthographic;
		camera.orthographicSize = cameraInfo.orthographicSize;
		camera.fieldOfView = cameraInfo.fieldOfView == 0 ? 50 : cameraInfo.fieldOfView;

		camera.cullingMask = cameraInfo.cullingMask| LayerMask.GetMask("Default");
		camera.depth = cameraInfo.cameraDepth;
        camera.enabled = cameraInfo.cameraEnable;

        sceneCamera.cameraName = cameraInfo.cameraName;
        sceneCamera.cameraInfo = cameraInfo;

        cameraDic.Add(cameraInfo.cameraName, sceneCamera);
        return sceneCamera;

    }

	void CreateContainer(SceneContainerInfo containerInfo)
	{
		GameObject containerGO = new GameObject(containerInfo.containerName + "_container");
		containerGO.transform.parent = this.transform;
		SceneContainer sceneContainer = containerGO.AddComponent<SceneContainer>();
		sceneContainer.containerName = containerInfo.containerName;
        sceneContainer.containerInfo = containerInfo;
        sceneContainer.gameObject.layer = containerInfo.layerMaskValue;
        sceneContainer.transform.position = containerInfo.posVector;
        sceneContainer.transform.rotation = Quaternion.Euler(containerInfo.rotationVector);
        sceneContainer.transform.localScale = containerInfo.scaleVector;
        int num = containerInfo.cellInfoArr.Length;
		sceneContainer.Init(num);
		for (int i = 0; i < num; i++)
		{
			SceneCellInfo cellInfo = containerInfo.cellInfoArr[i];
			CreateCell(sceneContainer, i, cellInfo);
		}
		containerDic.Add(sceneContainer.containerName, sceneContainer);
	}

	void CreatePosPoint(ScenePosPointInfo posPointInfo)
	{
		posPointDic.Add(posPointInfo.pointName, posPointInfo.posVector);
	}

	void CreateCell(SceneContainer sceneContainer, int index, SceneCellInfo cellInfo)
	{
		MainThread.Instance.StartCoroutine(AsyncCreatePrefab(sceneContainer, index, cellInfo));
	}
	IEnumerator AsyncCreatePrefab(SceneContainer sceneContainer, int index, SceneCellInfo cellInfo)
	{
        string abRelativePath = UtilMethod.ConnectStrs("scene/", sceneName, "/prefab/", cellInfo.prefabName, ".unity3d");
        ResLoadManager.LoadAsync(AssetType.Scene, sceneName, abRelativePath, (relativePath, res) =>
        {
            GameObject go = GameObject.Instantiate(res as GameObject);
            go.transform.parent = sceneContainer.transform;
            go.transform.position = cellInfo.posVector;
            go.transform.localScale = cellInfo.scaleVector;

            go.transform.rotation = Quaternion.Euler(cellInfo.rotationVector);
            SceneUtil.SetLayer(go, sceneContainer.gameObject.layer);
            SceneCell cell = go.GetComponent<SceneCell>();
            cell.cellInfo = cellInfo;
            cell.index = index;
            sceneContainer.AddCell(index, cell);
        });
        yield return null;
    }

    public SceneCamera[] GetAllCamera()
    {
        if (InitSign)
        {
            return allSceneCameraArr;
        }
        return null;
    }

    public SceneCamera GetCamera(string cameraName)
	{
		SceneCamera sceneCamera = null;
		cameraDic.TryGetValue(cameraName, out sceneCamera);        
		return sceneCamera;
	}

	public SceneContainer GetContainer(string containerName)
	{
		SceneContainer sceneContainer = null;
		containerDic.TryGetValue(containerName, out sceneContainer);
		return sceneContainer;
	}

	public Vector3 GetPosPoint(string pointName)
	{
		Vector3 pos = Vector3.zero;
		posPointDic.TryGetValue(pointName, out pos);
		return pos;
	}

	public void Del()
	{

		GameObject.Destroy(this.gameObject);
	}
}
