using System.Collections.Generic;
using UnityEngine;

public class MapItemBase : MonoBehaviour
{
    public MapItem mapItem;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Init(int posX, int posY, MapItem mapItem, int mapSize)
    {
        gameObject.layer = LayerMask.NameToLayer(GEM.MapItemLayerName);
        this.mapItem = mapItem;
        spriteRenderer.sprite = AtlasBank.GetMapItemSpriteById(mapItem.mapItemId);
        this.transform.localPosition = new Vector2(posX, posY);
        spriteRenderer.sortingLayerName = GEM.MapItemsSortingLayer;
        spriteRenderer.sortingOrder = (mapSize + 1) - posY;
    }

    protected virtual void MapItemHarvestingDone()
    {
        //TODO: ugly code, refactor it later
        MainGameMapManager.MapItemHarvestingDone(transform.position, mapItem.mapItemId);
        Destroy(this.gameObject);
    }
}