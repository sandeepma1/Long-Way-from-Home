using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameMapManager : MonoBehaviour
{
    private static MapSave mapSave = new MapSave();
    public static int CurrentMapSize { get; private set; }
    private static MapItemBase[,] mapItemsGO;

    private void Start()
    {
        LoadMap();
        MasterSave.RequestSaveData += RequestSaveData;
    }

    private void OnDestroy()
    {
        MasterSave.RequestSaveData -= RequestSaveData;
    }

    private void RequestSaveData()
    {
        MasterSave.SaveMapData(mapSave);
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
            mapSave = MasterSave.LoadMapData();
            CurrentMapSize = mapSave.mapSize;
            mapItemsGO = new MapItemBase[CurrentMapSize, CurrentMapSize];
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
        MapTile mapItems = Instantiate(PrefabBank.mapTilePrefab, transform);
        mapItems.Init(mapTileId, posX, posY);
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

    private void InstantiateMapItem(int posX, int posY, MapItem mapItem)
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
                mapItemChopable.Init(posX, posY, mapItem, CurrentMapSize);
                mapItemsGO[posX, posY] = mapItemChopable;
                break;
            case Actions.mineable:
                MapItemMineable mapItemMineable = Instantiate(PrefabBank.mapItemMineablePrefab, this.transform);
                mapItemMineable.Init(posX, posY, mapItem, CurrentMapSize);
                mapItemsGO[posX, posY] = mapItemMineable;
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
                mapItemPickable.Init(posX, posY, mapItem, CurrentMapSize);
                mapItemsGO[posX, posY] = mapItemPickable;
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
    public static MapItemBase GetMapItemPosition(Vector2 pos)
    {
        return mapItemsGO?[(int)pos.x, (int)pos.y];
    }

    public static void MapItemDone(int x, int y)
    {
        int key = x + (y * mapSave.mapSize);

        if (mapSave.mapItems.ContainsKey(key))
        {
            mapSave.mapItems.Remove(key);
        }
        mapItemsGO[x, y] = null;
    }
    #endregion
}