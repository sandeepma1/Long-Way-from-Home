using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ImageBank : MonoBehaviour
{
    [SerializeField] private SpriteAtlas uiAtlas;
    [SerializeField] private SpriteAtlas mapItemsAtlas;
    [SerializeField] private SpriteAtlas miscAtlas;
    private Sprite missingSprite;

    private void Start()
    {
        missingSprite = GetSprite("Blockx100", AtlasType.Misc);
    }

    public Sprite GetSprite(int itemId, AtlasType type)
    {
        return GetSprite(ItemBank.GetItemSlugById(itemId), type);
    }

    public Sprite GetSprite(string name, AtlasType type)
    {
        Sprite sprite = null;
        switch (type)
        {
            case AtlasType.Ui:
                sprite = uiAtlas.GetSprite(name);
                break;
            case AtlasType.MapItems:
                sprite = mapItemsAtlas.GetSprite(name);
                break;
            case AtlasType.Misc:
                sprite = miscAtlas.GetSprite(name);
                break;
        }

        if (sprite == null)
        {
            Debug.Log("Sprite named " + name + " not found in Bank Type " + type);
            sprite = missingSprite;
        }
        return sprite;
    }
}

public enum AtlasType
{
    Ui,
    MapItems,
    Misc
}