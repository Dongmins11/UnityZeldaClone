using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    protected virtual void Init()
    {
        Object Obj = GameObject.FindObjectOfType(typeof(EventSystem));

        if (null == Obj)
            GameManager.Resources.CreatePrefab("UI/EventSystem").name = "@EventSystem";
    }


    public abstract void Clear();

    void Start()
    {
        Init();    
    }


}
