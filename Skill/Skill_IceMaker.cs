using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class Skill_IceMaker : SkillBase
{
    private bool isMakingMode = false;

    private GameObject iceBlockPrefab;
    private float growthRate = 1f;
    private List<GameObject> iceBlocks = new List<GameObject>();
    private bool bInputActive = false;

    private bool bIsRenderMode = false;
    private float fYRenderValue = 0f;
    private float fDefalutYRenderValue = -4f;
    private float fRenderSpeedValue = 4f;
    private float fCreateSpeed = 2f;

    Transform My_CamTransform = null;
    GameObject TempRender_IceMaker = null;
    Vector3 BoxCastSize = Vector3.zero;

    [SerializeField]
    private float _maxDistance = 20.0f;

    void Start()
    {
        iceBlockPrefab = GameManager.Resources.GetPrefab("Skills/IceBlock");
        TempRender_IceMaker = GameManager.Resources.CreatePrefab("Skills/IceBlock_Render");
        TempRender_IceMaker.transform.position = transform.position;
        TempRender_IceMaker.SetActive(false);
        BoxCastSize = TempRender_IceMaker.transform.localScale;
        bIsRenderMode = false;

        GameManager.ObjectPool.CreatePool("IceBlock", iceBlockPrefab, 3);

        DontDestroyOnLoad(TempRender_IceMaker);
        Find_Cam();
    }

    void Find_Cam()
    {
        if (null == My_CamTransform)
            My_CamTransform = Camera.main.transform;
    }

    void Update()
    {
        IceMaker_SkillOn();
    }

    void initIceMaker()
    {
        fYRenderValue = fDefalutYRenderValue;
        bIsRenderMode = true;
        TempRender_IceMaker.SetActive(true);
        MasterAudio.PlaySound3DAtTransform("IceMaker1", transform);
    }

    void ResetIceMaker()
    {
        bIsRenderMode = false;
        fYRenderValue = fDefalutYRenderValue;
        TempRender_IceMaker.transform.position = Vector3.zero;
        TempRender_IceMaker.SetActive(false);
    }

    void IceMaker_RenderMode()
    {
        Find_Cam();

        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, _maxDistance, 1 << LayerMask.GetMask("Water")))
        {
            if (false == bIsRenderMode)
                initIceMaker();

            Vector3 HitPoint = hit.point;

            if (0 >= fYRenderValue)
                fYRenderValue += Time.deltaTime * fRenderSpeedValue;
            else
                fYRenderValue = fDefalutYRenderValue;

            HitPoint.y = fYRenderValue;

            TempRender_IceMaker.transform.position = HitPoint;

            if (Input.GetMouseButtonDown(0))
            {
                ResetIceMaker();
                isMakingMode = true;
                PlayerManager.weaponManager.ResetUseSkill();
                StartCoroutine(IceMaker_CreateMode(hit.point));
                MasterAudio.PlaySound3DAtTransform("IceMaker2", transform);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                PlayerManager.weaponManager.ResetUseSkill();

            ResetIceMaker();
        }

    }

    IEnumerator IceMaker_CreateMode(Vector3 _vPosition)
    {
        

        GameObject Block = GameManager.ObjectPool.Get_Object("IceBlock");
        Block.GetComponent<Rigidbody>().mass = 100;
        Block.transform.position = _vPosition;
        Vector3 BackUpScale = Block.transform.localScale;

        float fRatio = 0f;
        float fMaxRatio = 1f;

        while (fMaxRatio >= fRatio)
        {
            fRatio += Time.deltaTime * 2f;

            Vector3 TempScale = Vector3.Lerp(Vector3.zero, BackUpScale, fRatio);

            TempScale.x = BackUpScale.x;
            TempScale.z = BackUpScale.z;

            Block.transform.localScale = TempScale;

            yield return null;
        }

        StartCoroutine(DestroyIceMaker(Block));
        bInputActive = false;
        isMakingMode = false;
    }

    IEnumerator DestroyIceMaker(GameObject _Object)
    {
        yield return new WaitForSeconds(10f);

        GameManager.ObjectPool.RelaseObject("IceBlock", _Object);
    }

    void IceMaker_SkillOn()
    {
        if (true == bInputActive)
        {
            if (false == isMakingMode)
                IceMaker_RenderMode();
        }
    }

    public override void InputActive()
    {
        bInputActive = true;
    }

    public override bool GetSkillActive()
    {
        return isMakingMode;
    }
}