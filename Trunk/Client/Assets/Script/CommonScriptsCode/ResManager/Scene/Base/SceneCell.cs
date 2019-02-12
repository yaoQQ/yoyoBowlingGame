using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class SceneCell : MonoBehaviour
{

	public string prefabName;

	public SceneCellInfo cellInfo;

	bool isCollider = false;

	MeshCollider mc;

	public int index;

	public bool IsCollider
	{
		get
		{
			return isCollider;
		}

		set
		{
			isCollider = value;
			if (isCollider)
			{
				if (mc == null)
				{
					MeshFilter mf = gameObject.GetComponentInChildren<MeshFilter>();
					if (mf != null)
					{
						mc = mf.gameObject.AddComponent<MeshCollider>();
						mc.convex = true;
						mc.isTrigger = true;
						isCollider = true;
					}
					else
					{
						isCollider = false;
					}
				}
				else
				{
					isCollider = true;
					mc.enabled = true;
				}


			}
			else
			{
				if (mc != null)
				{
					isCollider = false;
					mc.enabled = false;
				}
			}
		}
	}

	void OnTrigger()
	{
		Debug.LogError("被点击到");
	}



	public void Reset(Transform parentTF)
	{
        this.gameObject.SetActive(true);
        transform.parent = parentTF;
		transform.position = cellInfo.posVector;
        transform.localScale = cellInfo.scaleVector;
		transform.rotation = Quaternion.Euler(cellInfo.rotationVector);
		SceneUtil.SetLayer(gameObject, parentTF.gameObject.layer);

	}

	public void Del()
	{
		GameObject.Destroy(this.gameObject);
	}


	//======================animator======================

	SceneAnimator sceneAnimator;
	void Awake()
	{
		if (sceneAnimator == null)
		{
			Animator anim = gameObject.GetComponent<Animator>();
            if (anim == null)
                anim = this.GetComponentInChildren<Animator>();
            if (anim != null)
				sceneAnimator = new SceneAnimator(anim);
		}
	}

	public SceneAnimator SceneAnimator
	{
		get
		{
			return sceneAnimator;
		}
	}
	public void OnAnimationPlayEnd(string parameter)
	{
        Debug.Log("OnAnimationPlayEnd===>>"+parameter);
		sceneAnimator.OnAnimationEvent(SceneAnimatorEvent.PLAY_END, parameter);
	}


}
