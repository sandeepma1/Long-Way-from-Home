using FullSerializer;
using System.Collections.Generic;
using UnityEngine;

public class ItemBank : MonoBehaviour
{
    private TextAsset textAsset;
    [SerializeField] private ItemDatabase itemDatabase = new ItemDatabase();
    private fsSerializer mSerialiser = null;

    private void Awake()
    {
        textAsset = (TextAsset)Resources.Load("Jsons/ItemDatabase");
        mSerialiser = new fsSerializer();
        mSerialiser.AddConverter(new UnityObjectConverter());
        Load();
    }

    public void Load()
    {
        fsData parsedData = fsJsonParser.Parse(textAsset.text);
        object deserializedData = null;
        mSerialiser.TryDeserialize(parsedData, typeof(ItemDatabase), ref deserializedData).AssertSuccessWithoutWarnings();
        itemDatabase = (ItemDatabase)deserializedData;
    }

    //public static ItemDb GetItemById(int id)
    //{
    //    //return itemDatabase.Items[id];
    //}
}

[System.Serializable]
public class ItemDatabase
{
    public ItemDb[] Items;
}

[System.Serializable]
public class ItemDb
{
    public int id;
    public string name;
    public int maxAge;
    public int nextStage;
    public ItemType type;
    public ItemTool tool;
    public int reduceToolDurability;
    public float hardness;
    public bool isHandMined;
    public float experience;
    public int drops1;
    public int drop1RateMin;
    public int drop1RateMax;
    public int drops2;
    public int drop2RateMin;
    public int drop2RateMax;
    public int drops3;
    public int drop3RateMin;
    public int drop3RateMax;
    public string slug;
    public string description;
    public int durability;
    public bool isStackable;
    public bool isPlaceable;
    public int itemID1;
    public int itemAmount1;
    public int itemID2;
    public int itemAmount2;
    public int itemID3;
    public int itemAmount3;
    public int itemID4;
    public int itemAmount4;
    public int spawnsOnTerrian;
    public float spawnProbability;
    public float toolQuality;
    public Sprite sprite;

    //public ItemDb(int itemId, string itemName, int itemMaxAge, int itemNextStage, ItemType itemType, ItemTool itemTool,
    //             int itemReduceToolDurability, float itemHardness, bool isItemHandMined, float itemExperience,
    //             int itemDrops1, int itemDrop1RateMin, int itemDrop1RateMax, int itemDrops2, int itemDrop2RateMin,
    //             int itemDrop2RateMax, int itemDrops3, int itemDrop3RateMin, int itemDrop3RateMax, string slug,
    //             string description, int durability, bool isStackable, bool isPlaceable, int itemID1, int itemAmount1,
    //             int itemID2, int itemAmount2, int itemID3, int itemAmount3, int itemID4, int itemAmount4,
    //             int spawnsOnTerrian, float spawnProbability, float toolQuality)
    //{
    //    Id = itemId;
    //    Name = itemName;
    //    MaxAge = itemMaxAge;
    //    NextStage = itemNextStage;
    //    Type = itemType;
    //    Tool = itemTool;
    //    ReduceToolDurability = itemReduceToolDurability;
    //    Hardness = itemHardness;
    //    IsHandMined = isItemHandMined;
    //    Experience = itemExperience;
    //    Drops1 = itemDrops1;
    //    Drop1RateMin = itemDrop1RateMin;
    //    Drop1RateMax = itemDrop1RateMax;
    //    Drops2 = itemDrops2;
    //    Drop2RateMin = itemDrop2RateMin;
    //    Drop2RateMax = itemDrop2RateMax;
    //    Drops3 = itemDrops3;
    //    Drop3RateMin = itemDrop3RateMin;
    //    Drop3RateMax = itemDrop3RateMax;
    //    Slug = slug;
    //    Description = description;
    //    Durability = durability;
    //    IsStackable = isStackable;
    //    IsPlaceable = isPlaceable;
    //    ItemID1 = itemID1;
    //    ItemAmount1 = itemAmount1;
    //    ItemID2 = itemID2;
    //    ItemAmount2 = itemAmount2;
    //    ItemID3 = itemID3;
    //    ItemAmount3 = itemAmount3;
    //    ItemID4 = itemID4;
    //    ItemAmount4 = itemAmount4;
    //    SpawnsOnTerrian = spawnsOnTerrian;
    //    SpawnProbability = spawnProbability;
    //    ToolQuality = toolQuality;
    //    this.Sprite = Resources.Load<Sprite>("Textures/Inventory/" + slug);
    //}

    //public ItemDb()
    //{
    //    this.Id = -1;
    //}
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
