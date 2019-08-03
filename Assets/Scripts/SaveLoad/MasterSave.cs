using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSave : Singleton<MasterSave>
{
    public static Action RequestSaveData;
    private static PlayerSaveAllFurnitures playerSaveAllFurnitures;
    private static int maxFurnitureSaves = 3;
    private static int furnitureSavesCounter = 0;

    protected override void Awake()
    {
        playerSaveAllFurnitures = new PlayerSaveAllFurnitures();
        GEM.PrintDebug("MasterSave Awake");
        if (SaveGame.Exists(GEM.AllFurnituresSaveName))
        {
            playerSaveAllFurnitures = SaveGame.Load<PlayerSaveAllFurnitures>(GEM.AllFurnituresSaveName);
        }
        else
        {
            SaveGame.Save<PlayerSaveAllFurnitures>(GEM.AllFurnituresSaveName, playerSaveAllFurnitures);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //RequestForSaveData();
        }
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
        playerSaveAllFurnitures = new PlayerSaveAllFurnitures();
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


    #region Furniture SlotItems Save/Load
    public static void SaveInventory(PlayerSaveFurniture playerSaveFurniture)
    {
        playerSaveAllFurnitures.playerInventory = playerSaveFurniture;
        GEM.PrintDebug("SaveInventory Complete");
        furnitureSavesCounter++;
        SavePlayerSaveAllFurnitures();
    }

    public static void SaveFurnitureChest(List<PlayerSaveFurniture> playerChests)
    {
        playerSaveAllFurnitures.playerChests = playerChests;
        GEM.PrintDebug("SaveFurnitureChest Complete");
        furnitureSavesCounter++;
        SavePlayerSaveAllFurnitures();
    }

    public static void SaveFurnitureFurnce(List<PlayerSaveFurniture> playerFurnaces)
    {
        playerSaveAllFurnitures.playerFurnaces = playerFurnaces;
        GEM.PrintDebug("SaveFurnitureFurnce Complete");
        furnitureSavesCounter++;
        SavePlayerSaveAllFurnitures();
    }

    private static void SavePlayerSaveAllFurnitures()
    {
        SaveGame.Save<PlayerSaveAllFurnitures>(GEM.AllFurnituresSaveName, playerSaveAllFurnitures);

        if (furnitureSavesCounter == maxFurnitureSaves)
        {
            furnitureSavesCounter = 0;
            SaveGame.Save<PlayerSaveAllFurnitures>(GEM.AllFurnituresSaveName, playerSaveAllFurnitures);
        }
    }

    public static PlayerSaveFurniture LoadInventory()
    {
        GEM.PrintDebug("Loading playerInventory data...");
        if (playerSaveAllFurnitures == null)
        {
            return null;
        }
        return playerSaveAllFurnitures.playerInventory;
    }

    public static List<PlayerSaveFurniture> LoadChests()
    {
        GEM.PrintDebug("Loading playerChests data...");
        return playerSaveAllFurnitures.playerChests;
    }

    public static List<PlayerSaveFurniture> LoadFurnaces()
    {
        GEM.PrintDebug("Loading playerFurnaces data...");
        return playerSaveAllFurnitures.playerFurnaces;
    }
    #endregion
}


public class PlayerSavePlayerInfo
{
    public float posX;
    public float posY;
    public PlayerSavePlayerInfo()
    { posX = 20; posY = 20; }
    public PlayerSavePlayerInfo(float posX, float posY)
    { this.posX = posX; this.posY = posY; }
}

public class PlayerSaveAllFurnitures
{
    public PlayerSaveFurniture playerInventory;
    public List<PlayerSaveFurniture> playerChests = new List<PlayerSaveFurniture>();
    public List<PlayerSaveFurniture> playerFurnaces = new List<PlayerSaveFurniture>();

    public PlayerSaveAllFurnitures()
    {
        playerInventory = new PlayerSaveFurniture();
    }
}
public class PlayerSaveFurniture
{
    public FurnitureType furnitureType;
    public int id;
    public int currentRunningGuage;
    public List<SlotItems> slotItems = new List<SlotItems>();

    public PlayerSaveFurniture()
    {
    }

    public PlayerSaveFurniture(FurnitureType furnitureType, int id, int currentRunningGuage, List<SlotItems> slotItems)
    {
        this.furnitureType = furnitureType;
        this.id = id;
        this.currentRunningGuage = currentRunningGuage;
        this.slotItems = slotItems;
    }
}

public class PlayerSavePlayerStats
{
    public int health;
    public PlayerSavePlayerStats()
    { health = 100; }
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
    public int furnitureId;
    public int healthPoints;
    public MapItem(int mapItemId, int furnitureId, int healthPoints)
    {
        this.mapItemId = mapItemId;
        this.furnitureId = furnitureId;
        this.healthPoints = healthPoints;
    }
}