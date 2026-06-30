using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text waterText;

    [Header("Game UI")]
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public RectTransform finishArea;

    private bool gameOver = false;

    public bool IsGameOver => gameOver;

    public CanvasGroup waterPanel;

    public int startingWater = 0;

    public int currentWater;

    private AnimalCardManager animalCardManager;

    private void Awake()
{
    Instance = this;

    animalCardManager = GetComponent<AnimalCardManager>();
}

   private void Start()
{
    currentWater = startingWater;

    InitializeGrid();

    UpdateUI();

    RefreshCards();
}

    void UpdateUI()
    {
        waterText.text = currentWater.ToString();
    }

    void RefreshCards()
    {
        CardManager[] cards =
            FindObjectsByType<CardManager>(
                FindObjectsSortMode.None);

        foreach (CardManager card in cards)
        {
            card.UpdateCardVisual();
        }
    }

    void InitializeGrid()
{
    if (animalCardManager == null)
        return;

    for (int i = 0; i < animalCardManager.gridSlots.Length; i++)
    {
        SlotManager slot =
            animalCardManager.gridSlots[i]
            .GetComponent<SlotManager>();

        if (slot == null)
            continue;

        slot.row = i / 5;
        slot.lane = i % 5;
    }
}

    public void AddWater(int amount)
    {
        currentWater += amount;

        UpdateUI();
        RefreshCards();
    }

    public void SpendWater(int amount)
    {
        currentWater -= amount;

        UpdateUI();

        RefreshCards();
    }

    public bool HasWater(int amount)
    {
        return currentWater >= amount;
    }

    public void FlashWaterPanel()
    {
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        waterPanel.alpha = 0.3f;

        yield return new WaitForSeconds(0.15f);

        waterPanel.alpha = 1f;
    }

    public void GameOver()
{
    if (gameOver)
        return;

    gameOver = true;

    Time.timeScale = 0f;

    gameOverPanel.SetActive(true);
}

    public void Win()
{
    if (gameOver)
        return;

    gameOver = true;

    Time.timeScale = 0f;

    winPanel.SetActive(true);
}
}