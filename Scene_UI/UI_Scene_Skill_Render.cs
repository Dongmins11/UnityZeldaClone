using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Scene_Skill_Render : UI_Base
{
    enum Images
    {
        Skill_Image,
    }

    public override void init()
    {
        Bind<Image>(typeof(Images));
    }

    public void Show_Skill(Skill _skill)
    {
        if(false == gameObject.activeSelf) 
            gameObject.SetActive(true);

        GetImage(0).sprite = _skill.My_QuickRenderSpiret;
    }

    public void Hide_Skill_Render()
    {
        gameObject.SetActive(false);
    }

}
