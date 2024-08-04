using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_World_Item_Title : UI_Base
{
    enum Canvases
    {
        World_UI_ItemTitle
    }

    enum GameObjects
    {
        Item_Title_BackGround,
    }

    enum Texts
    {
        Item_Title_Text,
    }

    Animator My_Animator = null;
    Camera My_Cam = null;
    int iTriggerHash = Animator.StringToHash("NewTitle");

    public override void init()
    {
        Bind<Canvas>(typeof(Canvases));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));

        ComponentInit();
    }

    private void ComponentInit()
    {
        My_Animator = GetComponent<Animator>();
        My_Cam = Camera.main;

        Vector3 InitPos = transform.position;
        InitPos.y += 0.6f;
        transform.position = InitPos;

        gameObject.SetActive(false);
    }

    private void Billboard_UI()
    {
        if(null == My_Cam)
            My_Cam = Camera.main;

        Util.Billbord_UI(transform, My_Cam.transform);
    }

    public void Show_Title_UI(ItemData _ItemData)
    {
        gameObject.SetActive(true);
        My_Animator.SetTrigger(iTriggerHash);

        GetText((int)Texts.Item_Title_Text).text = _ItemData.strname;
    }

    public void Hide_Title_UI()
    {
        GetText((int)Texts.Item_Title_Text).text = "";
        gameObject.SetActive(false);
    }


    private void LateUpdate()
    {
        Billboard_UI();
    }


}
