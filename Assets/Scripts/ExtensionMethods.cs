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

    //MapItem version
    public static MaqItem[,] SingleToMulti(this MaqItem[] array, int width, int height)
    {
        int index = 0;
        MaqItem[,] multi = new MaqItem[width, height];
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                //have to interchange x & y below as the retrival is reversed
                multi[y, x] = new MaqItem();
                multi[y, x].item = array[index].item;
                multi[y, x].structtId = array[index].structtId;
                multi[y, x].tessId = array[index].tessId;
                index++;
            }
        }
        return multi;
    }

    public static MaqItem[] MultiToSingle(this MaqItem[,] array, int width, int height)
    {
        int index = 0;
        MaqItem[] single = new MaqItem[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                single[index] = new MaqItem();
                single[index].item = array[x, y].item;
                single[index].structtId = array[x, y].structtId;
                single[index].tessId = array[x, y].tessId;
                index++;
            }
        }
        return single;
    }
}