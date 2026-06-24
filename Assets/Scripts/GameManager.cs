using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text waterText;

    public CanvasGroup waterPanel;

    public int startingWater = 0;

    public int currentWater;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentWater = startingWater;

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
}