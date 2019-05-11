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
                GameObject mapItem = InstantiateBlock(biomesPrefab[intMap[j * mapWidth + i]], this.transform);
                mapItem.transform.localPosition = new Vector3(i, j);
            }
        }
        //SetCompositeCollider(true);
        StartCoroutine(Generat());
    }

    private GameObject InstantiateBlock(GameObject prefab, Transform parent)
    {
        GameObject mapItem = Instantiate(prefab, this.transform);
        //adding random rotation Z to not appear texture pattern
        float randomRotation = Random.Range(0, 360);
        mapItem.transform.eulerAngles = new Vector3(mapItem.transform.eulerAngles.x, mapItem.transform.eulerAngles.y, randomRotation);
        return mapItem;
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
