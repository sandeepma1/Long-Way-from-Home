using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemMineable : MapItemBase, IMineable
{
    public void Mine(int damage)
    {
        print("Mine with damage " + damage);
        item.duraCount -= damage;
        if (item.duraCount <= 0)
        {
            print("item mined");
            MapItemDone();
        }
    }
}
