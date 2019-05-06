using System.Collections;
using UnityEngine;

public static class FalloffGenerator
{
    public static float[,] GenerateFalloffMap(int sizeX, int sizeY, Vector2 falloff)
    {
        float[,] map = new float[sizeX, sizeY];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                float x = i / (float)sizeX * 2 - 1;
                float y = j / (float)sizeY * 2 - 1;
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, j] = Evaluate(value, falloff);
            }
        }
        return map;
    }

    private static float Evaluate(float value, Vector2 falloff)
    {
        return Mathf.Pow(value, falloff.x) / (Mathf.Pow(value, falloff.x) + Mathf.Pow(falloff.y - falloff.y * value, falloff.x));
    }
}
