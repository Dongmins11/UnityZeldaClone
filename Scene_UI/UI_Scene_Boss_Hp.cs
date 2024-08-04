using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class UI_Scene_Boss_Hp : UI_Base
{
    enum GameObjects
    {
        Area_Left,
        Area_Right,
    }

    enum Texts
    {
        Area_Text,
        Boss_Text,
    }

    enum Images
    {
        Boss_Hp,
    }

    public enum Monster_Type
    {
        Lynel,
        Hinox,
        Ganon,
        End,
    }

    int iAnimHash = Animator.StringToHash("HpTrigger");

    const int iDefaultPontSize = 100;
    const int iGanonPontSize = 80;

    const int iDefaultAreaPosition = 120;
    const int iGanonAreaPosition = 220;

    string[] strBossAreaName;
    string[] strBossName;

    Monster_Type PreType = Monster_Type.End;
    Monster_Type CurType;

    Animator My_Anim;

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        AreaStringInit();

        My_Anim = gameObject.GetComponent<Animator>();
    }

    private void AreaStringInit()
    {
        strBossAreaName = new string[3];
        strBossName = new string[3];

        strBossAreaName[0] = "하이랄 평원의";
        strBossAreaName[1] = "코르그 숲의";
        strBossAreaName[2] = "신수 바 나보리스에 기생하는";

        strBossName[0] = "라이넬";
        strBossName[1] = "히녹스";
        strBossName[2] = "번개의 커스 가논";
    }

    private void ObjectSetting(Monster_Type _eType)
    {
        Vector3 LeftPosition = Vector3.zero;
        Vector3 RightPosition = Vector3.zero;

        switch (_eType)
        {
            case Monster_Type.Lynel:
            case Monster_Type.Hinox:
                LeftPosition = new Vector3(-iDefaultAreaPosition, -50, 0);
                RightPosition = new Vector3(iDefaultAreaPosition, -50, 0);
                GetObject((int)GameObjects.Area_Left).transform.localPosition = LeftPosition;
                GetObject((int)GameObjects.Area_Right).transform.localPosition = RightPosition;
                break;
            case Monster_Type.Ganon:
                LeftPosition = new Vector3(-iGanonAreaPosition, -50, 0);
                RightPosition = new Vector3(iGanonAreaPosition, -50, 0);
                GetObject((int)GameObjects.Area_Left).transform.localPosition = LeftPosition;
                GetObject((int)GameObjects.Area_Right).transform.localPosition = RightPosition;
                break;
        }
    }

    private void TextSetting(Monster_Type _eType)
    {
        switch (CurType)
        {
            case Monster_Type.Lynel:
                GetText((int)Texts.Boss_Text).text = strBossName[0];
                GetText((int)Texts.Area_Text).text = strBossAreaName[0];
                GetText((int)Texts.Boss_Text).fontSize = iDefaultPontSize;
                break;
            case Monster_Type.Hinox:

                GetText((int)Texts.Boss_Text).text = strBossName[1];
                GetText((int)Texts.Area_Text).text = strBossAreaName[1];
                GetText((int)Texts.Boss_Text).fontSize = iDefaultPontSize;
                break;
            case Monster_Type.Ganon:

                GetText((int)Texts.Boss_Text).text = strBossName[2];
                GetText((int)Texts.Area_Text).text = strBossAreaName[2];
                GetText((int)Texts.Boss_Text).fontSize = iGanonPontSize;
                break;
        }
    }

    public void Show_Text(Monster_Type _eType, int _iHp, int _iMaxHp)
    {
        gameObject.SetActive(true);

        CurType = _eType;

        if (CurType != PreType)
        {
            ObjectSetting(CurType);
            TextSetting(CurType);

            My_Anim.SetTrigger(iAnimHash);

            PreType = CurType;
        }

        float fHpRatio = (float)_iHp / _iMaxHp;

        GetImage((int)Images.Boss_Hp).fillAmount = fHpRatio;
    }

    public void HideText()
    {
        gameObject.SetActive(false);

        GetText((int)Texts.Boss_Text).text = "";
        GetText((int)Texts.Area_Text).text = "";

        PreType = Monster_Type.End;

        GetImage((int)Images.Boss_Hp).fillAmount = 1.0f;
    }
}
