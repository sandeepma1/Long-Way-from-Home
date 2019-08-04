using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudCanvas : MonoBehaviour
{
    [SerializeField] private bool showFps = true;
    public static Text fpsText;
    public static TextMeshProUGUI debugText;
    private static float frequency = 0.5f;
    private int lastFrameCount;
    private float lastTime;
    private float timeSpan;
    private int frameCount;

    private void Start()
    {
        fpsText = GetComponentInChildren<Text>();
        debugText = GetComponentInChildren<TextMeshProUGUI>();
        if (showFps)
        {
            StartCoroutine(FPS());
        }
    }

    private static void PrintHudText(string text)
    {
        debugText.text = text;
    }

    private IEnumerator FPS()
    {
        for (; ; )
        {
            // Capture frame-per-second
            lastFrameCount = Time.frameCount;
            lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            timeSpan = Time.realtimeSinceStartup - lastTime;
            frameCount = Time.frameCount - lastFrameCount;

            // Display it
            fpsText.text = Mathf.RoundToInt(frameCount / timeSpan).ToString("F0");
        }
    }
}
