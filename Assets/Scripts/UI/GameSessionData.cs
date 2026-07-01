using UnityEngine;
using System.Collections.Generic;

public class GameSessionData : MonoBehaviour
{
    public static GameSessionData Instance;

    // This persistent list tracks your selections across scenes
    public List<AnimalData> ChosenAnimals = new List<AnimalData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps data alive when scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}