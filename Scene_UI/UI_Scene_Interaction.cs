using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Scene_Interaction : UI_Base
{
    enum GameObjects
    {
        Interaction_Object,
        Selected_Object,
        R_Object,
        E_Object,
        F_Object,
        Shift_Run_Object,
    }

    enum Texts
    {
        R_Text,
        E_Text,
        F_Text,
        Shift_Text,
        E_Inter_Text,
    }

    enum Images
    {
        R_Inter_Select,
        E_Inter_Select,
        F_Inter_Select,
        Shift_Inter_Select,
        E_Back_Bar,
    }
    
    enum Selected
    {
        Selected_Object,
    }

    public enum Interaction_Type
    {
        Skill_R,
        Item_E,
        Shop_E,
        InterAction_E,
        Enter_E,
        Throw_F,
        Run_Shift,
    }

    Animator Object_Animator = null;
    Animator Select_Animator = null;

    Color Select_Color = new Color(0.0f,0.0f,0.0f,1.0f);
    Color NonSelect_Color = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    int Object_AnimHash = Animator.StringToHash("Interaction_Obj");
    int Select_AnimHash = Animator.StringToHash("Interaction");

    bool IsOn;

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<UI_Interaction_Selected>(typeof(Selected));

        ComponentInit();
        Hide_Interaction_UI();
    }

    private void ComponentInit()
    {
        Object_Animator = GetObject((int)GameObjects.Interaction_Object).GetComponent<Animator>();
        Select_Animator = GetObject((int)GameObjects.Selected_Object).GetComponent<Animator>();
        Get<UI_Interaction_Selected>((int)Selected.Selected_Object).init(this);
    }

    public void Show_Interaction_UI(Interaction_Type _eType)
    {
        if (true == IsOn)
            return;

        bool bBackBarActive = false;

        gameObject.SetActive(true);
        Object_Animator.SetTrigger(Object_AnimHash);

        switch (_eType)
        {
            case Interaction_Type.Skill_R:
                Ignore_SelectImageActive((int)_eType);
                break;
            case Interaction_Type.Item_E:
                bBackBarActive = true;
                Ignore_SelectImageActive((int)Interaction_Type.Item_E, "획득");
                break;
            case Interaction_Type.Shop_E:
                bBackBarActive = true;
                Ignore_SelectImageActive((int)Interaction_Type.Item_E, "구매");
                break;
            case Interaction_Type.InterAction_E:
                bBackBarActive = true;
                Ignore_SelectImageActive((int)Interaction_Type.Item_E, "조사하기");
                break;
            case Interaction_Type.Enter_E:
                bBackBarActive = true;
                Ignore_SelectImageActive((int)Interaction_Type.Item_E, "입장하기");
                break;
            case Interaction_Type.Throw_F:
                Ignore_SelectImageActive((int)_eType - 1);
                break;
            case Interaction_Type.Run_Shift:
                Ignore_SelectImageActive((int)_eType - 1);
                break;
        }

        GetImage((int)Images.E_Back_Bar).gameObject.SetActive(bBackBarActive);

        IsOn = false;
    }

    public void Hide_Interaction_UI()
    {
        if(true == IsOn)
        {
            IsOn = !IsOn;
            return;
        }

        gameObject.SetActive(false);
    }

    public void Show_Select_Interactio_UI()
    {
        Select_Animator.SetTrigger(Select_AnimHash);
    }

    public void Event_Select_Call()
    {
        Hide_Interaction_UI();
    }


    private void Ignore_SelectImageActive(int _ignoreIndex, string _strInterText = "", bool _bIsActive = true)
    {
        for(int i =0; i < (int)Images.E_Back_Bar; ++i)
        {
            if(_ignoreIndex != i)
            {
                GetImage(i).gameObject.SetActive(!_bIsActive);
                GetText(i).color = NonSelect_Color;
            }
            else
            {
                GetImage(i).gameObject.SetActive(_bIsActive);
                GetText(i).color = Select_Color;
            }
        }

        GetText((int)Texts.E_Inter_Text).text = _strInterText;
    }


    void Start()
    {
        init();
    }
}
