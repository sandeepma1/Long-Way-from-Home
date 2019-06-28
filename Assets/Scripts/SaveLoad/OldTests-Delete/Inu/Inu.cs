using BayatGames.SaveGameFree;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inu : MonoBehaviour
{
    [SerializeField] private InuBg inuBgPrefab;
    [SerializeField] private InuItem inuItemPrefab;
    [SerializeField] private Transform itemParent;
    private InuBg[] inuBgs;
    private InuItem draggedItem = null;
    private const string inuName = "AllInu";
    private const int count = 18;
    private const int staterItems = 10;
    private const int stdStack = 20;
    private List<InuItem> inuItems = new List<InuItem>();

    private void Start()
    {
        inuBgs = new InuBg[count];
        for (int i = 0; i < count; i++)
        {
            InuBg inuBg = Instantiate(inuBgPrefab, itemParent);
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
        AllInuItems loader = SaveGame.Load<AllInuItems>(inuName);
        if (loader.itemInus == null)
        {
            for (int i = 0; i < staterItems; i++)
            {
                AddNewItem(i, CreateRanItem());
            }
        }
        else
        {
            for (int i = 0; i < loader.itemInus.Count; i++)
            {
                AddNewItem(loader.itemInus[i].invSlotId, loader.itemInus[i].item);
            }
        }
    }

    private void SaveInu()
    {
        AllInuItems allInuItems;
        allInuItems.itemInus = new List<SlotItems>();
        for (int i = 0; i < inuItems.Count; i++)
        {
            allInuItems.itemInus.Add(inuItems[i].inuItem);
        }
        SaveGame.Save<AllInuItems>(inuName, allInuItems);
    }

    private void IncrementItem(int listId, int count)
    {
        inuItems[listId].inuItem.item.duraCount += count;
        inuItems[listId].UpdateTextValues();
    }

    private void AddNewItem(int bgId, Item itemToAdd)
    {
        inuBgs[bgId].isOccupied = true;
        InuItem item = Instantiate(inuItemPrefab, inuBgs[bgId].transform);
        item.inuItem.item = itemToAdd;
        item.UpdateTextValues();
        item.inuItem.invSlotId = bgId;
        item.OnItemDrop += OnItemDrop;
        item.OnItemDrag += OnItemDrag;
        inuItems.Add(item);
    }

    private void AddNewItem(Item item)
    {
        //if already exists, then increment and/or add
        List<InuItem> sameItems = new List<InuItem>();
        for (int i = 0; i < inuItems.Count; i++)
        {
            if (inuItems[i].inuItem.item.id.Value == item.id.Value)
            {
                sameItems.Add(inuItems[i]);
            }
        }
        if (sameItems.Count > 0)
        {
            print("found same " + sameItems.Count);
            CompareAndIncrementAndOrAdd(sameItems, item);
        }
        //if does not exists, then add new
        FindEmptyAndAdd(item);
    }

    private void CompareAndIncrementAndOrAdd(List<InuItem> sameList, Item itemToAdd)
    {
        //go through all same items and increment (if possible)
        for (int i = 0; i < sameList.Count; i++)
        {
            if (sameList[i].inuItem.item.duraCount > stdStack)
            {
                continue;
            }
            else
            {
                int canAdd = stdStack - (int)sameList[i].inuItem.item.duraCount;
                itemToAdd.duraCount -= canAdd;
                if (itemToAdd.duraCount > 0)
                {
                    sameList[i].inuItem.item.duraCount = canAdd;
                    continue;
                }
            }
        }
        //else create new
        if (itemToAdd.duraCount > 0)
        {
            FindEmptyAndAdd(itemToAdd);
        }
    }

    private void FindEmptyAndAdd(Item item)
    {
        for (int i = 0; i < inuBgs.Length; i++)
        {
            if (!inuBgs[i].isOccupied)
            {
                AddNewItem(i, item);
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
        int draggedBgId = draggedItem.inuItem.invSlotId;
        int currentBgId = inuItem.inuItem.invSlotId;
        int draggedItemId = (int)draggedItem.inuItem.item.id;
        int currentItemId = (int)inuItem.inuItem.item.id;
        int draggedDuraCount = (int)draggedItem.inuItem.item.duraCount;
        int currentDuraCount = (int)inuItem.inuItem.item.duraCount;

        if (draggedItemId == currentItemId)//merge
        {
            if (draggedDuraCount + currentDuraCount > stdStack)
            {
                int balanceAmt = (draggedDuraCount + currentDuraCount) - stdStack;
                inuItem.inuItem.item.duraCount = stdStack;
                draggedItem.inuItem.item.duraCount = balanceAmt;
                inuItem.UpdateTextValues();
                draggedItem.UpdateTextValues();
            }
            else
            {
                inuItem.inuItem.item.duraCount += draggedItem.inuItem.item.duraCount;
                inuItem.UpdateTextValues();
                inuItems.Remove(draggedItem);
                inuBgs[draggedBgId].isOccupied = false;
                Destroy(draggedItem.gameObject);
            }
        }
        else // swap
        {
            draggedItem.transform.SetParent(inuBgs[currentBgId].transform);
            draggedItem.inuItem.invSlotId = currentBgId;
            inuItem.transform.SetParent(inuBgs[draggedBgId].transform);
            inuItem.transform.localPosition = Vector3.zero;
            inuItem.inuItem.invSlotId = draggedBgId;
        }
    }

    private void OnBgDrop(int id)
    {
        inuBgs[draggedItem.inuItem.invSlotId].isOccupied = false;
        draggedItem.transform.SetParent(inuBgs[id].transform);
        draggedItem.inuItem.invSlotId = (byte)id;
    }

    #endregion

    #region Temp Functions
    private Item CreateRanItem()
    {
        Item item = new Item(UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(2, 20));
        print("id " + item.id + " val " + item.duraCount);
        return item;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            AddNewItem(CreateRanItem());
        }
    }
    #endregion
}

[System.Serializable]
public struct AllInuItems
{
    public List<SlotItems> itemInus;
}