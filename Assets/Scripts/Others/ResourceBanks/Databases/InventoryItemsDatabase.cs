
public class InventoryItemsDatabase : DatabaseBase<InventoryItemsDatabase>
{
    public static InventoryItemsDb InventoryItems = new InventoryItemsDb();

    protected override void LoadFromJson()
    {
        base.LoadFromJson();
        mSerialiser.TryDeserialize(parsedData, typeof(InventoryItemsDb), ref deserializedData).AssertSuccessWithoutWarnings();
        InventoryItems = (InventoryItemsDb)deserializedData;
    }

    public static string GetSlugById(int id)
    {
        return InventoryItems.InventoryItems?[id].slug;
    }

    public static InventoryItems[] GetMapItemsArray()
    {
        return InventoryItems.InventoryItems;
    }

    public static int GetMaxStackableById(int id)
    {
        return InventoryItems.InventoryItems[id].maxStackable;
    }
}

[System.Serializable]
public class InventoryItemsDb
{
    public InventoryItems[] InventoryItems;
}

[System.Serializable]
public class InventoryItems
{
    public int id;
    public string name;
    public string slug;
    public int maxStackable;
    public int hitPoints;
}