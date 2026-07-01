using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimal", menuName = "Game/Animal Data")]
public class AnimalData : ScriptableObject
{
    public string animalName;
    public int deploymentCost;
    public Sprite activeSprite;
    public Sprite grayscaleSprite;
}