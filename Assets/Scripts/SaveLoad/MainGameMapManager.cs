using BayatGames.SaveGameFree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameMapManager : MonoBehaviour
{
    private static MapSaveIsland mapSaveIsland = new MapSaveIsland();
    private static int mapWidth;
    private static int mapHeight;
    private static GameObject[,] mapItemsGO;

    private void Start()
    {
        LoadMap();
        MasterSave.RequestSaveData += RequestSaveData;
    }

    private void RequestSaveData()
    {
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
            GenerateMap();
            StartCoroutine(GenerateGeometryWithDelay());
        }
    }

    private IEnumerator GenerateGeometryWithDelay()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<CompositeCollider2D>().GenerateGeometry();
    }

    private void GenerateMap()
    {
        mapWidth = mapSaveIsland.mapData.mapWidth;
        mapHeight = mapSaveIsland.mapData.mapHeight;
        mapItemsGO = new GameObject[mapWidth, mapHeight];
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                int oneDIndex = j * mapWidth + i;
                //init mapTiles                
                InstantiateTerrianTile(oneDIndex, i, j);
                //randomly place map items
                int mapItemId = mapSaveIsland.mapData.mapItems[oneDIndex];
                if (mapItemId >= 0)
                {
                    InstantiateMapItem(mapItemId, i, j);
                }
            }
        }
    }

    private void InstantiateTerrianTile(int id, int posX, int posY)
    {
        int mapTileId = mapSaveIsland.mapData.mapTiles[id];
        if (mapTileId == 0)
        {
            return;
        }
        MapTile mapItems = Instantiate(PrefabBank.mapTilePrefab, transform);
        mapItems.Init(mapTileId, posX, posY);
    }

    private void InstantiateMapItem(int id, int posX, int posY)
    {
        Actions action = MapItemsDatabase.GetActionById(id);
        if (action == Actions.none)
        {
            return;
        }
        switch (action)
        {
            case Actions.chopable:
                MapItemChopable mapItemChopable = Instantiate(PrefabBank.mapItemChopablePrefab, this.transform);
                mapItemChopable.Init(id, posX, posY, mapHeight);
                mapItemsGO[posX, posY] = mapItemChopable.gameObject;
                break;
            case Actions.mineable:
                MapItemMineable mapItemMineable = Instantiate(PrefabBank.mapItemMineablePrefab, this.transform);
                mapItemMineable.Init(id, posX, posY, mapHeight);
                mapItemsGO[posX, posY] = mapItemMineable.gameObject;
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
                mapItemPickable.Init(id, posX, posY, mapHeight);
                mapItemsGO[posX, posY] = mapItemPickable.gameObject;
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
    public static GameObject GetMapItemGameObjectByPosition(Vector2 pos)
    {
        return mapItemsGO?[(int)pos.x, (int)pos.y];
    }

    public static void MapItemDone(int x, int y)
    {
        int oneDIndex = y * mapWidth + x;
        int mapItem = mapSaveIsland.mapData.mapItems[oneDIndex];
        print(MapItemsDatabase.GetSlugById(mapItem) + "removed");
        mapSaveIsland.mapData.mapItems[oneDIndex] = -1;
        mapItemsGO[x, y] = null;
    }
    #endregion
}
