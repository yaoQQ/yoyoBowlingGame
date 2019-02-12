using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection.Emit;
using XLua;

[LuaCallCSharp]
/*
 * @author SoftRare - www.softrare.eu
 * This class handles all global settings and executes intervals between updates. Please change values one at a time if you must, debug, and change the next if you are really sure everything is working,. Changes can have significant consequences for performance and overall functionality.
 * You may only use and change this code if you purchased it in a legal way.
 * Please read readme.txt, included in this package, to see further possibilities on how to use/execute this code.
 */
public class EZReplayManager 
{
    //dont change these constants
    public const int ACTION_RECORD = 0;
    public const int ACTION_PLAY = 1;
    public const int ACTION_PAUSED = 2;
    public const int ACTION_STOPPED = 3;
    public const int MODE_LIVE = 0;
    public const int MODE_REPLAY = 1;

    //these icons can be changed freely
    public Texture2D EZRicon;
    public Texture2D playIcon;
    public Texture2D pauseIcon;
    public Texture2D startRecordIcon;
    public Texture2D stopRecordIcon;
    public Texture2D replayIcon;
    public Texture2D stopIcon;
    public Texture2D closeIcon;
    public Texture2D rewindIcon;

    //these values have no influence on what the user sees:
    public const string S_PARENT_NAME = "EZReplayM_sParent"; //default: EZReplayM_sParent
    public const string S_EZR_ASSET_PATH = "EZReplayManagerAssetPrefabs";

    public const bool showErrors = true; //default: true
    public const bool showWarnings = true; //default: true
    public const bool showHints = false; //default: false

    public const string EZR_VERSION = "1.5";

    //don't change these manually unless you know what you are doing:
    private static int currentMode = MODE_LIVE;
    private static int currentAction = ACTION_STOPPED;
    private static int recorderPosition = 0;
    private static int maxPositions = 0;
    private static float playingInterval;
    private static int recorderPositionStep = 1; //default: 1
    private static int orgRecorderPositionStep = recorderPositionStep;
    private static bool exitOnFinished = false; //default: false
    private static float surplus = 0.0f;
    private static float timeelapsed = 0.0f;
    private static bool continueCallingUpdate = false;

    //these values determine what GUIs will be seen by the user:
    //public bool automaticallyRecordAllGameObjects = true; //default: true
    public bool useRecordingGUI = true; //default: true
    public bool useReplayGUI = true; //default: true	
    public bool useDarkStripesInReplay = true; //default: true
    public bool saveToFileOnDefault = true; //default: true

    public bool autoDeactivateLiveObjectsOnLoad = true; //default: true	

    //change these to configure the overall performance of the script:
    public float recordingInterval = 0.05f;  //default: 0.05f (20fps) .. if your replay is not fluent enough lower this value step by step.. try 0.04f (25fps) first, lower it then
    private const int maxSpeedSliderValue = 3; //default: 5
    private const int minSpeedSliderValue = -3; //default: -5
    private static int speedSliderValue = 0; //default: 0
    private static int speedSliderValueBackup = 0; //default: 0

    public Dictionary<GameObject, Object2PropertiesMapping> gOs2propMappingsRecordingSlot = new Dictionary<GameObject, Object2PropertiesMapping>();
    public Dictionary<GameObject, Object2PropertiesMapping> gOs2propMappingsLoadingSlot = new Dictionary<GameObject, Object2PropertiesMapping>();

    //here all mappings are done from the original game objects to their replay-counterpart clones
    public Dictionary<GameObject, Object2PropertiesMapping> gOs2propMappings;
    //these are the game objects to mark for recording. This way coding can be avoided.
    public List<GameObject> gameObjectsToRecord = new List<GameObject>();
    //fill these with names of components and scripts (strings). These will not be removed on replay.
    public List<string> componentsAndScriptsToKeepAtReplay = new List<string>();

    private List<GameObject> markedGameObjects = new List<GameObject>(); //don't fill on your own.

    private static EZReplayManager ezr_Singleton;

    //instantiation actions go here: DO NOT hit EZReplayManager.record() in "Awake()" function. Will fail. Call record in "Start()" instead.
    private void instantiateSingleton() {
        useRecordingSlot();
    }

