using System;
using System.Collections.Generic;
using UnityEngine;

public class AndroidStatusBar
{
    // Enums
    public enum States
    {
        Unknown,
        Visible,
        VisibleOverContent,
        TranslucentOverContent,
        Hidden,
    }

    // Constants
    private const uint DEFAULT_BACKGROUND_COLOR = 0xff000000;



#if !UNITY_EDITOR && UNITY_ANDROID
    // Original Android flags
    //默认标记
    private const int VIEW_SYSTEM_UI_FLAG_VISIBLE = 0;                                        // Added in API 14 (Android 4.0.x): Status bar visible (the default)
    //低调模式, 会隐藏不重要的状态栏图标
    private const int VIEW_SYSTEM_UI_FLAG_LOW_PROFILE = 1;                                    // Added in API 14 (Android 4.0.x): Low profile for games, book readers, and video players; the status bar and/or navigation icons are dimmed out (if visible)
    //隐藏导航栏
    private const int VIEW_SYSTEM_UI_FLAG_HIDE_NAVIGATION = 2;                                // Added in API 14 (Android 4.0.x): Hides all navigation. Cleared when theres any user interaction.
    //状态栏隐藏，效果同设置WindowManager.LayoutParams.FLAG_FULLSCREEN
    private const int VIEW_SYSTEM_UI_FLAG_FULLSCREEN = 4;                                     // Added in API 16 (Android 4.1.x): Hides status bar. Does nothing in Unity (already hidden if "status bar hidden" is checked)
    //保持整个View稳定, 常和控制System UI悬浮, 隐藏的Flags共用, 使View不会因为System UI的变化而重新layout
    private const int VIEW_SYSTEM_UI_FLAG_LAYOUT_STABLE = 256;                                // Added in API 16 (Android 4.1.x): ?
    //视图延伸至导航栏区域，导航栏上浮于视图之上
    private const int VIEW_SYSTEM_UI_FLAG_LAYOUT_HIDE_NAVIGATION = 512;                       // Added in API 16 (Android 4.1.x): like HIDE_NAVIGATION, but for layouts? it causes the layout to be drawn like that, even if the whole view isn't (to avoid artifacts in animation)
    //视图延伸至状态栏区域，状态栏上浮于视图之上
    private const int VIEW_SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN = 1024;                           // Added in API 16 (Android 4.1.x): like FULLSCREEN, but for layouts? it causes the layout to be drawn like that, even if the whole view isn't (to avoid artifacts in animation)
    //沉浸模式, 隐藏状态栏和导航栏, 并且在第一次会弹泡提醒, 并且在状态栏区域滑动可以呼出状态栏（这样会系统会清楚之前设置的View.SYSTEM_UI_FLAG_FULLSCREEN或View.SYSTEM_UI_FLAG_HIDE_NAVIGATION标志）。使之生效，需要和View.SYSTEM_UI_FLAG_FULLSCREEN，View.SYSTEM_UI_FLAG_HIDE_NAVIGATION中的一个或两个同时设置。
    private const int VIEW_SYSTEM_UI_FLAG_IMMERSIVE = 2048;                                   // Added in API 19 (Android 4.4): like HIDE_NAVIGATION, but interactive (it's a modifier for HIDE_NAVIGATION, needs to be used with it)
    //与上面唯一的区别是, 呼出隐藏的状态栏后不会清除之前设置的View.SYSTEM_UI_FLAG_FULLSCREEN或View.SYSTEM_UI_FLAG_HIDE_NAVIGATION标志，在一段时间后将再次隐藏系统栏
    private const int VIEW_SYSTEM_UI_FLAG_IMMERSIVE_STICKY = 4096;                            // Added in API 19 (Android 4.4): tells that HIDE_NAVIGATION and FULSCREEN are interactive (also just a modifier)
 
    private static int WINDOW_FLAG_FULLSCREEN = 0x00000400;
    private static int WINDOW_FLAG_FORCE_NOT_FULLSCREEN = 0x00000800;
    private static int WINDOW_FLAG_LAYOUT_IN_SCREEN = 0x00000100;
    private static int WINDOW_FLAG_TRANSLUCENT_STATUS = 0x04000000;
    private static int WINDOW_FLAG_TRANSLUCENT_NAVIGATION = 0x08000000;
    private static int WINDOW_FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS = -2147483648; // 0x80000000; // Added in API 21 (Android 5.0): tells the Window is responsible for drawing the background for the system bars. If set, the system bars are drawn with a transparent background and the corresponding areas in this window are filled with the colors specified in getStatusBarColor() and getNavigationBarColor()
 
