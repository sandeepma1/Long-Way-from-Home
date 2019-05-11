using BayatGames.SaveGameFree;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inu : MonoBehaviour
{
    [SerializeField] private InuBg inuBgPrefab;
    [SerializeField] private InuItem inuItemPrefab;
    [SerializeField] private Transform iteoParent;
    private InuBg[] inuBgs;
    private InuItem draggedItem = null;
    private const string inuName = "AllInu";
    private const int count = 18;
    private const int staterIteos = 10;
    private const int stdStack = 20;
    private List<InuItem> inuItems = new List<InuItem>();

    private void Start()
    {
        inuBgs = new InuBg[count];
        for (int i = 0; i < count; i++)
        {
            InuBg inuBg = Instantiate(inuBgPrefab, iteoParent);
            inuBg.id = i;
            inuBg.OnBgDrop += OnBgDrop;
            inuBgs[i] = inuBg;
        }
        LoadInu();
    }

    private void OnApplicationQuit()
    {
        SaveInu();
    }

    private void LoadInu()
    {
        AllInuIteos loader = SaveGame.Load<AllInuIteos>(inuName);
        if (loader.iteoInus == null)
        {
            for (int i = 0; i < staterIteos; i++)
            {
                AddNewIteo(i, CreateRanIteo());
            }
        }
        else
        {
            for (int i = 0; i < loader.iteoInus.Count; i++)
            {
                AddNewIteo(loader.iteoInus[i].invSlotId, loader.iteoInus[i].item);
            }
        }
    }

    private void SaveInu()
    {
        AllInuIteos allInuIteos;
        allInuIteos.iteoInus = new List<InventoryIteo>();
        for (int i = 0; i < inuItems.Count; i++)
        {
            allInuIteos.iteoInus.Add(inuItems[i].inuIteo);
        }
        SaveGame.Save<AllInuIteos>(inuName, allInuIteos);
    }

    private void IncrementIteo(int listId, int count)
    {
        inuItems[listId].inuIteo.item.duraCount += count;
        inuItems[listId].UpdateTextValues();
    }

    private void AddNewIteo(int bgId, Item iteo)
    {
        inuBgs[bgId].isOccupied = true;
        InuItem item = Instantiate(inuItemPrefab, inuBgs[bgId].transform);
        item.inuIteo.item = iteo;
        item.UpdateTextValues();
        item.inuIteo.invSlotId = bgId;
        item.OnItemDrop += OnItemDrop;
        item.OnItemDrag += OnItemDrag;
        inuItems.Add(item);
    }

    private void AddNewIteo(Item iteo)
    {
        //if already exists, then increment and/or add
        List<InuItem> sameItems = new List<InuItem>();
        for (int i = 0; i < inuItems.Count; i++)
        {
            if (inuItems[i].inuIteo.item.id == iteo.id)
            {
                sameItems.Add(inuItems[i]);
            }
        }
        if (sameItems.Count > 0)
        {
            print("found same " + sameItems.Count);
            CompareAndIncrementAndOrAdd(sameItems, iteo);
        }
        //if does not exists, then add new
        FindEmptyAndAdd(iteo);
    }

    private void CompareAndIncrementAndOrAdd(List<InuItem> sameList, Item iteoToAdd)
    {
        //go through all same iteos and increment (if possible)
        for (int i = 0; i < sameList.Count; i++)
        {
            if (sameList[i].inuIteo.item.duraCount > stdStack)
            {
                continue;
            }
            else
            {
                int canAdd = stdStack - sameList[i].inuIteo.item.duraCount;
                iteoToAdd.duraCount -= canAdd;
                if (iteoToAdd.duraCount > 0)
                {
                    sameList[i].inuIteo.item.duraCount = canAdd;
                    continue;
                }
            }
        }
        //else create new
        if (iteoToAdd.duraCount > 0)
        {
            FindEmptyAndAdd(iteoToAdd);
        }
    }

    private void FindEmptyAndAdd(Item iteo)
    {
        for (int i = 0; i < inuBgs.Length; i++)
        {
            if (!inuBgs[i].isOccupied)
            {
                AddNewIteo(i, iteo);
                return;
            }
        }
        //No space
        print("no empty");
    }

    #region All Actions functions

    private void OnItemDrag(InuItem inuItem)
    {
        draggedItem = inuItem;
    }

    private void OnItemDrop(InuItem inuItem)
    {
        int draggedBgId = draggedItem.inuIteo.invSlotId;
        int currentBgId = inuItem.inuIteo.invSlotId;
        int draggedItemId = draggedItem.inuIteo.item.id;
        int currentItemId = inuItem.inuIteo.item.id;
        int draggedDuraCount = draggedItem.inuIteo.item.duraCount;
        int currentDuraCount = inuItem.inuIteo.item.duraCount;

        if (draggedItemId == currentItemId)//merge
        {
            if (draggedDuraCount + currentDuraCount > stdStack)
            {
                int balanceAmt = (draggedDuraCount + currentDuraCount) - stdStack;
                inuItem.inuIteo.item.duraCount = stdStack;
                draggedItem.inuIteo.item.duraCount = balanceAmt;
                inuItem.UpdateTextValues();
                draggedItem.UpdateTextValues();
            }
            else
            {
                inuItem.inuIteo.item.duraCount += draggedItem.inuIteo.item.duraCount;
                inuItem.UpdateTextValues();
                inuItems.Remove(draggedItem);
                inuBgs[draggedBgId].isOccupied = false;
                Destroy(draggedItem.gameObject);
            }
        }
        else // swap
        {
            draggedItem.transform.SetParent(inuBgs[currentBgId].transform);
            draggedItem.inuIteo.invSlotId = currentBgId;
            inuItem.transform.SetParent(inuBgs[draggedBgId].transform);
            inuItem.transform.localPosition = Vector3.zero;
            inuItem.inuIteo.invSlotId = draggedBgId;
        }
    }

    private void OnBgDrop(int id)
    {
        inuBgs[draggedItem.inuIteo.invSlotId].isOccupied = false;
        draggedItem.transform.SetParent(inuBgs[id].transform);
        draggedItem.inuIteo.invSlotId = (byte)id;
    }

    #endregion

    #region Temp Functions
    private Item CreateRanIteo()
    {
        Item iteo = new Item(UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(2, 20));
        print("id " + iteo.id + " val " + iteo.duraCount);
        return iteo;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            AddNewIteo(CreateRanIteo());
        }
    }
    #endregion
}

[System.Serializable]
public struct AllInuIteos
{
    public List<InventoryIteo> iteoInus;
}