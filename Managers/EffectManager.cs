using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager
{
	Dictionary<string, GameObject> Effect_Dictionary = new Dictionary<string, GameObject>();

	bool IsInit = false;

	public void Init()
	{
		if (true == IsInit)
			return;

		IsInit = true;

		GameObject[] Effects = GameManager.Resources.GetPrefabs<GameObject>("Effect");

		for (int i = 0; i < Effects.Length; ++i)
		{
			Effects[i].GetOrAddComponent<Effect_Event>();
			Effect_Dictionary.Add(Effects[i].name, Effects[i]);
			GameManager.ObjectPool.CreatePool(Effects[i].name, Effects[i], 5);
		}
	}

	public GameObject GetEffect(string _strEffectName, Transform _Parent = null)
	{
		Init();

		GameObject EffectObject = GameManager.ObjectPool.Get_Object(_strEffectName);

		if (null != _Parent)
			EffectObject.transform.SetParent(_Parent);

		return EffectObject;
	}

	public GameObject GetEffect(string _strEffectName, Vector3 _vecPosition)
	{
		Init();

		GameObject EffectObject = GameManager.ObjectPool.Get_Object(_strEffectName);

		EffectObject.transform.position = _vecPosition;

		return EffectObject;
	}


	public void ReleaseEffect(string _strEffectName, GameObject _EffectObject)
	{
		GameManager.ObjectPool.RelaseObject(_strEffectName, _EffectObject);
	}

}
