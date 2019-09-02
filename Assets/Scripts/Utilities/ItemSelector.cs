using UnityEngine;

public class ItemSelector : MonoBehaviour
{
    [SerializeField] private SpriteRenderer itemSelectorSprite;

    private void Start()
    {
        GetObjectsAround.OnMoveClosestItemMarker += OnMoveClosestItemMarker;
        GetObjectsAround.OnToggleShowItemMarker += OnToggleShowItemMarker;
    }

    private void OnDestroy()
    {
        GetObjectsAround.OnMoveClosestItemMarker -= OnMoveClosestItemMarker;
        GetObjectsAround.OnToggleShowItemMarker -= OnToggleShowItemMarker;
    }

    private void OnToggleShowItemMarker(bool flag)
    {
        itemSelectorSprite.enabled = flag;
    }

    private void OnMoveClosestItemMarker(Vector2 nearestItemPosition)
    {
        transform.localPosition = nearestItemPosition;
    }
}