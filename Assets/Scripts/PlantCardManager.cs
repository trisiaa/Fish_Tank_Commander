using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantCardManager : MonoBehaviour
{
    public AnimalCardData[] levelCards;

    public Image[] icons;

    public TMP_Text[] costs;

    private void Start()
    {
        SetupCards();
    }

    void SetupCards()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].gameObject.SetActive(false);
            costs[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < levelCards.Length; i++)
        {
            icons[i].gameObject.SetActive(true);
            costs[i].gameObject.SetActive(true);

            icons[i].sprite = levelCards[i].icon;
            costs[i].text = levelCards[i].cost.ToString();
        }
    }
}