using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Init(int mapTileId)
    {
        spriteRenderer.sprite = ImageBank.GetMapItemSpriteById(mapTileId);
    }
}
