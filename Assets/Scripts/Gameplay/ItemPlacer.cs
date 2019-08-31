using Bronz.Ui;
using System;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Vector2 lastPosition;
    private Vector2 currentPosition;
    private bool isPlacementModeActive = false;

    private void Start()
    {
        UiInventory.OnInventoryItemClicked += OnInventoryItemClicked;
        PlayerMovement.OnPlayerMovedPerGrid += OnPlayerMovedPerGrid;
    }

    private void OnDestroy()
    {
        UiInventory.OnInventoryItemClicked -= OnInventoryItemClicked;
        PlayerMovement.OnPlayerMovedPerGrid -= OnPlayerMovedPerGrid;
    }

    private void OnInventoryItemClicked(UiSlotItem selectedUiSlotItem)
    {
        ItemType itemType = InventoryItemsDatabase.GetInventoryItemTypeById(selectedUiSlotItem.ItemId.Value);
        spriteRenderer.enabled = isPlacementModeActive = itemType == ItemType.placeable ? true : false;
        //if (itemType == ItemType.placeable)
        //{
        //    isPlacementModeActive = true;
        //    spriteRenderer.enabled = true;
        //}
        //else
        //{
        //    isPlacementModeActive = false;
        //    spriteRenderer.enabled = false;
        //}
    }

    private void OnPlayerMovedPerGrid(int posX, int posY)
    {
        //This will calculate one step ahead of player  as per his direction
        currentPosition = new Vector2(posX, posY);
        Vector2 heading = currentPosition - lastPosition;
        Vector2 direction = heading / heading.magnitude;
        lastPosition = currentPosition;
        transform.position = currentPosition + direction;
    }
}