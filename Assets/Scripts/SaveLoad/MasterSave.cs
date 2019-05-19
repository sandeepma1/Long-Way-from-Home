using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSave : MonoBehaviour
{
    public static Action RequestSaveData;
    public static Action<PlayerSavePlayerInfo> OnPlayerInfoLoaded;
    public static Action<List<PlayerSaveSlotItem>> OnSlotItemsLoaded;
    public static Action<PlayerSavePlayerStats> OnStatsLoaded;
    private AllGameSaveEntities allGameSaves = new AllGameSaveEntities();
    private const string playerInfoSaveName = "PlayerName";

    //*******
    private MapSaveIsland mapSaveIslands = new MapSaveIsland();
    public static Action<PlayerSaveSlotItem> OnSendSlotItemsToSave;
    public static Action<PlayerSavePlayerStats> OnSendPlayerStatsToSave;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        LoadGame();
        DragDropBase.OnSlotItemDataSend += OnSaveDataRequested;
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

    #region Load Game
    private void LoadGame()
    {
        allGameSaves = SaveGame.Load<AllGameSaveEntities>(playerInfoSaveName);
        if (allGameSaves == null)
        {
            GEM.PrintDebugWarning("no save found");
            OnSlotItemsLoaded?.Invoke(null);
            OnStatsLoaded?.Invoke(null);
            OnPlayerInfoLoaded?.Invoke(null);
        }
        else
        {
            for (int i = 0; i < allGameSaves.playerSaveItems.Count; i++)
            {
                print(allGameSaves.playerSaveItems[i].slotItemsType);
            }
            OnSlotItemsLoaded?.Invoke(allGameSaves.playerSaveItems);
            OnStatsLoaded?.Invoke(allGameSaves.playerSaveStats);
            OnPlayerInfoLoaded?.Invoke(allGameSaves.playerSavePlayerInfo);
            GEM.PrintDebug("game loaded");
        }
    }
    #endregion

    private void RequestForSaveData()
    {
        GEM.PrintDebug("requesting save data");
        allGameSaves = new AllGameSaveEntities();
        RequestSaveData?.Invoke();
        GEM.PrintDebug("all save received");
        SaveCompleteGame();
    }

    #region Save Game
    private void SaveCompleteGame()
    {
        SaveGame.Save<AllGameSaveEntities>(playerInfoSaveName, allGameSaves);
        GEM.PrintDebug("Save Game Complete");
    }
    #endregion


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

    // DropDrag based items
    private void OnSaveDataRequested(PlayerSaveSlotItem playerSaveItem)
    {
        GEM.PrintDebug(playerInfoSaveName);
        allGameSaves.playerSaveItems.Add(playerSaveItem);
    }
}

public class AllGameSaveEntities
{
    public PlayerSavePlayerInfo playerSavePlayerInfo = new PlayerSavePlayerInfo();
    public List<PlayerSaveSlotItem> playerSaveItems = new List<PlayerSaveSlotItem>();
    public PlayerSavePlayerStats playerSaveStats = new PlayerSavePlayerStats();
    public MapSaveIsland mapSaveIslands = new MapSaveIsland();
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

public class PlayerSaveSlotItem
{
    public SlotItemsType slotItemsType;
    public int id;
    public List<SlotItems> slotItems = new List<SlotItems>();
    public PlayerSaveSlotItem(SlotItemsType slotItemsType, int id, List<SlotItems> inventoryItems)
    {
        this.slotItemsType = slotItemsType;
        this.id = id;
        this.slotItems = inventoryItems;
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