    //serialize and save (do not call directly, call save() instead)
    private static void SerializeObject(string filename, Object2PropertiesMappingListWrapper objectToSerialize) {
        Stream stream = File.Open(filename, FileMode.Create);
        BinaryFormatter bFormatter = new BinaryFormatter();
        bFormatter.Serialize(stream, objectToSerialize);
        stream.Close();
    }

    //deserialize and return for loading (do not call directly, call load() instead)
    private static Object2PropertiesMappingListWrapper DeSerializeObject(string filename) {
        Object2PropertiesMappingListWrapper objectToSerialize = null;
        Stream stream = File.Open(filename, FileMode.Open);
        BinaryFormatter bFormatter = new BinaryFormatter();

        objectToSerialize = (Object2PropertiesMappingListWrapper)bFormatter.Deserialize(stream);
        //print("System.GC.GetTotalMemory(): "+System.GC.GetTotalMemory(false));
        stream.Close();

        return objectToSerialize;
    }

    // use to replay what you have just recorded in the same session
    public static void useRecordingSlot() {
        singleton.gOs2propMappings = singleton.gOs2propMappingsRecordingSlot;
    }

    //use to replay what was saved to file earlier
    public static void useLoadingSlot() {
        singleton.gOs2propMappings = singleton.gOs2propMappingsLoadingSlot;
    }

    //wrapper for saving to file
    public static void saveToFile(string filename) {

        Object2PropertiesMappingListWrapper o2pMappingListW = new Object2PropertiesMappingListWrapper();
        foreach (var entry in singleton.gOs2propMappings) {
            o2pMappingListW.addMapping(entry.Value);
        }
        o2pMappingListW.recordingInterval = singleton.recordingInterval;
        SerializeObject(filename, o2pMappingListW);

    }

    //wrapper for loading from file
    public static void loadFromFile(string filename) {

        if (singleton.autoDeactivateLiveObjectsOnLoad)
            foreach (var entry in singleton.markedGameObjects) {
                entry.SetActiveRecursively(false);
            }

        useLoadingSlot();
        //System.GC.Collect();
        Object2PropertiesMappingListWrapper reSerialized = (Object2PropertiesMappingListWrapper)DeSerializeObject(filename);

        singleton.gOs2propMappings.Clear();
        maxPositions = 0;
        recorderPosition = 0;
        singleton.recordingInterval = reSerialized.recordingInterval; //if you load a replay with a different recording interval, 
                                                                      //you have to reset it to the earlier value afterwards YOURSELF!


        if (reSerialized.EZR_VERSION != EZR_VERSION) {
            if (showWarnings)
                Logger.PrintDebug("EZReplayManager WARNING: The EZR version with which the file has been created differs from your version of the EZReplayManager. This can cause problems.");
        }

        foreach (var entry in reSerialized.object2PropertiesMappings) {

            if (entry.isParent()) {
                entry.prepareObjectForReplay();
                GameObject goClone = entry.getGameObjectClone();
                singleton.gOs2propMappings.Add(goClone, entry);

                foreach (KeyValuePair<int, SavedState> stateEntry in entry.savedStates) {
                    if (stateEntry.Key > maxPositions)
                        maxPositions = stateEntry.Key;
                }

            }
        }

        foreach (var entry in reSerialized.object2PropertiesMappings) {

            if (!entry.isParent()) {
                entry.prepareObjectForReplay();
                GameObject goClone = entry.getGameObjectClone();
                singleton.gOs2propMappings.Add(goClone, entry);
            }

            foreach (KeyValuePair<int, SavedState> stateEntry in entry.savedStates) {
                if (stateEntry.Key > maxPositions)
                    maxPositions = stateEntry.Key;
            }
        }

        currentMode = MODE_REPLAY;

        play(0);

    }

    //create an empty prefab at predefined location
#if UNITY_EDITOR
    //public static UnityEngine.Object createEmptyEZRPrefab(string filepath) {
    //    return PrefabUtility.CreateEmptyPrefab("Assets/Resources/" + filepath + ".prefab");
    //}
#endif

