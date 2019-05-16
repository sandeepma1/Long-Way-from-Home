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

    private MapTile InstantiateTerrianTile(int id, int posX, int posY)
    {
        int mapTileId = mapSaveIsland.mapData.mapTiles[id];
        MapTile mapItems = Instantiate(PrefabBank.mapTilePrefab, transform);
        mapItems.Init(mapTileId);
        mapItems.transform.localPosition = new Vector3(posX, posY);
        //adding random rotation Z to not appear texture pattern
        float randomRotation = UnityEngine.Random.Range(0, 360);
        mapItems.transform.eulerAngles = new Vector3(mapItems.transform.eulerAngles.x, mapItems.transform.eulerAngles.y, randomRotation);
        return mapItems;
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
                MapItemChopable mapItem = Instantiate(PrefabBank.mapItemChopablePrefab, this.transform);
                mapItem.Init(id);
                mapItem.transform.position = new Vector2(posX, posY);
                break;
            case Actions.mineable:
                MapItemMineable mapItemMineable = Instantiate(PrefabBank.mapItemMineablePrefab, this.transform);
                mapItemMineable.Init(id);
                mapItemMineable.transform.position = new Vector2(posX, posY);
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
                mapItemPickable.Init(id);
                mapItemPickable.transform.position = new Vector2(posX, posY);
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
        //MapItemBase mapItems = Instantiate(PrefabBank.mapTilePrefab, transform);
        //mapItems.Init(mapItemId);
        //mapItems.transform.localPosition = new Vector3(posX, posY);
        //GameObject mapItem = Instantiate(mapSaveIsland.mapData.mapItems[prefabIndex], transform);
        //mapItem.transform.localPosition = new Vector3(x, y);
    }

}
