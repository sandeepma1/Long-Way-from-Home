using UnityEngine;

namespace Bronz.Utilities
{
    [CreateAssetMenu(fileName = "ColorConstants", menuName = "Bronz/Singletons/ColorConstants")]
    public class ColorConstants : ScriptableSingleton<ColorConstants>
    {

#if UNITY_EDITOR
        [UnityEditor.MenuItem("LWFH/ColorConstants")]
        public static void ShowInEditor()
        {
            UnityEditor.Selection.activeObject = Instance;
        }
#endif
        #region World space
        // basic Colors
        [SerializeField] private Color white = new Color(1, 1, 1);
        public static Color White { get { return Instance.white; } }
        #endregion

        #region Map items
        [SerializeField] private Color fieldGlow = new Color(0.75f, 1, 0);
        public static Color FieldGlow { get { return Instance.fieldGlow; } }

        [SerializeField] private Color fieldNormal = new Color(1, 1, 1);
        public static Color FieldNormal { get { return Instance.fieldNormal; } }

        [SerializeField] private Color mapItemPlaceable = new Color(1, 1, 1);
        public static Color MapItemPlaceable { get { return Instance.mapItemPlaceable; } }

        [SerializeField] private Color mapItemNotPlaceable = new Color(1, 1, 1);
        public static Color MapItemNotPlaceable { get { return Instance.mapItemNotPlaceable; } }
        #endregion

        #region Menu UI        
        [SerializeField] private Color dehighlightedUiItem = new Color(0.25f, 0.25f, 0.25f);
        public static Color DehighlightedUiItem { get { return Instance.dehighlightedUiItem; } }

        [SerializeField] private Color normalUiItem = new Color(1, 1, 1);
        public static Color NormalUiItem { get { return Instance.normalUiItem; } }

        [SerializeField] private Color closeButtonBackground = new Color(0, 0, 0, 0.5f);
        public static Color CloseButtonBackground { get { return Instance.closeButtonBackground; } }

        [SerializeField] private Color fpsColor = new Color(0, 0, 1, 1f);
        public static Color FpsColor { get { return Instance.fpsColor; } }

        [SerializeField] private Color uiSlotSelectorInventory = new Color(1, 0, 0, 1f);
        public static Color UiSlotSelectorInventory { get { return Instance.uiSlotSelectorInventory; } }

        [SerializeField] private Color uiSlotSelectorCrafting = new Color(0, 1, 0, 1f);
        public static Color UiSlotSelectorCrafting { get { return Instance.uiSlotSelectorCrafting; } }
        #endregion
    }
}