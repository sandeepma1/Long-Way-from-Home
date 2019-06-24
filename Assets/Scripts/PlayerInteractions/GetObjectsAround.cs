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

    private void Start()
    {
        PlayerMovement.OnPlayerMoved += OnPlayerMoved;
    }

    private void OnDestroy()
    {
        PlayerMovement.OnPlayerMoved -= OnPlayerMoved;
    }

    private void OnPlayerMoved()
    {
        // CalculateNearestItem();
    }

    private void Update()
    {
        CalculateNearestItem();
    }

    private void CalculateNearestItem()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, itemPickupRadius, layerMask);
        if (colliders.Length == 0)
        {
            nearestItemPosition = Vector3.zero;
            closestItem = null;
        }
        else
        {
            closestItem = GetClosestItem(colliders);
            if (closestItem != null)
            {
                nearestItemPosition = closestItem.transform.position;
            }
        }
        closestItemMarker.transform.localPosition = nearestItemPosition;
    }

    private MapItemBase GetClosestItem(Collider2D[] colliders)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        if (colliders.Length == 1 && colliders[0] != null)
        {
            return colliders[0].GetComponent<MapItemBase>();
        }
        foreach (Collider2D t in colliders)
        {
            if (t != null)
            {
                float dist = Vector3.Distance(t.transform.position, transform.position);
                if (dist < minDist)
                {
                    tMin = t.transform;
                    minDist = dist;
                }
            }
        }
        return tMin.GetComponent<MapItemBase>();
    }
}