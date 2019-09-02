using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemMineable : MapItemBase, IMineable
{
    public void Mine(int damage)
    {
        GEM.PrintDebug("Mine with damage " + damage);
        mapItem.healthPoints -= damage;
        if (mapItem.healthPoints <= 0)
        {
            GEM.PrintDebug("item mined");
            MapItemHarvestingDone();
        }
    }
}
