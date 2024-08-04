using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Scene_Skill_Inven : UI_Base
{
    enum GameObjects
    {
        Skill_FrontGround_MagneCatch,
        Skill_FrontGround_RemoteBomb,
        Skill_FrontGround_IceMaker,
        Skill_FrontGround_TimeLock,

        Skill_Image_MagneCatch,
        Skill_Image_RemoteBomb,
        Skill_Image_IceMaker,
        Skill_Image_TimeLock,
    }

    enum Images
    {
        Skill_Inven_Image
    }

    enum OpenType
    {
        Open,
        Close,
        End,
    }

    OpenType Cur_Type = OpenType.Open;
    OpenType Pre_Type = OpenType.End;


    int iAnimHash = Animator.StringToHash("UI_Skill_Trigger");

    Animator My_Animator = null;

    UI_Scene_Talk UI_SkillTalk = null;

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));

        Bind<Image>(typeof(Images));

        GameManager.UI.SortingOrder(this.gameObject, 11);

        All_Hide();

        My_Animator = GetComponent<Animator>();

        UI_SkillTalk = GameManager.UI.ShowSceneUI<UI_Scene_Talk>("Skill_Inven_Talk_Canvas", false);

        Skill[] TempSkills = GameManager.UIContents.Skill_Inven.Get_Skills();

        for (int i = 0; i < TempSkills.Length; ++i)
        {
            if (null != TempSkills[i])
            {
                GameManager.UIContents.Get_QuickSlot_UI().Show_Skill_Slot(TempSkills[i]);
                GameManager.UIContents.Get_QuickSlot_UI().Update_QuickSlot_UI(TempSkills[i]);
                Setting_Image(TempSkills[i]);
            }
        }

    }

    public void TextEndCheck_EventCall()
    {
        UI_SkillTalk.gameObject.SetActive(true);
        UI_SkillTalk.Show_Skill_Talk();

        Cur_Type = OpenType.Close;
    }

    private void Hide_Skill_Inven()
    {
        UI_SkillTalk.gameObject.SetActive(false);
        gameObject.SetActive(false);

        Cur_Type = OpenType.Open;
        Pre_Type = OpenType.End;
    }


    private void All_Hide()
    {
        for (int i = 0; i <= (int)GameObjects.Skill_Image_TimeLock; ++i)
            GetObject(i).SetActive(false);
    }

    public void Setting_Image(Skill _Skill)
    {
        switch (_Skill.My_Type)
        {
            case Skill.Skill_Type.MagneCatch:
                GetObject((int)GameObjects.Skill_Image_MagneCatch).SetActive(true);
                break;
            case Skill.Skill_Type.RemoteBomb:
                GetObject((int)GameObjects.Skill_Image_RemoteBomb).SetActive(true);
                break;
            case Skill.Skill_Type.IceMaker:
                GetObject((int)GameObjects.Skill_Image_IceMaker).SetActive(true);
                break;
            case Skill.Skill_Type.TimeLock:
                GetObject((int)GameObjects.Skill_Image_TimeLock).SetActive(true);
                break;
        }

    }

    public void NewSetting(Skill _Skill)
    {
        gameObject.SetActive(true);
        My_Animator.SetTrigger(iAnimHash);

        for (int i = 0; i < (int)Skill.Skill_Type.End_Type; ++i)
            GetObject(i).SetActive(false);

        Setting_Image(_Skill);
        GetImage(0).sprite = null;

        switch (_Skill.My_Type)
        {
            case Skill.Skill_Type.MagneCatch:
                GetObject((int)GameObjects.Skill_FrontGround_MagneCatch).SetActive(true);
                GetImage((int)Images.Skill_Inven_Image).sprite = _Skill.My_Sprite;
                break;
            case Skill.Skill_Type.RemoteBomb:
                GetObject((int)GameObjects.Skill_FrontGround_RemoteBomb).SetActive(true);
                GetImage((int)Images.Skill_Inven_Image).sprite = _Skill.My_Sprite;
                break;
            case Skill.Skill_Type.IceMaker:
                GetObject((int)GameObjects.Skill_FrontGround_IceMaker).SetActive(true);
                GetImage((int)Images.Skill_Inven_Image).sprite = _Skill.My_Sprite;
                break;
            case Skill.Skill_Type.TimeLock:
                GetObject((int)GameObjects.Skill_FrontGround_TimeLock).SetActive(true);
                GetImage((int)Images.Skill_Inven_Image).sprite = _Skill.My_Sprite;
                break;
        }
    }


    public bool Skill_Inven_Add(Skill.Skill_Type _eType)
    {
        if (Cur_Type != Pre_Type)
        {
            Pre_Type = Cur_Type;

            switch (Cur_Type)
            {
                case OpenType.Open:
                    GameManager.UIContents.Skill_Inven.Add(_eType);
                    PlayerManager.input.bCanInput = false;
                    return false;
                case OpenType.Close:
                    Hide_Skill_Inven();
                    PlayerManager.input.bCanInput = true;
                    return true;
            }
        }

        return false;
    }


}
