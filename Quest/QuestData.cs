using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestData
{
    public int iQuestIndex;
    public int iNextQuestIndex;
    public int iCanStartIndex;
    public int iPreviorIndex;
    public string strQuestName;
    public string strQuestTitle;
    public string strExplain;
    public string strTalkName;
    public int iQuestType;
    public string strQuestValues;
    public string strQuestValueCount;
    public string strItemReward;
    public int iItemCount;
}
