using System;
using UnityEngine;
using UnityEngine.UI;

public class ChristmasLightsPickUp : MonoBehaviour
{
    public GameObject minigameCanvas;  
    bool playerInside = false;  
    [SerializeField] private int amount;

    [SerializeField] private bool testMode;
    
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        
        if (!otherCollider.CompareTag("Player")) return;
        if (lights_game.onCooldown) return;
        if (playerInside) return;

        playerInside = true;
        if (testMode)
        {
            MiniGameManager.Instance.OnMiniGameComplete(1);
            Destroy(gameObject);
        }

        
        if (minigameCanvas != null)
        {
            lights_game minigame = minigameCanvas.GetComponent<lights_game>();
            if (minigame == null)
            {
                minigame = minigameCanvas.GetComponentInChildren<lights_game>(true);
            }
            
            if (minigame != null && minigame.gameObject != minigameCanvas)
            {
                minigame.gameObject.SetActive(false);
            }
            minigameCanvas.SetActive(false);
            
            minigameCanvas.SetActive(true);
            
            
            Canvas canvas = minigameCanvas.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.enabled = true;
            }
            
            if (minigame != null && minigame.gameObject != minigameCanvas)
            {
                minigame.gameObject.SetActive(true);
            }
            
            
            for (int i = 0; i < minigameCanvas.transform.childCount; i++)
            {
                minigameCanvas.transform.GetChild(i).gameObject.SetActive(true);
            }
            
            
            if (minigame != null)
            {
                minigame.SetPickup(this, amount);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if(!otherCollider.CompareTag("Player")) return;
        
        playerInside = false;
            
        
    }
}
