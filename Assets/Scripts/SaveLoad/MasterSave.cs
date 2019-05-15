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
    private const string mapSaveName = "MapSaves";
    private MapSaveIsland mapSaveIslands = new MapSaveIsland();
    public static Action<MapSaveIsland> OnSendMapDataToSave;
    public static Action<MapSaveIsland> MapIslandsLoadedData;

    public static Action<PlayerSavePlayerInfo> OnSendPlayerInfoToSave;
    public static Action<PlayerSaveSlotItem> OnSendSlotItemsToSave;
    public static Action<PlayerSavePlayerStats> OnSendPlayerStatsToSave;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        OnSendMapDataToSave += OnSendMapDataToSaveEventHandler;
    }

    private void Start()
    {
        LoadGame();
        DragDropBase.OnSlotItemDataSend += OnSaveDataRequested;
        CreateMap.OnMapDataSend += OnMapDataSend;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            RequestForSaveData();
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

    #region Load Game
    private void LoadGame()
    {
        allGameSaves = SaveGame.Load<AllGameSaveEntities>(playerInfoSaveName);
        if (allGameSaves == null)
        {
            print("no save found");
            OnSlotItemsLoaded?.Invoke(null);
            OnStatsLoaded?.Invoke(null);
            OnPlayerInfoLoaded?.Invoke(null);
            MapIslandsLoadedData?.Invoke(null);
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
            MapIslandsLoadedData?.Invoke(allGameSaves.mapSaveIslands);
            print("game loaded");
        }
    }
    #endregion

    private void RequestForSaveData()
    {
        print("requesting save data");
        allGameSaves = new AllGameSaveEntities();
        RequestSaveData?.Invoke();
        print("all save received");
        SaveCompleteGame();
    }

    #region Save Game
    private void SaveCompleteGame()
    {
        SaveGame.Save<AllGameSaveEntities>(playerInfoSaveName, allGameSaves);
        print("Save Game Complete");
    }
    #endregion


    #region Map Save/Load
    private void OnSendMapDataToSaveEventHandler(MapSaveIsland mapSaveData)
    {
        SaveGame.Save<MapSaveIsland>(mapSaveName, mapSaveData);
        print("Map Save Complete");
    }

    public static MapSaveIsland LoadMapIslandData()
    {
        print("Loading map data...");
        return SaveGame.Load<MapSaveIsland>(mapSaveName);
    }
    #endregion





    // DropDrag based items
    private void OnSaveDataRequested(PlayerSaveSlotItem playerSaveItem)
    {
        print(playerInfoSaveName);
        allGameSaves.playerSaveItems.Add(playerSaveItem);
    }

    //Map Islands generator based items
    private void OnMapDataSend(MapSaveIsland obj)
    {
        allGameSaves.mapSaveIslands = obj;
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
    public int locX;
    public int locY;
    public PlayerSavePlayerInfo()
    { locX = 20; locY = 20; }
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