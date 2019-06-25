using UnityEngine;

public class UiMainCanvas : MonoBehaviour
{
    public static Transform mainCanvas;

    private void Start()
    {
        mainCanvas = GetComponent<Transform>();
    }
}