using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIContentsManager
{
    //사용할려는 친구들 모두 여기다가 캐싱해놓고 사용

    private Dictionary<Type, UnityEngine.Object> m_Componets = new Dictionary<Type, UnityEngine.Object>();

    private Dictionary<Type, UnityEngine.Object> m_DonDestroyComponets = new Dictionary<Type, UnityEngine.Object>();

    private Dictionary<GameObject, UnityEngine.Object> m_GetPool = new Dictionary<GameObject, UnityEngine.Object>();

    private Skill_Inventory SkillInven_Instance = null;

    public Skill_Inventory Skill_Inven { get { Skill_Inven_Init(); return SkillInven_Instance; } }

    public void SceneUI_Init()
    {
        m_Componets.Add(typeof(UI_Scene_Heart), GameManager.UI.ShowSceneUI<UI_Scene_Heart>("Heart_Canvas"));
        m_Componets.Add(typeof(UI_Scene_Interaction), GameManager.UI.ShowSceneUI<UI_Scene_Interaction>("Interaction_Canvas"));
        m_Componets.Add(typeof(UI_Shop_Information), GameManager.UI.ShowSceneUI<UI_Shop_Information>("Shop_Canvas", false));
        m_Componets.Add(typeof(UI_Scene_Boss_Hp), GameManager.UI.ShowSceneUI<UI_Scene_Boss_Hp>("Boss_Monster_Hp_Canvas", false));
        m_Componets.Add(typeof(UI_Scene_BoxOpenItem), GameManager.UI.ShowSceneUI<UI_Scene_BoxOpenItem>("BoxOpenItem_Canvas", false));
        m_Componets.Add(typeof(UI_Scene_Quest_Title), GameManager.UI.ShowSceneUI<UI_Scene_Quest_Title>("Scene_Quest_Title_Canvas", false));
        m_Componets.Add(typeof(UI_Scene_Skill_QuickSlot), GameManager.UI.ShowSceneUI<UI_Scene_Skill_QuickSlot>("Skill_QuickSlot_Canvas_Two", false));
        m_Componets.Add(typeof(UI_Scene_Skill_Inven), GameManager.UI.ShowSceneUI<UI_Scene_Skill_Inven>("Skill_Inven_Canvas", false));
        m_Componets.Add(typeof(UI_Scene_region_Title), GameManager.UI.ShowSceneUI<UI_Scene_region_Title>("Region", false));
    }

    public void PoolUI_Init()
    {
        GameManager.ObjectPool.CreatePool("Monster_Mark_Canvas", GameManager.Resources.GetPrefab("UI/World/Monster_Mark_Canvas"));
        GameManager.ObjectPool.CreatePool("Monster_Hp_Canvas", GameManager.Resources.GetPrefab("UI/World/Monster_Hp_Canvas"));
    }

    public void UI_Contents_Scene_Init()
    {
        if (null != DonDestroyGet<UI_MainPopUp>())
            return;

        m_DonDestroyComponets.Add(typeof(UI_MainPopUp), GameManager.UI.Show_DonDestroy_SceneUI<UI_MainPopUp>("MainPopUp"));
        m_DonDestroyComponets.Add(typeof(UI_Scene_FadeInOut), GameManager.UI.Show_DonDestroy_SceneUI<UI_Scene_FadeInOut>("FadeInout"));
        m_DonDestroyComponets.Add(typeof(UI_Scene_CrossHair), GameManager.UI.Show_DonDestroy_SceneUI<UI_Scene_CrossHair>("CrossHair"));


        UnityEngine.Object.DontDestroyOnLoad(DonDestroyGet<UI_MainPopUp>());
        UnityEngine.Object.DontDestroyOnLoad(DonDestroyGet<UI_Scene_FadeInOut>());
        UnityEngine.Object.DontDestroyOnLoad(DonDestroyGet<UI_Scene_CrossHair>());

        PoolUI_Init();

        PrivateInit();
    }

    private void PrivateInit()
    {
        DonDestroyGet<UI_MainPopUp>().init();
        DonDestroyGet<UI_Scene_FadeInOut>().init();
        DonDestroyGet<UI_Scene_CrossHair>().init();
    }

    private void Skill_Inven_Init()
    {
        if (null != SkillInven_Instance)
            return;

        Skill_Inventory TempInven = UnityEngine.Object.FindObjectOfType<Skill_Inventory>();

        if (null == TempInven)
        {
            GameObject Inven_Object = new GameObject("@Skill_Inventory");
            SkillInven_Instance = Util.GetOrAddComponent<Skill_Inventory>(Inven_Object);

        }
        else
            SkillInven_Instance = TempInven;

        SkillInven_Instance.Init();
        UnityEngine.Object.DontDestroyOnLoad(SkillInven_Instance);
    }


    private T Get<T>() where T : UnityEngine.Object
    {
        UnityEngine.Object TempObj = null;

        if (false == m_Componets.TryGetValue(typeof(T), out TempObj))
            return null;

        return TempObj as T;
    }

    private T DonDestroyGet<T>() where T : UnityEngine.Object
    {
        UnityEngine.Object TempObj = null;

        if (false == m_DonDestroyComponets.TryGetValue(typeof(T), out TempObj))
            return null;

        return TempObj as T;
    }


    private T Get_UI_Pool<T>(GameObject _thisObject, string _strPoolName) where T : Component
    {
        UnityEngine.Object TempObject = null;

        if (false == m_GetPool.TryGetValue(_thisObject, out TempObject))
        {
            T GetObject = GameManager.ObjectPool.Get_Object(_strPoolName).GetComponent<T>();

            if (null == GetObject)
                return null;

            m_GetPool.Add(_thisObject, GetObject);

            return GetObject;
        }
        else
            return TempObject as T;

    }

    public void All_Hide_Scene_UI()
    {
        foreach (var iter in m_Componets)
        {
            GameObject Object = iter.Value as GameObject;
            Object.SetActive(false);
        }
    }
    public void All_Show_Scene_UI()
    {
        foreach (var iter in m_Componets)
        {
            GameObject Object = iter.Value as GameObject;
            Object.SetActive(true);
        }
    }

    /*Player Hp*/
    public void Player_Health_Update(int _iHp, int _iMaxHp)
    {
        Get<UI_Scene_Heart>().Update_Heart_UI(_iHp, _iMaxHp);
    }
    public void Player_HealthUp(int _iHealthUp)
    {
        //임시용 지울꺼 플레이어 hp업데이트들어오면
        Get<UI_Scene_Heart>().HealthUp(_iHealthUp);
    }

    public void SetActive_Health(bool _bActive)
    {
        Get<UI_Scene_Heart>().SetActive_Health(_bActive);
    }

    /*SELECT_INTERACTION*/
    public void Show_Interaction_UI(UI_Scene_Interaction.Interaction_Type _eType)
    {
        Get<UI_Scene_Interaction>().Show_Interaction_UI(_eType);
    }
    public void Show_Select_Interactio_UI()
    {
        Get<UI_Scene_Interaction>().Show_Select_Interactio_UI();
    }
    public void Hide_Interaction_UI()
    {
        Get<UI_Scene_Interaction>().Hide_Interaction_UI();
    }

    /*SHOP_INFORMATION*/
    public void Show_Shop_Information(ItemData _ItemData)
    {
        Get<UI_Shop_Information>().Show_Shop_Information(_ItemData);
    }
    public void Hide_Shop_Information()
    {
        Get<UI_Shop_Information>().Hide_Shop_Information();
    }

    /*MAIN_POPUP*/
    public void Open_MainPopUpCanvas(UI_MainPopUp.CavasType _eType)
    {
        if (false == DonDestroyGet<UI_MainPopUp>().bIsOpening)
            DonDestroyGet<UI_MainPopUp>().OpenCanvas(_eType);
        else
            Close_MainPopUpCanvas();
    }
    public void Close_MainPopUpCanvas()
    {
        DonDestroyGet<UI_MainPopUp>().CloseCanvas();
    }

    /*MONSTER_MARK*/
    public void Show_Monster_Mark(GameObject _ThisObject, UI_World_Monster_Mark.Monster_State _eType, Transform _transform)
    {
        if (null == _ThisObject || null == _transform)
            return;

        Get_UI_Pool<UI_World_Monster_Mark>(_ThisObject, "Monster_Mark_Canvas").Show_Monster_Mark(_eType, _transform);
    }
    public void Hide_Monse_Mark(GameObject _thisObject)
    {
        if (null == _thisObject)
            return;

        UnityEngine.Object TempObject = null;

        if (false == m_GetPool.TryGetValue(_thisObject, out TempObject))
            return;

        if (TempObject is UI_World_Monster_Mark Mark)
        {
            m_GetPool.Remove(_thisObject);

            Mark.Hide_Monster_Mark();
            Mark.transform.SetParent(null);
            GameManager.ObjectPool.RelaseObject("Monster_Mark_Canvas", Mark.gameObject);
        }
    }

    /*MONSTER_HP*/
    public void Show_Monster_Hp(GameObject _thisObject, Transform _transform, int _iHp, int _iMaxHp)
    {
        if (null == _thisObject || null == _transform)
            return;

        _iHp = Mathf.Max(0, _iHp);
        _iMaxHp = Mathf.Max(0, _iMaxHp);

        Get_UI_Pool<UI_World_Monster_Hp>(_thisObject, "Monster_Hp_Canvas").Monster_HpUpdate(_iHp, _iMaxHp, _transform);
    }
    public void Hide_Monster_Hp(GameObject _thisObject)
    {
        if (null == _thisObject)
            return;

        UnityEngine.Object TempObject = null;

        if (false == m_GetPool.TryGetValue(_thisObject, out TempObject))
            return;

        if (TempObject is UI_World_Monster_Hp MonsterHp)
        {
            m_GetPool.Remove(_thisObject);

            MonsterHp.Init_Monster_Hp();
            MonsterHp.transform.SetParent(null);

            GameManager.ObjectPool.RelaseObject("Monster_Hp_Canvas", MonsterHp.gameObject);
        }
    }

    /*BOSS_MONSTER_HP*/
    public void Show_BossMonster_Hp(UI_Scene_Boss_Hp.Monster_Type _eType, int _iHp, int _iMaxHp)
    {
        Get<UI_Scene_Boss_Hp>().Show_Text(_eType, _iHp, _iMaxHp);
    }
    public void Hide_BossMonster_Hp()
    {
        Get<UI_Scene_Boss_Hp>().HideText();
    }


    /*BOXOPEN_ITEM*/
    public void Show_BoxOpenInfomation(ItemData _ItemData)
    {
        Get<UI_Scene_BoxOpenItem>().Show_BoxOpenInfomation(_ItemData);
    }


    /*Scene_Quest_Title*/
    public void Show_Quest_Title(string _strQuestName)
    {
        Get<UI_Scene_Quest_Title>().Show_Quest_Title(_strQuestName);
    }



    /*Scene_Quick_Slot*/

    public UI_Scene_Skill_QuickSlot Get_QuickSlot_UI()
    {
        return Get<UI_Scene_Skill_QuickSlot>();
    }

    /*Scene_Skill_Inven*/

    public void Skill_Inven_Setting_Image(Skill _skill)
    {
        Get<UI_Scene_Skill_Inven>().Setting_Image(_skill);
    }

    public void Skill_Inven_NewSetting(Skill _skill)
    {
        Get<UI_Scene_Skill_Inven>().NewSetting(_skill);
    }

    public bool Skill_Inven_Add(Skill.Skill_Type _eType)
    {
        return Get<UI_Scene_Skill_Inven>().Skill_Inven_Add(_eType);
    }

    /*UI_Scene_FadeInOut*/

    public void Fade_In(Action _Function = null)
    {
        DonDestroyGet<UI_Scene_FadeInOut>().FadeIn(_Function);
    }

    public void Fade_Out(Action _Function = null)
    {
        DonDestroyGet<UI_Scene_FadeInOut>().FadeOut(_Function);
    }

    public void Fade_Init()
    {
        DonDestroyGet<UI_Scene_FadeInOut>()?.FadeInit();
    }

    /*UI_Scene_CrossHair*/

    public void Region_Update(Region_Base.Region_Type _eType)
    {
        Get<UI_Scene_region_Title>().Update_Region_UI(_eType);
    }

    public void CrossHairOn()
    {
        DonDestroyGet<UI_Scene_CrossHair>()?.OnCrossHair();
    }

    public void CrossHairOff()
    {
        DonDestroyGet<UI_Scene_CrossHair>()?.OffCrossHair();
    }

    public void Clear()
    {
        m_Componets.Clear();
    }

}
