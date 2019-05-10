using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMapTest : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private Color32[] colors;

    private void Start()
    {
        Texture2D texture = new Texture2D(64, 64);
        for (int i = 0; i < 64; i++)
        {
            for (int j = 0; j < 64; j++)
            {
                int x = Random.Range(0, colors.Length);
                texture.SetPixel(i, j, colors[x]);
            }
        }
        texture.filterMode = FilterMode.Bilinear;
        texture.Apply();
        rawImage.texture = texture;
    }
}
