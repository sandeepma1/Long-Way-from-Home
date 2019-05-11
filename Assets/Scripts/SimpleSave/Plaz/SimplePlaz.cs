using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
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
            itemInu = new InventoryItem[40]
        };
        for (int i = 0; i < plaz.itemInu.Length; i++)
        {
            //plaz.itemInu[i] = new InuItem
            //{
            //    inuSlotId = Random.Range(0, 40),
            //};
            //plaz.itemInu[i].item = new Item(Random.Range(0, 255), Random.Range(10, 255));
        }
        SaveGame.Save<Plaz>(id, plaz);
    }
}

[System.Serializable]
public struct Plaz
{
    public int maqId;
    public Vector2Byte lastLoc;
    public int currentToom;
    public InventoryItem[] itemInu;
    public int stats1;
    public int stats2;
    public int stats3;//...
}