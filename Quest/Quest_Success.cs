using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_Success : Quest
{

    public Quest_Success(QuestData _Data) : base(_Data)
    {
    }

    public override void Quest_Update(int _iQuestIndex)
    {
        if (_iQuestIndex != My_Data.iQuestIndex ||
            Quest_Progress.Progress != My_Progress)
            return;

        My_Progress = Quest_Progress.Can_Success;

        base.Quest_Update();
    }

}
