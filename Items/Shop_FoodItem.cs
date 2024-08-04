using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class Shop_FoodItem : MonoBehaviour, InteractiveObject
{
    [SerializeField]
    private TextAsset My_Ink;

    ItemData My_ItemData;

    bool bisSell = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && false == bisSell)
        {
            GameManager.UIContents.Show_Shop_Information(My_ItemData);
            GameManager.UIContents.Show_Interaction_UI(UI_Scene_Interaction.Interaction_Type.Shop_E);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (false == bisSell)
            {
                GameManager.UIContents.Hide_Shop_Information();
                GameManager.UIContents.Hide_Interaction_UI();
            }
            else
                bisSell = false;
        }
    }
    void Start()
    {
        My_ItemData = GameManager.item.Get_ItemData(gameObject.name);
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }

    public void OnPlayerInteracting(PlayerAnimator _playerAnimator, InteractiveObject _interactObject)
    {
        if (!(_interactObject.Equals(this))) return;

        if (true == bisSell)
            return;

        bool bCheck = false;

        if (false == bisSell)
        {
            PlayerManager.input.bCanInput = false;
            bCheck = GameManager.Dialogue.Show_ShopTalkText(My_Ink, My_ItemData, DialogueManager.Talk_Type.FoodShop_Talk);
            GameManager.UIContents.Show_Select_Interactio_UI();
        }

        if (true == bCheck)
        {

            GameManager.UIContents.Hide_Shop_Information();
            GameManager.UIContents.Show_BoxOpenInfomation(My_ItemData);
            GameManager.item.Inven.Remove_Rupee(My_ItemData);
            GameManager.item.Inven.Add(My_ItemData, GameManager.Dialogue.Get_FoodCount());
            PlayerManager.input.bCanInput = true;
            bisSell = true;
        }


    }
}
