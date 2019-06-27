using System;
using UnityEngine;
using UnityEngine.UI;

public class UiCraftingItem : MonoBehaviour
{
    public Action<CraftableItem, Transform> OnCraftableItemClicked;
    [SerializeField] private Button itemButton;
    [SerializeField] private Image imageIcon;
    private CraftableItem craftableItem;

    private void Start()
    {
        itemButton.onClick.AddListener(() => OnCraftableItemClicked?.Invoke(craftableItem, transform));
    }

    private void OnDestroy()
    {
        itemButton.onClick.RemoveListener(() => OnCraftableItemClicked?.Invoke(craftableItem, transform));
    }

    public void Init(CraftableItem craftableItem)
    {
        imageIcon.sprite = AtlasBank.GetInventoryItemSpriteBySlug(craftableItem.slug);
        this.craftableItem = craftableItem;
    }
}