using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager m_Instance = null;

    CreatureManager     m_CreaturInstance       = new CreatureManager();
    InputManager        m_InputInstance         = new InputManager();
    ResourceManager     m_ResoureceInstance     = new ResourceManager();
    ObjectPoolManager   m_ObjectPoolInstance    = new ObjectPoolManager();
    UiManager           m_UiInstance            = new UiManager();
    SceneManagerEx      m_SceneManager          = new SceneManagerEx();
    SoundManager        m_SoundManager          = new SoundManager();
    TimeManager         m_TimeManager           = new TimeManager();
    DataManager         m_DataManager           = new DataManager();
    ItemManager         m_ItemManager           = new ItemManager();
    UIContentsManager   m_UIContentsInstance    = new UIContentsManager();
    DialogueManager     m_DialogueInstance      = new DialogueManager();
    EffectManager       m_EffectInstance        = new EffectManager();
    QuestManager        m_QuestInstance         = new QuestManager();
    GameContentsManager m_GameContensInstance   = new GameContentsManager();

    public static CreatureManager   Creature    { get { return Instance.m_CreaturInstance; } }
    public static InputManager      Input       { get { return Instance.m_InputInstance; } }
    public static ResourceManager   Resources   { get { return Instance.m_ResoureceInstance; } }
    public static ObjectPoolManager ObjectPool  { get { return Instance.m_ObjectPoolInstance; } }
    public static UiManager         UI          { get { return Instance.m_UiInstance; } }
    public static SceneManagerEx    Scene       { get { return Instance.m_SceneManager; } }
    public static SoundManager      Sound       { get { return Instance.m_SoundManager; } }
    public static TimeManager       Time        { get { return Instance.m_TimeManager; } }
    public static DataManager       Data        { get { return Instance.m_DataManager; } }
    public static ItemManager       item        { get { return Instance.m_ItemManager; } }
    public static UIContentsManager UIContents  { get { return Instance.m_UIContentsInstance; } }
    public static DialogueManager   Dialogue    { get { return Instance.m_DialogueInstance; } }
    public static EffectManager     Effect      { get { return Instance.m_EffectInstance; } }
    public static QuestManager      Quest       { get { return Instance.m_QuestInstance; } }
    public static GameContentsManager GameContens       { get { return Instance.m_GameContensInstance; } }


    private static bool IsInit = false;

    static public GameManager Instance 
    {
        get 
        {
            InitInstance();
            return m_Instance;
        }
    }

    static void InitInstance()
    {
        if (null != m_Instance || true == IsInit)
            return;

        IsInit = true;

        GameObject TempObject = GameObject.Find("@GameManager");

        if(null == TempObject)
        {   
            GameObject GameManager_Object = new GameObject("@GameManager");

            m_Instance = GameManager_Object.AddComponent<GameManager>();

            m_Instance.m_DataManager.Init();

            m_Instance.m_SoundManager.Init();

            m_Instance.m_ItemManager.Init();

            m_Instance.m_DialogueInstance.Init();

            m_Instance.m_QuestInstance.Init();
        }
        else
            m_Instance = TempObject.GetComponent<GameManager>();

        DontDestroyOnLoad(m_Instance);
    }


    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        Creature.Clear();
        Scene.Clear();
        UI.Clear();
        UIContents.Clear();
        GameContens.Clear();

        ObjectPool.AllClear();
    }


    private void FixedUpdate()
    {
        Input?.InputFiexdOnUpdate();
    }

    void Update()
    {
        Input?.InputOnUpdate();
    }
}
