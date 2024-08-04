using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Scene : BaseScene
{

    public override void Clear()
    {
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Boss;

        GameManager.UIContents.SceneUI_Init();
        GameManager.UIContents.UI_Contents_Scene_Init();

        if (null == FindObjectOfType<PlayerManager>())
        {
            Transform TempTr = GameObject.FindGameObjectWithTag("PlayerPositionSet").transform;
            GameObject PlayerObject = GameManager.Resources.CreatePrefab("Player/Player", TempTr.position, TempTr.rotation);
            Util.ObjectCloneNameRemove(PlayerObject);
            DontDestroyOnLoad(PlayerObject);

            GameManager.Resources.CreatePrefab("Monster/BossWayPoint");
        }
        else
        {
            Transform TempTrans = GameObject.FindGameObjectWithTag("PlayerPositionSet").transform;
            if (null != TempTrans)
            {
                PlayerManager.rigidbody.velocity = Vector3.zero;
                PlayerManager.rigidbody.transform.position = TempTrans.position;
                PlayerManager.rigidbody.transform.rotation = TempTrans.rotation;

                GameManager.Resources.CreatePrefab("Monster/BossWayPoint");
            }
        }

        PlayerManager.ResetPlayerSetting();
        GameManager.UIContents?.Fade_Out();
    }
}
