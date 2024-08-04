using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inven_Panel : UI_Base
{
    Inven_Slot[,] My_Item;

    const int iDefaltRow = 3;
    const int iDefaltColumn = 5;

    public int MaxRowIndex { get { return iDefaltRow - 1; } }
    public int MaxColumnIndex { get { return iDefaltColumn - 1; } }

    public override void init()
    {
        ItemSetting();
    }

    void ItemSetting()
    {
        My_Item = new Inven_Slot[iDefaltRow, iDefaltColumn];

        foreach (Transform child in transform)
        {
            GameManager.Resources.Destroy(child.gameObject);
        }

        for (int i = 0; i < iDefaltRow; ++i)
        {
            Inven_Slot[] ArrayTempItem = new Inven_Slot[iDefaltColumn];

            for (int j = 0; j < iDefaltColumn; ++j)
            {
                GameObject item = GameManager.UI.MakeSubItem<Inven_Slot>(transform).gameObject;

                Inven_Slot inventitem = item.GetOrAddComponent<Inven_Slot>();
                inventitem.init();

                My_Item[i, j] = inventitem;
            }

        }

    }


    public Inven_Slot Get_Slot(int _iRow, int _iColumn)
    {
        if(0 <= _iRow && _iRow < iDefaltRow && 0 <= _iColumn && _iColumn < iDefaltColumn)
        {
            return My_Item[_iRow, _iColumn];
        }    
        else
        {
            //Debug.Log("Failed to Get_Slot Out Of Range");
            return null;
        }    

       
    }
  
}
