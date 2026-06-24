using UnityEngine;

public class WaterSystem : MonoBehaviour
{
    public int value = 25;

    [Header("Fall Settings")]
    public float fallSpeed = 200f;

    [Header("PVZ Settings")]
    public float stopY = -500f;
    public float lifeTime = 5f;

    [Header("Source")]
    public bool fromRazorClam = false;

    RectTransform rect;

    bool hasStopped = false;
    float timer;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (!hasStopped)
{
    if(!fromRazorClam)
    {
        rect.anchoredPosition +=
            Vector2.down *
            fallSpeed *
            Time.deltaTime;

        if (rect.anchoredPosition.y <= stopY)
        {
            hasStopped = true;
        }
    }
    else
    {
        hasStopped = true;
    }
}
        else
        {
            timer += Time.deltaTime;

            if (timer >= lifeTime)
            {
                Destroy(gameObject);
            }
        }
    }

    public void CollectWater()
    {
        GameManager.Instance.AddWater(value);

        Destroy(gameObject);
    }
}