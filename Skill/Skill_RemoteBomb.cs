using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using DarkTonic.MasterAudio;

public class Skill_RemoteBomb : SkillBase
{
    public static UnityEvent<GameObject> DestroyedBomb = new UnityEvent<GameObject>();

    private List<GameObject> bombPool = new List<GameObject>();
    private const int iPoolSize = 2;
    List<GameObject> BombaAtiveInHierarchyList = new List<GameObject>();
    Transform Bomb_InitTr;
    Rigidbody bombRigidbody;
    GameObject currentBomb;
    private bool bSkillactive = false;
    private bool bInputR = false;

    void Start()
    {
        Bomb_InitTr = Util.FindChildWithName(transform, "Remote_Hold");
        GameManager.ObjectPool.CreatePool("bombPool", GameManager.Resources.GetPrefab("Skills/RemoteBomb"), iPoolSize);
    }

    void Update()
    {
        if (false == bSkillactive && true == bInputR)
        {
            bInputR = false;
            bSkillactive = true;
            holdRemoteBomb();
        }
        else if (true == bSkillactive && true == bInputR)
        {
            bInputR = false;
            bSkillactive = false;
            DestroyedBomb.Invoke(currentBomb);
        }
    }

	private void OnEnable()
	{
        DestroyedBomb.AddListener(DestroyLastBomb);
    }
	private void OnDisable()
	{
        DestroyedBomb.RemoveAllListeners();

    }

    public override void InputActive()
    {
        
        bInputR = true;
    }

    public override bool GetSkillActive()
    {
        return bSkillactive;
    }

    void holdRemoteBomb()
    {
        MasterAudio.PlaySound("RemoteBomb3");
        currentBomb = GameManager.ObjectPool.Get_Object("bombPool");
        BombaAtiveInHierarchyList.Add(currentBomb);
        currentBomb.transform.parent = Bomb_InitTr;

        currentBomb.transform.localPosition = Vector3.zero;
        currentBomb.transform.localRotation = Quaternion.identity;

        if (null == currentBomb)
        {
            Debug.LogWarning("Bomb pool exhausted. Consider increasing the pool size.");
            return;
        }

    }

    void DestroyLastBomb(GameObject thisbomb)
    {
        if (thisbomb != currentBomb) return;

        foreach (GameObject RemoteBomb in BombaAtiveInHierarchyList)
        {
            MasterAudio.PlaySound("RemoteBomb1");
            GameManager.Effect.GetEffect("StylisedBomb", RemoteBomb.transform.position);
            GameManager.ObjectPool.RelaseObject("bombPool", RemoteBomb);
        }

        BombaAtiveInHierarchyList.Clear();
        PlayerManager.weaponManager.ResetUseSkill();
    }

    public void DestroyBombAfterDelay(GameObject RemoteBomb, float delay)
    {
        StartCoroutine(DestroyBombCoroutine(RemoteBomb, delay));
    }

    IEnumerator DestroyBombCoroutine(GameObject RemoteBomb, float delay)
    {
        
        yield return new WaitForSeconds(delay);
        GameManager.ObjectPool.RelaseObject("bombPool", RemoteBomb);
    }
}