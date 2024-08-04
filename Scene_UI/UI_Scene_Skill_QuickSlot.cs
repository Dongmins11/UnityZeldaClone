using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class UI_Scene_Skill_QuickSlot : UI_Base
{
    enum GameObjects
    {
        Parent_Slot_Object,
        Skill_QuickSlot_Object_Magnecatch,
        Skill_QuickSlot_Object_RemoteBomb,
        Skill_QuickSlot_Object_IceMaker,
        Skill_QuickSlot_Object_TimeLock,

        Skill_QuickSlot_Object_Glow_Magnecatch,
        Skill_QuickSlot_Object_Glow_RemoteBomb,
        Skill_QuickSlot_Object_Glow_IceMaker,
        Skill_QuickSlot_Object_Glow_TimeLock,

        SlotFrontGround_Magnecatch,
        SlotFrontGround_RemoteBomb,
        SlotFrontGround_IceMaker,
        SlotFrontGround_TimeLock,

        OnePoint,
        TwoPoint,
        ThreePoint,
        FourPoint,
    }

    enum Images
    {
        Skill_Images_Magnecatch,
        Skill_Images_RemoteBomb,
        Skill_Images_IceMaker,
        Skill_Images_TimeLock,
    }

    enum PositionType
    {
        OnePoint,
        TwoPoint,
        ThreePoint,
        FourPoint,
    }

    enum Texts
    {
        Skill_QuickSlot_Text,
    }

    public enum Slot_State
    {
        State_NonMove,
        State_Move,
        State_Setter,
    }

    public enum Skill_Type
    {
        Magnecatch,
        RemoteBomb,
        IceMaker,
        TimeLock,
    }

    int iCurrent_SkillIndex = -1;
    int iPrevior_SkillIndex = 0;

    int iCur_EquipIndex = 0;
    int iPre_EquipIndex = 0;

    int iMaxIndex = 3;
    float fSlerpSpeed = 3.0f;

    public Slot_State Cur_State { get; private set; } = Slot_State.State_NonMove;

    UI_Scene_Skill_Render Skill_Render;

    Vector3[] Array_UI_Position;

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        PositionInit();

        AllHide();


        GameManager.UIContents.Skill_Inven.Quick_Slot_KeyAction -= MoveToSlot;
        GameManager.UIContents.Skill_Inven.Quick_Slot_KeyAction += MoveToSlot;

        Skill_Render = GameManager.UI.ShowSceneUI<UI_Scene_Skill_Render>("Scene_Skill_Render_Canvas", false);
    }

    private void OnEnable()
    {
        GameManager.UIContents.Skill_Inven.Quick_Slot_KeyAction -= MoveToSlot;
        GameManager.UIContents.Skill_Inven.Quick_Slot_KeyAction += MoveToSlot;
    }

    private void OnDisable()
    {
        GameManager.UIContents.Skill_Inven.Quick_Slot_KeyAction -= MoveToSlot;
    }

    public int Get_SkillType()
    {
        return iCurrent_SkillIndex;
    }

    private void PositionInit()
    {
        int iCount = 0;

        Array_UI_Position = new Vector3[4];

        for (int i = (int)GameObjects.OnePoint; i <= (int)GameObjects.FourPoint; ++i)
        {
            Array_UI_Position[iCount] = GetObject(i).transform.localPosition;
            ++iCount;
        }
    }
    public void Update_QuickSlot_UI(Skill _skill)
    {
        GameManager.UIContents.Skill_Inven.iCurIndex = iCurrent_SkillIndex;
        Get<GameObject>((int)GameObjects.Parent_Slot_Object).transform.localPosition = GetPosition(iCurrent_SkillIndex);

        TextChange(_skill);
        EquipSlot(_skill);
        Show_GlowImage();
    }

    private void TextChange(Skill _skill)
    {
        GetText((int)Texts.Skill_QuickSlot_Text).text = _skill.My_Name;
    }

    private void Equip_Skill_Slot(int _iPreIndex, int _iCurIndex)
    {
        switch (_iPreIndex)
        {
            case 0:
                Hide_Object((int)GameObjects.SlotFrontGround_Magnecatch);
                break;
            case 1:
                Hide_Object((int)GameObjects.SlotFrontGround_RemoteBomb);
                break;
            case 2:
                Hide_Object((int)GameObjects.SlotFrontGround_IceMaker);
                break;
            case 3:
                Hide_Object((int)GameObjects.SlotFrontGround_TimeLock);
                break;
        }

        switch (_iCurIndex)
        {
            case 0:
                Show_Object((int)GameObjects.SlotFrontGround_Magnecatch);
                break;
            case 1:
                Show_Object((int)GameObjects.SlotFrontGround_RemoteBomb);
                break;
            case 2:
                Show_Object((int)GameObjects.SlotFrontGround_IceMaker);
                break;
            case 3:
                Show_Object((int)GameObjects.SlotFrontGround_TimeLock);
                break;
        }
    }

    public bool IsMoveCheck()
    {
        return Slot_State.State_Move != Cur_State;
    }

    public void Show_Skill_Slot(Skill _Skill)
    {
        switch (_Skill.My_Type)
        {
            case Skill.Skill_Type.MagneCatch:
                Show_Object((int)GameObjects.Skill_QuickSlot_Object_Magnecatch);
                Show_Image((int)Images.Skill_Images_Magnecatch);
                break;
            case Skill.Skill_Type.RemoteBomb:
                Show_Object((int)GameObjects.Skill_QuickSlot_Object_RemoteBomb);
                Show_Image((int)Images.Skill_Images_RemoteBomb);
                break;
            case Skill.Skill_Type.IceMaker:
                Show_Object((int)GameObjects.Skill_QuickSlot_Object_IceMaker);
                Show_Image((int)Images.Skill_Images_IceMaker);
                break;
            case Skill.Skill_Type.TimeLock:
                Show_Object((int)GameObjects.Skill_QuickSlot_Object_TimeLock);
                Show_Image((int)Images.Skill_Images_TimeLock);
                break;
        }

        iCurrent_SkillIndex = (int)_Skill.My_Type;
        iMaxIndex = (int)_Skill.My_Type;
    }

    private void Show_GlowImage()
    {
        switch (iPrevior_SkillIndex)
        {
            case 0:
                Hide_Object((int)GameObjects.Skill_QuickSlot_Object_Glow_Magnecatch);
                break;
            case 1:
                Hide_Object((int)GameObjects.Skill_QuickSlot_Object_Glow_RemoteBomb);
                break;
            case 2:
                Hide_Object((int)GameObjects.Skill_QuickSlot_Object_Glow_IceMaker);
                break;
            case 3:
                Hide_Object((int)GameObjects.Skill_QuickSlot_Object_Glow_TimeLock);
                break;
        }

        switch (iCurrent_SkillIndex)
        {
            case 0:
                Show_Object((int)GameObjects.Skill_QuickSlot_Object_Glow_Magnecatch);
                MasterAudio.PlaySound("SkillChange");
                break;
            case 1:
                Show_Object((int)GameObjects.Skill_QuickSlot_Object_Glow_RemoteBomb);
                MasterAudio.PlaySound("SkillChange");
                break;
            case 2:
                Show_Object((int)GameObjects.Skill_QuickSlot_Object_Glow_IceMaker);
                MasterAudio.PlaySound("SkillChange");
                break;
            case 3:
                Show_Object((int)GameObjects.Skill_QuickSlot_Object_Glow_TimeLock);
                MasterAudio.PlaySound("SkillChange");
                break;
        }
    }

    private void Show_Object(int _iIndex)
    {
        GetObject(_iIndex).SetActive(true);
    }

    private void Show_Image(int _iIndex)
    {
        GetImage(_iIndex).gameObject.SetActive(true);
    }

    private void Hide_Object(int _iIndex)
    {
        GetObject(_iIndex).SetActive(false);
    }

    private void Hide_SkillImage(int _iIndex)
    {
        GetImage(_iIndex).gameObject.SetActive(false);
    }

    private void AllHide()
    {
        for (int i = (int)GameObjects.Skill_QuickSlot_Object_Magnecatch; i <= (int)GameObjects.SlotFrontGround_TimeLock; ++i)
            Hide_Object(i);

        for (int i = (int)Images.Skill_Images_Magnecatch; i <= (int)Images.Skill_Images_TimeLock; ++i)
            Hide_SkillImage(i);
    }

    public void EquipSlot(Skill _skill)
    {
        iPre_EquipIndex = iCur_EquipIndex;
        iCur_EquipIndex = iCurrent_SkillIndex;


        Skill_Render.Show_Skill(_skill);
        Equip_Skill_Slot(iPre_EquipIndex, iCur_EquipIndex);

        PlayerManager.weaponManager.SetCurrentSkill(iCurrent_SkillIndex);
    }


    private Vector3 GetPosition(int iIndex)
    {
        return Array_UI_Position[iIndex];
    }

    IEnumerator PositionSetting_Canvas()
    {
        Cur_State = Slot_State.State_Move;

        float Timer = 0.0f;

        while (true)
        {
            yield return null;

            Timer += Time.deltaTime * fSlerpSpeed;

            Get<GameObject>((int)GameObjects.Parent_Slot_Object).transform.localPosition = Vector3.Slerp(GetObject((int)GameObjects.Parent_Slot_Object).transform.localPosition, GetPosition(iCurrent_SkillIndex), Timer);

            if (1.0f <= Timer)
            {
                Cur_State = Slot_State.State_NonMove;
                yield break;
            }
        }
    }

    private void MoveToSlot()
    {
        float fwheelInput = Input.GetAxisRaw("Mouse ScrollWheel");

        switch (Cur_State)
        {
            case Slot_State.State_NonMove:

                if (iPrevior_SkillIndex != iCurrent_SkillIndex)
                    iPrevior_SkillIndex = iCurrent_SkillIndex;

                if (0 < fwheelInput)
                {
                    iCurrent_SkillIndex = Mathf.Max(0, --iCurrent_SkillIndex);
                    GameManager.UIContents.Skill_Inven.iCurIndex = iCurrent_SkillIndex;
                    TextChange(GameManager.UIContents.Skill_Inven.Get_Skill(iCurrent_SkillIndex));
                    StartCoroutine(PositionSetting_Canvas());
                    Show_GlowImage();
                }
                else if (0 > fwheelInput)
                {
                    iCurrent_SkillIndex = Mathf.Min(iMaxIndex, ++iCurrent_SkillIndex);
                    GameManager.UIContents.Skill_Inven.iCurIndex = iCurrent_SkillIndex;
                    TextChange(GameManager.UIContents.Skill_Inven.Get_Skill(iCurrent_SkillIndex));
                    StartCoroutine(PositionSetting_Canvas());
                    Show_GlowImage();
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    EquipSlot(GameManager.UIContents.Skill_Inven.Get_Skill(iCurrent_SkillIndex));
                }

                break;
            case Slot_State.State_Move:
                break;
        }
    }

}
