using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inven_ScrollView : UI_Base
{
    enum Inven_Panels
    {
        Weapon_Panel,
        Bow_Panel,
        Shlied_Panel,
        Armor_Panel,
        Food_Panel,
        Gita_Panel,
        End_Panel,
    }
    enum Contents
    {
        Content,
    }

    enum ScrollBars
    {
        ScrollbarHorizontal,
    }

    public Inven_Panel Panel { get { return Cur_Panel; } }
    public int PanelIndex { get { return iPanelCurIndex; } set { iPanelCurIndex = value; } }
    public int MaxPanelIndex { get { return (int)Inven_Panels.End_Panel - 1; } }


    Inven_Panel[] Array_Panel;

    Inven_Panel Cur_Panel;

    int iPanelCurIndex = 0;

    float fScrollValue = 0.0f;

    public override void init()
    {
        Bind<Scrollbar>(typeof(ScrollBars));

        Bind<GameObject>(typeof(Contents));

        Array_Panel = new Inven_Panel[(int)Inven_Panels.End_Panel];

        InvenCountInit(typeof(Inven_Panels));
    }

    public void Reset_Scrollbar()
    {
        iPanelCurIndex = 0;
        Cur_Panel = Array_Panel[iPanelCurIndex];
        Get<Scrollbar>((int)ScrollBars.ScrollbarHorizontal).value = 0f;
    }


    private void InvenCountInit(Type _type)
    {
        string[] names = Enum.GetNames(_type);

        for (int i = 0; i < names.Length - 1; ++i)
        {
            Array_Panel[i] = GameManager.UI.MakeSubItem<Inven_Panel>(Get<GameObject>((int)Contents.Content).transform, "Panel");
            Array_Panel[i].gameObject.name = names[i];
            Array_Panel[i].init();
        }

        Cur_Panel = Array_Panel[0];
    }

    public Inven_Panel Get_Panel(int _iType)
    {
        if (0 <= _iType && _iType < (int)Inven_Panels.End_Panel)
            return Array_Panel[_iType];
        else
            return null;
    }

    public void MoveTo_Panel(bool _IsFront)
    {
        if (_IsFront)
            iPanelCurIndex = Mathf.Min((int)Inven_Panels.Gita_Panel, iPanelCurIndex + 1);
        else
            iPanelCurIndex = Mathf.Max((int)Inven_Panels.Weapon_Panel, iPanelCurIndex - 1);


        Cur_Panel = Array_Panel[iPanelCurIndex];
    }

    public IEnumerator MoveTo_ScrollView(UI_Inventory _Object)
    {
        fScrollValue = (float)iPanelCurIndex / (int)Inven_Panels.Gita_Panel;

        _Object.State = UI_Inventory.InputState.Move;

        float fTimer = 0.0f;
        float fSpeed = 5.0f;

        while (true)
        {
            yield return null;

            fTimer += Time.fixedDeltaTime * fSpeed;

            Get<Scrollbar>((int)ScrollBars.ScrollbarHorizontal).value = Mathf.Lerp(Get<Scrollbar>((int)ScrollBars.ScrollbarHorizontal).value, fScrollValue, fTimer);

            if (1.0 <= fTimer)
            {
                _Object.State = UI_Inventory.InputState.NonMove;
                yield break;
            }
        }
    }


 

}
