using System.Collections.Generic;

public class MapItemsDatabase : DatabaseBase
{
    public static MapItemsDb MapItems = new MapItemsDb();

    protected override void LoadFromJson()
    {
        base.LoadFromJson();
        mSerialiser.TryDeserialize(parsedData, typeof(MapItemsDb), ref deserializedData).AssertSuccessWithoutWarnings();
        MapItems = (MapItemsDb)deserializedData;
    }

    public static Actions GetActionById(int id)
    {
        if (id < MapItems.MapItems.Length)
        {
            return MapItems.MapItems[id].primaryAction;
        }
        else
        {
            print("id incorrect " + id);
            return Actions.none;
        }
    }

    public static string GetSlugById(int id)
    {
        return MapItems.MapItems[id].slug;
    }

    public static MapItems[] GetMapItemsArray()
    {
        return MapItems.MapItems;
    }

    public static MapItemType GetMapItemTypeById(int id)
    {
        return MapItems.MapItems[id].mapItemType;
    }
}

[System.Serializable]
public class MapItemsDb
{
    public MapItems[] MapItems;
}

[System.Serializable]
public class MapItems
{
    public int id;
    public string name;
    public string slug;
    public Actions primaryAction;
    public Actions secondaryAction;
    public MapItemType mapItemType;
    public int healthPoints;
    public bool hasLifeCycle;
    public int nextStageId;
    public bool hasStructure;
    public int structureId;
    public List<Drops> drops;
}

[System.Serializable]
public enum MapItemType
{
    natural,
    manMade
}

public enum Actions
{
    chopable,
    mineable,
    hitable,
    fishable,
    breakable,
    openable,
    pickable,
    interactable,
    moveable,
    shakeable,
    placeable,
    shoveable,
    cutable,
    none,
    harvestable,
}

[System.Serializable]
public class Drops
{
    public int dropId;
    public int min;
    public int max;
}