using UnityEngine;

[CreateAssetMenu(menuName = "Humans/Human Data")]
public class HumanData : ScriptableObject
{
    [Header("Info")]
    public string humanName;

    [Header("Visual")]
    public Sprite sprite;

    public GameObject prefab;

    [Header("Movement")]
    public float moveSpeed = 150f;

    [Header("Health")]
    public int maxHealth = 100;

    [Header("Attack")]
    public int damage = 20;

    public float attackInterval = 1f;
}