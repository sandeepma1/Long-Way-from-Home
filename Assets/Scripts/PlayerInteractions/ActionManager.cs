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

    private void Start()
    {
        UiPlayerControlCanvas.OnActionButtonClicked += OnActionButtonClicked;
        UiPlayerControlCanvas.OnActionButtonDown += OnActionButtonDown;
        UiPlayerControlCanvas.OnActionButtonUp += OnActionButtonUp;
        UiPlayerControlCanvas.OnMoreButtonClicked += OnMoreButtonClicked;
        mainCamera = Camera.main;
    }

    private void OnDestroy()
    {
        UiPlayerControlCanvas.OnActionButtonClicked -= OnActionButtonClicked;
        UiPlayerControlCanvas.OnActionButtonDown -= OnActionButtonDown;
        UiPlayerControlCanvas.OnActionButtonUp -= OnActionButtonUp;
        UiPlayerControlCanvas.OnMoreButtonClicked -= OnMoreButtonClicked;
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
        isActionButtonDown = true;
    }
    #endregion


    private void ActionButtonPressed()
    {
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
        PlayerMovement.SetTriggerAnimation(MapItemsDatabase.GetActionById(itemId));
        yield return new WaitForSeconds(duration);
        PerformActionOnMapItem(GetObjectsAround.closestItem);
        isActionInProgress = false;
        OnPlayerActionDone?.Invoke();
    }

    private void PerformActionOnMapItem(MapItemBase clickedMapItem)
    {
        PlayerActions currentAction = MapItemsDatabase.GetActionById(clickedMapItem.mapItem.mapItemId);
        //TODO: Check if tool available in inventory to perform action       
        switch (currentAction)
        {
            case PlayerActions.chopable:
                //TODO: add weapon damage 
                clickedMapItem.GetComponent<IChopable>().Chop(2);
                break;
            case PlayerActions.mineable:
                clickedMapItem.GetComponent<IMineable>().Mine(2);
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
                clickedMapItem.GetComponent<IPickable>().Pick();
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

    private void MovePlayerToMapItem()
    {
        float step = 3 * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, GetObjectsAround.closestItem.transform.position, step);
    }
}