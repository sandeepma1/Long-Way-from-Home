using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemBase : MonoBehaviour
{
    public void Init(int id)
    {
        GetComponent<SpriteRenderer>().sprite = ImageBank.GetMapItemSpriteById(id);
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