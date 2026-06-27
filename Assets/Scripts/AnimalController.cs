using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public AnimalCardData data;

    private int currentHP;

    private SlotManager slot;

    private void Start()
    {
        currentHP = data.maxHP;

        slot = GetComponentInParent<SlotManager>();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log(data.animalName + " HP : " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (slot != null)
        {
            slot.occupied = false;
        }

        Destroy(gameObject);
    }
}