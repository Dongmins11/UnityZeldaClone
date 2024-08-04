using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inven_Slot : UI_Base
{
    enum Texts
    {
        Count_Text,
        Value_Text,
    }

    enum Images
    {
        HighLight_Icon,
        Item_Icon,
        Item_BackGround,
        Value_Icon,
    }

    Color HideColor = new Color(1f, 1f, 1f, 0f);
    Color ShowColor = new Color(1f, 1f, 1f, 1f);

    public string strItem_ToolTip { get; private set; } = string.Empty;
    public string strItem_TitleToolTip { get; private set; } = string.Empty;

    string strCountableItme_Count = string.Empty;


    public override void init()
    {
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        All_Hide_Icon();
    }

    public void All_Hide_Icon()
    {
        Hide_CountText_Icon();
        Hide_Value_Icon();

        Hide_Item_Icon();
    }

    public void Hide_Item_Icon()
    {
        strItem_ToolTip = string.Empty;
        strItem_TitleToolTip = string.Empty;


        GetImage((int)Images.Item_Icon).sprite = null;
        GetImage((int)Images.Item_Icon).color = HideColor;

        GetImage((int)Images.Value_Icon).gameObject.SetActive(false);
        GetText((int)Texts.Count_Text).gameObject.SetActive(false);

        GetImage((int)Images.Item_BackGround).gameObject.SetActive(false);
        GetImage((int)Images.HighLight_Icon).gameObject.SetActive(false);
    }

    public void Hide_CountText_Icon()
    {
        GetText((int)Texts.Count_Text).gameObject.SetActive(false);
        strCountableItme_Count = string.Empty;
    }

    public void Hide_Value_Icon()
    {
        GetImage((int)Images.Value_Icon).gameObject.SetActive(false);
        GetText((int)Texts.Value_Text).text = string.Empty;
    }

    public void Hide_HightLight_Icon()
    {
        GetImage((int)Images.HighLight_Icon).gameObject.SetActive(false);
    }

    public void Show_Equip_Slot()
    {
        GetImage((int)Images.HighLight_Icon).gameObject.SetActive(true);
        GetImage((int)Images.Item_BackGround).color = new Color(0f, 0f, 0f, 0f);
    }

    public void Hide_Equip_Slot()
    {
        GetImage((int)Images.HighLight_Icon).gameObject.SetActive(false);
        GetImage((int)Images.Item_BackGround).color = new Color(0f, 0f, 0f, 1f);
    }



    public void Show_Item_Icon(ItemData _Data)
    {
        strItem_ToolTip = _Data.strToolTip;
        strItem_TitleToolTip = _Data.strname;

        GetImage((int)Images.Item_Icon).sprite = GameManager.Resources.Get_ItemSprite(_Data.strTexturePath);
        GetImage((int)Images.Item_Icon).color = ShowColor;
        GetImage((int)Images.Item_BackGround).gameObject.SetActive(true);
    }

    public void Show_CountText_Icon(int _iCount)
    {
        GetText((int)Texts.Count_Text).gameObject.SetActive(true);
        GetText((int)Texts.Count_Text).text = string.Concat("X" , _iCount);
    }

    public void Show_Value_Icon(ItemData _Data)
    {
        ItemEquipData EquipData = _Data as ItemEquipData;

        GetImage((int)Images.Value_Icon).gameObject.SetActive(true);
        GetText((int)Texts.Value_Text).text = $"{EquipData.iValue}";
    }

}
