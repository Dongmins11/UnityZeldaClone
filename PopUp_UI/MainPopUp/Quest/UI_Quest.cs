using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Quest : UI_Base
{
    enum GameObjects { Content }

    enum Infomation { QuestInformation }

    enum Scrollbars { Scrollbar_Vertical }

    int iCurIndex = 0;
    int iPreIndex = 0;
    int iMaxIndex = 0;

    List<UI_QuestSlot> Quest_List = new List<UI_QuestSlot>();
    Dictionary<int, UI_QuestSlot> Find_QuestSlot = new Dictionary<int, UI_QuestSlot>();

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<UI_QeustInfo>(typeof(Infomation));
        Bind<Scrollbar>(typeof(Scrollbars));

        Quest_Slot_Init();

        GameManager.Quest.UI_Event_Action -= Add;
        GameManager.Quest.UI_Event_Action += Add;

        GameManager.Quest.UI_Remove_Action -= Remove;
        GameManager.Quest.UI_Remove_Action += Remove;
    }


    private void Add(Quest _Quest)
    {
        UI_QuestSlot QuestSlot = GameManager.UI.MakeSubItem<UI_QuestSlot>(GetObject((int)GameObjects.Content).transform, "QuestImage");

        QuestSlot.init();
        QuestSlot.Data = _Quest.My_Data;

        Quest_List.Add(QuestSlot);
        Find_QuestSlot.Add(QuestSlot.iIndex, QuestSlot);

        Show_Infomation(QuestSlot.Data);

        ++iMaxIndex;
    }

    private void Remove(QuestData _QuestData)
    {
        UI_QuestSlot TempSlot = null;

        Find_QuestSlot.TryGetValue(_QuestData.iPreviorIndex, out TempSlot);

        if (null != TempSlot && true == Quest_List.Remove(TempSlot))
        {
            Hide_Infomation();
            Find_QuestSlot.Remove(_QuestData.iPreviorIndex);

            TempSlot.transform.SetParent(null);
            GameManager.Resources.Destroy(TempSlot.gameObject);

            Reset_UI_Quest();

            --iMaxIndex;
        }
    }

    private void Show_Infomation(QuestData _Data)
    {
        Get<UI_QeustInfo>((int)Infomation.QuestInformation).TitleText = _Data.strQuestTitle;
        Get<UI_QeustInfo>((int)Infomation.QuestInformation).DetailText = _Data.strExplain;
    }

    private void Hide_Infomation()
    {
        Get<UI_QeustInfo>((int)Infomation.QuestInformation).TitleText = "";
        Get<UI_QeustInfo>((int)Infomation.QuestInformation).DetailText = "";
    }


    public void Reset_UI_Quest()
    {
        if (0 >= Quest_List.Count)
            return;

        iCurIndex = 0;
        iPreIndex = 0;

        foreach (var iter in Quest_List)
            iter.SetActive_Glow(false);

        Update_Quest();
    }

    private void Update_Quest()
    {
        Quest_List[iPreIndex].SetActive_Glow(false);
        Quest_List[iCurIndex].SetActive_Glow(true);
        Show_Infomation(Quest_List[iCurIndex].Data);
    }

    public void KeyInput()
    {
        if (0 >= Quest_List.Count)
            return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            iPreIndex = iCurIndex;
            iCurIndex = Mathf.Max(0, --iCurIndex);

            if (iPreIndex != iCurIndex)
                Update_Quest();
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            iPreIndex = iCurIndex;
            iCurIndex = Mathf.Min(iMaxIndex - 1, ++iCurIndex);

            if (iPreIndex != iCurIndex)
                Update_Quest();
        }
    }


    private void Quest_Slot_Init()
    {
        GameObject TempObject = GetObject((int)GameObjects.Content);

        if (null == TempObject)
            return;

        foreach(Transform iter in TempObject.transform)
            GameManager.Resources.Destroy(iter.gameObject);
    }

}
