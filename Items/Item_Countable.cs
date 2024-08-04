using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsableItem
{
    public bool Use();
}

public class Item_Countable : Item
{
    public int iCount { get; protected set; }

    public ItemCountableData My_CountableData { get; private set; }

    public const int iMaxCount = 99;

    public bool MaxCheck(){ return iCount >= iMaxCount; }
    
    public bool EmptyCheck() { return 0 >= iCount; }

    public void SetCount(int _iCount) { iCount = Mathf.Clamp(_iCount, 0, iMaxCount); }

    public int AddCount(int _iCount)
    {
        int NextCount = iCount + _iCount;
        SetCount(NextCount);

        return (NextCount > iMaxCount) ? (NextCount - iMaxCount) : 0;
    }


    public Item_Countable(ItemCountableData _Data, int _iCount = 1) : base(_Data)
    {
        My_CountableData = _Data;
        SetCount(_iCount);
    }







}
