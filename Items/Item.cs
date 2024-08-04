using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item
{
    public enum Inven_Type
    {
        Item_Weapon,
        Item_Bow,
        Item_Shield,
        Item_Armor,
        Item_Food,
        Item_Material,
        Item_Rupee,
    }

    public ItemData My_Data { get; set; }

    protected Item(ItemData _Data)
    {
        My_Data = _Data;
    }

}
