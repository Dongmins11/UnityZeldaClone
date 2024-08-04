using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> m_Objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void init();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        m_Objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; ++i)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(this.gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(this.gameObject, names[i], true);
        }
    }
    protected T Get<T>(int iIndex) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        if (false == m_Objects.TryGetValue(typeof(T), out objects))
            return null;

        return objects[iIndex] as T;
    }

    protected GameObject GetObject(int index) { return Get<GameObject>(index); }
    protected Text GetText(int iIndex) { return Get<Text>(iIndex); }
    protected Button GetButton(int iIndex) { return Get<Button>(iIndex); }
    protected Image GetImage(int iIndex) { return Get<Image>(iIndex); }

    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler Event = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch(type)
        {
            case Define.UIEvent.Click:
                Event.m_OnClickHandler -= action;
                Event.m_OnClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                Event.m_OnDragHandler -= action;
                Event.m_OnDragHandler += action;
                break;
            case Define.UIEvent.Down:
                Event.m_OnDownHandler -= action;
                Event.m_OnDownHandler += action;
                break;
            case Define.UIEvent.Up:
                Event.m_OnUpHandler -= action;
                Event.m_OnUpHandler += action;
                break;
        }
    }
}
