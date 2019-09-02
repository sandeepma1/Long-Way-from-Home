using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameMapManager : MonoBehaviour
{
    public static Action<int, int, MapItem> OnCreatePlacableMapItem;
    private static MapSave mapSave = new MapSave();
    public static int CurrentMapSize { get; private set; }
    private static MapItemBase[,] mapItemsGO;
    private static MapTile[,] mapTiles;

    private void Start()
    {
        LoadMap();
        MasterSave.RequestSaveData += RequestSaveData;
        OnCreatePlacableMapItem += OnCreatePlacableMapItemClicked;
    }

    private void OnDestroy()
    {
        MasterSave.RequestSaveData -= RequestSaveData;
        OnCreatePlacableMapItem -= OnCreatePlacableMapItemClicked;
    }

    #region Save/Load Stuff
    private void RequestSaveData()
    {
        MasterSave.SaveMapData(mapSave);
    }

    private void LoadMap()
    {
        if (!SaveGame.Exists(GEM.MapSaveName))
        {
            SceneLoader.LoadScene(SceneNames.StartMenuScene);
        }
        else
        {
            mapSave = MasterSave.LoadMapData();
            CurrentMapSize = mapSave.mapSize;
            mapItemsGO = new MapItemBase[CurrentMapSize, CurrentMapSize];
            mapTiles = new MapTile[CurrentMapSize, CurrentMapSize];
            GenerateMapTiles();
            GenerateMapItems();
            StartCoroutine(GenerateGeometryWithDelay());
        }
    }
    #endregion

    #region MapLoading and MapGeneration stuff
    private IEnumerator GenerateGeometryWithDelay()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<CompositeCollider2D>().GenerateGeometry();
    }

    private void GenerateMapTiles()
    {
        for (int i = 0; i < CurrentMapSize; i++)
        {
            for (int j = 0; j < CurrentMapSize; j++)
            {
                int oneDIndex = i + (j * CurrentMapSize);
                InstantiateMapTile(oneDIndex, i, j);
            }
        }
    }

    private void InstantiateMapTile(int id, int posX, int posY)
    {
        int mapTileId = mapSave.mapTiles[id];
        if (mapTileId == 0) //Dont intantiate DeepWater tiles
        {
            return;
        }
        MapTile mapTile = Instantiate(PrefabBank.mapTilePrefab, transform);
        mapTile.Init(mapTileId, posX, posY);
        mapTiles[posX, posY] = mapTile;
    }

    private void GenerateMapItems()
    {
        mapItemsGO = new MapItemBase[CurrentMapSize, CurrentMapSize];
        foreach (KeyValuePair<int, MapItem> mapItem in mapSave.mapItems)
        {
            int x = mapItem.Key % CurrentMapSize;
            int y = mapItem.Key / CurrentMapSize;
            InstantiateMapItem(x, y, mapItem.Value);
        }
    }

    private void OnCreatePlacableMapItemClicked(int posX, int posY, MapItem mapItem)
    {
        MapItemPlaceable mapItemPlaceable = Instantiate(PrefabBank.mapItemPlaceablePrefab, this.transform);
        mapItemPlaceable.Init(posX, posY, mapItem, CurrentMapSize);
        mapItemsGO[posX, posY] = mapItemPlaceable;
        AddNewMapItem(posX, posY, mapItem.mapItemId);
    }

    private void InstantiateMapItem(int posX, int posY, MapItem mapItem)
    {
        ItemType action = MapItemsDatabase.GetItemTypeById(mapItem.mapItemId);
        if (action == ItemType.none)
        {
            return;
        }
        switch (action)
        {
            case ItemType.chopable:
                MapItemChopable mapItemChopable = Instantiate(PrefabBank.mapItemChopablePrefab, this.transform);
                mapItemChopable.Init(posX, posY, mapItem, CurrentMapSize);
                mapItemsGO[posX, posY] = mapItemChopable;
                break;
            case ItemType.mineable:
                MapItemMineable mapItemMineable = Instantiate(PrefabBank.mapItemMineablePrefab, this.transform);
                mapItemMineable.Init(posX, posY, mapItem, CurrentMapSize);
                mapItemsGO[posX, posY] = mapItemMineable;
                break;
            case ItemType.hitable:
                break;
            case ItemType.fishable:
                break;
            case ItemType.breakable:
                break;
            case ItemType.openable:
                break;
            case ItemType.pickable:
                MapItemPickable mapItemPickable = Instantiate(PrefabBank.mapItemPickablePrefab, this.transform);
                mapItemPickable.Init(posX, posY, mapItem, CurrentMapSize);
                mapItemsGO[posX, posY] = mapItemPickable;
                break;
            case ItemType.interactable:
                break;
            case ItemType.moveable:
                break;
            case ItemType.shakeable:
                break;
            case ItemType.placeable:
                break;
            case ItemType.shoveable:
                break;
            case ItemType.cutable:
                break;
            case ItemType.none:
                break;
            case ItemType.harvestable:
                break;
            default:
                break;
        }
    }
    #endregion


    #region Static helper function
    public static MapItemBase GetMapItemByPosition(Vector2 pos)
    {
        return mapItemsGO?[(int)pos.x, (int)pos.y];
    }

    public static bool IsItemPlacableOnMapTileByPosition(Vector2 pos)
    {
        if (mapTiles == null)
        {
            return false;
        }
        if (mapTiles[(int)pos.x, (int)pos.y] == null)
        {
            return false;
        }
        else
        {
            return mapTiles[(int)pos.x, (int)pos.y].isItemPlacable;
        }
    }

    public void AddNewMapItem(int posX, int posY, int mapItemId)
    {
        int key = posX + (posY * mapSave.mapSize);
        if (!mapSave.mapItems.ContainsKey(key))
        {
            mapSave.mapItems.Add(key, CreateMapItemById(mapItemId));
        }
    }

    public static void MapItemHarvestingDone(Vector2 pos, int mapItemId)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int key = x + (y * mapSave.mapSize);
        if (mapSave.mapItems.ContainsKey(key))
        {
            mapSave.mapItems.Remove(key);
        }
        mapItemsGO[x, y] = null;
        DropItemsAfterDone(pos, mapItemId);
    }

    private static void DropItemsAfterDone(Vector2 pos, int mapItemId)
    {
        List<Drops> drops = MapItemsDatabase.GetDropsById(mapItemId);
        for (int i = 0; i < drops.Count; i++)
        {
            if (drops[i].dropId != null)
            {
                MapItemDropedItem mapItemDropedItem = Instantiate(PrefabBank.mapItemDroped, null);
                int dropCountRandom = UnityEngine.Random.Range((int)drops[i].min, (int)drops[i].max);
                mapItemDropedItem.Init(pos, (int)drops[i].dropId, dropCountRandom);
            }
        }
    }

    public static MapItem CreateMapItemById(int id, int furnitureId = -1)
    {
        int mapItemId = InventoryItemsDatabase.GetPlacableMapItemIdById(id);
        int healthPoints = MapItemsDatabase.GetHealthPointsById(mapItemId);
        return new MapItem(mapItemId, furnitureId, healthPoints);
    }

    #endregion
}