using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay
{
    public enum Delay_Type
    {
        Delay_Type_Normal,
        Delay_Type_First,
        Delay_Type_End
    }

    Delay_Type m_MyType = Delay_Type.Delay_Type_End;

    Action m_DelayAction = null;

    float m_fTime = 0.0f;
    float m_fDelayTime = 0.0f;

    float m_fDestroyTime = 0.0f;
    float m_fMaxDestroyTime = 10.0f;

    bool m_isCheck { get; set; }
    bool m_isDestroy;

    public Delay(float fDelayTime, Action Function, Delay_Type Type)
    {
        m_fDelayTime = fDelayTime;
        m_DelayAction = Function;
        m_isDestroy = false;
        m_MyType = Type;
        //GameManager.Time.AddDelayFunction(DelayFunction);
    }
    void NormalDelayFunction()
    {
        if (true == m_isCheck)
        {
            m_fTime += Time.deltaTime;
            m_fDestroyTime = 0.0f;

            if (m_fTime >= m_fDelayTime)
            {
                m_DelayAction?.Invoke();
                m_fTime = 0.0f;
                m_isCheck = false;
            }
        }
        else
        {
            m_fDestroyTime += Time.deltaTime;

            if (m_fMaxDestroyTime <= m_fDestroyTime)
            {
                if (!m_isDestroy /*&& GameManager.Time.DelayFindFunction(DelayFunction)*/)
                {
                    m_isDestroy = true;
                    //GameManager.Time.DestroyDelayFunction(DelayFunction);
                }
            }
        }
    }
    void FirstDelayFuntion()
    {
        if (true == m_isCheck)
        {
            m_fTime += Time.deltaTime;
            m_fDestroyTime = 0.0f;

            if (m_fTime >= m_fDelayTime)
            {
                m_fTime = 0.0f;
                m_isCheck = false;
            }
        }
        else
        {
            m_fDestroyTime += Time.deltaTime;

            if (m_fMaxDestroyTime <= m_fDestroyTime)
            {
                if (!m_isDestroy/* && GameManager.Time.DelayFindFunction(DelayFunction)*/)
                {
                    m_isDestroy = true;
                    //GameManager.Time.DestroyDelayFunction(DelayFunction);
                }
            }
        }


    }
    void NormalStartFuntion()
    {
        if (false == m_isCheck)
            m_isCheck = true;

        if (true == m_isDestroy)
        {
            //GameManager.Time.AddDelayFunction(DelayFunction);
            m_isDestroy = false;
        }
    }
    void FirstStartuntion()
    {
        if (false == m_isCheck)
        {
            m_DelayAction?.Invoke();
            m_isCheck = true;
        }
        if (true == m_isDestroy)
        {
            //GameManager.Time.AddDelayFunction(DelayFunction);
            m_isDestroy = false;
        }
    }
    void DelayFunction()
    {
        switch (m_MyType)
        {
            case Delay_Type.Delay_Type_Normal:
                NormalDelayFunction();
                break;
            case Delay_Type.Delay_Type_First:
                FirstDelayFuntion();
                break;
        }
    }
    public void StartFunction()
    {
        switch (m_MyType)
        {
            case Delay_Type.Delay_Type_Normal:
                NormalStartFuntion();
                break;
            case Delay_Type.Delay_Type_First:
                FirstStartuntion();
                break;
        }
    }
    public bool GetCheck()
    {
        return m_isCheck;
    }



}
