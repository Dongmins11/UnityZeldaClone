using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TalkSelect : UI_Base
{
    enum GameObjects
    {
        Select_Success,
        Select_Number,
        Select_Cancle,

    }

    enum Texts
    {
        Success_Text,
        Number_Text,
        Cancle_Text,
    }

    enum Images
    {
        Select_Glow_Success,
        Select_Glow_Number,
        Select_Glow_Cancle,
    }

    public enum Talk_Types
    {
        Success,
        Number,
        Talk,
    }

    Talk_Types Cur_TalkType = Talk_Types.Talk;

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    public void Show_TalkSelect(Talk_Types _eType)
    {
        Cur_TalkType = _eType;
    }

}
