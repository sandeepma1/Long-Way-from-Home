using UnityEngine;

public class SimpleIteo : MonoBehaviour
{

}

[System.Serializable]
public class Item
{
    public int id;
    public int duraCount;
    public Item(int id, int duraCount)
    {
        this.id = id;
        this.duraCount = duraCount;
    }
}


[System.Serializable]
public class InventoryIteo
{
    public Item item;
    public int invSlotId;
    public InventoryIteo(Item iteo, int invSlotId)
    {
        this.item = iteo;
        this.invSlotId = invSlotId;
    }

    public InventoryIteo(int iteoId, int iteoDuraCount, int inuSlotId)
    {
        item = new Item(iteoId, iteoDuraCount);
        this.invSlotId = inuSlotId;
    }
}