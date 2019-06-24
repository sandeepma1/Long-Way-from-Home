using Bronz.Ui;
using System;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static Action<Vector2> OnMouseClick;
    private Camera mainCamera;
    private bool isActionButtonDown;
    private float buttonDownDuration = 0.5f;
    private float buttonDownCounter = 0.0f;
    private const float itemMinDistance = 2.0f;

    private void Start()
    {
        UiPlayerControlCanvas.OnActionButtonPointerDown += OnActionButtonPointerDown;
        UiPlayerControlCanvas.OnActionButtonPointerUp += OnActionButtonPointerUp;
        UiPlayerControlCanvas.OnMoreButtonClicked += OnMoreButtonClicked;
        mainCamera = Camera.main;
    }

    private void OnDestroy()
    {
        UiPlayerControlCanvas.OnActionButtonPointerDown -= OnActionButtonPointerDown;
        UiPlayerControlCanvas.OnActionButtonPointerUp -= OnActionButtonPointerUp;
        UiPlayerControlCanvas.OnMoreButtonClicked -= OnMoreButtonClicked;
    }

    private void OnMoreButtonClicked()
    {
        throw new NotImplementedException();
    }

    private void OnActionButtonPointerUp()
    {
        isActionButtonDown = false;
        buttonDownCounter = buttonDownDuration;
    }

    private void OnActionButtonPointerDown()
    {
        isActionButtonDown = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            MapItemBase clickedGo = MainGameMapManager.GetMapItemByPosition(clickPosition);
            if (clickedGo != null)
            {
                PerformActionOnMapItem(clickedGo);
            }
        }

        if (isActionButtonDown)
        {
            if (GetObjectsAround.closestItem == null) { return; }
            if (!IsPlayerNearMapItem()) { return; }

            if (buttonDownCounter >= buttonDownDuration)
            {
                int itemId = GetObjectsAround.closestItem.mapItem.mapItemId;
                PlayerMovement.SetTriggerAnimation(MapItemsDatabase.GetActionById(itemId));
            }
            buttonDownCounter -= Time.deltaTime;
            if (buttonDownCounter <= 0)
            {
                buttonDownCounter = buttonDownDuration;
                PerformActionOnMapItem(GetObjectsAround.closestItem);
            }
        }
    }

    private void PerformActionOnMapItem(MapItemBase clickedGo)
    {
        PlayerActions currentAction = MapItemsDatabase.GetActionById(clickedGo.mapItem.mapItemId);
        //TODO: Check if tool available in inventory to perform action
        //if yes
        switch (currentAction)
        {
            case PlayerActions.chopable:
                clickedGo.GetComponent<IChopable>().Chop(2);
                break;
            case PlayerActions.mineable:
                clickedGo.GetComponent<IMineable>().Mine(2);
                break;
            case PlayerActions.hitable:
                break;
            case PlayerActions.fishable:
                break;
            case PlayerActions.breakable:
                break;
            case PlayerActions.openable:
                break;
            case PlayerActions.pickable:
                clickedGo.GetComponent<IPickable>().Pick();
                break;
            case PlayerActions.interactable:
                break;
            case PlayerActions.moveable:
                break;
            case PlayerActions.shakeable:
                break;
            case PlayerActions.placeable:
                break;
            case PlayerActions.shoveable:
                break;
            case PlayerActions.cutable:
                break;
            case PlayerActions.none:
                break;
            case PlayerActions.harvestable:
                break;
            default:
                break;
        }
    }

    private bool IsPlayerNearMapItem()
    {
        float distance = Vector3.Distance(transform.position, GetObjectsAround.closestItem.transform.position);
        DebugText.PrintDebugText(distance.ToString());
        if (distance <= itemMinDistance)
        {
            return true;
        }
        return false;
    }
}