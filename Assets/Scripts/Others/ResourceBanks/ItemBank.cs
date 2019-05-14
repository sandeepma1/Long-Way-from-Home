using FullSerializer;
using System.Collections.Generic;
using UnityEngine;

public class ItemBank : MonoBehaviour
{
    private TextAsset textAssetMapItems;
    private TextAsset textAssetInventoryItems;
    [SerializeField] private static ItemDatabase itemDatabase = new ItemDatabase();
    private fsSerializer mSerialiser = null;
    private string jsonStringMapItems;
    private string jsonStringInventoryItems;
    private string finalOutput;

    private void Awake()
    {
        textAssetMapItems = (TextAsset)Resources.Load("Jsons/ItemDatabase - MapItems");
        textAssetInventoryItems = (TextAsset)Resources.Load("Jsons/ItemDatabase - InventoryItems");

        jsonStringMapItems = textAssetMapItems.text;
        jsonStringInventoryItems = textAssetInventoryItems.text;

        jsonStringMapItems = jsonStringMapItems.Substring(0, jsonStringMapItems.Length - 1);
        jsonStringMapItems += ",";

        jsonStringInventoryItems = jsonStringInventoryItems.Substring(1);
        finalOutput = jsonStringMapItems + jsonStringInventoryItems;

        print(finalOutput);
        mSerialiser = new fsSerializer();
        mSerialiser.AddConverter(new UnityObjectConverter());
        Load();
    }

    public void Load()
    {
        fsData parsedData = fsJsonParser.Parse(finalOutput);
        object deserializedData = null;
        mSerialiser.TryDeserialize(parsedData, typeof(ItemDatabase), ref deserializedData).AssertSuccessWithoutWarnings();
        itemDatabase = (ItemDatabase)deserializedData;
    }

    public static string GetItemSlugById(int id)
    {
        return itemDatabase.MapItems[id].slug;
    }
}

[System.Serializable]
public class ItemDatabase
{
    public MapItems[] MapItems;
    public InventoryItems[] InventoryItems;
}

[System.Serializable]
public class MapItems
{
    public int id;
    public string name;
    public string slug;
    public Actions primaryAction;
    public Actions secondaryAction;
    public int healthPoints;
    public bool hasLifeCycle;
    public int nextStageId;
    public bool hasStructure;
    public int structureId;
    public List<Drops> drops;
}

[System.Serializable]
public class Drops
{
    public int dropId;
    public int min;
    public int max;
}

[System.Serializable]
public class InventoryItems
{
    public int id;
    public string name;
    public string slug;
    public int maxStackable;
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

public enum ItemType
{
    Armor,
    Food,
    Plant,
    Tree,
    RawMaterial,
    Ore,
    Mineral,
    Tool,
    Weapon,
    Consumable,
    Quest,
    Build
}

public enum ItemTool
{
    None,
    Hand,
    Axe,
    Pickaxe,
    Hammer,
    Hoe,
    Shovel,
    FishingRod,
    Sword
}
