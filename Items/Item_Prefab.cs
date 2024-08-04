using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

[RequireComponent(typeof(SphereCollider))]
public class Item_Prefab : MonoBehaviour, InteractiveObject
{
    enum ItemType
    {
        ItemType_Weapons,
        ItemType_Bows,
        ItemType_ItemShields,
        ItemType_Armors,
        ItemType_Foods,
        ItemType_Gitas,
        ItemType_Rupee,
    }

    ItemData My_ItemData;
    UI_World_Item_Title My_UI;
    PlayerManager playerManager;
    Weapon MyWeaponObj;
    SphereCollider My_Collider;

    private void OnTriggerEnter(Collider other)
    {
        if (null != MyWeaponObj && WPNOwner.None != MyWeaponObj.owner)
            return;

        if (other.CompareTag("Player"))
        {
            My_UI.Show_Title_UI(My_ItemData);
            GameManager.UIContents.Show_Interaction_UI(UI_Scene_Interaction.Interaction_Type.Item_E);
        }
    }

    private void OnDisable()
    {
        Setup();
    }

    private void OnTriggerExit(Collider other)
    {
        if (null != MyWeaponObj && WPNOwner.None != MyWeaponObj.owner)
            return;

        if (other.CompareTag("Player"))
        {
            GameManager.UIContents.Hide_Interaction_UI();
            My_UI.Hide_Title_UI();
        }
    }

    public void OnPlayerInteracting(PlayerAnimator _playerAnimator, InteractiveObject _interactObject)
    {
        if (null != MyWeaponObj && WPNOwner.None != MyWeaponObj.owner)
            return;

        if (_interactObject != this)
            return;

        if(null == playerManager)
        {
            GameObject TempPlayer = GameObject.FindGameObjectWithTag("Player");

            if (null != TempPlayer)
                playerManager = TempPlayer.GetComponent<PlayerManager>();
        }

        MasterAudio.PlaySound("ItemGet");

        AddItem();

        GameManager.UIContents.Show_Select_Interactio_UI();

        playerManager.iInteractive = null;

        Destroy(gameObject);
    }

    private void AddItem()
    {
        if ((int)ItemType.ItemType_Rupee == My_ItemData.iInvenType)
        {
            GameManager.item.Inven.Add_Rupee(My_ItemData);
            MasterAudio.PlaySound("RUPEEDOWN");
        }
        else
            GameManager.item.Inven.Add(My_ItemData);
    }

    private void UI_Init()
    {
        GameObject Itme_Title_UI = GameManager.Resources.CreatePrefab("UI/SubItem/World_UI_ItemTitle", transform);

        My_UI = Util.GetOrAddComponent<UI_World_Item_Title>(Itme_Title_UI);

        if(null == My_UI)
        {
            //Debug.Log("Failed to Item_Prefabs Funtion UI_Init");
            return;
        }

        My_UI.init();
    }

    private void Start()
    {
        My_ItemData = GameManager.item.Get_ItemData(gameObject.name);

        GameObject TempPlayer = GameObject.FindGameObjectWithTag("Player");

        if (null != TempPlayer)
            playerManager = TempPlayer.GetComponent<PlayerManager>();

        UI_Init();
        StartCoroutine(LateStart());
    }

    private void Setup()
    {
        if (null == MyWeaponObj)
            TryGetComponent(out MyWeaponObj);
    }

    private void SetupCollider()
    {
        My_Collider = gameObject.GetComponent<SphereCollider>();

        if (null == My_Collider)
            return;

        switch (My_ItemData.iInvenType)
        {
            case (int)ItemType.ItemType_Bows:
            case (int)ItemType.ItemType_Armors:
            case (int)ItemType.ItemType_ItemShields:
            case (int)ItemType.ItemType_Weapons:
                My_Collider.radius = 1.5f;
                My_Collider.center = new Vector3(0f,0.5f,0f);
                break;
            case (int)ItemType.ItemType_Foods:
            case (int)ItemType.ItemType_Gitas:
                My_Collider.radius = 0.8f;
                break;
        }

        My_Collider.isTrigger = true;
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();

        Setup();
        SetupCollider();
    }


}
