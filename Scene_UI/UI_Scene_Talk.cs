using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Scene_Talk : UI_Base
{
    enum Texts
    {
        Skill_Talk_Text,
    }



    public override void init()
    {
        Bind<Text>(typeof(Texts));
        GameManager.UI.SortingOrder(this.gameObject, 12);
    }

    public void Show_Skill_Talk()
    {
        GetText(0).DOText("", 0);
        GetText(0).DOText("아이템이 추가되었습니다", 0.5f);
    }


    void Update()
    {
        
    }
}
