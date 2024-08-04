using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest
{
    public enum Quest_Progress
    {
        Unknow,
        Can_Start,
        Progress,
        Can_Success,
        Success,
    }

    public enum Quest_Type
    {
        Success_Quest,
        Count_Quest,
    }

    public QuestData My_Data { get; protected set; }
    public Quest_Type My_Type { get; protected set; }

    public Quest_Progress My_Progress { get; set; }

    public TextAsset My_Talk { get; protected set; }

    public bool bIsRewardCheck { get; set; } = false;

    public Quest(QuestData _Data)
    {
        My_Data = _Data;
        My_Type = (Quest_Type)_Data.iQuestType;
        My_Progress = Quest_Progress.Unknow;
        My_Talk = GameManager.Resources.Load<TextAsset>($"Ink/{_Data.strTalkName}");
    }

    public virtual void Quest_Update()
    {
        GameManager.Quest.Quest_Update_Action?.Invoke();
    }
    public virtual void Quest_Update(int _iQuestIndex)
    {
        GameManager.Quest.Quest_Update_Action?.Invoke();
    }

    protected int[] StringArrayToIntArray(string[] _strintArray)
    {
        List<int> IntList = new List<int>();

        for(int i =0; i < _strintArray.Length; ++i)
        {
            string TempString = _strintArray[i];
            int TempInt = 0;

            for (int j =0; j < TempString.Length; ++j)
                TempInt = TempInt * 10 + (TempString[j] - '0');

            IntList.Add(TempInt);
        }

        return IntList.ToArray();
    }

}
