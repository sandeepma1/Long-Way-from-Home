﻿using System.Collections;
using UnityEngine;

public class TreesDisappear : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Disappear":
                other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.45f);
                other.transform.parent.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.45f);
                break;
            case "Fire":
                PlayerMovement_old.Instance.isPlayerNearFire = true;
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Disappear":
                other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                other.transform.parent.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                break;
            case "Fire":
                PlayerMovement_old.Instance.isPlayerNearFire = false;
                break;
            default:
                break;
        }
    }
}
