using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Food : Item_Countable , IUsableItem
{
    ItemFoodData My_FoodData = null;

    public Item_Food(ItemFoodData _FoodData, int _iCount = 1) : base(_FoodData, _iCount) 
    {
        My_FoodData = _FoodData;
    }

    public bool Use()
    {
        iCount = Mathf.Max(0,--iCount);

        GameManager.UIContents.Player_HealthUp(My_FoodData.iValue);

        return true;
    }
}
