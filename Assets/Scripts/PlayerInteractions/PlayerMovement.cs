using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static Action<int, int> OnStep;
    public float speed = 0.1f;
    private int tempPosX, tempPosY, currentPosX, currentPosY;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        MasterSave.RequestSaveData += RequestSaveData;
        LoadPlayerInfoData();
    }

    private void LoadPlayerInfoData()
    {
        PlayerSavePlayerInfo playerSavePlayerInfo = MasterSave.LoadPlayerInfo();
        transform.position = new Vector2(playerSavePlayerInfo.posX, playerSavePlayerInfo.posY);
    }

    private void RequestSaveData()
    {
        MasterSave.SavePlayerInfo(new PlayerSavePlayerInfo(currentPosX, currentPosY));
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if (moveHorizontal == 0 && moveVertical == 0)
        {
            return;
        }

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        transform.position += movement * speed;

        currentPosX = (int)transform.position.x;
        currentPosY = (int)transform.position.y;

        if (tempPosX != currentPosX || tempPosY != currentPosY)
        {
            tempPosX = currentPosX;
            tempPosY = currentPosY;
            OnStep?.Invoke(currentPosX, currentPosY);
            spriteRenderer.sortingOrder = 64 - currentPosY + 1;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.CompareTag(GEM.MapItemDroppedTagName))
        {
            collision.gameObject.GetComponent<MapItemDropedItem>().TouchedByPlayer();
        }
    }
}
