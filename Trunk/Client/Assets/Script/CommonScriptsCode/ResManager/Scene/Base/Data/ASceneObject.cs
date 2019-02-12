using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASceneObject
{


	public Vector3 posVector;

	public Vector3 rotationVector;

	public Vector3 scaleVector;


	public void SetTransformInfo(Transform tf)
	{
		posVector = tf.position;
		rotationVector = tf.rotation.eulerAngles;
		scaleVector = tf.localScale;
	}
}
