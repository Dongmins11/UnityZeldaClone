using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class UI_MainPopUp : UI_PopUp
{
    enum GameObjects
    {
        Quest,
        Inven,
        Setting,
    }

    enum BackGround
    {
        CanvasBackGround,
    }

    enum Texts
    {
        MainPopText,
    }

    enum Images
    {
        MainSelectIcon_1,
        MainSelectIcon_2,
        MainSelectIcon_3,
    }

    enum Inven_UI
    {
        Inven,
    }

    enum Quest_UI
    {
        Quest,
    }

    public enum Canvan_State
    {
        State_NonMove,
        State_Move,
        State_Setter,
    }

    public enum CavasType
    {
        CavasType_Quest = 3,
        CavasType_Inventory = 2,
        CavasType_Setting = 1,
    }

    public bool bIsOpening { get; private set; } = false;
    public Canvan_State State { get; private set; } = Canvan_State.State_NonMove;

    Vector3[] Array_UI_Position;

    List<GameObject> List_Canvas = new List<GameObject>();

    int iCurrentCanvasIndex = 0;
    int iMainCanvasIndex = 0;

    float fSlerpSpeed = 1.5f;

    const int iMaxCanvasIndex = 5;

    Coroutine InputCoroutine = null;

    public override void init()
    {
        base.init();

        Array_UI_Position = new Vector3[iMaxCanvasIndex];

        Bind<GameObject>(typeof(GameObjects));

        Bind<Text>(typeof(Texts));

        Bind<Image>(typeof(Images));

        Bind<Canvas>(typeof(BackGround));

        Bind<UI_Inventory>(typeof(Inven_UI));

        Bind<UI_Quest>(typeof(Quest_UI));

        Main_Cavnas_Init();

        First_Canvas_Init();

        gameObject.SetActive(false);
    }

    private void Main_Cavnas_Init()
    {
        Get<UI_Inventory>((int)Inven_UI.Inven).init();
        Get<UI_Quest>((int)Quest_UI.Quest).init();
    }

    private void OpenInit()
    {
        Switch_IndexForUI();
        Get<UI_Inventory>((int)Inven_UI.Inven).Reset_UI_Inven();
        Get<UI_Quest>((int)Quest_UI.Quest).Reset_UI_Quest();
        AddKeyAction(Switch_Canvas, true);
    }

    public void OpenCanvas(CavasType _eType)
    {
        if (true == bIsOpening)
            return;
        else
            bIsOpening = true;

        PlayerManager.input.bCanInput = false;

        gameObject.SetActive(true);

        iCurrentCanvasIndex = (int)_eType;

        iMainCanvasIndex = iCurrentCanvasIndex - 1;

        OpenInit();

        Vector3 InitPosition = Vector3.zero;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        for (int i = 0; i <= (int)GameObjects.Setting; ++i)
        {
            InitPosition = Array_UI_Position[i + (iCurrentCanvasIndex - 1)];

            Get<GameObject>(i).transform.localPosition = InitPosition;
        }

    }

    public void CloseCanvas()
    {
        if (Canvan_State.State_NonMove != State || false == bIsOpening)
            return;
        else
            bIsOpening = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        InputCoroutine = null;
        AllDeleteKeyAction();
        PlayerManager.input.bCanInput = true;
        gameObject.SetActive(false);
    }

    public void AddKeyAction(Action _Function, bool _bMyKeyInput = false)
    {
        if (true == _bMyKeyInput)
            State = Canvan_State.State_NonMove;

        GameManager.Input.AddInputAction(_Function);
    }

    public void DeleteKeyAction(Action _Function, bool _bMyKeyInput = false)
    {
        if (true == _bMyKeyInput)
            State = Canvan_State.State_Setter;

        GameManager.Input.DeleteInputAction(_Function);
    }

    private void AllDeleteKeyAction()
    {
        MasterAudio.PlaySound("INVEN4");
        DeleteKeyAction(Switch_Canvas, true);
        DeleteKeyAction(Get<UI_Inventory>((int)Inven_UI.Inven).KeyInput);
        DeleteKeyAction(Get<UI_Quest>((int)Quest_UI.Quest).KeyInput);
    }

    void Select_Images_Init(int _iIndex, float _fAlphaValue = 1.0f)
    {
        Color TempColor;

        for (int i = 0; i <= (int)Images.MainSelectIcon_3; ++i)
        {
            TempColor = Get<Image>(i).color;
            TempColor.a = 0.3f;
            Get<Image>(i).color = TempColor;
        }

        TempColor = Get<Image>(_iIndex).color;
        TempColor.a = _fAlphaValue;
        Get<Image>(_iIndex).color = TempColor;
    }

    void First_Canvas_Init()
    {
        GameManager.UI.SortingOrder(Get<Canvas>((int)BackGround.CanvasBackGround).gameObject, 0);

        for (int i = 0; i < iMaxCanvasIndex; ++i)
        {
            Array_UI_Position[i] = new Vector3(Screen.width * (i - 2), 0.0f, 0.0f);
        }

        for (int i = (int)GameObjects.Setting; i >= (int)GameObjects.Quest; --i)
        {
            GameManager.UI.SortingOrder(Get<GameObject>(i), 1);
            List_Canvas.Add(Get<GameObject>(i).gameObject);
        }
    }

    void Switch_Canvas()
    {
        float fwheelInput = Input.GetAxis("Mouse ScrollWheel");

        switch (State)
        {
            case Canvan_State.State_NonMove:
                if (0 < fwheelInput)
                {
                    iCurrentCanvasIndex = Mathf.Max(1, --iCurrentCanvasIndex);

                    State = Canvan_State.State_Move;

                    StartCoroutine(PositionSetting_Canvas());

                    Switch_IndexForUI();
                }
                else if (0 > fwheelInput)
                {
                    iCurrentCanvasIndex = Mathf.Min(iMaxCanvasIndex - 2, ++iCurrentCanvasIndex);

                    State = Canvan_State.State_Move;

                    StartCoroutine(PositionSetting_Canvas());

                    Switch_IndexForUI();
                }
                break;
            case Canvan_State.State_Move:
                break;
        }
    }

    IEnumerator PositionSetting_Canvas()
    {
        iMainCanvasIndex = iCurrentCanvasIndex - 1;

        float Timer = 0.0f;

        while (true)
        {
            yield return null;

            Timer += Time.deltaTime * fSlerpSpeed;

            for (int i = -1; i < (int)GameObjects.Setting; ++i)
                Get<GameObject>(i + 1).transform.localPosition = Vector3.Slerp(Get<GameObject>(i + 1).transform.localPosition, Array_UI_Position[iCurrentCanvasIndex + i], Timer);

            if (1.0f <= Timer)
            {
                State = Canvan_State.State_NonMove;
                yield break;
            }
        }
    }

    void Switch_IndexForUI()
    {
        if (false == bIsOpening)
            return;

        switch (iMainCanvasIndex)
        {
            case 2:
                MasterAudio.PlaySound("INVEN4");
                GetText((int)Texts.MainPopText).text = "QUEST";
                Select_Images_Init((int)Images.MainSelectIcon_1);
                DeleteKeyAction(Get<UI_Inventory>((int)Inven_UI.Inven).KeyInput);
                AddKeyAction(Get<UI_Quest>((int)Quest_UI.Quest).KeyInput);
                break;
            case 1:
                MasterAudio.PlaySound("INVEN4");
                GetText((int)Texts.MainPopText).text = "INVENTORY";
                Select_Images_Init((int)Images.MainSelectIcon_2);
                DeleteKeyAction(Get<UI_Quest>((int)Quest_UI.Quest).KeyInput);
                AddKeyAction(Get<UI_Inventory>((int)Inven_UI.Inven).KeyInput);
                break;
            case 0:
                MasterAudio.PlaySound("INVEN4");
                GetText((int)Texts.MainPopText).text = "Guide";
                Select_Images_Init((int)Images.MainSelectIcon_3);
                DeleteKeyAction(Get<UI_Quest>((int)Quest_UI.Quest).KeyInput);
                DeleteKeyAction(Get<UI_Inventory>((int)Inven_UI.Inven).KeyInput);
                break;
        }
    }

}
