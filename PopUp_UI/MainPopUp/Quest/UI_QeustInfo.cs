using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class UI_QeustInfo : UI_Base
{
    enum Texts
    {
        TitleText,
        DetailText,
    }

    private string m_TitleText;
    private string m_DetailText;

    public string TitleText 
    {
        get { return m_TitleText; }
        set 
        {
            m_TitleText = value;
            GetText((int)Texts.TitleText).text = m_TitleText;
        }
    }

    public string DetailText
    {
        get { return m_DetailText; }
        set
        {
            m_DetailText = value;
            GetText((int)Texts.DetailText).DOText("", 0.0f);
            GetText((int)Texts.DetailText).DOText(m_DetailText, 1.0f);
        }
    }

    public override void init()
    {
        Bind<Text>(typeof(Texts));
    }

    private void Awake()
    {
        init();    
    }

}
