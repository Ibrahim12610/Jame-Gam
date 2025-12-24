using UnityEngine;

public class Widget_interact : MonoBehaviour
{
    public widgets widgetData;

    Widget_popup popupManager;
    bool playerInRange;
    [SerializeField] private CanvasGroup indicator;
    
    void Awake()
    {
        popupManager = GetComponentInChildren<Widget_popup>(true);
        
        // Initialize indicator to hidden
        if (indicator != null)
        {
            indicator.alpha = 0f;
        }
    }
    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (popupManager != null)
            {
                // Toggle: if popup is visible, hide it; otherwise show it
                if (popupManager.IsVisible())
                {
                    popupManager.Hide();
                }
                else if (widgetData != null)
                {
                    popupManager.Show(widgetData);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            
            // Show the E indicator
            if (indicator != null)
            {
                indicator.alpha = 1f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            
            // Hide the E indicator
            if (indicator != null)
            {
                indicator.alpha = 0f;
            }
            
            // Close the widget when player exits the trigger
            if (popupManager != null && popupManager.IsVisible())
            {
                popupManager.Hide();
            }
        }
    }
}
