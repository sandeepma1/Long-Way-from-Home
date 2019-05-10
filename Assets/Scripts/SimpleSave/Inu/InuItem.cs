﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InuItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IEndDragHandler
{
    public Action<InuItem> OnItemDrop;
    public Action<InuItem> OnItemDrag;
    public InuIteo inuIteo;
    [SerializeField] private Image thisImage = null;
    [SerializeField] private Text valText;
    [SerializeField] private Text idText;

    public void UpdateTextValues()
    {
        valText.text = inuIteo.iteo.duraCount.ToString();
        idText.text = inuIteo.iteo.id.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        thisImage.raycastTarget = false;
        OnItemDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        thisImage.raycastTarget = true;
        OnItemDrag?.Invoke(null);
        transform.localPosition = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDrop?.Invoke(this);
    }
}