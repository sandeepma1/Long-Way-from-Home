using System;
using UnityEngine;

[RequireComponent(typeof(MapGenerator))]
public class CreateNewMap : MonoBehaviour
{
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
        mapData = mapGenerator.CreateMaps();
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
        int mapWidth = mapData.mapWidth;
        int mapHeight = mapData.mapHeight;

        mapData.mapItems = new int[mapWidth * mapHeight];

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                int oneDIndex = j * mapWidth + i;
                int mapTileId = mapData.mapTiles[oneDIndex];
                mapData.mapItems[oneDIndex] = -1;
                if (mapTileId == 3) // instantiate only on land
                {
                    int a = UnityEngine.Random.Range(0, 5); // that too probability is 1 in 5
                    if (a < 1)
                    {
                        int random = UnityEngine.Random.Range(0, 12);//TODO: write better logic
                        mapData.mapItems[oneDIndex] = random;
                    }
                }
            }
        }
    }
}
