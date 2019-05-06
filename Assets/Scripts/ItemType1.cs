using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType1 : ItemBase, IItem
{
    public int GetItemId() => itemId;
    public string GetItemName() => itemName;
    public ItemType GetItemType() => itemType;
}