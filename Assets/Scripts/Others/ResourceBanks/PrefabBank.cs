using UnityEngine;

public class PrefabBank : MonoBehaviour
{
    public static UiSlot uiSlotPrefab;
    public static UiSlotItem uiSlotItemPrefab;

    private void Awake()
    {
        LoadAllFromResource();
    }

    private void LoadAllFromResource()
    {
        uiSlotPrefab = Resources.Load<UiSlot>("Prefabs/UiPrefabs/UiSlot");
        uiSlotItemPrefab = Resources.Load<UiSlotItem>("Prefabs/UiPrefabs/UiSlotitem");
    }
}