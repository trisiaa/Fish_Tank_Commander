using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Animal Card")]
public class AnimalCardData : ScriptableObject
{
    public string animalName;

    public Sprite icon;

    public int cost;
}