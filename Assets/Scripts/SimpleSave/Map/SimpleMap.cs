using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Encoders;
using BayatGames.SaveGameFree.Serializers;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static BayatGames.SaveGameFree.SaveGameAuto;

public class SimpleMap : MonoBehaviour
{
    public bool doEncode;
    public Text text;
    public SaveFormat format = SaveFormat.JSON;
    public ISaveGameSerializer serializer;
    public ISaveGameEncoder encoder;
    public Encoding encoding;
    public SaveGamePath savePath = SaveGamePath.PersistentDataPath;
    private string idtentifier = "SimpleSaveMaq";
    private byte width = 32, height = 64;

    private void Awake()
    {
        if (serializer == null)
        {
            serializer = SaveGame.Serializer;
        }
        if (encoder == null)
        {
            encoder = SaveGame.Encoder;
        }
        if (encoding == null)
        {
            encoding = SaveGame.DefaultEncoding;
        }
        switch (format)
        {
            case SaveFormat.Binary:
                serializer = new SaveGameBinarySerializer();
                break;
            case SaveFormat.JSON:
                serializer = new SaveGameJsonSerializer();
                break;
            case SaveFormat.XML:
                serializer = new SaveGameXmlSerializer();
                break;
        }
    }

    private void Start()
    {
        Invoke("Save", 5f);
        //Load();
    }

    private void Save()
    {
        MaqIteo[,] data = new MaqIteo[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                data[i, j] = new MaqIteo
                {
                    tessId = Random.Range(0, 255),
                    iteo = new Item(Random.Range(0, 255), Random.Range(0, 255)),
                    structtId = (short)Random.Range(0, 255)
                };
            }
        }

        Maq maq = new Maq
        {
            data = data.MultiToSingle(width, height),
            height = height,
            width = width,
            maqId = 0
        };
        if (doEncode)
        {
            SaveGame.Save<Maq>(idtentifier, maq, true, "pass", serializer, encoder, encoding, savePath);
        }
        else
        {
            SaveGame.Save<Maq>(idtentifier, maq);
        }
    }

    private void Load()
    {
        Maq inItems;
        if (doEncode)
        {
            inItems = SaveGame.Load<Maq>(idtentifier, new Maq(), true, "pass", serializer, encoder, encoding, savePath);
        }
        else
        {
            inItems = SaveGame.Load<Maq>(idtentifier);
        }
        text.text = inItems.maqId.ToString() + " " + inItems.data.SingleToMulti(inItems.width, inItems.height).LongLength.ToString();
    }

    private void OnApplicationQuit()
    {
        Save();
        print("Saved");
    }
}

[System.Serializable]
public struct Maq
{
    public int width;
    public int height;
    public int maqId;
    public MaqIteo[] data;
}

[System.Serializable]
public struct MaqIteo
{
    public int tessId;
    public Item iteo;
    public int structtId;
}