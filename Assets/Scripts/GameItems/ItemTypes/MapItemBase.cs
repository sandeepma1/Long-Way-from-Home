using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemBase : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Init(int id)
    {
        spriteRenderer.sprite = AtlasBank.GetMapItemSpriteById(id);
    }
}

public enum MapItemSize
{
    grid16x16,
    grid16x32,
    grid32x32,
    grid16x48,
    grid32x48,
    grid48x48 // fill later
}