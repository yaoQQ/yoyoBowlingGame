using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 调试对话框，可输出调试信息，也可输入命令行参数
/// </summary>
public class DebugConsole : MonoBehaviour
{
    readonly Version VERSION = new Version("1.0");
    readonly string ENTRYFIELD = "DebugConsoleEntryField";
    public GameObject GamePosMap;

    /// <summary>
    /// This is the signature for the DebugCommand delegate if you use the command binding.
    ///
    /// So, if you have a JavaScript function named "SetFOV", that you wanted run when typing a
    /// debug command, it would have to have the following definition:
    ///
    /// \code
    /// function SetFOV(args)
    /// {
    ///     //...
    ///   return "value you want printed to console";
    /// }
    /// \endcode
    /// </summary>
    /// <param name="args">The text typed in the console after the name of the command.</param>
    public delegate object DebugCommand(params string[] args);

    /// <summary>
    /// How many lines of text this console will display.
    /// </summary>
    public int maxLinesForDisplay = 500;

    public int maxShortMessageForDisplay = 3;
    public float shortMessageShowTime = 30.0f; // 单位s

    public int distanceToTop = 20;
    public int distanceToBottom = 300;

    public int stackRectHeight = 130;

    private float _shortMessageShowTime = 0.0f;

    private static int  showDebugFingerNum = 3;

    /// <summary>
    /// Default color of the standard display text.
    /// </summary>
    //public Color defaultColor = Message.defaultColor;
    //public Color warningColor = Message.warningColor;
    //public Color errorColor = Message.errorColor;
    //public Color exceptionColor = Message.exceptionColor;
    //public Color systemColor = Message.systemColor;
    //public Color inputColor = Message.inputColor;
    //public Color outputColor = Message.outputColor;

    public bool IsDebug = true;

    /// <summary>
    /// Used to check (or toggle) the open state of the console.
    /// </summary>
    public static bool IsOpen
    {
        get { return DebugConsole.Instance._isOpen; }
        set { DebugConsole.Instance._isOpen = value; }
    }

    /// <summary>
    /// Static instance of the console.
    ///
    /// When you want to access the console without a direct
    /// reference (which you do in mose cases), use DebugConsole.Instance and the required
    /// GameObject initialization will be done for you.
    /// </summary>
    public static DebugConsole Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(DebugConsole)) as DebugConsole;

                if (_instance != null)
                    return _instance;

                GameObject console = new GameObject("__Debug Console__");
                _instance = console.AddComponent<DebugConsole>();
#if UNITY_EDITOR
                showDebugFingerNum = 3;
#else
                   showDebugFingerNum = 5;
