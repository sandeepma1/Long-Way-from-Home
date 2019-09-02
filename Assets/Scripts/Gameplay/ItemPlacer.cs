using Bronz.Ui;
using Bronz.Utilities;
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
    private bool isPlaceable = false;
    private bool IsPlaceable
    {
        get => isPlaceable;
        set
        {
            isPlaceable = value;
            spriteRenderer.color = value ? ColorConstants.MapItemPlaceable : ColorConstants.MapItemNotPlaceable;
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
        if (itemType == ItemType.placeable)
        {
            IsPlacementModeActive = true;
            int mapItemId = InventoryItemsDatabase.GetPlacableMapItemIdById(selectedUiSlotItem.ItemId.Value);
            spriteRenderer.sprite = AtlasBank.GetMapItemSpriteById(mapItemId);
        }
        else
        {
            IsPlacementModeActive = false;
        }
    }

    private void OnPlayerMovedPerGrid(int posX, int posY)
    {
        //This will calculate one step ahead of player as per his direction
        currentPosition = new Vector2(posX, posY);
        Vector2 heading = currentPosition - lastPosition;
        Vector2 direction = heading / heading.magnitude;
        lastPosition = currentPosition;
        transform.position = currentPosition + direction;

        //Check if item is overlapping other map items
        MapItemBase mapItem = MainGameMapManager.GetMapItemByPosition(transform.position);
        bool mapTile = MainGameMapManager.IsItemPlacableOnMapTileByPosition(transform.position);
        if (mapItem == null && mapTile)
        {
            IsPlaceable = true;
        }
        else
        {
            IsPlaceable = false;
        }
    }

    private void OnItemPlacedClicked(UiSlotItem lastClickedUiSlotItem)
    {
        if (IsPlacementModeActive && IsPlaceable)
        {
            UiInventory.RemoveSelectedUiSlotItemOnClicked();
            MapItem mapItemToPlace = MainGameMapManager.CreateMapItemById(lastClickedUiSlotItem.ItemId.Value);
            MainGameMapManager.OnCreatePlacableMapItem?.Invoke((int)transform.position.x, (int)transform.position.y, mapItemToPlace);
            IsPlaceable = false;
            IsPlacementModeActive = false;
        }
        else
        {
            //TODO: Show common message box 
            print("Cannot place object here");
        }
    }
}