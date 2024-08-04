using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class UI_Inventory : UI_Base
{
    enum Title_Panel
    {
        Icon_Panel,
    }
    enum Texts
    {
        RupeeText,
    }
    enum Inven_Scroll
    {
        InvenTory,
    }
    enum Inven_Info
    {
        InfoImage,
    }
    enum Inven_Set
    {
        Setter,
    }

    enum Inven_Values
    {
        AttackArmorValues,
    }


    enum GameObjects
    {
        MainSelect,
        Milestone_Left,
        Milestone_Right,
    }
    enum PositionState
    {
        Left,
        Middle,
        Right,
        Unknow,
    }

    public enum InputState
    {
        Move,
        NonMove,
    }

    private bool bIsFirstInit = false;

    private int iCurPanelIndex = 0;
    private int iRowIndex = 0;
    private int iColumnIndex = 0;

    public int iMaxPanelIndex { get; private set; }
    public int iMaxColumnIndex { get; private set; }
    public int iMaxRowIndex { get; private set; }

    public InputState State;

    private Vector3 HideScale = Vector3.zero;
    private Vector3 ShowScale;

    private UI_MainPopUp Main_PopUp;
    private Inventory My_Inven;

    public Inven_Slot Current_Slot;

    public Action<int, int> Inven_EquipUpdateAction = null;

    public Inven_Slot Get_Slot(int _iType, int _iRow, int _iColumn)
    {
        Inven_Panel TempPanel = Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory).Get_Panel(_iType);
        Inven_Slot Slot = null;

        if (null != TempPanel)
            Slot = TempPanel.Get_Slot(_iRow, _iColumn);

        if (null == Slot)
            return null;

        return Slot;
    }

    public void Remove_UI(int _iType, int _iRow, int _iColumn)
    {
        Inven_Slot Slot = Get_Slot(_iType, _iRow, _iColumn);

        if (null == Slot)
            return;

        Slot.All_Hide_Icon();
    }

    public void Set_Rupee(int _iCount)
    {
        GetText((int)Texts.RupeeText).text = $"{_iCount}";
    }

    public void SetterAddAction(System.Action _Funtion)
    {
        Get<Inven_Setter>((int)Inven_Set.Setter).AddSetterAction(_Funtion);
    }

    public void UseItem()
    {
        if (null != My_Inven)
        My_Inven.UseItem(iCurPanelIndex, iRowIndex, iColumnIndex);
    }

    public void Reset_UI_Inven()
    {
        iCurPanelIndex = 0;
        iRowIndex = 0;
        iColumnIndex = 0;

        Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory).Reset_Scrollbar();
        
        if(false == bIsFirstInit)
        {
            bIsFirstInit = true;
            StartCoroutine(Select_Init());
        }
        else
            MoveTo_Select();

        Inven_EquipUpdateAction?.Invoke(iCurPanelIndex, 0);
    }


    public void ShowAttackOrArmorValues(Item_Equipment _EquipItem)
    {
        switch (_EquipItem.My_EquipData.iInvenType)
        {
            case (int)Item.Inven_Type.Item_Weapon:
            case (int)Item.Inven_Type.Item_Bow:
                Get<Inven_ShowValue>((int)Inven_Values.AttackArmorValues).ShowAttackValueTexts(_EquipItem.My_EquipData.iValue);
                break;
            case (int)Item.Inven_Type.Item_Shield:
                Get<Inven_ShowValue>((int)Inven_Values.AttackArmorValues).ShowShieldValueTexts(_EquipItem.My_EquipData.iValue);
                break;
            case (int)Item.Inven_Type.Item_Armor:
                Get<Inven_ShowValue>((int)Inven_Values.AttackArmorValues).ShowArmorValueTexts(_EquipItem.My_EquipData.iValue);
                break;
        }
    }
    public void HideAttackOrArmorValues(Item_Equipment _EquipItem)
    {
        switch (_EquipItem.My_EquipData.iInvenType)
        {
            case (int)Item.Inven_Type.Item_Weapon:
            case (int)Item.Inven_Type.Item_Bow:
                Get<Inven_ShowValue>((int)Inven_Values.AttackArmorValues).HideAttackValueTexts(_EquipItem.My_EquipData.iValue);
                break;
            case (int)Item.Inven_Type.Item_Shield:
                Get<Inven_ShowValue>((int)Inven_Values.AttackArmorValues).HideShieldValueTexts(_EquipItem.My_EquipData.iValue);
                break;
            case (int)Item.Inven_Type.Item_Armor:
                Get<Inven_ShowValue>((int)Inven_Values.AttackArmorValues).HideArmorValueTexts(_EquipItem.My_EquipData.iValue);
                break;
        }
    }

    public override void init()
    {
        Bind<Inven_Title_Icon>(typeof(Title_Panel));
        Bind<Inven_ScrollView>(typeof(Inven_Scroll));
        Bind<Inven_Infomation>(typeof(Inven_Info));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
        Bind<Inven_Setter>(typeof(Inven_Set));
        Bind<Inven_ShowValue>(typeof(Inven_Values));

        AnywayInit();

        Bind_Init();

        Index_Init();

        MaxIndex_Init();

        Mileston_Update();

        MoveTo_Select();

        My_Inven.init(this);

        Inven_EquipUpdateAction?.Invoke(iCurPanelIndex, 0);
    }

    private void AnywayInit()
    {
        My_Inven = GameManager.item.Inven;

        State = InputState.NonMove;
        ShowScale = Get<GameObject>((int)GameObjects.Milestone_Left).transform.localScale;

        Main_PopUp = gameObject.transform.parent.GetComponent<UI_MainPopUp>();

        // if (null == Main_PopUp)
            //Debug.Log("Failed to Find MainPopup From UI_Inven AnyInit");
    }

    private void Bind_Init()
    {
        Get<Inven_Title_Icon>((int)Title_Panel.Icon_Panel).init();
        Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory).init();
        Get<Inven_Infomation>((int)Inven_Info.InfoImage).init();
        Get<Inven_Setter>((int)Inven_Set.Setter).CustomInit(this, Main_PopUp);
        Get<Inven_ShowValue>((int)Inven_Values.AttackArmorValues).init();
    }

    private void MaxIndex_Init()
    {
        iMaxPanelIndex = Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory).MaxPanelIndex;
        iMaxRowIndex = Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory).Panel.MaxRowIndex;
        iMaxColumnIndex = Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory).Panel.MaxColumnIndex;
    }

    private void Index_Init()
    {
        iRowIndex = 0;
        iColumnIndex = 0;
        iCurPanelIndex = 0;
    }

    private PositionState Get_PositionState(int _iCurIndex, int iMaxIndex, int iMinIndex = 0)
    {
        if (iMinIndex < _iCurIndex && iMaxIndex > _iCurIndex)
            return PositionState.Middle;
        else if (iMinIndex == _iCurIndex)
            return PositionState.Left;
        else if (iMaxIndex == _iCurIndex)
            return PositionState.Right;

        return PositionState.Unknow;
    }

    private void MoveTo_Select()
    {
        Inven_Panel Panel = Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory)?.Panel;

        Current_Slot = Panel?.Get_Slot(iRowIndex, iColumnIndex);

        if (null == Current_Slot)
            return;

        Get<Inven_Title_Icon>((int)Title_Panel.Icon_Panel).ChangeIcon(iCurPanelIndex);
        Get<GameObject>((int)GameObjects.MainSelect).transform.position = Current_Slot.transform.position;

        Information_ShowAndHide(Current_Slot);

        Mileston_Update();
    }

    private IEnumerator Select_Init()
    {
        yield return new WaitForEndOfFrame();

        Inven_Panel Panel = Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory)?.Panel;

        Current_Slot = Panel?.Get_Slot(iRowIndex, iColumnIndex);

        if (null == Current_Slot)
            yield break;

        Get<Inven_Title_Icon>((int)Title_Panel.Icon_Panel).ChangeIcon(iCurPanelIndex);
        Get<GameObject>((int)GameObjects.MainSelect).transform.position = Current_Slot.transform.position;

        Information_ShowAndHide(Current_Slot);

        Mileston_Update();
    }

    private void Information_ShowAndHide(Inven_Slot _Slot)
    {
        if (null != _Slot && string.Empty != _Slot.strItem_ToolTip)
        {
            Get<Inven_Infomation>((int)Inven_Info.InfoImage).gameObject.SetActive(true);
            Get<Inven_Infomation>((int)Inven_Info.InfoImage).Show_Text(Current_Slot.strItem_TitleToolTip, Current_Slot.strItem_ToolTip);
        }
        else
        {
            Get<Inven_Infomation>((int)Inven_Info.InfoImage).Hide_Text();
            Get<Inven_Infomation>((int)Inven_Info.InfoImage).gameObject.SetActive(false);
        }
    }

    private void Slot_State()
    {
        switch (Get_PositionState(iColumnIndex, iMaxColumnIndex))
        {
            case PositionState.Left:
                Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory).MoveTo_Panel(false);
                StartCoroutine(Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory).MoveTo_ScrollView(this));
                iCurPanelIndex = Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory).PanelIndex;
                iColumnIndex = iMaxColumnIndex;
                Inven_EquipUpdateAction?.Invoke(iCurPanelIndex, 0);
                break;
            case PositionState.Middle:
                break;
            case PositionState.Right:
                Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory).MoveTo_Panel(true);
                StartCoroutine(Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory).MoveTo_ScrollView(this));
                iCurPanelIndex = Get<Inven_ScrollView>((int)Inven_Scroll.InvenTory).PanelIndex;
                iColumnIndex = 0;
                Inven_EquipUpdateAction?.Invoke(iCurPanelIndex, 0);
                break;
            case PositionState.Unknow:
                break;
        }

    }

    private PositionState Panel_State()
    {
        switch (Get_PositionState(iCurPanelIndex, iMaxPanelIndex))
        {
            case PositionState.Left:
                return PositionState.Left;
            case PositionState.Middle:
                return PositionState.Middle;
            case PositionState.Right:
                return PositionState.Right;
        }

        return PositionState.Unknow;
    }

    private void Mileston_Update()
    {
        switch (Get_PositionState(iCurPanelIndex, iMaxPanelIndex))
        {
            case PositionState.Left:
                Mileston_ShowAndHide(ShowScale, HideScale);
                break;
            case PositionState.Middle:
                Mileston_ShowAndHide(ShowScale, ShowScale);
                break;
            case PositionState.Right:
                Mileston_ShowAndHide(HideScale, ShowScale);
                break;
        }
    }

    private void Mileston_ShowAndHide(Vector3 _RightScale, Vector3 _LeftScale)
    {
        Get<GameObject>((int)GameObjects.Milestone_Right).transform.localScale = _RightScale;
        Get<GameObject>((int)GameObjects.Milestone_Left).transform.localScale = _LeftScale;
    }

    public void KeyInput()
    {
        switch (State)
        {
            case InputState.Move:
                MoveTo_Select();
                MasterAudio.PlaySound("INVEN3");
                break;
            case InputState.NonMove:
                if (Input.GetKeyDown(KeyCode.W))
                {
                    iRowIndex = Mathf.Max(0, --iRowIndex);
                    MoveTo_Select();
                    MasterAudio.PlaySound("INVEN2");
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    iRowIndex = Mathf.Min(iMaxRowIndex, ++iRowIndex);
                    MoveTo_Select();
                    MasterAudio.PlaySound("INVEN2");
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    MasterAudio.PlaySound("INVEN2");
                    if (0 == iColumnIndex && PositionState.Left != Panel_State())
                        Slot_State();
                    else
                        iColumnIndex = Mathf.Max(0, --iColumnIndex);

                    MoveTo_Select();
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    MasterAudio.PlaySound("INVEN2");
                    if (iMaxColumnIndex == iColumnIndex && PositionState.Right != Panel_State())
                        Slot_State();
                    else
                        iColumnIndex = Mathf.Min(iMaxColumnIndex, ++iColumnIndex);

                    MoveTo_Select();
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    MasterAudio.PlaySound("INVEN1");
                    if (true == Get<Inven_Setter>((int)Inven_Set.Setter).ShowAndHide_Icon(My_Inven.Get_Item(iCurPanelIndex, iRowIndex, iColumnIndex), true))
                    {
                        Main_PopUp.AddKeyAction(Get<Inven_Setter>((int)Inven_Set.Setter).Key_Input);
                        Main_PopUp.DeleteKeyAction(KeyInput, true);
                    }
                }
                break;
        }
    }

}
