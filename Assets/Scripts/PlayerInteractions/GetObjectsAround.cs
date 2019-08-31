using Bronz.Ui;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GetObjectsAround : MonoBehaviour
{
    [SerializeField] private Transform closestItemMarker;
    [SerializeField] private LayerMask layerMask;
    private Vector3 nearestItemPosition;
    public static MapItemBase closestItem;
    private MapItemBase closestItemLast;
    [SerializeField] private float itemPickupRadius = 4f;
    private ItemType lastClickedItemType = ItemType.none;

    private void Start()
    {
        PlayerMovement.OnPlayerMoved += OnPlayerMoved;
        UiInventory.OnInventoryItemClicked += OnInventoryItemClicked;
        CalculateNearestItem();
    }

    private void OnDestroy()
    {
        PlayerMovement.OnPlayerMoved -= OnPlayerMoved;
        UiInventory.OnInventoryItemClicked -= OnInventoryItemClicked;
    }

    private void OnInventoryItemClicked(UiSlotItem uiSlotItem)
    {
        if (uiSlotItem == null)
        {
            lastClickedItemType = ItemType.none;
        }
        else
        {
            lastClickedItemType = uiSlotItem.itemType;
        }
    }

    private void OnPlayerMoved()
    {
        //CalculateNearestItem();
    }

    private void LateUpdate()
    {
        CalculateNearestItem();
    }

    private void CalculateNearestItem()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, itemPickupRadius, layerMask);
        List<MapItemBase> mapItems = new List<MapItemBase>();

        for (int i = 0; i < colliders.Length; i++)
        {
            MapItemBase mapItem = colliders[i].GetComponent<MapItemBase>();
            ItemType itemType = MapItemsDatabase.GetItemTypeById(mapItem.mapItem.mapItemId);
            if (itemType == lastClickedItemType ||
                itemType == ItemType.pickable ||
                itemType == ItemType.interactable)
            {
                mapItems.Add(mapItem);
            }
        }

        if (mapItems.Count == 0)
        {
            nearestItemPosition = Vector3.zero;
            closestItem = null;
        }
        else
        {
            closestItem = GetClosestItem(mapItems);
            if (closestItem != null)
            {
                nearestItemPosition = closestItem.transform.position;
            }
        }
        closestItemMarker.transform.localPosition = nearestItemPosition;
    }

    private MapItemBase GetClosestItem(List<MapItemBase> mapItems)
    {
        MapItemBase tMin = null;
        float minDist = Mathf.Infinity;
        if (mapItems.Count == 1 && mapItems[0] != null)
        {
            return mapItems[0].GetComponent<MapItemBase>();
        }
        foreach (MapItemBase t in mapItems)
        {
            if (t != null)
            {
                float dist = Vector3.Distance(t.transform.position, transform.position);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
        }
        return tMin;
    }
}