#endif
            }

            return _instance;
        }
    }

    /// <summary>
    /// Key to press to toggle the visibility of the console.
    /// </summary>
    public static KeyCode toggleKey = KeyCode.BackQuote;

    static DebugConsole _instance;
    Dictionary<string, DebugCommand> _cmdTable = new Dictionary<string, DebugCommand>();
    Dictionary<string, string> _cmdTableDiscribes = new Dictionary<string, string>(); //cmd的注释
    Dictionary<string, WatchVarBase> _watchVarTable = new Dictionary<string, WatchVarBase>();
    string _inputString = string.Empty;

    Rect _windowRect;


    Vector2 _logScrollPos = Vector2.zero;
    Vector2 _stackScrollPosition = Vector2.zero;
    Vector2 _rawLogScrollPos = Vector2.zero;
    Vector2 _watchVarsScrollPos = Vector2.zero;
    public bool _isOpen;

    bool _toggleTouch = false;

    string _curStackInfo = "";
    StringBuilder _displayString = new StringBuilder();

    FPSCounter fps;

    bool dirty;
    #region GUI position values
    // Make these values public if you want to adjust layout of console window
    Rect scrollRect = new Rect(10, 20, 580, 362);
    Rect stackRect = new Rect(0, 0, 600, 150);
    public Rect inputRect = new Rect(10, 388, 600, 50);
    Rect toolbarRect = new Rect(16, 428, 425, 40);
    Rect buttonsRect = new Rect(465, 428, 200, 100);
    Rect buttonsRect2 = new Rect(465, 428, 200, 40);
    Rect messageLine = new Rect(4, 0, 564, 20);
    Rect stackInfoRect = new Rect(0, 0, 600, 150);
    Rect stackButtonRect = new Rect(0, 0, 60, 20);

    public int lineOffset = 1;
    string[] tabs = new string[] { "Log", "Copy Log", "Watch Vars", "Setting" };

    // Keep these private, their values are generated automatically
    Rect nameRect;
    Rect valueRect;
    Rect innerRect = new Rect(0, 0, 0, 0);
    int innerHeight = 0;
    Rect stackInnerRect = new Rect(0, 0, 0, 0);
    int stackInnerHeight = 0;
    int toolbarIndex = 0;
    GUIContent guiContent = new GUIContent();
    GUI.WindowFunction[] windowMethods;
    GUIStyle labelStyle;
    GUIStyle verticalScrollbarStyle;
    GUIStyle horizontalScrollbarStyle;
    #endregion

    /// <summary>
    /// This Enum holds the message types used to easily control the formatting and display of a message.
    /// </summary>
    public enum MessageType
    {
        ERROR,
        ASSERT,
        WARNING,
        LOG,
        EXCEPTION,
        SYSTEM,
        INPUT,
        OUTPUT
    }

    /// <summary>
    /// Represents a single message, with formatting options.
    /// </summary>
    struct Message
    {
        string text;
        string formatted;
        public int sameMsgCount;       //同一个Msg多次重复出现次数
        public void MsgCountAdd() { sameMsgCount++; Debug.Log("Count=" + sameMsgCount); }
        public MessageType type;
        string stacktext;
        string timestr;

        public static Color defaultColor = Color.white;
        public static Color warningColor = Color.yellow;
        public static Color errorColor = Color.red;
        public static Color exceptionColor = Color.magenta;
        public static Color systemColor = Color.green;
        public static Color inputColor = Color.cyan;
        public static Color outputColor = Color.grey;

        public string StackText
        {
            get { return stacktext; }
        }

        public Color color
        {

            get
            {
                if (type == MessageType.WARNING)
                {
                    return warningColor;
                }
                else if (type == MessageType.ERROR)
                {
                    return errorColor;
                }
                else if (type == MessageType.EXCEPTION)
                {
                    return exceptionColor;
                }
                else if (type == MessageType.ASSERT)
                {
                    return exceptionColor;
                }
                else if (type == MessageType.SYSTEM)
                {
                    return systemColor;
                }
                else if (type == MessageType.INPUT)
                {
                    return inputColor;
                }
                else if (type == MessageType.OUTPUT)
                {
                    return outputColor;
                }
                return defaultColor;
            }

            //private set
            //{
            //    Color _color = value;
            //}
        }

        public bool handle;

        public Message(object messageObject)
            : this(messageObject, MessageType.LOG)
        {
        }

        public Message(object messageObject, MessageType messageType)
            : this(messageObject, messageType, string.Empty)
        {
        }

        public Message(object messageObject, MessageType messageType, string messageStackText)
            : this()
        {
            if (messageObject == null)
                this.text = "<null>";
            else
                this.text = messageObject.ToString();

            this.handle = false;

            this.formatted = string.Empty;
            this.type = messageType;
            this.stacktext = messageStackText;
            //timestr = System.DateTime.Now.ToString("[yy-MM-dd HH:mm:ss.fff]");

        }

        public static Message LogMsg(object message)
        {
            return new Message(message, MessageType.LOG);
        }

        public static Message SystemMsg(object message)
        {
            return new Message(message, MessageType.SYSTEM);
        }

        public static Message ExceptionMsg(object message)
        {
            return new Message(message, MessageType.EXCEPTION);
        }

        public static Message AssertMsg(object message)
        {
            return new Message(message, MessageType.ASSERT);
        }

        public static Message WarningMsg(object message)
        {
            return new Message(message, MessageType.WARNING);
        }

        public static Message ErrorMsg(object message)
        {
            return new Message(message, MessageType.ERROR);
        }

        public static Message OutputMsg(object message)
        {
            return new Message(message, MessageType.OUTPUT);
        }

        public static Message InputMsg(object message)
        {
            return new Message(message, MessageType.INPUT);
        }

        public override string ToString()
        {
            switch (type)
            {
                //case MessageType.ERROR:
                //    return string.Format("[{0}] {1}", type, text);
                //case MessageType.WARNING:
                //    return string.Format("[{0}] {1}", type, text);
                case MessageType.SYSTEM:
                case MessageType.INPUT:
                case MessageType.OUTPUT:
                    return ToGUIString();
                default:
                    return string.Format("[{0}] {1}", (handle ? type.ToString().ToUpper() : type.ToString().ToLower()), text);
            }
        }

        public string ToGUIString()
        {
            if (!string.IsNullOrEmpty(formatted))
                return formatted;

            switch (type)
            {
                case MessageType.INPUT:
                    formatted = ">>> " + text;
                    break;
                case MessageType.OUTPUT:
                    var lines = text.Trim('\n').Split('\n');
                    var output = new StringBuilder();

                    foreach (var line in lines)
                    {
                        output.AppendLine("= " + line);
                    }

                    formatted = output.ToString();
                    break;
                case MessageType.SYSTEM:
                    formatted = "# " + text;
                    break;
                //case MessageType.WARNING:
                //    formatted = "* " + text;
                //    break;
                //case MessageType.ERROR:
                //    formatted = "** " + text;
                //    break;
                default:
                    //formatted = timestr + " " + (handle ? type.ToString().ToUpper() : type.ToString().ToLower()) + ":" + text;
                    formatted = text;
                    //if (!string.IsNullOrEmpty(stacktext))
                    //{
                    //    string[] splits = stacktext.Split('\n');
                    //    if (splits.Length > 1)
                    //    {
                    //        formatted += "\n" + splits[0];
                    //    }
                    //    //formatted += "\n" + stacktext;
                    //}
                    break;
            }

            return formatted;
        }
    }

    class History
    {
        List<string> history = new List<string>();
        int index = 0;

        public void Add(string item)
        {
            history.Add(item);
            index = 0;
        }

        string current;

        public string Fetch(string current, bool next)
        {
            if (index == 0) this.current = current;

            if (history.Count == 0) return current;

            index += next ? -1 : 1;

            if (history.Count + index < 0 || history.Count + index > history.Count - 1)
            {
                index = 0;
                return this.current;
            }

            var result = history[history.Count + index];

            return result;
        }
    }

    List<Message> _messages = new List<Message>();
    List<Message> _shortMessages = new List<Message>();
    History _history = new History();

    Vector2 shortMsgScrollPos = Vector2.zero;
    GUIStyle shortMsgStyle = new GUIStyle();

    float originWidth = 1024.0f;
    float originHeight = 768.0f;

    public void SetSendGMResult(bool isSucceed)
    {
        if (isSucceed)
            LogMessage(Message.InputMsg("指令发送成功"));
        else
            LogMessage(Message.InputMsg("指令发送失败"));
    }

    public void AdjustGUIMatrix()
    {
        if (Application.isMobilePlatform)
        {
            Matrix4x4 temp = GUI.matrix;
            temp[0, 0] = Screen.width / originWidth;
            temp[1, 1] = Screen.height / originHeight;
            GUI.matrix = temp;
        }
        else
        {
            originWidth = Screen.width;
            originHeight = Screen.height;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(this, true);
            return;
        }

        _instance = this;

        shortMsgStyle.wordWrap = true;

        DontDestroyOnLoad(this);

        //Application.RegisterLogCallback(HandleLog);

        Application.logMessageReceived += HandleLog;
    }

    public void InitPosition()
    {
        _windowRect = new Rect(originWidth / 2 - 300, 30, 600, 470);
    }
    void OnEnable()
    {
        windowMethods = new GUI.WindowFunction[] { OnLogWindow, OnCopyLogWindow, OnWatchVarWindow, OnSetting };

        fps = new FPSCounter();
        StartCoroutine(fps.Update());

        nameRect = messageLine;
        valueRect = messageLine;

        //Message.defaultColor = defaultColor;
        //Message.warningColor = warningColor;
        //Message.errorColor = errorColor;
        //Message.exceptionColor = exceptionColor;
        //Message.systemColor = systemColor;
        //Message.inputColor = inputColor;
        //Message.outputColor = outputColor;

        _windowRect = new Rect(0, 0, originWidth, originHeight);


        /*LogMessage(Message.SystemMsg("输入 '/?' 显示帮助"));
        LogMessage(Message.LogMsg(""));

        this.RegisterCommandCallback("close", CMDClose, "关闭调试窗口");
        this.RegisterCommandCallback("clear", CMDClear, "清除调试信息");
        this.RegisterCommandCallback("sys", CMDSystemInfo, "显示系统信息");
        this.RegisterCommandCallback("call", CMDCallLuaFunc, "调用Lua的方法");
        this.RegisterCommandCallback("dostring", CMDDoStringFunc, "执行一段字符串");
        this.RegisterCommandCallback("/?", CMDHelp, "显示可用命令");*/

    }

    public void Update()
    {
        if (Input.GetKeyUp(toggleKey))
        {
            _isOpen = !_isOpen;
        }

        if (Input.touchCount == showDebugFingerNum && !_toggleTouch)
        {
            _isOpen = !_isOpen;
            _toggleTouch = true;
        }
        else if (Input.touchCount < showDebugFingerNum && _toggleTouch)
        {
            _toggleTouch = false;
        }

        if (_shortMessageShowTime > 0.0f)
        {
            _shortMessageShowTime -= Time.deltaTime;
        }
    }

    void OnGUI()
    {
        while (_messages.Count > maxLinesForDisplay)
        {
            _messages.RemoveAt(0);
        }


        //// Toggle key shows the console in non-iOS dev builds
        //if (!Event.current.shift && Event.current.keyCode == toggleKey && Event.current.type == EventType.KeyUp)
        //{
        //    _isOpen = !_isOpen;
        //}

        //if (Input.touchCount == 3)
        //    _isOpen = !_isOpen;

        AdjustGUIMatrix();

        if (!_isOpen)   // 没有显示
        {
            while (_shortMessages.Count > maxShortMessageForDisplay)
            {
                _shortMessages.RemoveAt(0);
            }

            if (_shortMessageShowTime > 0.0f)
            {
                shortMsgScrollPos = GUILayout.BeginScrollView(shortMsgScrollPos, GUILayout.Width(originWidth), GUILayout.Height(originHeight));

                //GUILayout.Label(string.Format("fps: {0:00.0}", fps.current));

                for (int i = 0; i < _shortMessages.Count; i++)
                {
                    Message msg = _shortMessages[i];
                    shortMsgStyle.normal.textColor = msg.color;
                    GUILayout.Label(msg.ToString(), shortMsgStyle);
                }

                GUILayout.EndScrollView();
            }
            else
            {
                if (_shortMessages.Count > 0)
                    _shortMessages.Clear();
            }

            return;
        }

        labelStyle = GUI.skin.label;
        labelStyle.fontSize = 22;
        verticalScrollbarStyle = new GUIStyle(GUI.skin.verticalScrollbar);
        verticalScrollbarStyle.fixedWidth = 30;
        horizontalScrollbarStyle = new GUIStyle(GUI.skin.horizontalScrollbar);
        _windowRect.width = originWidth;
        _windowRect.height = originHeight;
        scrollRect = _windowRect;
        scrollRect.y += distanceToTop;
        scrollRect.height -= distanceToTop + distanceToBottom;
        messageLine.width = scrollRect.width - 20;
        innerRect.width = messageLine.width;

        _windowRect = GUI.Window(-1100, _windowRect, windowMethods[toolbarIndex], string.Format("Debug Console v{0}\tfps: {1:00.0}", VERSION, fps.current), GUI.skin.box);
        GUI.BringWindowToFront(-1100);


        if (GUI.GetNameOfFocusedControl() == ENTRYFIELD)
        {
            var evt = Event.current;

            if (!Event.current.shift && Event.current.keyCode == toggleKey && Event.current.type == EventType.KeyUp)
            {
                _isOpen = !_isOpen;
            }

            if (evt.isKey && evt.type == EventType.KeyUp)
            {
                if (evt.keyCode == KeyCode.Return)
                {
                    SendGM2MainGateway(_inputString);
                    _inputString = string.Empty;
                }
                else if (evt.keyCode == KeyCode.UpArrow)
                {
                    _inputString = _history.Fetch(_inputString, true);
                }
                else if (evt.keyCode == KeyCode.DownArrow)
                {
                    _inputString = _history.Fetch(_inputString, false);
                }
            }
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();

        //Application.RegisterLogCallback(null);
        Application.logMessageReceived -= HandleLog;
    }
    #region StaticAccessors

    /// <summary>
    /// Prints a message string to the console.
    /// </summary>
    /// <param name="message">Message to print.</param>
    public static object Log(object message)
    {
        if (!DebugConsole.Instance.IsDebug)
            return null;

        DebugConsole.Instance.LogMessage(Message.LogMsg(message));

        return message;
    }

    /// <summary>
    /// Prints a message string to the console.
    /// </summary>
    /// <param name="message">Message to print.</param>
    /// <param name="messageType">The MessageType of the message. Used to provide
    /// formatting in order to distinguish between message types.</param>
    public static object Log(object message, MessageType messageType)
    {
        if (!DebugConsole.Instance.IsDebug)
            return null;
        DebugConsole.Instance.LogMessage(new Message(message, messageType));

        return message;
    }

    /// <summary>
    /// Prints a message string to the console using the "Warning" message type formatting.
    /// </summary>
    /// <param name="message">Message to print.</param>
    public static object LogWarning(object message)
    {
        if (!DebugConsole.Instance.IsDebug)
            return null;
        DebugConsole.Instance.LogMessage(Message.WarningMsg(message));

        return message;
    }

    /// <summary>
    /// Prints a message string to the console using the "Error" message type formatting.
    /// </summary>
    /// <param name="message">Message to print.</param>
    public static object LogError(object message)
    {
        if (!DebugConsole.Instance.IsDebug)
            return null;
        DebugConsole.Instance.LogMessage(Message.ErrorMsg(message));

        return message;
    }

    public static object LogException(object message)
    {
        if (!DebugConsole.Instance.IsDebug)
            return null;
        DebugConsole.Instance.LogMessage(Message.ExceptionMsg(message));

        return message;
    }

    /// <summary>
    /// Clears all console output.
    /// </summary>
    public static void Clear()
    {
        DebugConsole.Instance.ClearLog();
    }

    /// <summary>
    /// Registers a debug command that is "fired" when the specified command string is entered.
    /// </summary>
    /// <param name="commandString">The string that represents the command. For example: "FOV"</param>
    /// <param name="commandCallback">The method/function to call with the commandString is entered.
    /// For example: "SetFOV"</param>
    public static void RegisterCommand(string commandString, DebugCommand commandCallback, string CMD_Discribes)
    {
        DebugConsole.Instance.RegisterCommandCallback(commandString, commandCallback, CMD_Discribes);
    }

    /// <summary>
    /// Removes a previously-registered debug command.
    /// </summary>
    /// <param name="commandString">The string that represents the command.</param>
    public static void UnRegisterCommand(string commandString)
    {
        DebugConsole.Instance.UnRegisterCommandCallback(commandString);
    }

    /// <summary>
    /// Registers a named "watch var" for monitoring.
    /// </summary>
    /// <param name="name">Name of the watch var to be shown in the console.</param>
    /// <param name="watchVar">The WatchVar instance you want to monitor.</param>
    public static void RegisterWatchVar(WatchVarBase watchVar)
    {
        DebugConsole.Instance.AddWatchVarToTable(watchVar);
    }

    /// <summary>
    /// Removes a previously-registered watch var.
    /// </summary>
    /// <param name="name">Name of the watch var you wish to remove.</param>
    public static void UnRegisterWatchVar(string name)
    {
        DebugConsole.Instance.RemoveWatchVarFromTable(name);
    }
    #endregion
    #region Console commands

    //==== Built-in example DebugCommand handlers ====
    object CMDClose(params string[] args)
    {
        _isOpen = false;

        return "closed";
    }

    object CMDClear(params string[] args)
    {
        this.ClearLog();

        return "clear";
    }

    object CMDHelp(params string[] args)
    {
        var output = new StringBuilder();

        output.AppendLine("可用命令列表: ");
        output.AppendLine("--------------------------");
        foreach (string key in _cmdTable.Keys)
        {
            output.AppendLine(_cmdTableDiscribes[key] + "  " + key);
        }

        output.Append("--------------------------");

        return output.ToString();
    }

    object CMDSystemInfo(params string[] args)
    {
        var info = new StringBuilder();

        info.AppendLine("Unity Ver: " + Application.unityVersion);
        info.AppendLine("Platform: " + Application.platform);
        info.AppendLine("Language: " + Application.systemLanguage);
        info.AppendLine(string.Format("Level: {0} [{1}]", Application.loadedLevelName, Application.loadedLevel));
        info.AppendLine("Data Path: " + Application.dataPath);
        info.AppendLine("Persistent Path: " + Application.persistentDataPath);
        info.AppendLine("streamingAssets Path: " + Application.streamingAssetsPath);
        info.AppendLine("temporaryCache Path: " + Application.temporaryCachePath);

        info.AppendLine(string.Format("Resolution: {0} * {1}", Screen.width, Screen.height));

        info.AppendLine("SystemMemorySize: " + SystemInfo.systemMemorySize);
        info.AppendLine("DeviceModel: " + SystemInfo.deviceModel);
        info.AppendLine("DeviceType: " + SystemInfo.deviceType);
        info.AppendLine("GraphicsDeviceName: " + SystemInfo.graphicsDeviceName);
        info.AppendLine("GraphicsMemorySize: " + SystemInfo.graphicsMemorySize);
        //info.AppendLine("GraphicsPixelFillrate: " + SystemInfo.graphicsPixelFillrate);
        info.AppendLine("GraphicsShaderLevel: " + SystemInfo.graphicsShaderLevel);
        info.AppendLine("MaxTextureSize: " + SystemInfo.maxTextureSize);
        info.AppendLine("OperatingSystem: " + SystemInfo.operatingSystem);
        info.AppendLine("ProcessorCount: " + SystemInfo.processorCount);

        info.AppendLine("Profiler.enabled = : " + UnityEngine.Profiling.Profiler.enabled.ToString());

        System.GC.Collect();
        info.AppendLine(string.Format("Total memory: {0:###,###,###,##0} kb", (System.GC.GetTotalMemory(true)) / 1024f));

        return info.ToString();
    }

    object CMDCallLuaFunc(params string[] args)
    {
        /*const int argLength = 2;
        if (args != null && args.Length >= argLength)
        {
            string func = args[1];
            object[] array = new object[args.Length - argLength];
            for (int i = argLength; i < args.Length; i++)
                array[i - argLength] = args[i];
            object[] objs = Helper.mCommon.CallMethod("LuaDebugConsole", func, array);
            if (objs != null && objs.Length > 0)
                return objs[0];
        }*/
        return null;
    }

    object CMDDoStringFunc(params string[] args)
    {
        /*const int argLength = 2;
        if (args != null && args.Length >= argLength)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < args.Length; i++)
                sb.Append(args[i]).Append(" ");
            object[] objs = Helper.mCommon.DoString(sb.ToString());
            if (objs != null && objs.Length > 0)
                return objs[0];
        }*/
        return null;
    }

    #endregion
    #region GUI Window Methods

    void DrawBottomControls()
    {

        stackRect.y = scrollRect.y + scrollRect.height;
        stackRect.width = scrollRect.width;
        stackRect.height = stackRectHeight;
        //stackInfoRect.x = 0;
        //stackInfoRect.y = 0;
        stackInfoRect.width = stackRect.width - 20;
        stackInnerRect.width = stackInnerRect.width;
        stackInnerRect.height = stackInnerHeight < stackRect.height ? stackRect.height : stackInnerHeight;

        GUI.Box(stackRect, string.Empty);

        _stackScrollPosition = GUI.BeginScrollView(stackRect, _stackScrollPosition, stackInnerRect);

        guiContent.text = _curStackInfo;
        stackInfoRect.height = labelStyle.CalcHeight(guiContent, stackInfoRect.width);

        GUI.Label(stackInfoRect, guiContent);

        stackInnerHeight = (int)stackInfoRect.height;

        GUI.EndScrollView();

        inputRect.y = stackRect.y + stackRect.height + 5;
        toolbarRect.y = inputRect.y + inputRect.height + 5;

        GUIStyle inputStyle = new GUIStyle(GUI.skin.textField);
        inputStyle.fontSize = 40;

        GUI.SetNextControlName(ENTRYFIELD);
        _inputString = GUI.TextField(inputRect, _inputString, inputStyle);

        /*var index = GUI.Toolbar(toolbarRect, toolbarIndex, tabs);

        if (index != toolbarIndex)
        {
            toolbarIndex = index;
        }*/

        GUIStyle btnStyle = new GUIStyle(GUI.skin.button);
        btnStyle.fontSize = 40;

        buttonsRect.y = inputRect.y + 1;
        buttonsRect.x = inputRect.x + inputRect.width + 10;
        if (GUI.Button(buttonsRect, "Send", btnStyle))
        {
            SendGM2MainGateway(_inputString);
            _inputString = string.Empty;
        }

        buttonsRect.x = buttonsRect.x + buttonsRect.width + 10;
        if (GUI.Button(buttonsRect, "Clear", btnStyle))
        {
            Log(CMDClear(), MessageType.OUTPUT);
        }


        /*// 底部第二排按扭
        buttonsRect2.y = inputRect.y + buttonsRect2.height + 6;
        buttonsRect2.x = inputRect.x + inputRect.width + 10;
        if (GUI.Button(buttonsRect2, "test", myButtonStyle))
        {
            TestScript ts = gameObject.GetComponent<TestScript>();
            if (ts)
            {
                ts.enabled = !ts.enabled;
            }
            else
            {
                ts = gameObject.AddComponent<TestScript>();
            }
        }

        buttonsRect2.x = buttonsRect2.x + buttonsRect2.width + 10;
        if (GUI.Button(buttonsRect2, "GMlist", myButtonStyle))
        {

        }*/

        //GUI.DragWindow();

    }

    void OnLogWindow(int windowID)
    {
        GUI.Box(scrollRect, string.Empty);
            
        innerRect.height = innerHeight < scrollRect.height ? scrollRect.height : innerHeight;
        GUI.skin.verticalScrollbarThumb.fixedWidth = 30;
        _logScrollPos = GUI.BeginScrollView(scrollRect, _logScrollPos, innerRect, false, true, horizontalScrollbarStyle, verticalScrollbarStyle);

        if (_messages != null || _messages.Count > 0)
        {
            Color oldColor = GUI.contentColor;

            messageLine.y = 0;
            foreach (Message m in _messages)
            {
                GUI.contentColor = m.color;

                guiContent.text = m.ToGUIString();

                messageLine.height = labelStyle.CalcHeight(guiContent, messageLine.width);

                GUI.Label(messageLine, guiContent, labelStyle);

                if (!string.IsNullOrEmpty(m.StackText))
                {
                    stackButtonRect.x = messageLine.width - 80;
                    stackButtonRect.y = messageLine.y;
                    if (GUI.Button(stackButtonRect, "stack"))
                    {
                        _curStackInfo = m.StackText;
                    }
                }

                messageLine.y += (messageLine.height + lineOffset);

                innerHeight = messageLine.y > scrollRect.height ? (int)messageLine.y : (int)scrollRect.height;
            }
            GUI.contentColor = oldColor;
        }

        GUI.EndScrollView();

        DrawBottomControls();
        // GUI.Button(new Rect(100,100,40,40),"dddd");
    }

    string BuildDisplayString()
    {
        if (_messages == null)
            return string.Empty;

        if (!dirty)
            return _displayString.ToString();

        dirty = false;

        _displayString.Length = 0;
        //foreach (Message m in _messages) {
        //  _displayString.AppendLine(m.ToString());
        //}
        for (int i = 0; i < _messages.Count; i++)
        {
            //collapse
            if (i > 0 && _messages[i].ToString() == _messages[i - 1].ToString())
            {
                continue;
            }
            _displayString.AppendLine(_messages[i].ToString());
        }
        return _displayString.ToString();
    }

    void OnCopyLogWindow(int windowID)
    {

        guiContent.text = BuildDisplayString();

        var calcHeight = GUI.skin.textArea.CalcHeight(guiContent, messageLine.width);

        innerRect.height = calcHeight < scrollRect.height ? scrollRect.height : calcHeight;

        _rawLogScrollPos = GUI.BeginScrollView(scrollRect, _rawLogScrollPos, innerRect, false, true);

        GUI.TextArea(innerRect, guiContent.text);

        GUI.EndScrollView();

        DrawBottomControls();
    }

    void OnWatchVarWindow(int windowID)
    {
        GUI.Box(scrollRect, string.Empty);

        innerRect.height = innerHeight < scrollRect.height ? scrollRect.height : innerHeight;

        _watchVarsScrollPos = GUI.BeginScrollView(scrollRect, _watchVarsScrollPos, innerRect, false, true);

        int line = 0;

        //    var bgColor = GUI.backgroundColor;

        nameRect.y = valueRect.y = 0;

        nameRect.x = messageLine.x;

        float totalWidth = messageLine.width - messageLine.x;
        float nameMin;
        float nameMax;
        float valMin;
        float valMax;
        float stepHeight;

        var textAreaStyle = GUI.skin.textArea;

        foreach (var kvp in _watchVarTable)
        {

            var nameContent = new GUIContent(string.Format("{0}:", kvp.Value.Name));
            var valContent = new GUIContent(kvp.Value.ToString());

            labelStyle.CalcMinMaxWidth(nameContent, out nameMin, out nameMax);
            textAreaStyle.CalcMinMaxWidth(valContent, out valMin, out valMax);

            if (nameMax > totalWidth)
            {
                nameRect.width = totalWidth - valMin;
                valueRect.width = valMin;
            }


            else if (valMax + nameMax > totalWidth)
            {
                valueRect.width = totalWidth - nameMin;
                nameRect.width = nameMin;
            }
            else
            {
                valueRect.width = valMax;
                nameRect.width = nameMax;
            }

            nameRect.height = labelStyle.CalcHeight(nameContent, nameRect.width);
            valueRect.height = textAreaStyle.CalcHeight(valContent, valueRect.width);

            valueRect.x = totalWidth - valueRect.width + nameRect.x;

            //      GUI.backgroundColor = line % 2 == 0 ? Color.black : Color.gray;
            GUI.Label(nameRect, nameContent);
            GUI.TextArea(valueRect, valContent.text);

            stepHeight = Mathf.Max(nameRect.height, valueRect.height) + 4;

            nameRect.y += stepHeight;
            valueRect.y += stepHeight;

            innerHeight = valueRect.y > scrollRect.height ? (int)valueRect.y : (int)scrollRect.height;

            line++;
        }

        //    GUI.backgroundColor = bgColor;

        GUI.EndScrollView();

        DrawBottomControls();
    }

    void OnSetting(int windowID)
    {

        DrawBottomControls();
    }
    #endregion


    void HandleLog(string logString, string stackTrace, LogType type)
    {
        string msgStack = type != LogType.Log ? stackTrace : string.Empty;

        Message msg = new Message(logString, (MessageType)type, msgStack);
        msg.handle = true;
        LogMessage(msg);

    }

    #region InternalFunctionality
    void LogMessage(Message msg)
    {
        ////统计重复出现的Msg
        //  bool isSameMsg = false;
        //  foreach(Message oldMsg in _messages)
        //  {
        //      if (string.Compare(oldMsg.ToString(), msg.ToString()) == 0)
        //      {
        //          //已经出现过该Msg
        //          oldMsg.MsgCountAdd();
        //          isSameMsg = true;
        //          break;
        //      }
        //  }
        //  //全新的消息
        //if(!isSameMsg)
        _messages.Add(msg);


        _logScrollPos.y = 50000.0f;
        _rawLogScrollPos.y = 50000.0f;

        dirty = true;

        if (/*msg.type == MessageType.WARNING ||*/ msg.type == MessageType.ERROR
            || msg.type == MessageType.EXCEPTION || msg.type == MessageType.ASSERT)
        {
            _shortMessages.Add(msg);
            _shortMessageShowTime = shortMessageShowTime;
        }
    }

    //--- Local version. Use the static version above instead.
    void ClearLog()
    {
        _messages.Clear();
        _shortMessages.Clear();
    }

    //--- Local version. Use the static version above instead.
    void RegisterCommandCallback(string commandString, DebugCommand commandCallback, string CMD_Discribes)
    {
        try
        {
            _cmdTable[commandString] = new DebugCommand(commandCallback);
            _cmdTableDiscribes.Add(commandString, CMD_Discribes);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    //--- Local version. Use the static version above instead.
    void UnRegisterCommandCallback(string commandString)
    {
        try
        {
            _cmdTable.Remove(commandString);
            _cmdTableDiscribes.Remove(commandString);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }

    //--- Local version. Use the static version above instead.
    void AddWatchVarToTable(WatchVarBase watchVar)
    {
        _watchVarTable[watchVar.Name] = watchVar;
    }

    //--- Local version. Use the static version above instead.
    void RemoveWatchVarFromTable(string name)
    {
        _watchVarTable.Remove(name);
    }

    void SendGM2MainGateway(string inputString)
    {
        if (inputString == "")
            return;
        if (inputString == "show map") {
            GamePosMap.gameObject.SetActive(true);
        }
        if (inputString == "hide map") {
            GamePosMap.gameObject.SetActive(false);
        }
        LogMessage(Message.InputMsg(string.Format("发送GM指令: {0}", inputString)));
       // if (NetworkManager.Instance.IsConnected("MainGateway"))
            NoticeManager.Instance.Dispatch(NoticeType.GM_Send_To_MainGateway, inputString);
      //  else
         //   LogMessage(Message.ErrorMsg("MainGateway未连接"));

        /*var input = inputString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        _history.Add(inputString);

        if (input.Length == 0)
        {
            //LogMessage(Message.Input(string.Empty));
            return;
        }

        LogMessage(Message.InputMsg(inputString));

        //input = Array.ConvertAll<string, string>(input, (low) => { return low.ToLower(); });
        var cmd = input[0];

        if (_cmdTable.ContainsKey(cmd))
        {
            Log(_cmdTable[cmd](input), MessageType.OUTPUT);
        }
        else
        {
            LogMessage(Message.ErrorMsg(string.Format("*** Unknown Command: {0} ***", cmd)));
        }*/
    }

    void SendGM2BusinessGateway(string inputString)
    {
        if (inputString == "")
            return;
        LogMessage(Message.InputMsg(string.Format("发送GM指令: {0}", inputString)));
        if (NetworkManager.Instance.IsConnected("BusinessGateway"))
            NoticeManager.Instance.Dispatch(NoticeType.GM_Send_To_BusinessGateway, inputString);
        else
            LogMessage(Message.ErrorMsg("BusinessGateway未连接"));
    }

    #endregion
}

/// <summary>
/// Base class for WatchVars. Provides base functionality.
/// </summary>
public abstract class WatchVarBase
{
    /// <summary>
    /// Name of the WatchVar.
    /// </summary>
    public string Name { get; private set; }

    protected object _value;

    public WatchVarBase(string name, object val)
        : this(name)
    {
        _value = val;
    }

    public WatchVarBase(string name)
    {
        Name = name;
        Register();
    }

    public void Register()
    {
        DebugConsole.RegisterWatchVar(this);
    }

    public void UnRegister()
    {
        DebugConsole.UnRegisterWatchVar(Name);
    }

    public object ObjValue
    {
        get { return _value; }
    }

    public override string ToString()
    {
        if (_value == null)
            return "<null>";

        return _value.ToString();
    }
}

/// <summary>
///
/// </summary>
public class WatchVar<T> : WatchVarBase
{
    public T Value
    {
        get { return (T)_value; }
        set { _value = value; }
    }

    public WatchVar(string name)
        : base(name)
    {

    }

    public WatchVar(string name, T val)
        : base(name, val)
    {

    }
}

public class FPSCounter
{
    public float current = 0.0f;

    public float updateInterval = 0.5f;

    float accum = 0; // FPS accumulated over the interval
    int frames = 100; // Frames drawn over the interval
    float timeleft; // Left time for current interval

    float delta;

    public FPSCounter()
    {
        timeleft = updateInterval;
    }

    public IEnumerator Update()
    {
        while (true)
        {
            delta = Time.deltaTime;

            timeleft -= delta;
            accum += Time.timeScale / delta;
            ++frames;

            // Interval ended - update GUI text and start new interval
            if (timeleft <= 0.0f)
            {
                // display two fractional digits (f2 format)
                current = accum / frames;
                timeleft = updateInterval;
                accum = 0.0f;
                frames = 0;
            }

            yield return null;
        }
    }
}