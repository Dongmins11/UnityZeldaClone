using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_World_Monster_Hp : UI_Base
{
    enum Images
    {
        Monster_Hp,
    }

    Camera My_Cam;

    public override void init()
    {
        Bind<Image>(typeof(Images));
        Init_Monster_Hp();
    }

    public void Init_Monster_Hp()
    {
        GetImage((int)Images.Monster_Hp).fillAmount = 1.0f;
    }

    public void Monster_HpUpdate(int _iHp, int _iMaxHp, Transform _ParentTransform)
    {
        transform.SetParent(_ParentTransform);
        transform.localPosition = Vector3.zero;

        float fRatioHp = 0f;

        if (0f <= _iHp)
            fRatioHp = (float)_iHp / _iMaxHp;

        GetImage((int)Images.Monster_Hp).fillAmount = fRatioHp;
    }

    private void Awake()
    {
        init();

        My_Cam = Camera.main;
    }
}
