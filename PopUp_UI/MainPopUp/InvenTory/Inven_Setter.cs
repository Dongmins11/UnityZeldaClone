using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class Inven_Setter : UI_Base
{
    enum GameObjects
    {
        EquipSetterImage,
        UseableSetterImage,
        SettingSelector,
        HightLightImage,
    }
    enum Images
    {
        EquipImage,
        DropImage,
        Equip_CancelImage,
        UseImage,
        Use_CancelImage,
    }
    enum Texts
    {
        EquipText,
    }

    enum SetterType
    {
        Type_Equip,
        Type_Useable,
        Type_Unknow,
    }

    UI_Inventory My_Inven;
    UI_MainPopUp My_MainPopUp;

    Animator HightLightAnimator;
    Item CurItem;

    Action SetterEvent = null;

    int iCurrentIndex = 0;
    int iMaxIndex = 3;
    int iMinIndex = 2;

    SetterType CurType;
    SetterType PreType;

    int AnimationHash;

    public override void init() { }

    public void CustomInit(UI_Inventory _UI_Inven, UI_MainPopUp _MainPopUp)
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        if (null != _UI_Inven && null != _MainPopUp)
        {
            My_MainPopUp = _MainPopUp;
            My_Inven = _UI_Inven;
        }

        CurType = SetterType.Type_Unknow;
        PreType = SetterType.Type_Unknow;

        Get<GameObject>((int)GameObjects.SettingSelector).SetActive(false);
        Get<GameObject>((int)GameObjects.EquipSetterImage).SetActive(false);
        Get<GameObject>((int)GameObjects.UseableSetterImage).SetActive(false);

        HightLightInit();
    }


    void HightLightInit()
    {
        HightLightAnimator = GetComponent<Animator>();

        AnimationHash = Animator.StringToHash("SetterAlpha");
    }

    void TextChange(Item _CurItem)
    {
        if (null == _CurItem)
            return;

        if (_CurItem is Item_Equipment EquipItem)
        {
            if (false == EquipItem.IsEquip)
                GetText((int)Texts.EquipText).text = "Equip";
            else
                GetText((int)Texts.EquipText).text = "UnEquip";
        }
    }


    public bool ShowAndHide_Icon(Item _Item, bool _bActive)
    {
        if (null == _Item)
            return false;

        switch (_Item.My_Data.iInvenType)
        {
            case (int)Item.Inven_Type.Item_Weapon:
            case (int)Item.Inven_Type.Item_Bow:
            case (int)Item.Inven_Type.Item_Shield:
            case (int)Item.Inven_Type.Item_Armor:
                Get<GameObject>((int)GameObjects.EquipSetterImage).SetActive(_bActive);
                CurType = SetterType.Type_Equip;
                TypeChange();
                break;
            case (int)Item.Inven_Type.Item_Food:
                Get<GameObject>((int)GameObjects.UseableSetterImage).SetActive(_bActive);
                CurType = SetterType.Type_Useable;
                TypeChange();
                break;
            case (int)Item.Inven_Type.Item_Material:
                return false;
        }

        Move_To_Selector();
        Get<GameObject>((int)GameObjects.SettingSelector).SetActive(_bActive);

        if (false == _bActive)
            CurItem = null;
        else
            CurItem = _Item;

        TextChange(CurItem);

        return true;
    }


    void TypeChange()
    {
        if (CurType != PreType)
        {
            switch (CurType)
            {
                case SetterType.Type_Equip:
                    iMinIndex = (int)Images.EquipImage;
                    iMaxIndex = (int)Images.Equip_CancelImage;
                    break;
                case SetterType.Type_Useable:
                    iMinIndex = (int)Images.UseImage;
                    iMaxIndex = (int)Images.Use_CancelImage;
                    break;
            }
            PreType = CurType;
        }

        iCurrentIndex = iMinIndex;
    }

    void Move_To_Selector()
    {
        Vector3 CurPosition = GetImage(iCurrentIndex).gameObject.transform.position;

        Get<GameObject>((int)GameObjects.SettingSelector).transform.position = CurPosition;

        SelectAddAction();
    }

    void SelectAddAction()
    {
        switch (CurType)
        {
            case SetterType.Type_Equip:
                switch (iCurrentIndex)
                {
                    case (int)Images.EquipImage:
                        SetterEvent = My_Inven.UseItem;
                        break;
                    case (int)Images.DropImage:
                        SetterEvent = null;
                        break;
                    case (int)Images.Equip_CancelImage:
                        SetterEvent = null;
                        break;
                }
                break;
            case SetterType.Type_Useable:
                switch (iCurrentIndex)
                {
                    case (int)Images.UseImage:
                        SetterEvent = My_Inven.UseItem;
                        break;
                    case (int)Images.Use_CancelImage:
                        SetterEvent = null;
                        break;
                }
                break;
        }
    }

    void EventCall()
    {
        ShowAndHide_Icon(CurItem, false);
        My_MainPopUp.AddKeyAction(My_Inven.KeyInput, true);
        SetterEvent = null;
    }

    public void AddSetterAction(Action _Funtion)
    {
        SetterEvent -= _Funtion;
        SetterEvent += _Funtion;
    }

    public void Key_Input()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            iCurrentIndex = Mathf.Max(iMinIndex, --iCurrentIndex);
            Move_To_Selector();
            MasterAudio.PlaySound("INVEN2");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            iCurrentIndex = Mathf.Min(++iCurrentIndex, iMaxIndex);
            Move_To_Selector();
            MasterAudio.PlaySound("INVEN2");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SetterEvent?.Invoke();
            HightLightAnimator.SetTrigger(AnimationHash);
            My_MainPopUp.DeleteKeyAction(Key_Input);
            MasterAudio.PlaySound("INVEN3");
        }
    }

}
