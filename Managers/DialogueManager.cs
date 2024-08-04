using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager
{
    public enum Talk_Type
    {
        Normal_Talk,
        Quest_Talk,
        ArmorShop_Talk,
        FoodShop_Talk,
    }

    public Talk_Type Cur_TalkType { get; private set; }

    UI_Talk My_Talk_UI;

    public UI_Talk Talk_UIs
    {
        get 
        {
            Init();
            return My_Talk_UI;
        }
    }

    public void Init()
    {
        if(null == My_Talk_UI)
            My_Talk_UI = GameManager.UI.ShowSceneUI<UI_Talk>("Talk_Canvas", false);
    }

    public void Show_TalkText(TextAsset _InkJson)
    {
        Cur_TalkType = Talk_Type.Normal_Talk;
        Talk_UIs.Show_TalkText(_InkJson);
    }

    public void Show_QuestTalkText(Quest _Quest)
    {
        Cur_TalkType = Talk_Type.Normal_Talk;
        Talk_UIs.Show_QuestTalkText(_Quest);
    }

    public bool Show_ShopTalkText(TextAsset _InkJson, ItemData _ItemData, Talk_Type _eType)
    {
        Cur_TalkType = _eType;
        return Talk_UIs.Show_ShopTalkText(_InkJson, _ItemData);
    }

    public int Get_FoodCount()
    {
        return Talk_UIs.iFoodCountIndex;
    }

}
