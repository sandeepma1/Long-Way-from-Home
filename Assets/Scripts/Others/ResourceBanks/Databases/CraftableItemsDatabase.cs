using System.Collections.Generic;

public class CraftableItemsDatabase : DatabaseBase<MapItemsDatabase>
{
    public static CraftableItemsDb CraftableItemsDb = new CraftableItemsDb();

    protected override void LoadFromJson()
    {
        base.LoadFromJson();
        mSerialiser.TryDeserialize(parsedData, typeof(CraftableItemsDb), ref deserializedData).AssertSuccessWithoutWarnings();
        CraftableItemsDb = (CraftableItemsDb)deserializedData;
        GEM.PrintDebug("CraftableItemsDatabase loaded with " + CraftableItemsDb.CraftableItems.Length + " items");
    }

    public static List<Requires> GetRequiredItemsListById(int id)
    {
        return CraftableItemsDb.CraftableItems[id].requires;
    }

    public static CraftableItem GetCraftableItemById(int id)
    {
        return CraftableItemsDb.CraftableItems[id];
    }
}

[System.Serializable]
public class CraftableItemsDb
{
    public CraftableItem[] CraftableItems;
}

[System.Serializable]
public class CraftableItem
{
    public int id;
    public string name;
    public string slug;
    public int creates;
    public List<Requires> requires;
}

[System.Serializable]
public class Requires
{
    public int? itemId;
    public int? count;
}