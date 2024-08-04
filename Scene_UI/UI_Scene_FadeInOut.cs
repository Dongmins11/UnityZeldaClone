using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UI_Scene_FadeInOut : UI_Base
{
    enum Images
    {
        FadeImage,
    }

    public override void init()
    {
        Bind<Image>(typeof(Images));
        GetImage((int)Images.FadeImage).color = new Color(0f,0f,0f,0f);
    }

    public void FadeIn(Action _Function = null)
    {
        StartCoroutine(Fade(true, _Function));
    }

    public void FadeOut(Action _Function = null) 
    {
        StartCoroutine(Fade(false, _Function));
    }

    public void FadeInit()
    {
        GetImage((int)Images.FadeImage).color = new Color(0f, 0f, 0f, 0f);
    }

    private IEnumerator Fade(bool isFadeIn, Action _Funtion)
    {
        if (isFadeIn)
        {
            Color TempColor = GetImage((int)Images.FadeImage).color;
            TempColor.a = 0f;
            GetImage((int)Images.FadeImage).color = TempColor;

            Tween tween = GetImage((int)Images.FadeImage).DOFade(1f, 1f);
            yield return tween.WaitForCompletion();

            _Funtion?.Invoke();
        }
        else
        {
            Color TempColor = GetImage((int)Images.FadeImage).color;
            TempColor.a = 1f;
            GetImage((int)Images.FadeImage).color = TempColor;
            GetImage((int)Images.FadeImage).gameObject.SetActive(true);
            Tween tween = GetImage((int)Images.FadeImage).DOFade(0f, 7f);
            yield return tween.WaitForCompletion();

            _Funtion?.Invoke();
        }
    }
}
