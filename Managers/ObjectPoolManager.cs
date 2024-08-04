using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager
{
    Transform m_Root = null;

    Dictionary<string, Queue<GameObject>> m_PoolDictionary = new Dictionary<string, Queue<GameObject>>();

    Dictionary<string, GameObject> m_PoolObject = new Dictionary<string, GameObject>();

    Dictionary<string, GameObject> m_PoolParent = new Dictionary<string, GameObject>();

    GameObject CreateObject(GameObject _Prefab)
    {
        return UnityEngine.Object.Instantiate(_Prefab, new Vector3(0f,0f,0f), Quaternion.identity);
    }

    void InitPooling()
    {
        if (null == m_Root)
        {
            m_Root = new GameObject("@PoolRoot").transform;
            UnityEngine.Object.DontDestroyOnLoad(m_Root);
        }
    }

    bool FindPoolQueue(string _strName, out Queue<GameObject> _Queue)
    {
        if (!m_PoolDictionary.TryGetValue(_strName, out _Queue))
            return false;

        return true;
    }


    //풀을 생성시키기위한 용도의 함수
    public void CreatePool(string _strName, GameObject _Prefab, int _isize = 5)
    {
        //Pool오브젝트의 최상위 부모 초기화 및 변수 동적 할당
        InitPooling();

        //새롭게들어온 Key값이 이미 있다면 return
        if (m_PoolDictionary.ContainsKey(_strName))
            return;

        //새롭게 생성할 Pool객체를 담을 Queue
        Queue<GameObject> TempPool = new Queue<GameObject>();

        //Pool객체들의 부모 오브젝트(이름 별로 정리하기위해)
        GameObject ParentObject = new GameObject(_strName + "_Pool");

        //외부에서 들어온 매개변수 값을 반복해서 생성시키기위해
        for (int i = 0; i < _isize; ++i)
        {
            //프리팹을 받아서 새롭게 인스턴스화
            GameObject InputObject = CreateObject(_Prefab);

            //오브젝트 뒤에 (1) ~ (n) 붙여있는걸 지우기 위한 함수
            Util.ObjectCloneNameRemove(InputObject);

            //생성된 오브젝트 비활성화
            InputObject.SetActive(false);

            //부모 오브젝트의 하위를 두기위함
            InputObject.transform.SetParent(ParentObject.transform);

            //Poll Queue안 삽입
            TempPool.Enqueue(InputObject);
        }

        //부모 오브젝트를 Root오브젝트 하위에 두기위함
        ParentObject.transform.SetParent(m_Root);

        //부모 오브젝트를 관리하기위해 부모를 삽입함
        m_PoolParent.Add(_strName, ParentObject);

        //Get해 올 때 만약 Pool안의 오브젝트가 부족할 것을 생각해 프리팹 삽입
        m_PoolObject.Add(_strName, _Prefab);

        //Pool객체를 Dictionary 변수에 삽입시켜 관리
        m_PoolDictionary.Add(_strName, TempPool);

    }

    public void CreatePool(string _strName, GameObject _Prefab, Action<GameObject> _Funiton, int _isize = 5)
    {
        InitPooling();

        if (m_PoolDictionary.ContainsKey(_strName))
            return;

        Queue<GameObject> TempPool = new Queue<GameObject>();

        GameObject ParentObject = new GameObject(_strName + "_Pool");

        for (int i = 0; i < _isize; ++i)
        {
            GameObject InputObject = CreateObject(_Prefab);

            Util.ObjectCloneNameRemove(InputObject);

            _Funiton?.Invoke(InputObject);

            InputObject.SetActive(false);
            InputObject.transform.SetParent(ParentObject.transform);

            TempPool.Enqueue(InputObject);
        }

        ParentObject.transform.SetParent(m_Root);

        m_PoolParent.Add(_strName, ParentObject);

        m_PoolObject.Add(_strName, _Prefab);

        m_PoolDictionary.Add(_strName, TempPool);

    }

    public int FindObjectCountInQueue(string _strName)
    {
        Queue<GameObject> tempQueue;

        if (false == FindPoolQueue(_strName, out tempQueue))
            return -1;

        return tempQueue.Count;
    }

    public GameObject Get_Object(string _strName)
    {
        Queue<GameObject> TempQueue;

        if (false == FindPoolQueue(_strName, out TempQueue))
            return null;

        if (0 < TempQueue.Count)
        {
            GameObject PoolObject = TempQueue.Dequeue();
            PoolObject.SetActive(true);

            if (PoolObject.CompareTag("Monster"))
                PoolObject.transform.SetParent(null);
            else
                PoolObject.transform.SetParent(m_Root);

            return PoolObject;
        }
        else
        {
            GameObject NewObject = null;

            if (!m_PoolObject.TryGetValue(_strName, out NewObject))
            {
                return null;
            }

            GameObject TempObject = CreateObject(NewObject);

            TempObject.SetActive(true);
            TempObject.transform.SetParent(m_Root);

            return TempObject;
        }
    }


    public void RelaseObject(string _strName, GameObject _RetrunObject)
    {
        Queue<GameObject> TempQueue;
        GameObject ParentObject;

        if (false == FindPoolQueue(_strName, out TempQueue))
        {
            if (null != _RetrunObject)
                UnityEngine.Object.Destroy(_RetrunObject);

            return;
        }

        m_PoolParent.TryGetValue(_strName, out ParentObject);

        _RetrunObject.SetActive(false);

        _RetrunObject.transform.SetParent(ParentObject.transform);

        TempQueue.Enqueue(_RetrunObject);

    }

    public void DestroyParent(string _strName)
    {
        GameObject TempParent = null;
        Queue<GameObject> Pool = null;

        if (!m_PoolParent.TryGetValue(_strName, out TempParent))
            return;

        m_PoolObject.Remove(_strName);

        m_PoolDictionary.TryGetValue(_strName, out Pool);

        foreach (GameObject iter in Pool)
        {
            iter.transform.SetParent(null);

            UnityEngine.Object.Destroy(iter);
        }

        m_PoolParent.Clear();

        UnityEngine.Object.Destroy(TempParent);
    }

    public void AllClear()
    {
    }
}
