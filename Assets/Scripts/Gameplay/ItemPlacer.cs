using Bronz.Ui;
using System;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Vector2 lastPosition;
    private Vector2 currentPosition;
    private bool isPlacementModeActive;
    private bool IsPlacementModeActive
    {
        get => isPlacementModeActive;
        set
        {
            isPlacementModeActive = value;
            spriteRenderer.enabled = value;
        }
    }

    private void Start()
    {
        ActionManager.OnItemPlacedClicked += OnItemPlacedClicked;
        UiInventory.OnInventoryItemClicked += OnInventoryItemClicked;
        PlayerMovement.OnPlayerMovedPerGrid += OnPlayerMovedPerGrid;
    }

    private void OnDestroy()
    {
        ActionManager.OnItemPlacedClicked -= OnItemPlacedClicked;
        UiInventory.OnInventoryItemClicked -= OnInventoryItemClicked;
        PlayerMovement.OnPlayerMovedPerGrid -= OnPlayerMovedPerGrid;
    }

    private void OnInventoryItemClicked(UiSlotItem selectedUiSlotItem)
    {
        ItemType itemType = ItemType.none;
        if (selectedUiSlotItem != null)
        {
            itemType = InventoryItemsDatabase.GetInventoryItemTypeById(selectedUiSlotItem.ItemId.Value);
        }
        IsPlacementModeActive = itemType == ItemType.placeable ? true : false;
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

    private void OnItemPlacedClicked(UiSlotItem lastClickedUiSlotItem)
    {
        IsPlacementModeActive = false;
    }
}