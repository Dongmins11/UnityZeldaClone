using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    Dictionary<int, Quest> Dictionary_Quests = new Dictionary<int, Quest>();
    Dictionary<int, QuestData> Dictionary_QuestDatas = new Dictionary<int, QuestData>();
    public Dictionary<string, Quest> Dictionary_BackUpQuest = new Dictionary<string, Quest>();

    public Action<Quest> UI_Event_Action = null;
    public Action<QuestData> UI_Remove_Action = null;

    public Action Quest_Item_Action = null;

    public Action Quest_Item_Remove_Action = null;

    public Action<int> Quest_Success = null;

    public Action Quest_Update_Action = null;

    public Action Quest_Start_Action = null;

    public Action Create_Portal = null;

    public void Init()
    {
        Create_QuestData("Quest");
    }


    public Quest Create_Quest(int _QuestIndex)
    {
        if (-1 == _QuestIndex)
            return null;

        QuestData TempData = null;

        if (true == Dictionary_QuestDatas.ContainsKey(_QuestIndex))
        {
            Dictionary_QuestDatas.TryGetValue(_QuestIndex, out TempData);

            if (Quest.Quest_Type.Success_Quest == (Quest.Quest_Type)TempData.iQuestType)
                return new Quest_Success(TempData);
            else
                return new Quest_Count(TempData);
        }

        return null;
    }

    public void Ready_Quest(Quest _MyQuest)
    {
        if (null == _MyQuest)
            return;

        Dictionary_Quests.Add(_MyQuest.My_Data.iQuestIndex, _MyQuest);
    }

    public void Start_Quest(Quest _MyQuest)
    {
        if (null == _MyQuest)
            return;


        if (-1 == _MyQuest.My_Data.iNextQuestIndex)
        {
            _MyQuest.My_Progress = Quest.Quest_Progress.Progress;

            Create_Portal?.Invoke();

            Get_Reward(_MyQuest);
            AddAction(_MyQuest);

            UI_Remove_Action?.Invoke(_MyQuest.My_Data);
            Quest_Success?.Invoke(_MyQuest.My_Data.iQuestIndex);
            Quest_Success?.Invoke(_MyQuest.My_Data.iCanStartIndex);

            GameManager.UIContents.Show_Quest_Title(_MyQuest.My_Data.strQuestName);
            return;
        }

        if (true == Dictionary_Quests.ContainsKey(_MyQuest.My_Data.iQuestIndex))
        {
            Get_Reward(_MyQuest);
            AddAction(_MyQuest);
        }
        else
        {
            Dictionary_Quests.Add(_MyQuest.My_Data.iQuestIndex, _MyQuest);
            AddAction(_MyQuest);
        }

        GameManager.UIContents.Show_Quest_Title(_MyQuest.My_Data.strQuestName);

        UI_Remove_Action?.Invoke(_MyQuest.My_Data);
        UI_Event_Action?.Invoke(_MyQuest);

        _MyQuest.My_Progress = Quest.Quest_Progress.Progress;
    }

    private void AddAction(Quest _MyQuest)
    {
        if (Quest.Quest_Type.Success_Quest != _MyQuest.My_Type)
        {
            _MyQuest.Quest_Update();

            Quest_Item_Action -= _MyQuest.Quest_Update;
            Quest_Item_Action += _MyQuest.Quest_Update;
        }
        else
        {
            Quest_Success -= _MyQuest.Quest_Update;
            Quest_Success += _MyQuest.Quest_Update;
        }
    }

    private void Create_QuestData(string _strDataPath)
    {
        List<QuestData> Data;

        Data = GameManager.Data.LoadData<List<QuestData>>(_strDataPath);

        if (null == Data)
            return;

        foreach (var iter in Data)
            Dictionary_QuestDatas.Add(iter.iQuestIndex, iter);
    }

    public Quest Get_NextQuest(Quest _MyQuest)
    {
        if (-1 == _MyQuest.My_Data.iNextQuestIndex)
        {
            _MyQuest.My_Progress = Quest.Quest_Progress.Success;
            Remove_Quest(_MyQuest);
            return null;
        }

        Quest NextQuest = null;

        if (false == Dictionary_Quests.ContainsKey(_MyQuest.My_Data.iNextQuestIndex))
        {
            NextQuest = Create_Quest(_MyQuest.My_Data.iNextQuestIndex);

            NextQuest.bIsRewardCheck = true;

            Dictionary_Quests.Add(NextQuest.My_Data.iQuestIndex, NextQuest);

            _MyQuest.My_Progress = Quest.Quest_Progress.Success;

            Remove_Quest(_MyQuest);

            if (_MyQuest.My_Data.iNextQuestIndex == NextQuest.My_Data.iQuestIndex)
                NextQuest.My_Progress = Quest.Quest_Progress.Can_Start;
        }

        Can_StartQuest(_MyQuest);

        if (null != NextQuest)
            return NextQuest;

        return null;
    }

    private void Can_StartQuest(Quest _MyQuest)
    {
        if (-1 == _MyQuest.My_Data.iCanStartIndex)
            return;

        Quest CanQuest = null;

        Dictionary_Quests.TryGetValue(_MyQuest.My_Data.iCanStartIndex, out CanQuest);

        if (null != CanQuest)
            CanQuest.My_Progress = Quest.Quest_Progress.Can_Start;
    }

    private void Remove_Quest(Quest _MyQuest)
    {
        if (true == Dictionary_Quests.ContainsKey(_MyQuest.My_Data.iQuestIndex))
        {
            Quest TempQuest = null;

            Dictionary_Quests.TryGetValue(_MyQuest.My_Data.iQuestIndex, out TempQuest);

            if (Quest.Quest_Type.Success_Quest != (Quest.Quest_Type)TempQuest.My_Data.iQuestType)
                Quest_Item_Action -= TempQuest.Quest_Update;
            else
                Quest_Success -= TempQuest.Quest_Update;

            Dictionary_Quests.Remove(_MyQuest.My_Data.iQuestIndex);
        }
    }

    public void Get_Reward(Quest _MyQuest)
    {
        if (true != _MyQuest.bIsRewardCheck)
            return;

        QuestData TempData = null;

        if (false == Dictionary_QuestDatas.TryGetValue(_MyQuest.My_Data.iPreviorIndex, out TempData))
            return;

        _MyQuest.bIsRewardCheck = false;

        Quest_Item_Remove_Action?.Invoke();


        string[] strMyRewardItem = TempData.strItemReward.Split(',');

        for (int i = 0; i < strMyRewardItem.Length; ++i)
        {
            ItemData TempItemData = GameManager.item.Get_ItemData(strMyRewardItem[i]);

            if ((int)Item.Inven_Type.Item_Rupee == TempItemData.iInvenType)
                GameManager.item.Inven.Add_Rupee(TempData.iItemCount);
            else
                GameManager.item.Inven.Add(TempItemData, TempData.iItemCount);
        }
    }


}
