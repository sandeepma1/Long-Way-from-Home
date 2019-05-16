using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItemChopable : MapItemBase, IChopable
{
    public void Chop(int damage)
    {
        print("chop");
    }
}
