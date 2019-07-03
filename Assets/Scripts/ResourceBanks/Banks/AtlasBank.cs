using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasBank : MonoBehaviour
{
    private static SpriteAtlas uiSpriteAtlas;
    private static SpriteAtlas inventoryAtlas;
    private static SpriteAtlas mapItemsAtlas;
    private static SpriteAtlas mapTilesAtlas;
    private static SpriteAtlas miscAtlas;
    private static Sprite missingSprite;

    private void Awake()
    {
        uiSpriteAtlas = PrefabBank.uiSpritesAtlas;
        inventoryAtlas = PrefabBank.inventoryItemsAtlas;
        mapItemsAtlas = PrefabBank.mapItemsAtlas;
        miscAtlas = PrefabBank.miscAtlas;
        mapTilesAtlas = PrefabBank.mapTilesAtlas;
        missingSprite = GetSprite("Blockx100", AtlasType.Misc);
    }

    public static Sprite GetUiSpritesByName(string name)
    {
        return GetSprite(name, AtlasType.UiSprites);
    }

    public static Sprite GetMapItemSpriteById(int itemId)
    {
        return GetSprite(MapItemsDatabase.GetSlugById(itemId), AtlasType.MapItems);
    }

    public static Sprite GetInventoryItemSpriteById(int itemId)
    {
        return GetSprite(InventoryItemsDatabase.GetSlugById(itemId), AtlasType.InventoryItems);
    }

    public static Sprite GetInventoryItemSpriteBySlug(string slug)
    {
        return GetSprite(slug, AtlasType.InventoryItems);
    }

    public static Sprite GetMapTileSpriteById(int itemId)
    {
        return GetSprite(MapTilesDatabase.GetSlugById(itemId), AtlasType.MapTiles);
    }

    public static Sprite GetSprite(string name, AtlasType type)
    {
        Sprite sprite = null;
        switch (type)
        {
            case AtlasType.UiSprites:
                sprite = uiSpriteAtlas.GetSprite(name);
                break;
            case AtlasType.InventoryItems:
                sprite = inventoryAtlas.GetSprite(name);
                break;
            case AtlasType.MapItems:
                sprite = mapItemsAtlas.GetSprite(name);
                break;
            case AtlasType.MapTiles:
                sprite = mapTilesAtlas.GetSprite(name);
                break;
            case AtlasType.Misc:
                sprite = miscAtlas.GetSprite(name);
                break;
        }

        if (sprite == null)
        {
            //Debug.Log("Sprite named " + name + " not found in Bank Type " + type);
            sprite = missingSprite;
        }
        return sprite;
    }
}

public enum AtlasType
{
    UiSprites,
    InventoryItems,
    MapItems,
    MapTiles,
    Misc
}