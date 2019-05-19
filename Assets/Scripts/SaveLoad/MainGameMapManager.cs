using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MainGameMapManager : MonoBehaviour
{
    private static MapSaveIsland mapSaveIsland = new MapSaveIsland();
    public static int CurrentMapWidth { get; private set; }
    public static int CurrentMapHeight { get; private set; }
    private static MapItemBase[,] mapItemsGO;

    private void Start()
    {
        LoadMap();
        MasterSave.RequestSaveData += RequestSaveData;
    }

    private void RequestSaveData()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        mapSaveIsland.mapData.mapItems = new List<MapItem>();
        for (int i = 0; i < CurrentMapHeight; i++)
        {
            for (int j = 0; j < CurrentMapWidth; j++)
            {
                if (mapItemsGO[i, j] != null)
                {
                    mapSaveIsland.mapData.mapItems.Add(mapItemsGO[i, j].mapItem);
                }
            }
        }
        MasterSave.SaveMapData(mapSaveIsland);
    }


    #region MapLoading and MapGeneration stuff
    private void LoadMap()
    {
        if (!SaveGame.Exists(GEM.MapSaveName))
        {
            SceneLoader.LoadScene(SceneNames.StartMenuScene);
        }
        else
        {
            mapSaveIsland = MasterSave.LoadMapData();
            GenerateMapTiles();
            GenerateMapItems();
            StartCoroutine(GenerateGeometryWithDelay());
        }
    }

    private IEnumerator GenerateGeometryWithDelay()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<CompositeCollider2D>().GenerateGeometry();
    }

    private void GenerateMapTiles()
    {
        CurrentMapWidth = mapSaveIsland.mapData.mapWidth;
        CurrentMapHeight = mapSaveIsland.mapData.mapHeight;
        mapItemsGO = new MapItemBase[CurrentMapWidth, CurrentMapHeight];
        for (int i = 0; i < CurrentMapWidth; i++)
        {
            for (int j = 0; j < CurrentMapHeight; j++)
            {
                int oneDIndex = j * CurrentMapWidth + i;
                InstantiateTerrianTile(oneDIndex, i, j);
            }
        }
    }

    private void InstantiateTerrianTile(int id, int posX, int posY)
    {
        int mapTileId = mapSaveIsland.mapData.mapTiles[id];
        if (mapTileId == 0) //Dont intantiate DeepWater tiles
        {
            return;
        }
        MapTile mapItems = Instantiate(PrefabBank.mapTilePrefab, transform);
        mapItems.Init(mapTileId, posX, posY);
    }

    private void GenerateMapItems()
    {
        for (int i = 0; i < mapSaveIsland.mapData.mapItems.Count; i++)
        {
            InstantiateMapItem(mapSaveIsland.mapData.mapItems[i]);
        }
    }

    private void InstantiateMapItem(MapItem mapItem)
    {
        Actions action = MapItemsDatabase.GetActionById(mapItem.mapItemId);
        if (action == Actions.none)
        {
            return;
        }
        switch (action)
        {
            case Actions.chopable:
                MapItemChopable mapItemChopable = Instantiate(PrefabBank.mapItemChopablePrefab, this.transform);
                mapItemChopable.Init(mapItem, CurrentMapHeight);
                mapItemsGO[mapItem.posX, mapItem.posY] = mapItemChopable;
                break;
            case Actions.mineable:
                MapItemMineable mapItemMineable = Instantiate(PrefabBank.mapItemMineablePrefab, this.transform);
                mapItemMineable.Init(mapItem, CurrentMapHeight);
                mapItemsGO[mapItem.posX, mapItem.posY] = mapItemMineable;
                break;
            case Actions.hitable:
                break;
            case Actions.fishable:
                break;
            case Actions.breakable:
                break;
            case Actions.openable:
                break;
            case Actions.pickable:
                MapItemPickable mapItemPickable = Instantiate(PrefabBank.mapItemPickablePrefab, this.transform);
                mapItemPickable.Init(mapItem, CurrentMapHeight);
                mapItemsGO[mapItem.posX, mapItem.posY] = mapItemPickable;
                break;
            case Actions.interactable:
                break;
            case Actions.moveable:
                break;
            case Actions.shakeable:
                break;
            case Actions.placeable:
                break;
            case Actions.shoveable:
                break;
            case Actions.cutable:
                break;
            case Actions.none:
                break;
            case Actions.harvestable:
                break;
            default:
                break;
        }
    }
    #endregion


    #region Static helper function
    public static MapItemBase GetMapItemGameObjectByPosition(Vector2 pos)
    {
        return mapItemsGO?[(int)pos.x, (int)pos.y];
    }

    public static void MapItemDone(int x, int y)
    {
        mapItemsGO[x, y] = null;
    }
    #endregion
}
