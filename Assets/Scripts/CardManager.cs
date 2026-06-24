using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CardManager : MonoBehaviour, IPointerClickHandler
{
    public AnimalCardData animalCardData;
    public GameObject selectedBorder;

    public GameObject lockOverlay;

    public static AnimalCardData selectedCard;

    public static CardManager instance;
    private Image cardImage;
    public static CardManager currentSelectedCard;

    private void Awake()
{
    instance = this;

    cardImage = GetComponent<Image>();
}

    public void OnPointerClick(PointerEventData eventData)
{
    if(!GameManager.Instance.HasWater(
    animalCardData.cost))
{
    GameManager.Instance
        .FlashWaterPanel();

    return;
}

    if(currentSelectedCard != null)
    {
        currentSelectedCard.selectedBorder.SetActive(false);
    }

    currentSelectedCard = this;

    selectedCard = animalCardData;

    selectedBorder.SetActive(true);

    Debug.Log("Selected : " + animalCardData.animalName);
}

    public void PlaceAnimal(Transform slot)
{
    if (selectedCard == null)
        return;

    if (slot.childCount > 0)
        return;

    if (!GameManager.Instance.HasWater(selectedCard.cost))
    {
        Debug.Log("Water tidak cukup");
        return;
    }

    GameManager.Instance.SpendWater(selectedCard.cost);

    GameObject animal =
        Instantiate(selectedCard.animalPrefab, slot);

    RectTransform rect =
        animal.GetComponent<RectTransform>();

    rect.anchoredPosition = Vector2.zero;

    if(currentSelectedCard != null)
    {
        currentSelectedCard.selectedBorder.SetActive(false);
        currentSelectedCard = null;
    }

    selectedCard = null;
}

public void UpdateCardVisual()
{
    bool enoughWater =
        GameManager.Instance.HasWater(
            animalCardData.cost);

    lockOverlay.SetActive(!enoughWater);
}
}