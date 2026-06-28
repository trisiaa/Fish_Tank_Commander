using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class HumanManager : MonoBehaviour
{
    [Header("Human List")]
    public HumanData[] humans;

    public List<HumanController> activeHumans =
    new List<HumanController>();

    [Header("Spawn Lane")]
    public RectTransform[] lanePoints;

    [Header("Lane Aktif")]
    public int[] activeLanes;

    [Header("Parent")]
    public RectTransform humanParent;

    [Header("Spawn Time")]
    public float minSpawnTime = 2f;
    public float maxSpawnTime = 5f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(
                Random.Range(minSpawnTime, maxSpawnTime));

            SpawnHuman();
        }
    }

    void SpawnHuman()
    {
        if (humans.Length == 0)
            return;

        if (activeLanes.Length == 0)
            return;

        HumanData randomHuman =
            humans[Random.Range(0, humans.Length)];

        int laneIndex =
            activeLanes[Random.Range(0, activeLanes.Length)];

        RectTransform lane =
            lanePoints[laneIndex];

        GameObject newHuman =
            Instantiate(randomHuman.prefab, humanParent);

        RectTransform rect =
            newHuman.GetComponent<RectTransform>();

        rect.anchoredPosition =
            lane.anchoredPosition;

        HumanController controller =
    newHuman.GetComponent<HumanController>();

controller.data = randomHuman;

controller.SetLane(laneIndex);

activeHumans.Add(controller);
    }
}