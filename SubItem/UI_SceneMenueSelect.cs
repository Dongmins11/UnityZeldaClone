using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class UI_SceneMenueSelect : UI_Base
{
    enum GameObjects
    {
        Selector,
        SelectPanel,
        Text_NewGame,
        Text_Exit,
    }

    private Vector3 SelectPosition = Vector3.zero;

    private List<GameObject> ListSelectText = new List<GameObject>();

    private int iCurSelectIndex = 0;

    private int iPreSelectIndex = 0;

    private const int iSelectMaxIndex = (int)GameObjects.Text_Exit - (int)GameObjects.Text_NewGame;

    private Color BackUpColor;

    private bool IsOn = false;

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));

       GameManager.Input.AddInputAction(SelectKeyChange);

        for (int i = (int)GameObjects.Text_NewGame; i <= (int)GameObjects.Text_Exit; ++i)
            ListSelectText.Add(GetObject(i));

        BackUpColor = ListSelectText[iCurSelectIndex ].GetComponent<Text>().color;

        ListSelectText[iCurSelectIndex].GetComponent<Text>().color = Color.black;
    }

    private void SelectKeyChange()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MasterAudio.FireCustomEvent("startevent", transform);
            iPreSelectIndex = iCurSelectIndex;
            iCurSelectIndex = Mathf.Max(0, --iCurSelectIndex );
            Selecter();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MasterAudio.FireCustomEvent("startevent", transform);
            iPreSelectIndex = iCurSelectIndex;
            iCurSelectIndex = Mathf.Min(iSelectMaxIndex, ++iCurSelectIndex );
            Selecter();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.Input.DeleteInputAction(SelectKeyChange);
            MasterAudio.FireCustomEvent("startevent", transform);
            ListSelectText[iCurSelectIndex ].GetComponent<UI_SceneSelectorEvent>().SelectEventCall(iCurSelectIndex );
        }
    }

    private void Selecter()
    {
        SelectPosition = Get<GameObject>((int)GameObjects.Selector).transform.position;
         
        SelectPosition.y = ListSelectText[iCurSelectIndex ].transform.position.y;

        Get<GameObject>((int)GameObjects.Selector).transform.position = SelectPosition;

        ListSelectText[iPreSelectIndex].GetComponent<Text>().color = BackUpColor;

        ListSelectText[iCurSelectIndex].GetComponent<Text>().color = Color.black;
    }

    void Start()
    {       init();

    }

}
