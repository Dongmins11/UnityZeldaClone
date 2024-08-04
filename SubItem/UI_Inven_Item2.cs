using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inven_Item2 : UI_Base
{
    enum GameObjects
    {
        ItemIcon,
        ItemNameText,
    }
    enum Imges
    {
        ItemIcon,
    }

    string m_strName = "";

    private Vector2 m_VecBeginPoint = Vector2.zero;
    private Vector2 m_VecMoveBegin = Vector2.zero;

    public void SetInformation(string name)
    {
        m_strName = name;
    }

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));

        Bind<Image>(typeof(Imges));

        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<Text>().text = m_strName;

        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent(OnPointDown, Define.UIEvent.Down);
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent(OnDrag, Define.UIEvent.Drag);
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent(OnPointUp, Define.UIEvent.Up);
    }

    void OnPointDown(PointerEventData eventData)
    {
        m_VecBeginPoint = transform.position;
        m_VecMoveBegin = eventData.position;

        GetImage((int)Imges.ItemIcon).color = Color.red;
    }

    void OnDrag(PointerEventData eventData)
    {
        transform.position = m_VecBeginPoint + (eventData.position - m_VecMoveBegin);
    }

    void OnPointUp(PointerEventData eventData)
    {
        transform.position = m_VecBeginPoint;

        GetImage((int)Imges.ItemIcon).color = Color.white;
    }

    void Start()
    {
        init();

    }
}
