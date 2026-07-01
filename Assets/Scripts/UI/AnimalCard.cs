using UnityEngine;
using UnityEngine.UI;

public class AnimalCard : MonoBehaviour
{
    public AnimalData animalData;
    public Image cardImage;
    
    // Strict true/false flag—can only be picked once
    public bool isSelected = false; 

    private void Start()
    {
        cardImage = GetComponent<Image>();
        UpdateVisuals();
    }

    public void SetAnimal(AnimalData data)
    {
        animalData = data;
        UpdateVisuals();
    }

    public void ClearCard()
    {
        animalData = null;
        isSelected = false;
        if (cardImage != null)
        {
            cardImage.sprite = null; 
            cardImage.color = new Color(1, 1, 1, 0.2f); // Semi-transparent frame
        }
    }

    public void UpdateVisuals()
    {
        if (animalData == null) return;

        cardImage.color = Color.white;
        
        // If selected, it turns grayscale and un-clickable
        if (isSelected)
        {
            cardImage.sprite = animalData.grayscaleSprite; 
        }
        else
        {
            cardImage.sprite = animalData.activeSprite;
        }
    }
}