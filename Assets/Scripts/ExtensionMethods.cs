using System.Collections;
using UnityEngine;

public static class ExtensionMethods
{
    // int version
    public static int[,] SingleToMulti(this int[] array, int width, int height)
    {
        int index = 0;
        int[,] multi = new int[width, height];
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                //have to interchange x & y below as the retrival is reversed
                multi[y, x] = array[index];
                index++;
            }
        }
        return multi;
    }

    public static int[] MultiToSingle(this int[,] array, int width, int height)
    {
        int index = 0;
        int[] single = new int[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                single[index] = array[x, y];
                index++;
            }
        }
        return single;
    }

    //MapIteo version
    public static MaqIteo[,] SingleToMulti(this MaqIteo[] array, int width, int height)
    {
        int index = 0;
        MaqIteo[,] multi = new MaqIteo[width, height];
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                //have to interchange x & y below as the retrival is reversed
                multi[y, x] = new MaqIteo();
                multi[y, x].iteo = array[index].iteo;
                multi[y, x].structtId = array[index].structtId;
                multi[y, x].tessId = array[index].tessId;
                index++;
            }
        }
        return multi;
    }

    public static MaqIteo[] MultiToSingle(this MaqIteo[,] array, int width, int height)
    {
        int index = 0;
        MaqIteo[] single = new MaqIteo[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                single[index] = new MaqIteo();
                single[index].iteo = array[x, y].iteo;
                single[index].structtId = array[x, y].structtId;
                single[index].tessId = array[x, y].tessId;
                index++;
            }
        }
        return single;
    }
}