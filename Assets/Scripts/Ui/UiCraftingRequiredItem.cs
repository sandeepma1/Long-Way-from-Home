using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCraftingRequiredItem : MonoBehaviour
{
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TextMeshProUGUI requiredText;

    public void Init(Requires requires)
    {
        itemIconImage.sprite = AtlasBank.GetMapItemSpriteById(requires.itemId ?? default); //use ?? for nullable types
        requiredText.text = "x " + requires.count;
    }
}