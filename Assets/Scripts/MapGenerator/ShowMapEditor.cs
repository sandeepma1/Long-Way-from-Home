using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMapEditor : MonoBehaviour
{
    public void GenerateMap()
    {
        FindObjectOfType<MapGenerator>().CreateMaps();
    }
}
