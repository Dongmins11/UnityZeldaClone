using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Impa : Npc_Base, InteractiveObject
{
    GameObject PortalPosObejct = null;

    bool isPortalCreateed = false;

    public override void Init()
    {
        Quest TempQuest = null;
        My_Type = NpcType.Impa;

        PortalPosObejct = Util.FindChild(gameObject, "PotalCreate");

        if(false == GameManager.Quest.Dictionary_BackUpQuest.TryGetValue(gameObject.name, out TempQuest))
        {
            iQuestIndex = 0;
            My_Quest = GameManager.Quest.Create_Quest(iQuestIndex);
            My_Quest.My_Progress = Quest.Quest_Progress.Can_Start;
            GameManager.Quest.Dictionary_BackUpQuest.Add(gameObject.name, My_Quest);
        }
        else
            My_Quest = TempQuest;

        GameManager.Quest.Create_Portal -= Created_Portal;
        GameManager.Quest.Create_Portal += Created_Portal;

        GameManager.Quest.Quest_Update_Action -= Update_NpcQuest;
        GameManager.Quest.Quest_Update_Action += Update_NpcQuest;

        UI_Init();
    }

    private void Created_Portal()
    {
        if (5 == My_Quest.My_Data.iQuestIndex && null != PortalPosObejct && false == isPortalCreateed)
        {
            isPortalCreateed = true;
            GameManager.Resources.CreatePrefab("Map/PortalSeven", PortalPosObejct.transform);
        }
    }

    private void Update_NpcQuest()
    {
        if (null == My_Quest)
            return;

        if (Quest.Quest_Progress.Can_Success == My_Quest.My_Progress)
        {
            if(true == GameManager.Quest.Dictionary_BackUpQuest.TryGetValue(gameObject.name, out Quest Temp))
                GameManager.Quest.Dictionary_BackUpQuest.Remove(gameObject.name);

            My_Quest = GameManager.Quest.Get_NextQuest(My_Quest);
            GameManager.Quest.Dictionary_BackUpQuest.Add(gameObject.name, My_Quest);
        }
    }

    protected override void UI_Init()
    {
        base.UI_Init();
    }

    protected override void UI_Event_Off()
    {
        base.UI_Event_Off();
    }

    protected override void UI_Event_On()
    {
        base.UI_Event_On();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            UI_Event_On();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            UI_Event_Off();
    }

    private void OnDisable()
    {
        GameManager.Quest.Create_Portal -= Created_Portal;
        GameManager.Quest.Quest_Update_Action -= Update_NpcQuest;
    }

    void Start()
    {
        Init();
    }

    public void OnPlayerInteracting(PlayerAnimator _playerAnimator, InteractiveObject _interactObject)
    {
        if (null == My_Quest)
            return;

        GameManager.Dialogue.Show_QuestTalkText(My_Quest);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            GameManager.Quest.Quest_Success?.Invoke(0);
        else if (Input.GetKeyDown(KeyCode.F2))
            GameManager.Quest.Quest_Success?.Invoke(1);
        else if (Input.GetKeyDown(KeyCode.F3))
            GameManager.Quest.Quest_Success?.Invoke(2);
    }
}