    //mark an object for recording, can be done while recording and while script is nonactive, but not while replaying a recording
    //v1.5: Specify "prefabLoadPath" if you know exactly where "go" is located as a prefab in "Resources" directory.
    public static void mark4Recording(GameObject go, bool prepareToSaveToFile, string prefabLoadPath) {
        Logger.PrintColor("yellow", "mark4Recording gameobject.name=" + go.name + " prefabLoadPath=" + prefabLoadPath);
        if (currentMode == MODE_LIVE) {
            if (currentAction != ACTION_PLAY) {

                if (!singleton.gOs2propMappings.ContainsKey(go)) { //if not already existant	
                                                                   //add to map

                    if (!prepareToSaveToFile) {
                        //if you dont need to save the replay to a file:
                        singleton.gOs2propMappings.Add(go, new Object2PropertiesMapping(go, true, null, 0));
                        singleton.addTestTarget.Add(go);

                    }
                    else {
                        //prepare for saving the replay to file:
                        string standardFilename = "go_" + go.name;
                        if (prefabLoadPath == "") {
                            prefabLoadPath = EZReplayManager.S_EZR_ASSET_PATH + "/" + standardFilename;
                        }
//#if UNITY_EDITOR
//                        if (Resources.Load(prefabLoadPath) == null) {
//                            //object has not been assigned to a specific prefab-path. Create our own:

//                            if (!Directory.Exists("Assets/Resources")) {
//                                AssetDatabase.CreateFolder("Assets", "Resources");
//                            }
//                            if (!Directory.Exists("Assets/Resources/" + S_EZR_ASSET_PATH)) {
//                                AssetDatabase.CreateFolder("Assets/Resources", S_EZR_ASSET_PATH);
//                            }

//                            if (Resources.Load(prefabLoadPath) == null) {
//                                UnityEngine.Object asset = createEmptyEZRPrefab(prefabLoadPath);

//                                PrefabUtility.ReplacePrefab(go, asset);
//                            }
//                        }
//#endif

                        singleton.gOs2propMappings.Add(go, new Object2PropertiesMapping(go, true, null, 0, prefabLoadPath));

                    }

                    if (singleton.autoDeactivateLiveObjectsOnLoad && !singleton.markedGameObjects.Contains(go)) {
                        singleton.markedGameObjects.Add(go);
                    }

                }
                else
                    if (showHints)
                    Logger.PrintDebug("EZReplayManager HINT: GameObject '" + go + "' has already been marked for recording.");


            }
            else
                if (showWarnings)
                Logger.PrintDebug("EZReplayManager WARNING: You cannot mark GameObject '" + go + "' for recording while a recording is being played.");
        }
        else {
            if (showWarnings)
                Logger.PrintDebug("EZReplayManager WARNING: You cannot mark GameObject '" + go + "' for recording while in replay mode.");

        }
    }

    //mark an object for recording, can be done while recording and while script is nonactive, but not while replaying a recording
    //v1.5: use this if you want to be able to save the recording to file
    public List<GameObject> addTestTarget = new List<GameObject>();
    public static void mark4Recording(GameObject go, bool prepareToSaveToFile) {
        mark4Recording(go, prepareToSaveToFile, "");

    }

    //mark an object for recording, can be done while recording and while script is nonactive, but not while replaying a recording
    //v1.5: legacy function for not saving the recording
    public static void mark4Recording(GameObject go) {
        if (!singleton.saveToFileOnDefault)
            mark4Recording(go, false, "");
        else
            mark4Recording(go, true, "");
    }

    public static int getMaxFrames(Dictionary<GameObject, Object2PropertiesMapping> go2o2pm) {
        int maxframes = 0;
        foreach (var entry in go2o2pm) {
            int tmp = entry.Value.getMaxFrames();
            if (tmp > maxframes)
                maxframes = tmp;
        }
        return maxframes;
    }

    public static int getMaxFrames(Object2PropertiesMapping o2pm) {
        return o2pm.getMaxFrames();
    }

    /*//experimental, use with care. Only call if you know what you are doing!
	public static void clearObjects() {
		singleton.gOs2propMappings.Clear();
		print ("clearObjects: gOs2propMappings.Count: "+singleton.gOs2propMappings.Count);
	}	*/

