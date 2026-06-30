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

    private bool finalWaveStarted = false;

    private void Awake()
    {
        Instance = this;
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

        waveSlider.value = currentNormalSpawn;
    }

    public void StartFinalWave()
    {
        finalWaveStarted = true;

        waveSlider.value = totalNormalSpawn;

        Debug.Log("FINAL WAVE!");
    }
}