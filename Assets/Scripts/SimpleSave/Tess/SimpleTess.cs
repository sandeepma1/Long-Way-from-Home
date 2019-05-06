using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.UI;

public class SimpleTess : MonoBehaviour
{
    public Text text;
    private readonly string idtentifier = "SimpleTess";
    private byte widht = 32, height = 64;

    private void Start()
    {
        Save();
    }

    private void Save()
    {
        byte[,] data = new byte[widht, height];

        for (int i = 0; i < widht; i++)
        {
            for (int j = 0; j < height; j++)
            {
                data[i, j] = (byte)Random.Range(0, 255);
            }
        }
        Tess tess = new Tess(widht, height, 0, data.MultiToSingle(widht, height));
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
public class Tess
{
    public byte width;
    public byte height;
    public byte tessId;
    public byte[] data;
    public Tess() { }
    public Tess(byte _width, byte _height, byte _tessId, byte[] _data)
    {
        width = _width;
        height = _height;
        tessId = _tessId;
        data = _data;
    }
}