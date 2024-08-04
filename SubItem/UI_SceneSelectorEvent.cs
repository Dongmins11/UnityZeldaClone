using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SceneSelectorEvent : MonoBehaviour
{
    public enum Select_Type
    {
        Type_NewGame,
        Type_Exit,
    }

    public void SelectEventCall(int _iIndex)
    {

        switch (_iIndex)
        {
            case (int)Select_Type.Type_NewGame:
                CallToNewGameFunction();
                break;
            case (int)Select_Type.Type_Exit:
                CallToExitFuntion();
                break;
        }
    }


    void CallToNewGameFunction()
    {
        GameManager.Scene.LoadToNextScene(Define.Scene.Game);
    }

    void CallToExitFuntion()
    {
    }


}
