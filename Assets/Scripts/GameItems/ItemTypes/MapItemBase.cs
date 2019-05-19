using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemBase : MonoBehaviour
{
    public MapItem mapItem;
    public int structId;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void Init(MapItem mapItem, int mapHeight)
    {
        this.mapItem = mapItem;
        structId = mapItem.structId;
        spriteRenderer.sprite = AtlasBank.GetMapItemSpriteById(mapItem.mapItemId);
        this.transform.localPosition = new Vector2(mapItem.posX, mapItem.posY);
        spriteRenderer.sortingLayerName = GEM.MapItemsSortingLayer;
        spriteRenderer.sortingOrder = (mapHeight + 1) - mapItem.posY;
    }

    protected virtual void MapItemDone()
    {
        //TODO: ugly code, refactor it later
        MainGameMapManager.MapItemDone((int)transform.position.x, (int)transform.position.y);
        DropItemsAfterDone();
        Destroy(this.gameObject);
    }

    private void DropItemsAfterDone()
    {
        List<Drops> drops = MapItemsDatabase.GetDropsById(mapItem.mapItemId);
        for (int i = 0; i < drops.Count; i++)
        {
            if (drops[i].dropId != null)
            {
                MapItemDropedItem mapItemDropedItem = Instantiate(PrefabBank.mapItemDroped, null);
                int dropCountRandom = UnityEngine.Random.Range((int)drops[i].min, (int)drops[i].max);
                mapItemDropedItem.Init(this.transform.position, (int)drops[i].dropId, dropCountRandom);
            }
        }
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