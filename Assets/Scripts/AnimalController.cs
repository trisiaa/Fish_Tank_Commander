using System.Collections;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public AnimalCardData data;

    private Animator animator;

    private float actionTimer;

    private HumanController targetHuman;

    private HumanManager humanManager;

    private RectTransform projectileParent;

    private bool hasExploded = false;

    private float explodeTimer;

    private int currentHP;

    private SlotManager slot;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        currentHP = data.maxHP;

        slot = GetComponentInParent<SlotManager>();

        humanManager =
            FindFirstObjectByType<HumanManager>();

        projectileParent =
            GameObject.Find("ProjectileParent")
            .GetComponent<RectTransform>();
    }

    private void Update()
{
    if (data.animalName == "Razor Clam")
    {
        return;
    }

    if (data.animalName == "Porcupine")
    {
        PorcupineAttack();
        return;
    }

    if (data.animalName == "Tortoise")
    {
        return;
    }

    FindTarget();

    Attack();
}

    void FindTarget()
{
    targetHuman = null;

    foreach (HumanController human in humanManager.activeHumans)
    {
        if (human.currentLane == slot.lane)
{
    int distance =
        Mathf.Abs(human.currentRow - slot.row);

    if (distance <= data.attackRange)
    {
        targetHuman = human;
        return;
    }
}
    }
}

    void Attack()
{
    if (targetHuman == null)
        return;

    actionTimer += Time.deltaTime;

    if (actionTimer >= data.actionCooldown)
    {
        actionTimer = 0;

        PlayActionAnimation();

        SpawnProjectile();

    }
}

    void PorcupineAttack()
{
    if (hasExploded)
        return;

    explodeTimer += Time.deltaTime;

    if (explodeTimer >= data.actionCooldown)
    {
        hasExploded = true;

         StartCoroutine(PorcupineExplodeRoutine());
    }
}

    IEnumerator PorcupineExplodeRoutine()
{
    animator.enabled = true;

    yield return new WaitForSeconds(2.1f);

    Explode();
}

    void Explode()
{
    Debug.Log("PORCUPINE BOOM");

    for (int i = humanManager.activeHumans.Count - 1; i >= 0; i--)
    {
        HumanController human =
            humanManager.activeHumans[i];

        RectTransform humanRect =
            human.GetComponent<RectTransform>();

        RectTransform slotRect =
            slot.GetComponent<RectTransform>();

        float yDistance =
            Mathf.Abs(
                humanRect.position.y -
                slotRect.position.y);

        int laneDistance =
            Mathf.Abs(
                human.currentLane -
                slot.lane);

        if (yDistance <= 350f &&
            laneDistance <= data.attackRange)
        {
            human.TakeDamage(data.damage);
        }
    }

    if (slot != null)
    {
        slot.occupied = false;
    }

    gameObject.SetActive(false);

    Destroy(gameObject);
}

    void SpawnProjectile()
{
    if (data.projectilePrefab == null)
        return;

    GameObject projectile =
        Instantiate(data.projectilePrefab, projectileParent);

    ProjectileController projectileController =
        projectile.GetComponent<ProjectileController>();

    projectileController.damage = data.damage;

    if (data.animalName == "Poison Dart Frog")
{
    projectileController.isPoison = true;
}

    RectTransform projectileRect =
        projectile.GetComponent<RectTransform>();

    RectTransform animalRect =
        GetComponent<RectTransform>();

    projectileRect.position =
        animalRect.position;
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

    public void PlayActionAnimation()
{
    Debug.Log("PLAY ACTION DIPANGGIL");

    if (animator == null)
    {
        Debug.Log("Animator NULL");
        return;
    }

    animator.SetTrigger("Action");
}
}