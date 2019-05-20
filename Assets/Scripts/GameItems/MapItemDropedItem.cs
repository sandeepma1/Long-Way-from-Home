using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemDropedItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D circleCollider2D;
    public Item item;
    public const float randomPosRange = 0.75f;

    public void Init(Vector2 postion, int dropId, int dropCpunt)
    {
        circleCollider2D.enabled = false;
        //TODO: fallen animation on drops
        this.item = new Item(dropId, dropCpunt);
        spriteRenderer.sprite = AtlasBank.GetInventoryItemSpriteById(dropId);
        float randomX = UnityEngine.Random.Range(postion.x - randomPosRange, postion.x + randomPosRange);
        float randomY = UnityEngine.Random.Range(postion.y - randomPosRange, postion.y + randomPosRange);
        transform.position = new Vector2(randomX, randomY);
        StartCoroutine(EnableThisAfterDelay());
    }

    private IEnumerator EnableThisAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        circleCollider2D.enabled = true;
    }

    public void TouchedByPlayer()
    {
        UiInventory.AddItemToInventory?.Invoke(item);
        //TODO: if inventory not full then destroy, else drop again with remaining items
        Destroy(this.gameObject);
    }
}