using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemChopable : MapItemBase, IChopable
{
    public void Chop(int damage)
    {
        print("chop with damage " + damage);
        item.duraCount -= damage;
        if (item.duraCount <= 0)
        {
            print("item chopped");
            MapItemDone();
        }
    }
}
