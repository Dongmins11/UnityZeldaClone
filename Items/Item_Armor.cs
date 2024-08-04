using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Armor : Item_Equipment, IEquipItem
{
    public ItemArmorData My_ArmorData { get; private set; }

    public Item_Armor(ItemArmorData _ArmorData) : base(_ArmorData)
    {
        My_ArmorData = _ArmorData;
    }
    public void Equip()
    {
        GameManager.item.InvenEquipComponet?.AddEquipment_Armor(My_EquipData.strId, My_EquipData.iType);
        GameManager.item.PlayerEquipComponet?.AddEquipment_Armor(My_EquipData.strId, My_EquipData.iType);

        //foreach (Equipment iter in GameManager.item.EquipComponent)
           //iter.AddEquipment_Armor(My_EquipData.strId, My_EquipData.iType);

        //여기다가 플레이어 무기 장착
        //데이터를 넘겨준다.

        IsEquip = true;
    }

    public void UnEquip()
    {
        GameManager.item.InvenEquipComponet?.RemoveEquipment_Armor(My_EquipData.iType);
        GameManager.item.PlayerEquipComponet?.RemoveEquipment_Armor(My_EquipData.iType);

        //foreach (Equipment iter in GameManager.item.EquipComponent)
            //iter.RemoveEquipment_Armor(My_EquipData.iType);

        IsEquip = false;
    }
}
