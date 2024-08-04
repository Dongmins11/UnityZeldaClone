using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene_Loading_Scene : MonoBehaviour
{
    [SerializeField]
    Animator My_Anim;

    int iStartAnimHash = Animator.StringToHash("Load_Open");
    int iEndAnimHash = Animator.StringToHash("Load_Close");

    public bool bIsLoadStartOk { get; set; } = false;
    public bool bIsLoadEndOk { get; set; } = false;

    public void Start_EventCall()
    {
        bIsLoadStartOk = true;
    }

    public void End_EventCall()
    {
        bIsLoadEndOk = true;
    }

    public void InitLoadBoolean()
    {
        bIsLoadStartOk = false;
        bIsLoadEndOk = false;
    }

    public void StartAnim()
    {
        My_Anim.SetTrigger(iStartAnimHash);
    }

    public void EndAnim()
    {
        My_Anim.SetTrigger(iEndAnimHash);
    }

}
