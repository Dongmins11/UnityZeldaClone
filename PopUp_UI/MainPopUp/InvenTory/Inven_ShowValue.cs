using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inven_ShowValue : UI_Base
{
    enum Texts
    {
        Attack_Text,
        Armor_Text,
    }

    enum Values
    {
        Attack,
        Armor,
        Shield,
    }

    string strDeafultValue = "0";

    int[] iArrayValues;

    public override void init()
    {
        Bind<Text>(typeof(Texts));

        iArrayValues = new int[(int)Values.Shield + 1];

        ShowDefaultValueTexts();
    }

    public void ShowAttackValueTexts(int _iAttackValue)
    {
        iArrayValues[(int)Values.Attack] += _iAttackValue;

        GetText((int)Texts.Attack_Text).text = $"{iArrayValues[(int)Values.Attack]}";
    }

    public void ShowArmorValueTexts(int _iArmorValue)
    {
        iArrayValues[(int)Values.Armor] += _iArmorValue;

        GetText((int)Texts.Armor_Text).text = $"{iArrayValues[(int)Values.Armor]}";
    }

    public void ShowShieldValueTexts(int _iShieldValue)
    {
        int ArmorValue = 0;

        iArrayValues[(int)Values.Shield] += _iShieldValue;
        ArmorValue = iArrayValues[(int)Values.Armor] + iArrayValues[(int)Values.Shield];

        GetText((int)Texts.Armor_Text).text = $"{ArmorValue}";
    }



    public void HideAttackValueTexts(int _iAttackValue)
    {
        iArrayValues[(int)Values.Attack] = Mathf.Max(0, iArrayValues[(int)Values.Attack] -= _iAttackValue);

        GetText((int)Texts.Attack_Text).text = $"{iArrayValues[(int)Values.Attack]}";
    }

    public void HideArmorValueTexts(int _iArmorValue)
    {
        iArrayValues[(int)Values.Armor] = Mathf.Max(0, iArrayValues[(int)Values.Armor] -= _iArmorValue);

        GetText((int)Texts.Armor_Text).text = $"{iArrayValues[(int)Values.Armor]}";
    }

    public void HideShieldValueTexts(int _iShieldValue)
    {
        int ArmorValue = 0;

        iArrayValues[(int)Values.Shield] = Mathf.Max(0, iArrayValues[(int)Values.Shield] -= _iShieldValue);
        ArmorValue  = Mathf.Max(0, iArrayValues[(int)Values.Armor] - iArrayValues[(int)Values.Shield]);

        GetText((int)Texts.Armor_Text).text = $"{ArmorValue}";
    }


    public void ShowDefaultValueTexts()
    {
        GetText((int)Texts.Attack_Text).text = strDeafultValue;
        GetText((int)Texts.Armor_Text).text = strDeafultValue;
    }

}
