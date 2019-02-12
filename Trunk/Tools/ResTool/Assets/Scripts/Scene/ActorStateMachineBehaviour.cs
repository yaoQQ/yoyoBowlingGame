using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;
using System;

public class ActorStateMachineBehaviour : StateMachineBehaviour
{

    public Action OnIdleAction;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     
        if (stateInfo.IsName("idle"))
        {
            //Debug.LogError("=======================================");
            //Debug.LogError(stateInfo.shortNameHash);
            //Debug.LogError(Animator.StringToHash("idle"));
            Debug.LogError("===================idle====================");
            if (OnIdleAction!=null)
            {
                
                OnIdleAction.Invoke();
            }
            
        }
       
    }

}
