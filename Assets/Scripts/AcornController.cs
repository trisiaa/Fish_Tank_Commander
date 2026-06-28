using UnityEngine;

public class AcornController : MonoBehaviour
{
    public int damage;

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

    Destroy(gameObject);
}
}