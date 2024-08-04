using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Inven_Infomation : UI_Base
{
    enum Texts
    {
        TitleText,
        InfoText,
    }

    public override void init()
    {
        Bind<Text>(typeof(Texts));

    }

    public void Show_Text(string _strTitleText, string _strInfoText)
    {
        GetText((int)Texts.TitleText).text = _strTitleText;
        GetText((int)Texts.InfoText).DOText("", 0.0f);
        GetText((int)Texts.InfoText).DOText(_strInfoText, 1.0f);
    }

    public void Hide_Text()
    {
        GetText((int)Texts.TitleText).text = string.Empty;
        GetText((int)Texts.InfoText).DOText("", 0.0f);

    }

}
