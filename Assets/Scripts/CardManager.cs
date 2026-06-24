using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CardManager : MonoBehaviour, IPointerClickHandler
{
    public AnimalCardData animalCardData;
    public GameObject selectedBorder;

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
}