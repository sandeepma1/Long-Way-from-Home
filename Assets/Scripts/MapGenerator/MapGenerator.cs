using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Renderer mainRenderer;
    [SerializeField] private Noise.NormalizeMode normalizeMode;
    [SerializeField] private int mapSizeX = 64;
    [SerializeField] private int mapSizeY = 64;
    [SerializeField] private float noiseScale = 15;
    [SerializeField] private int octaves = 8;
    [Range(0, 1)] [SerializeField] private float persistance = 0;
    [SerializeField] private float lacunarity = 1;
    [SerializeField] private int seed;
    [SerializeField] private Vector2 falloff = new Vector2(2, 6);
    [SerializeField] private bool useFalloff = true;
    [SerializeField] private BiomeType[] regions = new BiomeType[6];
    private float[,] falloffMap;

    private void Start()
    {
        regions[0] = new BiomeType(0, Color.blue);
        regions[1] = new BiomeType(0.1f, Color.cyan);
        regions[2] = new BiomeType(0.285f, Color.yellow);
        regions[3] = new BiomeType(0.4f, Color.green);
        regions[4] = new BiomeType(0.8f, Color.red);
        regions[5] = new BiomeType(1, Color.gray);
    }

    public MapData CreateMaps()
    {
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapSizeX, mapSizeY, falloff);
        MapDataArray mapDataArray = GenerateMapData(Vector2.zero);
        Texture2D finalTexture = TextureGenerator.TextureFromColourMap(mapDataArray.colourMap, mapSizeX, mapSizeY);
        mainRenderer.material.mainTexture = finalTexture;
        MapData mapData = new MapData(mapSizeX, mapSizeY, mapDataArray.intMap);
        return mapData;
    }

    private MapDataArray GenerateMapData(Vector2 centre)
    {
        seed = (int)System.DateTime.Now.Ticks;
        float[,] noiseMap = Noise.GenerateNoiseMap(mapSizeX, mapSizeY, seed,
            noiseScale, octaves, persistance, lacunarity, centre, normalizeMode);

        Color[] colourMap = new Color[mapSizeX * mapSizeY];
        int[] intMap = new int[mapSizeX * mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (useFalloff)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight >= regions[i].height)
                    {
                        colourMap[y * mapSizeX + x] = regions[i].color;
                        intMap[y * mapSizeX + x] = i;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        return new MapDataArray(noiseMap, colourMap, intMap);
    }
}

[System.Serializable]
public class BiomeType
{
    public float height;
    public Color color;
    public BiomeType(float _height, Color _color)
    {
        height = _height;
        color = _color;
    }
}

public class MapDataArray
{
    public readonly float[,] heightMap;
    public readonly Color[] colourMap;
    public readonly int[] intMap;
    public MapDataArray() { }
    public MapDataArray(float[,] heightMap, Color[] colourMap, int[] intMap)
    {
        this.heightMap = heightMap;
        this.colourMap = colourMap;
        this.intMap = intMap;
    }
}

public class MapData
{
    public int mapWidth;
    public int mapHeight;
    public int[] terrianTiles;
    public int[] mapItems;
    public MapData() { }
    public MapData(int mapWidth, int mapHeight, int[] terrianTiles, int[] mapItems)
    {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        this.terrianTiles = terrianTiles;
        this.mapItems = mapItems;
    }
    public MapData(int mapWidth, int mapHeight, int[] terrianTiles)
    {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        this.terrianTiles = terrianTiles;
    }
}

public class MapItems
{
    public int posX;
    public int posY;
    public int itemId;
}