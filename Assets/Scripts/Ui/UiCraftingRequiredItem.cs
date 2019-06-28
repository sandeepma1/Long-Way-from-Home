using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCraftingRequiredItem : MonoBehaviour
{
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TextMeshProUGUI requiredText;

    public void Init(Item item)
    {
        itemIconImage.sprite = AtlasBank.GetMapItemSpriteById(item.id.Value);
        requiredText.text = "x " + item.duraCount;
    }
}