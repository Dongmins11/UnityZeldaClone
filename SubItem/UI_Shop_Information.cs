using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class UI_Shop_Information : UI_Base
{
    enum GameObjects
    {
        Shop_Canvas,
        Shop_Infomation,
    }

    enum Texts
    {
        Inven_SlotCheck_Text,
        Rupee_Text,
        Itme_TitleText,
        Itme_InfoText,
    }

    enum Images
    {
        Rupee,
        ItemImage,
    }

    int AnimTriggerHash = Animator.StringToHash("Shop_Info_Trigger");

    Animator Shop_InfoAnim;


    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        ComponentInit();
    }

    private void ComponentInit()
    {
        Shop_InfoAnim = GetObject((int)GameObjects.Shop_Infomation).GetComponent<Animator>();
        
    }

    private void Show_Images(ItemData _ItemData)
    {
        MasterAudio.PlaySound("ShopOpen");
        GetImage((int)Images.ItemImage).sprite = GameManager.Resources.Get_ItemSprite(_ItemData.strTexturePath);
        
        int iPrice = 0;

        if (_ItemData is ItemFoodData FoodData)
        {
            iPrice = FoodData.iPrice;
            GetText((int)Texts.Rupee_Text).text = $"{FoodData.iPrice}";
        }
        else if(_ItemData is ItemArmorData ArmorData)
        {
            iPrice = ArmorData.iPrice;
            GetText((int)Texts.Rupee_Text).text = $"{ArmorData.iPrice}";
        }

        if (true == GameManager.item.Inven.Price_Rupee(iPrice))
        {
            GetImage((int)Images.Rupee).color = Color.green;
            GetText((int)Texts.Rupee_Text).color = Color.white;
        }
        else
        {
            GetImage((int)Images.Rupee).color = Color.red;
            GetText((int)Texts.Rupee_Text).color = Color.red;
        }
    }
    private void Hide_Images()
    {
        
        GetImage((int)Images.ItemImage).sprite = null;
        GetImage((int)Images.Rupee).color = Color.white;

        GetText((int)Texts.Rupee_Text).text = "0";
        GetText((int)Texts.Rupee_Text).color = Color.white;
    }
    private void Show_Texts(ItemData _ItemData)
    {
        int iSlotIndex = GameManager.item.Inven.Get_CountableAmount(_ItemData);

        Shop_InfoAnim.SetTrigger(AnimTriggerHash);

        GetText((int)Texts.Inven_SlotCheck_Text).text = $"x{iSlotIndex}";

        GetText((int)Texts.Itme_TitleText).text = _ItemData.strname;

        GetText((int)Texts.Itme_InfoText).DOText("", 0.0f);
        GetText((int)Texts.Itme_InfoText).DOText(_ItemData.strToolTip, 1.0f);
        
    }
    private void Hide_Texts()
    {
        GetText((int)Texts.Itme_InfoText).DOText("", 0.0f);
        GetText((int)Texts.Inven_SlotCheck_Text).text = "0";
        GetText((int)Texts.Itme_TitleText).text = "";
        
    }    

    public void Show_Shop_Information(ItemData _ItemData)
    {
        gameObject.SetActive(true);

        Show_Images(_ItemData);
        Show_Texts(_ItemData);
        
    }

    public void Hide_Shop_Information()
    {
        gameObject.SetActive(false);

        Hide_Images();
        Hide_Texts();

        MasterAudio.PlaySound("Cancel");
    }


}
