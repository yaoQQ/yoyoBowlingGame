using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

public class SceneTool
{

    static GameObject sceneRoot;


    static void CreateSceneRoot()
    {
        sceneRoot = GameObject.Find("[Scene]");
        if (sceneRoot==null)
        {
            sceneRoot = new GameObject("[Scene]");
        }
    }

    [MenuItem("Tool/Scene/Add Scene Camera")]
    public static void AddSceneCamera()
    {
        CreateSceneRoot();
        GameObject sceneCamera = new GameObject("SceneCamera");
        SceneCamera sc= sceneCamera.AddComponent<SceneCamera>();
        sceneCamera.AddComponent<Camera>();

        sceneCamera.transform.parent = sceneRoot.transform;

    }

    [MenuItem("Tool/Scene/Add Pos Point")]
    public static void AddPosPoint()
    {
        CreateSceneRoot();
        GameObject posPointGO = new GameObject("PosPoint");
        posPointGO.AddComponent<ScenePosPoint>();
        posPointGO.transform.parent = sceneRoot.transform;

    }


    [MenuItem("Tool/Scene/Add Scene Container")]
    public static void AddSceneContainer()
    {
        CreateSceneRoot();
        GameObject containerGO = new GameObject("Container");
        containerGO.AddComponent<SceneContainer>();
        containerGO.transform.parent = sceneRoot.transform;
        
    }

    [MenuItem("Tool/Scene/Creat Animation Controller")]
    public static void CreatAnimationController()
    {
        AnimationClip ac = Selection.activeObject as AnimationClip;
        string path = AssetDatabase.GetAssetPath(ac);
      
        string[] pathArr = path.Split('/');
        string fileName = Path.GetFileNameWithoutExtension(path);
        string clipsPath = Path.GetDirectoryName(path).Replace(Path.GetFileName(path),"");
        //Debug.Log(clipsPath);
        //Debug.Log("模块名： "+ pathArr[3]);
        //Debug.Log(fileName);
        string[] controllerArr = fileName.Split('_');
        string controllerName = controllerArr[0];
        string controllerPath = "Assets/Project/"+ProjectUtil.GetCurProjectName()+"/" + pathArr[3] + "/ctrl/ctrl_" + controllerName + ".controller";
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            
         AnimatorControllerLayer layer = animatorController.layers[0];
        AnimatorStateMachine sm = layer.stateMachine;
        sm.AddStateMachineBehaviour<ActorStateMachineBehaviour>();
        string[] filePathArr = Directory.GetFileSystemEntries(Application.dataPath+ clipsPath.Replace("Assets",""));

        AnimatorState idleState=null;
        Dictionary<string, List<AnimatorStateStruct>> stateDic = new Dictionary<string, List<AnimatorStateStruct>>();
        foreach (string filePath in filePathArr)
        {
            string clipFilePath = "Assets"+filePath.Replace(Application.dataPath,"");
            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipFilePath);
            if (clip != null)
            {
                string[] nameArr = clip.name.Split('_');
                if (nameArr[0] != controllerName) continue;
                string stateName = clip.name.Replace(controllerName + "_", "").ToLower();
                AnimatorState state = sm.AddState(stateName);
                state.motion = clip;
                if(stateName=="idle")
                {
                    idleState = state;
                    state.speed = 2;
                    sm.defaultState=state;
                }
                else
                {
                    string[] stateNameArr = stateName.Split('@');
                    List<AnimatorStateStruct> stateGroup = null;
                    stateDic.TryGetValue(stateNameArr[0],out stateGroup);
                    if(stateGroup==null)
                    {
                        stateGroup = new List<AnimatorStateStruct>();
                        stateDic.Add(stateNameArr[0], stateGroup);
                    }
                    AnimatorStateStruct asStruct = new AnimatorStateStruct();
                    if(stateNameArr.Length>1)
                    {
                        asStruct.sequenceCode =int.Parse(stateNameArr[1]);
                    }
                    else
                    {
                        asStruct.sequenceCode = 1;
                    }
                    asStruct.animatorState = state;
                    stateGroup.Add(asStruct);
                }
            }
        }
        animatorController.AddParameter("endSign", AnimatorControllerParameterType.Trigger); 
        foreach (KeyValuePair<string ,List<AnimatorStateStruct>> stateData in stateDic)
        {
            string groupName = stateData.Key;

            animatorController.AddParameter(groupName, AnimatorControllerParameterType.Trigger);


            List<AnimatorStateStruct> stateGroup = stateData.Value;
            stateGroup.Sort(delegate (AnimatorStateStruct x, AnimatorStateStruct y)
            {
                return x.sequenceCode.CompareTo(y.sequenceCode);
            });

            AnimatorStateTransition idleStateTransition=null;
            AnimatorStateTransition endStateTransition = null;
            for (int i=0;i< stateGroup.Count;i++)
            {
                AnimatorStateStruct asStruct = stateGroup[i];
                if(i==0)
                {
                    idleStateTransition = idleState.AddTransition(asStruct.animatorState);
                    idleStateTransition.AddCondition(AnimatorConditionMode.If, 1, groupName);
                    idleStateTransition.hasExitTime = true;

                    if (stateGroup.Count>1)
                    {
                        AnimatorStateTransition stateTransition = asStruct.animatorState.AddTransition(stateGroup[i + 1].animatorState);
                        stateTransition.AddCondition(AnimatorConditionMode.If, 1, "endSign");
                    }
                    else
                    {
                        endStateTransition = asStruct.animatorState.AddTransition(idleState);
                        endStateTransition.AddCondition(AnimatorConditionMode.If, 1, "endSign");
                        endStateTransition.hasExitTime = true;
                    }
                }
                else if(i== (stateGroup.Count-1))
                {
                    endStateTransition = asStruct.animatorState.AddTransition(idleState);
                    endStateTransition.AddCondition(AnimatorConditionMode.If, 1, "endSign");
                    endStateTransition.hasExitTime = true;
                }
                else
                {
                    AnimatorStateTransition stateTransition = asStruct.animatorState.AddTransition(stateGroup[i+1].animatorState);
                    stateTransition.AddCondition(AnimatorConditionMode.If, 1, "endSign");
                }
                

                //添加结束事件
                AnimationClip animationClip = asStruct.animatorState.motion as AnimationClip;
                //Debug.Log(animationClip.name);

                List<AnimationEvent> tempEvents = new List<AnimationEvent>();
                AnimationEvent addAE = new AnimationEvent();
                addAE.functionName = "OnAnimationPlayEnd";
                addAE.time = animationClip.length;
                addAE.stringParameter = asStruct.animatorState.name;
                tempEvents.Add(addAE);
                AnimationUtility.SetAnimationEvents(animationClip, tempEvents.ToArray());
            }

           
        }
        AssetDatabase.SaveAssets();

    }

    struct AnimatorStateStruct
    {
        public int sequenceCode;
        public AnimatorState animatorState;
    }


    //[MenuItem("Tool/Scene/Creat Animation Controller", true)]
    public static bool Check_CreatAnimationController()
    {
        AnimationClip ac = Selection.activeObject as AnimationClip;
        if (ac==null)
        {
            return false;
        }
        return true;
    }

   




}
