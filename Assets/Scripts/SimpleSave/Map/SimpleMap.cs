using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Encoders;
using BayatGames.SaveGameFree.Serializers;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static BayatGames.SaveGameFree.SaveGameAuto;

public class SimpleMap : MonoBehaviour
{
    public bool doEncode;
    public bool passwordProtected;
    public Text text;
    public SaveFormat format = SaveFormat.JSON;
    public ISaveGameSerializer serializer;
    public ISaveGameEncoder encoder;
    public Encoding encoding;
    public SaveGamePath savePath = SaveGamePath.PersistentDataPath;
    private string idtentifier = "SimpleSaveMap";
    public byte width = 32, height = 32;

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
        Save();
        Load();
    }

    private void Save()
    {
        MapItem[,] mapItem = new MapItem[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                mapItem[i, j] = new MapItem();
                mapItem[i, j].tessId = (byte)Random.Range(0, 255);
                mapItem[i, j].iteoId = new Iteo((short)Random.Range(0, 255), (short)Random.Range(0, 255));
                mapItem[i, j].structtId = (short)Random.Range(0, 255);
            }
        }

        Map inItems;
        inItems = new Map(width, height, 0, mapItem.MultiToSingle(width, height));
        if (doEncode)
        {
            SaveGame.Save<Map>(idtentifier, inItems, passwordProtected, "pass", serializer, encoder, encoding, savePath);
        }
        else
        {
            SaveGame.Save<Map>(idtentifier, inItems);//, true, "pass", serializer, encoder, encoding, savePath);
        }
    }

    private void Load()
    {
        Map inItems;
        if (doEncode)
        {
            inItems = SaveGame.Load<Map>(idtentifier, new Map(), passwordProtected, "pass", serializer, encoder, encoding, savePath);
        }
        else
        {
            inItems = SaveGame.Load<Map>(idtentifier);
        }
        text.text = inItems.mapId.ToString() + " " + inItems.mapItems.SingleToMulti(inItems.width, inItems.height).LongLength.ToString();
    }

    private void OnApplicationQuit()
    {
        //Save();
        //print("Saved");
    }
}

[System.Serializable]
public class Map
{
    public byte width;
    public byte height;
    public byte mapId;
    public MapItem[] mapItems;
    public Map() { }
    public Map(byte _width, byte _height, byte _mapId, MapItem[] _mapItems)
    {
        width = _width;
        height = _height;
        mapId = _mapId;
        mapItems = _mapItems;
    }
}
[System.Serializable]
public class MapItem
{
    public byte tessId;
    public Iteo iteoId;
    public short structtId;
    public MapItem() { }
    public MapItem(byte _tessId, Iteo _iteoId, short _structtId)
    {
        tessId = _tessId;
        iteoId = _iteoId;
        structtId = _structtId;
    }
}