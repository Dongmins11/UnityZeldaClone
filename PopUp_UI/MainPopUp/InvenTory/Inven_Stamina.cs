using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inven_Stamina : UI_Base
{
    enum Images
    {
        Stamina,
        Stamina_Two,
    }

    public enum StaminaType
    {
        StaminaType,
        StaminaType_Two
    }


    float fStaminaList;
    float fStaminaListTwo;

    StaminaType My_Type;

    UI_World_Stamina Wolrd_Parent = null;

    public override void init()
    {
        Bind<Image>(typeof(Images));

        StaminaStateChange(StaminaType.StaminaType);

        Wolrd_Parent = transform.parent.GetComponent<UI_World_Stamina>();
     
    }

    private void StaminaTwoFillAmount(float _fValue, float _fMaxValue)
    {
        float fStamina = _fValue / 2.0f;
        float fStaminaRatio = (_fValue * 2.0f) / _fMaxValue;


        if (0.5 <= fStaminaRatio)
        {
            fStaminaList = fStamina / (_fMaxValue / 2.0f);
            GetImage((int)Images.Stamina_Two).fillAmount = fStaminaList;
        }
        else
        {
            fStaminaListTwo = fStaminaRatio * 2.0f;
            GetImage((int)Images.Stamina).fillAmount = fStaminaListTwo;
        }
    }


    private void StaminaFillAmount(float _fValue, float _fMaxValue)
    {
        _fMaxValue = Mathf.Max(1, _fMaxValue);

        float fRatio = _fValue / _fMaxValue;

        fRatio = Mathf.Max(0, fRatio);

        if (1.0 <= GetImage((int)Images.Stamina).fillAmount)
        {
            if (null != Wolrd_Parent)
                Wolrd_Parent.Stamina_ActiveFalse();
        }
        else
        {
            if(null != Wolrd_Parent)
                Wolrd_Parent.Stamina_ActiveTrue();
        }

        GetImage((int)Images.Stamina).fillAmount = fRatio;
    }

    public void StaminaStateChange(StaminaType _Type)
    {
        My_Type = _Type;

        switch (My_Type)
        {
            case StaminaType.StaminaType:
                GetImage((int)Images.Stamina_Two).gameObject.SetActive(false);
                break;
            case StaminaType.StaminaType_Two:
                GetImage((int)Images.Stamina_Two).gameObject.SetActive(true);
                break;
        }
    }

    public void StaminaUpdate(float _fValue, float _fMaxValue)
    {
        if (Define.Scene.Castle == GameManager.Scene.CurrentScene.SceneType)
            return;

        switch (My_Type)
        {
            case StaminaType.StaminaType:
                StaminaFillAmount(_fValue, _fMaxValue);
                break;
            case StaminaType.StaminaType_Two:
                StaminaTwoFillAmount(_fValue, _fMaxValue);
                break;
        }
    }

    void Awake()
    {
        init();
    }

}
