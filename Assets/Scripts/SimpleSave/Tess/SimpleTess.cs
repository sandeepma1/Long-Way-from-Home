using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.UI;

public class SimpleTess : MonoBehaviour
{
    public Text text;
    private readonly string idtentifier = "SimpleTess";
    private int widht = 32, height = 64;

    private void Start()
    {
        Save();
    }

    private void Save()
    {
        int[,] data = new int[widht, height];

        for (int i = 0; i < widht; i++)
        {
            for (int j = 0; j < height; j++)
            {
                data[i, j] = Random.Range(0, 255);
            }
        }
        Tess tess = new Tess
        {
            width = widht,
            height = height,
            tessId = 0,
            data = data.MultiToSingle(widht, height)
        };
        SaveGame.Save<Tess>(idtentifier, tess);//, true, "pass", serializer, encoder, encoding, savePath);
        Load();
    }

    private void Load()
    {
        Tess inItems = SaveGame.Load<Tess>(idtentifier);//, new Tess(), true, "pass", serializer, encoder, encoding, savePath);
        text.text = inItems.tessId.ToString() + " " + inItems.data.SingleToMulti(widht, height).LongLength.ToString();
    }
}

[System.Serializable]
public struct Tess
{
    public int width;
    public int height;
    public int tessId;
    public int[] data;
}