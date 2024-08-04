using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipItem
{
    public void Equip();
    public void UnEquip();
}

public class Item_Equipment : Item
{
    public enum EquipType
    {
        Equip,
        UnEquip,
        Change,
    }

    public bool IsEquip { get; protected set; } = false;

    public EquipType EquipState { get; protected set; }

    public ItemEquipData My_EquipData { get; private set; }

    protected Item_Equipment EquiptedItem = null;

    public Item_Equipment(ItemEquipData _Data) : base(_Data)
    {
        EquipState = EquipType.UnEquip;

        My_EquipData = _Data;
    }
}
