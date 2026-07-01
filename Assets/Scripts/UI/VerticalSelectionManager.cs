using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Required for loading scenes

public class VerticalSelectionManager : MonoBehaviour
{
    [Header("UI Structure Hooks")]
    public Transform bankPack;           // Holds your 6 vertical slot frames
    public Transform gridPack;           // Holds your 5 available card items
    public Button startButton;        
    public Transform overlayAnimationLayer; 

    [Header("Juice Prefabs & Tuning")]
    public GameObject juicyFlyerPrefab;
    public float cardFlySpeed = 0.4f;

    private List<AnimalCard> bankSlots = new List<AnimalCard>();
    private int totalAvailableCardsInGrid = 0;
    private bool inputLock = false;

    private void Start()
    {
        // 1. Gather all the pre-built slot objects inside our vertical bank container
        foreach (Transform child in bankPack)
        {
            AnimalCard slot = child.GetComponent<AnimalCard>();
            if (slot != null)
            {
                bankSlots.Add(slot);
                slot.ClearCard();
            }
        }

        // 2. Count exactly how many unique animal cards are in the library grid
        AnimalCard[] allCardsInGrid = gridPack.GetComponentsInChildren<AnimalCard>();
        totalAvailableCardsInGrid = allCardsInGrid.Length; // This will return exactly 5

        startButton.interactable = false;
        
        inputLock = true;
        GameCameraSystem.OnIntroSequenceComplete += UnlockInput;
    }

    private void OnDestroy()
    {
        GameCameraSystem.OnIntroSequenceComplete -= UnlockInput;
    }

    private void UnlockInput() => inputLock = false;

    public void TrySelectAnimal(AnimalCard gridCard)
    {
        if (inputLock || gridCard.animalData == null || gridCard.isSelected) return;

        // Search through the 6 available bank slots for the first empty spot
        AnimalCard openSlot = bankSlots.Find(slot => slot.animalData == null);

        if (openSlot != null)
        {
            inputLock = true;
            gridCard.isSelected = true; 
            gridCard.UpdateVisuals();

            GameObject flyer = Instantiate(juicyFlyerPrefab, overlayAnimationLayer);
            
            flyer.GetComponent<JuicyAnimalFlyer>().AnimateFlight(
                gridCard.transform.position,
                openSlot.transform.position,
                gridCard.animalData.activeSprite,
                cardFlySpeed,
                () => {
                    openSlot.SetAnimal(gridCard.animalData);
                    inputLock = false;
                    CheckBankStatus();
                }
            );
        }
    }

    public void DeselectAnimal(AnimalCard bankSlot)
    {
        if (inputLock || bankSlot.animalData == null) return;

        inputLock = true;
        AnimalData animalToReturn = bankSlot.animalData;

        AnimalCard targetGridCard = null;
        AnimalCard[] allGridCards = gridPack.GetComponentsInChildren<AnimalCard>();
        foreach (AnimalCard gc in allGridCards)
        {
            if (gc.animalData == animalToReturn)
            {
                targetGridCard = gc;
                break;
            }
        }

        Vector3 returnTarget = targetGridCard != null ? targetGridCard.transform.position : bankSlot.transform.position;

        GameObject flyer = Instantiate(juicyFlyerPrefab, overlayAnimationLayer);
        bankSlot.ClearCard();

        flyer.GetComponent<JuicyAnimalFlyer>().AnimateFlight(
            bankSlot.transform.position,
            returnTarget,
            animalToReturn.activeSprite,
            cardFlySpeed,
            () => {
                if (targetGridCard != null)
                {
                    targetGridCard.isSelected = false; 
                    targetGridCard.UpdateVisuals();
                }
                ShiftBankSlotsVertical();
                inputLock = false;
                CheckBankStatus();
            }
        );
    }

    private void ShiftBankSlotsVertical()
    {
        List<AnimalData> activeAnimals = new List<AnimalData>();
        foreach (var slot in bankSlots)
        {
            if (slot.animalData != null) activeAnimals.Add(slot.animalData);
        }

        foreach (var slot in bankSlots) slot.ClearCard();

        for (int i = 0; i < activeAnimals.Count; i++)
        {
            bankSlots[i].SetAnimal(activeAnimals[i]);
        }
    }

    private void CheckBankStatus()
    {
        // Count how many inventory slots currently have an animal loaded into them
        int activeSelectedCount = 0;
        foreach (var slot in bankSlots)
        {
            if (slot.animalData != null) activeSelectedCount++;
        }

        // Activates strictly when all available library cards (5) have been added
        bool allAvailableCardsSelected = (activeSelectedCount == totalAvailableCardsInGrid);
        
        if (allAvailableCardsSelected && !startButton.interactable)
        {
            startButton.interactable = true;
            startButton.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            
            // Juicy LeanTween overshoot button bounce pop!
            LeanTween.scale(startButton.gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack);
        }
        else if (!allAvailableCardsSelected)
        {
            startButton.interactable = false;
        }
    }

    public void StartGameAction()
    {
        // Guardrail 1: Block input if an animation is currently running
        if (inputLock) return;

        // 1. Calculate exactly how many cards the player has selected right now
        int activeSelectedCount = 0;
        foreach (var slot in bankSlots)
        {
            if (slot.animalData != null) activeSelectedCount++;
        }

        // Guardrail 2: Strictly verify if ALL 5 unique available cards have been chosen.
        if (activeSelectedCount < totalAvailableCardsInGrid)
        {
            Debug.LogWarning("Cannot start game! You must select all available animal cards first.");
            return; 
        }

        // 3. Store the selection data safely into the persistent singleton container
        if (GameSessionData.Instance != null)
        {
            GameSessionData.Instance.ChosenAnimals.Clear();
            foreach (var slot in bankSlots)
            {
                if (slot.animalData != null)
                {
                    GameSessionData.Instance.ChosenAnimals.Add(slot.animalData);
                }
            }
        }

        // 4. Load the scene "Lv1"
        SceneManager.LoadScene("Lv1");
    }
}