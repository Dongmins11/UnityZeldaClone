using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        GameManager.UIContents.SceneUI_Init();
        GameManager.UIContents.UI_Contents_Scene_Init();

        if (null == FindObjectOfType<PlayerManager>())
        {
            Transform TempTrans = GameObject.FindGameObjectWithTag("PlayerPositionSet").transform;

            GameObject PlayerObject = GameManager.Resources.CreatePrefab("Player/Player", TempTrans.position, TempTrans.rotation);
            Util.ObjectCloneNameRemove(PlayerObject);
            DontDestroyOnLoad(PlayerObject);

            GameManager.Resources.CreatePrefab("Monster/WayPoint"); 
        }
        else
        {
            GameManager.Creature.Respawn();

            PlayerDefaultPositionSet();
        }


        PlayerManager.ResetPlayerSetting();
        GameManager.UIContents.Fade_Out();
    }

    private void PlayerDefaultPositionSet()
    {
        Transform TempTrans = null;

        switch (GameManager.Scene.PreSceneIndex)
        {
            case Define.Scene.Dungeon_1:
                TempTrans = GameObject.Find("WayPoint_Dgn_1")?.transform;
                break;
            case Define.Scene.Dungeon_2:
                TempTrans = GameObject.Find("WayPoint_Dgn_2")?.transform;
                break;
        }

        if (null != TempTrans)
        {
            PlayerManager.rigidbody.velocity = Vector3.zero;
            PlayerManager.rigidbody.transform.position = TempTrans.position;
            PlayerManager.rigidbody.transform.rotation = TempTrans.rotation;
        }
    }

    public override void Clear()
    {
        //Debug.Log("Clear");
    }


}
