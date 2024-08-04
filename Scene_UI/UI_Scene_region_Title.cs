using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UI_Scene_region_Title : UI_Base
{
    enum Images
    {
        Region_Image,
    }

    Dictionary<string, Sprite> My_Sprites;

    Region_Base.Region_Type Cur_Type = Region_Base.Region_Type.Unknow;
    Region_Base.Region_Type Pre_Type = Region_Base.Region_Type.End;

    IEnumerator Fade_Funtion;
    IEnumerator FadeOut_Funtion;

    public override void init()
    {
        Bind<Image>(typeof(Images));

        My_Sprites = new Dictionary<string, Sprite>();

        GetImage((int)Images.Region_Image).color = new Color(1f, 1f, 1f, 0f);

        Sprite_Load();

        Fade_Funtion = Fade_InOut(true);
        FadeOut_Funtion = Fade_InOut(false);
    }

    private void Sprite_Load()
    {
        Sprite[] TempSprites = GameManager.Resources.LoadAll<Sprite>("Textures/UI/RegionTexture");

        for (int i = 0; i < TempSprites.Length; ++i)
            My_Sprites.Add(TempSprites[i].name, TempSprites[i]);
    }

    private void SwitchSpriteTexture(Region_Base.Region_Type _eType)
    {
        Cur_Type = _eType;

        if (Pre_Type != Cur_Type)
        {
            Pre_Type = Cur_Type;

            Sprite TempSprite = null;

            switch (_eType)
            {
                case Region_Base.Region_Type.Hiral:
                    My_Sprites.TryGetValue("Hyrule", out TempSprite);
                    break;
                case Region_Base.Region_Type.Korg_Forest:
                    My_Sprites.TryGetValue("Corce", out TempSprite);
                    break;
                case Region_Base.Region_Type.Kakirico_Village:
                    My_Sprites.TryGetValue("Kakiriko", out TempSprite);
                    break;
                case Region_Base.Region_Type.Linell_Province:
                    My_Sprites.TryGetValue("Rinel", out TempSprite);
                    break;
                case Region_Base.Region_Type.Ma_Ouns:
                    My_Sprites.TryGetValue("Maonue", out TempSprite);
                    break;
                case Region_Base.Region_Type.Tummi_Mke:
                    My_Sprites.TryGetValue("TomiUmke", out TempSprite);
                    break;
                case Region_Base.Region_Type.Naboris:
                    My_Sprites.TryGetValue("Naboris", out TempSprite);
                    break;
            }

            if (null != TempSprite)
                GetImage((int)Images.Region_Image).sprite = TempSprite;

            Fade_InOut();
        }
    }

    public void Fade_InOut()
    {
        StartCoroutine(Fade_InOut(true));
    }

    private IEnumerator Fade_InOut(bool isFadeIn)
    {
        if (isFadeIn)
        {
            Color TempColor = GetImage((int)Images.Region_Image).color;
            TempColor.a = 0f;
            GetImage((int)Images.Region_Image).color = TempColor;
            Tween tween = GetImage((int)Images.Region_Image).DOFade(1f, 1f);
            yield return tween.WaitForCompletion();

            StartCoroutine(Fade_InOut(false));
        }
        else
        {
            Color TempColor = GetImage((int)Images.Region_Image).color;
            TempColor.a = 1f;
            GetImage((int)Images.Region_Image).color = TempColor;
            GetImage((int)Images.Region_Image).gameObject.SetActive(true);
            Tween tween = GetImage((int)Images.Region_Image).DOFade(0f, 2f);
            yield return tween.WaitForCompletion();
        }
    }


    public void Update_Region_UI(Region_Base.Region_Type _eType)
    {
        if (false == gameObject.activeSelf)
            gameObject.SetActive(true);

        SwitchSpriteTexture(_eType);
    }
}