    //switch to different mode.. so far there are MODE_LIVE for viewing a normal game action and MODE_REPLAY for viewing a replay of a recording
    private static void switchModeTo(int newMode) {

        if (newMode == MODE_LIVE) {

            //reset game object (i.e. rigidbody state)
            foreach (KeyValuePair<GameObject, Object2PropertiesMapping> entry in singleton.gOs2propMappings) {

                //GameObject go = entry.Key;
                Object2PropertiesMapping propMapping = entry.Value;
                if (propMapping.getGameObject() != null)
                    propMapping.resetObject();

            }

            if (singleton.autoDeactivateLiveObjectsOnLoad)
                for (int i = 0; i < singleton.markedGameObjects.Count; i++) {
                    if (singleton.markedGameObjects[i] != null)
                        singleton.markedGameObjects[i].SetActiveRecursively(true);
                }

            useRecordingSlot();

            //COUNTFRAMES
            maxPositions = getMaxFrames(singleton.gOs2propMappings);


        }
        else {

            if (maxPositions > 0) {
                //prepare parents first
                foreach (KeyValuePair<GameObject, Object2PropertiesMapping> entry in singleton.gOs2propMappings) {
                    //GameObject go = entry.Key;
                    Object2PropertiesMapping propMapping = entry.Value;
                    if (propMapping.isParent()) {
                        if (propMapping.getGameObject() != null)
                            propMapping.prepareObjectForReplay();
                    }
                }
                //..then childs
                foreach (KeyValuePair<GameObject, Object2PropertiesMapping> entry in singleton.gOs2propMappings) {
                    //GameObject go = entry.Key;
                    Object2PropertiesMapping propMapping = entry.Value;
                    if (!propMapping.isParent()) {
                        if (propMapping.getGameObject() != null)
                            propMapping.prepareObjectForReplay();
                    }
                }
            }
            else {
                newMode = MODE_LIVE;
                if (showWarnings)
                    Logger.PrintDebug("EZReplayManager WARNING: You have not recorded anything yet. Will not replay.");
            }

        }

        currentMode = newMode;
        stop();
    }

    //stopping is essential to all other actions for resetting settings before switching
    public static void stop() {

        continueCallingUpdate = false;
        currentAction = ACTION_STOPPED;
        timeelapsed = 0.0f;
        if (orgRecorderPositionStep < 0)
            recorderPosition = maxPositions;
        else
            recorderPosition = 0;

        surplus = 0.0f;


        singleton.execRecorderAction();


    }


    /*//experimental, use with care. Only call if you know what you are doing!
	public static void cleanUp() {
		List<Object2PropertiesMapping> go2cleanUp = new List<Object2PropertiesMapping>();
		
		foreach(KeyValuePair<GameObject,Object2PropertiesMapping> entry in singleton.gOs2propMappings) {
			//GameObject go = entry.Key;
			Object2PropertiesMapping propMapping = entry.Value;
			if (propMapping.getGameObject() != null)
				go2cleanUp.Add(propMapping);
			
		}		
		
		foreach(Object2PropertiesMapping p in go2cleanUp) {
			singleton.gOs2propMappings.Remove(p.getGameObject());
		}
	}*/

    /*//experimental, use with care. Only call if you know what you are doing!
	private static void markAllObjects4Recording() {
		GameObject[] gos = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (GameObject go in gos) {
			mark4Recording(go);
		}
	}*/

    //mark the game objects from the public gui list for recording. Can be filled in Unity3d GUI to avoid coding.
    private static void markPredefinedObjects4Recording() {
        for (int i = 0; i < singleton.gameObjectsToRecord.Count; i++) {
            mark4Recording(singleton.gameObjectsToRecord[i]);
        }
    }

    //this starts the recording of objects, which have been marked for recoring previously or while recording in progress
    public static void record() {

        /*//experimental, use with care. Only call if you know what you are doing!
		if (singleton.automaticallyRecordAllGameObjects)
			markAllObjects4Recording(); //mark all the game objects in the scene for recording
		else*/
        markPredefinedObjects4Recording(); //mark the game objects from the public gui list for recording

        if (currentMode == MODE_LIVE) {
            if (currentAction == ACTION_STOPPED) {

                //remove a previous recording
                foreach (KeyValuePair<GameObject, Object2PropertiesMapping> entry in singleton.gOs2propMappings) {
                    //GameObject go = entry.Key;
                    Object2PropertiesMapping propMapping = entry.Value;
                    propMapping.clearStates();
                }

                //reset everything to standard values
                recorderPosition = 0;
                recorderPositionStep = 1;
                orgRecorderPositionStep = 1;
                //set new action
                currentAction = ACTION_RECORD;
                continueCallingUpdate = true;
                singleton.updateRecording(currentAction);
            }
            else {
                if (showWarnings)
                    Logger.PrintDebug("EZReplayManager WARNING: Ordered to record when recorder was not in stopped-state. Will not start recording.");
            }
        }
        else {
            if (showWarnings)
                Logger.PrintDebug("EZReplayManager WARNING: Ordered to record when recorder was in replay mode. Will not start recording.");
        }
    }
    //halt a replay
    public static void pause() {
        if (currentMode == MODE_REPLAY) {
            currentAction = ACTION_PAUSED;
            setReplaySpeed(minSpeedSliderValue);
        }
        else
            if (showWarnings)
            Logger.PrintDebug("EZReplayManager WARNING: Ordered to pause when recorder was not in replay mode. Will not pause.");

    }
    //simple wrapper for the play-method
    public static void play(int speed) {
        play(speed, false, false, false);
    }

