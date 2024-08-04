using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class UI_Scene_BoxOpenItem : UI_Base
{

    enum GameObjects
    {
        BoxOpenItem_Canvas,
        BoxOpenItem_Object,
        Arrow_Object,
    }

    enum Texts
    {
        Itme_TitleText,
        Itme_InfoText,
    }

    enum Images
    {
        ItemImage,
    }

    enum OpenType
    {
        Open,
        Close,
        End,
    }

    OpenType Cur_Type = OpenType.Open;
    OpenType Pre_Type = OpenType.End;

    public bool bIsOpen { get; private set; } = false;

    int AnimTriggerHash = Animator.StringToHash("Box_OpenTrigger");
    int ArrowTriggerHash = Animator.StringToHash("Arrow_Trigger");

    Animator My_Anim;
    Animator Arrow_Anim;

    public override void init()
    {

        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        ComponentInit();

        Hide_ArrowImages();
    }

    private void ComponentInit()
    {
        My_Anim = gameObject.GetComponent<Animator>();
        Arrow_Anim = GetObject((int)GameObjects.Arrow_Object).GetComponent<Animator>();
    }

    public void TextEndCheck_EventCall()
    {
        Cur_Type = OpenType.Close;
        StartCoroutine(Hide_BoxInfo());
        Show_ArrowImages();
    }

    private void Show_Images(ItemData _ItemData)
    {
        GetImage((int)Images.ItemImage).sprite = GameManager.Resources.Get_ItemSprite(_ItemData.strTexturePath);
    }
    private void Hide_Images()
    {
        GetImage((int)Images.ItemImage).sprite = null;
    }

    private void Show_ArrowImages()
    {
        GetObject((int)GameObjects.Arrow_Object).SetActive(true);
        Arrow_Anim.SetTrigger(ArrowTriggerHash);
    }

    private void Hide_ArrowImages()
    {
        GetObject((int)GameObjects.Arrow_Object).SetActive(false);
    }

    private void Show_Texts(ItemData _ItemData)
    {
        My_Anim.SetTrigger(AnimTriggerHash);

        GetText((int)Texts.Itme_TitleText).text = _ItemData.strname;
        GetText((int)Texts.Itme_InfoText).text = _ItemData.strToolTip;

    }
    private void Hide_Texts()
    {
        GetText((int)Texts.Itme_InfoText).text = "";
        GetText((int)Texts.Itme_TitleText).text = "";
    }

    private void Show_BoxOpen_Information(ItemData _ItemData)
    {
        MasterAudio.PlaySound("BoxOpen");
        gameObject.SetActive(true);

        Show_Images(_ItemData);
        Show_Texts(_ItemData);
    }

    private IEnumerator Hide_BoxInfo()
    {
        while (true)
        {
            yield return null;

            if(Input.GetKeyDown(KeyCode.E))
            {
                Hide_BoxOpen_Information();
                yield break;
            }
        }
    }

    private void Hide_BoxOpen_Information()
    {
        gameObject.SetActive(false);

        Cur_Type = OpenType.Open;
        Pre_Type = OpenType.End;

        Hide_Images();
        Hide_Texts();
        Hide_ArrowImages();
    }

    public void Show_BoxOpenInfomation(ItemData _ItemData)
    {
        if(Cur_Type != Pre_Type)
        {
            Pre_Type = Cur_Type;

            switch (Cur_Type)
            {
                case OpenType.Open:
                    Show_BoxOpen_Information(_ItemData);
                    break;
                case OpenType.Close:
                    break;
            }
        }
    }


}
