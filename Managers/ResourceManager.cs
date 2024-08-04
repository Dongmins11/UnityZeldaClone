using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string _strPath) where T : Object
    {
        return Resources.Load<T>(_strPath);
    }

    public T[] LoadAll<T>(string _strPath) where T : Object
    {
        return Resources.LoadAll<T>(_strPath);
    }

    public Object Load(string _strPath)
    {
        return Resources.Load(_strPath);
    }

    public GameObject GetPrefab(string _strPath)
    {
        GameObject Prefab = Load<GameObject>($"Prefabs/{_strPath}");

        if (null == Prefab)
            return null;

        return Prefab;
    }

    public T GetPrefab<T>(string _strPath) where T : Object
    {
        T Prefab = Load<T>($"Prefabs/{_strPath}");

        if (null == Prefab)
            return null;

        return Prefab;
    }

    public T[] GetPrefabs<T>(string _strPath) where T : Object
    {
        T[] Prefabs = LoadAll<T>($"Prefabs/{_strPath}");

        if(null == Prefabs)
            return null;

        return Prefabs;
    }


    public Sprite Get_ItemSprite(string _strTexturePath)
    {
        return Load<Sprite>($"Textures/UI/ItemUI_Texture/{_strTexturePath}");
    }



    public GameObject CreatePrefab(string _strPath, Transform _Parent = null)
    {
        GameObject CreateObejct = Load<GameObject>($"Prefabs/{_strPath}");

        if (null == CreateObejct)
            return null;

        GameObject go = Object.Instantiate(CreateObejct, _Parent);

        Util.ObjectCloneNameRemove(go);

        return go;
    }

    public GameObject CreatePrefab(string _strPath, Vector3 _Position, Quaternion _quaternion)
    {
        GameObject CreateObejct = Load<GameObject>($"Prefabs/{_strPath}");

        if (null == CreateObejct)
            return null;

        return Object.Instantiate(CreateObejct, _Position, _quaternion);
    }

    public void Destroy(GameObject _go)
    {
        if (null != _go)
        {
            GameObject.Destroy(_go);
            _go = null;
        }
    }

}
