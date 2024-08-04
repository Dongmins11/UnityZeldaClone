using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    Action m_InputAction = null;
    Action m_FxiedInputAction = null;

    Action<Define.MouseEvent> m_MouseAction = null;

    bool m_isPressed = false;
    public void Clear()
    {
        m_isPressed = false;

    }

    public void AddMouseAction(Action<Define.MouseEvent> _MouseEvent)
    {
        m_MouseAction -= _MouseEvent;
        m_MouseAction += _MouseEvent;
    }

    public void AddFixedAction(Action _FixedFunction)
    {
        m_FxiedInputAction -= _FixedFunction;
        m_FxiedInputAction += _FixedFunction;
    }

    public void AddInputAction(Action _InputFunction)
    {
        m_InputAction -= _InputFunction;
        m_InputAction += _InputFunction;
    }

    public void DeleteInputAction(Action _InputFunction)
    {
        if (null == _InputFunction)
            return;

        if (true == Util.FindKeyAction(m_InputAction, _InputFunction))
            m_InputAction -= _InputFunction;
    }

    public void InputFiexdOnUpdate()
    {
        if (Input.anyKey && null != m_FxiedInputAction)
            m_FxiedInputAction?.Invoke();

        if (null != m_MouseAction)
        {
            if (Input.GetMouseButton(0))
            {
                m_MouseAction?.Invoke(Define.MouseEvent.Press);

                m_isPressed = true;
            }
            else
            {
                if (true == m_isPressed)
                {
                    m_MouseAction?.Invoke(Define.MouseEvent.Click);
                    m_isPressed = false;
                }
            }
        }
    }

    public void InputOnUpdate()
    {
        m_InputAction?.Invoke();

        if (null != m_MouseAction)
        {
            if (Input.GetMouseButton(0))
            {
                m_MouseAction?.Invoke(Define.MouseEvent.Press);

                m_isPressed = true;
            }
            else
            {
                if (true == m_isPressed)
                {
                    m_MouseAction?.Invoke(Define.MouseEvent.Click);
                    m_isPressed = false;
                }
            }
        }
    }




}