    // Current values
    private static int systemUiVisibilityValue;
    private static int flagsValue;
#endif

    //Properties
    private static States _statusBarState;
    private static uint _statusBarColor = DEFAULT_BACKGROUND_COLOR;
    private static bool _isStatusBarTranslucent;
    private static bool _dimmed;

    static AndroidStatusBar()
    {
        applyUIStates();
        applyUIColors();
    }

    private static void applyUIStates()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        int newFlagsValue = 0;
        int newSystemUiVisibilityValue = 0;

        // Apply dim values
        if (_dimmed) newSystemUiVisibilityValue |= VIEW_SYSTEM_UI_FLAG_LOW_PROFILE;

        // Apply color values
        if (_statusBarColor != DEFAULT_BACKGROUND_COLOR) newFlagsValue |= WINDOW_FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS;

        // Apply status bar values
        switch (_statusBarState)
        {
            case States.Visible:
                _isStatusBarTranslucent = false;
                newFlagsValue |= WINDOW_FLAG_FORCE_NOT_FULLSCREEN;
                break;
            case States.VisibleOverContent:
                _isStatusBarTranslucent = false;
                newFlagsValue |= WINDOW_FLAG_FORCE_NOT_FULLSCREEN | WINDOW_FLAG_LAYOUT_IN_SCREEN;
                newSystemUiVisibilityValue |= VIEW_SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN;
                break;
            case States.TranslucentOverContent:
                _isStatusBarTranslucent = true;
                newFlagsValue |= WINDOW_FLAG_FORCE_NOT_FULLSCREEN | WINDOW_FLAG_LAYOUT_IN_SCREEN | WINDOW_FLAG_TRANSLUCENT_STATUS;
                newSystemUiVisibilityValue |= VIEW_SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN;
                break;
            case States.Hidden:
                newFlagsValue |= WINDOW_FLAG_FULLSCREEN | WINDOW_FLAG_LAYOUT_IN_SCREEN;
                if (_isStatusBarTranslucent) newFlagsValue |= WINDOW_FLAG_TRANSLUCENT_STATUS;
                break;
        }
        if (Screen.fullScreen) Screen.fullScreen = false;

        // Applies everything natively
        setFlags(newFlagsValue);
        setSystemUiVisibility(newSystemUiVisibilityValue);
#endif
    }

    private static void applyUIColors()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        runOnAndroidUiThread(applyUIColorsAndroidInThread);
#endif
    }

#if !UNITY_EDITOR && UNITY_ANDROID
    private static void runOnAndroidUiThread(Action target)
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                activity.Call("runOnUiThread", new AndroidJavaRunnable(target));
            }
        }
    }
 
    private static void setSystemUiVisibility(int value)
    {
        if (systemUiVisibilityValue != value)
        {
            systemUiVisibilityValue = value;
            runOnAndroidUiThread(setSystemUiVisibilityInThread);
        }
    }
 
    private static void setSystemUiVisibilityInThread()
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (var window = activity.Call<AndroidJavaObject>("getWindow"))
                {
                    using (var view = window.Call<AndroidJavaObject>("getDecorView"))
                    {
                        view.Call("setSystemUiVisibility", systemUiVisibilityValue);
                    }
                }
            }
        }
    }
 
    private static void setFlags(int value)
    {
        if (flagsValue != value)
        {
            flagsValue = value;
            runOnAndroidUiThread(setFlagsInThread);
        }
    }
 
    private static void setFlagsInThread()
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (var window = activity.Call<AndroidJavaObject>("getWindow"))
                {
                    window.Call("setFlags", flagsValue, -1); // (int)0x7FFFFFFF
                }
            }
        }
    }
 
    private static void applyUIColorsAndroidInThread()
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (var window = activity.Call<AndroidJavaObject>("getWindow"))
                {
                    window.Call("setStatusBarColor", unchecked((int)_statusBarColor));
                }
            }
        }
    }
#endif
    public static States statusBarState
    {
        get { return _statusBarState; }
        set
        {
            if (_statusBarState != value)
            {
                _statusBarState = value;
                applyUIStates();
            }
        }
    }

    public static bool dimmed
    {
        get { return _dimmed; }
        set
        {
            if (_dimmed != value)
            {
                _dimmed = value;
                applyUIStates();
            }
        }
    }

    public static uint statusBarColor
    {
        get { return _statusBarColor; }
        set
        {
            if (_statusBarColor != value)
            {
                _statusBarColor = value;
                applyUIColors();
                applyUIStates();
            }
        }
    }
}