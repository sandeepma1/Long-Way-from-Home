using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemGenerator : MonoBehaviour
{
    public MapItemPickable mapItemPickablePrefab;
    public List<MapItemPickable> mapItemPickables;

    private void GenerateMapItems()
    {
        MapItems[] mapItems = MapItemsDatabase.GetMapItemsArray();
        for (int i = 0; i < mapItems.Length; i++)
        {
            if (mapItems[i].primaryAction == Actions.pickable)
            {
                MapItemPickable mapItemPickable = Instantiate(mapItemPickablePrefab, this.transform);
                mapItemPickable.Init(i);
                mapItemPickables.Add(mapItemPickable);
            }
        }
    }

    public static void GenerateMapTiles()
    {

    }
}