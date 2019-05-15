using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBiomesDatabase : DatabaseBase
{
    public MapBiomesDb MapBiomes = new MapBiomesDb();

    protected override void LoadFromJson()
    {
        base.LoadFromJson();
        mSerialiser.TryDeserialize(parsedData, typeof(MapBiomesDb), ref deserializedData).AssertSuccessWithoutWarnings();
        MapBiomes = (MapBiomesDb)deserializedData;
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
    public string name;
    public string slug;
    public List<Regions> regions;
}

[System.Serializable]
public class Regions
{
    public int tileId;
    public float height;
}