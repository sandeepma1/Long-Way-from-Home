using UnityEngine;

public class MapTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public int mapTileId;
    public bool isItemPlacable;

    public void Init(int mapTileId, int posX, int posY)
    {
        this.mapTileId = mapTileId;
        isItemPlacable = MapTilesDatabase.GetMapTileIsPlacableById(mapTileId);
        spriteRenderer.sprite = AtlasBank.GetMapTileSpriteById(mapTileId);
        spriteRenderer.sortingOrder = mapTileId;
        spriteRenderer.sortingLayerName = GEM.MapTilesSortingLayer;
        transform.localPosition = new Vector3(posX, posY);
        //adding random rotation Z to not appear texture pattern
        float randomRotation = UnityEngine.Random.Range(0, 360);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, randomRotation);
    }
}
