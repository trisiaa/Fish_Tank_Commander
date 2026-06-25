using System.Collections;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
    [Header("Human List")]
    public HumanData[] humans;

    [Header("Lane Spawn")]
    public RectTransform[] laneSpawnPoints;

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
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
            SpawnHuman();
        }
    }

    void SpawnHuman()
    {
        if (humans.Length == 0 || laneSpawnPoints.Length == 0)
            return;

        // Pilih human secara acak
        HumanData randomHuman = humans[Random.Range(0, humans.Length)];

        // Pilih lane secara acak
        RectTransform lane = laneSpawnPoints[Random.Range(0, laneSpawnPoints.Length)];

        // Spawn prefab
        GameObject human = Instantiate(randomHuman.prefab, humanParent);

        RectTransform rect = human.GetComponent<RectTransform>();

        // Posisi sama dengan lane
        rect.anchoredPosition = lane.anchoredPosition;
    }
}