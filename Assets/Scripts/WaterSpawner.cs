using System.Collections;
using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    public GameObject waterPrefab;

    public RectTransform spawnArea;

    public float minTime = 3f;
    public float maxTime = 7f;

    [Header("Clam Settings")]
    public bool isRazorClam;
    public float clamInterval = 8f;

    [Header("Spawn Offset")]
    public Vector2 clamOffset = new Vector2(0, 80);

    private AnimalController animalController;

    private void Start()
{
    animalController = GetComponent<AnimalController>();

    if (isRazorClam)
    {
        StartCoroutine(SpawnFromClam());
    }
    else
    {
        StartCoroutine(SpawnWater());
    }
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

    IEnumerator SpawnFromClam()
{
    while (true)
    {
        yield return new WaitForSeconds(clamInterval);

        if (animalController != null)
        {
            animalController.PlayActionAnimation();
        }

        yield return new WaitForSeconds(2f);

        SpawnAtPosition(Vector3.zero);
    }
}
    public void SpawnAtPosition(Vector3 position)
{
    GameObject water = Instantiate(waterPrefab);

    WaterSystem waterSystem =
    water.GetComponent<WaterSystem>();

waterSystem.fromRazorClam = true;

    RectTransform rect =
        water.GetComponent<RectTransform>();

    rect.SetParent(
    transform,
    false);

rect.anchoredPosition = Vector2.zero;
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

        float startY =
    spawnArea.rect.height / 2
    - rect.rect.height / 2;

rect.anchoredPosition =
    new Vector2(
        randomX,
        startY);
    }
}