using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemData
{
    public string strId;
    public string strname;
    public string strToolTip;
    public string strTexturePath;
    public string strPrefabPath;
    public int iInvenType;

}
[Serializable]
public class ItemEquipData : ItemData
{
    public int iValue;
    public int iType;
}
[Serializable]
public class ItemArmorData : ItemEquipData
{
    public int iPrice;
}
[Serializable]
public class ItemCountableData : ItemData
{
    public int iAmount;
}
[Serializable]
public class ItemFoodData : ItemCountableData
{
    public int iValue;
    public int iPrice;
}
[Serializable]
public class ItemMaterial : ItemCountableData
{
    public float fValue;
}
