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

    [Header("Level")]
    public int totalHuman = 25;
    private int spawnedHuman = 0;
    private bool spawnFinished = false;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private void Update()
{
    CheckWin();
}

    IEnumerator SpawnRoutine()
    {
        while (spawnedHuman < totalHuman)
{
    yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

    if (GameManager.Instance.IsGameOver)
        yield break;

    SpawnHuman();

    spawnedHuman++;
}

spawnFinished = true;
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

    private bool winTriggered = false;

void CheckWin()
{
    if (GameManager.Instance.IsGameOver)
        return;

    if (winTriggered)
        return;

    if (!spawnFinished)
        return;

    if (activeHumans.Count > 0)
        return;

    winTriggered = true;

    StartCoroutine(WinRoutine());
}

    IEnumerator WinRoutine()
{
    yield return new WaitForSeconds(2f);

    GameManager.Instance.Win();
}
}