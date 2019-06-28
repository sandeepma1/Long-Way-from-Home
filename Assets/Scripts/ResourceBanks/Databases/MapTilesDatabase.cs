using System.Collections.Generic;
using UnityEngine;

public class MapTilesDatabase : DatabaseBase<MapTilesDatabase>
{
    private static MapTilesDb MapTiles = new MapTilesDb();

    protected override void LoadFromJson()
    {
        base.LoadFromJson();
        mSerialiser.TryDeserialize(parsedData, typeof(MapTilesDb), ref deserializedData).AssertSuccessWithoutWarnings();
        MapTiles = (MapTilesDb)deserializedData;
        if (MapTiles.MapTiles.Length <= 0)
        {
            Debug.LogError("MapTiles db not loaded or empty");
        }
        else
        {
            GEM.PrintDebug("MapTiles loaded with " + MapTiles.MapTiles.Length + " items");
        }
        for (int i = 0; i < MapTiles.MapTiles.Length; i++)
        {
            for (int j = MapTiles.MapTiles[i].grows.Count - 1; j >= 0; j--)
            {
                if (MapTiles.MapTiles[i].grows[j].growId == null)
                {
                    MapTiles.MapTiles[i].grows.RemoveAt(j);
                }
            }
        }
    }

    public static string GetSlugById(int id)
    {
        return MapTiles.MapTiles[id].slug;
    }

    public static MapTiles[] GetMapItemsArray()
    {
        return MapTiles.MapTiles;
    }

    public static List<Grows> GetAllGrowsById(int id)
    {
        return MapTiles.MapTiles[id].grows;
    }

    public static int? GetMapTileGrowsCountById(int id)
    {
        if (MapTiles.MapTiles[id].grows.Count > 0)
        {
            return MapTiles.MapTiles[id].grows.Count;
        }
        else
        {
            return null;
        }
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
    public List<Grows> grows;
}

[System.Serializable]
public class Grows
{
    public int? growId;
    public float? prob;
}