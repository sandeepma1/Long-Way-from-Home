using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemCollector : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GEM.MapItemDroppedTagName))
        {
            collision.gameObject.GetComponent<MapItemDropedItem>().TouchedByPlayer();
        }
    }
}