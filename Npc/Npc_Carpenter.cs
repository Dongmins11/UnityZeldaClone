using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Carpenter : Npc_Base, InteractiveObject
{
    public override void Init()
    {
        Quest TempQuest = null;
        My_Type = NpcType.Carpenter;

        if (false == GameManager.Quest.Dictionary_BackUpQuest.TryGetValue(gameObject.name, out TempQuest))
        {
            iQuestIndex = 3;
            My_Quest = GameManager.Quest.Create_Quest(iQuestIndex);
            GameManager.Quest.Ready_Quest(My_Quest);
            GameManager.Quest.Dictionary_BackUpQuest.Add(gameObject.name, My_Quest);
        }
        else
            My_Quest = TempQuest;

        GameManager.Quest.Quest_Update_Action -= Update_NpcQuest;
        GameManager.Quest.Quest_Update_Action += Update_NpcQuest;

        UI_Init();
    }

    private void Update_NpcQuest()
    {
        if (null == My_Quest)
            return;

        if (Quest.Quest_Progress.Can_Success == My_Quest.My_Progress)
        {
            if (true == GameManager.Quest.Dictionary_BackUpQuest.TryGetValue(gameObject.name, out Quest Temp))
                GameManager.Quest.Dictionary_BackUpQuest.Remove(gameObject.name);

            My_Quest = GameManager.Quest.Get_NextQuest(My_Quest);
            GameManager.Quest.Dictionary_BackUpQuest.Add(gameObject.name, My_Quest);
        }
    }

    protected override void UI_Event_Off()
    {
        base.UI_Event_Off();
    }

    protected override void UI_Event_On()
    {
        base.UI_Event_On();
    }

    private void OnDisable()
    {
        GameManager.Quest.Quest_Update_Action -= Update_NpcQuest;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI_Event_On();

            //GameManager.UIContents.Show_Interaction_UI(UI_Scene_Interaction.Interaction_Type.Item_E);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI_Event_Off();
            //GameManager.UIContents.Show_Interaction_UI(UI_Scene_Interaction.Interaction_Type.Item_E);
        }
    }

    public void OnPlayerInteracting(PlayerAnimator _playerAnimator, InteractiveObject _interactObject)
    {
        if (null == My_Quest)
            return;

        GameManager.Dialogue.Show_QuestTalkText(My_Quest);
    }

    void Start()
    {
        Init();
    }
}
