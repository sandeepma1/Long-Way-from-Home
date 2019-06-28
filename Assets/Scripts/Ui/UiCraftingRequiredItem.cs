using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCraftingRequiredItem : MonoBehaviour
{
    [SerializeField] private Image itemIconImage;
    [SerializeField] private TextMeshProUGUI requiredText;

    public void Init(Item item)
    {
        itemIconImage.sprite = AtlasBank.GetMapItemSpriteById((int)item.id);
        requiredText.text = "x " + item.duraCount;
    }
}