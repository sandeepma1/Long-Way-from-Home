using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameMapManager : MonoBehaviour
{
    private MapSaveIsland mapSaveIsland = new MapSaveIsland();
    private int mapWidth;
    private int mapHeight;

    private void Start()
    {
        LoadMap();
    }

    private void LoadMap()
    {
        mapSaveIsland = MasterSave.LoadMapIslandData();
        GenerateMap();
    }

    private void GenerateMap()
    {
        mapWidth = mapSaveIsland.mapData.mapWidth;
        mapHeight = mapSaveIsland.mapData.mapHeight;
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                int oneDIndex = j * mapWidth + i;
                //init mapTiles
                int mapTileId = mapSaveIsland.mapData.mapTiles[oneDIndex];
                MapTile mapItems = Instantiate(PrefabBank.mapTilePrefab, transform);
                mapItems.Init(mapTileId);
                mapItems.transform.localPosition = new Vector3(i, j);

                if (mapSaveIsland.mapData.mapItems[oneDIndex] >= 0)
                {
                    InstantiateMapItem(mapSaveIsland.mapData.mapItems[oneDIndex], i, j);
                }
            }
        }
    }


    private void InstantiateMapItem(int prefabIndex, int x, int y)
    {
        //GameObject mapItem = Instantiate(mapSaveIsland.mapData.mapItems[prefabIndex], transform);
        //mapItem.transform.localPosition = new Vector3(x, y);
    }

}
