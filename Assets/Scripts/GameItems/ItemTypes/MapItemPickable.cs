using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemPickable : MapItemBase, IPickable
{
    public void Pick()
    {
        MapItemDone();
    }
}
