using UnityEngine;

public class InventoryItemsDatabase : DatabaseBase<InventoryItemsDatabase>
{
    public static InventoryItemsDb InventoryItems = new InventoryItemsDb();

    protected override void LoadFromJson()
    {
        base.LoadFromJson();
        mSerialiser.TryDeserialize(parsedData, typeof(InventoryItemsDb), ref deserializedData).AssertSuccessWithoutWarnings();
        InventoryItems = (InventoryItemsDb)deserializedData;

        if (InventoryItems.InventoryItems.Length <= 0)
        {
            Debug.LogError("InventoryItems db not loaded or empty");
        }
        else
        {
            GEM.PrintDebug("InventoryItems loaded with " + InventoryItems.InventoryItems.Length + " items");
        }
    }

    public static string GetSlugById(int id)
    {
        return InventoryItems.InventoryItems?[id].slug;
    }

    public static string GetNameById(int id)
    {
        return InventoryItems.InventoryItems?[id].name;
    }

    public static string GetDescriptionById(int id)
    {
        return InventoryItems.InventoryItems?[id].description;
    }

    public static InventoryItems[] GetMapItemsArray()
    {
        return InventoryItems.InventoryItems;
    }

    public static int GetMaxStackableById(int id)
    {
        return InventoryItems.InventoryItems[id].maxStackable;
    }

    public static InventoryItemType GetInventoryItemTypeById(int id)
    {
        return InventoryItems.InventoryItems[id].inventoryItemType;
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
    public string description;
    public string slug;
    public int maxStackable;
    public int hitPoints;
    public InventoryItemType inventoryItemType;
}

public enum ToolType
{
    axe,
    pickaxe,
    shovel,
    hammer,
    fishingPole,
    hand,
    scyth
}

public enum InventoryItemType
{
    none,
    food,
    equip,
    pickaxe,
    axe,
    shovel,
    weapon,
    placeable
}