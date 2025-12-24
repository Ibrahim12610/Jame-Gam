using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Widget_popup : MonoBehaviour
{
    private static Widget_popup currentlyOpenWidget;

    [Header("UI References")]
    public GameObject panel;
    [Tooltip("Reference to the UI Image component. The sprite will be set from the ScriptableObject.")]
    public Image image;
    public TMP_Text descriptionText;

    void Awake()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void Show(widgets data)
    {
        if (data == null) return;
        PlayerManager.Instance.DisablePlayerLogic("PopUp");
        
        // Close any previously open widget
        if (currentlyOpenWidget != null && currentlyOpenWidget != this)
        {
            currentlyOpenWidget.Hide();
        }
        
        // Set this as the currently open widget
        currentlyOpenWidget = this;
        
        // Always set the sprite from the ScriptableObject (this overrides any default sprite)
        if (image != null)
        {
            if (data.image != null)
            {
                image.sprite = data.image;
            }
            else
            {
                image.sprite = null; // Clear if no sprite in ScriptableObject
            }
        }
        
        if (descriptionText != null)
            descriptionText.text = data.description ?? string.Empty;
        
        if (panel != null)
            panel.SetActive(true);
    }

    public void Hide()
    {
        if (panel != null)
            panel.SetActive(false);

        // Clear the reference if this widget was the open one
        if (currentlyOpenWidget == this)
        {
            currentlyOpenWidget = null;
        }
        PlayerManager.Instance.EnablePlayerLogic("PopUp");
    }

    public bool IsVisible()
    {
        return panel != null && panel.activeSelf;
    }
}
