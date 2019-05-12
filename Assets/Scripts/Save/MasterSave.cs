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
    public static Action<PlayerSaveStats> OnStatsLoaded;
    public static Action<MapSaveIsland> MapIslandsLoadedData;
    private AllGameSaveEntities allPlayerItems = new AllGameSaveEntities();
    private const string saveName = "PlayerName";

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
        allPlayerItems = SaveGame.Load<AllGameSaveEntities>(saveName);
        if (allPlayerItems == null)
        {
            print("no save found");
            OnSlotItemsLoaded?.Invoke(null);
            OnStatsLoaded?.Invoke(null);
            OnPlayerInfoLoaded?.Invoke(null);
            MapIslandsLoadedData?.Invoke(null);
        }
        else
        {
            for (int i = 0; i < allPlayerItems.playerSaveItems.Count; i++)
            {
                print(allPlayerItems.playerSaveItems[i].slotItemsType);
            }
            OnSlotItemsLoaded?.Invoke(allPlayerItems.playerSaveItems);
            OnStatsLoaded?.Invoke(allPlayerItems.playerSaveStats);
            OnPlayerInfoLoaded?.Invoke(allPlayerItems.playerSavePlayerInfo);
            MapIslandsLoadedData?.Invoke(allPlayerItems.mapSaveIslands);
            print("game loaded");
        }
    }
    #endregion

    private void RequestForSaveData()
    {
        print("requesting save data");
        allPlayerItems = new AllGameSaveEntities();
        RequestSaveData?.Invoke();
        print("all save received");
        SaveCompleteGame();
    }

    #region Save Game
    private void SaveCompleteGame()
    {
        SaveGame.Save<AllGameSaveEntities>(saveName, allPlayerItems);
        print("Save Game Complete");
    }
    #endregion

    // DropDrag based items
    private void OnSaveDataRequested(PlayerSaveSlotItem playerSaveItem)
    {
        print(saveName);
        allPlayerItems.playerSaveItems.Add(playerSaveItem);
    }

    //Map Islands generator based items
    private void OnMapDataSend(MapSaveIsland obj)
    {
        allPlayerItems.mapSaveIslands = obj;
    }
}

public class AllGameSaveEntities
{
    public PlayerSavePlayerInfo playerSavePlayerInfo = new PlayerSavePlayerInfo();
    public List<PlayerSaveSlotItem> playerSaveItems = new List<PlayerSaveSlotItem>();
    public PlayerSaveStats playerSaveStats = new PlayerSaveStats();
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

public class PlayerSaveStats
{
    public int health;
    public PlayerSaveStats()
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