using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    [Header("Grid Position")]
    public int row;

    public int lane;

    public bool occupied = false;

    public bool unlocked = true;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        button.interactable = unlocked;
    }
}