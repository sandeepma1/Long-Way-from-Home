using Bronz.Ui;
using CnControls;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static Action<int> OnPlayerDataLoaded;
    public static Action<int, int> OnPlayerMovedPerGrid;
    public static Action OnPlayerMovedPerMeter;
    public static Action OnPlayerMoved;
    [SerializeField] private GameObject characterGO;
    [SerializeField] private SpriteRenderer playerHead;
    [SerializeField] private SpriteRenderer playerEyes;
    [SerializeField] private SpriteRenderer playerBody;
    [SerializeField] private SpriteRenderer playerLimbLeft;
    [SerializeField] private SpriteRenderer playerLimbRight;
    [SerializeField] private SpriteRenderer playerLegLeft;
    [SerializeField] private SpriteRenderer playerLegRight;
    [SerializeField] private SpriteRenderer playerRightWeapon;
    private static Animator anim;
    public bool isPlayerRunning = false;
    public float speed = 3;
    private int tempPosX, tempPosY, currentPosX, currentPosY;
    public static GameObject player;

    private void Start()
    {
        player = this.gameObject;
        anim = GetComponentInChildren<Animator>();
        MasterSave.RequestSaveData += RequestSaveData;
        LoadPlayerInfoData();
    }

    private void OnDestroy()
    {
        MasterSave.RequestSaveData -= RequestSaveData;
    }

    private void FixedUpdate()
    {
        float moveHorizontal = CnInputManager.GetAxisRaw("Horizontal");
        float moveVertical = CnInputManager.GetAxisRaw("Vertical");
        if (moveHorizontal == 0 && moveVertical == 0)
        {
            anim.SetBool("isWalking", false);
            return;
        }
        WalkingCalculation(moveHorizontal, moveVertical);
        SetSortingOrder();
        OnPlayerMoved?.Invoke();
    }

    #region --Save Load Stuff--
    private void LoadPlayerInfoData()
    {
        PlayerSavePlayerInfo playerSavePlayerInfo = MasterSave.LoadPlayerInfo();
        transform.position = new Vector2(playerSavePlayerInfo.posX, playerSavePlayerInfo.posY);
        currentPosY = (int)transform.position.y;
        OnPlayerDataLoaded?.Invoke(playerSavePlayerInfo.lastSelectedInventorySlotId);
        SetSortingOrder();
    }

    private void RequestSaveData()
    {
        MasterSave.SavePlayerInfo(new PlayerSavePlayerInfo(transform.position.x,
            transform.position.y, UiInventory.GetLastClikcedSlotId()));
    }
    #endregion

    public static void SetTriggerAnimation(ItemType playerAction)
    {
        anim.SetTrigger(playerAction.ToString());
    }

    public static void SetBoolAnimation(ItemType playerAction, bool flag)
    {
        anim.SetBool(playerAction.ToString(), flag);
    }

    public void WalkingCalculation(float x, float y)
    {
        if (x < 0)
        {
            characterGO.transform.localScale = new Vector3(1, 1, 1);
            playerEyes.transform.localPosition = new Vector3(x * 0.07f, y * 0.07f);
        }
        else
        {
            characterGO.transform.localScale = new Vector3(-1, 1, 1);
            playerEyes.transform.localPosition = new Vector3(-x * 0.07f, y * 0.07f);
        }
        if (isPlayerRunning)
        {
            anim.SetBool("isRunning", true);
            anim.SetFloat("RunningX", x);
            anim.SetFloat("RunningY", y);
        }
        else
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("WalkingX", x);
            anim.SetFloat("WalkingY", y);
        }
        transform.position += new Vector3(x, y, 0).normalized * Time.deltaTime * speed;
        currentPosX = (int)transform.position.x;
        currentPosY = (int)transform.position.y;
    }

    private void SetSortingOrder()
    {
        if (tempPosX == currentPosX && tempPosY == currentPosY)
        {
            return;
        }
        OnPlayerMovedPerGrid?.Invoke(currentPosX, currentPosY);
        OnPlayerMovedPerMeter?.Invoke();
        tempPosX = currentPosX;
        tempPosY = currentPosY;
        int order = (MainGameMapManager.CurrentMapSize + 1) - currentPosY;
        playerHead.sortingOrder = order;
        playerBody.sortingOrder = order;
        playerEyes.sortingOrder = order + 1;
        playerLimbLeft.sortingOrder = order + 1;
        playerLimbRight.sortingOrder = order - 1;
        playerRightWeapon.sortingOrder = order - 1;
        playerLegLeft.sortingOrder = order + 1;
        playerLegRight.sortingOrder = order - 1;
    }
}