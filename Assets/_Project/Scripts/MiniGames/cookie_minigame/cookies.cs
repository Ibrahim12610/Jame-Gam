using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class cookies : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public RectTransform emptyJar;
    Vector2 originalPosition;
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    [Obsolete("Obsolete")]
    public void OnEndDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(emptyJar, eventData.position))
        {
            // Cookie successfully placed in jar - notify minigame
            cookie_minigame minigame = FindObjectOfType<cookie_minigame>();
            if (minigame != null)
            {
                minigame.CookiePlaced();
            }
            
            Destroy(gameObject);
        }
        else
        {
            rect.anchoredPosition = originalPosition;
        }
    }
}
