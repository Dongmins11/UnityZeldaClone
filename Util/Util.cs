using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{

    // 이름 문자열로 자식 개체 찾기
    // 주의 
    // 1. 이름이 같은 개체가 여러개라면 구조상 가장 상위 개체가 반환됨 (의도된 부분)
    // 2. Contains가 아니라 Equal을 썼기 때문에 이름 정확히 풀네임으로 입력해줘야 함
    // 3. null 검사 꼭 해주세요

    public static Transform FindChildWithName(Transform _Parent, String _strName)
    {
        Transform[] Children;
        Children = _Parent.GetComponentsInChildren<Transform>();

        foreach (Transform Child in Children)
        {
            if (Child.name.Equals(_strName))
                return Child;
        }

        return null;
    }

    public static void ObjectCloneNameRemove(GameObject go)
    {
        int index = go.name.IndexOf("(Clone)");

        if (0 < index)
            go.name = go.name.Substring(0, index);
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T Component = go.GetComponent<T>();

        if (null == Component)
            Component = go.AddComponent<T>();

        return Component;
    }

    public static GameObject FindChild(GameObject _GameObject, string _name = null, bool Recusive = false)
    {
        Transform transform = FindChild<Transform>(_GameObject, _name, Recusive);

        if (null == transform)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject _GameObject, string _name = null, bool Recusive = false) where T : UnityEngine.Object
    {
        if (null == _GameObject)
            return null;

        if (false == Recusive)
        {
            for (int i = 0; i < _GameObject.transform.childCount; ++i)
            {
                Transform transform = _GameObject.transform.GetChild(i);

                if (string.IsNullOrEmpty(_name) || transform.name == _name)
                {
                    T component = transform.GetComponent<T>();

                    if (null != component)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in _GameObject.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(_name) || component.name == _name)
                    return component;
            }
        }

        return null;
    }

    public static int Get_MiddleIndex(int _iStartIndex, int _iEndIndex)
    {
        int iResult = _iStartIndex + _iEndIndex;

        if (0 == (iResult & 1))
            iResult /= 2;
        else
            iResult = (iResult / 2) + 1;

        return iResult;
    }

    public static bool FindKeyAction(Action _MyAction, Action _FindFunction)
    {
        if (null == _MyAction)
            return false;

        Delegate[] TempDelegates = _MyAction.GetInvocationList();

        foreach (var iter in TempDelegates)
        {
            if (_MyAction.Method.Name == iter.Method.Name)
                return true;
        }

        return false;
    }

    public static void MoveToMaterial(Material _Material, float _fOffset)
    {
        _Material.SetTextureOffset("_BaseMap", new Vector2(_fOffset, 0.0f));
    }

    public static void Billbord_UI(Transform _MyTrasnform, Transform _CamTrans, bool _bIsYActive = false)
    {
        if (null == _CamTrans)
            return;

        Vector3 Billboard_Direction = _MyTrasnform.position - _CamTrans.transform.position;

        if (false == _bIsYActive)
            Billboard_Direction.y = 0.0f;
            
        Billboard_Direction.Normalize();

        Quaternion LookRot = Quaternion.LookRotation(Billboard_Direction);

        _MyTrasnform.rotation = LookRot;
    }

}
