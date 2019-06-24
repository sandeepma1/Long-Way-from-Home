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