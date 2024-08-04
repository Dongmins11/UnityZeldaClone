using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inven_Title_Icon : UI_Base
{
    enum Inven_Titles
    {
        WeaponIcon,
        BowIcon,
        ArmorIcon,
        ClothIcon,
        FoodIcon,
        GitaIcon,
    }

    enum Inven_TitleText
    {
        WeaponText,
        BowText,
        ArmorText,
        ClothText,
        FoodText,
        GitalText,
    }

    Color Change_Color;
    Color NonChagne_Color;

    int iPreIndex = 0;
    int iCurIndex = 0;

    public override void init()
    {
        Bind<Image>(typeof(Inven_Titles));
        Bind<Text>(typeof(Inven_TitleText));

        Change_Color = new Color(1f, 1f, 1f, 1.0f);

        NonChagne_Color = new Color(1f, 1f, 1f, 0.2f);

        for (int i = 0; i <= (int)Inven_TitleText.GitalText; ++i)
            GetText(i).gameObject.SetActive(false);

        for (int i = 0; i <= (int)Inven_Titles.GitaIcon; ++i)
            GetImage(i).color = NonChagne_Color;

        ChangeIcon(iCurIndex);
    }

    public void ChangeIcon(int _iIndex)
    {
        iPreIndex = iCurIndex;
        iCurIndex = _iIndex;

        GetText(iPreIndex).gameObject.SetActive(false);
        GetText(iCurIndex).gameObject.SetActive(true);

        GetImage(iPreIndex).color = NonChagne_Color;
        GetImage(iCurIndex).color = Change_Color;
    }


}
