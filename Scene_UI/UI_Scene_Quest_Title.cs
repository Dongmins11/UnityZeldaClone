using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Scene_Quest_Title : UI_Base
{
    enum Texts
    {
        Scene_Quest_Text,
    }

    bool bIsOpen = false;

    int iAnimHash = Animator.StringToHash("Scene_Quest_Trigger");

    Animator My_Anim;


    public override void init()
    {
        Bind<Text>(typeof(Texts));
        My_Anim = gameObject.GetComponent<Animator>();
    }


    public void Show_Quest_Title(string _strQuestName)
    {
        if (true == bIsOpen)
            return;

        bIsOpen = true;
        gameObject.SetActive(bIsOpen);
        My_Anim.SetTrigger(iAnimHash);
        GetText((int)Texts.Scene_Quest_Text).text = _strQuestName;
        MasterAudio.PlaySound("MAINQ");
    }

    public void Hide_Quest_Title()
    {
        bIsOpen = false;
        GetText((int)Texts.Scene_Quest_Text).text = "";
        gameObject.SetActive(bIsOpen);
    }


}
