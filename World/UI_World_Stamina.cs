using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_World_Stamina : UI_Base
{
    GameObject My_Cam;

    enum Images
    {
        Stamina_Object,
        Stamina,
        Stamina_Two,
    }

    float fFadeSpeed = 5.0f;
    bool bCurActive = false;
    bool bPreActive = true;

    IEnumerator FadeIn_Funtion = null;
    IEnumerator FadeOut_Funtion = null;
    Coroutine My_Corutine = null;
    private void Awake()
    {
        Bind<Image>(typeof(Images));

        FadeOut_Funtion = Fade_Stamina();

        bCurActive = false;

        StartCoroutine(LateActiveFalse());
    }

    private void Start()
    {
        My_Cam = PlayerManager.cameraManager.mainCamera.gameObject;
    }

    public void Stamina_Active(bool _bActive)
    {
        bCurActive = _bActive;

        if (bPreActive != bCurActive)
        {
            bPreActive = bCurActive;

            if (true == bCurActive)
            {
                gameObject.SetActive(true);

                for (int i = 0; i <= (int)Images.Stamina_Two; ++i)
                {
                    Color TmpeColor = GetImage(i).color;
                    TmpeColor.a = 1f;
                    GetImage(i).color = TmpeColor;
                }
            }
            else
            {
                if (false == gameObject.activeSelf)
                    return;

                StartCoroutine(Fade_Stamina());
            }
        }
    }


    public void Stamina_ActiveTrue()
    {
        if (true == gameObject.activeSelf)
            return;

        gameObject.SetActive(true);

        for (int i = 0; i <= (int)Images.Stamina_Two; ++i)
        {
            Color TmpeColor = GetImage(i).color;
            TmpeColor.a = 1f;
            GetImage(i).color = TmpeColor;
        }
    }


    public void Stamina_ActiveFalse()
    {
        if (false == gameObject.activeSelf)
            return;

        My_Corutine = StartCoroutine(Fade_Stamina());
    }

    private IEnumerator Fade_Stamina()
    {
        if (true == bCurActive)
            yield break;

        bCurActive = true;
        float fRatio = 0.0f;

        while (true)
        {
            yield return null;

            fRatio += Time.deltaTime * fFadeSpeed;

            for (int i = 0; i <= (int)Images.Stamina_Two; ++i)
            {
                Color TmpeColor = GetImage(i).color;

                TmpeColor.a = Mathf.Lerp(1f, 0f, fRatio);

                GetImage(i).color = TmpeColor;
            }

            if (1.0f <= fRatio)
            {
                bCurActive = false;
                gameObject.SetActive(false);
                yield break;
            }
        }
    }


    public void Stamina_Set_Position(Transform _transform)
    {
        transform.position = _transform.position;
    }

    void LateUpdate()
    {
        if (null == My_Cam)
            My_Cam = PlayerManager.cameraManager.camTransform.gameObject;

        Util.Billbord_UI(transform, My_Cam.transform, true);
    }

    private IEnumerator LateActiveFalse()
    {
        yield return null;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        bCurActive = false;
    }

    public override void init()
    {
    }
}
