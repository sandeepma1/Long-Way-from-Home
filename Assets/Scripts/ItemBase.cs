using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    protected ItemType itemType = ItemType.typeA;
    protected int itemId = 0;
    protected string itemName = "type1";
}

public enum ItemType
{
    typeA,
    typeB
}

public interface IItem
{
    int GetItemId();
    string GetItemName();
    ItemType GetItemType();
}