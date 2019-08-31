using Bronz.Ui;
using System;
using System.Collections;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static Action<Vector2> OnMouseClick;
    public static Action OnPlayerActionDone;
    private Camera mainCamera;
    private bool isActionButtonDown;
    private const float actionInterval = 0.5f;
    private const float itemMinDistance = 1.5f;
    private bool isActionInProgress = false;
    private UiSlotItem lastClikcedUiSlotItem = null;
    private ItemType lastClickedItemType;

    private void Start()
    {
        UiPlayerControlCanvas.OnActionButtonClicked += OnActionButtonClicked;
        UiPlayerControlCanvas.OnActionButtonDown += OnActionButtonDown;
        UiPlayerControlCanvas.OnActionButtonUp += OnActionButtonUp;
        UiPlayerControlCanvas.OnMoreButtonClicked += OnMoreButtonClicked;
        UiInventory.OnInventoryItemClicked += OnInventoryItemClicked;
        mainCamera = Camera.main;
    }

    private void OnDestroy()
    {
        UiPlayerControlCanvas.OnActionButtonClicked -= OnActionButtonClicked;
        UiPlayerControlCanvas.OnActionButtonDown -= OnActionButtonDown;
        UiPlayerControlCanvas.OnActionButtonUp -= OnActionButtonUp;
        UiPlayerControlCanvas.OnMoreButtonClicked -= OnMoreButtonClicked;
        UiInventory.OnInventoryItemClicked -= OnInventoryItemClicked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isActionButtonDown = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            isActionButtonDown = false;
        }
        if (isActionButtonDown)
        {
            ActionButtonPressed();
        }
    }

    private void OnInventoryItemClicked(UiSlotItem uiSlotItemClicked)
    {
        lastClikcedUiSlotItem = uiSlotItemClicked;
        if (uiSlotItemClicked == null)
        {
            lastClickedItemType = ItemType.none;
        }
        else
        {
            lastClickedItemType = InventoryItemsDatabase.GetInventoryItemTypeById(lastClikcedUiSlotItem.ItemId.Value);
        }

        if (lastClikcedUiSlotItem == null)
        {
            UiPlayerControlCanvas.OnActionButtonTextChange?.Invoke("");
            //Empty UiSlot clicked
            return;
        }
        UiPlayerControlCanvas.OnActionButtonTextChange?.Invoke(uiSlotItemClicked.itemType.ToString());
    }


    #region -- UiPlayerControlCanvas Actions --

    private void OnActionButtonClicked()
    {
        ActionButtonPressed();
    }

    private void OnMoreButtonClicked()
    {
        throw new NotImplementedException();
    }

    private void OnActionButtonUp()
    {
        isActionButtonDown = false;
    }

    private void OnActionButtonDown()
    {
        if (lastClickedItemType == ItemType.edible)
        {
            UiInventory.OnUseActionClicked?.Invoke();
        }
        isActionButtonDown = true;
    }
    #endregion


    private void ActionButtonPressed()
    {
        if (lastClickedItemType == ItemType.edible)
        {
            return;
        }
        if (GetObjectsAround.closestItem == null) { return; }
        if (!IsPlayerNearMapItem())
        {
            MovePlayerToMapItem();
            return;
        }
        if (!isActionInProgress)
        {
            StartCoroutine(PerformActionAfterDelay(actionInterval));
        }
    }

    private IEnumerator PerformActionAfterDelay(float duration)
    {
        isActionInProgress = true;
        int itemId = GetObjectsAround.closestItem.mapItem.mapItemId;
        PlayerMovement.SetTriggerAnimation(MapItemsDatabase.GetItemTypeById(itemId));
        yield return new WaitForSeconds(duration);
        PerformActionOnMapItem(GetObjectsAround.closestItem);
        isActionInProgress = false;
        OnPlayerActionDone?.Invoke();
    }

    private void PerformActionOnMapItem(MapItemBase clickedMapItem)
    {
        ItemType currentAction = MapItemsDatabase.GetItemTypeById(clickedMapItem.mapItem.mapItemId);
        //TODO: Check if tool available in inventory to perform action       
        switch (currentAction)
        {
            case ItemType.chopable:
                //TODO: add weapon damage 
                clickedMapItem.GetComponent<IChopable>().Chop(2);
                break;
            case ItemType.mineable:
                clickedMapItem.GetComponent<IMineable>().Mine(2);
                break;
            case ItemType.hitable:
                break;
            case ItemType.fishable:
                break;
            case ItemType.breakable:
                break;
            case ItemType.openable:
                break;
            case ItemType.pickable:
                clickedMapItem.GetComponent<IPickable>().Pick();
                break;
            case ItemType.interactable:
                break;
            case ItemType.moveable:
                break;
            case ItemType.shakeable:
                break;
            case ItemType.placeable:
                break;
            case ItemType.shoveable:
                break;
            case ItemType.cutable:
                break;
            case ItemType.none:
                break;
            case ItemType.harvestable:
                break;
            default:
                break;
        }
    }

    private bool IsPlayerNearMapItem()
    {
        float distance = Vector3.Distance(transform.position, GetObjectsAround.closestItem.transform.position);
        if (distance <= itemMinDistance)
        {
            return true;
        }
        return false;
    }

    private void MovePlayerToMapItem()
    {
        float step = 3 * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, GetObjectsAround.closestItem.transform.position, step);
    }
}