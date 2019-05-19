using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapGenerator))]
public class CreateNewMap : MonoBehaviour
{
    public BiomeName biomeName = BiomeName.Normal; // TODO: make it as per island traveled **imp feature
    private MapGenerator mapGenerator;
    private MapData mapData = new MapData();
    private int[,] mapTiles2d;

    private void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();
        UiStartMenu.OnNewGameClicked += CreateNewMapAndSave;
    }

    private void CreateNewMapAndSave()
    {
        //Delete all save before creating new game
        MasterSave.CreatingNewGame();
        //Create procedural map
        mapData = mapGenerator.CreateMaps(biomeName);
        mapTiles2d = Make2DArray(mapData.mapTiles, mapData.mapWidth, mapData.mapHeight);
        //Create random map itmes
        CreateRandomMapItems();

        //Start saving complete map
        MapSaveIsland mapSaveIsland = new MapSaveIsland(0, mapData);
        print("new map create " + mapData.mapHeight);
        MasterSave.SaveMapData(mapSaveIsland);
        //Load main scene
        SceneLoader.LoadScene(SceneNames.MainGameScene);
    }

    private void CreateRandomMapItems()
    {
        mapData.mapItems = new List<MapItem>();
        for (int i = 0; i < mapData.mapWidth; i++)
        {
            for (int j = 0; j < mapData.mapHeight; j++)
            {
                int mapTileId = mapTiles2d[i, j];
                List<Grows> allGrows = MapTilesDatabase.GetAllGrowsById(mapTileId);
                if (allGrows.Count > 0)
                {
                    int? mapItemId = CalculateMapItemByTile(allGrows);
                    if (mapItemId != null)
                    {
                        mapData.mapItems.Add(new MapItem((int)mapItemId, i, j, -1));
                    }
                }
            }
        }
    }

    //TODO: implement a good probability distribution system.
    private int? CalculateMapItemByTile(List<Grows> allGrows)
    {
        int pickRandomIdIndex = UnityEngine.Random.Range(0, allGrows.Count);
        float probability = UnityEngine.Random.Range(0.0f, 1.0f);
        if (allGrows[pickRandomIdIndex].prob >= probability)
        {
            return allGrows[pickRandomIdIndex].growId;
        }
        return null;
    }

    //TODO: research on  buffer.blockcopy() as it is fast
    private T[,] Make2DArray<T>(T[] input, int height, int width)
    {
        T[,] output = new T[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                output[i, j] = input[j * height + i];
            }
        }
        return output;
    }
}
