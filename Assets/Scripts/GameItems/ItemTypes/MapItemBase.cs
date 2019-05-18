using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemBase : MonoBehaviour
{
    public Item item;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Init(int id, int posX, int posY, int mapHeight)
    {
        item = new Item(id, MapItemsDatabase.GetHealthPointsById(id));
        spriteRenderer.sprite = AtlasBank.GetMapItemSpriteById(id);
        this.transform.localPosition = new Vector2(posX, posY);
        spriteRenderer.sortingLayerName = GEM.MapItemsSortingLayer;
        spriteRenderer.sortingOrder = (mapHeight + 1) - posY;
    }

    protected virtual void MapItemDone()
    {
        //TODO: ugly code, refactor it later
        MainGameMapManager.MapItemDone((int)transform.position.x, (int)transform.position.y);
        Destroy(this.gameObject);
    }
}

[System.Serializable]
public class MapItem
{
    public Item item;
    public Actions action;
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