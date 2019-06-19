using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    private float t;
    [SerializeField] private float length = 1.15f;
    [SerializeField] private float divide = -3;

    private void Update()
    {
        t = Time.time / divide;
        transform.localScale = new Vector3(Mathf.PingPong(t, length - 1) + 1, Mathf.PingPong(t, length - 1) + 1, 0);
    }
}
