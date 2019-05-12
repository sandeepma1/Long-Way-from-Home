using UnityEngine;

public class GrassRandomness : MonoBehaviour
{
    private void Start()
    {
        foreach (Transform child in transform)
        {
            child.localPosition = Random.insideUnitCircle;
            child.GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.localPosition.y * -10);
        }
    }
}
