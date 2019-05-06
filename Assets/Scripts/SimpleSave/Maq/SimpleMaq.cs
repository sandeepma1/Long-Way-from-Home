using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Encoders;
using BayatGames.SaveGameFree.Serializers;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static BayatGames.SaveGameFree.SaveGameAuto;

public class SimpleMaq : MonoBehaviour
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
                data[i, j] = new MaqIteo();
                data[i, j].tessId = (byte)Random.Range(0, 255);
                data[i, j].iteoId = new Iteo((short)Random.Range(0, 255), (short)Random.Range(0, 255));
                data[i, j].structtId = (short)Random.Range(0, 255);
            }
        }

        Maq inItems;
        inItems = new Maq(width, height, 0, data.MultiToSingle(width, height));
        if (doEncode)
        {
            SaveGame.Save<Maq>(idtentifier, inItems, true, "pass", serializer, encoder, encoding, savePath);
        }
        else
        {
            SaveGame.Save<Maq>(idtentifier, inItems);//, true, "pass", serializer, encoder, encoding, savePath);
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
public class Maq
{
    public byte width;
    public byte height;
    public byte maqId;
    public MaqIteo[] data;
    public Maq() { }
    public Maq(byte _width, byte _height, byte _maqId, MaqIteo[] _data)
    {
        width = _width;
        height = _height;
        maqId = _maqId;
        data = _data;
    }
}
[System.Serializable]
public class MaqIteo
{
    public byte tessId;
    public Iteo iteoId;
    public short structtId;
    public MaqIteo() { }
    public MaqIteo(byte _tessId, Iteo _iteoId, short _structtId)
    {
        tessId = _tessId;
        iteoId = _iteoId;
        structtId = _structtId;
    }
}