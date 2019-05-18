using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMapEditor : MonoBehaviour
{
    public BiomeName biomeName = BiomeName.Normal;

    public void GenerateMap()
    {
        FindObjectOfType<MapGenerator>().CreateMaps(biomeName);
    }
}
