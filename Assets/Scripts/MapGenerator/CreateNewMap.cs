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

    private void CreateNewMapAndSave()
    {
        //Create procedural map
        mapData = mapGenerator.CreateMaps(biomeName);
        //Create random map itmes
        CreateRandomMapItems();

        //Start saving complete map
        MapSaveIsland mapSaveIsland = new MapSaveIsland(0, mapData);
        print("new map create " + mapData.mapHeight);
        MasterSave.OnSendMapDataToSave?.Invoke(mapSaveIsland);
        //Load main scene
        SceneLoader.LoadScene(SceneNames.MainGameScene);
    }

    private void CreateRandomMapItems()
    {
        int mapLength = mapData.mapWidth * mapData.mapHeight;
        mapData.mapItems = new int[mapLength];

        for (int i = 0; i < mapData.mapItems.Length; i++)
        {
            int mapTileId = mapData.mapTiles[i];
            mapData.mapItems[i] = CalculateMapItemByTile(mapTileId);
        }
    }

    private int CalculateMapItemByTile(int mapTileId)
    {
        List<Grows> allGrows = MapTilesDatabase.GetAllGrowsById(mapTileId);
        if (allGrows.Count > 0)
        {
            int pickRandomIdIndex = UnityEngine.Random.Range(0, allGrows.Count);
            float probability = UnityEngine.Random.Range(0.0f, 1.0f);
            if (allGrows[pickRandomIdIndex].prob >= probability)
            {
                return (int)allGrows[pickRandomIdIndex].growId;
            }
        }
        return -1;
    }
}
