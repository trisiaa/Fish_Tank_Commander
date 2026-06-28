using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public int damage;

    public bool isPoison;

    public float slowMultiplier = 0.5f;

    public float slowDuration = 10f;

    public float speed = 600f;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rect.anchoredPosition +=
            Vector2.up *
            speed *
            Time.deltaTime;

        if (rect.anchoredPosition.y > 1400)
{
    Destroy(gameObject);
}
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    HumanController human =
        other.GetComponent<HumanController>();

    if (human == null)
        return;

    human.TakeDamage(damage);

if (isPoison)
{
    human.Slow(
        slowMultiplier,
        slowDuration);
}

Destroy(gameObject);
}
}