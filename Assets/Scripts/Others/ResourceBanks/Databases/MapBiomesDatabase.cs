using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBiomesDatabase : DatabaseBase<MapBiomesDatabase>
{
    public static MapBiomesDb MapBiomes = new MapBiomesDb();

    protected override void LoadFromJson()
    {
        base.LoadFromJson();
        mSerialiser.TryDeserialize(parsedData, typeof(MapBiomesDb), ref deserializedData).AssertSuccessWithoutWarnings();
        MapBiomes = (MapBiomesDb)deserializedData;
    }

    public static List<Regions> GetRegionsByBiomeName(BiomeName biomeName)
    {
        for (int i = 0; i < MapBiomes.MapBiomes.Length; i++)
        {
            if (MapBiomes.MapBiomes[i].biomeName == biomeName)
            {
                return MapBiomes.MapBiomes[i].regions;
            }
        }
        Debug.Log("Biome with name " + biomeName + " not found");
        return null;
    }
}

[System.Serializable]
public class MapBiomesDb
{
    public MapBiomes[] MapBiomes;
}

[System.Serializable]
public class MapBiomes
{
    public int id;
    public BiomeName biomeName;
    public List<Regions> regions;
}

[System.Serializable]
public class Regions
{
    public int tileId;
    public float height;
}

[System.Serializable]
public enum BiomeName
{
    Normal,
    Sandy,
    Green,
    Rocky,
    Beachy,
    Swampy,
    Dry
}