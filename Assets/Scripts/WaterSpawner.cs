using System.Collections;
using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    public GameObject waterPrefab;

    public RectTransform spawnArea;

    public float minTime = 3f;
    public float maxTime = 7f;

    private void Start()
    {
        StartCoroutine(SpawnWater());
    }

    IEnumerator SpawnWater()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                Random.Range(minTime, maxTime));

            Spawn();
        }
    }

    public void SpawnAtPosition(Vector3 position)
{
    GameObject water =
        Instantiate(
            waterPrefab,
            spawnArea);

    RectTransform rect =
        water.GetComponent<RectTransform>();

    rect.position = position;
}

    void Spawn()
    {
        GameObject water =
            Instantiate(waterPrefab,
                        spawnArea);

        RectTransform rect =
            water.GetComponent<RectTransform>();

        float randomX =
            Random.Range(
                -spawnArea.rect.width / 2,
                 spawnArea.rect.width / 2);

        rect.anchoredPosition =
            new Vector2(
                randomX,
                spawnArea.rect.height / 2);
    }
}