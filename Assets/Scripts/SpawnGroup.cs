using UnityEngine;

[System.Serializable]
public class SpawnGroup
{
    public HumanData human;

    [Header("Normal Spawn")]
    public int normalAmount;

    [Header("Final Wave")]
    public int waveAmount;
}