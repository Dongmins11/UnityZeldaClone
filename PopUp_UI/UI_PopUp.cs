using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PopUp : UI_Base
{
    public override void init()
    {
        GameManager.UI.SetCanvas(this.gameObject, true);
    }

    public virtual void ClosePopUpUI()
    {
        GameManager.UI.ClosePopupUI(this);
    }

}
