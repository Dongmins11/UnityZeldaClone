using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    enum Inventory_Type
    {
        Type_Unknow = -1,
        Type_EquipmentSlot,
        Type_Countable,
        Type_NonSlot,
    }

    enum Add_Type
    {
        Type_Failed = -1,
        Type_Success,
    }

    private int iSize;

    private int iMaxRow;
    private int iMaxColumn;
    private int iMaxSize;
    private int iMaxType;

    private const int iMaxRupeeCount = 5000;

    private UI_Inventory Inventory_UI;

    private Item[,] ArrayItems;

    public int iRupeeCount { get; private set; } = 0;

    public Action<Item> WeaponEquipAction = null;
    public Action<Item> WeaponUnEquipAction = null;

    public void init(UI_Inventory _UI_Inven)
    {
        if (null == _UI_Inven)
            return;

        Inventory_UI = _UI_Inven;

        iMaxRow = Inventory_UI.iMaxRowIndex + 1;

        iMaxColumn = Inventory_UI.iMaxColumnIndex + 1;

        iMaxType = Inventory_UI.iMaxPanelIndex + 1;

        iMaxSize = Get_Index(iMaxRow - 1, iMaxColumn);

        iSize = iMaxSize;

        ArrayItems = new Item[Inventory_UI.iMaxPanelIndex + 1, iMaxSize];

        Inventory_UI.Inven_EquipUpdateAction -= WeaponEquipItem;
        Inventory_UI.Inven_EquipUpdateAction += WeaponEquipItem;
    }

    private bool IsValidIndex(int _iIndex) { return !(0 <= _iIndex && _iIndex < iSize); }
    private bool IsValidType(int _iType) { return !(0 <= _iType && _iType < iMaxType); }

    private int Get_Index(int _iRow, int _iColumn) { return (iMaxColumn * _iRow) + _iColumn; }
    private int Get_Row(int _iIndex, int _iNumRow = 5)
    {
        int Row = _iIndex / _iNumRow;
        return Row;
    }
    private int Get_Column(int _iIndex, int _iNumColumn = 5)
    {
        int Col = _iIndex % _iNumColumn;
        return Col;
    }

    private int FindEmptySlotIndex(int _iType, int _iStartIndex = 0)
    {
        for (int i = _iStartIndex; i < iSize; i++)
            if (ArrayItems[_iType, i] == null)
                return i;

        return -1;
    }

    private int FindCountableItemSlotIndex(ItemCountableData _ItemCountData, int _iStartIndex = 0)
    {
        for (int i = _iStartIndex; i < iSize; i++)
        {
            Item current = ArrayItems[_ItemCountData.iInvenType, i];
            if (current == null)
                continue;

            if (current.My_Data == _ItemCountData && current is Item_Countable CountData)
            {
                if (!CountData.MaxCheck())
                    return i;
            }
        }

        return -1;
    }

    private Item_Equipment Find_TypeEquipItem(Item_Equipment _Item, out int _FindIndex, int _iStartIndex = 0)
    {
        for (int i = _iStartIndex; i < iSize; i++)
        {
            Item_Equipment current = ArrayItems[_Item.My_Data.iInvenType, i] as Item_Equipment;

            if (current == null)
                continue;

            if (current is Item_Armor)
            {
                if (true == current.IsEquip && current.My_EquipData.iType == _Item.My_EquipData.iType)
                {
                    _FindIndex = i;
                    return current;
                }
            }
            else
            {
                if (true == current.IsEquip && current.My_EquipData.iInvenType == _Item.My_EquipData.iInvenType)
                {
                    _FindIndex = i;
                    return current;
                }
            }

        }
        _FindIndex = -1;

        return null;
    }

    private void WeaponEquipItem(int _iInvenType, int _iStartIndex = 0)
    {
        bool IsEquipCheck = false;

        for (int i = _iStartIndex; i < iSize; i++)
        {
            Item current = ArrayItems[_iInvenType, i];
            if (current == null)
                continue;

            if (current is Item_Weapon Weapon)
            {
                if (true == Weapon.IsEquip)
                {
                    WeaponEquipAction?.Invoke(Weapon);
                    IsEquipCheck = true;
                }
                else
                    WeaponUnEquipAction?.Invoke(current);
            }
            else
            {
                WeaponUnEquipAction?.Invoke(current);
                return;
            }
        }

        if (false == IsEquipCheck)
            WeaponUnEquipAction?.Invoke(null);
    }

    private bool HasItem(int _iType, int _iIndex)
    {
        return IsValidIndex(_iIndex) && IsValidType(_iType) && ArrayItems[_iType, _iIndex] != null;
    }

    public ItemData Get_ItemData(int _iType, int _iIndex)
    {
        if (HasItem(_iType, _iIndex))
            return null;

        return ArrayItems[_iType, _iIndex].My_Data;
    }

    public ItemData Get_ItemData(int _iType, int _iRow, int _iColumn)
    {
        if (HasItem(_iType, Get_Index(_iRow, _iColumn)))
            return null;

        return ArrayItems[_iType, Get_Index(_iRow, _iColumn)].My_Data;
    }

    public Item Get_Item(int _iType, int _iRow, int _iColumn)
    {
        if (HasItem(_iType, Get_Index(_iRow, _iColumn)))
            return null;

        return ArrayItems[_iType, Get_Index(_iRow, _iColumn)];
    }

    public int Get_CountableAmount(int _iType, int _iIndex)
    {
        if (IsValidIndex(_iIndex) && IsValidType(_iType))
            return (int)Inventory_Type.Type_Unknow;

        if (null == ArrayItems[_iType, _iIndex])
            return (int)Inventory_Type.Type_NonSlot;

        Item_Countable Item = ArrayItems[_iType, _iIndex] as Item_Countable;

        if (null == Item)
            return (int)Inventory_Type.Type_EquipmentSlot;

        return Item.iCount;
    }

    public int Get_CountableAmount(ItemData _Data)
    {
        if (IsValidType(_Data.iInvenType))
            return (int)Inventory_Type.Type_Unknow;

        if (_Data is ItemCountableData CountData)
        {
            int iIndex = -1;

            iIndex = FindCountableItemSlotIndex(CountData, iIndex + 1);

            if (-1 == iIndex)
                return 0;

            Item_Countable Item = ArrayItems[_Data.iInvenType, iIndex] as Item_Countable;

            return Item.iCount;
        }

        return (int)Inventory_Type.Type_EquipmentSlot;
    }

    public void Update_Inven_UI(int _iType, int _iRow, int _iColumn)
    {
        int iCurrentIndex = Get_Index(_iRow, _iColumn);

        if (IsValidIndex(iCurrentIndex))
            return;

        Item MyItem = ArrayItems[_iType, iCurrentIndex];

        Inven_Slot Slot = Inventory_UI.Get_Slot(_iType, _iRow, _iColumn);

        if (null != MyItem && null != Slot)
        {
            Slot.Show_Item_Icon(MyItem.My_Data);

            if (MyItem is Item_Countable CountItem)
            {
                if (CountItem.EmptyCheck())
                {
                    ArrayItems[_iType, iCurrentIndex] = null;
                    Slot.Hide_Item_Icon();
                    return;
                }
                else
                    Slot.Show_CountText_Icon(CountItem.iCount);
            }
            else if (MyItem is Item_Equipment EquipTime)
                Slot.Show_Value_Icon(MyItem.My_Data);
        }
        else
        {
            Slot.All_Hide_Icon();
        }
    }

    public int Add(ItemData _Data, int _iCount = 1)
    {
        int iIndex = 0;

        if (_Data is ItemCountableData CountData)
        {
            bool bIsCountItemCheck = true;
            iIndex = -1;

            while (0 < _iCount)
            {
                if (bIsCountItemCheck)
                {
                    iIndex = FindCountableItemSlotIndex(CountData, iIndex + 1);

                    if (-1 == iIndex)
                        bIsCountItemCheck = false;
                    else
                    {
                        Item_Countable CountItem = ArrayItems[_Data.iInvenType, iIndex] as Item_Countable;

                        _iCount = CountItem.AddCount(_iCount);

                        Update_Inven_UI(_Data.iInvenType, Get_Row(iIndex, iMaxColumn), Get_Column(iIndex, iMaxColumn));

                    }
                }
                else
                {
                    iIndex = FindEmptySlotIndex(_Data.iInvenType, iIndex + 1);

                    if (-1 == iIndex)
                        break;
                    else
                    {
                        Item_Countable CountItem = Get_CountableWithType(CountData);

                        _iCount = CountItem.AddCount(_iCount);

                        ArrayItems[CountItem.My_CountableData.iInvenType, iIndex] = CountItem;

                        Update_Inven_UI(CountItem.My_CountableData.iInvenType, Get_Row(iIndex), Get_Column(iIndex));
                    }

                }
            }
        }
        else if (_Data is ItemEquipData EquipData)
        {
            if (_iCount == 1)
            {
                iIndex = FindEmptySlotIndex(_Data.iInvenType);

                if (iIndex != -1)
                {

                    Item_Equipment EquipItem = Get_EquipWithType(EquipData);

                    ArrayItems[_Data.iInvenType, iIndex] = EquipItem;

                    Update_Inven_UI(EquipItem.My_Data.iInvenType, Get_Row(iIndex), Get_Column(iIndex));

                    _iCount = 0;
                }
            }
            else
                return -1;
        }

        GameManager.Quest.Quest_Item_Action?.Invoke();

        return _iCount;
    }

    private Item_Equipment Get_EquipWithType(ItemEquipData _Data)
    {
        int iType = _Data.iInvenType;

        switch (iType)
        {
            case (int)Item.Inven_Type.Item_Weapon:
                return new Item_Weapon(_Data);
            case (int)Item.Inven_Type.Item_Bow:
                return new Item_Weapon(_Data);
            case (int)Item.Inven_Type.Item_Shield:
                return new Item_Weapon(_Data);
            case (int)Item.Inven_Type.Item_Armor:
                if (_Data is ItemArmorData ArmorData)
                    return new Item_Armor(ArmorData);
                else
                    return null;
        }
        return null;
    }

    private Item_Countable Get_CountableWithType(ItemCountableData _Data, int _iCount = 0)
    {
        int iType = _Data.iInvenType;

        switch (iType)
        {
            case (int)Item.Inven_Type.Item_Food:
                if (_Data is ItemFoodData FoodData)
                    return new Item_Food(FoodData, _iCount);
                else
                    return null;
            case (int)Item.Inven_Type.Item_Material:
                return new Item_Countable(_Data, _iCount);
        }
        return null;
    }

    public void Add_Rupee(ItemData _Data)
    {
        if (_Data is ItemCountableData CountData)
        {
            iRupeeCount = Mathf.Min(iRupeeCount + CountData.iAmount, iMaxRupeeCount);
            Inventory_UI.Set_Rupee(iRupeeCount);
        }
    }

    public int Remove_Rupee(ItemData _Data)
    {
        if (_Data is ItemArmorData ArmorData)
        {
            iRupeeCount = Mathf.Max(0, iRupeeCount - ArmorData.iPrice);
            Inventory_UI.Set_Rupee(iRupeeCount);
        }

        return iRupeeCount;
    }

    public void Add_Rupee(int _iCount)
    {
        iRupeeCount = Mathf.Min(iRupeeCount + _iCount, iMaxRupeeCount);
        Inventory_UI.Set_Rupee(iRupeeCount);
    }

    public int Remove_Rupee(int _iCount)
    {
        iRupeeCount = Mathf.Max(0, iRupeeCount - _iCount);
        Inventory_UI.Set_Rupee(iRupeeCount);

        return iRupeeCount;
    }

    public bool Price_Rupee(int _iPriceRupee)
    {
        if (_iPriceRupee <= iRupeeCount)
            return true;

        return false;
    }

    public void Remove(int _iType, int _iIndex)
    {
        if (IsValidIndex(_iIndex)) return;

        ArrayItems[_iType, _iIndex] = null;
        Inventory_UI.Remove_UI(_iType, Get_Row(_iIndex), Get_Column(_iIndex));
    }

    public void Remove(ItemData _Data)
    {
        int iIndex = 1;

        if (_Data is ItemCountableData _CountData)
            iIndex = FindCountableItemSlotIndex(_CountData);

        if (-1 != iIndex)
            Remove(_Data.iInvenType, iIndex);
    }

    public void UseItem(int _iType, int _iRow, int _iColumn)
    {
        int iIndex = Get_Index(_iRow, _iColumn);

        if (IsValidIndex(iIndex) || ArrayItems[_iType, iIndex] == null)
            return;

        if (ArrayItems[_iType, iIndex] is IUsableItem uItem)
        {
            bool IsTrue = uItem.Use();

            if (IsTrue)
                Update_Inven_UI(_iType, Get_Row(iIndex), Get_Column(iIndex));
        }
        else if (ArrayItems[_iType, iIndex] is Item_Equipment EquipItem)
        {
            if (EquipItem is IEquipItem CurItem)
            {
                Item_Equipment FindItem = null;
                int iFindIndex = -1;

                if (null != (FindItem = Find_TypeEquipItem(EquipItem, out iFindIndex)))
                {
                    IEquipItem PreviorItem = FindItem as IEquipItem;

                    if (FindItem.My_EquipData.strId == EquipItem.My_EquipData.strId)
                    {
                        Update_UI_UnEquip(_iType, _iRow, _iColumn, EquipItem);

                        CurItem.UnEquip();
                    }
                    else
                    {
                        Update_UI_PreUnEquipAndCurEquip(_iType, _iRow, _iColumn, iFindIndex, FindItem, EquipItem);

                        PreviorItem.UnEquip();
                        CurItem.Equip();
                    }
                }
                else
                {
                    Update_UI_Equip(_iType, _iRow, _iColumn, EquipItem);
                    CurItem.Equip();
                }

                Update_Inven_UI(_iType, _iRow, _iColumn);
                WeaponEquipItem(_iType, 0);
            }
        }
    }

    private void Update_UI_UnEquip(int _iType, int _iRow, int _iCol, Item_Equipment _EquipItem)
    {
        Inventory_UI.Get_Slot(_iType, _iRow, _iCol).Hide_Equip_Slot();
        Inventory_UI.HideAttackOrArmorValues(_EquipItem);
    }

    private void Update_UI_Equip(int _iType, int _iRow, int _iCol, Item_Equipment _EquipItem)
    {
        Inventory_UI.ShowAttackOrArmorValues(_EquipItem);
        Inventory_UI.Get_Slot(_iType, _iRow, _iCol).Show_Equip_Slot();
    }

    private void Update_UI_PreUnEquipAndCurEquip(int _iType, int _iRow, int _iCol,int _iFindIndex, Item_Equipment PreItem, Item_Equipment CurItem)
    {
        Inventory_UI.HideAttackOrArmorValues(PreItem);
        Inventory_UI.ShowAttackOrArmorValues(CurItem);

        Inventory_UI.Get_Slot(_iType, Get_Row(_iFindIndex), Get_Column(_iFindIndex)).Hide_Equip_Slot();
        Inventory_UI.Get_Slot(_iType, _iRow, _iCol).Show_Equip_Slot();
    }


}
