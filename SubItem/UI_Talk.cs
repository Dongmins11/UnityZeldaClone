using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class UI_Talk : UI_Base
{
    enum Texts
    {
        CharaterName_Text,
        Talk_Text,
        Success_Text,
        Number_Text,
        Cancle_Text,
    }

    enum GameObjects
    {
        Select_Success,
        Select_Number,
        Select_Cancle,

        Select_Glow_Success,
        Select_Glow_Number,
        Select_Glow_Cancle,

        Talk_Select_Object,
        Selector,
    }

    enum Dialogue_Type
    {
        Normal,
        Choice,
    }

    Animator Canvas_Anim;
    Animator Select_Anim;
    UI_TalkSelect Talk_Select;

    Dialogue_Type My_DialogueType = Dialogue_Type.Normal;

    Story CurStory;
    Ink.Runtime.Object Npc_Name;

    int iMaxFontLength = 18;
    int iChangeFontSize = 40;
    int iDefalutFontSize;

    int iCurChoiceIndex = 0;
    int iPreChoiceIndex = 0;
    int iMaxChoiceIndex = 0;

    public int iFoodCountIndex { get; private set; } = 1;
    readonly int iFoodCountMaxIndex = 50;

    readonly int iAnim_Hash = Animator.StringToHash("Talk_Select");

    float fTextSpeed = 1.0f;

    bool bTalk_Start = false;
    bool bBox_InfoCheck = false;

    IEnumerator Input_Funtion;

    public override void init()
    {
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        iDefalutFontSize = GetText((int)Texts.Talk_Text).fontSize;

        HideSelect();

        AnimatorInit();
    }

    private void HideSelect()
    {
        GetObject((int)GameObjects.Select_Success).SetActive(false);
        GetObject((int)GameObjects.Select_Number).SetActive(false);
        GetObject((int)GameObjects.Select_Cancle).SetActive(false);

        GetObject((int)GameObjects.Select_Glow_Success).SetActive(false);
        GetObject((int)GameObjects.Select_Glow_Number).SetActive(false);
        GetObject((int)GameObjects.Select_Glow_Cancle).SetActive(false);

        GetObject((int)GameObjects.Selector).SetActive(false);
    }

    private void AnimatorInit()
    {
        GameObject TempSelectObject = null;
        TempSelectObject = GetObject((int)GameObjects.Talk_Select_Object);

        Canvas_Anim = GetComponent<Animator>();

        if (null != TempSelectObject)
        {
            Talk_Select = TempSelectObject.GetComponent<UI_TalkSelect>();
            Select_Anim = TempSelectObject.GetComponent<Animator>();
        }
    }

    public void Show_TalkText(TextAsset _InkJson)
    {
        if (false == bTalk_Start)
        {
            CurStory = new Story(_InkJson.text);
            Npc_Name = CurStory.variablesState.GetVariableWithName("My_Name");
            gameObject.SetActive(true);
            bTalk_Start = true;

            ContinueStroy();
            

            PlayerManager.input.bCanInput = false;
            
        }
        else
            ContinueStroy();
    }

    public void Show_QuestTalkText(Quest _Quest)
    {
        if (Quest.Quest_Progress.Can_Start != _Quest.My_Progress &&
            Quest.Quest_Progress.Can_Success != _Quest.My_Progress)
            return;

        if (false == bTalk_Start)
        {
            CurStory = new Story(_Quest.My_Talk.text);
            Npc_Name = CurStory.variablesState.GetVariableWithName("My_Name");
            gameObject.SetActive(true);
            bTalk_Start = true;

            ContinueStroy(_Quest);
            Npc_Anim.onNpcTalk.Invoke(Npc_Name.ToString());
            CinemachineManager.ActiveVirtualCam.Invoke(Npc_Name.ToString(), null);

            PlayerManager.input.bCanInput = false;
            MasterAudio.PlaySound("TalkStart");
        }
		else
		{
            ContinueStroy(_Quest);
            Npc_Anim.onNpcSay.Invoke(Npc_Name.ToString());
        }
    }

    public bool Show_ShopTalkText(TextAsset _InkJson, ItemData _ItemData)
    {
        if (false == bTalk_Start)
        {
            CurStory = new Story(_InkJson.text);
            Npc_Name = CurStory.variablesState.GetVariableWithName("My_Name");
            gameObject.SetActive(true);
            

            if (_ItemData is ItemArmorData ArmorData)
            {
                CurStory.variablesState["ItemName"] = ArmorData.strname;
                CurStory.variablesState["Rupee"] = ArmorData.iPrice;
                CurStory.variablesState["PLAYER_RUPEE"] = GameManager.item.Inven.iRupeeCount;
                CurStory.variablesState["IsVaildeRupee"] = false;
            }
            else if (_ItemData is ItemFoodData FoodData)
            {
                iFoodCountIndex = 1;
                Input_Funtion = FoodCount_Input();
                StartCoroutine(Input_Funtion);

                CurStory.variablesState["ItemName"] = FoodData.strname;
                CurStory.variablesState["Rupee"] = FoodData.iPrice;
                CurStory.variablesState["PLAYER_RUPEE"] = GameManager.item.Inven.iRupeeCount;
                CurStory.variablesState["IsVaildeRupee"] = false;
                CurStory.variablesState["ItemCount"] = iFoodCountIndex;
                GetText((int)Texts.Number_Text).text = iFoodCountIndex.ToString();
            }

            bBox_InfoCheck = false;
            bTalk_Start = true;

            ContinueStroy();
            

        }
        else
            ContinueStroy();

        return bBox_InfoCheck;
    }


    public void Hide_TalkText()
    {
        PlayerManager.input.bCanInput = true;

        bTalk_Start = false;
        gameObject.SetActive(false);
        CurStory = null;

        iCurChoiceIndex = 0;

        HideSelect();

        GetText((int)Texts.CharaterName_Text).text = "";
        GetText((int)Texts.Talk_Text).DOText("", 0f);

        MasterAudio.PlaySound("TalkEnd");
    }

    public void Hide_TalkText(Quest _Quest)
    {
        Hide_TalkText();

        if (Quest.Quest_Progress.Can_Start == _Quest.My_Progress)
            GameManager.Quest.Start_Quest(_Quest);

        CinemachineManager.InActiveVirtualCam.Invoke();

    }

    private void ContinueStroy()
    {
        switch (My_DialogueType)
        {
            case Dialogue_Type.Normal:
                break;
            case Dialogue_Type.Choice:
                CurStory.ChooseChoiceIndex(iCurChoiceIndex);
                break;
        }

        if (CurStory.canContinue)
        {
            string strCurText = CurStory.Continue();
            ChangeFontSize(CurStory.currentText);

            GetText((int)Texts.CharaterName_Text).text = Npc_Name.ToString();
            GetText((int)Texts.Talk_Text).DOText("", 0f);
            GetText((int)Texts.Talk_Text).DOText(strCurText, fTextSpeed);

            ChoiceStory();
        }
        else
        {
            if (null != CurStory.variablesState["IsVaildeRupee"])
            {
                bBox_InfoCheck = (bool)CurStory.variablesState["IsVaildeRupee"];

                if (true == bBox_InfoCheck && GameManager.Dialogue.Cur_TalkType == DialogueManager.Talk_Type.FoodShop_Talk)
                    StopCoroutine(Input_Funtion);
            }

            Hide_TalkText();
        }
    }

    private void ContinueStroy(Quest _Quest)
    {
        switch (My_DialogueType)
        {
            case Dialogue_Type.Choice:
                CurStory.ChooseChoiceIndex(iCurChoiceIndex);
                break;
        }

        if (CurStory.canContinue)
        {
            string strCurText = CurStory.Continue();
            ChangeFontSize(CurStory.currentText);

            GetText((int)Texts.CharaterName_Text).text = Npc_Name.ToString();
            GetText((int)Texts.Talk_Text).DOText("", 0f);
            GetText((int)Texts.Talk_Text).DOText(strCurText, fTextSpeed);

            ChoiceStory();
        }
        else
            Hide_TalkText(_Quest);
    }

    private void ChoiceStory()
    {
        List<Choice> CurChoiceList = CurStory.currentChoices;

        if (0 == CurChoiceList.Count)
        {
            iCurChoiceIndex = 0;
            Move_Selector();
            Show_Glow_Object();
            HideSelect();
            My_DialogueType = Dialogue_Type.Normal;
            return;
        }
        else
        {
            MasterAudio.PlaySound("TalkNext");
            My_DialogueType = Dialogue_Type.Choice;
            iMaxChoiceIndex = CurChoiceList.Count - 1;
        }


        switch (GameManager.Dialogue.Cur_TalkType)
        {
            case DialogueManager.Talk_Type.Normal_Talk:
            case DialogueManager.Talk_Type.Quest_Talk:
                if (1 < CurChoiceList.Count)
                {
                    GetObject((int)GameObjects.Select_Cancle).SetActive(true);
                    GetText((int)Texts.Cancle_Text).text = CurChoiceList[1].text;
                }

                GetObject((int)GameObjects.Select_Success).SetActive(true);
                GetText((int)Texts.Success_Text).text = CurChoiceList[0].text;
                break;
            case DialogueManager.Talk_Type.ArmorShop_Talk:
                GetObject((int)GameObjects.Select_Success).SetActive(true);
                GetText((int)Texts.Success_Text).text = CurChoiceList[0].text;

                GetObject((int)GameObjects.Select_Cancle).SetActive(true);
                GetText((int)Texts.Cancle_Text).text = CurChoiceList[1].text;
                break;
            case DialogueManager.Talk_Type.FoodShop_Talk:
                GetObject((int)GameObjects.Select_Number).SetActive(true);
                GetObject((int)GameObjects.Select_Cancle).SetActive(true);
                GetText((int)Texts.Cancle_Text).text = CurChoiceList[1].text;
                break;
        }

        Move_Selector();
        Show_Glow_Object();

        Select_Anim.SetTrigger(iAnim_Hash);
        GetObject((int)GameObjects.Selector).SetActive(true);
    }


    private void Show_Glow_Object()
    {
        if (DialogueManager.Talk_Type.FoodShop_Talk != GameManager.Dialogue.Cur_TalkType)
        {
            switch (iPreChoiceIndex)
            {
                case 0:
                    GetObject((int)GameObjects.Select_Glow_Success).SetActive(false);
                    break;
                case 1:
                    GetObject((int)GameObjects.Select_Glow_Cancle).SetActive(false);
                    break;
            }
            switch (iCurChoiceIndex)
            {
                case 0:
                    GetObject((int)GameObjects.Select_Glow_Success).SetActive(true);
                    break;
                case 1:
                    GetObject((int)GameObjects.Select_Glow_Cancle).SetActive(true);
                    break;
            }
        }
        else
        {
            switch (iPreChoiceIndex)
            {
                case 0:
                    GetObject((int)GameObjects.Select_Glow_Number).SetActive(false);
                    break;
                case 1:
                    GetObject((int)GameObjects.Select_Glow_Cancle).SetActive(false);
                    break;
            }
            switch (iCurChoiceIndex)
            {
                case 0:
                    GetObject((int)GameObjects.Select_Glow_Number).SetActive(true);
                    break;
                case 1:
                    GetObject((int)GameObjects.Select_Glow_Cancle).SetActive(true);
                    break;
            }
        }
    }



    private void Move_Selector()
    {
        GameObject CurObject = null;

        if (DialogueManager.Talk_Type.FoodShop_Talk != GameManager.Dialogue.Cur_TalkType)
        {
            switch (iCurChoiceIndex)
            {
                case 0:
                    CurObject = GetObject((int)GameObjects.Select_Success);
                    break;
                case 1:
                    CurObject = GetObject((int)GameObjects.Select_Cancle);
                    break;
            }
        }
        else
        {
            switch (iCurChoiceIndex)
            {
                case 0:
                    CurObject = GetObject((int)GameObjects.Select_Number);
                    break;
                case 1:
                    CurObject = GetObject((int)GameObjects.Select_Cancle);
                    break;
            }
        }

        if (null != CurObject)
            Get<GameObject>((int)GameObjects.Selector).transform.localPosition = CurObject.transform.localPosition;
    }

    private void Choice_Input()
    {
        if (iPreChoiceIndex != iCurChoiceIndex)
            iPreChoiceIndex = iCurChoiceIndex;

        if (Input.GetKeyDown(KeyCode.S))
        {
            iCurChoiceIndex = Mathf.Min(iMaxChoiceIndex, ++iCurChoiceIndex);
            Move_Selector();
            Show_Glow_Object();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            iCurChoiceIndex = Mathf.Max(0, --iCurChoiceIndex);
            Move_Selector();
            Show_Glow_Object();
        }

    }

    private IEnumerator FoodCount_Input()
    {
        while (true)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.D))
            {
                iFoodCountIndex = Mathf.Min(iFoodCountMaxIndex, ++iFoodCountIndex);

                GetText((int)Texts.Number_Text).text = iFoodCountIndex.ToString();
                CurStory.variablesState["ItemCount"] = iFoodCountIndex;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                iFoodCountIndex = Mathf.Max(1, --iFoodCountIndex);

                GetText((int)Texts.Number_Text).text = iFoodCountIndex.ToString();
                CurStory.variablesState["ItemCount"] = iFoodCountIndex;
            }
        }
    }


    private void ChangeFontSize(string _strCurText)
    {
        if (iMaxFontLength < _strCurText.Length)
        {
            GetText((int)Texts.Talk_Text).fontSize = iChangeFontSize;
            fTextSpeed = 1.5f;
        }
        else
        {
            GetText((int)Texts.Talk_Text).fontSize = iDefalutFontSize;
            fTextSpeed = 1.0f;
        }
    }

    private void Key_Input()
    {
        switch (My_DialogueType)
        {
            case Dialogue_Type.Normal:
               
                break;
            case Dialogue_Type.Choice:
                
                Choice_Input();
                break;
        }
    }

    private void Update()
    {
        Key_Input();
    }

}
