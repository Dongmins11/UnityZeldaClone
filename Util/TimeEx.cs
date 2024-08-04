using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEx : MonoBehaviour
{
    List<float> m_ListTimes = new List<float>();
    
    public float TimeScale
    {
        get;
        set;
    } = 1.0f;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        for(int i =0; i < (int)Define.TimeType.End; ++i)
            m_ListTimes.Add(new float());
    }
}
