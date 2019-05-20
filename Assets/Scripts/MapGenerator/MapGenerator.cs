using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Renderer mainRenderer;
    [SerializeField] private Noise.NormalizeMode normalizeMode;
    [SerializeField] private int mapSize = 64;
    [SerializeField] private float noiseScale = 15;
    [SerializeField] private int octaves = 8;
    [Range(0, 1)] [SerializeField] private float persistance = 0;
    [SerializeField] private float lacunarity = 1;
    [SerializeField] private int seed;
    [SerializeField] private Vector2 falloff = new Vector2(2, 6);
    [SerializeField] private bool useFalloff = true;
    //[SerializeField] private BiomeType[] regions = new BiomeType[6];
    private float[,] falloffMap;

    //private void Start()
    //{
    //    regions[0] = new BiomeType(0, Color.blue);
    //    regions[1] = new BiomeType(0.1f, Color.cyan);
    //    regions[2] = new BiomeType(0.285f, Color.yellow);
    //    regions[3] = new BiomeType(0.4f, Color.green);
    //    regions[4] = new BiomeType(0.8f, Color.red);
    //    regions[5] = new BiomeType(1, Color.gray);
    //}

    public MapData CreateMaps(BiomeName biomeName)
    {
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapSize, mapSize, falloff);
        MapDataArray mapDataArray = GenerateMapData(Vector2.zero, biomeName);
        //Texture2D finalTexture = TextureGenerator.TextureFromColourMap(mapDataArray.colourMap, mapSizeX, mapSizeY);
        //mainRenderer.material.mainTexture = finalTexture;
        MapData mapData = new MapData(mapSize, mapDataArray.mapTiles);
        return mapData;
    }

    private MapDataArray GenerateMapData(Vector2 centre, BiomeName biomeName)
    {
        List<Regions> regions = MapBiomesDatabase.GetRegionsByBiomeName(biomeName);
        seed = (int)System.DateTime.Now.Ticks;
        float[,] noiseMap = Noise.GenerateNoiseMap(mapSize, mapSize, seed,
            noiseScale, octaves, persistance, lacunarity, centre, normalizeMode);
        int[] intMap = new int[mapSize * mapSize];
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Count; i++)
                {
                    if (currentHeight >= regions[i].height)
                    {
                        intMap[y * mapSize + x] = i;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        return new MapDataArray(noiseMap, intMap);
    }
}

public class MapDataArray
{
    public readonly float[,] heightMap;
    public readonly int[] mapTiles;
    public MapDataArray() { }
    public MapDataArray(float[,] heightMap, int[] mapTiles)
    {
        this.heightMap = heightMap;
        this.mapTiles = mapTiles;
    }
}

public class MapData
{
    public int mapSize;
    public int[] mapTiles;
    public MapData() { }
    public MapData(int mapSize, int[] mapTiles)
    {
        this.mapSize = mapSize;
        this.mapTiles = mapTiles;
    }
}