using Bronz.Ui;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MapItemDropedItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D circleCollider2D;
    public Item item;
    public const float randomPosRange = 0.75f;
    public const float moveDuration = 0.25f;
    public const float activateDuration = 0.5f;

    public void Init(Vector2 postion, int dropId, int dropCpunt)
    {
        circleCollider2D.enabled = false;
        item = new Item(dropId, dropCpunt);
        spriteRenderer.sprite = AtlasBank.GetInventoryItemSpriteById(dropId);
        transform.position = postion;
        StartCoroutine(EnableThisAfterDelay());
    }

    private IEnumerator EnableThisAfterDelay()
    {
        transform.DOMoveY(transform.position.y + 1, 0.15f).OnComplete(() =>
        transform.DOMoveY(transform.position.y - 1.5f, 0.25f).SetEase(Ease.OutBounce));
        yield return new WaitForSeconds(activateDuration);
        circleCollider2D.enabled = true;
    }

    public void TouchedByPlayer()
    {
        //TODO: if inventory not full then destroy, else drop again with remaining items
        transform.DOMove(PlayerMovement.player.transform.position, moveDuration).OnComplete(() => ItemTakenByPlayer());
    }

    private void ItemTakenByPlayer()
    {
        UiInventory.AddItemToInventory?.Invoke(item);
        Destroy(gameObject);
    }
}