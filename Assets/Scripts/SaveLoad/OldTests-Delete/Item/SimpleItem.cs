using UnityEngine;

public class SimpleItem : MonoBehaviour
{

}

[System.Serializable]
public class Item
{
    public int? id;
    public int? duraCount;
    public Item(int? id, int? duraCount)
    {
        this.id = id;
        this.duraCount = duraCount;
    }
}


[System.Serializable]
public class SlotItems
{
    public Item item;
    public int invSlotId;
    public SlotItems(Item item, int invSlotId)
    {
        this.item = item;
        this.invSlotId = invSlotId;
    }

    public SlotItems(int itemId, int itemDuraCount, int inuSlotId)
    {
        item = new Item(itemId, itemDuraCount);
        this.invSlotId = inuSlotId;
    }
}