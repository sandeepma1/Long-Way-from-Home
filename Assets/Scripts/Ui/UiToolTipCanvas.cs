using System;
using TMPro;
using UnityEngine;

public class UiToolTipCanvas : MonoBehaviour
{
    public static Action<bool, Vector2, int> OnShowToolTip;
    [SerializeField] private GameObject parentPanel;
    [SerializeField] private TextMeshProUGUI tooltipHeader;
    [SerializeField] private TextMeshProUGUI tooltipDescription;
    private bool isClickDown;
    private const float clickAndHoldDuration = 0.65f;
    private float counter = 0;
    private int lastClickedItemId = -1;
    private Vector2 lastClickedUiSlotItemPosition;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = parentPanel.GetComponent<RectTransform>();
        OnShowToolTip += ShowToolTip;
        ShowToolTip(false, Vector2.zero);
    }

    private void OnDestroy()
    {
        OnShowToolTip -= ShowToolTip;
    }

    private void Update()
    {
        if (isClickDown)
        {
            counter += Time.deltaTime;
            if (counter >= clickAndHoldDuration)
            {
                isClickDown = false;
                parentPanel.SetActive(true);
                if (lastClickedItemId >= 0)
                {
                    tooltipHeader.text = InventoryItemsDatabase.GetNameById(lastClickedItemId);
                    tooltipDescription.text = InventoryItemsDatabase.GetDescriptionById(lastClickedItemId);
                    rectTransform.anchoredPosition = lastClickedUiSlotItemPosition;
                }
            }
        }
    }

    private void ShowToolTip(bool showToolTip, Vector2 uiSlotPosition, int itemId = -1)
    {
        if (showToolTip)
        {
            isClickDown = true;
            lastClickedItemId = itemId;
            lastClickedUiSlotItemPosition = uiSlotPosition;
        }
        else
        {
            isClickDown = false;
            parentPanel.SetActive(false);
            counter = 0;
        }
    }
}