using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalCardManager : MonoBehaviour
{
    public AnimalCardData[] levelCards;

    public Image[] icons;

    public TMP_Text[] costs;

    [Header("Grid")]
public Transform gridArea;

[HideInInspector]
public Transform[] gridSlots;

    private void Start()
{
    LoadGridSlots();

    SetupCards();
}

    void LoadGridSlots()
{
    if (gridArea == null)
    {
        Debug.LogError("Grid Area belum diisi di AnimalCardManager!");
        return;
    }

    gridSlots = new Transform[gridArea.childCount];

    for (int i = 0; i < gridArea.childCount; i++)
    {
        gridSlots[i] = gridArea.GetChild(i);
    }
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

    public void PlaceOnSlot(int slotIndex)
    {
        CardManager.instance.PlaceAnimal(gridSlots[slotIndex]);
    }
}