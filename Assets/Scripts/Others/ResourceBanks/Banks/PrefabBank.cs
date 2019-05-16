using UnityEngine;
using UnityEngine.U2D;

public class PrefabBank : MonoBehaviour
{
    public static UiSlot uiSlotPrefab;
    public static UiSlotItem uiSlotItemPrefab;
    public static SpriteAtlas mapItemsAtlas;
    public static SpriteAtlas mapTilesAtlas;
    public static SpriteAtlas miscAtlas;
    public static MapItemChopable mapItemChopablePrefab;
    public static MapItemMineable mapItemMineablePrefab;
    public static MapItemPickable mapItemPickablePrefab;
    public static MapTile mapTilePrefab;

    private void Awake()
    {
        LoadAllFromResource();
    }

    private void LoadAllFromResource()
    {
        uiSlotPrefab = Resources.Load<UiSlot>("Prefabs/UiPrefabs/UiSlot");
        uiSlotItemPrefab = Resources.Load<UiSlotItem>("Prefabs/UiPrefabs/UiSlotitem");
        mapItemsAtlas = Resources.Load<SpriteAtlas>("Atlas/MapItemsAtlas");
        mapTilesAtlas = Resources.Load<SpriteAtlas>("Atlas/MapTilesAtlas");
        miscAtlas = Resources.Load<SpriteAtlas>("Atlas/MiscAtlas");
        mapItemChopablePrefab = Resources.Load<MapItemChopable>("Prefabs/MapItems/MapItemChopable");
        mapItemMineablePrefab = Resources.Load<MapItemMineable>("Prefabs/MapItems/MapItemMineable");
        mapItemPickablePrefab = Resources.Load<MapItemPickable>("Prefabs/MapItems/MapItemPickable");
        mapTilePrefab = Resources.Load<MapTile>("Prefabs/MapItems/MapTile");
    }
}
