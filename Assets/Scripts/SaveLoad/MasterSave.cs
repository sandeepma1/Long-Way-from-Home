using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSave : MonoBehaviour
{
    public static Action RequestSaveData;
    private static PlayerSaveSlotItems playerSaveSlotItems = new PlayerSaveSlotItems();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
        SaveGame.DeleteAll();
    }

    private void RequestForSaveData()
    {
        GEM.PrintDebug("requesting save data to everyone");
        RequestSaveData?.Invoke();
    }


    #region Map Save/Load
    public static void SaveMapData(MapSaveIsland mapSaveData)
    {
        SaveGame.Save<MapSaveIsland>(GEM.MapSaveName, mapSaveData);
        GEM.PrintDebug("Map Save Complete");
    }

    public static MapSaveIsland LoadMapData()
    {
        GEM.PrintDebug("Loading map data...");
        return SaveGame.Load<MapSaveIsland>(GEM.MapSaveName);
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
        print("Loading PlayerInfo data...");
        if (!SaveGame.Exists(GEM.PlayerInfoSaveName))
        {
            GEM.PrintDebugWarning("PlayerInfo not found, creating new default data and saving");
            SavePlayerInfo(new PlayerSavePlayerInfo());
        }
        return SaveGame.Load<PlayerSavePlayerInfo>(GEM.PlayerInfoSaveName);
    }
    #endregion


    #region SlotItems Save/Load
    public static void SaveUiSlotItems()
    {
        SaveGame.Save<PlayerSaveSlotItems>(GEM.SlotItemsSaveName, playerSaveSlotItems);
        GEM.PrintDebug("All UiSlotItems Save Complete");
    }

    public static void LoadUiSlotItems()
    {
        print("Loading UiSlotItems data...");
        //if (!SaveGame.Exists(GEM.PlayerInfoSaveName))
        //{
        //    GEM.PrintDebugWarning("UiSlotItems not found, creating new default data and saving");
        //    SavePlayerInfo(new PlayerSavePlayerInfo());
        //}
        //return SaveGame.Load<PlayerSavePlayerInfo>(GEM.PlayerInfoSaveName);
    }
    #endregion
}


public class PlayerSavePlayerInfo
{
    public int posX;
    public int posY;
    public PlayerSavePlayerInfo()
    { posX = 20; posY = 20; }
    public PlayerSavePlayerInfo(int posX, int posY)
    { this.posX = posX; this.posY = posY; }
}

public class PlayerSaveSlotItems
{
    public List<PlayerSaveSlotItem> playerSaveItems = new List<PlayerSaveSlotItem>();
}
public class PlayerSaveSlotItem
{
    public SlotItemsType slotItemsType;
    public int id;
    public int currentFuelGuage;
    public List<SlotItems> slotItems = new List<SlotItems>();
    public PlayerSaveSlotItem(SlotItemsType slotItemsType, int id, int currentFuelGuage, List<SlotItems> slotItems)
    {
        this.slotItemsType = slotItemsType;
        this.id = id;
        this.currentFuelGuage = currentFuelGuage;
        this.slotItems = slotItems;
    }
}

public class PlayerSavePlayerStats
{
    public int health;
    public PlayerSavePlayerStats()
    { health = 100; }
}

public class MapSaveIsland
{
    public int mapId;
    public MapData mapData;
    public MapSaveIsland() { }
    public MapSaveIsland(int mapId, MapData mapData)
    {
        this.mapId = mapId;
        this.mapData = mapData;
    }
}

[System.Serializable]
public class MapItem
{
    public int mapItemId;
    public int healthPoints;
    public int posX;
    public int posY;
    public int structId;
    public MapItem(int mapItemId, int posX, int posY, int structId)
    {
        this.mapItemId = mapItemId;
        this.posX = posX;
        this.posY = posY;
        this.structId = structId;
    }
}