    //replays a recording. 
    public static void play(int speed, bool playImmediately, bool backwards, bool exitOnFinished) {

        //switch to correct mode
        if (currentMode != MODE_REPLAY) {
            switchModeTo(MODE_REPLAY);
        }

        if (speed >= minSpeedSliderValue && speed <= maxSpeedSliderValue)
            speedSliderValue = speed;
        else
            speedSliderValue = 0;

        //revert playing direction if neccessary
        if ((backwards && orgRecorderPositionStep > 0) || (!backwards && orgRecorderPositionStep < 0)) {
            orgRecorderPositionStep *= -1;
        }
        //set playing speed
        setReplaySpeed(speedSliderValue);

        if (currentAction == ACTION_STOPPED || currentAction == ACTION_PAUSED) {

            if (currentAction != ACTION_PAUSED)
                stop();

            if (playImmediately)
                currentAction = ACTION_PLAY;

            EZReplayManager.exitOnFinished = exitOnFinished;
            continueCallingUpdate = playImmediately;
            singleton.updateRecording(currentAction);

        }
        else
            if (showHints)
            Logger.PrintDebug("EZReplayManager HINT: Ordered to play when not in stopped or paused state.");
    }

    public static int getCurrentPosition() {
        return recorderPosition;
    }

    public static int getCurrentAction() {
        return currentAction;
    }

    public static int getCurrentMode() {
        return currentMode;
    }

    void Awake() {
        Logger.PrintColor("blue","init EZReplayManager Awake()");
        singleton.instantiateSingleton();

    }

    // Use this for initialization
    void Start() {
        //markPredefinedObjects4Recording();
        //recorderPosition=0;
        //maxPositions=0;


    }

    //execute one cycle of the current action on the current recorder position
    private void execRecorderAction() {

        if (gOs2propMappings != null)
            foreach (KeyValuePair<GameObject, Object2PropertiesMapping> entry in gOs2propMappings) {
                GameObject go = entry.Key;
                Object2PropertiesMapping propMapping = entry.Value;

                if (currentAction == ACTION_RECORD && currentMode == MODE_LIVE) { //if recording

                    if (go != null && go.active) {
                        maxPositions = recorderPosition;
                        if (propMapping.getGameObject() != null)
                            propMapping.insertStateAtPos(recorderPosition);

                    }

                }
                else if (currentMode == MODE_REPLAY) { //if replaying
                                                       //if in between start and finish position
                    if ((recorderPosition <= maxPositions && orgRecorderPositionStep > 0) || (recorderPosition > 0 && orgRecorderPositionStep < 0)) {

                        //lerping not integrated yet
                        //float updateSyncTime = Time.realtimeSinceStartup;
                        //float lerpInterval = interval - ((updateSyncTime - updateStartingTime) % interval) ;
                        if (propMapping.getGameObject() != null && propMapping.getGameObjectClone() != null)
                            propMapping.synchronizeProperties(recorderPosition);

                    }
                    else { //else if reached the finishing position
                        stop();

                        if (exitOnFinished)
                            switchModeTo(MODE_LIVE);
                    }
                }
            }
    }

