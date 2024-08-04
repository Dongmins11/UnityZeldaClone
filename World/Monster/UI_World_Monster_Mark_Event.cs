using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_World_Monster_Mark_Event : MonoBehaviour
{
    UI_World_Monster_Mark Mark = null;

    private void Start()
    {
        Mark = transform.parent.GetComponent<UI_World_Monster_Mark>();
    }

    public void Anim_Evnet_Call()
    {
        Mark?.Hide_Monster_Mark();
    }



}
