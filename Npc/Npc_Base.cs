using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Npc_Base : MonoBehaviour
{
    public enum NpcType
    {
        Carpenter,
        Impa,
        Paya,
        End,
    }

    [SerializeField]
    protected TextAsset My_Ink;
    public NpcType My_Type { get; protected set; } = NpcType.End;

    protected GameObject My_Child_UI_GameObject;
    protected UI_World_Npc_Name UI_Name;

    protected int iQuestIndex = 0;

    protected Quest My_Quest;

    public abstract void Init();

    protected virtual void UI_Init()
    {
        My_Child_UI_GameObject = Util.FindChild(gameObject, "Name_UI_Position");

        if(null == My_Child_UI_GameObject)
            return;

        GameObject Npc_Name_UI = GameManager.Resources.CreatePrefab("UI/World/Npc_Name_Canvas", My_Child_UI_GameObject.transform);

        UI_Name = Util.GetOrAddComponent<UI_World_Npc_Name>(Npc_Name_UI);

        if (null == UI_Name)
            return;

        UI_Name.init();
    }

    protected virtual void UI_Event_On()
    {
        UI_Name.Show_Npc_Name(My_Type);
    }

    protected virtual void UI_Event_Off()
    {
        UI_Name.Hide_Npc_Name();

    }





}
