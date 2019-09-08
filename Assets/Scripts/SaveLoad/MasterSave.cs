using BayatGames.SaveGameFree;
using System;
using System.Collections.Generic;

public class MasterSave : Singleton<MasterSave>
{
    public static Action RequestSaveData;
    private static int maxFurnitureSaves = 3;
    private static int furnitureSavesCounter = 0;

    protected override void Awake()
    {
        GEM.PrintDebug("MasterSave Awake");
    }

    private void OnApplicationQuit()
    {
        RequestForSaveData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            //RequestForSaveData();
        }
    }

    /// <summary>
    /// Delete all old save games as new game is been created
    /// </summary>
    public static void CreatingNewGame()
    {
        SaveGame.DeleteAll();
    }

    public static void RequestForSaveData()
    {
        GEM.PrintDebug("requesting save data to everyone");
        RequestSaveData?.Invoke();
    }


    #region Map Save/Load
    public static void SaveMapData(MapSave mapSaveData)
    {
        SaveGame.Save<MapSave>(GEM.MapSaveName, mapSaveData);
        GEM.PrintDebug("Map Save Complete");
    }

    public static MapSave LoadMapData()
    {
        GEM.PrintDebug("Loading map data...");
        return SaveGame.Load<MapSave>(GEM.MapSaveName);
    }
    #endregion


    #region PlayerInfo Save/Load
    public static void SavePlayerInfo(PlayerSavePlayerInfo playerInfoSaveData)
    {
        SaveGame.Save<PlayerSavePlayerInfo>(GEM.PlayerInfoSaveName, playerInfoSaveData);
        GEM.PrintDebug("PlayerInfo Save Complete");
    }

    public static PlayerSavePlayerInfo LoadPlayerInfo()
    {
        GEM.PrintDebug("Loading PlayerInfo data...");
        if (!SaveGame.Exists(GEM.PlayerInfoSaveName))
        {
            GEM.PrintDebugWarning("PlayerInfo not found, creating new default data and saving");
            SavePlayerInfo(new PlayerSavePlayerInfo());
        }
        return SaveGame.Load<PlayerSavePlayerInfo>(GEM.PlayerInfoSaveName);
    }
    #endregion


    #region PlayerStats Save/Load
    public static void SavePlayerStats(PlayerSavePlayerStats playerSavePlayerStats)
    {
        SaveGame.Save<PlayerSavePlayerStats>(GEM.PlayerStatsSaveName, playerSavePlayerStats);
        GEM.PrintDebug("PlayerStats Save Complete");
    }

    public static PlayerSavePlayerStats LoadPlayerStats()
    {
        GEM.PrintDebug("Loading PlayerStats data...");
        if (!SaveGame.Exists(GEM.PlayerStatsSaveName))
        {
            GEM.PrintDebugWarning("PlayerStats not found, creating new default data and saving");
            SavePlayerStats(new PlayerSavePlayerStats());
        }
        return SaveGame.Load<PlayerSavePlayerStats>(GEM.PlayerStatsSaveName);
    }
    #endregion


    #region PlayerInventory Save/Load
    public static void SaveInventory(PlayerSaveFurniture playerSaveFurniture)
    {
        SaveGame.Save<PlayerSaveFurniture>(GEM.PlayerInventorySaveName, playerSaveFurniture);
        GEM.PrintDebug("SaveInventory Complete");
    }

    public static PlayerSaveFurniture LoadInventory()
    {
        GEM.PrintDebug("Loading PlayerStats data...");
        if (!SaveGame.Exists(GEM.PlayerInventorySaveName))
        {
            GEM.PrintDebugWarning("PlayerStats not found, creating new default data and saving");
            SaveInventory(new PlayerSaveFurniture());
        }
        return SaveGame.Load<PlayerSaveFurniture>(GEM.PlayerInventorySaveName);
    }
    #endregion
}


public class PlayerSavePlayerInfo
{
    public float posX;
    public float posY;
    public int lastSelectedInventorySlotId;
    public PlayerSavePlayerInfo()
    { posX = 20; posY = 20; lastSelectedInventorySlotId = 0; }
    public PlayerSavePlayerInfo(float posX, float posY, int slotId)
    { this.posX = posX; this.posY = posY; lastSelectedInventorySlotId = slotId; }
}

public class PlayerSaveFurniture
{
    public FurnitureType furnitureType;
    public int id;
    public int currentRunningGuage;
    public List<SlotItem> slotItems = new List<SlotItem>();

    public PlayerSaveFurniture()
    {
    }

    public PlayerSaveFurniture(FurnitureType furnitureType, int id, int currentRunningGuage, List<SlotItem> slotItems)
    {
        this.furnitureType = furnitureType;
        this.id = id;
        this.currentRunningGuage = currentRunningGuage;
        this.slotItems = slotItems;
    }
}

public class PlayerSavePlayerStats
{
    public float health;
    public float hunger;
    public float thirst;
    public PlayerSavePlayerStats(float health, float hunger, float thirst)
    {
        this.health = health;
        this.hunger = hunger;
        this.thirst = thirst;
    }
    public PlayerSavePlayerStats()
    {
        health = 100;
        hunger = 100;
        thirst = 100;
    }
}

public class MapSave
{
    public int mapId;
    public int mapSize;
    public int[] mapTiles;
    public Dictionary<int, MapItem> mapItems;
    public MapSave() { }
    public MapSave(int mapId, int mapSize, int[] mapTiles, Dictionary<int, MapItem> mapItems)
    {
        this.mapId = mapId;
        this.mapSize = mapSize;
        this.mapTiles = mapTiles;
        this.mapItems = mapItems;
    }
}

[System.Serializable]
public class MapItem
{
    public int mapItemId;
    public int structureId;
    public int healthPoints;
    public MapItem(int mapItemId, int structureId, int healthPoints)
    {
        this.mapItemId = mapItemId;
        this.structureId = structureId;
        this.healthPoints = healthPoints;
    }
}