using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapGenerator))]
public class CreateNewMap : MonoBehaviour
{
    public BiomeName biomeName = BiomeName.Normal; // TODO: make it as per island traveled **imp feature
    private MapGenerator mapGenerator;
    private MapData mapData = new MapData();

    private void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();
        UiStartMenu.OnNewGameClicked += CreateNewMapAndSave;
    }

    private void OnDestroy()
    {
        UiStartMenu.OnNewGameClicked -= CreateNewMapAndSave;
    }

    private void CreateNewMapAndSave()
    {
        //Delete all save before creating new game
        MasterSave.CreatingNewGame();
        //Create procedural map
        mapData = mapGenerator.CreateMaps(biomeName);
        //ConvertArrayBlockCopy(mapData.mapTiles, mapTiles2d, mapData.mapTiles.Length);
        //Create random map itmes
        CreateRandomMapItems();

        //Start saving complete map
        MapSave mapSave = new MapSave(0, mapData.mapSize, mapData.mapTiles, CreateRandomMapItems());
        GEM.PrintDebug("new map create with size" + mapData.mapSize);
        MasterSave.SaveMapData(mapSave);
        //Load main scene
        SceneLoader.LoadScene(SceneNames.MainGameScene);
    }

    private Dictionary<int, MapItem> CreateRandomMapItems()
    {
        Dictionary<int, MapItem> mapItems = new Dictionary<int, MapItem>();
        for (int i = 0; i < mapData.mapTiles.Length; i++)
        {
            List<Grows> allGrows = MapTilesDatabase.GetAllGrowsById(mapData.mapTiles[i]);
            if (allGrows.Count > 0)
            {
                int? mapItemId = CalculateMapItemByTile(allGrows);
                if (mapItemId != null)
                {
                    mapItems.Add(i, new MapItem((int)mapItemId, -1, MapItemsDatabase.GetHealthPointsById((int)mapItemId)));
                }
            }
        }
        return mapItems;
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

    private void ConvertArrayBlockCopy(Array src, Array dst, int oneDArraySize)
    {
        System.Buffer.BlockCopy(src, 0, dst, 0, oneDArraySize * 4);
    }
}
