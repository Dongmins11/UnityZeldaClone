using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager
{
    int m_iOrder = 10;

    Stack<UI_PopUp> m_PopupStack = new Stack<UI_PopUp>();

    private GameObject Root
    {
        get
        {
            GameObject Root = GameObject.Find("@UI_Root");

            if (null == Root)
                Root = new GameObject { name = "@UI_Root" };

            return Root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (true == sort)
        {
            canvas.sortingOrder = (m_iOrder);
            ++m_iOrder;
        }
        else
            canvas.sortingOrder = 0;
    }

    public void SortingOrder(GameObject _Object, int _iSortValue)
    {
        Canvas canvas = _Object.GetComponent<Canvas>();

        if (null == canvas)
            return;

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        canvas.sortingOrder = _iSortValue;
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resources.CreatePrefab($"UI/SubItem/{name}");

        if (null != parent)
            go.transform.SetParent(parent);

        return go.GetOrAddComponent<T>();
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_PopUp
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resources.CreatePrefab($"UI/PopUp/{name}");

        T popup = Util.GetOrAddComponent<T>(go);

        m_PopupStack.Push(popup);

        go.transform.SetParent(Root.transform);

        return popup;

    }


    public T ShowSceneUI<T>(string name = null, bool _bIsActive = true) where T : Component
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resources.CreatePrefab($"UI/Scene/{name}");
        T Scene = Util.GetOrAddComponent<T>(go);

        go.transform.SetParent(Root.transform);

        if (false == _bIsActive)
        {
            go.GetComponent<UI_Base>().init();
            go.SetActive(_bIsActive);
        }

        return Scene;
    }


    public T CreateWorldUI<T>(string name = null, bool _bIsActive = true) where T : Component
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resources.CreatePrefab($"UI/World/{name}");
        T Scene = Util.GetOrAddComponent<T>(go);

        if (false == _bIsActive)
        {
            go.GetComponent<UI_Base>().init();
            go.SetActive(_bIsActive);
        }

        return Scene;
    }

    public T Show_DonDestroy_SceneUI<T>(string name = null) where T : Component
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resources.CreatePrefab($"UI/Scene/{name}");
        T Scene = Util.GetOrAddComponent<T>(go);

        return Scene;
    }
    public void ClosePopupUI(UI_PopUp popup)
    {
        if (0 == m_PopupStack.Count)
            return;

        if (popup != m_PopupStack.Peek())
            return;

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (0 == m_PopupStack.Count)
            return;

        --m_iOrder;
        UI_PopUp popup = m_PopupStack.Pop();
        GameManager.Resources.Destroy(popup.gameObject);

    }

    public void CloseAllPopupUi()
    {
        while (0 < m_PopupStack.Count)
        {
            ClosePopupUI();
        }
    }
    public void Clear()
    {
    }

}
