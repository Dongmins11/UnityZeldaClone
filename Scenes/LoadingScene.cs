using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : BaseScene
{
    [SerializeField]
    private Image Loading_Progress;

    [SerializeField]
    private Material BackGround_Mat;

    [SerializeField]
    private UI_Scene_Loading_Scene LoadScene_UI;

    bool bLoadAnimStartCheck = false;
    bool bLoadAnimEndCheck = false;

    IEnumerator Load = null;

    public override void Clear()
    {
        if (Define.Scene.Loading != GameManager.Scene.PreSceneIndex)
            GameManager.Scene.UnloadScene(GameManager.Scene.PreSceneIndex);
    }

    protected override void Init()
    {
        base.Init();

        Load = LoadScene();

        GameManager.UIContents.Fade_Init();

        SceneType = Define.Scene.Loading;

        Loading_Progress.fillAmount = 0.0f;

        StartCoroutine(StartAnimation());

    }

    private IEnumerator StartAnimation()
    {
        while (false == LoadScene_UI.bIsLoadStartOk)
        {
            yield return null;

            if (false == bLoadAnimStartCheck)
            {
                LoadScene_UI.StartAnim();
                bLoadAnimStartCheck = true;
            }
        }

        bLoadAnimStartCheck = false;
        StartCoroutine(Load);
    }


    IEnumerator LoadScene()
    {
        yield return null;

        if (Define.Scene.Unknown == GameManager.Scene.NextSceneIndex)
        {
            yield break;
        }

        AsyncOperation Async = GameManager.Scene.LoadSceneAsync(GameManager.Scene.NextSceneIndex);
        Async.allowSceneActivation = false;

        float fTime = 0.0f;

        while (false == Async.isDone)
        {
            yield return null;

            fTime += Time.deltaTime;

            if (0.9f > Async.progress)
            {
                Loading_Progress.fillAmount = Mathf.Lerp(Loading_Progress.fillAmount, Async.progress, fTime);

                if (Loading_Progress.fillAmount >= Async.progress)
                    fTime = 0.0f;
            }
            else
            {
                Loading_Progress.fillAmount = Mathf.Lerp(Loading_Progress.fillAmount, 1.0f, fTime);

                if (false == bLoadAnimEndCheck)
                {
                    LoadScene_UI.EndAnim();
                    bLoadAnimEndCheck = true;
                }


                if (1.0f == Loading_Progress.fillAmount && true == LoadScene_UI.bIsLoadEndOk)
                {
                    bLoadAnimEndCheck = false;
                    LoadScene_UI.InitLoadBoolean();
                    Async.allowSceneActivation = true;
                    yield break;
                }


            }
        }
    }

}
