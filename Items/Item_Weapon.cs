using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Weapon : Item_Equipment, IEquipItem
{
    public Item_Weapon(ItemEquipData _Data) : base(_Data) { }

    public void Equip()
    {
        GameManager.item.InvenEquipComponet?.AddEquipment_Weapon(My_EquipData);
        GameManager.item.PlayerEquipComponet?.AddEquipment_Weapon(My_EquipData);

        IsEquip = true;
    }

    public void UnEquip()
    {
        GameManager.item.InvenEquipComponet?.RemoveEquipment_Weapon(My_EquipData);
        GameManager.item.PlayerEquipComponet?.RemoveEquipment_Weapon(My_EquipData);

        IsEquip = false;
    }
}
