using System.Collections.Generic;
using UnityEngine;

public class CraftableItemsDatabase : DatabaseBase<MapItemsDatabase>
{
    public static CraftableItemsDb CraftableItemsDb = new CraftableItemsDb();

    protected override void LoadFromJson()
    {
        base.LoadFromJson();
        mSerialiser.TryDeserialize(parsedData, typeof(CraftableItemsDb), ref deserializedData).AssertSuccessWithoutWarnings();
        CraftableItemsDb = (CraftableItemsDb)deserializedData;
        if (CraftableItemsDb.CraftableItems.Length <= 0)
        {
            Debug.LogError("CraftableItemsDb db not loaded or empty");
        }
        else
        {
            GEM.PrintDebug("CraftableItemsDatabase loaded with " + CraftableItemsDb.CraftableItems.Length + " items");
        }
        for (int i = 0; i < CraftableItemsDb.CraftableItems.Length; i++)
        {
            for (int j = CraftableItemsDb.CraftableItems[i].requires.Count - 1; j >= 0; j--)
            {
                if (CraftableItemsDb.CraftableItems[i].requires[j].id == null)
                {
                    CraftableItemsDb.CraftableItems[i].requires.RemoveAt(j);
                }
            }
        }
    }

    public static List<Item> GetRequiredItemsListById(int id)
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
    public List<Item> requires;
}

//[System.Serializable]
//public class Requires
//{
//    public int? itemId;
//    public int? count;
//}