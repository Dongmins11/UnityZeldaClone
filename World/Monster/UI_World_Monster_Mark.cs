using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_World_Monster_Mark : UI_Base
{
    enum GameObjects
    {
        Question_Object,
        Exclamation_Object
    }

    public enum Monster_State
    {
        Find,
        Chase,
    }

    Animator My_Anim;
    Transform My_Cam;

    int AnimHash = Animator.StringToHash("Exclamation");

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));

        Hide_Monster_Mark();

        My_Anim = GetObject((int)GameObjects.Exclamation_Object).GetComponent<Animator>();
    }

    public void Show_Monster_Mark(Monster_State _eType, Transform _transform)
    {
        transform.SetParent(_transform);
        transform.localPosition = Vector3.zero;

        switch (_eType)
        {
            case Monster_State.Find:
                Show_Find_Mark();
                break;
            case Monster_State.Chase:
                Show_Chase_Mark();
                break;
        }
    }

    public void Hide_Monster_Mark()
    {
        transform.localPosition = Vector3.zero;

        Hide_Find_Mark();
        Hide_Chase_Mark();
    }

    private void Show_Find_Mark()
    {
        Hide_Chase_Mark();
        GetObject((int)GameObjects.Question_Object).gameObject.SetActive(true);
    }

    private void Hide_Find_Mark()
    {
        GetObject((int)GameObjects.Question_Object).gameObject.SetActive(false);
    }


    private void Show_Chase_Mark()
    {
        Hide_Find_Mark();
        GetObject((int)GameObjects.Exclamation_Object).gameObject.SetActive(true);
        My_Anim.SetTrigger(AnimHash);
    }

    private void Hide_Chase_Mark()
    {
        GetObject((int)GameObjects.Exclamation_Object).gameObject.SetActive(false);
    }

    private void Billboard_UI()
    {
        if (null == My_Cam)
            My_Cam = PlayerManager.cameraManager.camTransform;

        Util.Billbord_UI(transform, My_Cam.transform);
    }

    private void Awake()
    {
        init();
    }

    private void LateUpdate()
    {
        transform.localPosition = Vector3.zero;
        Billboard_UI();
    }
}
