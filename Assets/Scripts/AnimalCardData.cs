using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Animal Card")]
public class AnimalCardData : ScriptableObject
{
    public string animalName;

    public Sprite icon;

    [Header("Card")]
    public int cost;

    [Header("Animal")]
    public int maxHP;

    public GameObject animalPrefab;
}