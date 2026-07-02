using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Animal Card")]
public class AnimalCardData : ScriptableObject
{
    public string animalName;

    public Sprite icon;

    [Header("Card")]
    public int cost;
    public float cardCooldown;

    [Header("Animal")]
    public int maxHP;

    [Header("Ability")]

    public int damage;

    public float actionCooldown;

    public int attackRange;

    public GameObject projectilePrefab;

    public GameObject animalPrefab;
}