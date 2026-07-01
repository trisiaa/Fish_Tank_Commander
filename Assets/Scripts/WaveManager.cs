using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Reference")]
    public HumanManager humanManager;

    [Header("UI")]
    public Slider waveSlider;

    [Header("Final Wave Icon")]
    public GameObject waveIcon;

    private int currentNormalSpawn = 0;
    private int totalNormalSpawn = 0;

    private float targetValue;
    public float sliderSpeed = 3f;

    private Vector3 originalWaveScale;

    private bool finalWaveStarted = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        originalWaveScale = waveIcon.transform.localScale;
    }

    private void Update()
    {
        waveSlider.value =
            Mathf.Lerp(
                waveSlider.value,
                targetValue,
                Time.deltaTime * sliderSpeed);
    }

    public void Initialize(int normalCount)
    {
        totalNormalSpawn = normalCount;

        currentNormalSpawn = 0;

        waveSlider.minValue = 0;
        waveSlider.maxValue = totalNormalSpawn;
        waveSlider.value = 0;
    }

    public void HumanSpawned()
{
    if (finalWaveStarted)
        return;

    currentNormalSpawn++;

    float progress =
        (float)currentNormalSpawn /
        totalNormalSpawn;

    targetValue =
        progress *
        (totalNormalSpawn * 0.95f);
}

    public IEnumerator StartFinalWave()
{
    finalWaveStarted = true;

    targetValue = totalNormalSpawn;

    while (Mathf.Abs(waveSlider.value - targetValue) > 0.05f)
    {
        yield return null;
    }

    yield return StartCoroutine(WaveAnimation());
}
    IEnumerator WaveAnimation()
{
    Vector3 big = originalWaveScale * 1.8f;

    for (int i = 0; i < 2; i++)
    {
        float t = 0;

        while (t < 0.25f)
        {
            t += Time.deltaTime;

            waveIcon.transform.localScale =
                Vector3.Lerp(
                    originalWaveScale,
                    big,
                    t / 0.25f);

            yield return null;
        }

        t = 0;

        while (t < 0.25f)
        {
            t += Time.deltaTime;

            waveIcon.transform.localScale =
                Vector3.Lerp(
                    big,
                    originalWaveScale,
                    t / 0.25f);

            yield return null;
        }
    }

    waveIcon.transform.localScale =
        originalWaveScale;
}
}