using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene
    {
        get
        { return GameObject.FindObjectOfType<BaseScene>(); }
    }

    public Define.Scene PreSceneIndex { get; private set; }
    public Define.Scene NextSceneIndex { get; private set; } = Define.Scene.Unknown;

    string GetSceneName(Define.Scene _TypeName)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), _TypeName);

        return name;
    }

    public void UnloadScene(Define.Scene _TypeName)
    {
        SceneManager.UnloadSceneAsync(GetSceneName(_TypeName));
    }

    public void LoadScene(Define.Scene _type)
    {
        PreSceneIndex = CurrentScene.SceneType;

        GameManager.Clear();

        SceneManager.LoadScene(GetSceneName(_type));
    }

    public AsyncOperation LoadSceneAsync(Define.Scene _Type)
    {
        return SceneManager.LoadSceneAsync(GetSceneName(_Type));
    }

    public void LoadToNextScene(Define.Scene _Type)
    {
        PreSceneIndex = CurrentScene.SceneType;
        NextSceneIndex = _Type;

        GameManager.Clear();

        SceneManager.LoadScene(GetSceneName(Define.Scene.Loading));
    }

    public AsyncOperation LoadToNextSceneAsync(Define.Scene _Type)
    {
        PreSceneIndex = CurrentScene.SceneType;
        NextSceneIndex = _Type;

        GameManager.Clear();

        return SceneManager.LoadSceneAsync(GetSceneName(Define.Scene.Loading));
    }


    public void Clear()
    {
        CurrentScene.Clear();
    }


}
