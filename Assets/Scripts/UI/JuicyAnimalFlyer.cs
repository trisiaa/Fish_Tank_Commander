using UnityEngine;
using UnityEngine.UI;
using System;

public class JuicyAnimalFlyer : MonoBehaviour
{
    private Image cardImage;
    private RectTransform rectTransform;

    private void Awake()
    {
        cardImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void AnimateFlight(Vector3 start, Vector3 end, Sprite sprite, float duration, Action onComplete)
    {
        // 1. Assign the sprite dynamically so it matches the card you clicked
        cardImage.sprite = sprite;
        cardImage.color = Color.white; // Ensure it's fully visible
        
        // 2. Set the starting point
        rectTransform.position = start;
        rectTransform.localScale = Vector3.one;

        // 3. Generate a curved path arcing out slightly to the right/side
        Vector3 midPoint = Vector3.Lerp(start, end, 0.5f) + new Vector3(180f, 20f, 0f);
        Vector3[] bezierPath = new Vector3[] { start, midPoint, end, end };

        // 4. Move the flyer along the path
        LeanTween.move(gameObject, bezierPath, duration)
            .setEase(LeanTweenType.easeOutQuad);

        // 5. Rubbery wind stretching (Mid-air) followed by landing impact squash (On Arrival)
        LeanTween.scaleY(gameObject, 1.25f, duration * 0.4f).setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() => {
                LeanTween.scaleY(gameObject, 0.8f, duration * 0.4f).setEase(LeanTweenType.easeInQuad)
                    .setOnComplete(() => LeanTween.scale(gameObject, Vector3.one, 0.12f).setEase(LeanTweenType.easeOutQuad));
            });

        LeanTween.scaleX(gameObject, 0.82f, duration * 0.4f).setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() => {
                LeanTween.scaleX(gameObject, 1.22f, duration * 0.4f).setEase(LeanTweenType.easeInQuad)
                    .setOnComplete(() => LeanTween.scale(gameObject, Vector3.one, 0.12f).setEase(LeanTweenType.easeOutQuad));
            });

        // 6. Complete execution trigger synchronization hook
        LeanTween.delayedCall(duration, () => {
            onComplete?.Invoke();
            Destroy(gameObject); // Safely clean up the flyer object
        });
    }
}