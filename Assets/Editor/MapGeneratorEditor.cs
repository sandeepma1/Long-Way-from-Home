using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShowMapEditor))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ShowMapEditor mapGen = (ShowMapEditor)target;

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }
}