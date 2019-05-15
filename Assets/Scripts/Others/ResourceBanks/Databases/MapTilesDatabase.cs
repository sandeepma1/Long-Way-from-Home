using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTilesDatabase : DatabaseBase
{
    private static MapTilesDb MapTiles = new MapTilesDb();

    protected override void LoadFromJson()
    {
        base.LoadFromJson();
        mSerialiser.TryDeserialize(parsedData, typeof(MapTilesDb), ref deserializedData).AssertSuccessWithoutWarnings();
        MapTiles = (MapTilesDb)deserializedData;
    }

    public static string GetSlugById(int id)
    {
        return MapTiles.MapTiles[id].slug;
    }

    public static MapTiles[] GetMapItemsArray()
    {
        return MapTiles.MapTiles;
    }
}

[System.Serializable]
public class MapTilesDb
{
    public MapTiles[] MapTiles;
}

[System.Serializable]
public class MapTiles
{
    public int id;
    public string name;
    public string slug;
}
