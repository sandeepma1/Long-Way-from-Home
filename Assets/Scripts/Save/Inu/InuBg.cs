using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InuBg : MonoBehaviour, IDropHandler
{
    public Action<int> OnBgDrop;
    public int id;
    public bool isOccupied = false;

    public void OnDrop(PointerEventData eventData)
    {
        isOccupied = true;
        OnBgDrop?.Invoke(id);
    }
}