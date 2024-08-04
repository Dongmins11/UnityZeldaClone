using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class UI_World_Npc_Name : UI_Base
{
    enum GameObjects
    {
        Npc_Arrow_Object,
    }

    enum Texts
    {
        Npc_Name_Text,
    }


    int iAnimHash = Animator.StringToHash("Arrow_Trigger");

    Npc_Base.NpcType CurType;
    Npc_Base.NpcType PreType = Npc_Base.NpcType.End;

    Animator My_Anim;

    Camera My_Cam;

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));

        My_Anim = GetObject(0).GetComponent<Animator>();
        My_Cam = Camera.main;
        Hide_Npc_Name();
    }

    private void Text_Setting(Npc_Base.NpcType _eType)
    {
        CurType = _eType;

        if (CurType != PreType)
        {
            switch (CurType)
            {
                case Npc_Base.NpcType.Carpenter:
                    GetText(0).text = "허드슨";
                    MasterAudio.PlaySound("NPC3MAN");
                    MasterAudio.PlaySound("NPC4");
                    break;
                case Npc_Base.NpcType.Impa:
                    GetText(0).text = "임파";
                    MasterAudio.PlaySound("NPC2");
                    MasterAudio.PlaySound("NPC4");
                    break;
                case Npc_Base.NpcType.Paya:
                    GetText(0).text = "파야";
                    MasterAudio.PlaySound("NPC1");
                    MasterAudio.PlaySound("NPC4");
                    break;
            }

            PreType = CurType;
        }
    }

    public void Show_Npc_Name(Npc_Base.NpcType _eType)
    {
        gameObject.SetActive(true);
        My_Anim.SetTrigger(iAnimHash);
        Text_Setting(_eType);
    }

    public void Hide_Npc_Name()
    {
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (null == My_Cam)
            return;

        Util.Billbord_UI(transform, My_Cam.transform);
    }

}
