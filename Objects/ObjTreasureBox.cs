using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjTreasureBox : WorldObject, InteractiveObject
{
    private ObjTreasureBox instance;
    public enum BoxType
    {
        WeaponBow036,
        Rupee_Gold,
        WeaponSpear024,
        WeaponSwoard002,
        End,
    }

    bool bIsOpenBox = false;
    int MyTriggerHash = Animator.StringToHash("BoxOpen");

    ItemData MyItemData = null;
    Animator MyAnimtor = null;

    public BoxType eBoxType = BoxType.End;

    private void Awake()
    {
        if (null == instance) instance = this;
    }

    private void Start()
    {
        if (BoxType.End == eBoxType)
            return;

        MyAnimtor = GetComponent<Animator>();

        string TempNanme = System.Enum.GetName(typeof(BoxType), eBoxType);

        MyItemData = GameManager.item.Get_ItemData(TempNanme);
        GameManager.Time.AddObject(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && false == bIsOpenBox)
            GameManager.UIContents.Show_Interaction_UI(UI_Scene_Interaction.Interaction_Type.InterAction_E);
    }

    public void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player") && false == bIsOpenBox)
            GameManager.UIContents.Hide_Interaction_UI();
    }

    public void OnPlayerInteracting(PlayerAnimator _playerAnimator, InteractiveObject _interactObject)
    {
        if (true == bIsOpenBox)
            return;

        if (!(_interactObject.Equals(this))) return;

        bIsOpenBox = true;
        GameManager.UIContents.Show_Select_Interactio_UI();

        if (PlayerManager.playerArmors != null)
        {
            if (PlayerManager.playerArmors.TryGetValue(PlayerArmorValues.Lower, out GameObject lower))
            {
                PlayerManager.animator.PlayerTargetAnimation("Get_Kick_TreasureBox");
                StartCoroutine(ItemBoxOpen());
                return;
            }
        }
        PlayerManager.animator.PlayerTargetAnimation("Get_Kick_Treasurebox_Damage");


        StartCoroutine(ItemBoxOpen());
    }

    private IEnumerator ItemBoxOpen()
    {
        yield return new WaitForSeconds(0.2f);

        MyAnimtor.SetTrigger(MyTriggerHash);

        yield return new WaitForSeconds(1.5f);

        if (null == MyItemData)
            yield break;

        GameManager.UIContents.Show_BoxOpenInfomation(MyItemData);

        if ((int)Item.Inven_Type.Item_Rupee == MyItemData.iInvenType)
            GameManager.item.Inven.Add_Rupee(MyItemData);
        else
            GameManager.item.Inven.Add(MyItemData);
    }
}
