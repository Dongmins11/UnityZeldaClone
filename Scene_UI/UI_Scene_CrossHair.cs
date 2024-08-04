using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene_CrossHair : UI_Base
{
    public override void init()
    {
        gameObject.SetActive(false);
    }


    public void OnCrossHair()
    {
        if(false == gameObject.activeSelf)
            gameObject.SetActive(true);
    }

    public void OffCrossHair()
    {
        if (true == gameObject.activeSelf)
            gameObject.SetActive(false);
    }

}
