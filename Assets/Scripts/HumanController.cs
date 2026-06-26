using UnityEngine;

public class HumanController : MonoBehaviour
{
    public HumanData data;

    [Header("Lane")]
    public int currentLane;

    [HideInInspector]
    public int currentRow = 0;

    private RectTransform rect;

    private AnimalCardManager cardManager;

    private bool canMove = true;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        cardManager =
    GameManager.Instance.GetComponent<AnimalCardManager>();
    }

    private void Update()
{

    if (canMove)
        Move();
}

    void Move()
    {
        rect.anchoredPosition +=
            Vector2.down *
            data.moveSpeed *
            Time.deltaTime;

        UpdateCurrentRow();
        CheckAnimal();

        if (rect.anchoredPosition.y < -1800)
        {
            Destroy(gameObject);
        }
    }

    void UpdateCurrentRow()
{
    float top = 630f;

    float y = rect.anchoredPosition.y;

    currentRow = Mathf.FloorToInt((top - y) / 180f);

    currentRow = Mathf.Clamp(currentRow, 0, 6);
}

    void CheckAnimal()
{
    int index = currentRow * 5 + currentLane;

    if (index >= cardManager.gridSlots.Length)
        return;

    SlotManager slot =
        cardManager.gridSlots[index].GetComponent<SlotManager>();

    if (slot.occupied)
    {
        StopMoving();
    }
    else
    {
        ContinueMoving();
    }
}

    public void StopMoving()
    {
        canMove = false;
    }

    public void ContinueMoving()
    {
        canMove = true;
    }

    public void SetLane(int lane)
{
    currentLane = lane;
}

}