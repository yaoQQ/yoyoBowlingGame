using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneContainer : MonoBehaviour
{

    public string containerName;

    public SceneCell[] cellArr;


    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
