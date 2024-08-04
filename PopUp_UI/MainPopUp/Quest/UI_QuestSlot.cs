using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestSlot : UI_Base
{
    enum GameObjects { GlowImage }

    enum Texts { QuestNameText }

    private string m_Text = "";

    private QuestData m_Data;

    public int iIndex { get; private set; }

    public QuestData Data {
        get { return m_Data; }
        set
        {
            m_Data = value;
            Text = m_Data.strQuestName;
            iIndex = m_Data.iQuestIndex;
        }
    }

    public string Text 
    {
        get { return m_Text; }
        set 
        {
            m_Text = value;
            SetText(m_Text);
        }
    }

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));

        SetActive_Glow(false);
    }

    private void SetText(string _StrText)
    {
        GetText((int)Texts.QuestNameText).text = _StrText;
    }

    public void SetActive_Glow(bool _bActive)
    {
        GetObject((int)GameObjects.GlowImage).SetActive(_bActive);
    }

}