    //update recording/replaying position and start timer to next update
    private void updateRecording(int action) {

        float updateStartingTime = Time.realtimeSinceStartup;
        bool mayBeNull = false;

        float interval = recordingInterval;

        if (currentAction != ACTION_STOPPED && action == currentAction) { //if action has not changed sinds last update

            if (currentMode == MODE_REPLAY) {
                interval = playingInterval;
            }
            //execute current recorder action
            execRecorderAction();

            float updateEndingTime = Time.realtimeSinceStartup;

            if ((updateEndingTime - updateStartingTime) < interval) { // if updating didn't take longer than the current interval
                                                                      //substract surplus during more than one frame cycle to come to zero surplus

                float surplusToEliminate = 0.0f;
                if (surplus > 0.0f) { //if there is interval surplus..
                                      //..it has to be eliminated
                    surplusToEliminate = (interval - (updateEndingTime - updateStartingTime));

                    if (surplusToEliminate > surplus)
                        surplusToEliminate = surplus;
                }
                //determine interval to next update
                interval = interval - (updateEndingTime - updateStartingTime) - surplusToEliminate;

                mayBeNull = true;
                if (surplusToEliminate > 0.0f) {
                    surplus -= surplusToEliminate;

                    if (surplus < 0.0f)
                        surplus = 0.0f;

                }

            }
            else { //if updating took longer than the interval
                surplus += (updateEndingTime - updateStartingTime) - interval; //..add to surplus

                //update immediately
                interval = 0.0f;
                mayBeNull = true;
            }

            /*timeelapsed += (updateEndingTime - updateStartingTime) + surplus;
			print("timeelapsed: "+timeelapsed);*/  //<-- uncomment to make timeelapsed global
                                                   //float timeelapsed = (updateEndingTime - updateStartingTime); //<-- uncomment to make timeelapsed only about this cycle


            if ((((recorderPosition + recorderPositionStep > -1)) ||
                ((recorderPosition + recorderPositionStep < maxPositions))) && continueCallingUpdate) //if in the "middle of something"
                                                                                                      //should be the only place where to increase recorderPosition
                recorderPosition += recorderPositionStep;
            else
                stop(); //stop on finishing an action

            if ((interval > 0.0f || mayBeNull) && currentAction != ACTION_PAUSED
                && currentAction != ACTION_STOPPED && continueCallingUpdate) { //if another update can be done
                MainThread.Instance.StartCoroutine(waitForNewUpdate(interval, timeelapsed, action));
            }
        }

    }

    //there is an interval to wait for before the update will be done
    private static IEnumerator waitForNewUpdate(float delay, float timeelapsed, int action) {
        yield return new WaitForSeconds(delay);

        if (currentAction != ACTION_STOPPED)
            timeelapsed += delay;
        //print("timeelapsed: "+timeelapsed);
        if (continueCallingUpdate)
            singleton.updateRecording(action);

    }

    //bad function name: not only sets replay speed but also returns a string describing the replaying speed relative to the recording speed
    private static string setReplaySpeed(int speed) {

        string ret = "";

        if (speed == minSpeedSliderValue) {

            playingInterval = 0.0f;
            recorderPositionStep = orgRecorderPositionStep;
            currentAction = ACTION_PAUSED;
            ret = "Paused";
        }
        else if (speed > speedSliderValueBackup) {

            playingInterval = singleton.recordingInterval;

            int increaser = 1;
            if (orgRecorderPositionStep < 0)
                increaser = -1;

            recorderPositionStep = (orgRecorderPositionStep * speed) + increaser;
            int multiplicator = (int)Mathf.Round(recorderPositionStep / orgRecorderPositionStep);
            ret = "~ " + multiplicator + "x faster";
        }
        else if (speed < speedSliderValueBackup) {

            playingInterval = singleton.recordingInterval * ((speed - 1) * -2);
            recorderPositionStep = orgRecorderPositionStep;
            int divisor = (int)Mathf.Round(playingInterval / singleton.recordingInterval);
            ret = "~ " + divisor + "x slower";
        }
        else /*if (speed == sliderValueBackup)*/ {

            playingInterval = singleton.recordingInterval;
            recorderPositionStep = orgRecorderPositionStep;
            ret = "~ Recording speed";
        }

        return ret;
    }

