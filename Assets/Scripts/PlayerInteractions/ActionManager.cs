using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static Action<Vector2> OnMouseClick;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            GameObject clickedGo = MainGameMapManager.GetMapItemGameObjectByPosition(clickPosition);
            if (clickedGo != null)
            {
                PerformActionOnSelectedMapItem(clickedGo);
            }
        }
    }

    private void PerformActionOnSelectedMapItem(GameObject clickedGo)
    {
        int clickedItemId = clickedGo.GetComponent<MapItemBase>().item.id;
        Actions currentAction = MapItemsDatabase.GetActionById(clickedItemId);
        //Check if tool available in inventory to perform action
        //if yes
        switch (currentAction)
        {
            case Actions.chopable:
                clickedGo.GetComponent<IChopable>().Chop(10);
                break;
            case Actions.mineable:
                clickedGo.GetComponent<IMineable>().Mine(10);
                break;
            case Actions.hitable:
                break;
            case Actions.fishable:
                break;
            case Actions.breakable:
                break;
            case Actions.openable:
                break;
            case Actions.pickable:
                clickedGo.GetComponent<IPickable>().Pick();
                break;
            case Actions.interactable:
                break;
            case Actions.moveable:
                break;
            case Actions.shakeable:
                break;
            case Actions.placeable:
                break;
            case Actions.shoveable:
                break;
            case Actions.cutable:
                break;
            case Actions.none:
                break;
            case Actions.harvestable:
                break;
            default:
                break;
        }
    }
}