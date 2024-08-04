using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Inventory : MonoBehaviour
{
    public int iCurIndex { get; set; } = 0;
    public Action Quick_Slot_KeyAction;

    int iMaxIndex = (int)Skill.Skill_Type.TimeLock;
    int iCapacity = (int)Skill.Skill_Type.End_Type;

    Skill[] ArraySkills;

    bool bQuickSlotOpen = false;

    public bool InvaliledIndex(int _iIndex)
    {
        return 0 <= _iIndex && iMaxIndex >= _iIndex;
    }

    public Skill Get_Skill(int _iIndex)
    {
        if (false == InvaliledIndex(_iIndex))
            return null;

        return ArraySkills[_iIndex];
    }

    public Skill[] Get_Skills()
    {
        if (0 >= ArraySkills.Length)
            return null;

        return ArraySkills;
    }

    public void Init()
    {
        ArrayInit();
    }

    private void ArrayInit()
    {
        ArraySkills = new Skill[iCapacity];
    }

    public void Update_Skill_UI(Skill _Skill)
    {
        GameManager.UIContents.Get_QuickSlot_UI().Show_Skill_Slot(_Skill);
        GameManager.UIContents.Get_QuickSlot_UI().Update_QuickSlot_UI(_Skill);

        for (int i = 0; i < iCapacity; ++i)
        {
            if(null != ArraySkills[i])
                GameManager.UIContents.Skill_Inven_Setting_Image(_Skill);
            else
                GameManager.UIContents.Skill_Inven_NewSetting(_Skill);
        }
    }

    public void Add(Skill.Skill_Type _eType)
    {
        if (false == InvaliledIndex((int)_eType))
            return;

        Skill AddSkill = null;

        switch (_eType)
        {
            case Skill.Skill_Type.MagneCatch:
                AddSkill = new Skill(Skill.Skill_Type.MagneCatch, "MagneCatch2", "Obj_Magnetglove");
                break;
            case Skill.Skill_Type.RemoteBomb:
                AddSkill = new Skill(Skill.Skill_Type.RemoteBomb, "RemoteBomb", "Obj_RemoteBombBall");
                break;
            case Skill.Skill_Type.IceMaker:
                AddSkill = new Skill(Skill.Skill_Type.IceMaker, "Ice Maker", "Obj_IceMaker");
                break;
            case Skill.Skill_Type.TimeLock:
                AddSkill = new Skill(Skill.Skill_Type.TimeLock, "TimeLock", "Obj_StopTimer");
                break;
        }

        Update_Skill_UI(AddSkill);

        ArraySkills[(int)_eType] = AddSkill;
    }

    private void Open_Skill_QuickSlot()
    {
        if (null == ArraySkills)
            return;

        if (Input.GetKey(KeyCode.Tab))
        {
            if (false == bQuickSlotOpen)
            {
                GameManager.UIContents.Get_QuickSlot_UI().gameObject.SetActive(true);
                GameManager.UIContents.Get_QuickSlot_UI().Show_BlurTexture();
            }

            Quick_Slot_KeyAction?.Invoke();
            bQuickSlotOpen = true;
        }
        else
        {
            if (true == bQuickSlotOpen && GameManager.UIContents.Get_QuickSlot_UI().IsMoveCheck())
            {
                GameManager.UIContents.Get_QuickSlot_UI().gameObject.SetActive(false);
                GameManager.UIContents.Get_QuickSlot_UI().Hide_BlurTexture();
                bQuickSlotOpen = false;
            }

        }
    }


    private void Update()
    {
        Open_Skill_QuickSlot();
    }
}
