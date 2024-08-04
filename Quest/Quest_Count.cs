using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_Count : Quest
{
    private Dictionary<string, int> QuestEndCount;
    private Dictionary<string, int> QuestCount;
    private Dictionary<string, ItemData> QuestItemData;

    private int iQuestCount = 0;

    public Quest_Count(QuestData _Data) : base(_Data)
    {
        QuestCount = new Dictionary<string, int>();
        QuestEndCount = new Dictionary<string, int>();
        QuestItemData = new Dictionary<string, ItemData>();
        SetQuestValueCount();
    }

    public override void Quest_Update()
    {
        if (null == QuestCount && 0 >= QuestItemData.Count)
            return;

        foreach (var iter in QuestEndCount.Keys)
        {
            if (true == QuestItemData.ContainsKey(iter))
                QuestCount[iter] = GameManager.item.Inven.Get_CountableAmount(QuestItemData[iter]);
        }

        foreach (var iter in QuestCount)
        {
            if (iter.Value < QuestEndCount[iter.Key])
                return;
        }

        GameManager.Quest.Quest_Item_Remove_Action -= Remove_QuestItem;
        GameManager.Quest.Quest_Item_Remove_Action += Remove_QuestItem;

        My_Progress = Quest_Progress.Can_Success;

        base.Quest_Update();
    }

    private void Remove_QuestItem()
    {
        if (0 >= QuestItemData.Count)
            return;

        foreach (var iter in QuestItemData)
            GameManager.item.Inven.Remove(iter.Value);

        GameManager.Quest.Quest_Item_Remove_Action -= Remove_QuestItem;
    }

    protected void SetQuestValueCount()
    {
        if (null == My_Data)
            return;

        string[] words = My_Data.strQuestValues.Split(',');
        string[] ValueCount = My_Data.strQuestValueCount.Split(',');

        int[] ValueCountArray = StringArrayToIntArray(ValueCount);

        for (int i = 0; i < words.Length; ++i)
        {
            QuestEndCount.Add(words[i], ValueCountArray[i]);
            QuestCount.Add(words[i], 0);

            ItemData TempData = GameManager.item.Get_ItemData(words[i]);

            if (null != TempData)
                QuestItemData.Add(words[i], TempData);

            ++iQuestCount;
        }
    }
}