    //build EZ Replay Manager GUI 
    void OnGUI() {

        if (currentMode == MODE_LIVE && useRecordingGUI) {
            Rect r0;

            if (currentAction == ACTION_RECORD || (maxPositions > 0))
                r0 = new Rect(10, 20, 150, 200);
            else
                r0 = new Rect(10, 20, 150, 170);

            GUI.Box(r0, EZRicon);

            Rect r1 = new Rect(20, 160, 130, 20);
            Rect r2 = new Rect(20, 190, 130, 20);


            if (currentAction == ACTION_STOPPED) {

                if (GUI.Button(r1, startRecordIcon)) {
                    EZReplayManager.record();
                }

            }
            else if (currentAction == ACTION_RECORD) {

                if (GUI.Button(r1, stopRecordIcon)) {
                    EZReplayManager.stop();
                }

            }

            if (maxPositions > 0)
                if (GUI.Button(r2, replayIcon)) {
                    EZReplayManager.stop();
                    EZReplayManager.play(0, false, false, false);
                }


        }
        else if (currentMode == MODE_REPLAY) {

            if (useDarkStripesInReplay) {
                GUI.Box(new Rect(0, 0, Screen.width, 100), EZRicon);

                GUI.Box(new Rect(0, Screen.height - 100, Screen.width, Screen.height - 100), "");
            }

            if (useReplayGUI) {
                //<!-- SPEED SLIDER
                bool stopped = false;

                if (speedSliderValue <= minSpeedSliderValue)
                    stopped = true;

                speedSliderValue = (int)GUI.HorizontalSlider(new Rect(10, 60, 120, 70), speedSliderValue, minSpeedSliderValue, maxSpeedSliderValue);

                //setReplaySpeed: bad function name
                string speedIndicator = setReplaySpeed(speedSliderValue);

                if (stopped && playingInterval > 0.0f) {
                    stopped = false;
                }

                GUI.Box(new Rect(10, 10, 120, 45), "Replay speed:\n" + speedIndicator); //+ "\n, interval: "+playingInterval+", step: "+recorderPositionStep);

                // SPEED SLIDE //--> 

                //<!-- TIME SLIDER 
                int recorderPositionTemp = (int)GUI.HorizontalSlider(new Rect(150, 60, 240, 10), recorderPosition, 0, maxPositions);

                if (recorderPositionTemp != recorderPosition) {
                    pause();
                    recorderPosition = recorderPositionTemp;
                    singleton.execRecorderAction();
                }

                float percentage = Mathf.Round(((float)recorderPosition / (float)maxPositions) * 100.0f);
                GUI.Box(new Rect(150, 10, 240, 45), "Replay position: " + percentage + "%");
                // TIME SLIDER //--> 

                //<!-- POSITION MANIPULATION TOOLS 
                if (GUI.Button(new Rect(158, 72, 40, 23), playIcon)) {
                    play(speedSliderValue, true, false, false);
                }

                if (GUI.Button(new Rect(203, 72, 40, 23), rewindIcon)) {
                    play(speedSliderValue, true, true, false);
                }

                if (GUI.Button(new Rect(248, 72, 40, 23), pauseIcon)) {
                    pause();
                }

                if (GUI.Button(new Rect(293, 72, 40, 23), stopIcon)) {
                    stop();
                }

                if (GUI.Button(new Rect(338, 72, 40, 23), closeIcon)) {
                    switchModeTo(MODE_LIVE);
                }
                // POSITION MANIPULATION TOOLS //-->
            }

        }
    }

    public static void close() {
        switchModeTo(MODE_LIVE);
    }

    public static void dispose() {
        close();
        singleton.gOs2propMappingsRecordingSlot.Clear();
        singleton.gOs2propMappingsLoadingSlot.Clear();
        if (singleton.gOs2propMappings != null) {
            singleton.gOs2propMappings.Clear();
        }
        singleton.gameObjectsToRecord.Clear();
        singleton.markedGameObjects.Clear();
        ezr_Singleton = null;
    }

    void OnLevelWasLoaded() {
        stop();
    }

    //returns the only instance of EZReplayManager. Don't put this script on just some GameObject but instantiate the prefab which comes with the package.
    //public static EZReplayManager singleton
    //{
    //    get
    //    {
    //        if (ezr_Singleton == null) {
    //            GameObject ezr = GameObject.Find("EZReplayManager");
    //            ezr_Singleton = ezr.GetComponent(typeof(EZReplayManager)) as EZReplayManager;
    //        }
    //        Logger.PrintColor("blue", "init EZReplayManager singleton()");
    //        return ezr_Singleton;
    //    }
    //}
    public static EZReplayManager singleton
    {

        get
        {
            if (ezr_Singleton == null) {
                ezr_Singleton = new EZReplayManager();
                singleton.instantiateSingleton();
                Logger.PrintColor("blue", "init EZReplayManager singleton()");
            }
            return ezr_Singleton;
        }
    }
}


