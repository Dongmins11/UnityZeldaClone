using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Shop_Item : MonoBehaviour, InteractiveObject
{
    [SerializeField]
    private TextAsset My_Ink;

    ItemData My_ItemData;
    GameObject My_Meshes;

    bool bisSell = false;
    bool bIsInterActionCheck = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && false == bisSell)
        {
            GameManager.UIContents.Show_Shop_Information(My_ItemData);
            GameManager.UIContents.Show_Interaction_UI(UI_Scene_Interaction.Interaction_Type.Shop_E);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F4) && other.CompareTag("Player"))
            GameManager.item.Inven.Add_Rupee(500);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && false == bisSell)
        {
            bIsInterActionCheck = false;
            GameManager.UIContents.Hide_Shop_Information();
            GameManager.UIContents.Hide_Interaction_UI();
        }
    }
    void Start()
    {
        My_ItemData = GameManager.item.Get_ItemData(gameObject.name);
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
        bIsInterActionCheck = false;
        My_Meshes = Util.FindChild(gameObject, "Armor");
    }

    public void OnPlayerInteracting(PlayerAnimator _playerAnimator, InteractiveObject _interactObject)
    {
        if (!(_interactObject.Equals(this))) return;

        if (true == bisSell)
            return;

        bool bCheck = false;

        //완료되기전
        if (false == bisSell)
        {
            bCheck = GameManager.Dialogue.Show_ShopTalkText(My_Ink, My_ItemData, DialogueManager.Talk_Type.Normal_Talk);

            if(false == bIsInterActionCheck)
            {
                PlayerManager.input.bCanInput = false;
                GameManager.UIContents.Show_Select_Interactio_UI();
                bIsInterActionCheck = true;
            }
        }

        //완료되었을때
        if (true == bCheck)
        {
            GameManager.UIContents.Hide_Shop_Information();
            GameManager.UIContents.Show_BoxOpenInfomation(My_ItemData);
            GameManager.item.Inven.Remove_Rupee(My_ItemData);
            GameManager.item.Inven.Add(My_ItemData);

            PlayerManager.input.bCanInput = true;
            My_Meshes.SetActive(false);
            bIsInterActionCheck = false;
            bisSell = true;
        }
    }
}
