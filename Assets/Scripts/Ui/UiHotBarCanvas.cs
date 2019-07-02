using UnityEngine;

public class UiHotBarCanvas : MonoBehaviour
{
    public static RectTransform hotBarPanelHolder;

    private void Start()
    {
        hotBarPanelHolder = transform.GetChild(0).GetComponent<RectTransform>();
    }
}