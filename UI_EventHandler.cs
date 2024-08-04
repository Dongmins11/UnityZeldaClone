using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    public Action<PointerEventData> m_OnBeginDragHandler = null;
    public Action<PointerEventData> m_OnDragHandler = null;
    public Action<PointerEventData> m_OnClickHandler = null;
    public Action<PointerEventData> m_OnDownHandler = null;
    public Action<PointerEventData> m_OnUpHandler = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (null != m_OnClickHandler)
            m_OnClickHandler?.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (null != m_OnBeginDragHandler)
            m_OnBeginDragHandler?.Invoke(eventData);

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (null != m_OnDragHandler)
            m_OnDragHandler?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (null != m_OnDownHandler)
            m_OnDownHandler?.Invoke(eventData);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (null != m_OnUpHandler)
            m_OnUpHandler?.Invoke(eventData);
    }
}
