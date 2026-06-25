using UnityEngine;

public class HumanController : MonoBehaviour
{
    public HumanData data;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        rect.anchoredPosition += Vector2.down * data.moveSpeed * Time.deltaTime;

        // Hapus jika sudah keluar layar
        if (rect.anchoredPosition.y < -1800)
        {
            Destroy(gameObject);
        }
    }
}