using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    [SerializeField] private CompositeCollider2D compositeCollider2D;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private GameObject[] biomesPrefab;
    public MapGenerator mapGenerator;
    private int mapWidth;
    private int mapHeight;

    private void Start()
    {
        // SetCompositeCollider(false);
        MapData mapData = mapGenerator.CreateMaps();
        byte[] intMap = mapData.intMap;
        mapWidth = mapData.heightMap.GetLength(0);
        mapHeight = mapData.heightMap.GetLength(1);
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                GameObject mapItem = Instantiate(biomesPrefab[intMap[j * mapWidth + i]], this.transform);
                mapItem.transform.localPosition = new Vector3(i, j);
            }
        }
        //SetCompositeCollider(true);
        StartCoroutine(Generat());
    }

    private IEnumerator Generat()
    {
        yield return new WaitForSeconds(3);
        compositeCollider2D.GenerateGeometry();
    }
    private void SetCompositeCollider(bool flag)
    {
        compositeCollider2D.enabled = flag;
    }
}
