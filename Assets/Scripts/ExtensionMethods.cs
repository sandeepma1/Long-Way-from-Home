using System.Collections;
using UnityEngine;

public static class ExtensionMethods
{
    // short version
    public static short[,] SingleToMulti(this short[] array, int width, int height)
    {
        int index = 0;
        short[,] multi = new short[width, height];
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

    public static short[] MultiToSingle(this short[,] array, int width, int height)
    {
        int index = 0;
        short[] single = new short[width * height];
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

    //byte version
    public static byte[,] SingleToMulti(this byte[] array, int width, int height)
    {
        int index = 0;
        byte[,] multi = new byte[width, height];
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

    public static byte[] MultiToSingle(this byte[,] array, int width, int height)
    {
        int index = 0;
        //int width = array.GetLength(0);
        //int height = array.GetLength(1);
        byte[] single = new byte[width * height];
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
                multi[y, x].iteoId = array[index].iteoId;
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
                single[index].iteoId = array[x, y].iteoId;
                single[index].structtId = array[x, y].structtId;
                single[index].tessId = array[x, y].tessId;
                index++;
            }
        }
        return single;
    }
}