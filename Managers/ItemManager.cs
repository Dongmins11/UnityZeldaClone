using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    Dictionary<string, ItemData> Dictionary_ItemDatas = new Dictionary<string, ItemData>();

    private Inventory Inven_Tory;

    public Inventory Inven
    {
        get
        {
            InvenInit();
            return Inven_Tory;
        }
    }

    private Equipment Player_EquipMent = null;

    public Equipment PlayerEquipComponet
    {
        get
        {
            Player_EquipSetting();
            return Player_EquipMent;
        }
    }

    private Equipment InvenPlayer_EquipMent = null;

    public Equipment InvenEquipComponet
    {
        get
        {
            InvenPlayer_EquipSetting();
            return InvenPlayer_EquipMent;
        }
    }

    public List<Equipment> EquipComponent { get; private set; } = new List<Equipment>();

    public void Init()
    {
        Create_ItemData<ItemEquipData>("ItemWeapons");
        Create_ItemData<ItemEquipData>("ItemBows");
        Create_ItemData<ItemArmorData>("ItemArmors");
        Create_ItemData<ItemEquipData>("ItemShields");
        Create_ItemData<ItemFoodData>("ItemFoods");
        Create_ItemData<ItemCountableData>("ItemGitas");
    }

    private void InvenInit()
    {
        if (null != Inven_Tory)
            return;

        Inventory TempInven = UnityEngine.Object.FindObjectOfType<Inventory>();

        if (null == TempInven)
        {
            GameObject Player = GameObject.FindGameObjectWithTag("Player");

            if (null == Player)
            {
                GameObject Inven_Object = new GameObject("@Inventory");
                Inven_Tory = Util.GetOrAddComponent<Inventory>(Inven_Object);
            }
            else
                Inven_Tory = Util.GetOrAddComponent<Inventory>(Player);

        }
        else
            Inven_Tory = TempInven;

        UnityEngine.Object.DontDestroyOnLoad(Inven_Tory);
    }

    private void Player_EquipSetting()
    {
        if (null == Player_EquipMent)
        {
            GameObject Player = GameObject.FindGameObjectWithTag("Player");
            if (null != Player)
                Player_EquipMent = Util.GetOrAddComponent<Equipment>(Player);
        }
    }

    private void InvenPlayer_EquipSetting()
    {
        if (null == InvenPlayer_EquipMent)
        {
            GameObject Player = GameObject.FindGameObjectWithTag("InvenPlayer");
            if (null != Player)
                InvenPlayer_EquipMent = Util.GetOrAddComponent<Equipment>(Player);
        }
    }

    void Create_ItemData<T>(string _DataPath) where T : ItemData
    {
        List<T> Data = null;

        Data = GameManager.Data.LoadData<List<T>>(_DataPath);

        if (null != Data)
        {
            for (int i = 0; i < Data.Count; ++i)
            {
                Dictionary_ItemDatas.Add(Data[i].strId, Data[i]);
            }
        }
    }

    public GameObject Create_Item(string _ItemName)
    {
        GameObject Item = GameManager.Resources.CreatePrefab($"Prefabs/Item/{_ItemName}");

        if (null == Item)
            return null;

        return Item;
    }

    public T Get_ItemData<T>(string _strId) where T : ItemData
    {
        ItemData OutData;

        Dictionary_ItemDatas.TryGetValue(_strId, out OutData);

        return OutData as T;
    }

    public ItemData Get_ItemData(string _strId)
    {
        ItemData OutData;

        Dictionary_ItemDatas.TryGetValue(_strId, out OutData);

        return OutData;
    }

}



