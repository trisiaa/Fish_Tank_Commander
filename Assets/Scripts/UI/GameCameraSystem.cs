using UnityEngine;
using System;

public class GameCameraSystem : MonoBehaviour
{
    [Header("Vertical Transform Landmarks")]
    public Transform humanPreviewPoint; 
    public Transform houseUIPoint;        

    [Header("Transition Settings")]
    public float humanPreviewDuration = 2.5f;
    public float panDownToHouseDuration = 1.4f; 

    [Header("Target UI Panel")]
    public RectTransform uiPanelToSlide; // Drag your 'AnimalSelectionPanel' RectTransform here
    public CanvasGroup selectionCanvasGroup; 

    public static Action OnIntroSequenceComplete;

    private Vector2 uiAnchoredCenterPosition;

    private void Start()
    {
        selectionCanvasGroup.alpha = 0f;
        selectionCanvasGroup.interactable = false;
        selectionCanvasGroup.blocksRaycasts = false;

        // Save the perfect center position where the UI is supposed to end up
        uiAnchoredCenterPosition = uiPanelToSlide.anchoredPosition;

        // Position the UI panel completely off-screen at the top before starting
        uiPanelToSlide.anchoredPosition = new Vector2(uiAnchoredCenterPosition.x, Screen.height + 500f);

        // Snap camera directly to the top peak
        transform.position = new Vector3(transform.position.x, humanPreviewPoint.position.y, transform.position.z);
        
        ExecuteVerticalIntro();
    }

    private void ExecuteVerticalIntro()
    {
        Vector3 targetPos = new Vector3(transform.position.x, houseUIPoint.position.y, transform.position.z);

        LeanTween.move(gameObject, targetPos, panDownToHouseDuration)
            .setDelay(humanPreviewDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(SlideInVerticalUI);
    }

    private void SlideInVerticalUI()
    {
        selectionCanvasGroup.interactable = true;
        selectionCanvasGroup.blocksRaycasts = true;

        // Fade in the transparency alpha cleanly
        LeanTween.alphaCanvas(selectionCanvasGroup, 1f, 0.3f);

        // Physically slide the UI panel from the top of the sky down to its center position 
        // with a gorgeous elastic bounce (easeOutBack)
        LeanTween.value(uiPanelToSlide.gameObject, uiPanelToSlide.anchoredPosition.y, uiAnchoredCenterPosition.y, 0.6f)
            .setEase(LeanTweenType.easeOutBack)
            .setOnUpdate((float val) => {
                uiPanelToSlide.anchoredPosition = new Vector2(uiAnchoredCenterPosition.x, val);
            })
            .setOnComplete(() => {
                OnIntroSequenceComplete?.Invoke();
            });
    }
}