using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    public override void Clear()
    {
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Title;
    }

}
