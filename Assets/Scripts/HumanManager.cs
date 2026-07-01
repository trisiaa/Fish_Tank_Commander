using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class HumanManager : MonoBehaviour
{
    [Header("Spawn Group")]
    public SpawnGroup[] spawnGroups;

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

    [Header("Wave")]
    public float waveInterval = 0.4f;

    private int spawnedHuman = 0;

    private int totalHuman = 0;

    private int totalNormalHuman = 0;

    private bool spawnFinished = false;

    private bool finalWaveStarted = false;

    private List<HumanData> normalQueue =
    new List<HumanData>();

    private List<HumanData> waveQueue =
    new List<HumanData>();

    private void Start()
    {
        CalculateHuman();

        StartCoroutine(SpawnRoutine());
    }

    void CalculateHuman()
{
    totalHuman = 0;
    totalNormalHuman = 0;

    normalQueue.Clear();
    waveQueue.Clear();

    foreach (SpawnGroup group in spawnGroups)
    {
        for (int i = 0; i < group.normalAmount; i++)
        {
            normalQueue.Add(group.human);

            totalNormalHuman++;
            totalHuman++;
        }

        for (int i = 0; i < group.waveAmount; i++)
        {
            waveQueue.Add(group.human);

            totalHuman++;
        }
    }

    if (WaveManager.Instance != null)
{
    WaveManager.Instance.Initialize(totalNormalHuman);
}

}

    private void Update()
{
    CheckWin();
}

    IEnumerator SpawnRoutine()
{
    while (normalQueue.Count > 0)
    {
        yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

        if (GameManager.Instance.IsGameOver)
            yield break;

        int index =
            Random.Range(0, normalQueue.Count);

        HumanData human =
            normalQueue[index];

        normalQueue.RemoveAt(index);

        SpawnHuman(human);

spawnedHuman++;

if (WaveManager.Instance != null)
{
    WaveManager.Instance.HumanSpawned();
}
    }

    // Arena tenang selama 30 detik
yield return new WaitForSeconds(30f);

finalWaveStarted = true;

if (WaveManager.Instance != null)
{
    yield return StartCoroutine(
    WaveManager.Instance.StartFinalWave());
}

    while (waveQueue.Count > 0)
    {
        yield return new WaitForSeconds(waveInterval);

        if (GameManager.Instance.IsGameOver)
            yield break;

        HumanData human =
            waveQueue[0];

        waveQueue.RemoveAt(0);

        SpawnHuman(human);

        spawnedHuman++;
    }

    spawnFinished = true;
}

    void SpawnHuman(HumanData human)
{
    if (activeLanes.Length == 0)
        return;

    int laneIndex =
        activeLanes[Random.Range(0, activeLanes.Length)];

    RectTransform lane =
        lanePoints[laneIndex];

    GameObject newHuman =
        Instantiate(human.prefab, humanParent);

    RectTransform rect =
        newHuman.GetComponent<RectTransform>();

    rect.anchoredPosition =
        lane.anchoredPosition;

    HumanController controller =
        newHuman.GetComponent<HumanController>();

    controller.data = human;

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