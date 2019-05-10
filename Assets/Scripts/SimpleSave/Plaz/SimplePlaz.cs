using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlaz : MonoBehaviour
{
    private string id = "TestSaveSystem";
    private void Start()
    {
        Plaz plaz = new Plaz
        {
            currentToom = 1,
            lastLoc = new Vector2Byte(1, 1),
            maqId = 0,
            stats1 = 1,
            stats2 = 2,
            stats3 = 3,
            iteoInu = new InuIteo[40]
        };
        for (int i = 0; i < plaz.iteoInu.Length; i++)
        {
            plaz.iteoInu[i] = new InuIteo
            {
                inuBgId = (byte)Random.Range(0, 40),
            };
            plaz.iteoInu[i].iteo = new Iteo
            {
                id = (byte)Random.Range(0, 255),
                duraCount = (byte)Random.Range(10, 255)
            };
        }
        SaveGame.Save<Plaz>(id, plaz);
    }
}

[System.Serializable]
public struct Plaz
{
    public byte maqId;
    public Vector2Byte lastLoc;
    public byte currentToom;
    public InuIteo[] iteoInu;
    public byte stats1;
    public byte stats2;
    public byte stats3;//...
}