using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class UI_Scene_Heart : UI_Base
{
    List<Image> HeartList = new List<Image>();
    List<Sprite> HeartImageList = new List<Sprite>();

    const int iDefaultValue = 25;
    const int iSectionMaxValue = 100;

    int iTopIndex = -1;
    int iMaxIndex = 0;

    int iHp = 400;
    int iMaxHp = 400;

    public override void init()
    {
        GameManager.UI.SortingOrder(this.gameObject, 2);

        HeartObjectInit();
        HeartSpriteInit();

        if (0 != HeartList.Count)
            iMaxIndex = HeartList.Count;

        if(null != PlayerManager.playerData)
            Update_Heart_UI(PlayerManager.playerData.iHp, PlayerManager.playerData.iMaxHp);
    }

    private void HeartObjectInit()
    {
        foreach (Image iter in gameObject.GetComponentsInChildren<Image>())
        {
            HeartList.Add(iter);
            ++iTopIndex;
        }
    }
    private void HeartSpriteInit()
    {
        Sprite[] TempSprites = GameManager.Resources.LoadAll<Sprite>("Textures/UI/InGameUI_Texture/Heart");

        foreach (Sprite iter in TempSprites)
            HeartImageList.Add(iter);
    }
    private bool IsEmpty(int _iIndex)
    {
        
        if (-1 == _iIndex)
            return false;

        return true;
    }
    private int MinMaxCheckHp(int _iHp, int _iMaxHp)
    {
        if (0 >= _iHp)
            _iHp = 0;
        else if (_iMaxHp <= _iHp)
            _iHp = _iMaxHp;

        return _iHp;
    }

    public void HealthUp(int _iHealthUp)
    {
        MasterAudio.PlaySound("Eating");
        PlayerManager.playerData.iHp = Mathf.Min(PlayerManager.playerData.iMaxHp, PlayerManager.playerData.iHp + _iHealthUp);
        Update_Heart_UI(PlayerManager.playerData.iHp, PlayerManager.playerData.iMaxHp);
        MasterAudio.PlaySound("HeartUp",2f);
    }

    public void SetActive_Health(bool _bActive)
    {
        gameObject.SetActive(_bActive);
    }

    public void Update_Heart_UI(int _iHp, int _iMaxHp)
    {
        if (false == IsEmpty(iTopIndex))
            return;

        _iHp = MinMaxCheckHp(_iHp, _iMaxHp);

        int healthPerSegment = _iMaxHp / HeartList.Count / (HeartImageList.Count - 1);

        int heelthCount = _iHp / iDefaultValue;
        int heelthMaxCount = _iMaxHp / iDefaultValue;

        int HeartCount = _iHp / iDefaultValue / HeartList.Count;

        int FullCurHpIndex = 0;

        for (int i = 0; i < HeartCount; ++i)
        {
            ++FullCurHpIndex;
            HeartList[i].sprite = HeartImageList[4];
        }

        if (0 < heelthCount && heelthMaxCount > heelthCount)
            HeartList[FullCurHpIndex++].sprite = HeartImageList[(_iHp % iSectionMaxValue) / healthPerSegment];

        for (int i = FullCurHpIndex; i < HeartList.Count; ++i)
            HeartList[i].sprite = HeartImageList[0];
    }


    private void Start()
    {
        init();
    }

}

