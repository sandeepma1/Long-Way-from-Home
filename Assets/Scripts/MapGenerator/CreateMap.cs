using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    public static Action<MapSaveIsland> OnMapDataSend;

    [SerializeField] private CompositeCollider2D compositeCollider2D;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private GameObject[] biomesPrefab;
    [SerializeField] private GameObject[] mapItems;
    public MapGenerator mapGenerator;
    private int mapWidth;
    private int mapHeight;
    private MapData mapData = new MapData();

    private void Awake()
    {
        MasterSave.RequestSaveData += RequestSaveData;
        MasterSave.MapIslandsLoadedData += MapIslandsLoadedData;
    }

    private void MapIslandsLoadedData(MapSaveIsland mapSaveIsland)
    {
        if (mapSaveIsland == null)
        {
            mapData = mapGenerator.CreateMaps();
            InstantiateTerrian(true);
        }
        else
        {
            mapData = mapSaveIsland.mapData;
            InstantiateTerrian(false);
        }
        StartCoroutine(GenerateCompositeCollider());
    }

    private void InstantiateTerrian(bool isNewGame)
    {
        mapWidth = mapData.mapWidth;
        mapHeight = mapData.mapHeight;
        if (isNewGame)
        {
            mapData.itemMap = new int[mapWidth * mapHeight];
            mapData.playeItemMap = new int[mapWidth * mapHeight];
        }

        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                int oneDIndex = j * mapWidth + i;
                int terrianId = mapData.terrianMap[oneDIndex];
                GameObject terrianTile = InstantiateTerrianTile(biomesPrefab[terrianId], transform);
                if (isNewGame) // if new game/map
                {
                    mapData.playeItemMap[oneDIndex] = -1;
                    mapData.itemMap[oneDIndex] = -1;
                    if (terrianId == 3) // instantiate only on land
                    {
                        int a = UnityEngine.Random.Range(0, 5); // that too probability is 1 in 5
                        if (a < 1)
                        {
                            int random = UnityEngine.Random.Range(0, mapItems.Length);
                            mapData.itemMap[oneDIndex] = random;
                            InstantiateMapItem(random, i, j);
                        }
                    }
                }
                else
                {
                    if (mapData.itemMap[oneDIndex] >= 0)
                    {
                        InstantiateMapItem(mapData.itemMap[oneDIndex], i, j);
                    }
                }
                terrianTile.transform.localPosition = new Vector3(i, j);
            }
        }
    }

    private void RequestSaveData()
    {
        MapSaveIsland mapSaveIsland = new MapSaveIsland(0, mapData);
        OnMapDataSend?.Invoke(mapSaveIsland);
    }

    private GameObject InstantiateTerrianTile(GameObject prefab, Transform parent)
    {
        GameObject mapItem = Instantiate(prefab, this.transform);
        //adding random rotation Z to not appear texture pattern
        float randomRotation = UnityEngine.Random.Range(0, 360);
        mapItem.transform.eulerAngles = new Vector3(mapItem.transform.eulerAngles.x, mapItem.transform.eulerAngles.y, randomRotation);
        return mapItem;
    }

    private void InstantiateMapItem(int prefabIndex, int x, int y)
    {
        GameObject mapItem = Instantiate(mapItems[prefabIndex], transform);
        mapItem.transform.localPosition = new Vector3(x, y);
    }


    private IEnumerator GenerateCompositeCollider()
    {
        yield return new WaitForSeconds(3);
        compositeCollider2D.GenerateGeometry();
    }

    private void SetCompositeCollider(bool flag)
    {
        compositeCollider2D.enabled = flag;
    }
}
