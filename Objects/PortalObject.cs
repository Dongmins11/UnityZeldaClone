using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using INab.Dissolve;

public class PortalObject : MonoBehaviour, InteractiveObject
{
    public Define.Scene Scene = Define.Scene.Unknown;

    GameObject My_VfxPortal = null;

    private void Start()
    {
        StartCoroutine(MaterializeEnter());

        My_VfxPortal = Util.FindChild(gameObject.transform.parent.gameObject, "Vfx_Portal");
        My_VfxPortal.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(null != My_VfxPortal && false == My_VfxPortal.activeSelf)
                My_VfxPortal.SetActive(true);

            GameManager.UIContents.Show_Interaction_UI(UI_Scene_Interaction.Interaction_Type.Enter_E);
        }
    }

    public void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            if (null != My_VfxPortal && true == My_VfxPortal.activeSelf)
                My_VfxPortal.SetActive(false);

            GameManager.UIContents.Hide_Interaction_UI();
        }
    }


    public void OnPlayerInteracting(PlayerAnimator _playerAnimator, InteractiveObject _interactObject)
    {
        if (!(_interactObject.Equals(this))) return;

        GameManager.UIContents.Show_Select_Interactio_UI();
        Monster.PlayerEnterTheShirne.Invoke();

        PlayerManager TempManager = _playerAnimator.GetComponent<PlayerManager>();
        if (null != TempManager)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            TempManager.InteractingObject = null;
            TempManager.iInteractive = null;
        }

        GameManager.UIContents.Fade_In(FuntionCall);
    }

    private IEnumerator MaterializeEnter()
    {
        Dissolver TempDissolver = gameObject.GetComponent<Dissolver>();
        TempDissolver.Materialize();

        while (Dissolver.DissolveState.Materialized != TempDissolver.currentState)
            yield return null;

        yield break;
    }

    private void FuntionCall()
    {
        if (Define.Scene.Unknown == Scene)
            return;

        PlayerManager Manager = GameObject.FindWithTag("Player")?.GetComponent<PlayerManager>();

        if (null != Manager)
            Manager.SetInteractingObject(null);

        GameManager.Scene.LoadToNextScene(Scene);
    }
}
