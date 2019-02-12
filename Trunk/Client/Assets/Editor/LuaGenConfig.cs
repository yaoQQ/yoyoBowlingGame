using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;
using Spine;

public static class LuaGenConfig
{
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharpList = new List<Type>()
    {
        typeof(DG.Tweening.AutoPlay),
        typeof(DG.Tweening.AxisConstraint),
        typeof(DG.Tweening.Ease),
        typeof(DG.Tweening.LogBehaviour),
        typeof(DG.Tweening.LoopType),
        typeof(DG.Tweening.PathMode),
        typeof(DG.Tweening.PathType),
        typeof(DG.Tweening.RotateMode),
        typeof(DG.Tweening.ScrambleMode),
        typeof(DG.Tweening.TweenType),
        typeof(DG.Tweening.UpdateType),
        typeof(DG.Tweening.DOTween),
        typeof(DG.Tweening.DOVirtual),
        typeof(DG.Tweening.EaseFactory),
        typeof(DG.Tweening.Tweener),
        typeof(DG.Tweening.Tween),
        typeof(DG.Tweening.Sequence),
        typeof(DG.Tweening.TweenParams),
        typeof(DG.Tweening.Core.ABSSequentiable),

        typeof(DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>),

        typeof(DG.Tweening.TweenCallback),
        typeof(DG.Tweening.TweenExtensions),
        typeof(DG.Tweening.TweenSettingsExtensions),
        typeof(DG.Tweening.ShortcutExtensions),
        typeof(DG.Tweening.ShortcutExtensions43),
        typeof(DG.Tweening.ShortcutExtensions46),
        typeof(DG.Tweening.ShortcutExtensions50),
        
        //System
        typeof(DateTime),

        //UnityEngine
		typeof(KeyCode),
        typeof(TouchPhase),
        typeof(Animator),
        typeof(Camera),
        typeof(Collider2D),
        typeof(Collision2D),
        typeof(Color),
        typeof(ContactPoint2D),
        typeof(GameObject),
        typeof(Image),
        typeof(Material),
        typeof(MeshFilter),
        typeof(MeshRenderer),
        typeof(Physics2D),
        typeof(PlayerPrefs),
        typeof(PolygonCollider2D),
        typeof(Quaternion),
        typeof(RaycastHit),
        typeof(RectTransform),
        typeof(Renderer),
        typeof(Rigidbody2D),
        typeof(Sprite),
        typeof(SpriteRenderer),
        typeof(Time),
        typeof(ToggleGroup),
        typeof(TrailRenderer),
        typeof(Transform),
        typeof(Vector2),
        typeof(Vector3),

        //自定义
        typeof(LoadManager.LoadedFinishDelegate),
        typeof(NumberPickerWidget.SetTextDelegate),
    };

    [CSharpCallLua]
    public static List<Type> CSharpCallLuaList = new List<Type>()
    {
        typeof(Vector2),
        typeof(Action),
        typeof(Action<bool>),
        typeof(Action<GameObject>),
        typeof(Action<UnityEngine.Object>),
		typeof(GameObject),
        typeof(Action<PointerEventData>),
        typeof(Action<int>),
        typeof(Action<uint>),
        typeof(Action<SceneCell,LuaTable>),
        typeof(PointerEventData),
        typeof(Action<GameObject, object, int>),
        typeof(Action<GameObject, object, int,int>),
        typeof(LoadManager.LoadedFinishDelegate),
        typeof(DG.Tweening.TweenCallback),
        typeof(Action<object>),
        typeof(Action<string>),
        typeof(Action<List<object>>),
        typeof(Action<EffectControler>),
        typeof(Action<TrackEntry>),
        typeof(UnityAction<string>),
        typeof(UnityAction<bool>),
        typeof(EventTriggerListener.BaseDelegate),
        typeof(Action<BaseEventData>),
        typeof(Action<PinchEventData>),
        typeof(Action<WWW>),
        typeof(Action<int, int, int, Sprite>),
        typeof(Action<byte[]>),
        typeof(Action<Texture2D>),
        typeof(Action<Texture2D, string>),
        typeof(Action<Sprite, byte[]>),
        typeof(Action<Collision2D>),
        typeof(Action<Collider2D>),
        typeof(Action<int, int, float, float>),
        typeof(Action<int, int>),
        typeof(NumberPickerWidget.SetTextDelegate),
    };
}