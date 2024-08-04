using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region_Base : MonoBehaviour
{
    public enum Region_Type
    {
        Unknow,
        Hiral,
        Korg_Forest,
        Kakirico_Village,
        Linell_Province,
        Ma_Ouns,
        Tummi_Mke,
        Naboris,
        End,
    }

    public Region_Type My_Type;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GameManager.UIContents.Region_Update(My_Type);
    }

}
