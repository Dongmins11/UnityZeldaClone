using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Paya : Npc_Base
{

    public override void Init()
    {
        My_Type = NpcType.Paya;

        UI_Init();
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
        {
            UI_Event_On();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI_Event_Off();
        }
    }

    void Start()
    {
        Init();
    }
}
