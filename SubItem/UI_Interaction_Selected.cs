using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Interaction_Selected : MonoBehaviour
{
    UI_Scene_Interaction ParentCanvas_Compo;

    public void init(UI_Scene_Interaction _ParentCanvas)
    {
        ParentCanvas_Compo = _ParentCanvas;
    }

    public void Event_Call()
    {
        ParentCanvas_Compo.Event_Select_Call();
    }




}
