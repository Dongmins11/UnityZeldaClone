using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public enum EquipType
    {
        EquipType_Hair,
        EquipType_Body,
        EquipType_Lower,
    }

    public enum WeaponEquip_Type
    {
        Weapon_Bow,
        Weapon_Shield,
        Weapon_Arrow,
        Weapon_Spear,
        Weapon_Sword,
        Weapon_Bow_Sheath,
        Weapon_Bow_Quiver,
        Weapon_Spear_Sheath,
        Weapon_Sword_Sheath,
        Weapon_Shield_Sheath,
    }

    enum Weapon_Type
    {
        Bow,
        Shield,
        Arrow,
        Spear,
        Sword,
        Quiver,
    }

    enum Holding_Type
    {
        Front,
        Back,
    }

    Dictionary<int, List<SkinnedMeshRenderer>> My_Meshes;

    Dictionary<string, GameObject> Partz_Object;

    Dictionary<string, List<SkinnedMeshRenderer>> Partz_Meshes;

    Dictionary<int, List<Equip_Weapon>> My_WeaponParent;

    Dictionary<string, List<GameObject>> My_Weapons;

    List<GameObject> Default_Meshes = new List<GameObject>();

    string[] strCurrentPartzName;
    string[] strCurrentWeaponName;

    int iFirstInitBitMask = 0;


    private Player_SwitchWeapon CurInvenWeapon = null;

    private Animator My_InvenAnimtor = null;

    private int iNormal = Animator.StringToHash("Normal");
    private int iSword = Animator.StringToHash("Sword");
    private int iSpear = Animator.StringToHash("Spear");
    private int iBow = Animator.StringToHash("Bow");

    private void newInit()
    {
        strCurrentPartzName = new string[3];
        strCurrentWeaponName = new string[6];

        My_Meshes = new Dictionary<int, List<SkinnedMeshRenderer>>();
        Partz_Object = new Dictionary<string, GameObject>();
        Partz_Meshes = new Dictionary<string, List<SkinnedMeshRenderer>>();

        My_WeaponParent = new Dictionary<int, List<Equip_Weapon>>();
        My_Weapons = new Dictionary<string, List<GameObject>>();

        CurInvenWeapon = new Player_SwitchWeapon();

        if (gameObject.CompareTag("InvenPlayer"))
        {
            GameManager.item.Inven.WeaponEquipAction -= EquipWeaponItem;
            GameManager.item.Inven.WeaponEquipAction += EquipWeaponItem;

            GameManager.item.Inven.WeaponUnEquipAction -= UnEquipWeaponItem;
            GameManager.item.Inven.WeaponUnEquipAction += UnEquipWeaponItem;

            My_InvenAnimtor = gameObject.GetComponent<Animator>();
        }

        iFirstInitBitMask |= (1 << (int)EquipType.EquipType_Hair);
        iFirstInitBitMask |= (1 << (int)EquipType.EquipType_Body);
        iFirstInitBitMask |= (1 << (int)EquipType.EquipType_Lower);
    }

    private void PartzBindInit()
    {
        GameObject[] PartzObjects = GameManager.Resources.LoadAll<GameObject>("Prefabs/Item/Armor");

        if (null == PartzObjects)
            return;

        for (int i = 0; i < PartzObjects.Length; ++i)
        {
            GameObject Partz = GameManager.Resources.CreatePrefab($"Item/Armor/{PartzObjects[i].name}", gameObject.transform);

            DestroyComponent<Item_Prefab>(Partz);
            DestroyComponent<SphereCollider>(Partz);
            DestroyComponent<BoxCollider>(Partz);

            Partz.SetActive(false);
            Partz_Object.Add(PartzObjects[i].name, Partz);

            SkinnedMeshRenderer[] SkinMeshs = Partz.GetComponentsInChildren<SkinnedMeshRenderer>();

            if (null != SkinMeshs)
            {
                List<SkinnedMeshRenderer> SkinList = new List<SkinnedMeshRenderer>();

                foreach (SkinnedMeshRenderer Skin in SkinMeshs)
                {
                    Transform[] MyBones = new Transform[Skin.bones.Length];

                    for (int k = 0; k < Skin.bones.Length; k++)
                        MyBones[k] = FindChildByName(Skin.bones[k].name, transform);

                    Skin.bones = MyBones;
                    SkinList.Add(Skin);
                }

                Partz_Meshes.Add(PartzObjects[i].name, SkinList);
            }
        }
    }

    private void MyMeshInit()
    {
        SkinnedMeshRenderer[] MeshSkinRenders = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        for (int i = 0; i <= (int)EquipType.EquipType_Lower; ++i)
        {
            List<SkinnedMeshRenderer> Temp = new List<SkinnedMeshRenderer>();

            switch (i)
            {
                case (int)EquipType.EquipType_Hair:
                    break;
                case (int)EquipType.EquipType_Body:
                    Temp.Add(MeshSkinRenders[0]);
                    Temp.Add(MeshSkinRenders[1]);
                    Temp.Add(MeshSkinRenders[10]);
                    My_Meshes.Add((int)EquipType.EquipType_Body, Temp);
                    break;
                case (int)EquipType.EquipType_Lower:
                    Temp.Add(MeshSkinRenders[8]);
                    Temp.Add(MeshSkinRenders[9]);
                    My_Meshes.Add((int)EquipType.EquipType_Lower, Temp);
                    break;
            }
        }
    }

    private void Weapon_ParentInit()
    {
        Equip_Weapon[] MyWeaponsParent = gameObject.GetComponentsInChildren<Equip_Weapon>();

        Dictionary<string, Equip_Weapon> DicWeaponParent = new Dictionary<string, Equip_Weapon>();

        foreach (Equip_Weapon iter in MyWeaponsParent)
            DicWeaponParent.Add(iter.gameObject.name, iter);

        for (int i = 0; i <= (int)Weapon_Type.Quiver; ++i)
        {
            List<Equip_Weapon> TempList = new List<Equip_Weapon>();
            Equip_Weapon EquipTeompObject = null;

            switch (i)
            {
                case (int)Weapon_Type.Bow:
                    DicWeaponParent.TryGetValue(Enum.GetName(typeof(WeaponEquip_Type), WeaponEquip_Type.Weapon_Bow_Sheath), out EquipTeompObject);
                    TempList.Add(EquipTeompObject);
                    DicWeaponParent.TryGetValue(Enum.GetName(typeof(WeaponEquip_Type), WeaponEquip_Type.Weapon_Bow), out EquipTeompObject);
                    TempList.Add(EquipTeompObject);
                    break;
                case (int)Weapon_Type.Shield:
                    DicWeaponParent.TryGetValue(Enum.GetName(typeof(WeaponEquip_Type), WeaponEquip_Type.Weapon_Shield_Sheath), out EquipTeompObject);
                    TempList.Add(EquipTeompObject);
                    DicWeaponParent.TryGetValue(Enum.GetName(typeof(WeaponEquip_Type), WeaponEquip_Type.Weapon_Shield), out EquipTeompObject);
                    TempList.Add(EquipTeompObject);
                    break;
                case (int)Weapon_Type.Arrow:
                    DicWeaponParent.TryGetValue(Enum.GetName(typeof(WeaponEquip_Type), WeaponEquip_Type.Weapon_Arrow), out EquipTeompObject);
                    TempList.Add(EquipTeompObject);
                    break;
                case (int)Weapon_Type.Spear:
                    DicWeaponParent.TryGetValue(Enum.GetName(typeof(WeaponEquip_Type), WeaponEquip_Type.Weapon_Spear_Sheath), out EquipTeompObject);
                    TempList.Add(EquipTeompObject);
                    DicWeaponParent.TryGetValue(Enum.GetName(typeof(WeaponEquip_Type), WeaponEquip_Type.Weapon_Spear), out EquipTeompObject);
                    TempList.Add(EquipTeompObject);
                    break;
                case (int)Weapon_Type.Sword:
                    DicWeaponParent.TryGetValue(Enum.GetName(typeof(WeaponEquip_Type), WeaponEquip_Type.Weapon_Sword_Sheath), out EquipTeompObject);
                    TempList.Add(EquipTeompObject);
                    DicWeaponParent.TryGetValue(Enum.GetName(typeof(WeaponEquip_Type), WeaponEquip_Type.Weapon_Sword), out EquipTeompObject);
                    TempList.Add(EquipTeompObject);
                    break;
                case (int)Weapon_Type.Quiver:
                    DicWeaponParent.TryGetValue(Enum.GetName(typeof(WeaponEquip_Type), WeaponEquip_Type.Weapon_Bow_Quiver), out EquipTeompObject);
                    TempList.Add(EquipTeompObject);
                    break;
            }

            My_WeaponParent.Add(i, TempList);
        }


        DicWeaponParent.Clear();
    }

    private void MyWeapon_Init(string _strWeaponFileName, Weapon_Type _eWeaponType)
    {
        GameObject[] WeaponObjects = GameManager.Resources.LoadAll<GameObject>($"Prefabs/Item/{_strWeaponFileName}");

        if (null == WeaponObjects)
            return;

        for (int i = 0; i < WeaponObjects.Length; ++i)
        {
            List<Equip_Weapon> OutParentList;
            List<GameObject> WeaponObjectList = new List<GameObject>();

            if (false == My_WeaponParent.TryGetValue((int)_eWeaponType, out OutParentList))
                return;

            for (int j = 0; j < OutParentList.Count; ++j)
            {
                GameObject WeaponObject = GameManager.Resources.CreatePrefab($"Item/{Enum.GetName(typeof(Weapon_Type), _eWeaponType)}"
                    + $"/{WeaponObjects[i].name}", OutParentList[j].transform);

                DestroyComponent<Item_Prefab>(WeaponObject);
                EnableComponent<SphereCollider>(WeaponObject);

                if (gameObject.CompareTag("InvenPlayer"))
                {
                    DestroyComponent<Weapon>(WeaponObject);
                    DestroyComponent<Arrow>(WeaponObject);
                    DestroyComponent<Rigidbody>(WeaponObject);
                }


                if (gameObject.CompareTag("Player"))
                {
                    if (OutParentList[j].transform.parent.CompareTag("PlayerWeaponPot"))
                    {
                        PlayerManager.weaponManager.SetPodWeapons(WeaponObject);
                        Remove_ComponentEx(WeaponObject, _eWeaponType);
                    }
                    else
                        Add_ComponentEx(WeaponObject, _eWeaponType);
                }

                WeaponObject.SetActive(false);
                WeaponObjectList.Add(WeaponObject);
            }

            My_Weapons.Add($"{WeaponObjects[i].name}", WeaponObjectList);
        }
    }

    private void Add_ComponentEx(GameObject _WeaponObject, Weapon_Type _eWeaponType)
    {
        switch (_eWeaponType)
        {
            case Weapon_Type.Bow:
                _WeaponObject.GetOrAddComponent<Bow>();
                break;
            case Weapon_Type.Shield:
                _WeaponObject.GetOrAddComponent<Shield>();
                break;
            case Weapon_Type.Spear:
                _WeaponObject.GetOrAddComponent<WeaponSpear>();
                break;
            case Weapon_Type.Sword:
                _WeaponObject.GetOrAddComponent<WeaponSword>();
                break;
        }
    }


    private void Remove_ComponentEx(GameObject _WeaponObject, Weapon_Type _eWeaponType)
    {
        switch (_eWeaponType)
        {
            case Weapon_Type.Bow:
                Bow BowCompoent = null;
                BoxCollider BoxColider = null;
                Rigidbody Rigid = null;

                if (_WeaponObject.TryGetComponent(out BowCompoent))
                    Destroy(BowCompoent);

                if (_WeaponObject.TryGetComponent(out BoxColider))
                    Destroy(BoxColider);

                if (_WeaponObject.TryGetComponent(out Rigid))
                    Destroy(Rigid);
                break;
            case Weapon_Type.Shield:
                Shield ShieldCompoent = null;
                if (_WeaponObject.TryGetComponent(out ShieldCompoent))
                    Destroy(ShieldCompoent);
                break;
            case Weapon_Type.Spear:
                WeaponSpear SpearCompoent = null;
                if (_WeaponObject.TryGetComponent(out SpearCompoent))
                    Destroy(SpearCompoent);
                break;
            case Weapon_Type.Sword:
                WeaponSword SwordCompoent = null;
                if (_WeaponObject.TryGetComponent(out SwordCompoent))
                    Destroy(SwordCompoent);
                break;
        }
    }

    private void All_Weapon_Init()
    {
        MyWeapon_Init("Bow", Weapon_Type.Bow);
        MyWeapon_Init("Shield", Weapon_Type.Shield);
        MyWeapon_Init("Arrow", Weapon_Type.Arrow);
        MyWeapon_Init("Spear", Weapon_Type.Spear);
        MyWeapon_Init("Sword", Weapon_Type.Sword);
        MyWeapon_Init("Quiver", Weapon_Type.Quiver);
    }

    private void DestroyComponent<T>(GameObject _Object) where T : Component
    {
        T TempComponent = _Object.GetComponent<T>();

        if (null != TempComponent)
            Destroy(TempComponent);
    }


    private void EnableComponent<T>(GameObject _Object) where T : Component
    {
        T[] TempComponent = _Object.GetComponents<T>();

        if (null != TempComponent)
        {
            foreach (var iter in TempComponent)
            {
                if (iter is SphereCollider SphereColider)
                    SphereColider.enabled = false;
                else if (iter is Rigidbody rigidbody)
                    rigidbody.isKinematic = true;
            }
        }
    }


    private void DefaultPartzChange(int _iType, bool _bActive)
    {
        if (false == _bActive && true == Default_Meshes[_iType].activeInHierarchy)
            Default_Meshes[_iType].SetActive(_bActive);
        else if (true == _bActive && false == Default_Meshes[_iType].activeInHierarchy)
            Default_Meshes[_iType].SetActive(_bActive);

        if ((int)EquipType.EquipType_Body == _iType && true == Default_Meshes[_iType].activeInHierarchy)
            DefaultBeltActive(false);
        else if ((int)EquipType.EquipType_Body == _iType && false == Default_Meshes[_iType].activeInHierarchy)
            DefaultBeltActive(true);
    }

    private void DefaultBeltActive(bool _bActive)
    {
        List<SkinnedMeshRenderer> DefaultMeshes;
        My_Meshes.TryGetValue((int)EquipType.EquipType_Body, out DefaultMeshes);

        if (null != DefaultMeshes)
            DefaultMeshes[0].gameObject.SetActive(_bActive);
    }

    private void DefaultPartzInit(string _strPartzName, int _iType)
    {
        MyMeshesActive(_iType, false);

        GameObject PartzObject;

        Partz_Object.TryGetValue(_strPartzName, out PartzObject);

        Default_Meshes.Add(PartzObject);

        AddEquipment_Armor(_strPartzName, _iType);
    }

    private void MyMeshesActive(int _iType, bool _bIsActive)
    {
        List<SkinnedMeshRenderer> MySkinRenderer;
        My_Meshes.TryGetValue(_iType, out MySkinRenderer);

        if (null == MySkinRenderer)
            return;

        foreach (SkinnedMeshRenderer iter in MySkinRenderer)
            iter.gameObject.SetActive(_bIsActive);
    }

    private void PartzMeshesActive(bool _bIsActive, int _iType, string _strPartzName = null)
    {
        GameObject PartzObject;

        if (null != _strPartzName)
            Partz_Object.TryGetValue(_strPartzName, out PartzObject);
        else
        {
            Partz_Object.TryGetValue(strCurrentPartzName[_iType], out PartzObject);
        }

        switch (_iType)
        {
            case (int)EquipType.EquipType_Hair:
                strCurrentPartzName[(int)EquipType.EquipType_Hair] = _strPartzName;
                break;
            case (int)EquipType.EquipType_Body:
                strCurrentPartzName[(int)EquipType.EquipType_Body] = _strPartzName;
                break;
            case (int)EquipType.EquipType_Lower:
                strCurrentPartzName[(int)EquipType.EquipType_Lower] = _strPartzName;
                break;
        }

        PartzObject?.SetActive(_bIsActive);

        PlayerArmorOrRemoveAdd(_iType, PartzObject, _bIsActive);
    }

    private void PlayerArmorOrRemoveAdd(int _iType, GameObject _PartzObject, bool _bIsActive)
    {
        if (gameObject.CompareTag("Player"))
        {
            if (0 != (iFirstInitBitMask & (1 << _iType)))
            {
                iFirstInitBitMask &= (iFirstInitBitMask << _iType);
                return;
            }

            if (true == _bIsActive)
                PlayerManager.playerArmors.Add((PlayerArmorValues)_iType, _PartzObject);
            else
                PlayerManager.playerArmors.Remove((PlayerArmorValues)_iType);
        }
    }

    private void DefaultWeponsActive(bool _bIsActive, int _iType)
    {
        List<GameObject> DefaultWeaponObject = null;

        switch (_iType)
        {
            case (int)Weapon_Type.Bow:
                My_Weapons.TryGetValue("Quiver", out DefaultWeaponObject);

                if (gameObject.CompareTag("Player"))
                {
                    if (true == _bIsActive)
                        PlayerManager.weaponManager.SetDeafultWeapon(DefaultWeaponObject[0], Get_WeaponType(_iType));
                }
                else
                    DefaultWeaponObject[0].SetActive(_bIsActive);
                break;
            case (int)Weapon_Type.Sword:
                My_Weapons.TryGetValue("Shield022", out DefaultWeaponObject);

                if (gameObject.CompareTag("Player"))
                {
                    if (true == _bIsActive)
                        PlayerManager.weaponManager.SetDeafultWeapon(DefaultWeaponObject[1], Get_WeaponType(_iType));
                }

                DefaultWeaponObject[1].SetActive(_bIsActive);
                break;
        }
    }

    private void WeaponsActive(bool _bIsActive, int _iType, string _strWeaponName = null)
    {
        List<GameObject> WeaponObjects;

        if (null != _strWeaponName)
        {
            My_Weapons.TryGetValue(_strWeaponName, out WeaponObjects);
            DefaultWeponsActive(true, _iType);
        }
        else
        {
            My_Weapons.TryGetValue(strCurrentWeaponName[_iType], out WeaponObjects);
            DefaultWeponsActive(false, _iType);
        }

        switch (_iType)
        {
            case (int)Weapon_Type.Bow:
                strCurrentWeaponName[(int)Weapon_Type.Bow] = _strWeaponName;
                break;
            case (int)Weapon_Type.Shield:
                strCurrentWeaponName[(int)Weapon_Type.Shield] = _strWeaponName;
                break;
            case (int)Weapon_Type.Arrow:
                strCurrentWeaponName[(int)Weapon_Type.Arrow] = _strWeaponName;
                break;
            case (int)Weapon_Type.Spear:
                strCurrentWeaponName[(int)Weapon_Type.Spear] = _strWeaponName;
                break;
            case (int)Weapon_Type.Sword:
                strCurrentWeaponName[(int)Weapon_Type.Sword] = _strWeaponName;
                break;
            case (int)Weapon_Type.Quiver:
                strCurrentWeaponName[(int)Weapon_Type.Quiver] = _strWeaponName;
                break;
        }

        WeaponObjects[1]?.SetActive(_bIsActive);
    }

    private void WeaponsActive(bool _bIsActive, ItemEquipData _EquipData, string _strWeaponName = null)
    {
        List<GameObject> WeaponObjects;

        if (null != _strWeaponName)
        {
            My_Weapons.TryGetValue(_strWeaponName, out WeaponObjects);
            DefaultWeponsActive(true, _EquipData.iType);
        }
        else
        {
            My_Weapons.TryGetValue(strCurrentWeaponName[_EquipData.iType], out WeaponObjects);
            DefaultWeponsActive(false, _EquipData.iType);
        }

        switch (_EquipData.iType)
        {
            case (int)Weapon_Type.Bow:
                strCurrentWeaponName[(int)Weapon_Type.Bow] = _strWeaponName;
                break;
            case (int)Weapon_Type.Shield:
                strCurrentWeaponName[(int)Weapon_Type.Shield] = _strWeaponName;
                break;
            case (int)Weapon_Type.Arrow:
                strCurrentWeaponName[(int)Weapon_Type.Arrow] = _strWeaponName;
                break;
            case (int)Weapon_Type.Spear:
                strCurrentWeaponName[(int)Weapon_Type.Spear] = _strWeaponName;
                break;
            case (int)Weapon_Type.Sword:
                strCurrentWeaponName[(int)Weapon_Type.Sword] = _strWeaponName;
                break;
            case (int)Weapon_Type.Quiver:
                strCurrentWeaponName[(int)Weapon_Type.Quiver] = _strWeaponName;
                break;
        }


        if (gameObject.CompareTag("Player"))
        {
            if (true == _bIsActive)
                PlayerManager.weaponManager.SetCurrentWeapon(WeaponObjects[1], Get_WeaponType(_EquipData.iType), _EquipData);
            else
                PlayerManager.weaponManager.SetRemoveWeapon(Get_WeaponType(_EquipData.iType));
        }
    }

    private void EquipWeaponItem(Item _Item)
    {
        if (_Item is Item_Weapon _WeaponItem)
        {
            List<GameObject> WeaponObjects;

            if (true == _WeaponItem.IsEquip)
            {
                if (null != CurInvenWeapon.Weapon_Object && true == CurInvenWeapon.Weapon_Object.activeSelf)
                {
                    DefaultWeponsActive(false, CurInvenWeapon.My_Data.iType);
                    CurInvenWeapon.Weapon_Object.SetActive(false);
                    CurInvenWeapon.Weapon_Object = null;
                    CurInvenWeapon.My_Data = null;
                }

                AnimChange(_WeaponItem.My_EquipData.iType);
                My_Weapons.TryGetValue(_WeaponItem.My_Data.strId, out WeaponObjects);
                DefaultWeponsActive(true, _WeaponItem.My_EquipData.iType);
                WeaponObjects[1]?.SetActive(_WeaponItem.IsEquip);
                CurInvenWeapon.Weapon_Object = WeaponObjects[1];
                CurInvenWeapon.My_Data = _WeaponItem.My_EquipData;
            }
        }
    }

    private void AnimChange(int _iIndex)
    {
        if (null == My_InvenAnimtor)
            return;

        switch (_iIndex)
        {
            case 0:
                My_InvenAnimtor.SetTrigger(iBow);
                My_InvenAnimtor.ResetTrigger(iSword);
                My_InvenAnimtor.ResetTrigger(iNormal);
                break;
            case 3:
                My_InvenAnimtor.SetTrigger(iSpear);
                My_InvenAnimtor.ResetTrigger(iSword);
                My_InvenAnimtor.ResetTrigger(iNormal);
                break;
            case 4:
                My_InvenAnimtor.SetTrigger(iSword);
                My_InvenAnimtor.ResetTrigger(iBow);
                My_InvenAnimtor.ResetTrigger(iNormal);
                break;
            default:
                My_InvenAnimtor.SetTrigger(iNormal);
                My_InvenAnimtor.ResetTrigger(iSword);
                My_InvenAnimtor.ResetTrigger(iBow);
                break;
        }


    }

    private void UnEquipWeaponItem(Item _Item)
    {
        List<GameObject> WeaponObjects = null;

        if (_Item is Item_Weapon _WeaponItem)
        {
            My_Weapons.TryGetValue(_WeaponItem.My_Data.strId, out WeaponObjects);

            if (null != WeaponObjects[1] && true == WeaponObjects[1].activeSelf)
            {
                DefaultWeponsActive(false, _WeaponItem.My_EquipData.iType);
                WeaponObjects[1]?.SetActive(_WeaponItem.IsEquip);
                AnimChange(-1);
            }
        }
        else
        {
            if (null != CurInvenWeapon.Weapon_Object && true == CurInvenWeapon.Weapon_Object.activeSelf)
            {
                DefaultWeponsActive(false, CurInvenWeapon.My_Data.iType);
                CurInvenWeapon.Weapon_Object.SetActive(false);
                CurInvenWeapon.Weapon_Object = null;
                CurInvenWeapon.My_Data = null;
                AnimChange(-1);
            }
        }
    }


    private WeaponEquip_Type Get_WeaponType(int _iType)
    {
        return (WeaponEquip_Type)_iType;
    }

    public void AddEquipment_Armor(string _strPartzName, int _iType)
    {
        if (string.Empty == _strPartzName)
            return;

        DefaultPartzChange(_iType, false);

        PartzMeshesActive(true, _iType, _strPartzName);
    }

    public void RemoveEquipment_Armor(int _iType)
    {
        DefaultPartzChange(_iType, true);

        PartzMeshesActive(false, _iType);
    }

    public void AddEquipment_Weapon(string _strPartzName, int _iType)
    {
        if (string.Empty == _strPartzName)
            return;

        WeaponsActive(true, _iType, _strPartzName);
    }

    public void AddEquipment_Weapon(ItemEquipData _EquipData)
    {
        if (null == _EquipData || gameObject.CompareTag("InvenPlayer"))
            return;

        WeaponsActive(true, _EquipData, _EquipData.strId);
    }

    public void RemoveEquipment_Weapon(ItemEquipData _EquipData)
    {
        if (null == _EquipData || gameObject.CompareTag("InvenPlayer"))
            return;

        WeaponsActive(false, _EquipData);
    }


    public void RemoveEquipment_Weapon(int _iType)
    {
        WeaponsActive(false, _iType);
    }

    private Transform FindChildByName(string ThisName, Transform ThisGObj)
    {
        Transform ReturnObj;

        if (ThisGObj.name == ThisName)
            return ThisGObj.transform;

        foreach (Transform child in ThisGObj)
        {
            ReturnObj = FindChildByName(ThisName, child);

            if (ReturnObj != null)
                return ReturnObj;
        }

        return null;
    }


    private void Start()
    {
        newInit();
        MyMeshInit();
        PartzBindInit();
        Weapon_ParentInit();
        All_Weapon_Init();

        DefaultPartzInit("Armor_Default_Head", (int)EquipType.EquipType_Hair);
        DefaultPartzInit("Armor_116_Upper", (int)EquipType.EquipType_Body);
        DefaultPartzInit("Armor_043_Lower", (int)EquipType.EquipType_Lower);
        DefaultBeltActive(true);
    }
}
