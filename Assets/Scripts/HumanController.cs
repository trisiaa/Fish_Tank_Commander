using System.Collections;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    public HumanData data;

    private Animator animator;

    [Header("Lane")]
    public int currentLane;

    [HideInInspector]
    public int currentRow = 0;

    private RectTransform rect;

    private AnimalCardManager cardManager;

    private bool canMove = true;

    private bool isDead = false;

    private int currentHP;

    private float currentMoveSpeed;

    private float originalMoveSpeed;

    private AnimalController targetAnimal;
    
    private float attackTimer;

    private void Awake()
{
    animator = GetComponent<Animator>();

    rect = GetComponent<RectTransform>();

    cardManager =
        GameManager.Instance.GetComponent<AnimalCardManager>();

    currentHP = data.maxHealth;

    originalMoveSpeed = data.moveSpeed;
    currentMoveSpeed = originalMoveSpeed;

    PlayWalkAnimation();
}

    private void Update()
{
    if (isDead)
        return;

    CheckAnimal();

    if (canMove)
    {
        Move();
    }
    else
    {
        Attack();
    }
}

    void Move()
    {
        rect.anchoredPosition +=
            Vector2.down *
            currentMoveSpeed *
            Time.deltaTime;

        UpdateCurrentRow();

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
        targetAnimal =
            slot.GetComponentInChildren<AnimalController>();

        StopMoving();
    }
    else
    {
        targetAnimal = null;

        ContinueMoving();
    }
}

    void Attack()
{
    if (targetAnimal == null)
    {
        attackTimer = 0;
        return;
    }

    attackTimer += Time.deltaTime;

    if (attackTimer >= data.attackInterval)
    {
        attackTimer = 0;

        targetAnimal.TakeDamage(data.damage);
    }
}

    public void TakeDamage(int damage)
{
    currentHP -= damage;

    Debug.Log(data.humanName + " HP : " + currentHP);

    if (currentHP <= 0)
    {
        Die();
    }
}

   public void Slow(float multiplier, float duration)
{
    CancelInvoke(nameof(RemoveSlow));

    currentMoveSpeed = originalMoveSpeed * multiplier;

    Invoke(nameof(RemoveSlow), duration);
}

void RemoveSlow()
{
    currentMoveSpeed = originalMoveSpeed;
}

    public void StopMoving()
    {
        canMove = false;

        PlayAttackAnimation();
    }

    public void ContinueMoving()
    {
        canMove = true;

        PlayWalkAnimation();
    }

    public void SetLane(int lane)
{
    currentLane = lane;
}

    void Die()
{
    isDead = true;
    canMove = false;

    PlayDieAnimation();

    HumanManager manager =
        FindFirstObjectByType<HumanManager>();

    if (manager != null)
    {
        manager.activeHumans.Remove(this);
    }

    StartCoroutine(DieRoutine());
}

IEnumerator DieRoutine()
{
    canMove = false;

    yield return new WaitForSeconds(2f);

    Destroy(gameObject);
}

    public void PlayWalkAnimation()
{
    if (animator == null)
        return;

    animator.SetBool("Walk", true);
    animator.SetBool("Attack", false);
}

public void PlayAttackAnimation()
{
    if (animator == null)
        return;

    animator.SetBool("Walk", false);
    animator.SetBool("Attack", true);
}

public void PlayDieAnimation()
{
    if (animator == null)
        return;

    animator.SetTrigger("Die");
}

}