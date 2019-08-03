using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool printDebug;
    [SerializeField] private bool printDebugWarning;
    [SerializeField] private bool printDebugError;

    private void Awake()
    {
        GEM.ShowDebug = printDebug;
        GEM.ShowDebugWarning = printDebugWarning;
        GEM.ShowDebugError = printDebugError;
    }
}