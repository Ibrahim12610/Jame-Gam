using UnityEngine;

public class KeyIndicatorController : MonoBehaviour
{
    [SerializeField] private CanvasGroup indicator;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        indicator.alpha = 1f;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        indicator.alpha = 0f;
    }
}
